using System;
using System.Collections.Generic;

namespace Az_FnHttpAPI.Models
{
    public partial class Department : EntityBase
    {
        public Department()
        {
            Employees = new HashSet<Employee>();
        }

        public int DeptNo { get; set; }
        public string DeptName { get; set; }
        public int Capacity { get; set; }
        public string Location { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
