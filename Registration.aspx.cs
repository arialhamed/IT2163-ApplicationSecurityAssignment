using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text.RegularExpressions;
using System.Drawing;

namespace IT2163_ApplicationSecurityAssignment
{
    public partial class Registration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private bool[] checkPassword(string password)
        {
            bool[] score = new bool[5];
            /*// Score 0
            if (password.Length < 8)
            { return 1; } else
            { score++; } // Score 1
            // Score 2
            if (Regex.IsMatch(password, "[a-z]")) { score++; }
            // Score 3
            if (Regex.IsMatch(password, "[A-Z]")) { score++; }
            // Score 4
            if (Regex.IsMatch(password, "[0-9]")) { score++; }
            // Score 5
            if (Regex.IsMatch(password, "[^A-Za-z0-9]")) { score++; }*/
            if (password.Length >= 12) { score[0] = true; }
            if (Regex.IsMatch(password, "[a-z]")) { score[1] = true; }
            if (Regex.IsMatch(password, "[A-Z]")) { score[2] = true; }
            if (Regex.IsMatch(password, "[0-9]")) { score[3] = true; }
            if (Regex.IsMatch(password, "[\x21-\x2F\x3A-\x40\x5B-\x60\x7B-\x7E]")) { score[4] = true; }


            return score;
        }

        protected void btn_checkPassword_Click(object sender, EventArgs e)
        {
            bool[] scores = checkPassword(tb_password.Text);
            bool maxScoreFalse = false;
            string status = "";
            if (!scores[0]) { status += "Password must be at least 12 characters long.<br/>"; maxScoreFalse = true; }
            if (!scores[1]) { status += "Password must contain lowercase characters.<br/>"; maxScoreFalse = true; }
            if (!scores[2]) { status += "Password must contain uppercase characters.<br/>"; maxScoreFalse = true; }
            if (!scores[3]) { status += "Password must contain at least 1 number.<br/>"; maxScoreFalse = true; }
            if (!scores[4]) { status += "Password must contain special characters.<br/>"; maxScoreFalse = true; }

            if (maxScoreFalse) {
                lbl_pwdchecker.Text = status;
                lbl_pwdchecker.ForeColor = Color.Red; 
                
            } else
            {
                lbl_pwdchecker.Text = "All set! Password is strong.";
                lbl_pwdchecker.ForeColor = Color.Green;
            }

            return;
        }
    }
}