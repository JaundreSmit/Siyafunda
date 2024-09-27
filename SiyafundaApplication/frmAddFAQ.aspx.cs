using System;
using System.Data.SqlClient;
using System.Web.UI;

namespace SiyafundaApplication
{
    public partial class frmAddFAQ : System.Web.UI.Page
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
                        lblModuleName.Text = result.ToString(); // Display the module title
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

        protected void btnAddFAQ_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtQuestion.Text) || string.IsNullOrWhiteSpace(txtAnwer.Text))
            {
                lblErrors.Text = "Please fill in both the question and the answer.";
                lblErrors.Visible = true;
                return;
            }

            int userID = Convert.ToInt32(Session["UserID"]);
            int module_id = GetModuleId(lblModuleName.Text);
            string question = txtQuestion.Text.Trim();
            string answer = txtAnwer.Text.Trim();
            DateTime dateTime = DateTime.Now;

            try
            {
                string insertQuery = @"
                    INSERT INTO FAQs (question, answer, user_id, module_id, created_at)
                    VALUES (@Question, @Answer, @UserId, @ModuleId, @created_at)";

                using (SqlCommand cmd = new SqlCommand(insertQuery, Con))
                {
                    cmd.Parameters.AddWithValue("@Question", question);
                    cmd.Parameters.AddWithValue("@Answer", answer);
                    cmd.Parameters.AddWithValue("@UserId", userID);
                    cmd.Parameters.AddWithValue("@ModuleId", module_id);
                    cmd.Parameters.AddWithValue("@created_at", dateTime);

                    Con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        lblErrors.Text = "FAQ added successfully.";
                        lblErrors.Visible = true;
                        // Optionally clear the fields after successful insert
                        txtQuestion.Text = string.Empty;
                        txtAnwer.Text = string.Empty;
                    }
                    else
                    {
                        lblErrors.Text = "Failed to add the FAQ.";
                        lblErrors.Visible = true;
                    }
                }
            }
            catch (SqlException ex)
            {
                lblErrors.Text = "An error occurred while adding the FAQ: " + ex.Message;
                lblErrors.Visible = true;
            }
            finally
            {
                Con.Close();
            }
        }

        protected void Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmEducator.aspx");
        }
    }
}