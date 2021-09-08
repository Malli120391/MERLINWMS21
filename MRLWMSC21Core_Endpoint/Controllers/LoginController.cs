using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MRLWMSC21_Endpoint.Security;
using MRLWMSC21Core.Business;
using MRLWMSC21Core.Entities;
using MRLWMSC21Core.Library;
using MRLWMSC21_Endpoint.DTO;
using MRLWMSC21_Endpoint.Interfaces;
using Newtonsoft.Json;

/*

    ******************************************************************************************
                                        HEADER INFORMATION
    ******************************************************************************************
         Author          : Aditya Srinivas                                                   
         Created Date    : 31st May 2018                                                      
         Purpose         : Primarily used for Logging into the WMS Application.
    ******************************************************************************************
      Sl. No.       Modified By           Modified On                  Purpose                
    ******************************************************************************************
         1      
         2      
         3      
         4      
    ******************************************************************************************
 */
namespace MRLWMSC21_Endpoint.Controllers
{
    /// <summary>
    /// Login Controller is used for Logging in to and out of WMS Application.
    /// </summary>
    [RoutePrefix("Login")]
    public class LoginController : BaseController, ILogin
    {
        private string _ClassCode = string.Empty;

        public LoginController()        
        {
            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_Enpoint.LoginController);
        }



        // Developed & Unit Tested
        [Route("UserLogin")]
        [HttpPost]
        public string UserLogin(WMSCoreMessage oRequest)
        {
            try
            {
                ProfileDTO profileDTO = new ProfileDTO();
                List<ProfileDTO> _lstProfileDTO = new List<ProfileDTO>();

                MRLWMSC21Service.LoginUserData oLoginUserData = new MRLWMSC21Service.LoginUserData();
                if (oRequest != null)
                {
                    // LoginUserDTO _oLoginUserDTO = (LoginUserDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    LoginUserDTO _oLoginUserDTO = (LoginUserDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);


                    if (_oLoginUserDTO!=null)
                    {
                        MRLWMSC21Service.ValidateUserLogin oUserlogin = new MRLWMSC21Service.ValidateUserLogin()
                        {
                        Apps_MST_Application_ID = 2,
                        ClientMAC = _oLoginUserDTO.ClientMAC,
                        ClientCookieIdentifier = _oLoginUserDTO.CookieIdentifier,
                        EMail = _oLoginUserDTO.MailID,
                        Password = _oLoginUserDTO.PasswordEncrypted,
                        SessionIdentifier = _oLoginUserDTO.SessionIdentifier,
                       
                    };
                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient ofalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        oLoginUserData= ofalcon.GetUserDetails(_oLoginUserDTO.MailID, oUserlogin.Password, _oLoginUserDTO.PrinterIP, oUserlogin);
                        if(oLoginUserData.Result=="0")
                        {
                            

                            profileDTO.UserID = oLoginUserData.UserID.ToString();
                            profileDTO.FirstName = oLoginUserData.FirstName;
                            profileDTO.LastName = oLoginUserData.LastName;
                            profileDTO.EMail = oLoginUserData.Email;
                            profileDTO.Password = oLoginUserData.Password;
                            profileDTO.UserRole =oLoginUserData.Roles;
                            profileDTO.WarehouseID = ConversionUtility.ConvertToInt(oLoginUserData.Warehouses.ToString());
                            profileDTO.SiteCodes = oLoginUserData.SiteCodes;
                            profileDTO.DepartmentIDs = oLoginUserData.DepartmentIDs;
                            profileDTO.MachineIPAddress = oLoginUserData.MachineIPAddress;
                            profileDTO.TenantID = oLoginUserData.TenantID;
                            profileDTO.SSOUserID = oLoginUserData.SSOID;
                            profileDTO.AccountId =oLoginUserData.AccountID.ToString();
                            _lstProfileDTO.Add(profileDTO);
                        }
                        else
                        {
                            throw new WMSExceptionMessage() {WMSExceptionCode= "WMC_CNTRL_LOGIN__0001", WMSMessage =ErrorMessages.WMC_CNTRL_LOGIN_0001, ShowAsError = true };
                        }
                    }
                    else
                        profileDTO = null;

                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.LoginUserDTO, _lstProfileDTO));
                    return json;
                }
                else return null;
            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                return json;

            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "001");
                return null;
            }
        }
        public string EnryptString(string strEncrypted)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }
        // Developed & Unit Tested
        [Route("UserLogOut")]
        [HttpPost]
        public WMSCoreMessage UserLogout(WMSCoreMessage oRequest)
        {
            try
            {

                ProfileDTO _oProfileDTO = null;

                if (oRequest != null)
                {
                    _oProfileDTO = (ProfileDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oProfileDTO != null)
                    {
                        LoginBL _oLoginBL = new LoginBL(MRLWMSC21Core.Library.Constants.ApplicationAuthentication.DBAuthhentication, LoggedInUserID, ConnectionString);

                        UserProfile _oUserPRofile = new UserProfile()
                        {
                            SessionIdentifier = _oProfileDTO.SessionIdentifier,
                            CookieIdentifier = _oProfileDTO.CookieIdentifier
                        };

                        bool _bStatus = _oLoginBL.UserLogout(_oUserPRofile);

                        return WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, _bStatus);
                    }
                    else return null;
                }
                else return null;
            }
            //catch (WMSExceptionMessage ex)
            //{
            //    List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
            //    _lstwMSExceptionMessage.Add(ex);
            //    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
            //    return json;

            //}
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "002");
                return null;
            }
        }

        // Developed & Unit Tested
        [Route("ValidateSession")]
        [HttpPost]
        public WMSCoreMessage ValidateUserSession(WMSCoreMessage oRequest)
        {
            try
            {

                ProfileDTO _oProfileDTO = null;

                if (oRequest != null)
                {
                    _oProfileDTO = (ProfileDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oProfileDTO != null)
                    {

                        LoginBL oLoginBL = new LoginBL(MRLWMSC21Core.Library.Constants.ApplicationAuthentication.DBAuthhentication, LoggedInUserID, ConnectionString);

                        UserProfile _oUserProfile = new UserProfile()
                        {
                            EMailID = _oProfileDTO.EMail,
                            CookieIdentifier = _oProfileDTO.CookieIdentifier,
                            SessionIdentifier = _oProfileDTO.SessionIdentifier,
                            ClientMAC = _oProfileDTO.ClientMAC,
                            SSOUserID = _oProfileDTO.SSOUserID,
                            LastRequestTimestamp = _oProfileDTO.LastRequestTimestamp
                        };

                        _oUserProfile = oLoginBL.ValidateUserSession(_oUserProfile);

                        return WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, _oProfileDTO);
                    }
                    else return null;
                }
                else return null;
            }
            //catch (WMSExceptionMessage ex)
            //{
            //    List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
            //    _lstwMSExceptionMessage.Add(ex);
            //    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
            //    return json;

            //}
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "003");
                return null;
            }
        }

        [Route("Checktest")]
        [HttpPost]
        public string Checktest(WMSCoreMessage oRequest)
        {
            try
            {
               
                 ProfileDTO _oProfileDTO = new ProfileDTO();
                _oProfileDTO.IsLoggedIn = true;
                List<ProfileDTO> _lstProfile = new List<ProfileDTO>();
                _lstProfile.Add(_oProfileDTO);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.LoginUserDTO, _lstProfile));
                return json;

            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                return json;

            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "004");
                return null;
            }
        }


    }

}
