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

        protected void EmpNameDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedName = EmpNameDropDown.SelectedItem.Text;
            WelcomeEmp.Text = $"<p>This is the annual report of the employee named \"{selectedName}\"</p>";

            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string sqlQuery = "SELECT vac_name, start_date, end_date, holiday_days_number " +
                                      "FROM holidays " +
                                      "JOIN vacancytype ON holidays.vac_ID = vacancytype.vac_ID " +
                                      "JOIN employee ON employee.emp_ID = holidays.emp_ID " +
                                      "WHERE employee.emp_name=@EmpName";

                    MySqlCommand command = new MySqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@EmpName", selectedName);

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataSet dt = new DataSet();
                        adapter.Fill(dt);

                        if (dt.Tables.Count > 0 && dt.Tables[0].Rows.Count > 0)
                        {
                            gvEmpReport.DataSource = dt;
                            gvEmpReport.DataBind();
                            gvEmpReport.Visible = true;
                            GetHolidayDaysAndMaxDays(connection, selectedName);
                        }
                        else
                        {
                            // Handle case where no data is found for the selected employee.
                            EmpVacDaysRemain.Text = "This employee is a hardworker ! No vacancy days found! ";
                            gvEmpReport.DataSource = null;
                            gvEmpReport.DataBind();

                            gvEmpReport.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabelEmpVac.Text = "Error: " + ex.Message;
            }
        }

        private void GetHolidayDaysAndMaxDays(MySqlConnection connection, string employeeName)
        {
            string query = "SELECT COALESCE(SUM(holiday_days_number), 0) AS totalDays, emp_max_days " +
                           "FROM holidays " +
                           "JOIN employee ON holidays.emp_ID = employee.emp_ID " +
                           "WHERE vac_ID = 1 AND emp_name=@EmpName";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@EmpName", employeeName);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int totalHolidayDays = Convert.ToInt32(reader["totalDays"]);
                        int empMaxDays = Convert.ToInt32(reader["emp_max_days"]);
                        int remainingDays = empMaxDays - totalHolidayDays;

                        EmpVacDaysRemain.Text =
                            $"<p>The employee name \"{employeeName}\"</p>" +
                            $"<p>The taken days are \"{totalHolidayDays}\" days</p>" +
                            $"<p>From the available maximum number of days \"{empMaxDays}\" days</p>" +
                            $"<p>So the remaining number of days are \"{remainingDays}\" days</p> <br/>";
                    }
                }
            }
        }

        protected void ExportPDF_Click(object sender, EventArgs e)
        {
            // Create label content
            string labelContent = "Label content.";

            // Specify the file path where the PDF will be saved
            string filePath = Server.MapPath("~/App_Data/exported.pdf");

            // Check if the GridView has data source
            if (gvEmpReport.DataSource != null)
            {
                // Cast the data source of the GridView to a DataTable
                DataTable dataTable = gvEmpReport.DataSource as DataTable;

                // Generate the PDF and save it to the specified file path
                PdfGenerator.GeneratePdfFromDataSource(dataTable, labelContent, filePath);
            }
        }
    }
}