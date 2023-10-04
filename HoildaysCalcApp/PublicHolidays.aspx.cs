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

        protected void btnInsertNewHoliday_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    DateTime newHolidayDate1 = newHolidayDate.SelectedDate;



                    // Make sure newHolidayDate is a valid DateTime and txtNewHolidayDesc is not null or empty
                    if (DateTime.TryParse(newHolidayDate1.ToString(), out DateTime holidayDate) && !string.IsNullOrEmpty(txtNewHolidayDesc.Text))
                    {
                        string sqlQuery = "INSERT INTO publicholidays (holiday_date, holiday_desc) VALUES (@holidayDate, @holidayDesc)";
                        MySqlCommand command = new MySqlCommand(sqlQuery, connection);

                        // Use the correct parameter names
                        command.Parameters.AddWithValue("@holidayDate", newHolidayDate1);
                        command.Parameters.AddWithValue("@holidayDesc", txtNewHolidayDesc.Text);
                        command.ExecuteNonQuery();

                        DataBindToGrid();
                        newHolidayDate.SelectedDate = DateTime.Now;
                        txtNewHolidayDesc.Text = "";
                    }
                    else
                    {
                        ltError.Text = newHolidayDate.ToString();
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