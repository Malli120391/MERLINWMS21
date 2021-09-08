using System.Net;
using System.Net.Http;
using System.Web.Http;
using MRLWMSC21Core.Business;
using MRLWMSC21Core.DataAccess;
using MRLWMSC21Core.Entities;
using MRLWMSC21Core.Library;
using MRLWMSC21_Endpoint.Interfaces;
using MRLWMSC21_Endpoint.Security;
using MRLWMSC21_Endpoint.DTO;
using MRLWMSC21_Endpoint.Constants;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;

namespace MRLWMSC21_Endpoint.Controllers
{
    [RoutePrefix("Exception")]
    public class ExceptionController : BaseController, iException
    {
        private string _ClassCode = string.Empty;

        public ExceptionController()
        {
            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_Enpoint.ExceptionController);
        }


        [HttpPost]
        [Route("LogException")]
        public string LogException(WMSCoreMessage oRequest)
        {
            try
            {
                bool Result = false;
                LoadControllerDefaults(oRequest);
                InventoryDTO _oResponse = new InventoryDTO();
                List<InventoryDTO> lstReponse = new List<InventoryDTO>();
                if (oRequest != null)
                {

                    
                    WMSExceptionMessage _oWMSException = (WMSExceptionMessage)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    GenericBL _oGenericBL = new GenericBL(LoggedInUserID, ConnectionString);

                    if (_oWMSException != null)
                    {

                        Result = _oGenericBL.LogException(_oWMSException);
                        if (Result == true)
                        {
                            _oResponse.Result = "1";
                        }
                        else
                        {
                            _oResponse.Result = "0";
                        }

                       
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, _oResponse));
                        return json;
                    }
                }
                return null;
                   
            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                return json;


            }
            catch (System.Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "001");
                return null;
            }

        }
    }
}