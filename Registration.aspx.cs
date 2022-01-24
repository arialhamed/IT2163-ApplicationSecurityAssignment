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
    public partial class Registration : System.Web.UI.Page
    {
        string ASDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ASDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;

        static string line = "\r";

        public class CaptchaSuccessObject
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

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
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Accounts VALUES(@Email, @FirstName, @LastName, @Mobile, @Nric, @PasswordHash, @PasswordSalt, @DateTimeRegistered, @MobileVerified, @EmailVerified, @IV, @Key, @DOB, @CardNumber, @CardCV, @CardExpiry, @ProfileURL)"))
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
                            cmd.Parameters.Add("@MobileVerified", SqlDbType.NChar).Value = DBNull.Value;
                            cmd.Parameters.Add("@EmailVerified", SqlDbType.NChar).Value = DBNull.Value;
                            cmd.Parameters.Add("@IV", SqlDbType.NVarChar).Value = Convert.ToBase64String(IV);
                            cmd.Parameters.Add("@Key", SqlDbType.NVarChar).Value = Convert.ToBase64String(Key);
                            cmd.Parameters.Add("@DOB", SqlDbType.NVarChar).Value = tb_dob.Text.Trim();
                            cmd.Parameters.Add("@CardNumber", SqlDbType.NVarChar).Value = Convert.ToBase64String(encryptData(tb_cardnumber.Text.Trim()));
                            cmd.Parameters.Add("@CardCV", SqlDbType.NVarChar).Value = Convert.ToBase64String(encryptData(tb_cardcv.Text.Trim()));
                            cmd.Parameters.Add("@CardExpiry", SqlDbType.NVarChar).Value = Convert.ToBase64String(encryptData(tb_cardexpiry.Text.Trim()));
                            cmd.Parameters.Add("@ProfileURL", SqlDbType.NVarChar).Value = "";

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
            } catch (WebException ex)
            {
                throw ex;
            }
        }
    }
}