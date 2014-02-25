using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using PMDashboard.ModelLayer;

namespace PMDashboard.BusinessLayer
{
    public class BLTask
    {
        /// <summary>
        /// This method is used to insert task details into database.
        /// </summary>
        ///<param name="objMLTask">MLTask</param>
        /// <returns>int</returns>>
        /// <Developer>Mayank Gaur</Developer>
        /// <DateCreated>June 13, 2013</DateCreated>
        public int InsertTask(MLTask objMLTask)
        {
            SqlCommand com = null;
            int result = 0;
            string Connectionstring = string.Empty;
            try
            {
                Connectionstring = ConfigurationManager.ConnectionStrings["FBAll"].ToString();
                using (SqlConnection con = new SqlConnection(Connectionstring))
                {
                    // command object
                    com = new SqlCommand();
                    com.Connection = con;
                    con.Open();
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "USP_InsertOrUpdateTask";
                    com.Parameters.AddWithValue("TaskID", objMLTask.TaskID);

                    com.Parameters.AddWithValue("FileID", objMLTask.FileID);
                    com.Parameters.AddWithValue("Name", objMLTask.Name);
                    com.Parameters.AddWithValue("Description", objMLTask.Description);
                    com.Parameters.AddWithValue("StartDate", objMLTask.StartDate);
                    com.Parameters.AddWithValue("EndDate", objMLTask.EndDate);
                    com.Parameters.AddWithValue("DateCreated", objMLTask.DateCreated);

                    com.Parameters.AddWithValue("DateModified", objMLTask.DateModified);
                    com.Parameters.AddWithValue("AssignedTo", objMLTask.AssignedTo);

                    com.Parameters.AddWithValue("WorkCompletedOnTask", objMLTask.WorkCompletedOnTask);
                    com.Parameters.AddWithValue("WorkPendingOnTask", objMLTask.WorkPendingOnTask);
                    com.Parameters.AddWithValue("Status", objMLTask.Status);

                   // result = com.ExecuteNonQuery();
                    result = (int)com.ExecuteScalar();
                    con.Close();
                    con.Dispose();
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
                objMLTask = null;                
            }

            return result;
        }

        /// <summary>
        /// This method is used to insert task details into database.
        /// </summary>
        ///<param name="objMLTask">MLTask</param>
        /// <returns>int</returns>>
        /// <Developer>Mayank Gaur</Developer>
        /// <DateCreated>June 13, 2013</DateCreated>
        public int AuditTaskDetails(MLTask objMLTask)
        {
            SqlCommand com = null;
            int result = 0;
            string Connectionstring = string.Empty;
            try
            {
                Connectionstring = ConfigurationManager.ConnectionStrings["FBAll"].ToString();
                using (SqlConnection con = new SqlConnection(Connectionstring))
                {
                    // command object
                    com = new SqlCommand();
                    com.Connection = con;
                    con.Open();
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "USP_AuditTaskDetails";
                    com.Parameters.AddWithValue("TaskID", objMLTask.TaskID);

                    com.Parameters.AddWithValue("StartTime", objMLTask.StartTime);
                    com.Parameters.AddWithValue("EndTime", objMLTask.EndTime);
                    
                   // result = com.ExecuteNonQuery();
                    result = (int)com.ExecuteScalar();
                    con.Close();
                    con.Dispose();
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
                objMLTask = null;                
            }

            return result;
        }

        /// <summary>
        /// This method is used to insert task details into database.
        /// </summary>
        ///<param name="objMLTask">MLTask</param>
        /// <returns>int</returns>>
        /// <Developer>Mayank Gaur</Developer>
        /// <DateCreated>June 13, 2013</DateCreated>
        public DataTable GetOverallTime(int TaskID)
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
                    con.Open();
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "USP_GetOverallTime";
                    com.Parameters.AddWithValue("TaskID",TaskID);

                    ds = new DataSet();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);

                    con.Close();
                    con.Dispose();
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

            return ds.Tables[0];
        }
    }
}