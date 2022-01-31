using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text.RegularExpressions;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace IT2163_ApplicationSecurityAssignment
{
    public partial class Login : System.Web.UI.Page
    {
        string ASDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ASDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;

        static int loginAttempts;

        static string line = "/r";

        public class CaptchaSuccessObject
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["Attempts"] = 0;
            loginAttempts = 0;
        }

        protected void btn_login_Click(object sender, EventArgs e)
        {
            if (ValidateCaptcha())
            {
                string email = tb_email.Text.ToString().Trim();
                string passw = tb_password.Text.ToString().Trim();

                SHA512Managed hashing = new SHA512Managed();
                string dbHash = getDBHash(email);
                string dbSalt = getDBSalt(email);

                try
                {
                    if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                    {
                        string pwdWithSalt = passw + dbSalt;
                        byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                        string userHash = Convert.ToBase64String(hashWithSalt);
                        
                        if (userHash.Equals(dbHash)){
                            Session["LoggedIn"] = tb_email.Text.Trim();

                            string guid = Guid.NewGuid().ToString();
                            Session["AuthToken"] = guid;

                            Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                            Response.Redirect("Home.aspx", false);
                        } else
                        {
                            lbl_error.Text = "Email or Password is not valid. Please try again.";
                            Response.Redirect("Login.aspx", false);
                        }
                    } else
                    {
                        if (loginAttempts <= 3)
                        {
                            loginAttempts += 1;
                        }
                        else
                        {
                            using (SqlConnection con = new SqlConnection(ASDBConnectionString))
                            {
                                using (SqlCommand cmd = new SqlCommand("UPDATE FROM Accounts SET Lockout = 1 WHERE Email = @Email"))
                                {
                                    using (SqlDataAdapter sda = new SqlDataAdapter())
                                    {
                                        cmd.CommandType = CommandType.Text;
                                        cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = tb_email.Text.Trim();
                                        cmd.Connection = con;

                                        try
                                        {
                                            con.Open();
                                            cmd.ExecuteNonQuery();

                                            // Line below should never run, it would show that that account exists
                                            //lbl_error.Text = "Account " + tb_email.Text.Trim() + " has been locked.";

                                            // Send notification to email that this email has been locked
                                            //EMAIL
                                        } catch (Exception ex)
                                        {
                                            //throw new Exception(ex.ToString());
                                            // not much to do here
                                        } finally
                                        {
                                            con.Close();

                                            // Reset value just in case they meant to log in to a different email
                                            loginAttempts = 0;
                                        }
                                    }
                                }
                            }
                        }
                    }
                } catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                } finally { }
                
                
                

            } else
            {
                lbl_error.Text = "Your Captcha is invalid, please try again";
            }
        }
        protected bool ValidateCaptcha()
        {
            bool result = true;
            string captchaResponse = Request.Form["g-recaptcha-response"];
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=6LfGcDIeAAAAAInUDDMPS3IAZPuUtnrsK-cyNtJy&response=" + captchaResponse);
            try
            {
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        CaptchaSuccessObject jsonObject = js.Deserialize<CaptchaSuccessObject>(jsonResponse);
                        result = Convert.ToBoolean(jsonObject.success);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        protected void btn_register_Click(object sender, EventArgs e)
        {
            Response.Redirect("Registration.aspx", false);
        }

        protected string getDBSalt(string email)
        {
            string s = null;
            SqlConnection connection = new SqlConnection(ASDBConnectionString);
            string sql = "select PASSWORDSALT FROM ACCOUNTS WHERE Email=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EMAIL", email);

            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PASSWORDSALT"] != null)
                        {
                            if (reader["PASSWORDSALT"] != DBNull.Value)
                            {
                                s = reader["PASSWORDSALT"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return s;
        }
        protected string getDBHash(string email)
        {
            string s = null;
            SqlConnection connection = new SqlConnection(ASDBConnectionString);
            string sql = "select PASSWORDHASH FROM ACCOUNTS WHERE Email=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EMAIL", email);

            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PASSWORDHASH"] != null)
                        {
                            if (reader["PASSWORDHASH"] != DBNull.Value)
                            {
                                s = reader["PASSWORDHASH"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return s;
        }
    }
}