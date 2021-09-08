using System;
using System.Collections.Generic;
using System.Linq;
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

//===============Modified By M.D.Prasad On 05-May-2020 =======================//
namespace MRLWMSC21_Endpoint.Controllers
{
    [RoutePrefix("CycleCount")]

    public class CycleCountController : BaseController, iCycleCount
    {
        private string _ClassCode = string.Empty;

        public CycleCountController()
        {
            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_Enpoint.CycleCountController);
        }

        [HttpPost]
        [Route("GetCCNames")]
        public string GetCCNames(WMSCoreMessage oRequest)
        {
            try
            {
                
                
                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.UserDataTable userDataTable = new MRLWMSC21Service.UserDataTable();

                List<CycleCountDTO> _lstcyclecount = new List<CycleCountDTO>();

                if (oRequest != null)
                {

                    CycleCountDTO _ocyclecountDTO = (CycleCountDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_ocyclecountDTO != null)
                    {

                        MRLWMSC21Service.CycleCount ocycleCount = new MRLWMSC21Service.CycleCount()
                        {

                            AccountId = _ocyclecountDTO.AccountID,
                            UserId = Convert.ToInt32(_ocyclecountDTO.UserId)
                        };


                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        userDataTable = oFalcon.GetCCNames(ocycleCount);
                    }
                    foreach (System.Data.DataRow row in userDataTable.Table.Rows)
                    {
                        CycleCountDTO responseDTO = new CycleCountDTO();
                        responseDTO.CCName = row["CCHeader"].ToString();
                        responseDTO.WarehouseID= row["WarehouseID"].ToString();
                        responseDTO.TenantId= row["TenantID"].ToString();
                        responseDTO.Rack = row["Rack"].ToString();
                        responseDTO.Column = row["Column"].ToString();
                        responseDTO.Level = row["Level"].ToString();
                        responseDTO.CycleCountSeqCode = row["CycleCountCode"].ToString();
                        _lstcyclecount.Add(responseDTO);
                    }

                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.CycleCount, _lstcyclecount));
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



        [HttpPost]
        [Route("IsBlockedLocation")]
        public string IsBlockedLocation(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.CycleCount oCyclecount = new MRLWMSC21Service.CycleCount();

                List<CycleCountDTO> _lstcyclecount = new List<CycleCountDTO>();

                if (oRequest != null)
                {

                    CycleCountDTO _ocyclecountDTO = (CycleCountDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_ocyclecountDTO != null)
                    {

                        MRLWMSC21Service.CycleCount ocycleCount = new MRLWMSC21Service.CycleCount()
                        {

                            AccountId = _ocyclecountDTO.AccountID,
                            Location = _ocyclecountDTO.Location,
                            CCName = _ocyclecountDTO.CCName,
                            UserId = Convert.ToInt32(_ocyclecountDTO.UserId),
                            WarehouseID = Convert.ToInt32(_ocyclecountDTO.WarehouseID)
                        };


                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        oCyclecount = oFalcon.IsBlockedLocation(ocycleCount);
                    }

                    CycleCountDTO responseDTO = new CycleCountDTO();
                    if (oCyclecount.Result == "-4")
                    {
                        throw new WMSExceptionMessage() { WMSExceptionCode = null, WMSMessage = "Not Blocked for CycleCount", ShowAsError = true };
                    }
                    responseDTO.Result = oCyclecount.Result;

                    _lstcyclecount.Add(responseDTO);


                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.CycleCount, _lstcyclecount));
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


        [HttpPost]
        [Route("BlockLocationForCycleCount")]
        public string BlockLocationForCycleCount(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.CycleCount oCyclecount = new MRLWMSC21Service.CycleCount();

                List<CycleCountDTO> _lstcyclecount = new List<CycleCountDTO>();

                if (oRequest != null)
                {

                    CycleCountDTO _ocyclecountDTO = (CycleCountDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_ocyclecountDTO != null)
                    {

                        MRLWMSC21Service.CycleCount ocycleCount = new MRLWMSC21Service.CycleCount()
                        {

                            AccountId = _ocyclecountDTO.AccountID,
                            Location = _ocyclecountDTO.Location,
                            CCName = _ocyclecountDTO.CCName
                        };


                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        oCyclecount = oFalcon.BlockLocationForCycleCount(ocycleCount);
                    }

                    CycleCountDTO responseDTO = new CycleCountDTO();
                    responseDTO.Result = oCyclecount.Result;
                    if (responseDTO.Result == "")
                    {
                        responseDTO.Count = oCyclecount.Count;
                    }
                    else
                    {
                        throw new WMSExceptionMessage() { WMSExceptionCode = oCyclecount.Result, WMSMessage = oCyclecount.Result, ShowAsError = true };
                    }
                    _lstcyclecount.Add(responseDTO);


                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.CycleCount, _lstcyclecount));
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


        [HttpPost]
        [Route("CheckMaterialAvailablilty")]
        public string CheckMaterialAvailablilty(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.CycleCount oCyclecount = new MRLWMSC21Service.CycleCount();

                List<CycleCountDTO> _lstcyclecount = new List<CycleCountDTO>();

                if (oRequest != null)
                {

                    CycleCountDTO _ocyclecountDTO = (CycleCountDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_ocyclecountDTO != null)
                    {

                        MRLWMSC21Service.CycleCount ocycleCount = new MRLWMSC21Service.CycleCount()
                        {

                            AccountId = _ocyclecountDTO.AccountID,
                            Location = _ocyclecountDTO.Location,
                            SKU=_ocyclecountDTO.MaterialCode,
                            CCName=_ocyclecountDTO.CCName,
                            TenantID = Convert.ToInt32(_ocyclecountDTO.TenantId),
                            UserId = Convert.ToInt32(_ocyclecountDTO.UserId)
                        };


                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        oCyclecount = oFalcon.CheckMaterialAvailablilty(ocycleCount);
                    }

                    CycleCountDTO responseDTO = new CycleCountDTO();
                    responseDTO.Result = oCyclecount.Result;
                    if (responseDTO.Result == "")
                    {
                        responseDTO.Result = "1";
                    }
                    else
                    {
                        throw new WMSExceptionMessage() { WMSExceptionCode = oCyclecount.Result, WMSMessage = oCyclecount.Result, ShowAsError = true };
                    }
                    _lstcyclecount.Add(responseDTO);


                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.CycleCount, _lstcyclecount));
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

        [HttpPost]
        [Route("GetCycleCountInformation")]
        public string GetCycleCountInformation(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);


                List<CycleCountDTO> _lstcyclecount = new List<CycleCountDTO>();

                if (oRequest != null)
                {

                    CycleCountDTO _ocyclecountDTO = (CycleCountDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_ocyclecountDTO != null)
                    {

                        MRLWMSC21Service.CycleCount ocycleCountrequest = new MRLWMSC21Service.CycleCount()
                        {

                            AccountId = _ocyclecountDTO.AccountID,
                            Location = _ocyclecountDTO.Location,
                            CCName = _ocyclecountDTO.CCName
                        };


                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        MRLWMSC21Service.CycleCount[] _lstCC = oFalcon.GetCycleCountInformation(ocycleCountrequest);


                        foreach (MRLWMSC21Service.CycleCount oCycleCount in _lstCC)
                        {
                            _lstcyclecount.Add(new CycleCountDTO()
                            {

                                CCName = oCycleCount.CCName,
                                Location = oCycleCount.Location,
                                MaterialCode = oCycleCount.SKU,
                                MfgDate = oCycleCount.MfgDate,
                                ExpDate = oCycleCount.ExpDate,
                                SerialNo = oCycleCount.SerialNo,
                                BatchNo = oCycleCount.BatchNo,
                                ProjectRefNo = oCycleCount.ProjectRefNo,
                               
                                CCQty = oCycleCount.CCQty.ToString(),
                                


                            });

                        }
                    }

                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.CycleCount, _lstcyclecount));
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

        [HttpPost]
        [Route("ReleaseCycleCountLocationOLD")]
        public string ReleaseCycleCountLocation(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);

                MRLWMSC21Service.CycleCount oCycleCount = new MRLWMSC21Service.CycleCount();
                List<CycleCountDTO> _lstcyclecount = new List<CycleCountDTO>();

                if (oRequest != null)
                {

                    CycleCountDTO _ocyclecountDTO = (CycleCountDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_ocyclecountDTO != null)
                    {

                        MRLWMSC21Service.CycleCount ocycleCount = new MRLWMSC21Service.CycleCount()
                        {

                            AccountId = _ocyclecountDTO.AccountID,
                            Location = _ocyclecountDTO.Location,
                            CCName = _ocyclecountDTO.CCName,
                            WarehouseID=Convert.ToInt32(_ocyclecountDTO.WarehouseID)
                        };


                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        oCycleCount = oFalcon.ReleaseCycleCountLocation(ocycleCount);

                        CycleCountDTO orespnonseDTO = new CycleCountDTO();
                        orespnonseDTO.Result = oCycleCount.Result;

                        _lstcyclecount.Add(orespnonseDTO);

                    }
                }

                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.CycleCount, _lstcyclecount));
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
                ExceptionHandling.LogException(excp, _ClassCode + "001");
                return null;
            }
        }

        [HttpPost]
        [Route("ReleaseCycleCountLocation")]
        public string MarkBinComplete(WMSCoreMessage oRequest)
        {
            try
            {
                LoadControllerDefaults(oRequest);
                CycleCountDTO cycleCountDTO = new CycleCountDTO();
                CycleCountBL cycleCountBL = new CycleCountBL(LoggedInUserID, ConnectionString);
                List<CycleCountDTO> _lCycleCountDTO = new List<CycleCountDTO>();

                bool status = false;
                if (oRequest != null)
                {
                    CycleCountDTO _oCycleCountDTO = (CycleCountDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oCycleCountDTO != null)
                    {
                        Location loc = new Location()
                        {
                            LocationCode = _oCycleCountDTO.Location
                        };
                        CycleCount oCycleCount = new CycleCount()
                        {
                            CycleCountCode = _oCycleCountDTO.CycleCountSeqCode,
                            AccountCycleCountName = _oCycleCountDTO.CCName,
                            AccountID = _oCycleCountDTO.AccountID,
                        };
                        Inventory oInventory = new Inventory()
                        {
                            LocationCode = _oCycleCountDTO.Location,
                            CreatedBy = ConversionUtility.ConvertToInt(_oCycleCountDTO.UserId)
                        };

                        status = cycleCountBL.MarkBinComplete(loc, oCycleCount, oInventory);
                        if (status)
                        {
                            cycleCountDTO.Result = "Closed successfully";
                        }
                        else
                        {
                            cycleCountDTO = null;
                        }
                        _lCycleCountDTO.Add(cycleCountDTO);

                    }
                    //else
                    //    cycleCountDTO = null;

                }
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.CycleCount, _lCycleCountDTO));
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
                ExceptionHandling.LogException(excp, _ClassCode + "006");
                return null;
            }
        }

        [HttpPost]
        [Route("UpsertCycleCount")]
        public string UpsertCycleCount(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.CycleCount oresponse = new MRLWMSC21Service.CycleCount();

                List<CycleCountDTO> _lstcyclecount = new List<CycleCountDTO>();

                if (oRequest != null)
                {

                    CycleCountDTO _ocyclecountDTO = (CycleCountDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_ocyclecountDTO != null)
                    {

                        MRLWMSC21Service.CycleCount ocycleCount = new MRLWMSC21Service.CycleCount()
                        {

                            Location = _ocyclecountDTO.Location,
                            Container = _ocyclecountDTO.PalletNo,
                            SKU = _ocyclecountDTO.MaterialCode,
                            UserId = ConversionUtility.ConvertToInt(_ocyclecountDTO.UserId),
                            CCName = _ocyclecountDTO.CCName,
                            CCQty = Convert.ToDecimal(_ocyclecountDTO.CCQty),
                            BatchNo = _ocyclecountDTO.BatchNo,
                            SerialNo = _ocyclecountDTO.SerialNo,
                            ProjectRefNo = _ocyclecountDTO.ProjectRefNo,
                            MfgDate = _ocyclecountDTO.MfgDate,
                            ExpDate = _ocyclecountDTO.ExpDate,
                            MRP=_ocyclecountDTO.MRP,
                            WarehouseID=Convert.ToInt32(_ocyclecountDTO.WarehouseID),
                            TenantID= Convert.ToInt32(_ocyclecountDTO.TenantId),
                            StorageLocation = _ocyclecountDTO.StorageLocation

                        };


                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        oresponse = oFalcon.UpsertCycleCount(ocycleCount);
                    }

                    CycleCountDTO responseDTO = new CycleCountDTO();
                    responseDTO.Result = oresponse.Result;

                    _lstcyclecount.Add(responseDTO);


                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.CycleCount, _lstcyclecount));
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


        [HttpPost]
        [Route("ChekPalletLocation")]
        public string ChekPalletLocation(WMSCoreMessage oRequest)
        {
            try
            {
                LoadControllerDefaults(oRequest);

                string LocationCode = "", LocationCheck = "";
                MRLWMSC21Service.CycleCount oresponse = new MRLWMSC21Service.CycleCount();

                List<CycleCountDTO> _lstcyclecount = new List<CycleCountDTO>();
                if (oRequest != null)
                {
                    CycleCountDTO _ocyclecountDTO = (CycleCountDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                    if (_ocyclecountDTO != null)
                    {

                        LocationCheck = _ocyclecountDTO.Location;
                        LocationCode = oFalcon.GetConatinerLocationBin(_ocyclecountDTO.PalletNo, _ocyclecountDTO.WarehouseID);
                    }
                    if (LocationCode != null || LocationCode != string.Empty)
                    {
                        if (LocationCheck == LocationCode)
                        {
                            _ocyclecountDTO.Result = "1";


                        }
                        else
                        {

                            if (LocationCode == "-1")
                            {
                                throw new WMSExceptionMessage() { WMSExceptionCode = null, WMSMessage = "Container does not exist in the Warehouse", ShowAsError = true };
                            }
                            else 
                            {
                                throw new WMSExceptionMessage() { WMSExceptionCode = null, WMSMessage = "Container does not exist in the Location", ShowAsError = true };
                            }                          

                        }
                    }
                    _lstcyclecount.Add(_ocyclecountDTO);

                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, _lstcyclecount));
                    return json;
                }
                else
                    return null;
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
                ExceptionHandling.LogException(excp, _ClassCode + "005");
                return null;
            }
        }
    }
}