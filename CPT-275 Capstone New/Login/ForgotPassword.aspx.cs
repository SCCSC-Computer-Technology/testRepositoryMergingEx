using CPT_275_Capstone.App_Start;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CPT_275_Capstone.Login
{
    public partial class ForgotPassword : System.Web.UI.Page
    {

        protected void btnreset_Click(object sender, EventArgs e)
        {
            string email = txtemail.Text.Trim();
            string license = txtdl.Text.Trim();
            string newPassword = txtnewpassword.Text.Trim();
            string confirmPassword = txtconfirmpassword.Text.Trim();

            // Check if the new password and confirm password match
            if (newPassword == confirmPassword)
            {
                if (isPasswordValid(newPassword))
                {
                    // Get the connection string from the web.config file
                    string connectionString = ConfigurationManager.ConnectionStrings["FleetProd"].ConnectionString;

                    try
                    {
                        // Create a new SqlConnection and SqlCommand
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();

                            // Check if the user exists by matching email and driver's license
                            using (SqlCommand selectCmd = new SqlCommand("SELECT * FROM users WHERE users_email = @Email AND users_DL = @License", conn))
                            {
                                selectCmd.Parameters.AddWithValue("@Email", email);
                                selectCmd.Parameters.AddWithValue("@License", license);

                                using (SqlDataReader reader = selectCmd.ExecuteReader())
                                {
                                    if (reader.HasRows)
                                    {
                                        reader.Close();

                                        // Update the password in the database
                                        using (SqlCommand updateCmd = new SqlCommand("UPDATE users SET users_password = @NewPassword WHERE users_email = @Email", conn))
                                        {
                                            updateCmd.Parameters.AddWithValue("@NewPassword", newPassword);
                                            updateCmd.Parameters.AddWithValue("@Email", email);
                                            updateCmd.ExecuteNonQuery();
                                        }

                                        // Send an email to the user with the new password
                                        SendEmail(email, newPassword);

                                        // Display a success message
                                        ScriptManager.RegisterStartupScript(this, GetType(), "ShowEmailSentMessage", "ShowEmailSentMessage();", true);
                                    }
                                    else
                                    {
                                        // Credentials do not match, display an error message
                                        lblerrormessage.Text = "Invalid email or driver license.";
                                        lblerrormessage.Visible = true;
                                    }
                                }
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        // Display an error message
                        string eventSource = "SCCFleetServices";
                        string errorMessage = "Forgot Password screen error message: " + ex.Message;
                        EventViewerLogger logger = new EventViewerLogger(eventSource);
                        logger.LogError(errorMessage);
                        lblerrormessage.Text = "An error occurred. Please try again later.";
                        lblerrormessage.Visible = true;
                    }
                }
                else
                {
                    // Display an error message if the new password does not meet the requirements
                    lblerrormessage.Text = "New password must contain an uppercase letter, a lowercase letter, a number, a special character, and be at least 8 characters long.";
                    lblerrormessage.Visible = true;
                }
            }
            else
            {
                // Display an error message if the new password and confirm password do not match
                lblerrormessage.Text = "New password and confirm password do not match.";
                lblerrormessage.Visible = true;
            }
            }
        private bool isPasswordValid(string password)
        {
            // Password requirements: uppercase letter, lowercase letter, number, special character, minimum length of 8 characters
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
            mail.Subject = "Password Change Notification";
            mail.Body = "Your password has been changed successfully."+ "\n\n" + "Your new password is: " + password;

            SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

            // Send the email
            smtpClient.Send(mail);
        }
    }
}