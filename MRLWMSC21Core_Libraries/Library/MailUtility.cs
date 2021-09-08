using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace MRLWMSC21Core.Library
{
    public class MailUtility
    {
        public void SendExceptionEmail(Exception ex, string MethodCode, ExceptionData _Data)
        {
            try
            {
                string _sFromeMail = FrameworkUtilities.ReadApplicationKey("FromEMail");
                string _sSMTPServer = FrameworkUtilities.ReadApplicationKey("SMTPServer");
                string _sFromMailPassword = FrameworkUtilities.ReadApplicationKey("FromMailPassword");
                string _eMail = FrameworkUtilities.ReadApplicationKey("FromEMail");

                bool _bRequireAuthentication = ConversionUtility.ConvertToBool(FrameworkUtilities.ReadApplicationKey("SMTPServerRequiresAuthentication"));

                MailMessage mail = new MailMessage();
                // SmtpClient SmtpServer = new SmtpClient("smtpout.asia.secureserver.net");
                SmtpClient SmtpServer = new SmtpClient(_sSMTPServer);

                StringBuilder str = new StringBuilder();

                DateTime _EventTimestamp = DateTime.Now;
                string _Date = ConversionUtility.ConvertToDateString(_EventTimestamp);
                string _Time = ConversionUtility.ConvertToTimeString(_EventTimestamp);

                string _Environment = FrameworkUtilities.ReadApplicationKey("ApplicationEnvironment");
                
                bool IsProduction = _Environment.ToUpper().Contains("PROD") ? true : false;

                // mail.From = new MailAddress("merlinwms@inventrax.com");
                mail.From = new MailAddress(_sFromeMail);

                string _INVDevelopers = FrameworkUtilities.ReadApplicationKey("INVDevelopers");
                foreach (string _ToMail in _INVDevelopers.Split(','))
                {
                    if (_ToMail != string.Empty && _ToMail.ToLower().Contains("@inventrax.com"))
                        mail.To.Add(_ToMail);
                }

                string _INVQualityTeam = FrameworkUtilities.ReadApplicationKey("INVQualityTeam");
                foreach (string _CCMail in _INVQualityTeam.Split(','))
                {
                    if (_CCMail != string.Empty && _CCMail.ToLower().Contains("@inventrax.com"))
                        mail.CC.Add(_CCMail);
                }

                string _INVSupportTeam = FrameworkUtilities.ReadApplicationKey("INVSupportTeam");
                foreach (string _ToMail in _INVSupportTeam.Split(','))
                {
                    if (_ToMail != string.Empty && _ToMail.ToLower().Contains("@inventrax.com"))
                        mail.To.Add(_ToMail);
                }


                string _INVPMTeam = FrameworkUtilities.ReadApplicationKey("INVPMTeam");
                foreach (string _CCMail in _INVPMTeam.Split(','))
                {
                    if (_CCMail != string.Empty && _CCMail.ToLower().Contains("@inventrax.com"))
                        mail.CC.Add(_CCMail);
                }

                //if (IsProduction)
                //{
                //    mail.Bcc.Add("adityag@inventrax.com");
                //    mail.Bcc.Add("sureshy@inventrax.com");
                //}

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
                str.Append("<td>Method Block Code</td>");
                str.Append("<td>:</td>");
                str.Append("<td>" + MethodCode + " </td>");
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
                str.Append("<td>Exception Timestamp</td>");
                str.Append("<td>:</td>");
                str.Append("<td>" + ConversionUtility.ConvertToDateTimeString(DateTime.Now) + " </td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<td colspan='3'><br/><br/><center><b>Method Data</b></center></td>");
                str.Append("</tr>");


                foreach (KeyValuePair<string, object> _DataItem in _Data.MethodInputs)
                {
                    string json = JsonConvert.SerializeObject(_DataItem.Value);

                    str.Append("<tr>");
                    str.Append("<td>" + _DataItem.Key + " </td>");
                    str.Append("<td>:</td>");
                    str.Append("<td>" + json + " </td>");
                    str.Append("</tr>");
                }
                str.Append("</table>");


                str.Append("<br/>");


                str.Append("Request you to resolve the same immediately. <br/><br/> Thanks & Regards <br/>- WMS Application Insights");

                mail.IsBodyHtml = true;
                StringWriter writer = new StringWriter();
                string htmlString = writer.ToString();

                mail.Subject = _Environment + " : An exception has occoured on " + _Date + " at " + _Time;
                mail.Body = str.ToString();
                mail.Priority = MailPriority.High;

                SmtpServer.Port = Convert.ToInt32(FrameworkUtilities.ReadApplicationKey("SMTPPort"));///80;
                SmtpServer.Credentials = new System.Net.NetworkCredential(_sFromeMail, _sFromMailPassword);
                SmtpServer.EnableSsl = _bRequireAuthentication;
                SmtpServer.Send(mail);
            }
            catch (Exception excp)
            {
                
            }
        }
        
    }
}
