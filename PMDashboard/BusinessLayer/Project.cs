using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace PMDashboard.BusinessLayer
{
    /// <summary>
    /// Project class of PM Dashboard project.
    /// </summary>
    /// <Developer>Mayank Gaur</Developer>
    /// <DateCreated>June 4, 2012</DateCreated>
    public class Project
    {
        /// <summary>
        /// This method is used to get data for PM Dashboard.
        /// </summary>
        ///<param name="UserName">string</param>
        /// <returns>Data Set</returns>
        /// <Developer>Mayank Gaur</Developer>
        /// <DateCreated>June 4, 2012</DateCreated>
        public DataSet GetDashboardData(string UserName, int ProjectID)
        {
            SqlCommand com = null;
            DataSet ds = null;
            SqlDataAdapter da = null;
            string Connectionstring = string.Empty;
            try
            {
                Connectionstring = ConfigurationManager.ConnectionStrings["FBAll"].ToString();
                using (SqlConnection con = new SqlConnection(Connectionstring))
                {
                    // command object
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "USP_GetDashboardData";
                    com.Parameters.AddWithValue("UserName", UserName);
                    com.Parameters.AddWithValue("ProjectID", ProjectID);
                    ds = new DataSet();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                // dispose all objects
                com = null;
                da = null;
            }
            
            return ds;
        }

        /// <summary>
        /// This method is used to get data for PM Dashboard.
        /// </summary>
        ///<param name="UserName">string</param>
        /// <returns>Data Set</returns>
        /// <Developer>Mayank Gaur</Developer>
        /// <DateCreated>June 4, 2012</DateCreated>
        public DataSet GetEmployeeList()
        {
            SqlCommand com = null;
            DataSet ds = null;
            SqlDataAdapter da = null;
            string Connectionstring = string.Empty;
            try
            {
                Connectionstring = ConfigurationManager.ConnectionStrings["FBAll"].ToString();
                using (SqlConnection con = new SqlConnection(Connectionstring))
                {
                    // command object
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "USP_GetEmployeeList";
                    ds = new DataSet();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                // dispose all objects
                com = null;
                da = null;
            }

            return ds;
        }

    }
}