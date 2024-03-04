using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CPT_275_Capstone
{
    public partial class WebForm1 : System.Web.UI.Page
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
                            drpcarsvehicle.Items.Add(item);
                        }
                        reader.Close();
                    }
                }
           
        }
    }
}