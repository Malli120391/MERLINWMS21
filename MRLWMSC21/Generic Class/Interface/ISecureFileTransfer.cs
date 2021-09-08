using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21.Generic_Class.Interface
{
    public interface ISecureFileTransfer
    {
        [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.LinkDemand, Name = "FullTrust")]
        [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, Name = "FullTrust")]
        void SSHClient(string host, string userName, int port, string filePath, uint buffersize);
    }
}