using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration; // Required for ConfigurationManager
using System.Data.SqlClient; // Required for SqlConnection, SqlCommand, SqlDataReader
using System.Data;
using CPT_275_Capstone.App_Start;

namespace CPT_275_Capstone.AdminDashboard
{
    public partial class AddVehicle : System.Web.UI.Page
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

        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["FleetProd"].ConnectionString;

            //Get vehicle information from the form
            int year = int.Parse(txtyear.Text);
            string make = txtmake.Text;
            string model = txtmodel.Text;
            string vin = txtvin.Text;
            string licenseplate = txtlicenseplate.Text;
            int mileage = int.Parse(txtodometer.Text);
            int vehicleid = 0;

            //Condition to check whether the car exist or not by checking the license plate and vin number
            bool carexist = CheckCarExisting(connectionString, licenseplate, vin);
            if (carexist)
            {
                lblerror.Text = "Vehicle with the same VIN# or license plate already exists in the system";
                lblerror.ForeColor = System.Drawing.Color.Red;
                return;
            }
            //Validation check for make and model fields to not allow the special characters
            if (ContainsSpecialCharacters(make) || ContainsSpecialCharacters(model) || ContainsSpecialCharacters(vin) || ContainsSpecialCharacters(licenseplate))
            {
                lblerror.Text = "Error: No fields should not contain special characters.";
                lblerror.ForeColor = System.Drawing.Color.Red;
                return;
            }

            // Create the SQL query to insert the vehicle information
            string query = "INSERT INTO vehicle (vehicle_year,vehicle_make, vehicle_model, vehicle_number, vehicle_mileage, vehicle_plate) " +
                           "VALUES (@Year,@Make, @Model, @VIN, @Mileage, @LicensePlate);" +
                           "SELECT CAST(SCOPE_IDENTITY() AS INT)";

            //string retrieveIdQuery = "SELECT SCOPE_IDENTITY()";

            //Execute the query
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    //Set the parameters
                    command.Parameters.AddWithValue("@Year", year);
                    command.Parameters.AddWithValue("@Make", make);
                    command.Parameters.AddWithValue("@Model", model);
                    command.Parameters.AddWithValue("@VIN", vin);
                    command.Parameters.AddWithValue("@LicensePlate", licenseplate);
                    command.Parameters.AddWithValue("@Mileage", mileage);
                    try
                    {
                        //Open the connection
                        connection.Open();

                        //Execute the query
                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            vehicleid = Convert.ToInt32(result);
                            lblerror.Text = "Vehicle added successfully! Vehicle ID: " + vehicleid;
                            lblerror.ForeColor = System.Drawing.Color.Green;
                        }
                        else
                        {
                            lblerror.Text = "Error: Vehicle not added";
                            lblerror.ForeColor = System.Drawing.Color.Red;
                        }

                    }
                    catch (Exception ex)
                    {
                        string eventSource = "SCCFleetServices";
                        string errorMessage = "Add Vehicle screen error message " + ex.Message;
                        EventViewerLogger logger = new EventViewerLogger(eventSource);
                        logger.LogError(errorMessage);
                        lblerror.Text = "Error: " + ex.Message;
                        lblerror.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }

        }
        private bool CheckCarExisting(string connectionString, string licenseplate, string vin)
        {
            string query2 = "SELECT COUNT(*) FROM vehicle WHERE vehicle_plate = @Plate OR vehicle_number = @VinNumber";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query2, connection))
                {
                    command.Parameters.AddWithValue("@Plate", licenseplate);
                    command.Parameters.AddWithValue("@VinNumber", vin);
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
        // Helper method to check if a string contains special characters
        private bool ContainsSpecialCharacters(string input)
        {
            string specialCharacters = @"~`!@#$%^&*()-+=[]{}\|;:'"",.<>?/";

            foreach (char character in input)
            {
                if (specialCharacters.Contains(character))
                    return true;
            }

            return false;
        }
    }
}