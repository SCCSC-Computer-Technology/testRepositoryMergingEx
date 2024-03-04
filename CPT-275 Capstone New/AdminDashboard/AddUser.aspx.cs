using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;
using CPT_275_Capstone.App_Start;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading;

namespace CPT_275_Capstone.AdminDashboard
{
    public partial class AddDriver : System.Web.UI.Page
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

                List<Vehicle> vehicles = GetVehicleList();
                lstvehicle.DataSource = vehicles;
                lstvehicle.DataTextField = "VehicleInfo";
                lstvehicle.DataValueField = "VehicleID";
                lstvehicle.DataBind();

            }
        }

        private List<Vehicle> GetVehicleList()
        {
            List<Vehicle> vehicles = new List<Vehicle>();

            // Fetch the vehicle information from the database
            string connectionString = ConfigurationManager.ConnectionStrings["FleetProd"].ConnectionString;
            string query = "SELECT vehicle_id, CONCAT(vehicle_year, ' ', vehicle_make, ' ', vehicle_model, ' - ', vehicle_plate) as VehicleInfo FROM vehicle";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    // Retrieve the vehicle information and store it in the list
                    while (reader.Read())
                    {
                        int vehicleID = Convert.ToInt32(reader["vehicle_id"]);
                        string vehicleInfo = reader["VehicleInfo"].ToString();
                        Vehicle vehicle = new Vehicle(vehicleID, vehicleInfo);
                        vehicles.Add(vehicle);
                    }
                    reader.Close();
                }
            }
            return vehicles;
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["FleetProd"].ConnectionString;
                string firstName = txtfirstname.Text;
                string lastName = txtlastname.Text;
                string phone = txtphone.Text;
                string email = txtemail.Text;
                string password = txtpassword.Text;
                string dlNumber = txtdlNumber.Text;
                string dlState = txtdlState.Text;
                string usersType = accountType.Value;
                int? selectedVehicleID = null; // Use nullable int to allow null value for selected vehicle

                if (!string.IsNullOrEmpty(lstvehicle.SelectedValue))
                {
                    selectedVehicleID = Convert.ToInt32(lstvehicle.SelectedValue);
                }

                // Check if the combination of first name and last name already exists
                bool userExists = CheckUserExists(connectionString, firstName, lastName);
                bool userExistsemailanddlnumber = CheckUserExistsemail(connectionString, email,dlNumber);

                if (userExists)
                {
                    lblerror.Text = "Error: User with the same first name and last name already exists.";
                    lblerror.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                if (userExistsemailanddlnumber)
                {
                    lblerror.Text = "Error: User with the same email and DL number already exists.";
                    lblerror.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                // Validate the phone number format
                if (!Regex.IsMatch(phone, @"^\d{10}$"))
                {
                    lblerror.Text = "Error: Invalid phone number format. Please enter a 10-digit phone number.";
                    lblerror.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                // Validate the password format
                if (!Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$"))
                {
                    lblerror.Text = "Error: Invalid password format. Password must contain at least one uppercase letter, one lowercase letter, one digit, one special character, and be at least 8 characters long.";
                    lblerror.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                // Create the SQL query to insert the driver information
                string query = "INSERT INTO users (users_first_name, users_last_name, users_telephone, users_email, users_password, users_DL, users_DL_state, users_type, vehicle_id) " +
                           "VALUES (@FirstName, @LastName, @Phone, @Email, @Password, @DLNumber, @DLState, @UsersType, @VehicleID); SELECT SCOPE_IDENTITY()";

                // Execute the query
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Set the parameters
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@Phone", phone);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@DLNumber", dlNumber);
                        command.Parameters.AddWithValue("@DLState", dlState);
                        command.Parameters.AddWithValue("@UsersType", usersType); // Add the user type parameter


                        if (selectedVehicleID != null)
                        {
                            command.Parameters.AddWithValue("@VehicleID", selectedVehicleID);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@VehicleID", DBNull.Value);
                        }

                        try
                        {
                            connection.Open();
                            // Execute the query and retrieve the inserted user ID
                            int userId = Convert.ToInt32(command.ExecuteScalar());
                            if (userId > 0)
                            {
                                lblerror.Text = "User added successfully! User ID: " + userId;
                                lblerror.ForeColor = System.Drawing.Color.Green;
                                // Send an email to the user
                                SendEmail(txtemail.Text, txtpassword.Text);
                                ScriptManager.RegisterStartupScript(this, GetType(), "ShowAccountCreated", "ShowAccountCreated();", true);
                            }
                            else
                            {
                                lblerror.Text = "Error: User not added";
                                lblerror.ForeColor = System.Drawing.Color.Red;
                            }
                        }
                        catch (Exception ex)
                        {
                            string eventSource = "SCCFleetServices";
                            string errorMessage = "Add User screen error message: " + ex.Message;
                            EventViewerLogger logger = new EventViewerLogger(eventSource);
                            logger.LogError(errorMessage);
                            lblerror.Text = "Error: " + ex.Message;
                            lblerror.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string eventSource = "SCCFleetServices";
                string errorMessage = "Add User screen error message: " + ex.Message;
                EventViewerLogger logger = new EventViewerLogger(eventSource);
                logger.LogError(errorMessage);
                lblerror.Text = "Error: " + ex.Message;
                lblerror.ForeColor = System.Drawing.Color.Red;
            }
        }
        // Method to send an email to the user
        private void SendEmail(string email, string password)
        {
            // Replace with your email configuration settings
            string senderEmail = "maksbotukh@gmail.com";
            string senderPassword = "pkgobpgsrotbxnny";
            string smtpHost = "smtp.gmail.com";
            int smtpPort = 587;

            // Create a new MailMessage
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(senderEmail);
            mail.To.Add(email);
            mail.Subject = "Account Created";
            mail.Body = "Your account has been successfully created: " + email + "\n\n" + "Your password: " + password;

            SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

            // Send the email
            smtpClient.Send(mail);
        }
        private bool CheckUserExistsemail (string connectionString,string email, string dlNumber)
        {
            string query2 = "SELECT COUNT(*) FROM users WHERE users_email = @Email OR users_DL = @DLNum";

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
        private bool CheckUserExists(string connectionString, string firstName, string lastName)
        {
            string query = "SELECT COUNT(*) FROM users WHERE users_first_name = @FirstName AND users_last_name = @LastName";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
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
  
      
        public class Vehicle
        {
            public int VehicleID { get; set; }
            public string VehicleInfo { get; set; }

            public Vehicle(int vehicleID, string vehicleInfo)
            {
                VehicleID = vehicleID;
                VehicleInfo = vehicleInfo;
            }
        }