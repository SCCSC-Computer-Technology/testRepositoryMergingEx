using CPT_275_Capstone.App_Start;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CPT_275_Capstone.UserScreen
{
    public partial class WebForm1 : System.Web.UI.Page
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
                if (Session["SelectedCarId"] != null)
                {
                    int selectedCarId = Convert.ToInt32(Session["SelectedCarId"]);
                    lblselectedcar.Text = GetVehicleDetails(selectedCarId); // Display the selected vehicle details on the page
                }
                else
                {
                    //Handle the case where no car is selected
                    lblselectedcar.Text = "No vehicle selected";
                }

            }
        }
        private string GetVehicleDetails(int vehicleId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["FleetProd"].ConnectionString;
            //string query = "SELECT CONCAT(vehicle_year, ' ', vehicle_make, ' ', vehicle_model, ' - ', vehicle_plate) AS VehicleDetails FROM vehicle WHERE vehicle_id = @vehicle_id";
            string query = "SELECT CONCAT(vehicle_year, ' ', vehicle_make, ' ', vehicle_model, ' - ', vehicle_plate) AS VehicleDetails FROM vehicle WHERE vehicle_id = @vehicle_id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@vehicle_id", vehicleId);

                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null)
                    return result.ToString();
                }
            }

            return "Vehicle details not found";
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            // Return to the previous page
            if (Request.UrlReferrer != null)
            {
                Response.Redirect(Request.UrlReferrer.ToString());
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                // Get form field values
                string date = txtDate.Text;
                int beginningMileage = Convert.ToInt32(txtbeginningmilleage.Text);
                int endingMileage = Convert.ToInt32(txtendingmileage.Text);
                DateTime lastOilChangeDate = Convert.ToDateTime(txtlastoilchangedate.Text);
                DateTime oilChangeDue = Convert.ToDateTime(txtoilchangedue.Text);
                //int interval = Convert.ToInt32(txtinterval.Text);
                int interval = (oilChangeDue - lastOilChangeDate).Days;
                DateTime lastTireRotation = Convert.ToDateTime(txtlasttirerotation.Text);
                DateTime rotationDue = Convert.ToDateTime(txtrotationdue.Text);
                string tirePressure = txttirepressure.Text;
                string fluid;
                string battery;
                string gauge;
                string cleanInterior;
                string cleanCab;
                int vehicleID = 0;

                string selectedCarId = Session["SelectedCarId"] != null ? Session["SelectedCarId"].ToString() : "";
                //Calculate total miles driven
                int totalMilesDriven = endingMileage - beginningMileage;

                //Display the total miles driven
                lbltotalmilesdriventotal.Text = totalMilesDriven.ToString();
                lbltotalmilesdriventotal.Visible = true;

                txtinterval.Text = interval.ToString();

                if (chkfluidchecked.Checked)
                {
                    fluid = "Yes";
                }
                else
                {
                    fluid = "No";
                }

                if (chkBattery.Checked)
                {
                    battery = "Yes";
                }
                else
                {
                    battery = "No";
                }

                if (chkGaugesWorking.Checked)
                {
                    gauge = "Yes";
                }
                else
                {
                    gauge = "No";
                }

                if (chkCleanedInteriorExterior.Checked)
                {
                    cleanInterior = "Yes";
                }
                else
                {
                    cleanInterior = "No";
                }

                if (chkBedCabCleanOrganized.Checked)
                {
                    cleanCab = "Yes";
                }
                else
                {
                    cleanCab = "No";
                }

                string notes = txtnotes.Text;

                // Store the inspection data in the database
                bool success = InspectionDataInDatabase(date, selectedCarId, beginningMileage, endingMileage, totalMilesDriven, lastOilChangeDate, oilChangeDue,
                interval, lastTireRotation, rotationDue, tirePressure, fluid, battery, gauge, cleanInterior, cleanCab, notes);

                // Displays a success or error message
                if (success)
                {
                    lblmessage.Text = "Inspection was added successfully!";
                    lblmessage.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lblmessage.Text = "Enter a missing information in the fields";
                    lblmessage.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                string eventSource = "SCCFleetServices";
                string errorMessage = "Error on submit method on the inspeciton screen on the user screen side: " + ex.Message;
                EventViewerLogger logger = new EventViewerLogger(eventSource);
                logger.LogError(errorMessage);
                lblmessage.Text = "Enter a missing information in the fields";
                lblmessage.ForeColor = System.Drawing.Color.Red;
            }
        }

        private string GetSelectedCheckboxes()
        {
            List<string> selectedCheckboxes = new List<string>();

            if (chkfluidchecked.Checked)
            {
                selectedCheckboxes.Add(chkfluidchecked.Text);
            }

            if (chkBattery.Checked)
            {
                selectedCheckboxes.Add(chkBattery.Text);
            }

            if (chkGaugesWorking.Checked)
            {
                selectedCheckboxes.Add(chkGaugesWorking.Text);
            }

            if (chkCleanedInteriorExterior.Checked)
            {
                selectedCheckboxes.Add(chkCleanedInteriorExterior.Text);
            }

            if (chkBedCabCleanOrganized.Checked)
            {
                selectedCheckboxes.Add(chkBedCabCleanOrganized.Text);
            }

            if (selectedCheckboxes.Count > 0)
            {
                return string.Join(", ", selectedCheckboxes);
            }

            return null; // Return null when no checkboxes are selected
        }
        private bool InspectionDataInDatabase(string date, string vehicleId, int beginningMileage, int endingMileage, int totalMilesDriven,
            DateTime lastOilChangeDate, DateTime oilChangeDue, int interval, DateTime lastTireRotation,
            DateTime rotationDue, string tirePressure, string fluid, string battery, string gauge, string cleanInterior, string cleanCab, string notes)

        {
            string connectionString = ConfigurationManager.ConnectionStrings["FleetProd"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO inspection (inspection_date, inspection_beginning_mileage, inspection_ending_mileage, inspection_total_mileage_driven, " +
                           "inspection_last_oil_change_date, inspection_oil_change_due, inspection_interval, inspection_last_tire_rotation, " +
                           "inspection_tires_rotation_due, inspection_tires_pressure, " +
                           "inspection_additional_notes, vehicle_number, trip_fluid_level, battery_good, gauge_working, clean_cab, clean_exterior) " +
                           "VALUES (@inspection_date, @BeginningMileage, @EndingMileage, @TotalMilesDriven, " +
                           "@LastOilChangeDate, @OilChangeDue, @Interval, @LastTireRotation, @RotationDue, " +
                           "@TirePressure, @Notes, @VehicleId, @Fluid, @Battery, @Gauge, @Cab, @Exterior)"; // Updated query
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@inspection_date", date);
                        command.Parameters.AddWithValue("@BeginningMileage", beginningMileage);
                        command.Parameters.AddWithValue("@EndingMileage", endingMileage);
                        command.Parameters.AddWithValue("@TotalMilesDriven", totalMilesDriven);
                        command.Parameters.AddWithValue("@LastOilChangeDate", lastOilChangeDate.ToString("MM/dd/yyyy"));
                        command.Parameters.AddWithValue("@OilChangeDue", oilChangeDue.ToString("MM/dd/yyyy"));
                        command.Parameters.AddWithValue("@Interval", interval);
                        command.Parameters.AddWithValue("@LastTireRotation", lastTireRotation.ToString("MM/dd/yyyy"));
                        command.Parameters.AddWithValue("@RotationDue", rotationDue.ToString("MM/dd/yyyy"));
                        command.Parameters.AddWithValue("@TirePressure", tirePressure);
                        command.Parameters.AddWithValue("@Fluid", fluid);
                        command.Parameters.AddWithValue("@Battery", battery);
                        command.Parameters.AddWithValue("@Gauge", gauge);
                        command.Parameters.AddWithValue("@Cab", cleanCab);
                        command.Parameters.AddWithValue("@Exterior", cleanInterior);
                        command.Parameters.AddWithValue("@Notes", notes);
                        command.Parameters.AddWithValue("@VehicleId", vehicleId);

                        //Open connection
                        connection.Open();
                        //Execute the INSERT command
                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                string eventSource = "SCCFleetServices";
                string errorMessage = "Inserting the inspection data in the database on the inspection screen in the UserScreen folder: " + ex.Message;
                EventViewerLogger logger = new EventViewerLogger(eventSource);
                logger.LogError(errorMessage);
                lblmessage.Text = "Error storing inspection data in database " + ex.Message;
                lblmessage.ForeColor = System.Drawing.Color.Red;
                return false;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //Redirect to the previous screen
            Response.Redirect("User Screen.aspx");
        }
    }
    }