using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using MySql.Data.MySqlClient;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Xml.Linq;
using System.Collections;
using System.Runtime.InteropServices.ComTypes;

namespace HoildaysCalcApp
{
    public partial class EmpReport : System.Web.UI.Page
    {

        private void PopulateEmployeeNames()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Select employee names from the database
                    string selectQuery = "SELECT emp_name FROM Employee";
                    using (MySqlCommand cmd = new MySqlCommand(selectQuery, connection))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            EmpNameDropDown.DataSource = reader;
                            EmpNameDropDown.DataTextField = "emp_name";
                            EmpNameDropDown.DataValueField = "emp_name";
                            EmpNameDropDown.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabelEmpVac.Text = "Error: " + ex.Message;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateEmployeeNames();
            }
        }

        protected void gvEmpReport_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void gvEmpReport_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void gvEmpReport_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }

        protected void gvEmpReport_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }

        protected void EmpNameDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedName = EmpNameDropDown.SelectedItem.Text;
            WelcomeEmp.Text = $"<p>This is the annual report of the employee named \"{selectedName}\"</p>";
        }
    }
}