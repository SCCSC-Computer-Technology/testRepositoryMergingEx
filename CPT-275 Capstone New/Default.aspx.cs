using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CPT_275_Capstone

{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if there is a valid session user type
                if (!IsValidSessionUserType())
                {
                    Response.Redirect("~/Login/Login.aspx");
                }
                else
                {
                    
                    string script = @"
                <script type='text/javascript'>
                    if (window.performance && window.performance.navigation.type === 2) {
                        window.location.reload(true);
                    }
                </script>
            ";
                    ClientScript.RegisterStartupScript(this.GetType(), "RefreshScript", script);

                    // Retrieve the vehicle information from the database
                    List<string> vehicles = GetVehicleList();
                    List<string> users = GetUsersList();

                    // Bind the vehicle information to the listbox
                    lstVehicles.DataSource = vehicles;
                    lstVehicles.DataBind();

                    // Bind the users information to the listbox
                    lstUsers.DataSource = users;
                    lstUsers.DataBind();
                }
            }
        }

        private bool IsValidSessionUserType()
        {
            // Check if the session user type is valid
            if (Session["UserType"] != null )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string selectedVehicle = lstVehicles.SelectedItem?.Text;
            string selectedUser = lstUsers.SelectedItem?.Text;

            if (!string.IsNullOrEmpty(selectedVehicle))
            {
                // Remove the selected vehicle from the list
                lstVehicles.Items.Remove(selectedVehicle);

                // Delete the vehicle record from the database
                DeleteVehicle(selectedVehicle);

                string script = @"<script type='text/javascript'>
                            Swal.fire('Deleted!', 'The vehicle record has been deleted.', 'success');
                         </script>";
                ClientScript.RegisterStartupScript(this.GetType(), "DeleteVehicleScript", script);
            }

            if (!string.IsNullOrEmpty(selectedUser))
            {
                // Remove the selected user from the list
                lstUsers.Items.Remove(selectedUser);

                // Delete the user record from the database
                DeleteUser(selectedUser);

                // Show a success message 
                string script = @"<script type='text/javascript'>
                            Swal.fire('Deleted!', 'The user record has been deleted.', 'success');
                         </script>";
                ClientScript.RegisterStartupScript(this.GetType(), "DeleteUserScript", script);
            }
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            if (lstVehicles.SelectedItem != null)
            {
                // Vehicle list is selected
                string selectedVehicle = lstVehicles.SelectedItem.Text;
                string[] vehicleInfo = selectedVehicle.Split('-');
                string yearMakeModel = vehicleInfo[0].Trim();
                string plate = vehicleInfo[1].Trim();

                Response.Redirect($"~/AdminDashboard/UpdateVehicle.aspx?yearMakeModel={yearMakeModel}&plate={plate}");
            }
            else if (lstUsers.SelectedItem != null)
            {
                // User list is selected
                string selectedUser = lstUsers.SelectedItem.Text;
                string[] userInfo = selectedUser.Split('-');
                string firstNamelastName = userInfo[0].Trim();
                
                string dlNumber = userInfo[1].Trim();

                Response.Redirect($"~/AdminDashboard/UpdateUser.aspx?firstNamelastName={firstNamelastName}&dlNumber={dlNumber}");
            }
        }

        private string GetVehicleIdFromText(string vehicleText)
        {

            int hyphenIndex = vehicleText.IndexOf('-');
            if (hyphenIndex != -1)
            {
                string vehicleId = vehicleText.Substring(0, hyphenIndex).Trim();
                return vehicleId;
            }
            return null;
        }

        private List<string> GetVehicleList()
        {
            List<string> vehicles = new List<string>();

            // Fetch the vehicle information from the database
            string connectionString = ConfigurationManager.ConnectionStrings["FleetProd"].ConnectionString;
            string query = "SELECT CONCAT(vehicle_year, ' ', vehicle_make, ' ', vehicle_model, ' - ', vehicle_plate) as VehicleInfo FROM vehicle";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    // Retrieve the vehicle information and store it in the list
                    while (reader.Read())
                    {
                        string vehicleinfo = reader["VehicleInfo"].ToString();
                        vehicles.Add(vehicleinfo);
                    }
                    reader.Close();
                }
            }
            return vehicles;
        }

        private List<string> GetUsersList()
        {
            List<string> users = new List<string>();

            // Fetch the users information from the database
            string connectionString = ConfigurationManager.ConnectionStrings["FleetProd"].ConnectionString;
            string query = "SELECT CONCAT(users_first_name, ' ', users_last_name, ' - ', users_DL) as UserInfo FROM users";

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
                        users.Add(userinfo);
                    }
                    reader.Close();
                }
            }
            return users;
        }
        private void DeleteVehicle(string vehicleInfo)
        {
                string connectionString = ConfigurationManager.ConnectionStrings["FleetProd"].ConnectionString;
                string query = "DELETE FROM vehicle WHERE CONCAT(vehicle_year, ' ', vehicle_make, ' ', vehicle_model, ' - ', vehicle_plate) = @VehicleInfo";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@VehicleInfo", vehicleInfo);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
        }

        private void DeleteUser(string userInfo)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["FleetProd"].ConnectionString;
            string query = "DELETE FROM users WHERE CONCAT(users_first_name, ' ', users_last_name, ' - ', users_DL) = @UserInfo";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserInfo", userInfo);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}