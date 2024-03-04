using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CPT_275_Capstone.App_Start;

namespace CPT_275_Capstone.AdminDashboard
{
    public partial class UpdateUser : System.Web.UI.Page
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

                if (Request.QueryString["firstNamelastName"] != null && Request.QueryString["dlNumber"] != null)
                {
                    string firstNamelastName = Request.QueryString["firstNamelastName"];
                    string dlNumber = Request.QueryString["dlNumber"];

                    try
                    {
                        // Retrieve the user information by firstNamelastName and dlNumber from the database
                        string connectionString = ConfigurationManager.ConnectionStrings["FleetProd"].ConnectionString;
                        string query = "SELECT users_first_name, users_last_name, users_email, users_password, users_DL, users_DL_state," +
                            "users_telephone, users_type, vehicle_id, user_id FROM users WHERE CONCAT(users_first_name, ' ', users_last_name) = @FirstNamelastName AND users_DL = @DLNumber";

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@FirstNamelastName", firstNamelastName);
                                command.Parameters.AddWithValue("@DLNumber", dlNumber);
                                connection.Open();
                                SqlDataReader reader = command.ExecuteReader();

                                // Pre-fill the form with the retrieved user information
                                if (reader.Read())
                                {
                                    int userId = Convert.ToInt32(reader["user_id"]);
                                    txtfirstname.Text = reader["users_first_name"].ToString();
                                    txtlastname.Text = reader["users_last_name"].ToString();
                                    txtemail.Text = reader["users_email"].ToString();
                                    txtpassword.Text = reader["users_password"].ToString();
                                    txtdlNumber.Text = reader["users_DL"].ToString();
                                    txtdlState.Text = reader["users_DL_state"].ToString();
                                    txtphone.Text = reader["users_telephone"].ToString();
                                    userIdHiddenField.Value = userId.ToString();

                                    string userType = reader["users_type"].ToString();
                                    ListItem selectedUserType = accountType.Items.FindByValue(userType);
                                    if (selectedUserType != null)
                                    {
                                        selectedUserType.Selected = true;
                                    }

                                    string defaultVehicle = reader["vehicle_id"].ToString();
                                    PopulateVehicleList(defaultVehicle);
                                }
                                reader.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log or display the exception message
                        string eventSource = "SCCFleetServices";
                        string errorMessage = $"An error occurred while retrieving the user information: {ex.Message}";
                        EventViewerLogger logger = new EventViewerLogger(eventSource);
                        logger.LogError(errorMessage);
                        Response.Write($"An error occurred while retrieving the user information: {ex.Message}");
                    }
                }
            }
        }

        private void PopulateVehicleList(string defaultVehicle)
        {
            // Retrieve the list of available vehicles from the database
            string connectionString = ConfigurationManager.ConnectionStrings["FleetProd"].ConnectionString;
            string query = "SELECT vehicle_id, CONCAT(vehicle_year, ' ', vehicle_make, ' ', vehicle_model,' ', vehicle_plate) AS vehicle_name FROM vehicle";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    // Populate the vehicle listbox with available vehicles
                    while (reader.Read())
                    {
                        ListItem item = new ListItem(reader["vehicle_name"].ToString(), reader["vehicle_id"].ToString());
                        lstvehicle.Items.Add(item);
                    }
                    reader.Close();
                }
            }

            // Select the default vehicle in the listbox
            ListItem selectedVehicle = lstvehicle.Items.FindByValue(defaultVehicle);
            if (selectedVehicle != null)
            {
                selectedVehicle.Selected = true;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string firstName = txtfirstname.Text;
            string lastName = txtlastname.Text;
            string email = txtemail.Text;
            string password = txtpassword.Text;
            string dlNumber = txtdlNumber.Text;
            string dlState = txtdlState.Text;
            string phone = txtphone.Text;
            int userId;
            if (int.TryParse(userIdHiddenField.Value, out userId))
            {
                try
                {
                    // Update the user information in the database
                    string connectionString = ConfigurationManager.ConnectionStrings["FleetProd"].ConnectionString;
                    string query = "UPDATE users SET users_first_name = @FirstName, users_last_name = @LastName, " +
                        "users_email = @Email, users_password = @Password, users_DL = @DLNumber, users_DL_state = @DLState, " +
                        "users_telephone = @Phone, users_type = @UserType, vehicle_id = @DefaultVehicle WHERE user_id = @UserId";

                    bool userExistsemailanddlnumber = CheckUserExistsemail(connectionString, email, dlNumber);

                    
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@FirstName", firstName);
                            command.Parameters.AddWithValue("@LastName", lastName);
                            command.Parameters.AddWithValue("@Email", email);
                            command.Parameters.AddWithValue("@Password", password);
                            command.Parameters.AddWithValue("@DLNumber", dlNumber);
                            command.Parameters.AddWithValue("@DLState", dlState);
                            command.Parameters.AddWithValue("@Phone", phone);
                            command.Parameters.AddWithValue("@UserType", accountType.SelectedValue);
                            command.Parameters.AddWithValue("@DefaultVehicle", lstvehicle.SelectedValue);
                            command.Parameters.AddWithValue("@UserId", userId);

                            connection.Open();
                            int rowsAffected = command.ExecuteNonQuery();
                            lblerror.Text = "The record has been updated.";
                            lblerror.ForeColor = System.Drawing.Color.Green;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log or display the exception message
                    Response.Write($"An error occurred while updating the user record: {ex.Message}");
                }
            }
        }
        private bool CheckUserExistsemail(string connectionString, string email, string dlNumber)
        {
            string query2 = "SELECT COUNT(*) FROM users WHERE users_email = @Email AND users_DL = @DLNum";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query2, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@DLNum", dlNumber);
                    connection.Open();
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    if (count > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}