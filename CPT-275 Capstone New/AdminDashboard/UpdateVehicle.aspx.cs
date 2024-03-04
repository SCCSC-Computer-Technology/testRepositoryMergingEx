using CPT_275_Capstone.App_Start;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace CPT_275_Capstone.AdminDashboard
{
    public partial class UpdateVehicle : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string script = @"
                <script type='text/javascript'>
                    if (window.performance && window.performance.navigation.type === 2) {
                        window.location.reload(true);
                    }
                </script>
            ";
                ClientScript.RegisterStartupScript(this.GetType(), "RefreshScript", script); 

                if (Request.QueryString["yearMakeModel"] != null && Request.QueryString["plate"] != null)
                {
                    string yearMakeModel = Request.QueryString["yearMakeModel"];
                    string plate = Request.QueryString["plate"];

                    try
                    {
                        // Retrieve the vehicle information by year, make, model, and plate from the database
                        string connectionString = ConfigurationManager.ConnectionStrings["FleetProd"].ConnectionString;
                        string query = "SELECT vehicle_id, vehicle_year, vehicle_make, vehicle_model, vehicle_number, vehicle_mileage, vehicle_plate FROM vehicle WHERE CONCAT(vehicle_year, ' ', vehicle_make, ' ', vehicle_model) = @YearMakeModel AND vehicle_plate = @Plate";

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@YearMakeModel", yearMakeModel);
                                command.Parameters.AddWithValue("@Plate", plate);
                                connection.Open();
                                SqlDataReader reader = command.ExecuteReader();

                                // Pre-fill the form with the retrieved vehicle information
                                if (reader.Read())
                                {
                                    int vehicleId = Convert.ToInt32(reader["vehicle_id"]);
                                    txtyear.Text = reader["vehicle_year"].ToString();
                                    txtmake.Text = reader["vehicle_make"].ToString();
                                    txtmodel.Text = reader["vehicle_model"].ToString();
                                    txtvin.Text = reader["vehicle_number"].ToString();
                                    txtodometer.Text = reader["vehicle_mileage"].ToString();
                                    txtlicenseplate.Text = reader["vehicle_plate"].ToString();

                                    // Store the vehicle ID in the hidden field
                                    vehicleIdHiddenField.Value = vehicleId.ToString();
                                }
                                reader.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string eventSource = "SCCFleetServices";
                        string errorMessage = "Update Vehicle Page Load method error: " + ex.Message;
                        EventViewerLogger logger = new EventViewerLogger(eventSource);
                        logger.LogError(errorMessage);
                        // Log or display the exception message
                        Response.Write($"An error occurred while retrieving the vehicle information: {ex.Message}");
                    }
                }
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // Handle the submission of the updated vehicle information
            // Retrieve the updated values from the form controls
            string year = txtyear.Text;
            string make = txtmake.Text;
            string model = txtmodel.Text;
            string vin = txtvin.Text;
            string licensePlate = txtlicenseplate.Text;
            int odometer;
            if (int.TryParse(txtodometer.Text, out odometer))
            {
                // Update the vehicle record in the database
                int vehicleId;
                if (int.TryParse(vehicleIdHiddenField.Value, out vehicleId))
                {
                    try
                    {
                        string connectionString = ConfigurationManager.ConnectionStrings["FleetProd"].ConnectionString;
                        string query = "UPDATE vehicle SET vehicle_year = @Year, vehicle_make = @Make, vehicle_model = @Model, vehicle_number = @Number, vehicle_mileage = @Mileage, vehicle_plate = @Plate WHERE vehicle_id = @VehicleId";

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@Year", year);
                                command.Parameters.AddWithValue("@Make", make);
                                command.Parameters.AddWithValue("@Model", model);
                                command.Parameters.AddWithValue("@Number", vin);
                                command.Parameters.AddWithValue("@Mileage", odometer);
                                command.Parameters.AddWithValue("@Plate", licensePlate);
                                command.Parameters.AddWithValue("@VehicleId", vehicleId);

                                connection.Open();
                                command.ExecuteNonQuery();
                            }
                        }


                        lblerror.Text = "The record has been updated.";
                        lblerror.ForeColor = System.Drawing.Color.Green;
                    }
                    catch (Exception ex)
                    {
                        // Log or display the exception message
                        Response.Write($"An error occurred while updating the vehicle record: {ex.Message}");
                    }
                   

                }
            }
        }

    }
}