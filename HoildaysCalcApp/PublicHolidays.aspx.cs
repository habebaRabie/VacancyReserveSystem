using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;

namespace HoildaysCalcApp
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public class Holidays
        {
            public DateTime HolidayDate { get; set; }
            public string HolidayDesc { get; set; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DataBindToGrid();
            }
        }

        public void DataBindToGrid()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sqlQuery = "SELECT holiday_date, holiday_desc FROM publicholidays";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(sqlQuery, connection))
                    {
                        DataSet dt = new DataSet();
                        adapter.Fill(dt);

                        if (dt.Tables[0].Rows.Count > 0)
                        {
                            gvHolidays.DataSource = dt;
                            gvHolidays.DataBind();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ltError.Text = "Error: " + ex.Message;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        protected void gvHolidays_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            ltError.Text = string.Empty;
            GridViewRow gvRow = gvHolidays.Rows[e.RowIndex];

            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Get the date value from the GridView and format it
                    string holidayDate = gvRow.Cells[1].Text;
                    DateTime dateValue = DateTime.Parse(holidayDate);
                    string formattedDate = dateValue.ToString("yyyy-MM-dd");

                    // Parameterized SQL query to delete based on the formatted date
                    string sqlQuery = "DELETE FROM publicholidays WHERE holiday_date = @HolidayDate";

                    MySqlCommand command = new MySqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@HolidayDate", formattedDate);

                    command.ExecuteNonQuery();
                    gvHolidays.EditIndex = -1;
                    DataBindToGrid();
                }
                catch (Exception ex)
                {
                    ltError.Text = "Error: " + ex.Message;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        protected List<DateTime> GetPublicHolidays()
        {
            List<DateTime> retrievedHolidays = new List<DateTime>();
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Select all holiday dates from the database
                    string selectQuery = "SELECT holiday_date FROM publicholidays";
                    using (MySqlCommand cmd = new MySqlCommand(selectQuery, connection))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DateTime holidayDate = reader.GetDateTime("holiday_date");
                                retrievedHolidays.Add(holidayDate);
                            }
                        }
                    }
                }

                return retrievedHolidays;
            }
            catch (Exception ex)
            {
                ltError.Text = "Error: " + ex.Message;
                return null;
            }
        }

        private void AddSeveralPublicHolidays(DateTime startDate, DateTime endDate, MySqlConnection connection, string txtNewHolidayDesc)
        {
            List<DateTime> publicHolidays = GetPublicHolidays();

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                // Check if the current day is not a public holiday
                if (!publicHolidays.Contains(date))
                {
                    string query = "INSERT INTO publicholidays (holiday_date, holiday_desc) VALUES (@holidayDate, @holidayDesc)";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@holidayDate", date);
                        cmd.Parameters.AddWithValue("@holidayDesc", txtNewHolidayDesc);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            ltError.Text = "Public Holidays have been added successfully ";
        }

        protected void btnInsertNewHoliday_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    DateTime newHolidayStartDate1 = newHolidayStartDate.SelectedDate;
/*                    newHolidayEndDate.SelectedDate = newHolidayStartDate1.Date;
*/                    DateTime newHolidayEndDate1 = newHolidayEndDate.SelectedDate;

                    if (DateTime.TryParse(newHolidayStartDate1.ToString(), out DateTime holidayStartDate) && DateTime.TryParse(newHolidayEndDate1.ToString(), out DateTime holidayEndDate) && !string.IsNullOrEmpty(txtNewHolidayDesc.Text))
                    {
                        AddSeveralPublicHolidays(holidayStartDate, holidayEndDate, connection, txtNewHolidayDesc.Text);
                        DataBindToGrid();
                        newHolidayStartDate.SelectedDate = DateTime.Now;
                        newHolidayEndDate.SelectedDate = DateTime.Now;
                        txtNewHolidayDesc.Text = "";
                    }
                    else
                    {
                        ltError.Text = "Please enter valid date and description";
                    }

                }
                catch (Exception ex)
                {
                    ltError.Text = "Error: " + ex.Message;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }


        protected void gvHolidays_RowEditing(object sender, GridViewEditEventArgs e)
        {
            ltError.Text = string.Empty;
            gvHolidays.EditIndex = e.NewEditIndex;
            DataBindToGrid();
        }

        protected void gvHolidays_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            ltError.Text = string.Empty;
            GridViewRow gvRow = gvHolidays.Rows[e.RowIndex];

            // Get the updated values from the GridView's TextBox controls
            string updatedHolidayDesc = ((TextBox)gvRow.Cells[0].Controls[0]).Text;
            string originalTime = ((TextBox)gvRow.Cells[1].Controls[0]).Text;

            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    DateTime formattedTime = DateTime.Parse(originalTime);

                    // Use parameterized query to update the record
                    string sqlQuery = "UPDATE publicholidays SET holiday_desc = @HolidayDesc WHERE holiday_date = @FormattedDate";
                    MySqlCommand command = new MySqlCommand(sqlQuery, connection);

                    // Add parameters
                    command.Parameters.AddWithValue("@HolidayDesc", updatedHolidayDesc);
                    command.Parameters.AddWithValue("@FormattedDate", formattedTime);

                    command.ExecuteNonQuery();
                    gvHolidays.EditIndex = -1;
                    DataBindToGrid();
                }
                catch (Exception ex)
                {
                    ltError.Text = "Error: " + ex.Message;
                }
                finally
                {
                    connection.Close();
                }
            }
        }


        protected void gvHolidays_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvHolidays.EditIndex = -1;
            DataBindToGrid();
        }
    }
}