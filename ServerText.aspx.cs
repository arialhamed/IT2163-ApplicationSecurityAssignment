using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IT2163_ApplicationSecurityAssignment
{
    public partial class ServerText : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                lbl_head.Text = Request.QueryString["head"];
                lbl_body.Text = Request.QueryString["body"];
            }
        }
    }
}