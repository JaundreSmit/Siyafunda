using System;
using System.Data.SqlClient;
using System.Web.UI;

namespace SiyafundaApplication
{
    public partial class frmQuizCompleted : System.Web.UI.Page
    {
        private int score = 0;
        private int total = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Display the appropriate completion message
                DisplayCompletionMessage();

                // Show the score and check if there were long-form questions
                DisplayScore();
            }
        }

        private void DisplayCompletionMessage()
        {
            // Check if the quiz ended due to time running out or manual submission
            if (Session["TimeRanOut"] != null && (bool)Session["TimeRanOut"])
            {
                lblCompletionMessage.Text = "Time Ran Out: Quiz Submitted Automatically";
            }
            else
            {
                lblCompletionMessage.Text = "Quiz Submitted";
            }
        }
        
        private void DisplayScore()
        {
            int quizId = (int)Session["QuizID"];
            int userId = (int)Session["UserID"];
            bool hasLongFormQuestions = false;

            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();

                // Calculate score for MCQuestions
                string mcqQuery = @"SELECT QR.response, MC.correct_answer 
                            FROM QuizResponses QR
                            JOIN MCQuestions MC ON QR.question_id = MC.question_id
                            WHERE QR.quiz_id = @QuizId AND QR.user_id = @UserId AND MC.quiz_id = @QuizId";
                SqlCommand cmd = new SqlCommand(mcqQuery, con);
                cmd.Parameters.AddWithValue("@QuizId", quizId);
                cmd.Parameters.AddWithValue("@UserId", userId);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string selectedAnswer = reader["response"].ToString();
                    string correctAnswer = reader["correct_answer"].ToString();

                    total += 1;  // MCQs are worth 1 point
                    if (selectedAnswer == correctAnswer)
                    {
                        score += 1;
                    }
                }
                reader.Close();

                // Calculate score for FBQuestions
                string fbqQuery = @"SELECT QR.response, FB.correct_answer 
                            FROM QuizResponses QR
                            JOIN FBQuestions FB ON QR.question_id = FB.question_id
                            WHERE QR.quiz_id = @QuizId AND QR.user_id = @UserId AND FB.quiz_id = @QuizId";
                cmd = new SqlCommand(fbqQuery, con);
                cmd.Parameters.AddWithValue("@QuizId", quizId);
                cmd.Parameters.AddWithValue("@UserId", userId);

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string selectedAnswer = reader["response"].ToString();
                    string correctAnswer = reader["correct_answer"].ToString();

                    total += 2;  // FBQs are worth 2 points
                    if (selectedAnswer == correctAnswer)
                    {
                        score += 2;
                    }
                }
                reader.Close();

                // Handle LFQuestions (Long Form)
                string lfqQuery = @"SELECT QR.response, LF.points 
                            FROM QuizResponses QR
                            JOIN LFQuestions LF ON QR.question_id = LF.question_id
                            WHERE QR.quiz_id = @QuizId AND QR.user_id = @UserId AND LF.quiz_id = @QuizId";
                cmd = new SqlCommand(lfqQuery, con);
                cmd.Parameters.AddWithValue("@QuizId", quizId);
                cmd.Parameters.AddWithValue("@UserId", userId);

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    hasLongFormQuestions = true;
                    int points = Convert.ToInt32(reader["points"]);
                    total += points;  // Add LFQ points to the total
                }

                reader.Close();
                con.Close();
            }

            // Set the score and total to the label
            lblScore.Text = $"Your score: {score} / {total}";

            // If there were LFQuestions, show preliminary score message
            if (hasLongFormQuestions)
            {
                lblPreliminaryMessage.Visible = true;
            }
        }


        protected void btnReturn_Click(object sender, EventArgs e)
        {
            int quizId = (int)Session["QuizID"];
            int moduleId = (int)Session["ModuleID"];
            int userId = (int)Session["UserID"];
            float grade = (total > 0) ? ((float)score / total) * 100 : 0;
            string comments = "Quiz completed";

            // Add a preliminary comment if there were long-form questions
            if (Session["HasLongFormQuestions"] != null && (bool)Session["HasLongFormQuestions"])
            {
                comments = "Preliminary Score. Long Form Questions to be marked.";
            }

            // Insert into the Gradebook table
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                string query = @"INSERT INTO Gradebook (module_id, user_id, grade, comments) 
                         VALUES (@ModuleId, @UserId, @Grade, @Comments)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ModuleId", moduleId);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Grade", grade);
                cmd.Parameters.AddWithValue("@Comments", comments);

                cmd.ExecuteNonQuery();
                con.Close();
            }

            // Redirect to the home or quiz selection page
            Response.Redirect("frmDashboard.aspx");
        }
    }
}
