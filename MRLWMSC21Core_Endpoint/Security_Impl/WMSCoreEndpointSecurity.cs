using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MRLWMSC21_Endpoint.DTO;
using MRLWMSC21_Endpoint.Constants;
using MRLWMSC21Core.Business;
using MRLWMSC21Core.Entities;
using MRLWMSC21Core.Library;


namespace MRLWMSC21_Endpoint.Security
{
    public static class WMSCoreEndpointSecurity
    {

        public static bool ValidateRequest(WMSCoreAuthentication AuthToken)
        {
            /*
            Debug Mode
                true : If application is in Debug Mode and Request is supposed to be validated.
                false : If application is not in debug mode and request validation is disabled.
             */

            bool _IsDebugMode = true, _IsValid = true;

            if (!_IsDebugMode)
            {
                if (_IsValid) // Encrypted Auth Token Matches Request
                {
                    LoginBL oLoginBL = new LoginBL(MRLWMSC21Core.Library.Constants.ApplicationAuthentication.DBAuthhentication, 1, string.Empty);

                    UserProfile _oUserProfile = new UserProfile()
                    {
                        CookieIdentifier = AuthToken.CookieIdentifier,
                        SessionIdentifier = AuthToken.AuthValue,
                        ClientMAC = AuthToken.AuthKey,
                        SSOUserID = AuthToken.SSOUSerID,
                        LastRequestTimestamp = AuthToken.LoginTimeStamp
                    };

                    _oUserProfile = oLoginBL.ValidateUserSession(_oUserProfile);

                    return _oUserProfile.IsLoggedIn; // Return Login Status
                }
                else
                    return false;
            }
            else
            {
                return true;
            }
            // return true;
        }

        public static object ValidateRequest(WMSCoreMessage Message)
        {
            if (ValidateRequest(Message.AuthToken))
            {

                Object _oObject = null;

                switch (Message.Type)
                {
                    case Constants.EndpointConstants.DTO.LoginUserDTO:
                        {
                            _oObject = JsonConvert.DeserializeObject<LoginUserDTO>(Message.EntityObject.ToString());
                            break;
                        }
                    case Constants.EndpointConstants.DTO.ProfileDTO:
                        {
                            _oObject = JsonConvert.DeserializeObject<ProfileDTO>(Message.EntityObject.ToString());
                            break;
                        }
                    case Constants.EndpointConstants.DTO.Inbound:
                        {
                            _oObject = JsonConvert.DeserializeObject<InboundDTO>(Message.EntityObject.ToString());
                            break;
                        }
                    case Constants.EndpointConstants.DTO.Inventory:
                        {
                            _oObject = JsonConvert.DeserializeObject<InventoryDTO>(Message.EntityObject.ToString());
                            break;
                        }
                    case Constants.EndpointConstants.DTO.CycleCount:
                        {
                            _oObject = JsonConvert.DeserializeObject<CycleCountDTO>(Message.EntityObject.ToString());
                            break;
                        }
                    case Constants.EndpointConstants.DTO.Outbound:
                        {
                            _oObject = JsonConvert.DeserializeObject<OutboundDTO>(Message.EntityObject.ToString());
                            break;
                        }
                    case Constants.EndpointConstants.DTO.DenestingDTO:
                        {
                            _oObject = JsonConvert.DeserializeObject<DenestingDTO>(Message.EntityObject.ToString());
                            break;
                        }

                    case Constants.EndpointConstants.DTO.Exception:
                        {
                            _oObject = JsonConvert.DeserializeObject<WMSExceptionMessage>(Message.EntityObject.ToString());
                            break;
                        }
                    case Constants.EndpointConstants.DTO.PutAwayDTO:
                        {
                            _oObject = JsonConvert.DeserializeObject<PutAwayDTO>(Message.EntityObject.ToString());
                            break;
                        }
                    case Constants.EndpointConstants.DTO.HouseKeepingDTO:
                        {
                            _oObject = JsonConvert.DeserializeObject<HouseKeepingDTO>(Message.EntityObject.ToString());
                            break;
                        }
                    case Constants.EndpointConstants.DTO.POD:
                        {
                            _oObject = JsonConvert.DeserializeObject<PODDTO>(Message.EntityObject.ToString());
                            break;
                        }
                    case Constants.EndpointConstants.DTO.InboundList:
                        {
                            _oObject = JsonConvert.DeserializeObject<InboundListDTO>(Message.EntityObject.ToString());
                            break;
                        }
                    case Constants.EndpointConstants.DTO.OutboundList:
                        {
                            _oObject = JsonConvert.DeserializeObject<OutboundListDTO>(Message.EntityObject.ToString());
                            break;
                        }
                    case Constants.EndpointConstants.DTO.POList:
                        {
                            _oObject = JsonConvert.DeserializeObject<POListDTO>(Message.EntityObject.ToString());
                            break;
                        }
                    case Constants.EndpointConstants.DTO.SOList:
                        {
                            _oObject = JsonConvert.DeserializeObject<SOListDTO>(Message.EntityObject.ToString());
                            break;
                        }
                    case Constants.EndpointConstants.DTO.Tenant:
                        {
                            _oObject = JsonConvert.DeserializeObject<TenantDTO>(Message.EntityObject.ToString());
                            break;
                        }
                    case Constants.EndpointConstants.DTO.Shipment:
                        {
                            _oObject = JsonConvert.DeserializeObject<ShipmentDTO>(Message.EntityObject.ToString());
                            break;
                        }
                    case Constants.EndpointConstants.DTO.ScanDTO:
                        {
                            _oObject = JsonConvert.DeserializeObject<ScanDTO>(Message.EntityObject.ToString());
                            break;
                        }

                    case Constants.EndpointConstants.DTO.APIInventoryDTO:
                        {
                            _oObject = JsonConvert.DeserializeObject<APIInventoryDTO>(Message.EntityObject.ToString());
                            break;
                        }

                    case Constants.EndpointConstants.DTO.MastersDTO:
                        {
                            _oObject = JsonConvert.DeserializeObject<MastersDTO>(Message.EntityObject.ToString());
                            break;
                        }

                    case Constants.EndpointConstants.DTO.ReleaseOutbound:
                        {
                            _oObject = JsonConvert.DeserializeObject<ReleaseOutbound>(Message.EntityObject.ToString());
                            break;
                        }


                    case Constants.EndpointConstants.DTO.OutboundOrdersDTO:
                        {
                            _oObject = JsonConvert.DeserializeObject<OutboundOrdersDTO>(Message.EntityObject.ToString());
                            break;
                        }
                    case Constants.EndpointConstants.DTO.BulkOrderDTO:
                        {
                            _oObject = JsonConvert.DeserializeObject<APISOCreateRequestDTO>(Message.EntityObject.ToString());
                            break;
                        }

                    case Constants.EndpointConstants.DTO.SupplierImportDTO:
                        {
                            _oObject = JsonConvert.DeserializeObject<List<SupplierImportDTO>>(Message.EntityObject.ToString());
                            break;
                        }

                    case Constants.EndpointConstants.DTO.ItemMasterDTO:
                        {
                            _oObject = JsonConvert.DeserializeObject<List<ItemMasterDTO>>(Message.EntityObject.ToString());
                            break;
                        }

                    case Constants.EndpointConstants.DTO.InboundDataDTO:
                        {
                            _oObject = JsonConvert.DeserializeObject<List<InboundDataDTO>>(Message.EntityObject.ToString());
                            break;
                        }
                    case Constants.EndpointConstants.DTO.CustomerCreationDTO:
                        {
                            _oObject = JsonConvert.DeserializeObject<List<CustomerCreationDTO>>(Message.EntityObject.ToString());
                            break;
                        }

                    case Constants.EndpointConstants.DTO.SupplierCreationDTO:
                        {
                            _oObject = JsonConvert.DeserializeObject<List<SupplierCreationDTO>>(Message.EntityObject.ToString());
                            break;
                        }


                    case Constants.EndpointConstants.DTO.SLOCToSLOCDTO:
                        {
                            _oObject = JsonConvert.DeserializeObject<SLOCToSLOCDTO>(Message.EntityObject.ToString());
                            break;
                        }

                    case Constants.EndpointConstants.DTO.BinToBinDTO:
                        {
                            _oObject = JsonConvert.DeserializeObject<BinToBinDTO>(Message.EntityObject.ToString());
                            break;
                        }

                    case Constants.EndpointConstants.DTO.SupplierReturnOBDDTO:
                        {
                            _oObject = JsonConvert.DeserializeObject<SupplierReturnOBDDTO>(Message.EntityObject.ToString());
                            break;
                        }


                    case Constants.EndpointConstants.DTO.CustomerReturnDTO:
                        {
                            _oObject = JsonConvert.DeserializeObject<List<CustomerReturnDTO>>(Message.EntityObject.ToString());
                            break;
                        }
                    case Constants.EndpointConstants.DTO.SupplierReturnDTO:
                        {
                            _oObject = JsonConvert.DeserializeObject<List<SupplierReturnDTO>>(Message.EntityObject.ToString());
                            break;
                        }

                    case Constants.EndpointConstants.DTO.UpdateItemMasterDTO:
                        {
                            _oObject = JsonConvert.DeserializeObject<List<UpdateItemMasterDTO>>(Message.EntityObject.ToString());
                            break;
                        }

                    case Constants.EndpointConstants.DTO.UpdateSupplierDTO:
                        {
                            _oObject = JsonConvert.DeserializeObject<List<UpdateSupplierDTO>>(Message.EntityObject.ToString());
                            break;
                        }
                    case Constants.EndpointConstants.DTO.MiscReceipt:
                        {
                            _oObject = JsonConvert.DeserializeObject<List<MisslleniousReceiptDTO>>(Message.EntityObject.ToString());
                            break;
                        }
                    case Constants.EndpointConstants.DTO.MiscIssue:
                        {
                            _oObject = JsonConvert.DeserializeObject<List<MisslleniousIssueDTO>>(Message.EntityObject.ToString());
                            break;
                        }


                }
                return _oObject;
            }
            else
                return null;
        }

        public static WMSCoreMessage PrepareResponse(WMSCoreAuthentication oAuthToken, object oToReturn)
        {
            WMSCoreMessage _oWMSCoreMessage = new WMSCoreMessage()
            {
                AuthToken = oAuthToken,
                Type = Constants.EndpointConstants.DTO.LoginUserDTO,
                EntityObject = oToReturn
            };

            return _oWMSCoreMessage;
        }


        public static WMSCoreMessage PrepareResponse(WMSCoreAuthentication oAuthToken, Constants.EndpointConstants.DTO DTOType, object oToReturn)
        {
            WMSCoreMessage _oWMSCoreMessage = new WMSCoreMessage()
            {
                AuthToken = oAuthToken,
                Type = DTOType,
                EntityObject = oToReturn
            };

            return _oWMSCoreMessage;
        }
    }


    public class WMSCoreMessage
    {
        private WMSCoreAuthentication _AuthToken;

        private Constants.EndpointConstants.DTO _Type;

        private object _EntityObject;

        private List<WMSExceptionMessage> _WMSMessages;

        public WMSCoreAuthentication AuthToken { get => _AuthToken; set => _AuthToken = value; }
        public object EntityObject { get => _EntityObject; set => _EntityObject = value; }
        public Constants.EndpointConstants.DTO Type { get => _Type; set => _Type = value; }
        public List<WMSExceptionMessage> WMSMessages { get => _WMSMessages; set => _WMSMessages = value; }
    }

    public class WMSCoreAuthentication
    {
        private string _AuthKey;            // MAP CLIENT MAC
        private string _UserID;             // MAP WMS USER ID
        private string _AuthValue;          // MAP SESSION IDENTIFIER
        private int _RequestNumber;         // Request Sequence Number
        private string _LoginTimeStamp;   // MAP Login Timestamp, as Returned by SSO
        private string _AuthToken;          // MAP Auth Token Generated by WMS

        private int _SSOUSerID;          // MAP Inventrax SSO User ID
        private string _CookieIdentifier;   // MAP Cookie Identifier

        public string AuthKey { get => _AuthKey; set => _AuthKey = value; }
        public string UserID { get => _UserID; set => _UserID = value; }
        public string AuthValue { get => _AuthValue; set => _AuthValue = value; }
        public string LoginTimeStamp { get => _LoginTimeStamp; set => _LoginTimeStamp = value; }
        public string AuthToken { get => _AuthToken; set => _AuthToken = value; }
        public int RequestNumber { get => _RequestNumber; set => _RequestNumber = value; }
        public int SSOUSerID { get => _SSOUSerID; set => _SSOUSerID = value; }
        public string CookieIdentifier { get => _CookieIdentifier; set => _CookieIdentifier = value; }
    }

}