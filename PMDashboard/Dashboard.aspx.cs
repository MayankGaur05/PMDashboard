using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Collections.Generic;

namespace PMDashboard
{
    /// <summary>
    /// Dashboard page of PM Dashboard project.
    /// </summary>
    /// <Developer>Mayank Gaur</Developer>
    /// <DateCreated>June 4, 2012</DateCreated>
    public partial class Dashboard :  FileBound.Web.FBBasePage
    {
        #region Global Variables
        private const string DASHBOARD = "Dashboard";
        
        #endregion

        /// <summary>
        /// Page load event of Dashboard page.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        /// <Developer>Mayank Gaur</Developer>
        /// <DateCreated>June 4, 2012</DateCreated>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Page is loading first time
            if (!IsPostBack)
            {
                // Adding post back event reference to button
                Page.GetPostBackEventReference(btnDefault);
                // get dash board data
                GetDashboardData();
            }
        }

        /// <summary>
        /// This method is used to get data for PM Dashboard.
        /// </summary>
        ///<param name="UserName">string</param>
        /// <returns>void</returns>
        /// <Developer>Mayank Gaur</Developer>
        /// <DateCreated>June 4, 2012</DateCreated>
        private void GetDashboardData()
        {
            DataSet dsData = null;
            PMDashboard.BusinessLayer.Project objProeject = new PMDashboard.BusinessLayer.Project();
            try
            {
                // later change to pass parameter dynamically
                string user = FBBusiness.LoggedInUser.Name;
                int ProjectID = State.CurrentProjectID;
                if (string.IsNullOrEmpty(user))
                {
                    FormsAuthentication.RedirectToLoginPage();
                }
                else if (ProjectID == 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "NoProject", "alert('Please select a project')", true);
                }
                else
                {
                    dsData = objProeject.GetDashboardData(user, ProjectID);
                    if (dsData != null && dsData.Tables.Count > 0)
                    {
                        // Adding data set to view state for future reference
                        ViewState.Add(DASHBOARD, dsData);
                        // bind employee's details
                        lblEmpMail.Text = dsData.Tables[1].Rows[0]["Email"].ToString();
                        lblEmpName.Text = dsData.Tables[1].Rows[0]["Contact"].ToString();
                        lblEmpPhone.Text = dsData.Tables[1].Rows[0]["Phone"].ToString();
                        if (!string.IsNullOrEmpty(dsData.Tables[1].Rows[0]["Image"].ToString()))
                        {
                            imgEmp.ImageUrl = "EmployessImages/" + dsData.Tables[1].Rows[0]["Image"].ToString();
                        }
                        // bind grid view 
                        gvProject.DataSource = dsData.Tables[2].Select("Type = '" + "Project" + "'") != null ? dsData.Tables[2].Select("Type = '" + "Project" + "' and ProjectName <> '" + string.Empty + "'").CopyToDataTable() : null;
                        gvProject.DataBind();

                        gvTicket.DataSource = dsData.Tables[2].Select("Type = '" + "Ticket" + "'") != null ? dsData.Tables[2].Select("Type = '" + "Ticket" + "' and TicketName <> '" + string.Empty + "'").CopyToDataTable() : null;
                        gvTicket.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", string.Format("alert{0}", ex.Message));
            }
        }

        protected void calProject_DayRender(object sender, DayRenderEventArgs e)
        {
            DateTime nextDate;
            DataSet dsPorject = ViewState[DASHBOARD] as DataSet;
            DataRow[] drProject = null;
            DataRow[] drTask = null;
            string companyName = string.Empty;
            if (dsPorject != null)
            {
                // filter task for projects only
                drProject = dsPorject.Tables[2].Select("Type = '" + "Project" + "'");
                if (drProject != null && drProject.Length > 0)
                {
                    for (int count = 0; count < drProject.Length; count++)
                    {
                        drTask = dsPorject.Tables[0].Select("FileID = '" + drProject[count]["FileID"].ToString() + "'");
                        if (drTask != null && drTask.Length > 0)
                        {

                            foreach (DataRow dr in drTask)
                            {
                                nextDate = (DateTime)dr["StartDate"];
                                if (nextDate.Date == e.Day.Date)
                                {
                                    String url = e.SelectUrl;
                                    e.Cell.Controls.Clear();
                                    HyperLink link = new HyperLink();
                                    link.ID = "link" + e.Day.Date.Day.ToString();
                                    link.Text = e.Day.Date.Day.ToString();
                                    link.ToolTip = companyName;
                                    link.ForeColor = System.Drawing.Color.White;
                                    link.Font.Bold = true;
                                    DataRow[] drStartDate = drTask.CopyToDataTable().Select("StartDate = '" + e.Day.Date + "'");
                                    if (drStartDate != null && drStartDate.Length > 1)
                                    {
                                        ModalPopupExtender1.Show();
                                        // Attaching java script function to link 
                                        link.Attributes.Add("onClick", "javascript:GetClick('" + e.Day.Date +"|Project" + "');");
                                    }
                                    else
                                    {
                                        ModalPopupExtender1.Hide();
                                        companyName = dsPorject.Tables[2].Select("FileID = '" + dr["FileID"].ToString() + "'")[0]["CompanyName"].ToString();
                                        e.Cell.ToolTip = companyName;
                                        link.NavigateUrl = "TaskDetails.aspx?FileID=" + dr["FileID"].ToString() +"&TaskID=" +dr["TaskID"].ToString();
                                    }

                                    e.Cell.Controls.Add(link);
                                    // if status is in open condition
                                    if (dr["status"].ToString() == "Open")
                                    {
                                        e.Cell.BackColor = System.Drawing.Color.Red;
                                    }
                                    // if status is in running condition
                                    else if (dr["status"].ToString() == "In Process")
                                    {
                                        e.Cell.BackColor = System.Drawing.Color.Lime;
                                    }
                                    // if status is in close condition
                                    else if (dr["status"].ToString() == "Close")
                                    {
                                        e.Cell.BackColor = System.Drawing.Color.Green;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void gvProject_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string cmdArgs = e.CommandArgument.ToString();
            int fileID = 0;
            int taskID = 0;
            if (cmdArgs.Contains(':'))
            {
                string[] strCmdArgs = cmdArgs.Split(':');
                fileID = Convert.ToInt32(strCmdArgs[0]);
                taskID = Convert.ToInt32(strCmdArgs[1]);
            }
            else
            {
                fileID = Convert.ToInt32(e.CommandArgument.ToString());
            }
            DataSet dsDashboard = ViewState[DASHBOARD] as DataSet;
            DataRow[] drDashboard = null;
            try
            {
                if (e.CommandName == "VeiwDetails")
                {
                    ModalPopupExtender1.Show();
                    if (fileID > 0 && dsDashboard != null)
                    {
                        drDashboard = dsDashboard.Tables[2].Select("FileID = '" + fileID + "'");
                        if (drDashboard != null && drDashboard.Length > 0)
                        {
                            divDetails.Visible = true;
                            divGrid.Visible = false;
                            rowTicket.Visible = false;
                            rowProject.Visible = true;
                            lblCompName.Text = drDashboard[0]["CompanyName"].ToString();
                            lblContactPerson.Text = drDashboard[0]["ContactPerson"].ToString();
                            lblCustomerAddress.Text = drDashboard[0]["Address"].ToString();
                            lblCutsomerEmail.Text = drDashboard[0]["EmailID"].ToString();
                            lblEndDate.Text = !string.IsNullOrEmpty(drDashboard[0]["EndDate"].ToString()) ? Convert.ToDateTime(drDashboard[0]["EndDate"].ToString()).ToShortDateString() : string.Empty;
                            lblITContact.Text = drDashboard[0]["ITContact"].ToString();
                            lblITEmail.Text = drDashboard[0]["ITEmail"].ToString();
                            lblITPhone.Text = drDashboard[0]["ITPhone"].ToString();
                            lblPMAssigned.Text = drDashboard[0]["PMAssigned"].ToString();
                            lblProjectName.Text = drDashboard[0]["ProjectName"].ToString();
                            lblStartDate.Text = !string.IsNullOrEmpty(drDashboard[0]["StartDate"].ToString()) ? Convert.ToDateTime(drDashboard[0]["StartDate"].ToString()).ToShortDateString() : string.Empty;
                            lblTechAssigned.Text = drDashboard[0]["TechAssigned"].ToString();
                            lblProDesc.Text = drDashboard[0]["ProjectDescription"].ToString();
                        }
                    }
                }
                else if (e.CommandName == "ViewTaskDetails")
                {
                    ModalPopupExtender1.Hide();
                    if (taskID > 0)
                    {
                        Response.Redirect("TaskDetails.aspx?FileID= "+ fileID + "&taskID=" + taskID, true);
                    }
                    else
                    {
                        Response.Redirect("TaskDetails.aspx?FileID= " + fileID, true);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void gvTicket_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int FileID = Convert.ToInt32(e.CommandArgument.ToString());
            DataSet dsDashboard = ViewState[DASHBOARD] as DataSet;
            DataRow[] drDashboard = null;
            try
            {
                if (e.CommandName == "TicketInfo")
                {
                    ModalPopupExtender1.Show();
                    divDetails.Visible = true;
                    divGrid.Visible = false;
                    if (FileID > 0 && dsDashboard != null)
                    {
                        drDashboard = dsDashboard.Tables[2].Select("FileID = '" + FileID + "'");
                        if (drDashboard != null && drDashboard.Length > 0)
                        {
                            rowTicket.Visible = true;
                            rowProject.Visible = false;
                            lblCompName.Text = drDashboard[0]["CompanyName"].ToString();
                            lblContactPerson.Text = drDashboard[0]["ContactPerson"].ToString();
                            lblCustomerAddress.Text = drDashboard[0]["Address"].ToString();
                            lblCutsomerEmail.Text = drDashboard[0]["EmailID"].ToString();
                            lblTicEnd.Text = !string.IsNullOrEmpty(drDashboard[0]["EndDate"].ToString()) ? Convert.ToDateTime(drDashboard[0]["EndDate"].ToString()).ToShortDateString() : string.Empty;
                            lblITContact.Text = drDashboard[0]["ITContact"].ToString();
                            lblITEmail.Text = drDashboard[0]["ITEmail"].ToString();
                            lblITPhone.Text = drDashboard[0]["ITPhone"].ToString();
                            lblPMAssigned.Text = drDashboard[0]["PMAssigned"].ToString();
                            lblProjectName.Text = drDashboard[0]["ProjectName"].ToString();
                            lblTicStart.Text = !string.IsNullOrEmpty(drDashboard[0]["StartDate"].ToString()) ? Convert.ToDateTime(drDashboard[0]["StartDate"].ToString()).ToShortDateString() : string.Empty;
                            lblTicTech.Text = drDashboard[0]["TechAssigned"].ToString();
                            lblTicketName.Text = drDashboard[0]["TicketName"].ToString();
                            lblTicketNumber.Text = drDashboard[0]["TicketNumber"].ToString();
                            lblPriority.Text = drDashboard[0]["TicketPriority"].ToString();
                            lblTicDesc.Text = drDashboard[0]["TicketDescription"].ToString();
                        }
                    }
                }
                else if (e.CommandName == "ViewTaskDetails")
                {
                    ModalPopupExtender1.Hide();
                    Response.Redirect("TaskDetails.aspx?FileID= " + e.CommandArgument.ToString(), true);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void btnDefault_Click(object sender, EventArgs e)
        {
            string selectedDate = hiddDate.Value;
            PMDashboard.BusinessLayer.Project objProeject = new PMDashboard.BusinessLayer.Project();
            DataSet dsData = null;
            DataTable dtTask = new DataTable();
            try
            {
                gvPro.DataSource = null;
                gvPro.DataBind();

                if (!string.IsNullOrEmpty(selectedDate))
                {
                    string[] strDate = selectedDate.Split('|');
                    if (strDate.Length > 0)
                    {
                        dtTask.Columns.Add("TaskID", typeof(int));
                        dtTask.Columns.Add("ProjectName", typeof(string));
                        dtTask.Columns.Add("PMAssigned", typeof(string));
                        dtTask.Columns.Add("Name", typeof(string));
                        dtTask.Columns.Add("AssignedTo", typeof(string));
                        dtTask.Columns.Add("FileID", typeof(int));
                        dtTask.Columns.Add("Priority", typeof(string));
                        dtTask.Columns.Add("Types", typeof(string));
                        string user = FBBusiness.LoggedInUser.Name;
                        int ProjectID = State.CurrentProjectID;
                        dsData = objProeject.GetDashboardData(user, ProjectID);
                        divGrid.Visible = true;
                        divDetails.Visible = false;
                        ModalPopupExtender1.Show();

                        var query = (from a in dsData.Tables[0].AsEnumerable()
                                     join b in dsData.Tables[2].AsEnumerable()
                                     on a.Field<int>("FileID") equals
                                     b.Field<int>("FileID")
                                     where
                                     a.Field<DateTime>("StartDate") == Convert.ToDateTime(strDate[0])
                                     &&
                                     b.Field<string>("Type") == strDate[1]
                                     select new
                                     {
                                         TaskID = a["TaskID"].ToString(),
                                         ProjectName = b["ProjectName"].ToString(),
                                         PMAssigned = b["PMAssigned"].ToString(),
                                         Task = a["Name"].ToString(),
                                         AssignedTo = a["AssignedTo"].ToString(),
                                         FileID = a["FileID"].ToString(),
                                         Priority = b["TicketPriority"].ToString(),
                                         Types = b["Type"].ToString()
                                     });
                        foreach (var result in query)
                        {
                            dtTask.Rows.Add(result.TaskID, result.ProjectName, result.PMAssigned, result.Task, result.AssignedTo, result.FileID, result.Priority, result.Types);
                        }

                        string[] strTask = { "TaskID", "ProjectName", "PMAssigned", "Name", "AssignedTo", "FileID", "Priority", "Types" };

                        if (strDate[1] == "Project")
                        {
                            // bind grid view 
                            gvPro.Visible = true;
                            gvTick.Visible = false;
                            gvPro.DataSource = dtTask.DefaultView.ToTable(true, strTask);
                            gvPro.DataBind();
                            gvTick.DataSource = null;
                            gvTick.DataBind();
                        }
                        else
                        {
                            // bind grid view 
                            gvPro.Visible = false;
                            gvTick.Visible = true;
                            gvPro.DataSource = null;
                            gvPro.DataBind();
                            gvTick.DataSource = dtTask.DefaultView.ToTable(true, strTask);
                            gvTick.DataBind();
                        }

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objProeject = null;
                dsData = null;
            }
        }

        protected void calcTicket_DayRender(object sender, DayRenderEventArgs e)
        {
            DateTime nextDate;
            DataSet dsPorject = ViewState[DASHBOARD] as DataSet;
            DataRow[] drProject = null;
            DataRow[] drTask = null;
            string companyName = string.Empty;
            if (dsPorject != null)
            {
                // filter task for projects only
                drProject = dsPorject.Tables[2].Select("Type = '" + "Ticket" + "'");
                if (drProject != null && drProject.Length > 0)
                {
                    for (int count = 0; count < drProject.Length; count++)
                    {
                        drTask = dsPorject.Tables[0].Select("FileID = '" + drProject[count]["FileID"].ToString() + "'");
                        if (drTask != null && drTask.Length > 0)
                        {

                            foreach (DataRow dr in drTask)
                            {
                                nextDate = (DateTime)dr["StartDate"];
                                if (nextDate.Date == e.Day.Date)
                                {
                                    String url = e.SelectUrl;
                                    e.Cell.Controls.Clear();
                                    HyperLink link = new HyperLink();
                                    link.ID = "link" + e.Day.Date.Day.ToString();
                                    link.Text = e.Day.Date.Day.ToString();
                                    link.ToolTip = companyName;
                                    link.ForeColor = System.Drawing.Color.White;
                                    link.Font.Bold = true;
                                    DataRow[] drStartDate = drTask.CopyToDataTable().Select("StartDate = '" + e.Day.Date + "'");


                                    if (drStartDate != null && drStartDate.Length > 1)
                                    {
                                        ModalPopupExtender1.Show();
                                        // Attaching java script function to link 
                                        link.Attributes.Add("onClick", "javascript:GetClick('" + e.Day.Date +"|Ticket" + "');");
                                    }
                                    else
                                    {
                                        ModalPopupExtender1.Hide();
                                        companyName = dsPorject.Tables[2].Select("FileID = '" + dr["FileID"].ToString() + "'")[0]["CompanyName"].ToString();
                                        e.Cell.ToolTip = companyName;
                                        link.NavigateUrl = "TaskDetails.aspx?FileID=" + dr["FileID"].ToString() + "&TaskID=" + dr["TaskID"].ToString();
                                    }

                                    e.Cell.Controls.Add(link);
                                    // if status is in open condition
                                    if (dr["status"].ToString() == "Open")
                                    {
                                        e.Cell.BackColor = System.Drawing.Color.Red;
                                    }
                                    // if status is in running condition
                                    else if (dr["status"].ToString() == "In Process")
                                    {
                                        e.Cell.BackColor = System.Drawing.Color.Lime;
                                    }
                                    // if status is in close condition
                                    else if (dr["status"].ToString() == "Close")
                                    {
                                        e.Cell.BackColor = System.Drawing.Color.Green;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}