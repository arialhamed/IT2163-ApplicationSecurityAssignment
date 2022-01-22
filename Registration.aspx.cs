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

namespace IT2163_ApplicationSecurityAssignment
{
    public partial class Registration : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;

        static string line = "\r";
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



            if (password.Length >= 12) { score[0] = true; status += "Password must be at least 12 characters long.<br/>"; }
            if (Regex.IsMatch(password, "[a-z]")) { score[1] = true; status += "Password must contain lowercase characters.<br/>"; }
            if (Regex.IsMatch(password, "[A-Z]")) { score[2] = true; status += "Password must contain uppercase characters.<br/>"; }
            if (Regex.IsMatch(password, "[0-9]")) { score[3] = true; status += "Password must contain at least 1 number.<br/>"; }
            if (Regex.IsMatch(password, "[\x21-\x2F\x3A-\x40\x5B-\x60\x7B-\x7E]")) { score[4] = true; status += "Password must contain special characters.<br/>"; }

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
            if (!checkPassword(tb_password.Text))
            {
                return;
            }
            string pwd = tb_password.Text.ToString().Trim(); ;
            

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

            /*lb_error1.Text = "Salt:" + salt;
            lb_error2.Text = "Hash with salt:" + finalHash;
*/
            RijndaelManaged cipher = new RijndaelManaged();
            cipher.GenerateKey();
            Key = cipher.Key;
            IV = cipher.IV;


            createAccount();
        }


        protected void createAccount()
        {

            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@Email, @Mobile,@Nric,@PasswordHash,@PasswordSalt,@DateTimeRegistered,@MobileVerified,@EmailVerified,@IV,@Key)"))
                    //using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@Email, @Mobile,@Nric,@PasswordHash,@PasswordSalt,@DateTimeRegistered,@MobileVerified,@EmailVerified)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Email", tb_email.Text.Trim());
                            cmd.Parameters.AddWithValue("@FirstName", tb_firstName);
                            cmd.Parameters.AddWithValue("@LastName", tb_lastName);
                            cmd.Parameters.AddWithValue("@Mobile", tb_mobile.Text.Trim());
                            cmd.Parameters.AddWithValue("@Nric", Convert.ToBase64String(encryptData(tb_nric.Text.Trim())));
                            cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                            cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                            cmd.Parameters.AddWithValue("@DateTimeRegistered", DateTime.Now);
                            cmd.Parameters.AddWithValue("@MobileVerified", DBNull.Value);
                            cmd.Parameters.AddWithValue("@EmailVerified", DBNull.Value);
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                            cmd.Parameters.AddWithValue("@DOB", tb_dob.Text.Trim());
                            cmd.Parameters.AddWithValue("@CardNumber", Convert.ToBase64String(encryptData(tb_cardnumber.Text.Trim())));
                            cmd.Parameters.AddWithValue("@CardCV", Convert.ToBase64String(encryptData(tb_cardcv.Text.Trim())));
                            cmd.Parameters.AddWithValue("@CardExpiry", Convert.ToBase64String(encryptData(tb_cardexpiry.Text.Trim())));
                            //cmd.Parameters.AddWithValue(); // profile url

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


            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
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
    }
}