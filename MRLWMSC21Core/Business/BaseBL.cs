using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRLWMSC21Core.Business
{
    public class BaseBL
    {
        private int _LoggedInUserID;
        private string _ConnectionString;

        public BaseBL(int LoginUserID, string ConnectionString)
        {
            _LoggedInUserID = LoginUserID;
            _ConnectionString = ConnectionString;
        }

        public int LoggedInUserID { get => _LoggedInUserID;}
        public string ConnectionString { get => _ConnectionString; }
    }
}
