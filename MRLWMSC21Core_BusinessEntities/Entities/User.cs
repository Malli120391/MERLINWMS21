using System;
using System.Collections.Generic;
using System.Text;

namespace MRLWMSC21Core.Entities
{
   public class User
    {

        private int _UserID;
        private int _AccountID;
        private int _TenantID;
        private int _LocaleID;
        private int _CreatedBy;
        private int _UpdatedBy;
        private int _SSOUserID;
        private int _UserTypeID;
        private int _AddressBookID;


        private bool _IsActive;
        private bool _IsDeleted;

        private string _EmailID;
        private string _EmployeeCode;
        private string _FirstName;
        private string _MiddleName;
        private string _LastName;
        private string _AlternateEMailID1;
        private string _AlternameEmailID2;

        private string _Password;
        private string _EncryptedPassword;
        private string _Mobile;

        public User()
        { }

        public User(int UserID, int SSOUserID, int AccountID, int TenantID, int UserTypeID, int LocaleID, int AddressBookID,
            int CreatedBy, int UpdatedBy, string eMailID, string eMailID1, string eMailID2, string FirstName, string MiddleName,
            string LastName, string AltEmail1, string AltEmail2, string sMobile, bool IsActive, bool IsDeleted)
        {
            this.UserID = UserID;
            this.SSOUserID = SSOUserID;
            this.AccountID = AccountID;
            this.TenantID = TenantID;
            this.UserTypeID = UserTypeID;
            this.LocaleID = LocaleID;
            this.AddressBookID = AddressBookID;
            this.CreatedBy = CreatedBy;
            this.UpdatedBy = UpdatedBy;

            this.EmailID = eMailID;
            this.AlternateEMailID1 = eMailID1;
            this.AlternameEmailID2 = eMailID2;

            this.FirstName = FirstName;
            this.MiddleName = MiddleName;
            this.LastName = LastName;

            this.IsActive = IsActive;
            this.IsDeleted = IsDeleted;
        }




        public int AccountID { get => _AccountID; set => _AccountID = value; }
        public int TenantID { get => _TenantID; set => _TenantID = value; }
        public int UserID { get => _UserID; set => _UserID = value; }
        public int LocaleID { get => _LocaleID; set => _LocaleID = value; }
        public int CreatedBy { get => _CreatedBy; set => _CreatedBy = value; }
        public int UpdatedBy { get => _UpdatedBy; set => _UpdatedBy = value; }
        public int SSOUserID { get => _SSOUserID; set => _SSOUserID = value; }
        public int UserTypeID { get => _UserTypeID; set => _UserTypeID = value; }
        public int AddressBookID { get => _AddressBookID; set => _AddressBookID = value; }
        public bool IsActive { get => _IsActive; set => _IsActive = value; }
        public bool IsDeleted { get => _IsDeleted; set => _IsDeleted = value; }
        public string EmailID { get => EmailID1; set => EmailID1 = value; }
        public string EmployeeCode { get => _EmployeeCode; set => _EmployeeCode = value; }
        public string FirstName { get => _FirstName; set => _FirstName = value; }
        public string MiddleName { get => _MiddleName; set => _MiddleName = value; }
        public string LastName { get => _LastName; set => _LastName = value; }
        public string AlternateEMailID1 { get => _AlternateEMailID1; set => _AlternateEMailID1 = value; }
        public string AlternameEmailID2 { get => _AlternameEmailID2; set => _AlternameEmailID2 = value; }
        public string Password { get => _Password; set => _Password = value; }
        public string EncryptedPassword { get => _EncryptedPassword; set => _EncryptedPassword = value; }
        public string Mobile { get => _Mobile; set => _Mobile = value; }
        public string EmailID1 { get => _EmailID; set => _EmailID = value; }


    }
}