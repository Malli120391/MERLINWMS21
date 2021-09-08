using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System.IO;
using MRLWMSC21.Generic_Class.Interface;
namespace MRLWMSC21.Generic_Class.Class
{
    [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.LinkDemand, Name = "FullTrust")]
    public class SecureFileTransfer:ISecureFileTransfer
    {
        /// <summary>
        /// Security Uploading the files like PNG, JPG ... PDF etc remotely to different location.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="userName"></param>
        /// <param name="port"></param>
        /// <param name="filePath"></param>
        /// <param name="buffersize"></param>
        public void  SSHClient(string host, string userName , int port , string filePath , uint buffersize)
        {
            using (SftpClient sftpClient = new SftpClient(getSftpConnection(host, userName, port, filePath)))
            {
               sftpClient.Connect();
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    sftpClient.BufferSize = buffersize; //eg 1024
                    sftpClient.UploadFile(fs, Path.GetFileName(filePath));
                }
                 sftpClient.Dispose();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="username"></param>
        /// <param name="port"></param>
        /// <param name="publicKeyPath"></param>
        /// <returns></returns>
        private static ConnectionInfo getSftpConnection(string host, string username, int port, string publicKeyPath)
        {
            return new ConnectionInfo(host, port, username, privateKeyObject(username, publicKeyPath));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="publicKeyPath"></param>
        /// <returns></returns>
        private static AuthenticationMethod[] privateKeyObject(string username, string publicKeyPath)
        {
            PrivateKeyFile privateKeyFile = new PrivateKeyFile(publicKeyPath);
            PrivateKeyAuthenticationMethod privateKeyAuthenticationMethod =
               new PrivateKeyAuthenticationMethod(username, privateKeyFile);
            return new AuthenticationMethod[]
             {
                  privateKeyAuthenticationMethod
             };
        }
    }
}