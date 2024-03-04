using CPT_275_Capstone.App_Start;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using ListItem = System.Web.UI.WebControls.ListItem;
using System.Net.Mail;

namespace CPT_275_Capstone.AdminDashboard
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                startDatePicker.Attributes.Add("autocomplete", "off");
                endDatePicker.Attributes.Add("autocomplete", "off");
                PopulateCarDropDownList();
                GetUsersList();
                string script = @"
                <script type='text/javascript'>
                    if (window.performance && window.performance.navigation.type === 2) {
                        window.location.reload(true);
                    }
                </script>
            ";
                ClientScript.RegisterStartupScript(this.GetType(), "RefreshScript", script);
            }

        }
        protected void PopulateCarDropDownList()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["FleetProd"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //string query = "Select CONCAT(vehicle_year, ' ', vehicle_make, ' ', vehicle_model) as VehicleInfo from vehicle";
                    string query = "SELECT vehicle_id, CONCAT(vehicle_year, ' ', vehicle_make, ' ', vehicle_model,' - ', vehicle_plate) AS CarName FROM vehicle";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            string carName = reader["CarName"].ToString();
                            string carID = reader["vehicle_id"].ToString();

                            ListItem item = new ListItem();
                            item.Text = carName;
                            item.Value = carID;
                            drpvehicles.Items.Add(item);
                        }
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                string eventSource = "SCCFleetServices";
                string errorMessage = "Error populating the list of vehicles on the trip screen on the admin side: " + ex.Message;
                EventViewerLogger logger = new EventViewerLogger(eventSource);
                logger.LogError(errorMessage);
                lblerrormessage.Text = "Error retrieving vehicle information" + ex.Message;
            }
        }
        protected void GetUsersList()
        {
            // Fetch the users information from the database
            string connectionString = ConfigurationManager.ConnectionStrings["FleetProd"].ConnectionString;
            string query = "SELECT user_id, CONCAT(users_first_name, ' ', users_last_name, ' - ', users_DL) as UserInfo FROM users";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    // Retrieve the users information and store it in the list
                    while (reader.Read())
                    {
                        string userinfo = reader["UserInfo"].ToString();
                        string userid = reader["user_id"].ToString();
                        ListItem item = new ListItem();
                        item.Text = userinfo;
                        item.Value = userid;
                        drpdriver.Items.Add(item);
                    }
                    reader.Close();
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                //// Get the selected values
                DateTime startDate = startDatePicker.SelectedDate;
                DateTime endDate = endDatePicker.SelectedDate;
                string selectedVehicle = drpvehicles.SelectedValue;
                string selectedUser = drpdriver.SelectedValue;

                bool selectVehicle = chkvehicle.Checked;
                bool selectUser = chkDriver.Checked;

                //// Query the database or perform any necessary data retrieval logic

                if (startDate== DateTime.MinValue || endDate == DateTime.MinValue)
                {
                    lblerrormessage.Text = "Please select both start date and end date";
                    return;
                }
                else
                {
                    lblerrormessage.Text = string.Empty;
                }

                DataTable data = null;

                if (selectVehicle && !selectUser)
                {
                    selectedUser = null;
                    data = GetDataFromDatabase(startDate, endDate, selectedVehicle, selectedUser);
                    //reportgried.DataSource = data;
                }
                else if (!selectVehicle && selectUser)
                {
                    selectedVehicle = null;
                    data = GetDataFromDatabase(startDate, endDate, selectedVehicle, selectedUser);
                }
                else if (selectUser && selectVehicle)
                {
                    
                    // Both checkboxes are checked, so ignore both selectedVehicle and selectedUser
                    selectedVehicle = null;
                    selectedUser = null;
                    data = GetDataFromDatabase(startDate, endDate, selectedVehicle, selectedUser);
                }
                else
                {
                    data = GetDataFromDatabase(startDate, endDate, selectedVehicle, selectedUser);
                }

                // Bind the data to the GridView
                reportgried.DataSource = data;
                reportgried.DataBind();

                btnexportaspdf.Visible = true;
                btnexportasexcel.Visible = true;
                //btnemail.Visible = true;
            }
            catch (Exception ex)
            {
                string eventSource = "SCCFleetServices";
                string errorMessage = "Error populating the trip report on the admin side: " + ex.Message;
                EventViewerLogger logger = new EventViewerLogger(eventSource);
                logger.LogError(errorMessage);
                lblerrormessage.Text = "Error retrieving trip information: " + ex.Message;
            }
        }
        private DataTable GetDataFromDatabase(DateTime startDate,DateTime endDate, string selectedVehicle, string selectedUser)
        {
           
                // Replace the connection string with your own
                string connectionString = ConfigurationManager.ConnectionStrings["FleetProd"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("trip_report_new", connection))
                    {
                        // Set the command type to stored procedure
                        command.CommandType = CommandType.StoredProcedure;

                        // Add the stored procedure parameters
                        command.Parameters.AddWithValue("@trip_beginning_date", startDate);
                        command.Parameters.AddWithValue("@trip_ending_date", endDate);

                    if (selectedVehicle !=null)
                    {
                        command.Parameters.AddWithValue("@selectd_vehicle", selectedVehicle);
                    }
                    if (selectedUser != null)
                    {
                        command.Parameters.AddWithValue("@select_user", selectedUser);
                    }
                        // Execute the stored procedure and retrieve the data
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable data = new DataTable();
                        adapter.Fill(data);

                        return data;
                    }
                }
        }

        protected void btnexportaspdf_Click(object sender, EventArgs e)
        {
            try
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);

                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                pnlGriedView.RenderControl(hw);

                StringReader sr = new StringReader(sw.ToString());
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                PdfWriter.GetInstance(pdfDoc, Response.OutputStream);

                pdfDoc.Open();
                htmlparser.Parse(sr);
                pdfDoc.Close();

                Response.Write(pdfDoc);
                Response.End();
            }
            catch (Exception ex)
            {
                string eventSource = "SCCFleetServices";
                string errorMessage = "Error exporting the gridview to pdf: " + ex.Message;
                EventViewerLogger logger = new EventViewerLogger(eventSource);
                logger.LogError(errorMessage);
                lblerrormessage.Text = "Error exporting the gridview to pdf" + ex.Message;
            }
        }
        protected void btnexportasexcel_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";

                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                pnlGriedView.RenderControl(hw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                string eventSource = "SCCFleetServices";
                string errorMessage = "Error exporting the gridview to excel: " + ex.Message;
                EventViewerLogger logger = new EventViewerLogger(eventSource);
                logger.LogError(errorMessage);
                lblerrormessage.Text = "Error exporting the gridview to excel" + ex.Message;
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            // This method is required to avoid the 'Control 'GridView' of type 'GridView' must be placed inside a form tag with runat=server.' error.
            // This method is required for exporting controls like GridView to Excel or PDF.

        }
        private string GenerateGridViewFile()
        {
            // Code to generate the GridView and export it to a file
            return "path/to/generated/file";
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }
    }
}