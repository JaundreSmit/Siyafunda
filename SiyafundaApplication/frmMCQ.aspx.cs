using System;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace SiyafundaApplication
{
    public partial class frmMCQ : System.Web.UI.Page
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

        // Add the Multiple Choice Question to the database
        protected void btnSubmitMCQ_Click(object sender, EventArgs e)
        {
            string questionText = txtQuestion.Text;
            string optionA = txtOptionA.Text;
            string optionB = txtOptionB.Text;
            string optionC = txtOptionC.Text;
            string optionD = txtOptionD.Text;
            string correctAnswer = ddlCorrectAnswer.SelectedValue;

            // Get the selected Quiz ID dynamically
            int quizId = GetSelectedQuizId();

            // Basic validation
            if (string.IsNullOrEmpty(questionText) || string.IsNullOrEmpty(optionA) ||
                string.IsNullOrEmpty(optionB) || string.IsNullOrEmpty(optionC) || string.IsNullOrEmpty(optionD))
            {
                lblMessage.Text = "Please fill all options.";
                lblMessage.Visible = true;
                return;
            }

            try
            {
                using (Con)
                {
                    Con.Open();

                    // Insert question into the MCQuestions table
                    string questionQuery = @"INSERT INTO [dbo].[MCQuestions] 
                                             (quiz_id, question_text, option_a, option_b, option_c, option_d, correct_answer) 
                                             VALUES (@QuizId, @QuestionText, @OptionA, @OptionB, @OptionC, @OptionD, @CorrectAnswer);";

                    SqlCommand cmd = new SqlCommand(questionQuery, Con);
                    cmd.Parameters.AddWithValue("@QuizId", quizId);
                    cmd.Parameters.AddWithValue("@QuestionText", questionText);
                    cmd.Parameters.AddWithValue("@OptionA", optionA);
                    cmd.Parameters.AddWithValue("@OptionB", optionB);
                    cmd.Parameters.AddWithValue("@OptionC", optionC);
                    cmd.Parameters.AddWithValue("@OptionD", optionD);
                    cmd.Parameters.AddWithValue("@CorrectAnswer", correctAnswer);

                    cmd.ExecuteNonQuery();

                    lblMessage.Text = "Question added successfully!";
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Visible = true;

                    Con.Close();
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

        private int GetSelectedQuizId()
        {
            return Convert.ToInt32(Session["QuizID"]);
        }
    }
}
