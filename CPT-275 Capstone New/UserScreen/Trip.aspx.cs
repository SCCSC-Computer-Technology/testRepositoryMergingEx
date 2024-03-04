using CPT_275_Capstone.AdminDashboard;
using CPT_275_Capstone.App_Start;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
//using System.Runtime.Serialization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CPT_275_Capstone.UserScreen
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string script = @"
                <script type='text/javascript'>
                    if (window.performance && window.performance.navigation.type === 2) {
                        window.location.reload(true);
                    }
                </script>
            ";
            ClientScript.RegisterStartupScript(this.GetType(), "RefreshScript", script);
            if (!IsPostBack)
            {
                if (Session["SelectedCarId"] != null)
                {
                    string selectedCarId = Session["SelectedCarId"].ToString();

                    // Split the selected car details into separate parts
                    string[] carParts = selectedCarId.Split(' ');

                    if (carParts.Length >= 1)
                    {
                        string vehicleId = carParts[carParts.Length - 1].Trim();

                        // Get the complete vehicle details (year, make, model, and license plate)
                        string vehicleDetails = GetVehicleDetails(vehicleId);

                        lblselectedcar.Text = vehicleDetails; // Display the selected vehicle details

                        // Save the necessary information in session variables or use them as needed
                        Session["SelectedCarId"] = vehicleId; // Use the vehicle ID as the selected car ID
                    }
                    else
                    {
                        lblmessage.Text = "Invalid vehicle details";
                    }
                }
                else
                {
                    lblmessage.Text = "No vehicle selected";
                }
            }
        }
        protected void TripForSelectedVehicle(string selectedCarId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["FleetProd"].ConnectionString;
            try
            {
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                int begginingMileage = 0;
                string destination = "";
                string purpose = "";
                int endingMileage = 0;
                int totalMiles = 0;
                int userId = 0;

                //Inserting the trip details into the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO trip (trip_date, trip_beginning_mileage, trip_destination, trip_purpose, trip_ending_mileage, trip_total_miles, vehicle_id, user_id) " +
                "VALUES (@trip_date, @trip_beginning_mileage, @trip_destination, @trip_purpose, @trip_ending_mileage, @trip_total_miles, @vehicle_id, @user_id)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@trip_date", date);
                        command.Parameters.AddWithValue("@trip_beginning_mileage", begginingMileage);
                        command.Parameters.AddWithValue("@trip_destination", destination);
                        command.Parameters.AddWithValue("@trip_purpose", purpose);
                        command.Parameters.AddWithValue("@trip_ending_mileage", endingMileage);
                        command.Parameters.AddWithValue("@trip_total_miles", totalMiles);
                        command.Parameters.AddWithValue("@user_id", userId);



                        int vehicleId;
                        if (!int.TryParse(selectedCarId, out vehicleId))
                        {
                            command.Parameters.AddWithValue("@vehicle_id", vehicleId);
                        }else
                        {
                            lblmessage.Text = "Invalid vehicle ID";
                            return;
                        }

                        connection.Open();
                        command.ExecuteNonQuery();
                    }


                }
                lblmessage.Text = "Trip details added successfully";
                lblmessage.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                string eventSource = "SCCFleetServices";
                string errorMessage = "Loading the vehicles from the user screen into the trip screen on the userscreen side: " + ex.Message;
                EventViewerLogger logger = new EventViewerLogger(eventSource);
                logger.LogError(errorMessage);
                lblmessage.Text = "Error retrieving vehicle information " + ex.Message;
            }

        }
        //Calculate the total miles driven
        private string GetVehicleDetails(string vehicleId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["FleetProd"].ConnectionString;
            string query = "SELECT CONCAT(vehicle_year, ' ', vehicle_make, ' ', vehicle_model, ' - ', vehicle_plate) AS VehicleDetails FROM vehicle WHERE vehicle_id = @vehicle_id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    int vehicleIdInt;
                    if (!int.TryParse(vehicleId, out vehicleIdInt))
                    {
                        return "Invalid vehicle ID";
                    }
                    command.Parameters.AddWithValue("@vehicle_id", vehicleIdInt);

                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null)
                        return result.ToString();
                }
            }

            return "Vehicle details not found";
        }
        protected void CalculateTotalMiles(object sender, EventArgs e)
        {
            int beginningMileage = 0;
            int endingMileage = 0;
            try
            {
                if (int.TryParse(txtBeginningMileage.Text, out beginningMileage) && int.TryParse(txtEndingMileage.Text, out endingMileage))
                {
                    int totalMiles = endingMileage - beginningMileage;
                    lbltotalmilese.Text = totalMiles.ToString();
                }
                else
                {
                    lbltotalmilese.Text = "0";
                }
            }
            catch (Exception ex)
            {
                string eventSource = "SCCFleetServices";
                string errorMessage = "Calculating the total miles on the trip screen under UserScreen: " + ex.Message;
                EventViewerLogger logger = new EventViewerLogger(eventSource);
                logger.LogError(errorMessage);
                lblmessage.Text = "Error: " + ex.Message;
            }
        }
        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    string selectedCarId = Session["SelectedCarId"].ToString();
                    string date = DateTime.Now.ToString("yyyy-MM-dd");
                    int beginningMileage = Convert.ToInt32(txtBeginningMileage.Text);
                    string destination = txtDestination.Text;
                    string purpose = txtPurposeoftrip.Text;
                    int endingMileage = Convert.ToInt32(txtEndingMileage.Text);
                    int totalMiles = Convert.ToInt32(lbltotalmilese.Text);
                    int userId = Convert.ToInt32(Session["UserId"]);

                    // Inserting the trip details into the database
                    string connectionString = ConfigurationManager.ConnectionStrings["FleetProd"].ConnectionString;
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
                            command.Parameters.AddWithValue("@vehicle_id", selectedCarId);
                            command.Parameters.AddWithValue("@user_id", userId);

                            connection.Open();
                            command.ExecuteNonQuery();
                            connection.Close();
                        }
                    }
                    lblmessage.Text = "Trip details added successfully";
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
                string errorMessage = "Submit method error on the trip screen under userScreen: " + ex.Message;
                EventViewerLogger logger = new EventViewerLogger(eventSource);
                logger.LogError(errorMessage);
                lblmessage.Text = "Error: " + ex.Message;
            }
        }

        protected void btncancel_Click(object sender, EventArgs e)
        {
            //Redirect to the previous screen
            Response.Redirect("User Screen.aspx");
        }
    }
}
