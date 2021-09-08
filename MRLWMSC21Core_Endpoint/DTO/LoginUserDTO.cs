using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.DTO
{

    public class LoginUserDTO
    {
        private string _MailID;
        private string _PasswordEncrypted;
        private string _ClientMAC;
        private string _SessionIdentifier;
        private string _CookieIdentifier;
        private string _PrinterIP;
        public string MailID { get => _MailID; set => _MailID = value; }
        public string PasswordEncrypted { get => _PasswordEncrypted; set => _PasswordEncrypted = value; }
        public string ClientMAC { get => _ClientMAC; set => _ClientMAC = value; }
        public string SessionIdentifier { get => _SessionIdentifier; set => _SessionIdentifier = value; }
        public string CookieIdentifier { get => _CookieIdentifier; set => _CookieIdentifier = value; }
        public string PrinterIP { get => _PrinterIP; set => _PrinterIP = value; }
    }
}