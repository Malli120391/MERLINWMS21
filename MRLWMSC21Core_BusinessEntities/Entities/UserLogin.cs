using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRLWMSC21Core.Entities
{
    public class UserLogin
    {
        
        private string _eMailID;
        private string _Password;

        public string EmailID { get => _eMailID; set => _eMailID = value; }
        public string Password { get => _Password; set => _Password = value; }
    }
}
