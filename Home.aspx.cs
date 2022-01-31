using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data.SqlClient;
using System.Data;

namespace IT2163_ApplicationSecurityAssignment
{
    public partial class Home : System.Web.UI.Page
    {
        string ASDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ASDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                } else
                {
                    lbl_body1.Text = "You are signed in as " + Session["LoggedIn"];
                    lbl_body1.ForeColor = Color.Green;
                    btn_logout.Visible = true;
                    // to get photo url
                    try
                    {
                        using (SqlConnection con = new SqlConnection(ASDBConnectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("SELECT ProfileURL FROM Accounts WHERE Email = @Email"))
                            {
                                using (SqlDataAdapter sda = new SqlDataAdapter())
                                {
                                    cmd.CommandType = CommandType.Text;
                                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = Session["LoggedIn"];
                                    cmd.Connection = con;
                                    con.Open();
                                    SqlDataReader nwReader = cmd.ExecuteReader();
                                    try
                                    {
                                        while (nwReader.Read())
                                        {
                                            // Somehow, this works
                                            showPhoto.ImageUrl = (string)nwReader["ProfileURL"];
                                        }
                                    } catch (Exception ex)
                                    {
                                        // heh
                                    }
                                    finally
                                    {
                                        nwReader.Close();
                                        con.Close();
                                    }
                                }
                            }
                        }
                    } catch (Exception ex)
                    {

                    }
                }
            } else
            {
                Response.Redirect("Login.aspx", false);
            }
        }

        protected void btn_logout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            Response.Redirect("Login.aspx", false);

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            } else if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }
        }
    }
}