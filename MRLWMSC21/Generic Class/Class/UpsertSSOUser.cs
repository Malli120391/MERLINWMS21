using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21.Generic_Class.Class
{
    public sealed class User
    {
        public int Apps_MST_User_ID { get; set; }

        public int Apps_MST_Account_ID { get; set; }

        public int TM_MST_Tenant_ID { get; set; }

        public int TM_MST_Division_ID { get; set; }

        public int TM_MST_Department_ID { get; set; }

        public string Salutation { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string CompanyName { get; set; }

        public string eMailID { get; set; }

        public string EmpCode { get; set; }

        public string Password { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        public string Website { get; set; }

        public string ZOHO_Customer_ID { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }
    }
}