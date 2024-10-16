using System;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace SiyafundaApplication
{
    public partial class frmCreateQuiz : System.Web.UI.Page
    {
        // Connection string to the database
        protected string getConnectionString()
        {
            return @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\SiyafundaDB.mdf;Integrated Security=True";
        }

        // Declare SQL connection
        private SqlConnection Con;

        protected void Page_Load(object sender, EventArgs e)
        {
            Con = new SqlConnection(getConnectionString());

            if (!IsPostBack)
            {
                // Initialize question count if it's not already set
                if (Session["QuestionsSubmitted"] == null)
                {
                    Session["QuestionsSubmitted"] = 0;
                }

                // Update the label with the current question count
                lblQuestionsSubmitted.Text = Session["QuestionsSubmitted"] + " questions submitted.";

                // Populate the module dropdown list
                LoadModules();
            }
        }

        // Populate the module dropdown
        protected void LoadModules()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(getConnectionString()))
                {
                    con.Open();
                    string query = "SELECT module_id FROM Modules";
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Clear any existing items before adding
                    ddlModules.Items.Clear();

                    // Add a default "Select Module" option
                    ddlModules.Items.Insert(0, new ListItem("Select Module", "0"));

                    // Iterate over the data and add it to the dropdown
                    while (reader.Read())
                    {
                        ListItem newItem = new ListItem(reader["module_id"].ToString());
                        ddlModules.Items.Add(newItem);
                    }

                    reader.Close();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading modules: " + ex.Message;
                lblMessage.Visible = true;
            }
        }

        protected void btnSubmitQuiz_Click(object sender, EventArgs e)
        {
            string quizTitle = txtQuizTitle.Text.Trim();
            int duration;
            int moduleId = Convert.ToInt32(ddlModules.SelectedValue);

            // Validate module selection
            if (moduleId == 0)
            {
                lblMessage.Text = "Please select a module.";
                lblMessage.Visible = true;
                return;
            }

            // Validate the input for the duration
            if (!int.TryParse(txtDuration.Text, out duration) || duration <= 0)
            {
                lblMessage.Text = "Please enter a valid time limit in minutes.";
                lblMessage.Visible = true;
                return;
            }

            // Validate the input for the due date
            DateTime dueDate;

            if (!DateTime.TryParse(txtDueDate.Text, out dueDate))
            {
                lblMessage.Text = "Please enter a valid due date and time in the format MM/dd/yyyy hh:mm tt.";
                lblMessage.Visible = true;
                return;
            }

            if (dueDate <= DateTime.Now)
            {
                lblMessage.Text = "Please select a due date that is after the current date and time.";
                lblMessage.Visible = true;
                return;
            }

            // Insert quiz into the database with the provided duration and module
            try
            {
                using (SqlConnection con = new SqlConnection(getConnectionString()))
                {
                    con.Open();

                    string insertQuizQuery = @"INSERT INTO [dbo].[Quizzes] 
                                                (module_id, title, duration, create_date, due_date) 
                                               VALUES 
                                                (@ModuleId, @Title, @Duration, @CreateDate, @DueDate);";

                    SqlCommand cmd = new SqlCommand(insertQuizQuery, con);
                    cmd.Parameters.AddWithValue("@ModuleId", moduleId);
                    cmd.Parameters.AddWithValue("@Title", quizTitle);
                    cmd.Parameters.AddWithValue("@Duration", duration);
                    cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@DueDate", dueDate);

                    cmd.ExecuteNonQuery();

                    lblMessage.Text = "Quiz created successfully!";
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Visible = true;

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
                lblMessage.Visible = true;
            }
        }

        // Button Click Event Handlers for Adding Questions
        protected void btnAddMCQ_Click(object sender, EventArgs e)
        {
            // Redirect to the MCQ page
            Response.Redirect("frmAddMCQ.aspx?quizId=" + Session["QuizID"]);
        }

        protected void btnAddFillBlank_Click(object sender, EventArgs e)
        {
            // Redirect to the Fill in the Blank page
            Response.Redirect("frmAddFillBlank.aspx?quizId=" + Session["QuizID"]);
        }

        protected void btnAddLongForm_Click(object sender, EventArgs e)
        {
            // Redirect to the Long Form page
            Response.Redirect("frmAddLongForm.aspx?quizId=" + Session["QuizID"]);
        }

        // Update the question count after successfully submitting a question
        public void IncrementQuestionsSubmitted()
        {
            int questionCount = Convert.ToInt32(Session["QuestionsSubmitted"]);
            Session["QuestionsSubmitted"] = questionCount + 1;
        }
    }
}
