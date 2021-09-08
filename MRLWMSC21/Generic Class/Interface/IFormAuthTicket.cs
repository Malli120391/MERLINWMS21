using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using MRLWMSC21.Generic_Class.Class;
namespace MRLWMSC21.Generic_Class.Interface
{
    
    public interface IFormAuthTicket
    {
        [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.LinkDemand, Name = "FullTrust")]
        [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, Name = "FullTrust")]
        ValidateUser ValidateUser(string User, string Password, bool Rememberme, string ClientIdentifier, string SessionIdentifier, string LoginTimeStamp);


    }
}
