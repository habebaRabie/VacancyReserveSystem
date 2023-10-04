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
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DataBindVacToGrid();
            }
        }

        public void DataBindVacToGrid()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sqlQuery = "SELECT vac_ID, vac_name FROM vacancytype";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(sqlQuery, connection))
                    {
                        DataSet dt = new DataSet();
                        adapter.Fill(dt);

                        if (dt.Tables[0].Rows.Count > 0)
                        {
                            gvVacancies.DataSource = dt;
                            gvVacancies.DataBind();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ltErrorVac.Text = "Error: " + ex.Message;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
        
        protected void gvVacancies_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            ltErrorVac.Text = string.Empty;
            GridViewRow gvRow = gvVacancies.Rows[e.RowIndex];

            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Get the date value from the GridView and format it
                    int vacancyId = int.Parse(gvRow.Cells[0].Text);
                    string vacancyType = gvRow.Cells[1].Text;

                    // Parameterized SQL query to delete based on the formatted date
                    string sqlQuery = "DELETE FROM vacancytype WHERE vac_ID = @vacancyId";

                    MySqlCommand command = new MySqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@vacancyId", vacancyId);

                    command.ExecuteNonQuery();
                    gvVacancies.EditIndex = -1;
                    DataBindVacToGrid();
                }
                catch (Exception ex)
                {
                    ltErrorVac.Text = "Error: " + ex.Message;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }


        protected void btnInsertNewVacancy_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string sqlQuery = "INSERT INTO vacancytype (vac_name) VALUES ( @vacancyName)";
                    MySqlCommand command = new MySqlCommand(sqlQuery, connection);

                    // Use the correct parameter names
                    command.Parameters.AddWithValue("@vacancyName", txtnewVacType.Text);
                    command.ExecuteNonQuery();

                    DataBindVacToGrid();
                    txtnewVacType.Text = "";

                }
                catch (Exception ex)
                {
                    ltErrorVac.Text = "Error: " + ex.Message;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }


        protected void gvVacancies_RowEditing(object sender, GridViewEditEventArgs e)
        {
            ltErrorVac.Text = string.Empty;
            gvVacancies.EditIndex = e.NewEditIndex;
            DataBindVacToGrid();
            
        }

        protected void gvVacancies_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            ltErrorVac.Text = string.Empty;
            GridViewRow gvRow = gvVacancies.Rows[e.RowIndex];

            // Get the updated values from the GridView's TextBox controls

            TextBox vacancyId = (TextBox)gvRow.Cells[0].Controls[0];
            TextBox vacancyType = (TextBox)gvRow.Cells[1].Controls[0];

            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Use parameterized query to update the record
                    string sqlQuery = "UPDATE vacancytype SET vac_name = @vacancyType WHERE vac_ID = @vacancyId";
                    MySqlCommand command = new MySqlCommand(sqlQuery, connection);

                    // Add parameters
                    command.Parameters.AddWithValue("@vacancyId", vacancyId.Text);
                    command.Parameters.AddWithValue("@vacancyType", vacancyType.Text);

                    command.ExecuteNonQuery();
                    gvVacancies.EditIndex = -1;
                    DataBindVacToGrid();
                }
                catch (Exception ex)
                {
                    ltErrorVac.Text = "Error: " + ex.Message;
                }
                finally
                {
                    connection.Close();
                }
            }
        }


        protected void gvVacancies_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvVacancies.EditIndex = -1;
            DataBindVacToGrid();
        }

    }
}