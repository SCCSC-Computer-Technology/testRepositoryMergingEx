using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Mail; // This is the namespace for the MailMessage class
using System.Net.Security;


namespace CPT_275_Capstone.Sender_Email
{
    public partial class Email_Sender : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnemail_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtemail.Text))
                {
                    MailMessage msg = new MailMessage();
                    msg.To.Add(txtemail.Text);
                    msg.From = new MailAddress("maksbotukh@gmail.com");
                    msg.Subject = "Subject";
                    msg.Body = txtmessage.Text;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.Credentials = new System.Net.NetworkCredential("maksbotukh@gmail.com", "pkgobpgsrotbxnny");
                    smtp.EnableSsl = true;
                    smtp.Send(msg);
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowEmailSentMessage", "ShowEmailSentMessage();", true);
                }
                else
                {
                    lblconfirmemail.Text = "Please enter an email address";
                }
            }
            catch (Exception ex)
            {
                lblconfirmemail.Text = "The email was not send due to the following error" + ex.Message;
            }
        }
    }
}