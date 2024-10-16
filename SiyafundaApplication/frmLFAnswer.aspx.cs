using System;
using System.Data.SqlClient;
using System.Web.UI;

namespace SiyafundaApplication
{
    public partial class frmLFAnswer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Start the timer
                StartTimer();

                // Load the current long-form question
                int questionId = Convert.ToInt32(Request.QueryString["questionId"]);
                LoadQuestion(questionId);
            }
        }

        private void StartTimer()
        {
            DateTime quizStartTime = (DateTime)Session["QuizStartTime"];
            int quizDuration = (int)Session["QuizDuration"];
            DateTime quizEndTime = quizStartTime.AddMinutes(quizDuration);

            if (DateTime.Now > quizEndTime)
            {
                // Time is up, redirect to quiz completion
                Response.Redirect("frmQuizCompleted.aspx");
            }
            else
            {
                TimeSpan remainingTime = quizEndTime - DateTime.Now;
                lblTimer.Text = "Time Remaining: " + remainingTime.ToString(@"hh\:mm\:ss");
            }
        }

        private void LoadQuestion(int questionId)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                string query = "SELECT question_text FROM LFQuestions WHERE question_id = @QuestionId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@QuestionId", questionId);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    lblQuestionText.Text = reader["question_text"].ToString();
                    lblQuestionNumber.Text = "Question " + Session["CurrentQuestionIndex"];
                }
                con.Close();
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            // Validate the user's input
            if (string.IsNullOrWhiteSpace(txtAnswer.Text))
            {
                lblMessage.Text = "Please provide an answer.";
                lblMessage.Visible = true;
                return;
            }

            // Save the user's response
            SaveAnswer();

            // Redirect to the next question
            RedirectToNextQuestion();
        }

        private void SaveAnswer()
        {
            int quizId = (int)Session["QuizID"];
            int questionId = Convert.ToInt32(Request.QueryString["questionId"]);
            string userAnswer = txtAnswer.Text.Trim();

            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                string query = "INSERT INTO QuizResponses (quiz_id, question_id, user_id, response) VALUES (@QuizId, @QuestionId, @UserId, @Response)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@QuizId", quizId);
                cmd.Parameters.AddWithValue("@QuestionId", questionId);
                cmd.Parameters.AddWithValue("@UserId", Session["UserId"]);
                cmd.Parameters.AddWithValue("@Response", userAnswer);

                cmd.ExecuteNonQuery();

                con.Close();  
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
                // All questions answered, handle quiz completion
                Response.Redirect("frmQuizCompleted.aspx");
            }
        }

        private string GetConnectionString()
        {
            return @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\SiyafundaDB.mdf;Integrated Security=True";
        }
    }
}
