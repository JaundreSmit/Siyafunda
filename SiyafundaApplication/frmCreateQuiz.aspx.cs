using System;
using System.Data.SqlClient;

namespace SiyafundaApplication
{
    public partial class frmCreateQuiz : System.Web.UI.Page
    {
        // Declare SQL connection
        private SqlConnection Con;

        protected void Page_Load(object sender, EventArgs e)
        {
            Con = new SqlConnection(getConnectionString());
        }

        // Connection string to the database
        protected string getConnectionString()
        {
            return @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\SiyafundaDB.mdf;Integrated Security=True";
        }

        // Handle creating the quiz entry in the database
        protected void btnSubmitQuiz_Click(object sender, EventArgs e)
        {
            string quizTitle = txtQuizTitle.Text;
            string quizDescription = txtQuizDescription.Text;
            int moduleId = 1; // This should be dynamic based on which module the quiz is being created for.

            // Validate the form inputs
            if (string.IsNullOrEmpty(quizTitle) || string.IsNullOrEmpty(quizDescription))
            {
                lblMessage.Text = "Please fill in all fields.";
                lblMessage.Visible = true;
                return;
            }

            try
            {
                using (Con)
                {
                    Con.Open();

                    // Insert the new quiz into the Quizzes table
                    string insertQuizQuery = @"INSERT INTO [dbo].[Quizzes] 
                                                (module_id, title, duration, due_date) 
                                               VALUES 
                                                (@ModuleId, @Title, @Duration, @DueDate); 
                                               SELECT SCOPE_IDENTITY();";

                    SqlCommand cmd = new SqlCommand(insertQuizQuery, Con);
                    cmd.Parameters.AddWithValue("@ModuleId", moduleId);
                    cmd.Parameters.AddWithValue("@Title", quizTitle);
                    cmd.Parameters.AddWithValue("@Duration", 60); // Static for now, can be added to the form if needed.
                    cmd.Parameters.AddWithValue("@DueDate", DateTime.Now.AddDays(7)); // Static, replace with actual due date field if required.

                    // Execute query and retrieve new quiz_id
                    int newQuizId = Convert.ToInt32(cmd.ExecuteScalar());

                    // Store the new quiz ID in session so questions can be linked to this quiz
                    Session["QuizID"] = newQuizId;

                    lblMessage.Text = "Quiz created successfully! Quiz ID: " + newQuizId;
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
                lblMessage.Visible = true;
            }
            finally
            {
                Con.Close();
            }
        }

        // Navigate to the Multiple Choice Question creation form
        protected void btnAddMCQ_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmAddMCQ.aspx"); // Use session-stored quiz ID for adding questions
        }

        // Navigate to the Fill in the Blank Question creation form
        protected void btnAddFillBlank_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmAddFillBlank.aspx"); // Use session-stored quiz ID for adding questions
        }

        // Navigate to the Long Form Question creation form
        protected void btnAddLongForm_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmAddLongForm.aspx"); // Use session-stored quiz ID for adding questions
        }
    }
}
