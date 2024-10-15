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
            if (Session["UserID"] != null && Convert.ToInt32(Session["RoleID"]) == 7) //Only students can create time tables
            {
                UserID = Convert.ToInt32(Session["UserID"]);
            }
            else
            {
                // Redirect back to dashboard
                Response.Redirect("frmDashboard.aspx");
            }

            if (!IsPostBack)
            {
                loadModuleData(UserID);
            }

            //Hide edit controls
            lblEditResults.Visible = false;
            lblEditDay.Visible = false;
            lblEditEndTime.Visible = false;
            lblEditStartTime.Visible = false;
            btnConfirmEdit.Visible = false;
            btnDeleteClass.Visible = false;
            txtEditEndTime.Visible = false;
            txtEditStartTime.Visible = false;
            ddlClass.Visible = false;
        }

        protected void loadModuleData(int Userid)
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

                // Validate Day selection
                if (dayId <= 0)
                {
                    lblResult.Text = "Please select a valid day from the dropdown.";
                    lblResult.Visible = true;
                    return;
                }

                // Validate Module selection
                if (moduleId <= 0)
                {
                    lblResult.Text = "Please select a valid module from the dropdown.";
                    lblResult.Visible = true;
                    return;
                }

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

        private void LoadTimeTableForEdit()
        {
            try
            {
                Con.Open();

                // SQL query to retrieve the timetable data for editing, ordered by day and start time
                string sql = @"
                            SELECT TT.time_table_id, D.day_name, M.module_name, TT.class_start_time, TT.class_end_time
                            FROM TimeTable TT
                            JOIN DaysOfTheWeek D ON TT.day_id = D.day_id
                            JOIN Modules M ON TT.module_id = M.module_id
                            WHERE TT.user_id = @UserID
                            ORDER BY TT.day_id, TT.class_start_time";

                using (SqlCommand cmd = new SqlCommand(sql, Con))
                {
                    cmd.Parameters.AddWithValue("@UserID", UserID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            ddlClass.Items.Clear();  // Clear previous items in the dropdown

                            while (reader.Read())
                            {
                                // Create formatted string for each class (e.g. Monday - Math 101 - 10:00 - 11:00)
                                string classInfo = $"{reader["day_name"]} - {reader["module_name"]} - {reader["class_start_time"]} - {reader["class_end_time"]}";

                                // Add class information to the dropdown
                                ddlClass.Items.Add(new ListItem(classInfo, reader["time_table_id"].ToString()));
                            }
                        }
                        else
                        {
                            lblEditResults.Text = "No timetable found.";
                            lblEditResults.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblEditResults.Text = "Error loading timetable for editing: " + ex.Message;
                lblEditResults.Visible = true;
            }
            finally
            {
                Con.Close();
            }
        }

        protected void btnConfirmEdit_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate selected class and inputs
                int classId = Convert.ToInt32(ddlClass.SelectedValue);
                int dayId = Convert.ToInt32(ddlEditDayOfWeek.SelectedValue);

                // Validate the time inputs
                if (!TimeSpan.TryParse(txtEditStartTime.Text, out TimeSpan startTime))
                {
                    lblEditResults.Text = "Invalid start time format. Please use HH:mm format.";
                    lblEditResults.Visible = true;
                    return;
                }

                if (!TimeSpan.TryParse(txtEditEndTime.Text, out TimeSpan endTime))
                {
                    lblEditResults.Text = "Invalid end time format. Please use HH:mm format.";
                    lblEditResults.Visible = true;
                    return;
                }

                if (endTime <= startTime)
                {
                    lblEditResults.Text = "End time must be after start time.";
                    lblEditResults.Visible = true;
                    return;
                }

                Con.Open();

                // Update the class details in the TimeTable
                string sqlUpdate = @"UPDATE TimeTable
                             SET day_id = @DayId, class_start_time = @StartTime, class_end_time = @EndTime
                             WHERE time_table_id = @ClassId AND user_id = @UserId";

                using (SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, Con))
                {
                    cmdUpdate.Parameters.AddWithValue("@ClassId", classId);
                    cmdUpdate.Parameters.AddWithValue("@DayId", dayId);
                    cmdUpdate.Parameters.AddWithValue("@StartTime", startTime);
                    cmdUpdate.Parameters.AddWithValue("@EndTime", endTime);
                    cmdUpdate.Parameters.AddWithValue("@UserId", UserID);

                    int rowsAffected = cmdUpdate.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        lblEditResults.Text = "Class updated successfully!";
                    }
                    else
                    {
                        lblEditResults.Text = "Failed to update class.";
                    }
                    lblEditResults.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblEditResults.Text = "Error updating class: " + ex.Message;
                lblEditResults.Visible = true;
            }
            finally
            {
                Con.Close();
            }
        }

        protected void btnDeleteClass_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the selected class ID
                int classId = Convert.ToInt32(ddlClass.SelectedValue);

                Con.Open();

                // SQL query to delete the selected class
                string sqlDelete = @"DELETE FROM TimeTable WHERE time_table_id = @ClassId AND user_id = @UserId";

                using (SqlCommand cmdDelete = new SqlCommand(sqlDelete, Con))
                {
                    cmdDelete.Parameters.AddWithValue("@ClassId", classId);
                    cmdDelete.Parameters.AddWithValue("@UserId", UserID);

                    int rowsAffected = cmdDelete.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        lblEditResults.Text = "Class deleted successfully!";
                        lblEditResults.Visible = true;
                    }
                    else
                    {
                        lblEditResults.Text = "Failed to delete class.";
                        lblEditResults.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblEditResults.Text = "Error deleting class: " + ex.Message;
                lblEditResults.Visible = true;
            }
            finally
            {
                Con.Close();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            // Redirect back to dashboard
            Response.Redirect("frmDashboard.aspx");
        }

        protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            //show edit controls
            lblEditResults.Visible = true;
            lblEditDay.Visible = true;
            lblEditEndTime.Visible = true;
            lblEditStartTime.Visible = true;
            btnConfirmEdit.Visible = true;
            btnDeleteClass.Visible = true;
            txtEditEndTime.Visible = true;
            txtEditStartTime.Visible = true;
            ddlClass.Visible = true;
        }
    }
}