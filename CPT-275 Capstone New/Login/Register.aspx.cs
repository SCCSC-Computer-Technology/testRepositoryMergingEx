using Microsoft.AspNet.FriendlyUrls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebGrease.Activities;
using CPT_275_Capstone.App_Start;
using System.Diagnostics.Tracing;

namespace CPT_275_Capstone.Login
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Login/Login.aspx");
        }
        protected void RegisterButton_Click(object sender, EventArgs e)
        {
            try
            {
                string password = Password.Text;
                //If condition to check if the passwords match
                if (ConfirmPassword.Text != password)
                {
                    lblErrorMessage.Text = "Passwords do not match";
                    return;
                }
                //Checking to see if the password meets the requirements
                if  (!IsPasswordValid(password))
                {
                    lblErrorMessage.Text = "Password must contain an uppercase letter, a lowercase letter, a number, a special character, and be at least 8 characters long.";
                    return;
                }
                // Code to handle the Register button click event
                string connectionString = ConfigurationManager.ConnectionStrings["FleetProd"].ConnectionString;
                string selectQuery = "SELECT COUNT(*) FROM Users WHERE users_email = @Email OR users_DL = @DriverLicenseNumber";
                string insertQuery = "INSERT INTO users (users_last_name,users_first_name,users_telephone,users_email,users_password,users_DL,users_DL_state,users_type) " +
                    "VALUES (@LastName,@FirstName,@PhoneNumber,@Email,@Password,@DriverLicenseNumber,@DriverLicenseState,@UserType)";
                

                // Perform the database query to insert the new user
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Check if the account already exists
                    SqlCommand selectCommand = new SqlCommand(selectQuery, connection);
                    selectCommand.Parameters.AddWithValue("@Email", Email.Text);
                    selectCommand.Parameters.AddWithValue("@DriverLicenseNumber", DLNumber.Text); // Update parameter name here

                    connection.Open();
                    int existingUserCount = (int)selectCommand.ExecuteScalar();

                    if (existingUserCount > 0)
                    {
                        lblErrorMessage.Text = "An account with the given email or driver's license already exists!";
                        return; // Prevents the rest of the code from executing
                    }

                    connection.Close();

                    // Validate the phone number
                    string phoneNumber = Phone.Text;
                    phoneNumber = new string(phoneNumber.Where(char.IsDigit).ToArray()); // Remove non-digit characters

                    if (phoneNumber.Length != 10)
                    {
                        lblErrorMessage.Text = "Phone number should be 10 digits long.";
                        return;
                    }
                    // Create the new user
                    SqlCommand insertcommand = new SqlCommand(insertQuery, connection);
                    insertcommand.Parameters.AddWithValue("@Password", Password.Text);
                    insertcommand.Parameters.AddWithValue("@Email", Email.Text);
                    insertcommand.Parameters.AddWithValue("@FirstName", FirstName.Text);
                    insertcommand.Parameters.AddWithValue("@LastName", LastName.Text);
                    insertcommand.Parameters.AddWithValue("@DriverLicenseNumber", DLNumber.Text);
                    insertcommand.Parameters.AddWithValue("@DriverLicenseState", DLState.Text);
                    insertcommand.Parameters.AddWithValue("@PhoneNumber", Phone.Text);
                    insertcommand.Parameters.AddWithValue("@UserType", "Basic"); // Set the user type as "Basic"

                    //Execute the commands
                    connection.Open();
                    insertcommand.ExecuteNonQuery();
                    connection.Close();//Close the connection
                    SendEmail(Email.Text, Password.Text);
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowAccountCreated", "ShowAccountCreated();", true);
                }

                // Redirect the user to a success page or perform any other desired actions
                Response.Redirect("~/Login/Login.aspx");
            }
            catch (Exception ex)
            {
                string eventSource = "SCCFleetServices";
                string errorMessage = ex.Message;
                EventViewerLogger logger = new EventViewerLogger(eventSource);
                logger.LogError(errorMessage);
                lblErrorMessage.Text = "An error occurred while creating the account. Please try again later.";
                lblErrorMessage.ForeColor = System.Drawing.Color.Red;
            }
        }
        private bool IsPasswordValid(string password)
        {
            Regex regex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^a-zA-Z\\d\\s:]).{8,}$");
            return regex.IsMatch(password);
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
            mail.Body = "Your account has been successfully created: " + email + "\n\n"+ "Your password: " + password;

            SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

            // Send the email
            smtpClient.Send(mail);
        }
    }
}
    
