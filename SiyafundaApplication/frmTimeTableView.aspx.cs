using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace SiyafundaApplication
{
    public partial class frmTimeTableView : Page
    {
        private SqlConnection Con;

        private string getConnectionString()
        {
            return @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\SiyafundaDB.mdf;Integrated Security=True";
        }

        // Temporary user ID
        private int UserID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Initialize connection
            Con = new SqlConnection(getConnectionString());
            lblErrors.Visible = false;
            if (Session["UserID"] != null && Convert.ToInt32(Session["RoleID"]) != 7) //Only students can create time tables
            {
                UserID = Convert.ToInt32(Session["UserID"]);
            }

            if (!IsPostBack)
            {
                DisplayTimeTable(UserID);
            }
        }

        protected void DisplayTimeTable(int userId)
        {
            try
            {
                Con.Open();
                string sql = "SELECT time_table_id, user_id, module_id, day_id, class_start_time, class_end_time FROM TimeTable WHERE user_id = @UserId";

                using (SqlCommand cmd = new SqlCommand(sql, Con))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        dgvTimeTable.DataSource = dt;
                        dgvTimeTable.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log it or show a message)
                lblErrors.Text = "Error retrieving timetable: " + ex.Message;
                lblErrors.Visible = true;
            }
            finally
            {
                Con.Close();
            }
        }

        protected void btnAddClass_Click(object sender, EventArgs e)
        {
            // Redirect back to frmTimeTableAddClass.aspx
            Response.Redirect("frmTimeTableAddClass.aspx");
        }
    }
}