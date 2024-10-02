using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace SiyafundaApplication
{
    public partial class frmTakeQuiz : System.Web.UI.Page
    {
        // Declare SQL connection
        private SqlConnection Con;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Con = new SqlConnection(getConnectionString());
                LoadQuizData();
            }
        }

        // Connection string method
        protected string getConnectionString()
        {
            return @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\SiyafundaDB.mdf;Integrated Security=True";
        }

        // Load quiz data: title, description, and questions
        protected void LoadQuizData()
        {
            int quizId = Convert.ToInt32(Session["QuizID"]); // Assuming QuizID is stored in Session when the quiz is selected

            try
            {
                using (Con)
                {
                    Con.Open();

                    // Retrieve the quiz details
                    string quizQuery = "SELECT title, description FROM Quizzes WHERE quiz_id = @QuizID";
                    SqlCommand quizCmd = new SqlCommand(quizQuery, Con);
                    quizCmd.Parameters.AddWithValue("@QuizID", quizId);

                    SqlDataReader reader = quizCmd.ExecuteReader();
                    if (reader.Read())
                    {
                        lblQuizTitle.Text = reader["title"].ToString();
                        lblQuizDescription.Text = reader["description"].ToString();
                    }
                    reader.Close();

                    // Retrieve the quiz questions
                    string questionsQuery = @"SELECT question_id, question_text, question_type FROM QuizQuestions WHERE quiz_id = @QuizID";
                    SqlCommand questionsCmd = new SqlCommand(questionsQuery, Con);
                    questionsCmd.Parameters.AddWithValue("@QuizID", quizId);

                    SqlDataAdapter da = new SqlDataAdapter(questionsCmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Prepare question data for Repeater
                    List<object> questionList = new List<object>();
                    foreach (DataRow row in dt.Rows)
                    {
                        string questionType = row["question_type"].ToString();
                        if (questionType == "MCQ")
                        {
                            // Load the multiple-choice options from the database
                            string optionsQuery = "SELECT option_text AS Text, option_id AS Value FROM QuestionOptions WHERE question_id = @QuestionID";
                            SqlCommand optionsCmd = new SqlCommand(optionsQuery, Con);
                            optionsCmd.Parameters.AddWithValue("@QuestionID", row["question_id"]);
                            SqlDataAdapter optionsAdapter = new SqlDataAdapter(optionsCmd);
                            DataTable optionsDt = new DataTable();
                            optionsAdapter.Fill(optionsDt);

                            questionList.Add(new
                            {
                                question_id = row["question_id"],
                                question_text = row["question_text"],
                                question_type = "MCQ",
                                options = optionsDt
                            });
                        }
                        else
                        {
                            questionList.Add(new
                            {
                                question_id = row["question_id"],
                                question_text = row["question_text"],
                                question_type = questionType
                            });
                        }
                    }

                    rptQuestions.DataSource = questionList;
                    rptQuestions.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
                lblMessage.Visible = true;
            }
        }

        // Handle quiz submission
        protected void btnSubmitQuiz_Click(object sender, EventArgs e)
        {
            try
            {
                using (Con)
                {
                    Con.Open();

                    foreach (RepeaterItem item in rptQuestions.Items)
                    {
                        string questionId = ((HiddenField)item.FindControl("hdnQuestionID")).Value;

                        string questionType = GetQuestionType(questionId);

                        string userAnswer = "";
                        if (questionType == "MCQ")
                        {
                            userAnswer = ((RadioButtonList)item.FindControl("rblOptions")).SelectedValue;
                        }
                        else if (questionType == "FillBlank")
                        {
                            userAnswer = ((TextBox)item.FindControl("txtFillBlank")).Text;
                        }
                        else if (questionType == "LongForm")
                        {
                            userAnswer = ((TextBox)item.FindControl("txtLongFormAnswer")).Text;
                        }

                        // Insert response into QuizResponses table
                        string insertResponseQuery = @"INSERT INTO [dbo].[QuizResponses]
                                                       (user_id, question_id, selected_answer, submitted_at)
                                                       VALUES (@UserID, @QuestionID, @Answer, @SubmittedAt)";
                        SqlCommand cmd = new SqlCommand(insertResponseQuery, Con);
                        cmd.Parameters.AddWithValue("@UserID", Session["UserID"]);
                        cmd.Parameters.AddWithValue("@QuestionID", questionId);
                        cmd.Parameters.AddWithValue("@Answer", userAnswer);
                        cmd.Parameters.AddWithValue("@SubmittedAt", DateTime.Now);

                        cmd.ExecuteNonQuery();
                    }

                    lblMessage.Text = "Quiz submitted successfully!";
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
                lblMessage.Visible = true;
            }
        }

        // Helper method to get question type by question ID
        private string GetQuestionType(string questionId)
        {
            string query = "SELECT question_type FROM QuizQuestions WHERE question_id = @QuestionID";
            SqlCommand cmd = new SqlCommand(query, Con);
            cmd.Parameters.AddWithValue("@QuestionID", questionId);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader["question_type"].ToString();
                }
            }
            return string.Empty; // Return an empty string if not found
        }
    }
}