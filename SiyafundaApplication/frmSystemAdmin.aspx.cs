using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SiyafundaApplication
{
    public partial class Admin : System.Web.UI.Page
    {
        protected string getConnectionString()
        {
            return @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\SiyafundaDB.mdf;Integrated Security=True";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is an admin
            if (Convert.ToInt32(Session["RoleID"]) != 2) // 2 = Admin role id
            {
                Response.Redirect("frmLandingPage.aspx"); // Redirect if not admin
            }

            if (!IsPostBack)
            {
                LoadUsers();
            }
        }

        // Load users into GridView
        private void LoadUsers()
        {
            using (SqlConnection conn = new SqlConnection(getConnectionString()))
            {
                string query = @"SELECT u.user_id, u.Name, u.Surname, u.Email, r.role_name
                                 FROM [dbo].[Users] u
                                 INNER JOIN [dbo].[Roles] r ON u.Role_id = r.role_id";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                UsersGridView.DataSource = dt;
                UsersGridView.DataBind();
            }
        }

        // Handle row command (delete user)
        protected void UsersGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteUser")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = UsersGridView.Rows[rowIndex];

                int userId = Convert.ToInt32(row.Cells[0].Text);

                DeleteUser(userId);
            }
        }

        // Delete user from the database
        private void DeleteUser(int userId)
        {
            using (SqlConnection conn = new SqlConnection(getConnectionString()))
            {
                string query = @"DELETE FROM [dbo].[Users] WHERE user_id = @user_id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@user_id", userId);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                LoadUsers(); // Reload users after deletion
            }
        }

        // Handle role change
        protected void RoleDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.NamingContainer;

            int userId = Convert.ToInt32(row.Cells[0].Text);
            int roleId = Convert.ToInt32(ddl.SelectedValue);

            UpdateUserRole(userId, roleId);
        }

        // Update user role in the database
        private void UpdateUserRole(int userId, int roleId)
        {
            using (SqlConnection conn = new SqlConnection(getConnectionString()))
            {
                string query = @"UPDATE [dbo].[Users] SET Role_id = @role_id WHERE user_id = @user_id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@role_id", roleId);
                cmd.Parameters.AddWithValue("@user_id", userId);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                LoadUsers(); // Reload users after updating role
            }
        }
    }
}