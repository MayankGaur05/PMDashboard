using System;

namespace PMDashboard.ModelLayer
{
    public class MLTask
    {
        public int TaskID
        {
            get;
            set;
        }

        public int FileID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public DateTime StartDate
        {
            get;
            set;
        }

        public DateTime EndDate
        {
            get;
            set;
        }

        public DateTime DateCreated
        {
            get;
            set;
        }

        public DateTime DateModified
        {
            get;
            set;
        }

        public string AssignedTo
        {
            get;
            set;
        }

        public string WorkCompletedOnTask
        {
            get;
            set;
        }

        public string WorkPendingOnTask
        {
            get;
            set;
        }

        public int Status
        {
            get;
            set;
        }

        public DateTime StartTime
        {
            get;
            set;
        }

        public DateTime EndTime
        {
            get;
            set;
        }
    }
}