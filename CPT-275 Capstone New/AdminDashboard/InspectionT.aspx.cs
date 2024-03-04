using CPT_275_Capstone.App_Start;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CPT_275_Capstone
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateCarDropDownList();
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
                string errorMessage = "Inspection screen on the admin side error message: " + ex.Message;
                EventViewerLogger logger = new EventViewerLogger(eventSource);
                logger.LogError(errorMessage);
                lblmessage.Text = "Error retrieving vehicle information" + ex.Message;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                // Get form field values
                string date = txtDate.Text;
                string model = drpcars.SelectedValue;
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

                //Calculate total miles driven
                int totalMilesDriven = endingMileage - beginningMileage;

                //Display the total miles driven
                lbltotalmilesdriventotal.Text = totalMilesDriven.ToString();
                lbltotalmilesdriventotal.Visible = true;

                txtinterval.Text = interval.ToString();

                // Get selected checkbox values
                //string checklist = GetSelectedCheckboxes();
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
                bool success = InspectionDataInDatabase(date, model, beginningMileage, endingMileage, totalMilesDriven, lastOilChangeDate, oilChangeDue,
                interval, lastTireRotation, rotationDue, tirePressure, fluid, battery, gauge, cleanInterior, cleanCab, notes);

                // Displays a success or error message
                if (success)
                {
                    lblmessage.Text = "Inspection was added successfully!";
                    lblmessage.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lblmessage.Text = "Error storing inspection data in the database.";
                    lblmessage.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                string eventSource = "SCCFleetServices";
                string errorMessage = "Inspection screen on the admin side error message: " + ex.Message;
                EventViewerLogger logger = new EventViewerLogger(eventSource);
                logger.LogError(errorMessage);
                lblmessage.Text = "Enter a missing information in the fields ";
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
        private bool InspectionDataInDatabase(string date, string model, int beginningMileage, int endingMileage, int totalMilesDriven,
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
                        "@TirePressure, @Notes, @Model, @Fluid, @Battery, @Gauge, @Cab, @Exterior)";
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
                        command.Parameters.AddWithValue("@Notes", notes);
                        command.Parameters.AddWithValue("@Model", model);
                        command.Parameters.AddWithValue("@Fluid", fluid);
                        command.Parameters.AddWithValue("@Battery", battery);
                        command.Parameters.AddWithValue("@Gauge", gauge);
                        command.Parameters.AddWithValue("@Cab", cleanCab);
                        command.Parameters.AddWithValue("@Exterior", cleanInterior);

                        //Open connection
                        connection.Open();
                            //Execute the INSERT command
                            command.ExecuteNonQuery();
                    }
                }
                return true;
             }catch(Exception ex)
              {
                lblmessage.Text = "Error storing inspection data in database " + ex.Message;
                lblmessage.ForeColor = System.Drawing.Color.Red;
                return false;
              }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }
    }
}