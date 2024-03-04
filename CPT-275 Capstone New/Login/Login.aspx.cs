using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CPT_275_Capstone.Login
{
    public partial class Login : System.Web.UI.Page
    {
        protected void btnlogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Text;

            // Validate user credentials against database
            if (ValidateUser(email, password))
            {
                string userType = GetUserType(email);
                if (userType == "Admin               ")
                {
                    Session["UserType"] = "Admin               ";
                    Response.Redirect("~/Default.aspx");
                }
                else if (userType == "Basic               ") 
                {
                    Session["UserType"] = "Basic               ";
                    Session["LastName"] = GetLastName(email);
                    Session["FirstNameInitial"] = GetFirstNameInitial(email);
                    Response.Redirect("~/UserScreen/User Screen.aspx");
                }
                else if (userType == "Power               ")
                {
                    Session["UserType"] = "Power               ";
                    Session["FirstName"] = GetLastName(email);
                    Session["LastNameInitial"] = GetFirstNameInitial(email);
                    Response.Redirect("~/UserScreen/User Screen.aspx");
                }
                else
                {
                    // Handle unknown user types
                    string errorMessage = "Invalid user type";
                    ClientScript.RegisterStartupScript(this.GetType(), "showError", $"showErrorMessage('{errorMessage}');", true);
                }
            }
            else
            {
                string errorMessage = "Invalid email or password";
                ClientScript.RegisterStartupScript(this.GetType(), "showError", $"showErrorMessage('{errorMessage}');", true);
            }
        }

        private string GetLastName(string email)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["FleetProd"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT users_last_name FROM users WHERE users_email = @Email";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);
                return command.ExecuteScalar()?.ToString();
            }
        }

        private string GetFirstNameInitial(string email)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["FleetProd"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT LEFT(users_first_name, 1) FROM users WHERE users_email = @Email";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);
                return command.ExecuteScalar()?.ToString();
            }
        }

        private bool ValidateUser(string email, string password)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["FleetProd"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM users WHERE users_email = @Email AND users_password = @Password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password);
                int count = (int)command.ExecuteScalar();

                if (count > 0)
                {
                    // User exists, retrieve first name and last name initial
                    string userDetailsQuery = "SELECT users_first_name, users_last_name FROM users WHERE users_email = @Email";
                    SqlCommand userDetailsCommand = new SqlCommand(userDetailsQuery, connection);
                    userDetailsCommand.Parameters.AddWithValue("@Email", email);
                    using (SqlDataReader reader = userDetailsCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string firstName = reader.GetString(reader.GetOrdinal("users_first_name"));
                            string lastName = reader.GetString(reader.GetOrdinal("users_last_name"));
                            Session["LastName"] = lastName;
                            Session["FirstNameInitial"] = firstName.Substring(0, 1);
                        }
                    }

                    return true;
                }

                return false;
            }
        }

        private string GetUserType(string email)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["FleetProd"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT users_type FROM users WHERE users_email = @Email";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string userType = reader.GetString(reader.GetOrdinal("users_type"));
                        return userType;
                    }
                }
            }
            return null;
        }

        protected void btnForgotPassword_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Login/ForgotPassword.aspx");
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Login/Register.aspx");
        }
    }
}