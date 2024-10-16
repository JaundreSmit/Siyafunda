using System;
using System.Data.SqlClient;
using System.Web.UI;

namespace SiyafundaApplication
{
    public partial class frmMCAnswer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadQuestion();
                StartTimer();
            }
        }

        // Load the MCQ from the database using the questionId passed in query string
        private void LoadQuestion()
        {
            int questionId = Convert.ToInt32(Request.QueryString["questionId"]);
            int currentQuestionIndex = Convert.ToInt32(Session["CurrentQuestionIndex"]) + 1;

            lblQuestionNumber.Text = currentQuestionIndex.ToString();

            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                string query = "SELECT question_text, option_a, option_b, option_c, option_d FROM MCQuestions WHERE question_id = @QuestionId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@QuestionId", questionId);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    lblQuestionText.Text = reader["question_text"].ToString();
                    rblOptions.Items[0].Text = reader["option_a"].ToString();
                    rblOptions.Items[1].Text = reader["option_b"].ToString();
                    rblOptions.Items[2].Text = reader["option_c"].ToString();
                    rblOptions.Items[3].Text = reader["option_d"].ToString();
                }

                reader.Close();
                con.Close();
            }
        }

        // Start the timer for the quiz
        private void StartTimer()
        {
            DateTime quizStartTime = (DateTime)Session["QuizStartTime"];
            int quizDuration = (int)Session["QuizDuration"]; // in minutes
            DateTime quizEndTime = quizStartTime.AddMinutes(quizDuration);

            if (DateTime.Now > quizEndTime)
            {
                // Time's up, handle it
                EndQuiz();
            }
            else
            {
                TimeSpan timeRemaining = quizEndTime - DateTime.Now;
                lblTimer.Text = "Time Remaining: " + timeRemaining.ToString(@"hh\:mm\:ss");
            }
        }

        // Handle the Next Question button click
        protected void btnNext_Click(object sender, EventArgs e)
        {
            // Save the selected answer to the database
            SaveAnswer();

            // Redirect to the next question
            RedirectToNextQuestion();
        }

        // Save the selected answer to the QuizResponses table
        private void SaveAnswer()
        {
            int quizId = Convert.ToInt32(Session["QuizID"]);
            int questionId = Convert.ToInt32(Request.QueryString["questionId"]);
            string selectedOption = rblOptions.SelectedValue;

            if (string.IsNullOrEmpty(selectedOption))
            {
                lblMessage.Text = "Please select an answer.";
                lblMessage.Visible = true;
                return;
            }

            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                string query = "INSERT INTO QuizResponses (quiz_id, question_id, user_id, response) VALUES (@QuizId, @QuestionId, @UserId, @Response)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@QuizId", quizId);
                cmd.Parameters.AddWithValue("@QuestionId", questionId);
                cmd.Parameters.AddWithValue("@UserId", Session["UserId"]); // Assumes user ID is stored in Session
                cmd.Parameters.AddWithValue("@Response", selectedOption);

                cmd.ExecuteNonQuery();
            }
        }

        private void RedirectToNextQuestion()
        {
            int currentQuestionIndex = Convert.ToInt32(Session["CurrentQuestionIndex"]);
            List<Question> questions = (List<Question>)Session["QuizQuestions"];

            if (currentQuestionIndex < questions.Count)
            {
                Question nextQuestion = questions[currentQuestionIndex];
                Session["CurrentQuestionIndex"] = currentQuestionIndex + 1;

                // Redirect based on the question type to specific forms
                if (nextQuestion.QuestionType == "MCQ")
                {
                    Response.Redirect("frmMCAnswer.aspx?questionId=" + nextQuestion.QuestionId);
                }
                else if (nextQuestion.QuestionType == "FBQ")
                {
                    Response.Redirect("frmFBAnswer.aspx?questionId=" + nextQuestion.QuestionId);
                }
                else if (nextQuestion.QuestionType == "LFQ")
                {
                    Response.Redirect("frmLFAnswer.aspx?questionId=" + nextQuestion.QuestionId);
                }
            }
            else
            {
                EndQuiz();
            }
        }

        // End the quiz when time is up or all questions are answered
        private void EndQuiz()
        {
            Response.Redirect("frmQuizCompleted.aspx");
        }

        private string GetConnectionString()
        {
            return @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\SiyafundaDB.mdf;Integrated Security=True";
        }
    }
}
