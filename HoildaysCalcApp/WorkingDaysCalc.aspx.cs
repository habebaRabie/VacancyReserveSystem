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

namespace HoildaysCalcApp
{
    public partial class Contact : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Calendar1.SelectedDate = DateTime.Now.Date;
            Calendar2.SelectedDate = DateTime.Now.Date;
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
                    string selectQuery = "SELECT holidayDate FROM PublicHolidays";
                    using (MySqlCommand cmd = new MySqlCommand(selectQuery, connection))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DateTime holidayDate = reader.GetDateTime("holidayDate");
                                retrievedHolidays.Add(holidayDate);
                            }
                        }
                    }
                }

                return retrievedHolidays;
            }
            catch (Exception ex)
            {
                ResultLabel.Text = "Error: " + ex.Message;
                return null;
            }
        }

        protected void AddWorkingDaysButton_Click(object sender, EventArgs e)
        {
            // Get the current vacDays from the database for the employee
            string employeeName = EmployeeNameTextBox.Text;
            DateTime startDate = DateTime.Parse(Calendar1.SelectedDate.ToString("D"));
            DateTime endDate = DateTime.Parse(Calendar2.SelectedDate.ToString("D"));

            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            int vacDays, workingDays;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sqlQuery = "SELECT id, vacDays FROM Employee WHERE name = @Name";

                    using (MySqlCommand cmd = new MySqlCommand(sqlQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Name", employeeName);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                int employeeId = reader.GetInt32("id");
                                vacDays = reader.GetInt32("vacDays");
                                reader.Close();
                                workingDays = CountWorkingDays(startDate, endDate);

                                // Insert the data into the holidays table
                                InsertHolidayData(connection, employeeId, startDate, endDate);

                                // Update the employee's vacDays in the database
                                int newVacDays = vacDays + workingDays;
                                UpdateEmployeeVacDays(connection, employeeId, newVacDays);

                                ResultLabel.Text = $"Enjoy your holiday! You have taken {newVacDays} days as a holiday until now";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ResultLabel.Text = "Error: " + ex.Message;
                }
                finally
                {
                    connection.Close();
                }
            }

            // Hide the "Add Working Days" button again
            AddWorkingDaysButton.Visible = false;
        }

        protected void CalculateButton_Click(object sender, EventArgs e)
        {
            string employeeName = EmployeeNameTextBox.Text;
            DateTime startDate = DateTime.Parse(Calendar1.SelectedDate.ToString("D"));
            DateTime endDate = DateTime.Parse(Calendar2.SelectedDate.ToString("D"));

            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            int vacDays, workingDays;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sqlQuery = "SELECT id, vacDays FROM Employee WHERE name = @Name";

                    using (MySqlCommand cmd = new MySqlCommand(sqlQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Name", employeeName);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int employeeId = reader.GetInt32("id");
                                vacDays = reader.GetInt32("vacDays");

                                reader.Close();

                                workingDays = CountWorkingDays(startDate, endDate);

                                // Check if the employee has enough remaining vacation days
                                if (vacDays + workingDays <= 30)
                                {
                                    // Check if the selected days are not already in the holidays table
                                    if (!IsDaysAlreadyBooked(connection, employeeId, startDate, endDate))
                                    {
                                        ResultLabel.Text = $"Hello {employeeName}, you will take {workingDays} working days between {startDate:yyyy-MM-dd} and {endDate:yyyy-MM-dd} as a holiday!";
                                        AddWorkingDaysButton.Visible = true; // Show the button
                                    }
                                    else
                                    {
                                        ResultLabel.Text = $"The selected days are already booked as a holiday.";
                                    }
                                }
                                else
                                {
                                    int remainingDays = 30 - vacDays;
                                    ResultLabel.Text = $"You only have {remainingDays} days left and you are asking for {workingDays}!";
                                }
                            }
                            else
                            {
                                ResultLabel.Text = $"Employee {employeeName} not found.";
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    ResultLabel.Text = "Error: " + ex.Message;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private bool IsDaysAlreadyBooked(MySqlConnection connection, int employeeId, DateTime startDate, DateTime endDate)
        {
            string query = "SELECT COUNT(*) FROM holidays WHERE EmpID = @EmployeeId AND start_date <= @EndDate AND end_date >= @StartDate";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }


        private void InsertHolidayData(MySqlConnection connection, int employeeId, DateTime startDate, DateTime endDate)
        {
            string query = "INSERT INTO holidays (EmpID, start_date, end_date) VALUES (@EmployeeId, @StartDate, @EndDate)";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);
                cmd.ExecuteNonQuery();
            }
        }
        private void UpdateEmployeeVacDays(MySqlConnection connection, int employeeId, int newVacDays)
        {
            string query = "UPDATE Employee SET vacDays = @NewVacDays WHERE id = @EmployeeId";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                cmd.Parameters.AddWithValue("@NewVacDays", newVacDays);
                cmd.ExecuteNonQuery();
            }
        }

        private int CountWorkingDays(DateTime startDate, DateTime endDate)
        {
            int workingDays = 0;
            List<DateTime> publicHolidays = GetPublicHolidays();

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                // Check if the current day is not Saturday (DayOfWeek.Saturday) or Sunday (DayOfWeek.Sunday)
                if (date.DayOfWeek != DayOfWeek.Friday && date.DayOfWeek != DayOfWeek.Saturday)
                {
                    // Check if the current day is not a public holiday
                    if (!publicHolidays.Contains(date))
                    {
                        workingDays++;
                    }
                }
            }

            return workingDays;
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            ResultLabel.Text = "You Selected: "+Calendar1.SelectedDate.ToString("D"); 
        }

        protected void Calendar2_SelectionChanged(object sender, EventArgs e)
        {
            ResultLabel.Text = "You Selected: " + Calendar2.SelectedDate.ToString("D");
        }
    }
}