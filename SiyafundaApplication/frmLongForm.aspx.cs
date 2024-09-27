using System;
using System.Data.SqlClient;

namespace SiyafundaApplication
{
    public partial class frmLongForm : System.Web.UI.Page
    {
        private SqlConnection Con;

        protected void Page_Load(object sender, EventArgs e)
        {
            Con = new SqlConnection(getConnectionString());
        }

        protected string getConnectionString()
        {
            return @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\SiyafundaDB.mdf;Integrated Security=True";
        }

        // Add the Long Form Question to the database
        protected void btnSubmitLongForm_Click(object sender, EventArgs e)
        {
            string questionText = txtQuestion.Text;
            int points;

            // Validation for numeric points
            if (string.IsNullOrEmpty(questionText) || !int.TryParse(txtPoints.Text, out points) || points > 6 || points < 1)
            {
                lblMessage.Text = "Please enter valid question text and points (1-6).";
                lblMessage.Visible = true;
                return;
            }

            try
            {
                using (Con)
                {
                    Con.Open();

                    // Insert question into the database
                    string query = "INSERT INTO [dbo].[QuizQuestions] (quiz_id, question_text, question_type, points) " +
                                   "VALUES (@QuizId, @QuestionText, @QuestionType, @Points)";

                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.Parameters.AddWithValue("@QuizId", /* Replace with selected quiz ID */ 1);
                    cmd.Parameters.AddWithValue("@QuestionText", questionText);
                    cmd.Parameters.AddWithValue("@QuestionType", "Long Form");
                    cmd.Parameters.AddWithValue("@Points", points);

                    int result = cmd.ExecuteNonQuery();

                    lblMessage.Text = result > 0 ? "Question added successfully!" : "Error adding question.";
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
    }
}
