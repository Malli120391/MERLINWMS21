using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Configuration;

namespace Inventrax_CommonBackGroundService
{
    public class MailUtility
    {
        public void SendExceptionEmail(Exception ex)
        {
            try
            {
                

                string _sFromeMail = ConfigurationManager.AppSettings["FromEMail"];
                string _sSMTPServer = ConfigurationManager.AppSettings["SMTPServer"];
                string _sFromMailPassword = ConfigurationManager.AppSettings["FromMailPassword"];
                string _eMail = ConfigurationManager.AppSettings["FromEMail"];



                MailMessage mail = new MailMessage();
                // SmtpClient SmtpServer = new SmtpClient("smtpout.asia.secureserver.net");
                SmtpClient SmtpServer = new SmtpClient(_sSMTPServer);

                StringBuilder str = new StringBuilder();

                DateTime _EventTimestamp = DateTime.Now;
                string _Date = DateTime.Now.ToString("dd-MMM-yyyy");

                string _Time = DateTime.Now.ToString("hh:mm:ss tt");

                string _Environment = ConfigurationManager.AppSettings["ApplicationEnvironment"];

                bool IsProduction = _Environment.ToUpper().Contains("PROD") ? true : false;

                // mail.From = new MailAddress("merlinwms@inventrax.com");
                mail.From = new MailAddress(_sFromeMail);


                string _INVDevelopers = ConfigurationManager.AppSettings["INVDevelopers"];

                foreach (string _ToMail in _INVDevelopers.Split(','))
                {
                    if (_ToMail != string.Empty && _ToMail.ToLower().Contains("@inventrax.com"))
                        mail.To.Add(_ToMail);
                }


                string _INVQualityTeam = ConfigurationManager.AppSettings["INVQualityTeam"];

                foreach (string _CCMail in _INVQualityTeam.Split(','))
                {
                    if (_CCMail != string.Empty && _CCMail.ToLower().Contains("@inventrax.com"))
                        mail.CC.Add(_CCMail);
                }

                string _INVSupportTeam = ConfigurationManager.AppSettings["INVSupportTeam"];

                foreach (string _ToMail in _INVSupportTeam.Split(','))
                {
                    if (_ToMail != string.Empty && _ToMail.ToLower().Contains("@inventrax.com"))
                        mail.To.Add(_ToMail);
                }


                string _INVPMTeam = ConfigurationManager.AppSettings["INVPMTeam"];
                foreach (string _CCMail in _INVPMTeam.Split(','))
                {
                    if (_CCMail != string.Empty && _CCMail.ToLower().Contains("@inventrax.com"))
                        mail.CC.Add(_CCMail);
                }

                if (IsProduction)
                {
                    //mail.Bcc.Add("adityag@inventrax.com");

                }

                //mail.To.Add(email);

                str.Append("Dear Team, <br/><br/>");
                str.Append("An error has occoured in " + _Environment);
                str.Append("<br/><br/> The following are the details: ");
                str.Append("<br/><br/><br/> ");
                str.Append("<table>");

                str.Append("<tr>");
                str.Append("<td colspan='3'><center><b>Exception Data</b></center></td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<td style='width:25%'>Application Environment</td>");
                str.Append("<td style='width:5%'>:</td>");
                str.Append("<td style='width:70%'>" + _Environment + " </td>");
                str.Append("</tr>");


                str.Append("<tr>");
                str.Append("<td>Exception Message</td>");
                str.Append("<td>:</td>");
                str.Append("<td>" + ex.Message + " </td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<td>Exception Stack Trace</td>");
                str.Append("<td>:</td>");
                str.Append("<td>" + ex.StackTrace + " </td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<td>Inner Exception</td>");
                str.Append("<td>:</td>");
                str.Append("<td>" + ex.InnerException + " </td>");
                str.Append("</tr>");


                str.Append("<tr>");
                str.Append("<td>Source</td>");
                str.Append("<td>:</td>");
                str.Append("<td>" + ex.Source + " </td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<td>Exception Timestamp</td>");
                str.Append("<td>:</td>");
                str.Append("<td>" + DateTime.Now.ToString() + " </td>");
                str.Append("</tr>");



                str.Append("</table>");


                str.Append("<br/>");


                str.Append("Request you to resolve the same immediately. <br/><br/> Thanks & Regards <br/>- WMS Application Insights");

                mail.IsBodyHtml = true;
                StringWriter writer = new StringWriter();
                string htmlString = writer.ToString();
                
                mail.Subject = _Environment + " : An exception has occoured on " + _Date + " at " + _Time;
                mail.Body = str.ToString();
                mail.Priority = MailPriority.High;

                SmtpServer.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);///80;
                SmtpServer.Credentials = new System.Net.NetworkCredential(_sFromeMail, _sFromMailPassword);
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
            }
            catch (Exception excp)
            {

            }
        }


        public void SendEmailNotification(string Message)
        {
            try
            {

                string _sFromeMail = ConfigurationManager.AppSettings["FromEMail"];
                string _sSMTPServer = ConfigurationManager.AppSettings["SMTPServer"];
                string _sFromMailPassword = ConfigurationManager.AppSettings["FromMailPassword"];
                string _eMail = ConfigurationManager.AppSettings["FromEMail"];



                MailMessage mail = new MailMessage();
                // SmtpClient SmtpServer = new SmtpClient("smtpout.asia.secureserver.net");
                SmtpClient SmtpServer = new SmtpClient(_sSMTPServer);

                StringBuilder str = new StringBuilder();

                DateTime _EventTimestamp = DateTime.Now;
                string _Date = DateTime.Now.ToString("dd-MMM-yyyy");

                string _Time = DateTime.Now.ToString("hh:mm:ss tt");

                string _Environment = ConfigurationManager.AppSettings["ApplicationEnvironment"];

                bool IsProduction = _Environment.ToUpper().Contains("PROD") ? true : false;

                // mail.From = new MailAddress("merlinwms@inventrax.com");
                mail.From = new MailAddress(_sFromeMail);


                string _INVDevelopers = ConfigurationManager.AppSettings["INVDevelopers"];

                foreach (string _ToMail in _INVDevelopers.Split(','))
                {
                    if (_ToMail != string.Empty && _ToMail.ToLower().Contains("@inventrax.com"))
                        mail.To.Add(_ToMail);
                }


                string _INVQualityTeam = ConfigurationManager.AppSettings["INVQualityTeam"];

                foreach (string _CCMail in _INVQualityTeam.Split(','))
                {
                    if (_CCMail != string.Empty && _CCMail.ToLower().Contains("@inventrax.com"))
                        mail.CC.Add(_CCMail);
                }

                string _INVSupportTeam = ConfigurationManager.AppSettings["INVSupportTeam"];

                foreach (string _ToMail in _INVSupportTeam.Split(','))
                {
                    if (_ToMail != string.Empty && _ToMail.ToLower().Contains("@inventrax.com"))
                        mail.To.Add(_ToMail);
                }


                string _INVPMTeam = ConfigurationManager.AppSettings["INVPMTeam"];
                foreach (string _CCMail in _INVPMTeam.Split(','))
                {
                    if (_CCMail != string.Empty && _CCMail.ToLower().Contains("@inventrax.com"))
                        mail.CC.Add(_CCMail);
                }

             

                str.Append("Dear Team, <br/><br/>");
                str.Append("Email Notification from " + _Environment);
                str.Append("<br/><br/><br/> ");

                str.Append("<table>");

                str.Append("<tr>");
                str.Append("<td style='width:25%'>Application Environment</td>");
                str.Append("<td style='width:5%'>:</td>");
                str.Append("<td style='width:70%'>" + _Environment + " </td>");
                str.Append("</tr>");


                str.Append("<tr>");
                str.Append("<td>Message</td>");
                str.Append("<td>:</td>");
                str.Append("<td>" +Message + " </td>");
                str.Append("</tr>");



                str.Append("<tr>");
                str.Append("<td>Timestamp</td>");
                str.Append("<td>:</td>");
                str.Append("<td>" + DateTime.Now.ToString() + " </td>");
                str.Append("</tr>");



                str.Append("</table>");


                str.Append("<br/>");


                mail.IsBodyHtml = true;
                StringWriter writer = new StringWriter();
                string htmlString = writer.ToString();

                mail.Subject = _Environment + "Email Notification " + _Date + " at " + _Time;
                mail.Body = str.ToString();
                mail.Priority = MailPriority.High;

                SmtpServer.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);///80;
                SmtpServer.Credentials = new System.Net.NetworkCredential(_sFromeMail, _sFromMailPassword);
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
            }
            catch (Exception excp)
            {

            }
        }

        public void SendGeneralEmail(string mailBody,string mailHeader,string toemailList,string mailSubject,string attachmentPath,string ccEmailList)
        {
            try
            {

                string _sFromeMail = ConfigurationManager.AppSettings["FromEMail"];
                string _sSMTPServer = ConfigurationManager.AppSettings["SMTPServer"];
                string _sFromMailPassword = ConfigurationManager.AppSettings["FromMailPassword"];
                string _eMail = ConfigurationManager.AppSettings["FromEMail"];



                MailMessage mail = new MailMessage();
                // SmtpClient SmtpServer = new SmtpClient("smtpout.asia.secureserver.net");
                SmtpClient SmtpServer = new SmtpClient(_sSMTPServer);

               // StringBuilder str = new StringBuilder();

                DateTime _EventTimestamp = DateTime.Now;
                string _Date = DateTime.Now.ToString("dd-MMM-yyyy");

                string _Time = DateTime.Now.ToString("hh:mm:ss tt");

                string _Environment = ConfigurationManager.AppSettings["ApplicationEnvironment"];

                string serverEnvironment = ConfigurationManager.AppSettings["ServerEnvironment"];

                

                mail.From = new MailAddress(_sFromeMail);


                foreach (string _ToMail in toemailList.Split(','))
                {
                    if (_ToMail != string.Empty)
                        mail.To.Add(_ToMail);
                }


                foreach (string _ccMail in ccEmailList.Split(','))
                {
                    if (_ccMail != string.Empty)
                        mail.CC.Add(_ccMail);
                }

                string _INVPMTeam = ConfigurationManager.AppSettings["INVPMTeam"];
                foreach (string _bCCMail in _INVPMTeam.Split(','))
                {

                    if (_bCCMail != string.Empty)
                        mail.Bcc.Add(_bCCMail);
                }




                mail.IsBodyHtml = true;
                StringWriter writer = new StringWriter();
                string htmlString = writer.ToString();

                mail.Subject = mailSubject;
                mail.Body = mailBody;
                mail.Priority = MailPriority.High;

                try
                {
                    Attachment attachment = new Attachment(attachmentPath);
                    mail.Attachments.Add(attachment);

                }
                catch(Exception ex)
                {
                    LogWriter.WriteLog("Exception in attachments section  " + ex.Message);
                }
               

                SmtpServer.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);///80;
                SmtpServer.Credentials = new System.Net.NetworkCredential(_sFromeMail, _sFromMailPassword);
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
            }
            catch (Exception excp)
            {
                LogWriter.WriteLog("Exception in method SendGeneralEmail    " +excp.Message);
            }
        }
    }
}
