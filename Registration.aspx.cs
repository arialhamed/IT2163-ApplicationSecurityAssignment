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
using System.Net.Mail;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace IT2163_ApplicationSecurityAssignment
{
    public partial class Registration : System.Web.UI.Page
    {
        string ASDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ASDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;

        private readonly Random rnd = new Random();
        static string verify_email;
        
        static string line = "\r";

        public class CaptchaSuccessObject
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            verify_email = rnd.Next(100000, 999999).ToString();

        }
        private bool checkPassword(string password)
        {
            bool[] score = new bool[5];

            string status = "";/*
            if (!scores[0]) { status += "Password must be at least 12 characters long.<br/>"; maxScoreFalse = true; }
            if (!scores[1]) { status += "Password must contain lowercase characters.<br/>"; maxScoreFalse = true; }
            if (!scores[2]) { status += "Password must contain uppercase characters.<br/>"; maxScoreFalse = true; }
            if (!scores[3]) { status += "Password must contain at least 1 number.<br/>"; maxScoreFalse = true; }
            if (!scores[4]) { status += "Password must contain special characters.<br/>"; maxScoreFalse = true; }*/



            if (password.Length < 12) { status += "Password must be at least 12 characters long.<br/>"; }
            if (!Regex.IsMatch(password, "[a-z]")) { status += "Password must contain lowercase characters.<br/>"; }
            if (!Regex.IsMatch(password, "[A-Z]")) { status += "Password must contain uppercase characters.<br/>"; }
            if (!Regex.IsMatch(password, "[0-9]")) { status += "Password must contain at least 1 number.<br/>"; }
            if (!Regex.IsMatch(password, "[\x21-\x2F\x3A-\x40\x5B-\x60\x7B-\x7E]")) { status += "Password must contain special characters.<br/>"; }

            if (status != "")
            {
                lbl_pwdchecker.Text = status;
                lbl_pwdchecker.ForeColor = Color.Red;
                return false;
            }
            else
            {
                lbl_pwdchecker.Text = "All set! Password is strong.";
                lbl_pwdchecker.ForeColor = Color.Green;
                return true;
            }

            //return status;
        }
        protected void btn_register_Click(object sender, EventArgs e)
        {
            if (!checkPassword(tb_password.Text.ToString()))
            {
                return;
            }
            string pwd = tb_password.Text.ToString().Trim(); ;

            if (ValidateCaptcha())
            {
                //Generate random "salt" 
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] saltByte = new byte[8];

                //Fills array of bytes with a cryptographically strong sequence of random values.
                rng.GetBytes(saltByte);
                salt = Convert.ToBase64String(saltByte);

                SHA512Managed hashing = new SHA512Managed();

                string pwdWithSalt = pwd + salt;
                byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
                byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));

                finalHash = Convert.ToBase64String(hashWithSalt);

                RijndaelManaged cipher = new RijndaelManaged();
                cipher.GenerateKey();
                Key = cipher.Key;
                IV = cipher.IV;

                var headString = "";
                var bodyString = "";
                if (createAccount())
                {
                    Response.Redirect(string.Format("ServerText.aspx?head={0}&body={1}", "Success!", "A link will be sent to your email, click on it to confirm your email. You may also close this tab."));
                }
                else
                {

                }
            }
        }


        protected bool createAccount()
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ASDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Accounts VALUES(@Email, @FirstName, @LastName, @Mobile, @Nric, @PasswordHash, @PasswordSalt, @DateTimeRegistered, @MobileVerified, @EmailVerified, @IV, @Key, @DOB, @CardNumber, @CardCV, @CardExpiry, @ProfileURL, @Lockout, @LockoutRecoveryDateTime)"))
                    //using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@Email, @Mobile,@Nric,@PasswordHash,@PasswordSalt,@DateTimeRegistered,@MobileVerified,@EmailVerified)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = tb_email.Text.Trim();
                            cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = tb_firstName.Text.Trim();
                            cmd.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = tb_lastName.Text.Trim();
                            cmd.Parameters.Add("@Mobile", SqlDbType.NVarChar).Value = tb_mobile.Text.Trim();
                            cmd.Parameters.Add("@Nric", SqlDbType.NVarChar).Value = Convert.ToBase64String(encryptData(tb_nric.Text.Trim()));
                            cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar).Value = finalHash;
                            cmd.Parameters.Add("@PasswordSalt", SqlDbType.NVarChar).Value = salt;
                            cmd.Parameters.Add("@DateTimeRegistered", SqlDbType.DateTime2).Value = DateTime.Parse(DateTime.Now.ToString().Trim());
                            if (verify_email == "VERIFIED")
                            {
                                cmd.Parameters.Add("@MobileVerified", SqlDbType.NChar).Value = "1";
                            } else
                            {
                                cmd.Parameters.Add("@MobileVerified", SqlDbType.NChar).Value = DBNull.Value;
                            }
                            cmd.Parameters.Add("@EmailVerified", SqlDbType.NChar).Value = DBNull.Value;
                            cmd.Parameters.Add("@IV", SqlDbType.NVarChar).Value = Convert.ToBase64String(IV);
                            cmd.Parameters.Add("@Key", SqlDbType.NVarChar).Value = Convert.ToBase64String(Key);
                            cmd.Parameters.Add("@DOB", SqlDbType.NVarChar).Value = tb_dob.Text.Trim();
                            cmd.Parameters.Add("@CardNumber", SqlDbType.NVarChar).Value = Convert.ToBase64String(encryptData(tb_cardnumber.Text.Trim()));
                            cmd.Parameters.Add("@CardCV", SqlDbType.NVarChar).Value = Convert.ToBase64String(encryptData(tb_cardcv.Text.Trim()));
                            cmd.Parameters.Add("@CardExpiry", SqlDbType.NVarChar).Value = Convert.ToBase64String(encryptData(tb_cardexpiry.Text.Trim()));
                            // Triggers saving of file to directory
                            cmd.Parameters.Add("@ProfileURL", SqlDbType.NVarChar).Value = uploadFile();
                            cmd.Parameters.Add("@Lockout", SqlDbType.NVarChar).Value = DBNull.Value;
                            cmd.Parameters.Add("@LockoutRecoveryDateTime", SqlDbType.NVarChar).Value = DBNull.Value;

                            cmd.Connection = con;
                            /*con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();*/

                            try
                            {
                                con.Open();
                                cmd.ExecuteNonQuery();
                                //con.Close();
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.ToString());
                                //lb_error1.Text = ex.ToString();
                            }
                            finally
                            {
                                con.Close();
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString()); // delete when not in production
                return false;
            }
        }

        protected string uploadFile()
        {
            string strFileName;
            string strFilePath = "";
            string strFolder;
            strFolder = Server.MapPath("./static/" + tb_email.Text.ToString().Trim() + "/");
            // Get the name of the file that is posted.
            strFileName = oFile.PostedFile.FileName;
            strFileName = Path.GetFileName(strFileName);
            if (oFile.Value != "")
            {
                // Create the directory if it does not exist.
                if (!Directory.Exists(strFolder))
                {
                    Directory.CreateDirectory(strFolder);
                }
                // Save the uploaded file to the server.
                strFilePath = strFolder + strFileName;
                if (File.Exists(strFilePath))
                {
                    lbl_photo_error.Text = strFileName + " already exists on the server!";
                }
                else
                {
                    oFile.PostedFile.SaveAs(strFilePath);
                    lbl_photo_error.Text = strFileName + " has been successfully uploaded.";
                }
            }
            else
            {
                //lblUploadResult.Text = "Click 'Browse' to select the file to upload.";
            }
            return "https://" + Request.Url.Authority + "/static/"+ tb_email.Text.ToString().Trim() + "/" + strFileName;
        }

        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                //ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);


                //Encrypt
                //cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
                //cipherString = Convert.ToBase64String(cipherText);
                //Console.WriteLine("Encrypted Text: " + cipherString);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { }
            return cipherText;
        }
        protected bool ValidateCaptcha()
        {
            bool result = true;
            string captchaResponse = Request.Form["g-recaptcha-response"];
            // Key obtainable from https://docs.google.com/document/d/1NRtJtsKDAIhXXeJESxyMo1fGHl-ljPmdHpIF9k3Pe4I/edit
            // the document is set to private, so you need arifhamed to grant access 
            // Take the string of text and add it into SECRET_KEY below
            string SECRET_KEY = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=" + SECRET_KEY + "&response=" + captchaResponse);
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
            } catch (WebException ex)
            {
                throw ex;
            }
        }
        protected bool ValidateEmail(string inemail)
        {
            bool result = true;
            string checkaddress;
            try
            {
                checkaddress = new MailAddress(inemail).Address;
            } catch (FormatException)
            {
                result = false;
            }
            return result;
        }
        // send the verify_email
        protected void btn_send_verify_Click(object sender, EventArgs e)
        {
            /*if (ValidateEmail(tb_email.Text.ToString()))
            {
                string to = tb_email.Text.ToString(); //To address    
                string from = "morphball420@gmail.com"; //From address    
                MailMessage message = new MailMessage(from, to);

                string mailbody = "Verification code: " + verify_email + "<br><br>Use this code in the Registration page.";
                message.Subject = "SITConnect Registration: Email verification code";
                message.Body = mailbody;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
*//*                System.Net.NetworkCredential basicCredential1 = new
                System.Net.NetworkCredential(from, "keueryknpfjhhdde");*//*
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(from, "keueryknpfjhhdde");
                try
                {
                    client.Send(message);
                    tb_verify_email.Visible = true;
                    btn_verify_email.Visible = true;
                }

                catch (Exception ex)
                {
                    
                    throw ex;
                }

            } else
            {

            }*/
            string inEmail = tb_email.Text.ToString().Trim();
            if (ValidateEmail(inEmail))
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(inEmail);
                //mail.To.Add("Another Email ID where you wanna send same email");
                mail.From = new MailAddress("stotlefake@gmail.com");
                mail.Subject = "SITConnect: Email Verification";

                string Body = "You have requested to verify your email" +
                              "Verification Code: " + verify_email;
                mail.Body = Body;

                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address
                smtp.Credentials = new System.Net.NetworkCredential
                     ("stotlefake@gmail.com", "iymhgkzhczaawstr"); // password is based off App Passwords from accounts.google.com security
                smtp.EnableSsl = true;
                try
                {
                    smtp.Send(mail);
                    tb_verify_email.Visible = true;
                    btn_verify_email.Visible = true;
                    //lbl_verify_email.Text = "";
                } catch (Exception ex)
                {
                    lbl_verify_email.Text = "Error occured when sending verification email. You may verify your email after creating account";
                }
                // this button would not work for the time being
            }
        }

        // check if both the user type in and the verify_email are the same
        protected void btn_verify_email_Click(object sender, EventArgs e)
        {
            if (tb_verify_email.Text.ToString().Trim() == verify_email)
            {
                btn_send_verify.Visible = false;
                tb_verify_email.Visible = false;
                btn_verify_email.Visible = false;
                lbl_verify_email.Text = "Email has been verified!";
                verify_email = "VERIFIED";
            }
        }
    }
}