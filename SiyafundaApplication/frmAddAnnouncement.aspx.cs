using System;
using System.Data.SqlClient;
using System.Web.UI;

namespace SiyafundaApplication
{
    public partial class frmAddAnnouncement : System.Web.UI.Page
    {
        protected string getConnectionString()
        {
            return @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\SiyafundaDB.mdf;Integrated Security=True";
        }

        private SqlConnection Con;

        protected void Page_Load(object sender, EventArgs e)
        {
            Con = new SqlConnection(getConnectionString());
            if (Session["UserID"] == null || Convert.ToInt32(Session["RoleID"]) != 6)
            {
                Response.Redirect("frmEducator.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadModule();
            }
            lblErrors.Visible = false;
        }

        private void LoadModule()
        {
            try
            {
                string query = @"
                    SELECT title
                    FROM Modules
                    WHERE educator_id = @EducatorId";

                using (SqlCommand cmd = new SqlCommand(query, Con))
                {
                    cmd.Parameters.AddWithValue("@EducatorId", Convert.ToInt32(Session["UserID"]));
                    Con.Open();
                    var result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        lblModuleName.Text = result.ToString(); // Assuming lblModuleName is the label to display the module title
                    }
                    else
                    {
                        lblErrors.Text = "No module found for this educator.";
                        lblErrors.Visible = true;
                    }
                }
            }
            catch (SqlException ex)
            {
                lblErrors.Text = "An error occurred while loading the module: " + ex.Message;
                lblErrors.Visible = true;
            }
            finally
            {
                Con.Close();
            }
        }

        protected void btnAddAnnouncement_Click(object sender, EventArgs e)
        {
            string title = txtTitle.Text.Trim();
            string content = txtContent.Text.Trim();
            DateTime created_at = DateTime.Now;
            int User_ID = Convert.ToInt32(Session["UserID"]);

            // Retrieve the ModuleID using the loaded module title
            int ModuleID = GetModuleId(lblModuleName.Text);

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content))
            {
                lblErrors.Text = "Title and content cannot be empty.";
                lblErrors.Visible = true;
                return;
            }

            try
            {
                // Check if the title already exists
                string checkQuery = @"
                    SELECT COUNT(*)
                    FROM Announcements
                    WHERE title = @Title AND module_id = @ModuleID";

                using (SqlCommand checkCmd = new SqlCommand(checkQuery, Con))
                {
                    checkCmd.Parameters.AddWithValue("@Title", title);
                    checkCmd.Parameters.AddWithValue("@ModuleID", ModuleID);
                    Con.Open();
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        lblErrors.Text = "An announcement with this title already exists for the selected module.";
                        lblErrors.Visible = true;
                        return;
                    }
                }

                // Insert into the database
                string insertQuery = @"
                    INSERT INTO Announcements (title, content, created_at, user_id, module_id)
                    VALUES (@Title, @Content, @CreatedAt, @UserId, @ModuleId)";

                using (SqlCommand insertCmd = new SqlCommand(insertQuery, Con))
                {
                    insertCmd.Parameters.AddWithValue("@Title", title);
                    insertCmd.Parameters.AddWithValue("@Content", content);
                    insertCmd.Parameters.AddWithValue("@CreatedAt", created_at);
                    insertCmd.Parameters.AddWithValue("@UserId", User_ID);
                    insertCmd.Parameters.AddWithValue("@ModuleId", ModuleID);

                    insertCmd.ExecuteNonQuery();
                    lblErrors.Text = "Announcement added successfully.";
                    lblErrors.Visible = true;
                }
            }
            catch (SqlException ex)
            {
                lblErrors.Text = "An error occurred while adding the announcement: " + ex.Message;
                lblErrors.Visible = true;
            }
            finally
            {
                Con.Close();
            }
        }

        private int GetModuleId(string moduleTitle)
        {
            int moduleId = 0;
            try
            {
                string query = @"
                    SELECT module_id
                    FROM Modules
                    WHERE title = @Title";

                using (SqlCommand cmd = new SqlCommand(query, Con))
                {
                    cmd.Parameters.AddWithValue("@Title", moduleTitle);
                    Con.Open();
                    var result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        moduleId = Convert.ToInt32(result);
                    }
                }
            }
            catch (SqlException ex)
            {
                lblErrors.Text = "An error occurred while retrieving the module ID: " + ex.Message;
                lblErrors.Visible = true;
            }
            finally
            {
                Con.Close();
            }

            return moduleId;
        }

        protected void Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmEducator.aspx");
        }
    }
}