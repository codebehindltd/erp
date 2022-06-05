using HotelManagement.Entity.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.TaskManagement
{
    public class SMTask
    {
        public long Id { get; set; }
        public string TaskName { get; set; }
        public DateTime TaskDate { get; set; }
        public DateTime StartTime { get; set; }
        public string Description { get; set; }
        public string TaskType { get; set; }
        public int? AssignToId { get; set; }
        public string EmailReminderType { get; set; }
        public bool IsEmailReminderSent { get; set; }
        public DateTime? EmailReminderDate { get; set; }
        public DateTime? EmailReminderTime { get; set; }
        public bool IsCompleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? TaskStage { get; set; }
        public long? SourceNameId { get; set; }
        public string SourceName { get; set; }
        public DateTime? EstimatedDoneDate { get; set; }
        public DateTime? EndTime { get; set; }
        public string CallToAction { get; set; }
        public decimal? EstimatedDoneHour { get; set; }
        public decimal? Complete { get; set; }
        public decimal? ProjectComplete { get; set; }
        public int? ParentTaskId { get; set; }
        public int? DependentTaskId { get; set; }
        public int? HasChild { get; set; }

        public string ProjectName { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNameList { get; set; }
        public string AssignType { get; set; }
        public string Status { get; set; }
        public string TaskStatus { get; set; }
        public string PerticipentFromClient { get; set; }
        public int? EmpDepartment { get; set; }
        public bool? ImplementStatus { get; set; }

        public string TaskFor { get; set; }
        public int? AccountManagerId { get; set; }
        public string AccountManagerName { get; set; }
        public int? TaskPriority { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int? ContactId { get; set; }
        public string ContactName { get; set; }
        public int? DealId { get; set; }
        public int? EmpId { get; set; }
        public string DealName { get; set; }
        public string DueDateTime { get; set; }
        public DateTime? ReminderDateFrom { get; set; }
        public DateTime? ReminderDateTo { get; set; }
    }
}
