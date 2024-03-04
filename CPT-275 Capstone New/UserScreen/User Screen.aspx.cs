using CPT_275_Capstone.App_Start;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CPT_275_Capstone.UserScreen
{
    public partial class User_Screen : System.Web.UI.Page
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

                if (IsValidSessionUserType())
                { 
                List<ListItem> vehicles = GetVehicleList();
                lstvehicle.DataTextField = "Text";
                lstvehicle.DataValueField = "Value";
                lstvehicle.DataSource = vehicles;
                lstvehicle.DataBind();

                // Retrieve user information from the session
                string lastName = Session["LastName"]?.ToString();
                string firstNameInitial = Session["FirstNameInitial"]?.ToString();

                // Retrieve the default vehicle for the user
                string defaultVehicleId = GetDefaultVehicleIdForUser(lastName, firstNameInitial);

                // Set the default vehicle as the selected item in the list
                if (!string.IsNullOrEmpty(defaultVehicleId))
                {
                    lstvehicle.SelectedValue = defaultVehicleId;
                }
                }
                else
                {
                    // Redirect to the login page
                    Response.Redirect("~/Login/Login.aspx");
                }
            }
        }

        private bool IsValidSessionUserType()
        {
            // Check if the session user type is valid
            // You can replace this with your own logic to validate the session user type
            if (Session["UserType"] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Vehicles listbox
        private List<ListItem> GetVehicleList()
        {
            List<ListItem> vehicles = new List<ListItem>();

            // Fetch the vehicle information from the database
            string connectionString = ConfigurationManager.ConnectionStrings["FleetProd"].ConnectionString;
            string query = "SELECT vehicle_id, CONCAT(vehicle_year, ' ', vehicle_make, ' ', vehicle_model, ' - ', vehicle_plate) AS VehicleInfo FROM vehicle";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    // Retrieve the vehicle information and store it in the list
                    while (reader.Read())
                    {
                        int vehicleId = Convert.ToInt32(reader["vehicle_id"]);
                        string vehicleInfo = reader["VehicleInfo"].ToString();

                        ListItem vehicleItem = new ListItem(vehicleInfo, vehicleId.ToString());
                        vehicles.Add(vehicleItem);
                    }
                    reader.Close();
                }
            }

            return vehicles;
        }
        private int GetUserIdByLastNameAndFirstNameInitial(string lastName, string firstNameInitial)
        {
            int userId = 0;

            // Retrieve the user ID from the database
            string connectionString = ConfigurationManager.ConnectionStrings["FleetProd"].ConnectionString;
            string query = "SELECT user_id FROM users WHERE users_last_name = @LastName AND users_first_name LIKE @FirstName";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@FirstName", firstNameInitial + "%"); // Use a wildcard for the first name initial

                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        userId = Convert.ToInt32(result);
                    }
                }
            }

            return userId;
        }

        protected void NewInspection_Click(object sender, EventArgs e)
        {
            // Check if a vehicle is selected
            if (lstvehicle.SelectedItem != null)
            {
                Session["SelectedCarId"] = Convert.ToInt32(lstvehicle.SelectedItem.Value); // Store the selected vehicle ID in the session as an integer
                                                                                           // Store the selected vehicle in the session
                Response.Redirect("Inspection.aspx");
            }
            else
            {
                ErrorMessageLabel.Text = "Please select a vehicle before proceeding.";
            }
        }

        protected void NewTripButton_Click(object sender, EventArgs e)
        {
            // Check if a vehicle is selected
            if (lstvehicle.SelectedItem != null)
            {
                Session["SelectedCarId"] = lstvehicle.SelectedItem.Value; // Store the selected vehicle ID in the session

                // Retrieve the user information from the session
                string lastName = Session["LastName"]?.ToString();
                string firstNameInitial = Session["FirstNameInitial"]?.ToString();

                // Retrieve the user ID based on last name and first name initial
                int userId = GetUserIdByLastNameAndFirstNameInitial(lastName, firstNameInitial);

                // Store the user ID in the session
                Session["UserId"] = userId;

                Response.Redirect("Trip.aspx");
            }
            else
            {
                ErrorMessageLabel.Text = "Please select a vehicle before proceeding.";
            }
        }

        protected void ExitButton_Click(object sender, EventArgs e)
        {
            {
                Session.Clear();
                Session.Abandon();
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
                Response.Cache.SetNoStore();
                HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate");
                HttpContext.Current.Response.AddHeader("Pragma", "no-cache");
                HttpContext.Current.Response.AddHeader("Expires", "0");
                Response.Redirect("~/Login/Logout.aspx");
            }
        }

        private string GetDefaultVehicleIdForUser(string lastName, string firstNameInitial)
        {
            try
            { 
            string defaultVehicleId = null;

            // Retrieve the default vehicle ID from the database
            string connectionString = ConfigurationManager.ConnectionStrings["FleetProd"].ConnectionString;
            string query = "SELECT vehicle.vehicle_id " +
                           "FROM vehicle " +
                           "INNER JOIN users ON vehicle.vehicle_id = users.vehicle_id " +
                           "WHERE users.users_last_name = @LastName AND users.users_first_name LIKE @FirstName";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Lastname", lastName);
                    command.Parameters.AddWithValue("@FirstName", firstNameInitial + "%"); // Use a wildcard for last name matching

                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        defaultVehicleId = result.ToString();
                    }
                }
            }

            return defaultVehicleId;
        }catch (Exception ex)
            {
                string eventSource = "SCCFleetServices";
                string errorMessage = "Loading the vehicles from the user screen into the trip screen on the userscreen side: " + ex.Message;
                EventViewerLogger logger = new EventViewerLogger(eventSource);
                logger.LogError(errorMessage);
                ErrorMessageLabel.Text = ex.Message;
                return null;
            }
        }
    }
}





