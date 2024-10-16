using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using SiyafundaApplication;

namespace SiyafundaApplication
{
    public partial class Question
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
    }

    public partial class frmTakeQuiz : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadModules();
            }
        }

        // Load available modules from the database
        private void LoadModules()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {
                    con.Open();
                    string query = "SELECT module_id, title FROM Modules";
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlModules.DataSource = reader;
                    ddlModules.DataTextField = "title";
                    ddlModules.DataValueField = "module_id";
                    ddlModules.DataBind();

                    ddlModules.Items.Insert(0, new ListItem("Select a Module", "0"));

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                lblModuleError.Visible = true;
            }
        }

        // Event when a module is selected, load quizzes for that module
        protected void ddlModules_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlModules.SelectedValue != "0")
            {
                LoadQuizzes();
            }
            else
            {
                ddlQuizzes.Enabled = false;
            }
        }

        // Load quizzes based on the selected module
        private void LoadQuizzes()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {
                    con.Open();

                    string query = "SELECT quiz_id, title FROM Quizzes WHERE module_id = @ModuleId";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ModuleId", ddlModules.SelectedValue);

                    SqlDataReader reader = cmd.ExecuteReader();
                    ddlQuizzes.DataSource = reader;
                    ddlQuizzes.DataTextField = "title";
                    ddlQuizzes.DataValueField = "quiz_id";
                    ddlQuizzes.DataBind();

                    ddlQuizzes.Enabled = true;
                    ddlQuizzes.Items.Insert(0, new ListItem("Select a Quiz", "0"));

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                lblQuizError.Visible = true;
            }
        }

        // Event when a quiz is selected, display the quiz details
        protected void ddlQuizzes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlQuizzes.SelectedValue != "0")
            {
                LoadQuizDetails();
            }
            else
            {
                ClearQuizDetails();
            }
        }

        // Load the quiz details based on the selected quiz
        private void LoadQuizDetails()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {
                    con.Open();
                    string query = "SELECT title, duration, create_date, due_date FROM Quizzes WHERE quiz_id = @QuizId";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@QuizId", ddlQuizzes.SelectedValue);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        lblQuizTitle.Text = reader["title"].ToString();
                        lblDuration.Text = reader["duration"].ToString();
                        lblCreateDate.Text = Convert.ToDateTime(reader["create_date"]).ToString("MM/dd/yyyy");
                        lblDueDate.Text = Convert.ToDateTime(reader["due_date"]).ToString("MM/dd/yyyy  hh:mm tt");

                        // Check if the quiz is still available
                        DateTime dueDate = Convert.ToDateTime(reader["due_date"]);
                        if (dueDate >= DateTime.Now)
                        {
                            btnStartQuiz.Enabled = true;
                            lblWarning.Text = "";
                        }
                        else
                        {
                            btnStartQuiz.Enabled = false;
                            lblWarning.Text = "The quiz is past its due date and cannot be taken.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblWarning.Text = "There was an issue loading the quiz info.";
            }
        }

        // Clear the quiz details if no quiz is selected
        private void ClearQuizDetails()
        {
            lblQuizTitle.Text = "N/A";
            lblDuration.Text = "N/A";
            lblCreateDate.Text = "N/A";
            lblDueDate.Text = "N/A";
            btnStartQuiz.Enabled = false;
            lblWarning.Text = "";
        }

        // This method will start the quiz
        protected void btnStartQuiz_Click(object sender, EventArgs e)
        {
            // Initialize timer based on the quiz duration
            Session["QuizStartTime"] = DateTime.Now;
            Session["QuizDuration"] = Convert.ToInt32(lblDuration.Text);

            // Load all questions for the selected quiz and sort them by type
            List<Question> questions = LoadSortedQuizQuestions(Convert.ToInt32(ddlQuizzes.SelectedValue));

            // Store the sorted questions in session
            Session["QuizQuestions"] = questions;
            Session["CurrentQuestionIndex"] = 0;

            // Redirect to the first question
            RedirectToNextQuestion();
        }

        // Load and sort all questions for the selected quiz (MCQ first, then FBQ, then LFQ)
        private List<Question> LoadSortedQuizQuestions(int quizId)
        {
            List<Question> mcqQuestions = new List<Question>();
            List<Question> fbqQuestions = new List<Question>();
            List<Question> lfqQuestions = new List<Question>();

            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();

                // Load MCQs first
                string mcqQuery = "SELECT question_id, question_text FROM MCQuestions WHERE quiz_id = @QuizId";
                SqlCommand cmd = new SqlCommand(mcqQuery, con);
                cmd.Parameters.AddWithValue("@QuizId", quizId);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    mcqQuestions.Add(new Question
                    {
                        QuestionId = Convert.ToInt32(reader["question_id"]),
                        QuestionText = reader["question_text"].ToString(),
                        QuestionType = "MCQ"
                    });
                }
                reader.Close();

                // Load Fill in the Blank Questions
                string fbqQuery = "SELECT question_id, question_text FROM FBQuestions WHERE quiz_id = @QuizId";
                cmd = new SqlCommand(fbqQuery, con);
                cmd.Parameters.AddWithValue("@QuizId", quizId);

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    fbqQuestions.Add(new Question
                    {
                        QuestionId = Convert.ToInt32(reader["question_id"]),
                        QuestionText = reader["question_text"].ToString(),
                        QuestionType = "FBQ"
                    });
                }
                reader.Close();

                // Load Long Form Questions
                string lfqQuery = "SELECT question_id, question_text FROM LFQuestions WHERE quiz_id = @QuizId";
                cmd = new SqlCommand(lfqQuery, con);
                cmd.Parameters.AddWithValue("@QuizId", quizId);

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lfqQuestions.Add(new Question
                    {
                        QuestionId = Convert.ToInt32(reader["question_id"]),
                        QuestionText = reader["question_text"].ToString(),
                        QuestionType = "LFQ"
                    });
                }
                reader.Close();
                con.Close();
            }

            // Combine all questions in the desired order: MCQ first, then FBQ, then LFQ
            List<Question> sortedQuestions = new List<Question>();
            sortedQuestions.AddRange(mcqQuestions);
            sortedQuestions.AddRange(fbqQuestions);
            sortedQuestions.AddRange(lfqQuestions);

            return sortedQuestions;
        }

        // Redirect to the next question based on the current index
        private void RedirectToNextQuestion()
        {
            int currentQuestionIndex = Convert.ToInt32(Session["CurrentQuestionIndex"]);
            List<Question> questions = (List<Question>)Session["QuizQuestions"];

            if (currentQuestionIndex < questions.Count)
            {
                Question currentQuestion = questions[currentQuestionIndex];
                Session["CurrentQuestionIndex"] = currentQuestionIndex + 1;

                // Redirect based on the question type to specific forms
                if (currentQuestion.QuestionType == "MCQ")
                {
                    Response.Redirect("frmMCAnswer.aspx?questionId=" + currentQuestion.QuestionId);
                }
                else if (currentQuestion.QuestionType == "FBQ")
                {
                    Response.Redirect("frmFBAnswer.aspx?questionId=" + currentQuestion.QuestionId);
                }
                else if (currentQuestion.QuestionType == "LFQ")
                {
                    Response.Redirect("frmLFAnswer.aspx?questionId=" + currentQuestion.QuestionId);
                }
            }
            else
            {
                // All questions answered, handle quiz completion
                Response.Redirect("frmQuizCompleted.aspx");
            }
        }

        // Helper method to get the connection string
        private string GetConnectionString()
        {
            return @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\SiyafundaDB.mdf;Integrated Security=True";
        }
    }
}
