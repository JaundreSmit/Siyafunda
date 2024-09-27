﻿using System;
using System.Data.SqlClient;

namespace SiyafundaApplication
{
    public partial class frmFillBlank : System.Web.UI.Page
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

        // Add the Fill in the Blank Question to the database
        protected void btnSubmitFillBlank_Click(object sender, EventArgs e)
        {
            string questionText = txtQuestion.Text;
            string correctAnswer = txtCorrectAnswer.Text;

            // Basic validation
            if (string.IsNullOrEmpty(questionText) || string.IsNullOrEmpty(correctAnswer))
            {
                lblMessage.Text = "Please fill all fields.";
                lblMessage.Visible = true;
                return;
            }

            try
            {
                using (Con)
                {
                    Con.Open();

                    // Insert question into the database
                    string query = "INSERT INTO [dbo].[QuizQuestions] (quiz_id, question_text, question_type, correct_answer) " +
                                   "VALUES (@QuizId, @QuestionText, @QuestionType, @CorrectAnswer)";

                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.Parameters.AddWithValue("@QuizId", /* Replace with selected quiz ID */ 1);
                    cmd.Parameters.AddWithValue("@QuestionText", questionText);
                    cmd.Parameters.AddWithValue("@QuestionType", "Fill in the Blank");
                    cmd.Parameters.AddWithValue("@CorrectAnswer", correctAnswer);

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
