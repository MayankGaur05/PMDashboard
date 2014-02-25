using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMDashboard.BusinessLayer;
using PMDashboard.ModelLayer;

namespace PMDashboard
{
    public partial class ProjectDetails : FileBound.Web.FBBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["FileID"] != null)
                {
                    int fileID = Convert.ToInt32(Request.QueryString["FileID"].ToString());
                    PopulateTaskGrid(fileID);
                }
                PopulateDropdowns();
            }
        }

        #region Button_Event
        /// <summary>
        /// This method is used to handle click event of add new task button
        /// </summary>
        ///<param name="sender">object</param>
        ///<param name="e">Event argument</param>
        /// <returns>void</returns>
        /// <Developer>Mayank Gaur</Developer>
        /// <DateCreated>June 13, 2012</DateCreated>
        protected void btnAddTask_Click(object sender, EventArgs e)
        {
            // clear all fields
            ClearFields();
            divTime.Visible = false;
            divGrid.Visible = false;
            divTask.Visible = true;
            divCombo.Visible = false;
        }
        /// <summary>
        /// This method is used to handle click event of add new task button
        /// </summary>
        ///<param name="sender">object</param>
        ///<param name="e">Event argument</param>
        /// <returns>void</returns>
        /// <Developer>Mayank Gaur</Developer>
        /// <DateCreated>June 13, 2012</DateCreated>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveTask();
        }

        protected void btnStart_Click(object sender, EventArgs e)
        {
            if (btnStart.Text == "Start")
            {
                btnStart.Text = "Stop";
                Timer1.Enabled = true;
                ViewState.Add("StartTime", DateTime.Now);
                CalculateWholeTime(Convert.ToInt32(hiddTaskID.Value));
            }
            else
            {
                btnStart.Text = "Start";
                Timer1.Enabled = false;
                //AuditTaskDetails();
                SaveTask();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            divGrid.Visible = true;
            divTask.Visible = false;
            divTime.Visible = false;
            divCombo.Visible = true;
        }
        #endregion

        #region PrivateMethods

        /// <summary>
        /// This method is used to save task in database.
        /// </summary>
        ///<param name="UserName">string</param>
        /// <returns>void</returns>
        /// <Developer>Mayank Gaur</Developer>
        /// <DateCreated>June 4, 2012</DateCreated>
        private void SaveTask()
        {
            BLTask objBLTask = new BLTask();
            MLTask objMLTask = new MLTask();
            try
            {
                objMLTask.TaskID = string.IsNullOrEmpty(hiddTaskID.Value) ? 0 : Convert.ToInt32(hiddTaskID.Value);
                objMLTask.FileID = Convert.ToInt32(Request.QueryString["FileID"].ToString());
                objMLTask.Name = txtTaskNm.Text;
                objMLTask.Description = txtTaskDesc.Text;
                objMLTask.StartDate = Convert.ToDateTime(txtStartDate.Text);
                objMLTask.EndDate = Convert.ToDateTime(txtEndDate.Text);
                objMLTask.AssignedTo = ddlAssignTo.SelectedIndex >= -1 ? ddlAssignTo.SelectedItem.Text : string.Empty;
                objMLTask.DateCreated = DateTime.Now;
                objMLTask.DateModified = DateTime.Now;
                if (!string.IsNullOrEmpty(hiddTaskID.Value))
                {
                    objMLTask.Status = rbtnComplete.Checked && !rbtnPending.Checked ? 2 : 1;
                }
                else
                {
                    objMLTask.Status = 0;
                }
                objMLTask.WorkCompletedOnTask = txtTaskPer.Text;
                objMLTask.WorkPendingOnTask = txtTaskLeft.Text;
                int i = objBLTask.InsertTask(objMLTask);
                if (i > 0)
                {
                    // show success message
                    if (string.IsNullOrEmpty(hiddTaskID.Value))
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "alert('Task has been created sucessfully.')", true);
                    }
                    else
                    {
                        //Audit task details
                        AuditTaskDetails();
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "alert('Task has been updated sucessfully.')", true);
                    }
                    // clear fields
                    ClearFields();
                    // show grid and hide task div
                    divGrid.Visible = true;
                    divCombo.Visible = true;
                    divTask.Visible = false;
                    // Refresh task grid
                    PopulateTaskGrid(Convert.ToInt32(Request.QueryString["FileID"].ToString()));
                }
                else
                {
                    // show error message
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", "alert('An error has occured.Please try again.')", true);
                }
            }
            catch (Exception ex)
            {
                // show error message
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("alert('{0}')", ex.Message), true);
            }
            finally
            {
                objBLTask = null;
                objMLTask = null;
                hiddTaskID.Value = string.Empty;
            }
        }
        /// <summary>
        /// This method is used to get data for PM Dashboard.
        /// </summary>
        ///<param name="UserName">string</param>
        /// <returns>void</returns>
        /// <Developer>Mayank Gaur</Developer>
        /// <DateCreated>June 4, 2012</DateCreated>
        private void PopulateTaskGrid(int fileID)
        {
            DataSet dsData = null;
            Project objProeject = new Project();
            DataTable dtTask = new DataTable();
            DataRow[] drTask = null;
            try
            {
                string user = FBBusiness.LoggedInUser.Name;
                int ProjectID = State.CurrentProjectID;
                dsData = objProeject.GetDashboardData(user, ProjectID);
                if (dsData != null && dsData.Tables.Count > 0)
                {
                    if (Request.QueryString["taskID"] == null)
                    {
                        drTask = dsData.Tables[0].Select("FileID = '" + fileID + "'");
                    }
                    else
                    {
                        drTask = dsData.Tables[0].Select("FileID = '" + fileID + "' and TaskID = '" + Request.QueryString["taskID"].ToString() + "'");
                    }
                    // bind grid view 
                    if (drTask != null && drTask.Length > 0)
                    {
                        ViewState.Add("Task", drTask.CopyToDataTable());
                        gvTask.DataSource = drTask.CopyToDataTable();
                    }
                    else
                    {
                        gvTask.DataSource = null;
                    }
                    gvTask.DataBind();

                    if (dsData.Tables[2].Select("FileID = '" + Convert.ToInt32(Request.QueryString["FileID"].ToString()) + "'") != null)
                    {

                        if (dsData.Tables[2].Select("FileID = '" + Convert.ToInt32(Request.QueryString["FileID"].ToString()) + "'")[0]["type"].ToString() == "Project")
                        {
                            lblProjectName.Text = dsData.Tables[2].Select("FileID = '" + Convert.ToInt32(Request.QueryString["FileID"].ToString()) + "'")[0]["ProjectName"].ToString();
                            lblType.Text = "Project Name" + ":";
                        }
                        else
                        {
                            lblProjectName.Text = dsData.Tables[2].Select("FileID = '" + Convert.ToInt32(Request.QueryString["FileID"].ToString()) + "'")[0]["TicketName"].ToString();
                            lblType.Text = "Ticket Name" + ":";
                        }
                    }
                    // if user select a task
                    if (Request.QueryString["TaskID"] != null)
                    {
                        // show task details
                        divTask.Visible = true;
                        divTime.Visible = true;
                        hiddTaskID.Value = Request.QueryString["taskID"].ToString();
                        // populate controls with previous data
                        PopulateControls(Convert.ToInt32(Request.QueryString["taskID"].ToString()));
                        CalculateWholeTime(Convert.ToInt32(Request.QueryString["taskID"].ToString()));
                        string totalTime = ViewState["TotalTime"] != null ? ViewState["TotalTime"].ToString() : "00:00:00";
                        TimeSpan total = (TimeSpan.Parse(totalTime));
                        lblOverallTime.Text = total.ToString(@"dd\:hh\:mm\:ss");
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", string.Format("alert{0}", ex.Message));
            }
        }

        /// <summary>
        /// This method is used to populate dropdowns.
        /// </summary>
        /// <returns>void</returns>
        /// <Developer>Mayank Gaur</Developer>
        /// <DateCreated>June 13, 2013</DateCreated>
        private void PopulateDropdowns()
        {
            DataSet dsData = null;
            Project objProeject = new Project();
            ListItem lstAll = null;
            try
            {
                // later change to pass parameter dynamically
                dsData = objProeject.GetEmployeeList();
                if (dsData != null && dsData.Tables.Count > 0)
                {
                    // Bind Assign TO drop down
                    ddlAssignTo.DataSource = dsData.Tables[0];
                    ddlAssignTo.DataTextField = "FullName";
                    ddlAssignTo.DataValueField = "FullName";
                    ddlAssignTo.DataBind();

                    ddlAssigned.DataSource = dsData.Tables[0];
                    ddlAssigned.DataTextField = "FullName";
                    ddlAssigned.DataValueField = "FullName";
                    ddlAssigned.DataBind();

                    lstAll = new ListItem("All", "0");
                    ddlAssigned.Items.Insert(0, lstAll);
                    lstAll = new ListItem("---Select---", "---Select---");
                    ddlAssignTo.Items.Insert(0, lstAll);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", string.Format("alert('{0}')", ex.Message));
            }
            finally
            {
                dsData = null;
                objProeject = null;
                lstAll = null;
            }
        }

        /// <summary>
        /// This method is used to populate dropdowns.
        /// </summary>
        /// <returns>void</returns>
        /// <Developer>Mayank Gaur</Developer>
        /// <DateCreated>June 13, 2013</DateCreated>
        private void ClearFields()
        {
            txtEndDate.Text = string.Empty;
            txtStartDate.Text = string.Empty;
            txtTaskDesc.Text = string.Empty;
            txtTaskLeft.Text = string.Empty;
            txtTaskNm.Text = string.Empty;
            txtTaskPer.Text = string.Empty;
            ddlAssignTo.SelectedIndex = 0;
            lblOverallTime.Text = string.Empty;
            lblTotalTimeToday.Text = string.Empty;
            hiddTaskID.Value = string.Empty;
            Timer1.Enabled = false;
            ViewState["TotalTime"] = null;
        }

        /// <summary>
        /// This method is used to populate controls.
        /// </summary>
        /// <returns>void</returns>
        /// <Developer>Mayank Gaur</Developer>
        /// <DateCreated>June 14, 2013</DateCreated>
        private void PopulateControls(int TaskID)
        {
            DataTable dtTask = ViewState["Task"] != null ? ViewState["Task"] as DataTable : null;
            DataRow[] drTask = null;
            try
            {
                // check data table is not null
                if (dtTask != null && dtTask.Rows.Count > 0)
                {
                    drTask = dtTask.Select("TaskID ='" + TaskID + "'");
                    if (dtTask != null && drTask.Length > 0)
                    {
                        txtEndDate.Text = drTask[0]["TaskEndDate"].ToString();
                        txtStartDate.Text = drTask[0]["TaskStartDate"].ToString();
                        txtTaskDesc.Text = drTask[0]["Description"].ToString();
                        txtTaskLeft.Text = drTask[0]["WorkPendingOnTask"].ToString();
                        txtTaskNm.Text = drTask[0]["Name"].ToString();
                        txtTaskPer.Text = drTask[0]["WorkCompletedOnTask"].ToString();
                        if (drTask[0]["Status"].ToString() == "In Process")
                        {
                            rbtnPending.Checked = true;
                            rbtnComplete.Checked = false;
                        }
                        else
                        {
                            rbtnComplete.Checked = true;
                            rbtnPending.Checked = false;
                        }
                        ddlAssignTo.SelectedValue = drTask[0]["AssignedTo"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", string.Format("alert{0}", ex.Message));
            }
        }

        /// <summary>
        /// This method is used to calculate whole time for a task.
        /// </summary>
        /// <returns>void</returns>
        /// <Developer>Mayank Gaur</Developer>
        /// <DateCreated>June 17, 2013</DateCreated>
        private void CalculateWholeTime(int TaskID)
        {
            BLTask objBLTask = new BLTask();
            DataTable dtTask = null;
            string startTime = string.Empty;
            TimeSpan start = new TimeSpan(0, 0, 0);
            string time = string.Empty;
            try
            {
                dtTask = objBLTask.GetOverallTime(TaskID);
                // check data table is not null
                if (dtTask != null && dtTask.Rows.Count > 0)
                {
                    for (int rowCount = 0; rowCount < dtTask.Rows.Count; rowCount++)
                    {
                        startTime = dtTask.Rows[rowCount]["Duration"].ToString();
                        if (!string.IsNullOrEmpty(startTime))
                        {
                            time = Convert.ToDateTime(startTime).Hour.ToString() + ":" + Convert.ToDateTime(startTime).Minute.ToString() + ":" + Convert.ToDateTime(startTime).Second.ToString();
                            start += TimeSpan.Parse(time);
                        }
                    }
                    ViewState.Add("TotalTime", start.ToString());
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                objBLTask = null;
                dtTask = null;
            }
        }

        private void AuditTaskDetails()
        {
            BLTask objBLTask = new BLTask();
            MLTask objMLTask = new MLTask();
            try
            {
                objMLTask.TaskID = string.IsNullOrEmpty(hiddTaskID.Value) ? 0 : Convert.ToInt32(hiddTaskID.Value);
                objMLTask.StartTime = Convert.ToDateTime(ViewState["StartTime"].ToString());
                objMLTask.EndTime = Convert.ToDateTime(ViewState["StartTime"].ToString()).Add(TimeSpan.Parse(lblTotalTimeToday.Text));
                int i = objBLTask.AuditTaskDetails(objMLTask);
                if (i > 0)
                {
                    // show success message
                    if (string.IsNullOrEmpty(hiddTaskID.Value))
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "alert('Task has been created sucessfully.')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert", "alert('Task has been updated sucessfully.')", true);
                    }
                    // clear fields
                    ClearFields();
                    // show grid and hide task div
                    divGrid.Visible = true;
                    divTask.Visible = false;
                    // Refresh task grid
                    PopulateTaskGrid(Convert.ToInt32(Request.QueryString["FileID"].ToString()));
                }
                else
                {
                    // show error message
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", "alert('An error has occured.Please try again.')", true);
                }
            }
            catch (Exception ex)
            {
                // show error message
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("alert('{0}')", ex.Message), true);
            }
            finally
            {
                objBLTask = null;
                objMLTask = null;
                hiddTaskID.Value = string.Empty;
            }
        }

        #endregion

        #region GridEvents
        /// <summary>
        /// This method is used to handle grid view row command event.
        /// </summary>
        ///<param name="sender">object</param>
        ///<param name="e">GridViewCommandEventArgs</param>
        /// <returns>void</returns>
        /// <Developer>Mayank Gaur</Developer>
        /// <DateCreated>June 14, 2013</DateCreated>
        protected void gvTask_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "OpenEdit")
            {
                ViewState["TotalTime"] = null;
                divTask.Visible = true;
                divTime.Visible = true;
                hiddTaskID.Value = e.CommandArgument.ToString();
                // populate controls with previous data
                PopulateControls(Convert.ToInt32(e.CommandArgument.ToString()));

                CalculateWholeTime(Convert.ToInt32(e.CommandArgument.ToString()));
                string totalTime = ViewState["TotalTime"] != null ? ViewState["TotalTime"].ToString() : "00:00:00";
                TimeSpan total = (TimeSpan.Parse(totalTime));
                lblOverallTime.Text = total.ToString(@"dd\:hh\:mm\:ss");
            }
            else if (e.CommandName == "ViewDocument")
            {
                //ModalPopupExtender1.Show();
                string URL = Request.Url.AbsoluteUri.ToString();
                if (URL.Contains("Plugins"))
                {
                    int index = URL.IndexOf("Plugins");
                    URL = URL.Remove(index);
                    URL = URL + "FileDetail.aspx?FileID=" + e.CommandArgument.ToString();
                    //ifreameFileDetails.Attributes.Add("src", URL);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "ViewDocumnet", "window.open('" + URL + "','_blank','width=800,height=800,scrollbars=yes');", true);
                }
                //URL = "~/FileDetail.aspx?FileID=" + e.CommandArgument.ToString();

                //Response.Redirect("~/FileDetail.aspx?FileID=" + e.CommandArgument.ToString());
            }
        }
        #endregion

        #region TimerEvent
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            string totalTime = ViewState["TotalTime"] != null ? ViewState["TotalTime"].ToString() : "00:00:00";

            if (ViewState["StartTime"] != null)
            {
                DateTime toDate = Convert.ToDateTime(ViewState["StartTime"].ToString());

                DateTime fromDate = DateTime.Now;

                TimeSpan difference = fromDate.Subtract(toDate);
                lblTotalTimeToday.Text = difference.ToString(@"dd\:hh\:mm\:ss");
                // difference.Hours + ":" + difference.Minutes + ":" + difference.Seconds;
                TimeSpan total = difference.Add(TimeSpan.Parse(totalTime));
                lblOverallTime.Text = total.ToString(@"dd\:hh\:mm\:ss"); //total.Hours +":" + total.Minutes +":" +total.Seconds;
            }
        }
        #endregion

        #region DropdownEvents
        protected void ddlAssigned_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewState["Task"] != null)
            {
                DataTable dtTask = ViewState["Task"] as DataTable;
                DataRow[] drTask = null;
                if (ddlAssigned.SelectedIndex > 0)
                {
                    if (ddlStatus.SelectedIndex == 0)
                    {
                        drTask = dtTask.Select("AssignedTo = '" + ddlAssigned.SelectedItem.Text + "'");
                    }
                    else
                    {
                        drTask = dtTask.Select("AssignedTo = '" + ddlAssigned.SelectedItem.Text + "' and status = '" + ddlStatus.SelectedItem.Text + "'");
                    }

                    if (drTask != null && drTask.Length > 0)
                    {
                        gvTask.DataSource = drTask.CopyToDataTable();
                    }
                    else
                    {
                        gvTask.DataSource = null;
                    }

                }
                else
                {
                    if (ddlStatus.SelectedIndex > 0)
                    {
                        drTask = dtTask.Select("Status = '" + ddlStatus.SelectedItem.Text + "'");
                        gvTask.DataSource = drTask != null && drTask.Length > 0 ? drTask.CopyToDataTable() : null;
                    }
                    else
                    {
                        gvTask.DataSource = dtTask;
                    }
                }
                gvTask.DataBind();
            }
        }

        protected void dlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewState["Task"] != null)
            {
                DataTable dtTask = ViewState["Task"] as DataTable;
                if (ddlStatus.SelectedIndex > 0)
                {
                    DataRow[] drTask = dtTask.Select("Status = '" + ddlStatus.SelectedItem.Text + "'");

                    if (drTask != null && drTask.Length > 0)
                    {
                        if (ddlAssigned.SelectedIndex > 0)
                        {
                            drTask = dtTask.Select("AssignedTo = '" + ddlAssigned.SelectedItem.Text + "' and status = '" + ddlStatus.SelectedItem.Text + "'");
                        }
                        gvTask.DataSource = drTask != null && drTask.Length > 0 ? drTask.CopyToDataTable() : null;
                    }
                    else
                    {
                        gvTask.DataSource = null;
                    }

                }
                else
                {
                    if (ddlAssigned.SelectedIndex > 0)
                    {
                        DataRow[] drTask = dtTask.Select("AssignedTo = '" + ddlAssigned.SelectedItem.Text + "'");
                        gvTask.DataSource = drTask != null && drTask.Length > 0 ? drTask.CopyToDataTable() : null;
                    }
                    else
                    {
                        gvTask.DataSource = dtTask;
                    }
                }
                gvTask.DataBind();
            }
        }
        #endregion
    }
}