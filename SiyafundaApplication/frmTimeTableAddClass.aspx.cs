using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SiyafundaApplication
{
    public partial class frmTimeTableAddClass : System.Web.UI.Page
    {
        // Define connection string method
        protected string getConnectionString()
        {
            return @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\SiyafundaDB.mdf;Integrated Security=True";
        }

        private SqlConnection Con;

        private int UserID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Initialize connection using getConnectionString method
            Con = new SqlConnection(getConnectionString());
            if (Session["UserID"] != null)
            {
                UserID = Convert.ToInt32(Session["UserID"]);
            }

            if (!IsPostBack)
            {
                loadData(UserID);
            }
        }

        protected void loadData(int Userid)
        {
            try
            {
                // Open SQL connection
                Con.Open();

                // 1. Populate ddlDayOfWeek with all day_name from DaysOfTheWeek table
                string sqlDay = "SELECT day_id, day_name FROM DaysOfTheWeek";
                using (SqlCommand cmd = new SqlCommand(sqlDay, Con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            ddlDayOfWeek.DataSource = reader;
                            ddlDayOfWeek.DataTextField = "day_name";
                            ddlDayOfWeek.DataValueField = "day_id";
                            ddlDayOfWeek.DataBind();
                        }
                        else
                        {
                            lblResult.Text = "No data found in DaysOfTheWeek.";
                            lblResult.Visible = true;
                        }
                    }
                }

                // 2. Populate ddlModule with module_name from the Modules table where user_id matches in Stu_To_Module table
                string sqlModule = @"
            SELECT m.module_id, m.title AS module_name
            FROM Stu_To_Module stm
            INNER JOIN Modules m ON stm.module_id = m.module_id
            WHERE stm.user_id = @UserId";

                using (SqlCommand cmdModule = new SqlCommand(sqlModule, Con))
                {
                    cmdModule.Parameters.AddWithValue("@UserId", Userid);
                    using (SqlDataReader readerModule = cmdModule.ExecuteReader())
                    {
                        if (readerModule.HasRows)
                        {
                            ddlModule.DataSource = readerModule;
                            ddlModule.DataTextField = "module_name"; // Display module name in dropdown
                            ddlModule.DataValueField = "module_id";  // Store module_id as value
                            ddlModule.DataBind();
                        }
                        else
                        {
                            lblResult.Text = "No modules found for the user.";
                            lblResult.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblResult.Text = "Error loading data: " + ex.Message;
                lblResult.Visible = true;
            }
            finally
            {
                // Ensure the connection is closed even if an error occurs
                Con.Close();
            }
        }

        protected void btnAddClass_Click(object sender, EventArgs e)
        {
            try
            {
                // Get values from the form with validation
                int dayId = Convert.ToInt32(ddlDayOfWeek.SelectedValue);
                int moduleId = Convert.ToInt32(ddlModule.SelectedValue);

                // Validate time inputs
                if (!TimeSpan.TryParse(txtStartTime.Text, out TimeSpan startTime))
                {
                    lblResult.Text = "Invalid start time format. Please use HH:mm format.";
                    lblResult.Visible = true;
                    return;
                }

                if (!TimeSpan.TryParse(txtEndTime.Text, out TimeSpan endTime))
                {
                    lblResult.Text = "Invalid end time format. Please use HH:mm format.";
                    lblResult.Visible = true;
                    return;
                }

                // Ensure end time is after start time
                if (endTime <= startTime)
                {
                    lblResult.Text = "End time must be after start time.";
                    lblResult.Visible = true;
                    return;
                }

                // Open connection
                Con.Open();

                // Insert into the TimeTable
                string sqlInsert = @"INSERT INTO TimeTable (user_id, module_id, day_id, class_start_time, class_end_time)
                                     VALUES (@UserId, @ModuleId, @DayId, @StartTime, @EndTime)";
                using (SqlCommand cmdInsert = new SqlCommand(sqlInsert, Con))
                {
                    cmdInsert.Parameters.AddWithValue("@UserId", UserID); // Assuming UserID is set elsewhere
                    cmdInsert.Parameters.AddWithValue("@ModuleId", moduleId);
                    cmdInsert.Parameters.AddWithValue("@DayId", dayId);
                    cmdInsert.Parameters.AddWithValue("@StartTime", startTime);
                    cmdInsert.Parameters.AddWithValue("@EndTime", endTime);

                    int rowsAffected = cmdInsert.ExecuteNonQuery();

                    // Display result
                    if (rowsAffected > 0)
                    {
                        lblResult.Text = "Class added successfully!";
                    }
                    else
                    {
                        lblResult.Text = "Failed to add class.";
                    }
                    lblResult.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblResult.Text = "Error adding class: " + ex.Message;
                lblResult.Visible = true;
            }
            finally
            {
                // Close the connection
                Con.Close();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            // Redirect back to frmTimeTableView.aspx
            Response.Redirect("frmTimeTableView.aspx");
        }
    }
}