using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.AspNet.FriendlyUrls;
using CPT_275_Capstone.App_Start;

namespace CPT_275_Capstone
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateCarDropDownList();
                PopulateUsersListBox();

                string script = @"
                <script type='text/javascript'>
                    if (window.performance && window.performance.navigation.type === 2) {
                        window.location.reload(true);
                    }
                </script>
            ";
                ClientScript.RegisterStartupScript(this.GetType(), "RefreshScript", script);
            }
            if (!IsPostBack)
            {
                
                List<string> users = GetUsersList();
                lstUsers.DataSource = users;
                lstUsers.DataBind();
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

                            ListItem item = new ListItem(carName, carID);
                            drpcars.Items.Add(item);
                        }
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                string eventSource = "SCCFleetServices";
                string errorMessage = "Error populating the list of vehicles on the trip screen on the admin side: "+ ex.Message;
                EventViewerLogger logger = new EventViewerLogger(eventSource);
                logger.LogError(errorMessage);
                lblmessage.Text = "Error retrieving vehicle information" + ex.Message;
            }
        }
        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            try
            { 
            if (Page.IsValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["FleetProd"].ConnectionString;

                // Retrieve the trip details and store it in the database
                string date = txtDate.Text;
                int beginningMileage = int.Parse(txtBeginningMileage.Text);
                string destination = txtDestination.Text;
                string purpose = txtPurposeoftrip.Text;
                int endingMileage = int.Parse(txtEndingMileage.Text);
                int totalMiles = endingMileage - beginningMileage;

                // Getting the selected vehicle from the dropdown list
                string selectedVehicle = drpcars.SelectedItem?.Value;

                // Getting the selected users from the listbox and retrieving their corresponding user IDs
                List<string> selectedUserIDs = new List<string>();
                foreach (ListItem item in lstUsers.Items)
                {
                    if (item.Selected)
                    {
                        string[] userInfoParts = item.Text.Split('-');
                        string userFullName = userInfoParts[0].Trim();
                        string dlNumber = userInfoParts[1].Trim();

                        string[] userFullNameParts = userFullName.Split(' ');
                        string firstName = userFullNameParts[0];
                        string lastName = userFullNameParts[1];

                        string userID = GetUserID(firstName, lastName, dlNumber, connectionString);
                        if (!string.IsNullOrEmpty(userID))
                        {
                            selectedUserIDs.Add(userID);
                        }
                    }
                }

                // Insert the trip details into the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO trip (trip_date, trip_beginning_mileage, trip_destination, trip_purpose, trip_ending_mileage, trip_total_miles, vehicle_id, user_id) " +
                        "VALUES (@trip_date, @trip_beginning_mileage, @trip_destination, @trip_purpose, @trip_ending_mileage, @trip_total_miles, @vehicle_id, @user_id)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@trip_date", date);
                        command.Parameters.AddWithValue("@trip_beginning_mileage", beginningMileage);
                        command.Parameters.AddWithValue("@trip_destination", destination);
                        command.Parameters.AddWithValue("@trip_purpose", purpose);
                        command.Parameters.AddWithValue("@trip_ending_mileage", endingMileage);
                        command.Parameters.AddWithValue("@trip_total_miles", totalMiles);
                        command.Parameters.AddWithValue("@vehicle_id", selectedVehicle);
                        command.Parameters.AddWithValue("@user_id", string.Join(",", selectedUserIDs));

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                // Output message to the user
                lblmessage.Text = "Trip details have been submitted!";
                lblmessage.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lblmessage.Text = "Please fill out all the required fields";
                lblmessage.ForeColor = System.Drawing.Color.Red;
            }
        }
            catch (Exception ex)
            {
                string eventSource = "SCCFleetServices";
                string errorMessage = "Error submitting trip details on the admin side: " + ex.Message;
                EventViewerLogger logger = new EventViewerLogger(eventSource);
                logger.LogError(errorMessage);
                lblmessage.Text = "Enter a missing information in the fields";
                lblmessage.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void PopulateUsersListBox()
        {
            List<string> users = GetUsersList();

            lstUsers.DataSource = users;
            lstUsers.DataBind();
        }

        protected void btncancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

        private string GetUserID(string firstName, string lastName, string dlNumber, string connectionString)
        {
            string userID = string.Empty;

            try
            {
                string query = "SELECT user_id FROM users WHERE users_first_name = @first_name AND users_last_name = @last_name AND users_DL = @dl_number";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@first_name", firstName);
                        command.Parameters.AddWithValue("@last_name", lastName);
                        command.Parameters.AddWithValue("@dl_number", dlNumber);

                        connection.Open();

                        // Use SqlDataReader to retrieve the user_id
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            userID = reader["user_id"].ToString();
                        }

                        reader.Close();
                    }
                }
            }
            catch (Exception)
            {
                // Handle the exception or log it if needed
            }

            return userID;
        }
        private List<string> GetUsersList()
        {
            List<string> users = new List<string>();

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
                        string userId = reader["user_id"].ToString();
                        string userinfo = reader["UserInfo"].ToString();
                        users.Add(userinfo);
                        lstUsers.Items.Add(new ListItem(userinfo, userId));
                    }
                    reader.Close();
                }
            }
            return users;
        }
    }
}