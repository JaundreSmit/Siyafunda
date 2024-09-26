using System;
using System.Data.SqlClient;

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
            string option1 = txtOption1.Text;
            string option2 = txtOption2.Text;
            string option3 = txtOption3.Text;
            string option4 = txtOption4.Text;
            int correctOption = Convert.ToInt32(ddlCorrectOption.SelectedValue);

            // Basic validation
            if (string.IsNullOrEmpty(questionText) || string.IsNullOrEmpty(option1) ||
                string.IsNullOrEmpty(option2) || string.IsNullOrEmpty(option3) || string.IsNullOrEmpty(option4))
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

                    // Insert question into the database
                    string questionQuery = "INSERT INTO [dbo].[QuizQuestions] (quiz_id, question_text, question_type, correct_answer) " +
                                           "VALUES (@QuizId, @QuestionText, @QuestionType, @CorrectAnswer); SELECT SCOPE_IDENTITY();";

                    SqlCommand cmd = new SqlCommand(questionQuery, Con);
                    cmd.Parameters.AddWithValue("@QuizId", /* Replace with selected quiz ID */ 1);
                    cmd.Parameters.AddWithValue("@QuestionText", questionText);
                    cmd.Parameters.AddWithValue("@QuestionType", "Multiple Choice");
                    cmd.Parameters.AddWithValue("@CorrectAnswer", correctOption);

                    int questionId = Convert.ToInt32(cmd.ExecuteScalar());

                    // Insert each option into the database
                    string optionQuery = "INSERT INTO [dbo].[Options] (question_id, option_text) VALUES (@QuestionId, @OptionText)";
                    SqlCommand optionCmd = new SqlCommand(optionQuery, Con);

                    optionCmd.Parameters.AddWithValue("@QuestionId", questionId);
                    optionCmd.Parameters.AddWithValue("@OptionText", option1);
                    optionCmd.ExecuteNonQuery();

                    optionCmd.Parameters["@OptionText"].Value = option2;
                    optionCmd.ExecuteNonQuery();

                    optionCmd.Parameters["@OptionText"].Value = option3;
                    optionCmd.ExecuteNonQuery();

                    optionCmd.Parameters["@OptionText"].Value = option4;
                    optionCmd.ExecuteNonQuery();

                    lblMessage.Text = "Question added successfully!";
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
