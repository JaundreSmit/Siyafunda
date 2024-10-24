using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SiyafundaApplication
{
    public partial class frmTimeTableEdit : System.Web.UI.Page
    {
        private SqlConnection Con;
        private int UserID = 0;

        // Async method to get the connection string
        protected async Task<string> getConnectionStringAsync()
        {
            // Assuming SiyafundaFunctions is a class with a method to get the connection string
            return await SiyafundaFunctions.GetConnectionStringAsync();
        }

        protected async void Page_Load(object sender, EventArgs e)
        {
            // Initialize connection using async method
            string connectionString = await getConnectionStringAsync();
            Con = new SqlConnection(connectionString);

            // Check user role for access
            if (Session["UserID"] != null && Convert.ToInt32(Session["RoleID"]) == 7) // Only students can create time tables
            {
                UserID = Convert.ToInt32(Session["UserID"]);
            }
            else
            {
                // Redirect to the dashboard if user is not valid
                Response.Redirect("frmDashboard.aspx");
            }

            if (!IsPostBack)
            {
                await loadModuleDataAsync(UserID);
            }

            // Hide edit controls initially
            HideEditControls();
        }

        // Async method to load module data into dropdowns
        protected async Task loadModuleDataAsync(int UserId)
        {
            try
            {
                await Con.OpenAsync();

                // Populate ddlDayOfWeek with day names from DaysOfTheWeek table
                string sqlDay = "SELECT day_id, day_name FROM DaysOfTheWeek";
                using (SqlCommand cmd = new SqlCommand(sqlDay, Con))
                {
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
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

                // Populate ddlModule with module names from Modules table based on user
                string sqlModule = @"
                SELECT m.module_id, m.title AS module_name
                FROM Stu_To_Module stm
                INNER JOIN Modules m ON stm.module_id = m.module_id
                WHERE stm.user_id = @UserId";

                using (SqlCommand cmdModule = new SqlCommand(sqlModule, Con))
                {
                    cmdModule.Parameters.AddWithValue("@UserId", UserId);
                    using (SqlDataReader readerModule = await cmdModule.ExecuteReaderAsync())
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
                Con.Close(); // Use the synchronous Close() method
            }
        }

        protected async void btnAddClass_Click(object sender, EventArgs e)
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
                await Con.OpenAsync();

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

                    int rowsAffected = await cmdInsert.ExecuteNonQueryAsync();

                    // Display result
                    lblResult.Text = rowsAffected > 0 ? "Class added successfully!" : "Failed to add class.";
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
                Con.Close(); // Use the synchronous Close() method
            }
        }

        private async Task LoadTimeTableForEditAsync()
        {
            try
            {
                await Con.OpenAsync();

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

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            ddlClass.Items.Clear();  // Clear previous items in the dropdown

                            while (await reader.ReadAsync())
                            {
                                // Create formatted string for each class
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
                Con.Close(); // Use the synchronous Close() method
            }
        }

        protected async void btnConfirmEdit_Click(object sender, EventArgs e)
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

                await Con.OpenAsync();

                // Update the class details in the TimeTable
                string sqlUpdate = @"UPDATE TimeTable
                             SET day_id = @DayId, class_start_time = @StartTime, class_end_time = @EndTime
                             WHERE time_table_id = @ClassId AND user_id = @UserId";
                using (SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, Con))
                {
                    cmdUpdate.Parameters.AddWithValue("@ClassId", classId);
                    cmdUpdate.Parameters.AddWithValue("@UserId", UserID);
                    cmdUpdate.Parameters.AddWithValue("@DayId", dayId);
                    cmdUpdate.Parameters.AddWithValue("@StartTime", startTime);
                    cmdUpdate.Parameters.AddWithValue("@EndTime", endTime);

                    int rowsAffected = await cmdUpdate.ExecuteNonQueryAsync();
                    lblEditResults.Text = rowsAffected > 0 ? "Class updated successfully!" : "Failed to update class.";
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
                Con.Close(); // Use the synchronous Close() method
            }
        }

        protected async void btnDeleteClass_Click(object sender, EventArgs e)
        {
            try
            {
                int classId = Convert.ToInt32(ddlClass.SelectedValue);

                // Ensure a class is selected for deletion
                if (classId <= 0)
                {
                    lblEditResults.Text = "Please select a class to delete.";
                    lblEditResults.Visible = true;
                    return;
                }

                await Con.OpenAsync();

                // Delete the selected class from the TimeTable
                string sqlDelete = @"DELETE FROM TimeTable WHERE time_table_id = @ClassId AND user_id = @UserId";
                using (SqlCommand cmdDelete = new SqlCommand(sqlDelete, Con))
                {
                    cmdDelete.Parameters.AddWithValue("@ClassId", classId);
                    cmdDelete.Parameters.AddWithValue("@UserId", UserID);

                    int rowsAffected = await cmdDelete.ExecuteNonQueryAsync();
                    lblEditResults.Text = rowsAffected > 0 ? "Class deleted successfully!" : "Failed to delete class.";
                    lblEditResults.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblEditResults.Text = "Error deleting class: " + ex.Message;
                lblEditResults.Visible = true;
            }
            finally
            {
                Con.Close(); // Use the synchronous Close() method
            }
        }

        // Method to hide edit controls
        private void HideEditControls()
        {
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
    }
}