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
using System.Data;


namespace MRLWMSC21_Endpoint.Controllers
{
    [RoutePrefix("Outbound")]
    public class OutboundController : BaseController, iOutbound
    {
        private string _ClassCode = string.Empty;

        public OutboundController()
        {
            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_Enpoint.OutboundController);
        }

        [HttpPost]
        [Route("GetobdRefNos")]
        public string GetobdRefNos(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.UserDataTable userDataTable = new MRLWMSC21Service.UserDataTable();

                List<OutboundDTO> _lstoutboundDTO = new List<OutboundDTO>();

                if (oRequest != null)
                {

                    OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    OutboundBL outboundBL = new OutboundBL(LoggedInUserID, ConnectionString);

                    List<Outbound> _lstOutbound = new List<Outbound>();

                    //if (_ooutboundDTO != null)
                    //{

                    //    MRLWMSC21Service.Outbound ooutbound = new MRLWMSC21Service.Outbound()
                    //    {

                    //        AccountId = ConversionUtility.ConvertToInt(_ooutboundDTO.AccountID),

                    //    };


                    // MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                    //  userDataTable = oFalcon.GetobdRefNos(ooutbound);
                    //}
                    //foreach (DataRow row in userDataTable.Table.Rows)
                    //{
                    //    OutboundDTO responseDTO = new OutboundDTO();
                    //    responseDTO.OBDNo = row["OBDNumber"].ToString();
                    //    responseDTO.OutboundID = row["OutboundID"].ToString();
                    //    _lstoutbound.Add(responseDTO);
                    //}


                    if (_ooutboundDTO != null)
                    {

                        Outbound outbound = new Outbound()
                        {
                            AccountID = ConversionUtility.ConvertToInt(_ooutboundDTO.AccountID),
                            UserId = ConversionUtility.ConvertToInt(_ooutboundDTO.UserId)
                        };

                        _lstOutbound = outboundBL.GetOutboundRefNo(outbound);

                        foreach (Outbound outboundItem in _lstOutbound)
                        {
                            OutboundDTO outboundDTO = new OutboundDTO();
                            outboundDTO.OBDNo = outboundItem.OBDNumber.ToString();
                            outboundDTO.OutboundID = outboundItem.OutboundID.ToString();

                            _lstoutboundDTO.Add(outboundDTO);
                        }


                    }

                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Outbound, _lstoutboundDTO));
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
        [Route("OBDSkipItem")]
        public string OBDSkipItem(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.Outbound ooutboundresponse = new MRLWMSC21Service.Outbound();
                OutboundDTO responseDTO = new OutboundDTO();
                List<OutboundDTO> _lstoutbound = new List<OutboundDTO>();

                if (oRequest != null)
                {

                    OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_ooutboundDTO != null)
                    {

                        MRLWMSC21Service.Outbound ooutbound = new MRLWMSC21Service.Outbound()
                        {


                            TotalPickedQty = ConversionUtility.ConvertToDecimal(_ooutboundDTO.PickedQty),
                            Flag = 1,
                            SkipReason = _ooutboundDTO.SkipReason,
                            OutboundId = _ooutboundDTO.OutboundID,
                            SkipQty = ConversionUtility.ConvertToDecimal(_ooutboundDTO.SkipQty),
                            Assignedid = Convert.ToInt32(_ooutboundDTO.AssignedID),
                            MCode = _ooutboundDTO.SKU,
                            MfgDate = _ooutboundDTO.MfgDate,
                            ExpDate = _ooutboundDTO.ExpDate,
                            SerialNo = _ooutboundDTO.SerialNo,
                            BatchNo = _ooutboundDTO.BatchNo,
                            ProjectNo = _ooutboundDTO.ProjectNo,
                            CreatedBy = _ooutboundDTO.UserId,
                            SLoc = _ooutboundDTO.SLoc,
                            CartonNo = _ooutboundDTO.PalletNo,
                            Location = _ooutboundDTO.Location,
                            MRP = _ooutboundDTO.MRP
                        };

                        
                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        ooutboundresponse = oFalcon.OBDSkipItem(ooutbound);
                        if (ooutboundresponse.Result == "Success")
                        {
                            return GetOBDItemToPick(oRequest);
                        }
                        else
                        {
                            throw new WMSExceptionMessage() { WMSExceptionCode = ooutboundresponse.Erocode, WMSMessage = ooutboundresponse.Result, ShowAsError = true };
                        }



                    }

                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Outbound, _lstoutbound));
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
        [Route("GetOBDItemToPick")]
        public string GetOBDItemToPick(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.Outbound oOutboundresponse = new MRLWMSC21Service.Outbound();
                OutboundDTO responseDTO = new OutboundDTO();
                List<OutboundDTO> _lstoutbound = new List<OutboundDTO>();

                if (oRequest != null)
                {

                    OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_ooutboundDTO != null)
                    {
                        MRLWMSC21Service.Outbound oOutbound = new MRLWMSC21Service.Outbound()
                        {

                            CreatedBy = _ooutboundDTO.UserId,
                            OutboundId = _ooutboundDTO.OutboundID



                        };
                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        oOutboundresponse = oFalcon.GetOBDItemToPick(oOutbound);
                        if (oOutboundresponse != null)
                        {
                            responseDTO.AssignedID = oOutboundresponse.Assignedid.ToString();
                            responseDTO.MaterialMasterId = oOutboundresponse.MaterialMasterId;
                            responseDTO.SKU = oOutboundresponse.MCode;
                            responseDTO.MaterialDescription = oOutboundresponse.MDescription;
                            responseDTO.PalletNo = oOutboundresponse.CartonNo;
                            responseDTO.CartonID = oOutboundresponse.CartonId;
                            responseDTO.Location = oOutboundresponse.Location;
                            responseDTO.LocationId = oOutboundresponse.LocationId.ToString();
                            responseDTO.MfgDate = oOutboundresponse.MfgDate;
                            responseDTO.ExpDate = oOutboundresponse.ExpDate;
                            responseDTO.SerialNo = oOutboundresponse.SerialNo;
                            responseDTO.BatchNo = oOutboundresponse.BatchNo;
                            responseDTO.ProjectNo = oOutboundresponse.ProjectNo;
                            responseDTO.AssignedQuantity = oOutboundresponse.AssignedQuantity;
                            responseDTO.PickedQty = oOutboundresponse.PickedQty;
                            responseDTO.OutboundID = oOutboundresponse.OutboundId;
                            responseDTO.SODetailsID = oOutboundresponse.SODetailsID;
                            responseDTO.SLocId = oOutboundresponse.SLocId;
                            responseDTO.SLoc = oOutboundresponse.SLoc;
                            responseDTO.GoodsmomentDeatilsId = oOutboundresponse.GoodsmomentDeatilsId.ToString();
                            responseDTO.Lineno = oOutboundresponse.Lineno;
                            responseDTO.MaterialMaster_IUoMID = oOutboundresponse.MaterialMaster_IUoMID;
                            responseDTO.CF = oOutboundresponse.CF.ToString();
                            responseDTO.POSOHeaderId = oOutboundresponse.POSOHeaderId;
                            responseDTO.PendingQty = oOutboundresponse.PendingQty;
                            responseDTO.MRP = oOutboundresponse.MRP;
                            responseDTO.HUNo = oOutboundresponse.HUNo.ToString();
                            responseDTO.HUSize = oOutboundresponse.HUSize.ToString();
                        }
                        _lstoutbound.Add(responseDTO);
                    }


                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Outbound, _lstoutbound));
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
        [Route("CheckStrickyCompliance")]
        public string CheckStrickyCompliance(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                String result = null;
                OutboundDTO responseDTO = new OutboundDTO();
                List<OutboundDTO> _lstoutbound = new List<OutboundDTO>();

                if (oRequest != null)
                {

                    OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_ooutboundDTO != null)
                    {
                        MRLWMSC21Service.ValidateUserLogin oUserLogin = new MRLWMSC21Service.ValidateUserLogin()
                        {

                            AccountId = Convert.ToInt32(_ooutboundDTO.AccountID),
                            Apps_MST_User_ID = Convert.ToInt32(_ooutboundDTO.UserId)


                        };
                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        result = oFalcon.CheckStrickyCompliance(oUserLogin);
                        if (result != null)
                        {
                            responseDTO.Result = result;
                        }
                        _lstoutbound.Add(responseDTO);
                    }


                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Outbound, _lstoutbound));
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
        [Route("UpdatePickItem")]
        public string UpdatePickItem(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.Outbound oOutboundresponse = new MRLWMSC21Service.Outbound();
                OutboundDTO responseDTO = new OutboundDTO();
                List<OutboundDTO> _lstoutbound = new List<OutboundDTO>();

                if (oRequest != null)
                {

                    OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_ooutboundDTO != null)
                    {
                        MRLWMSC21Service.Outbound oOutbound = new MRLWMSC21Service.Outbound()
                        {
                            AccountId = Convert.ToInt32(_ooutboundDTO.AccountID),
                            CartonNo = _ooutboundDTO.PalletNo,
                            Qty = ConversionUtility.ConvertToDecimal(_ooutboundDTO.PickedQty),
                            MCode = _ooutboundDTO.SKU,
                            CreatedBy = _ooutboundDTO.UserId,
                            MaterialStorageParameterIDs = "",
                            MaterialStorageParameterValues = "",
                            KitNo = _ooutboundDTO.KitId,
                            ToCartonNo = _ooutboundDTO.ToCartonNO,
                            SerialNo = _ooutboundDTO.SerialNo,
                            MfgDate = _ooutboundDTO.MfgDate,
                            ExpDate = _ooutboundDTO.ExpDate,
                            BatchNo = _ooutboundDTO.BatchNo,
                            ProjectNo = _ooutboundDTO.ProjectNo,
                            Assignedid = Convert.ToInt32(_ooutboundDTO.AssignedID),
                            Obdno = _ooutboundDTO.OBDNo,
                            Lineno = _ooutboundDTO.Lineno,
                            POSOHeaderId = _ooutboundDTO.POSOHeaderId,
                            Location = _ooutboundDTO.Location,
                            IsDam = _ooutboundDTO.IsDam,
                            HasDisc = _ooutboundDTO.HasDis,
                            OutboundId = _ooutboundDTO.OutboundID,
                            SODetailsID = _ooutboundDTO.SODetailsID,
                            MRP = _ooutboundDTO.MRP,
                            HUNo = ConversionUtility.ConvertToInt(_ooutboundDTO.HUNo),
                            HUSize = ConversionUtility.ConvertToInt(_ooutboundDTO.HUSize)


                        };
                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        oOutboundresponse = oFalcon.UpdatePickItem(oOutbound);
                        if (oOutboundresponse != null)
                        {
                            if (oOutboundresponse.Result == "Success")
                            {
                                responseDTO.AssignedID = oOutboundresponse.Assignedid.ToString();
                                responseDTO.MaterialMasterId = oOutboundresponse.MaterialMasterId;
                                responseDTO.SKU = oOutboundresponse.MCode;
                                responseDTO.MaterialDescription = oOutboundresponse.MDescription;
                                responseDTO.PalletNo = oOutboundresponse.CartonNo;
                                responseDTO.CartonID = oOutboundresponse.CartonId;
                                responseDTO.Location = oOutboundresponse.Location;
                                responseDTO.LocationId = oOutboundresponse.LocationId.ToString();
                                responseDTO.MfgDate = oOutboundresponse.MfgDate;
                                responseDTO.ExpDate = oOutboundresponse.ExpDate;
                                responseDTO.SerialNo = oOutboundresponse.SerialNo;
                                responseDTO.BatchNo = oOutboundresponse.BatchNo;
                                responseDTO.ProjectNo = oOutboundresponse.ProjectNo;
                                responseDTO.AssignedQuantity = oOutboundresponse.AssignedQuantity;
                                responseDTO.PickedQty = oOutboundresponse.PickedQty;
                                responseDTO.OutboundID = oOutboundresponse.OutboundId;
                                responseDTO.SODetailsID = oOutboundresponse.SODetailsID;
                                responseDTO.SLocId = oOutboundresponse.SLocId;
                                responseDTO.SLoc = oOutboundresponse.SLoc;
                                responseDTO.GoodsmomentDeatilsId = oOutboundresponse.GoodsmomentDeatilsId.ToString();
                                responseDTO.Lineno = oOutboundresponse.Lineno;
                                responseDTO.MaterialMaster_IUoMID = oOutboundresponse.MaterialMaster_IUoMID;
                                responseDTO.CF = oOutboundresponse.CF.ToString();
                                responseDTO.POSOHeaderId = oOutboundresponse.POSOHeaderId;
                                responseDTO.PendingQty = oOutboundresponse.PendingQty;
                                responseDTO.Result = oOutboundresponse.Result;
                                responseDTO.MRP = oOutboundresponse.MRP;
                                responseDTO.HUNo = oOutboundresponse.HUNo.ToString();
                                responseDTO.HUSize = oOutboundresponse.HUSize.ToString();
                            }
                            else if (oOutboundresponse.Result == "-444")
                            {
                                throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_DAL_010", WMSMessage = ErrorMessages.EMC_OB_DAL_010, ShowAsError = true };
                            }
                            else if (oOutboundresponse.Result == "-333")
                            {
                                throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_DAL_011", WMSMessage = ErrorMessages.EMC_OB_DAL_011, ShowAsError = true };
                            }
                            else
                            {
                                throw new WMSExceptionMessage() { WMSExceptionCode = null, WMSMessage = oOutboundresponse.Result, ShowAsError = true };
                            }
                        }

                        _lstoutbound.Add(responseDTO);
                    }


                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Outbound, _lstoutbound));
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
        [Route("GetOpenVLPDNos")]
        public string GetOpenVLPDNos(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.UserDataTable userDataTable = new MRLWMSC21Service.UserDataTable();

                List<OutboundDTO> _lstoutbound = new List<OutboundDTO>();

                if (oRequest != null)
                {

                    OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_ooutboundDTO != null)
                    {

                        MRLWMSC21Service.VLPD oVLPD = new MRLWMSC21Service.VLPD()
                        {

                            AccountId = ConversionUtility.ConvertToInt(_ooutboundDTO.AccountID)
                        };


                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        userDataTable = oFalcon.GetOpenVLPDNos(oVLPD);
                    }
                    //foreach (DataRow row in userDataTable.Table.Rows)
                    //{
                    //    OutboundDTO responseDTO = new OutboundDTO();
                    //    _ooutboundDTO.VLPDNumber = row["VLPDNumber"].ToString();
                    //    _ooutboundDTO.VLPDId = row["Id"].ToString();
                    //    _lstoutbound.Add(_ooutboundDTO);
                    //}

                    foreach (DataRow row in userDataTable.Table.Rows)
                    {
                        OutboundDTO vlpdresponseDTO = new OutboundDTO();
                        vlpdresponseDTO.VLPDNumber = row["VLPDNumber"].ToString();
                        vlpdresponseDTO.VLPDId = row["Id"].ToString();
                        _lstoutbound.Add(vlpdresponseDTO);
                    }

                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Outbound, _lstoutbound));
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
        [Route("GetItemToPick")]
        public string GetItemToPick(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.VLPD oOutboundresponse = new MRLWMSC21Service.VLPD();
                OutboundDTO responseDTO = new OutboundDTO();
                List<OutboundDTO> _lstoutbound = new List<OutboundDTO>();

                if (oRequest != null)
                {

                    OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_ooutboundDTO != null)
                    {
                        MRLWMSC21Service.VLPD oVLPD = new MRLWMSC21Service.VLPD()
                        {
                            UserId = ConversionUtility.ConvertToInt(_ooutboundDTO.UserId),
                            VLPDId = ConversionUtility.ConvertToInt(_ooutboundDTO.VLPDId),
                            TransferRequestId = _ooutboundDTO.TransferRequestId


                        };
                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        oOutboundresponse = oFalcon.GetItemToPick(oVLPD);
                        if (oOutboundresponse != null)
                        {

                            responseDTO.VLPDNumber = oOutboundresponse.VLPDNo;
                            responseDTO.AssignedID = oOutboundresponse.Assignedid.ToString();
                            responseDTO.MaterialMasterId = oOutboundresponse.MaterialMasterId.ToString();
                            responseDTO.SKU = oOutboundresponse.MCode;
                            responseDTO.MaterialDescription = oOutboundresponse.MDescription;
                            responseDTO.PalletNo = oOutboundresponse.FromCartonCode;
                            responseDTO.CartonID = oOutboundresponse.FromCartonID.ToString();
                            responseDTO.Location = oOutboundresponse.Location;
                            responseDTO.LocationId = oOutboundresponse.LocationID.ToString();
                            responseDTO.MfgDate = oOutboundresponse.MfgDate;
                            responseDTO.ExpDate = oOutboundresponse.ExpDate;
                            responseDTO.SerialNo = oOutboundresponse.SerialNo;
                            responseDTO.BatchNo = oOutboundresponse.BatchNo;
                            responseDTO.ProjectNo = oOutboundresponse.ProjectRefNo;
                            responseDTO.AssignedQuantity = oOutboundresponse.AssignedQuantity;
                            responseDTO.PickedQty = oOutboundresponse.PickedQty;
                            responseDTO.OutboundID = oOutboundresponse.OutboundID;
                            responseDTO.SODetailsID = oOutboundresponse.SODetailsID;
                            responseDTO.SLocId = oOutboundresponse.StorageLocationID;
                            responseDTO.SLoc = oOutboundresponse.StorageLocation;
                            responseDTO.GoodsmomentDeatilsId = oOutboundresponse.GoodsmomentDeatilsId.ToString();
                            responseDTO.Lineno = oOutboundresponse.Lineno;
                            responseDTO.PendingQty = oOutboundresponse.PendingQty;
                            responseDTO.TransferRequestDetailsId = oOutboundresponse.TransferRequestDetailsId;
                            responseDTO.TransferRequestId = oOutboundresponse.TransferRequestId;
                            responseDTO.MRP = oOutboundresponse.MRP;
                        }
                        _lstoutbound.Add(responseDTO);
                    }


                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Outbound, _lstoutbound));
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
        [Route("VLPDSkipItem")]
        public string VLPDSkipItem(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.VLPD ooutboundresponse = new MRLWMSC21Service.VLPD();
                OutboundDTO responseDTO = new OutboundDTO();
                List<OutboundDTO> _lstoutbound = new List<OutboundDTO>();

                if (oRequest != null)
                {

                    OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_ooutboundDTO != null)
                    {

                        MRLWMSC21Service.VLPD ooutbound = new MRLWMSC21Service.VLPD()
                        {


                            TotalPickedQty = ConversionUtility.ConvertToDecimal(_ooutboundDTO.PickedQty),
                            Flag = 1,
                            SkipReason = _ooutboundDTO.SkipReason,
                            VLPDId = ConversionUtility.ConvertToInt(_ooutboundDTO.VLPDId),
                            SkipQty = ConversionUtility.ConvertToDecimal(_ooutboundDTO.SkipQty),
                            Assignedid = Convert.ToInt32(_ooutboundDTO.AssignedID),
                            MCode = _ooutboundDTO.SKU,
                            MfgDate = _ooutboundDTO.MfgDate,
                            ExpDate = _ooutboundDTO.ExpDate,
                            BatchNo = _ooutboundDTO.BatchNo,
                            SerialNo = _ooutboundDTO.SerialNo,
                            ProjectRefNo = _ooutboundDTO.ProjectNo,
                            UserId = Convert.ToInt32(_ooutboundDTO.UserId),
                            StorageLocation = _ooutboundDTO.SLoc,
                            FromCartonCode = _ooutboundDTO.PalletNo,
                            Location = _ooutboundDTO.Location


                        };


                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        ooutboundresponse = oFalcon.VLPDSkipItem(ooutbound);
                        if (ooutboundresponse.Result == "Success")
                        {
                            return GetItemToPick(oRequest);
                        }
                        else
                        {
                            throw new WMSExceptionMessage() { WMSExceptionCode = ooutboundresponse.Erocode, WMSMessage = ooutboundresponse.Result, ShowAsError = true };
                        }



                    }

                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Outbound, _lstoutbound));
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
        [Route("UpsertPickItem")]
        public string UpsertPickItem(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.VLPD oOutboundresponse = new MRLWMSC21Service.VLPD();
                OutboundDTO responseDTO = new OutboundDTO();
                List<OutboundDTO> _lstoutbound = new List<OutboundDTO>();

                if (oRequest != null)
                {

                    OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_ooutboundDTO != null)
                    {
                        MRLWMSC21Service.VLPD oOutbound = new MRLWMSC21Service.VLPD()
                        {
                            Location = _ooutboundDTO.Location,
                            MfgDate = _ooutboundDTO.MfgDate,
                            ExpDate = _ooutboundDTO.ExpDate,
                            BatchNo = _ooutboundDTO.BatchNo,
                            ProjectRefNo = _ooutboundDTO.ProjectNo,
                            SerialNo = _ooutboundDTO.SerialNo,
                            PickedQty = _ooutboundDTO.PickedQty,
                            Assignedid = Convert.ToInt32(_ooutboundDTO.AssignedID),
                            MaterialMasterId = Convert.ToInt32(_ooutboundDTO.MaterialMasterId),
                            UserId = Convert.ToInt32(_ooutboundDTO.UserId),
                            LocationID = Convert.ToInt32(_ooutboundDTO.LocationId),
                            FromCartonID = Convert.ToInt32(_ooutboundDTO.CartonID),
                            VLPDId = Convert.ToInt32(_ooutboundDTO.VLPDId),
                            OutboundID = _ooutboundDTO.OutboundID,
                            SODetailsID = _ooutboundDTO.SODetailsID,
                            // StorageLocation=_ooutboundDTO.SLoc,
                            StorageLocationID = _ooutboundDTO.SLocId,
                            TransferRequestId = Convert.ToInt32(_ooutboundDTO.TransferRequestId),
                            TransferRequestDetailsId = Convert.ToInt32(_ooutboundDTO.TransferRequestDetailsId),
                            MCode = _ooutboundDTO.SKU,
                            ToCartonCode = _ooutboundDTO.ToCartonNO,
                            MRP = _ooutboundDTO.MRP


                        };
                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        oOutboundresponse = oFalcon.UpsertPickItem(oOutbound);
                        if (oOutboundresponse != null)
                        {
                            responseDTO.VLPDNumber = oOutboundresponse.VLPDNo;
                            responseDTO.AssignedID = oOutboundresponse.Assignedid.ToString();
                            responseDTO.MaterialMasterId = oOutboundresponse.MaterialMasterId.ToString();
                            responseDTO.SKU = oOutboundresponse.MCode;
                            responseDTO.MaterialDescription = oOutboundresponse.MDescription;
                            responseDTO.PalletNo = oOutboundresponse.FromCartonCode;
                            responseDTO.CartonID = oOutboundresponse.FromCartonID.ToString();
                            responseDTO.Location = oOutboundresponse.Location;
                            responseDTO.LocationId = oOutboundresponse.LocationID.ToString();
                            responseDTO.MfgDate = oOutboundresponse.MfgDate;
                            responseDTO.ExpDate = oOutboundresponse.ExpDate;
                            responseDTO.SerialNo = oOutboundresponse.SerialNo;
                            responseDTO.BatchNo = oOutboundresponse.BatchNo;
                            responseDTO.ProjectNo = oOutboundresponse.ProjectRefNo;
                            responseDTO.AssignedQuantity = oOutboundresponse.AssignedQuantity;
                            responseDTO.PickedQty = oOutboundresponse.PickedQty;
                            responseDTO.OutboundID = oOutboundresponse.OutboundID;
                            responseDTO.SODetailsID = oOutboundresponse.SODetailsID;
                            responseDTO.SLocId = oOutboundresponse.StorageLocationID;
                            responseDTO.SLoc = oOutboundresponse.StorageLocation;
                            responseDTO.GoodsmomentDeatilsId = oOutboundresponse.GoodsmomentDeatilsId.ToString();
                            responseDTO.Lineno = oOutboundresponse.Lineno;
                            responseDTO.PendingQty = oOutboundresponse.PendingQty;
                            responseDTO.MRP = oOutboundresponse.MRP;

                            if (oOutboundresponse.Result == "-444")
                            {
                                throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_OB_DAL_010", WMSMessage = ErrorMessages.EMC_OB_DAL_010, ShowAsError = true };
                            }
                        }
                        _lstoutbound.Add(responseDTO);
                    }


                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Outbound, _lstoutbound));
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
        [Route("GetVLPDPickedList")]
        public string GetVLPDPickedList(WMSCoreMessage oRequest)
        {
            try
            {
                string Result = "0";
                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.UserDataTable userDataTable = new MRLWMSC21Service.UserDataTable();

                List<OutboundDTO> _lstoutbound = new List<OutboundDTO>();

                if (oRequest != null)
                {

                    OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_ooutboundDTO != null)
                    {

                        MRLWMSC21Service.VLPD oVLPD = new MRLWMSC21Service.VLPD()
                        {

                            VLPDId = ConversionUtility.ConvertToInt(_ooutboundDTO.VLPDId),
                            MCode = _ooutboundDTO.SKU,
                            MfgDate = _ooutboundDTO.MfgDate,
                            ExpDate = _ooutboundDTO.ExpDate,
                            BatchNo = _ooutboundDTO.BatchNo,
                            SerialNo = _ooutboundDTO.SerialNo,
                            ProjectRefNo = _ooutboundDTO.ProjectNo,
                            MRP = _ooutboundDTO.MRP
                        };


                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        userDataTable = oFalcon.GetVLPDPickedList(oVLPD, out Result);
                    }

                    if (Result == "-1")
                    {
                        throw new WMSExceptionMessage() { WMSExceptionCode = null, WMSMessage = "Scanned SKU does not belong to the VLPD", ShowAsError = true };
                    }
                    foreach (DataRow row in userDataTable.Table.Rows)
                    {
                        OutboundDTO responseDTO = new OutboundDTO();
                        responseDTO.PickedId = row["PickedID"].ToString();
                        responseDTO.MaterialMasterId = row["MaterialMasterID"].ToString();
                        responseDTO.SKU = row["MCode"].ToString();
                        responseDTO.LocationId = row["LocationID"].ToString();
                        responseDTO.Location = row["Location"].ToString();
                        responseDTO.CartonID = row["CartonID"].ToString();
                        responseDTO.PalletNo = row["CartonCode"].ToString();
                        responseDTO.PickedQty = row["PickedQty"].ToString();
                        _lstoutbound.Add(responseDTO);
                    }

                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Outbound, _lstoutbound));
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
        [Route("GetOBDPickedList")]
        public string GetOBDPickedList(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.UserDataTable userDataTable = new MRLWMSC21Service.UserDataTable();
                string Result = "0";
                List<OutboundDTO> _lstoutbound = new List<OutboundDTO>();

                if (oRequest != null)
                {

                    OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_ooutboundDTO != null)
                    {

                        MRLWMSC21Service.Outbound _oOutbound = new MRLWMSC21Service.Outbound()
                        {

                            OutboundId = _ooutboundDTO.OutboundID,
                            MCode = _ooutboundDTO.SKU,
                            MfgDate = _ooutboundDTO.MfgDate,
                            ExpDate = _ooutboundDTO.ExpDate,
                            BatchNo = _ooutboundDTO.BatchNo,
                            SerialNo = _ooutboundDTO.SerialNo,
                            ProjectNo = _ooutboundDTO.ProjectNo,
                            MRP = _ooutboundDTO.MRP
                        };


                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        userDataTable = oFalcon.GetOBDPickedList(_oOutbound, out Result);
                    }
                    if (Result == "-1")
                    {
                        throw new WMSExceptionMessage() { WMSExceptionCode = null, WMSMessage = "Scanned SKU does not belong to the Outbound", ShowAsError = true };
                    }
                    foreach (DataRow row in userDataTable.Table.Rows)
                    {
                        OutboundDTO responseDTO = new OutboundDTO();
                        responseDTO.PickedId = row["PickedID"].ToString();
                        responseDTO.MaterialMasterId = row["MaterialMasterID"].ToString();
                        responseDTO.SKU = row["MCode"].ToString();
                        responseDTO.LocationId = row["LocationID"].ToString();
                        responseDTO.Location = row["Location"].ToString();
                        responseDTO.CartonID = row["CartonID"].ToString();
                        responseDTO.PalletNo = row["CartonCode"].ToString();
                        responseDTO.PickedQty = row["PickedQty"].ToString();
                        _lstoutbound.Add(responseDTO);
                    }

                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Outbound, _lstoutbound));
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
        [Route("DeleteVLPDPickedItems")]
        public string DeleteVLPDPickedItems(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.VLPD oVLPd = new MRLWMSC21Service.VLPD();

                List<OutboundDTO> _lstoutbound = new List<OutboundDTO>();

                if (oRequest != null)
                {

                    OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_ooutboundDTO != null)
                    {

                        MRLWMSC21Service.VLPD oVLPD = new MRLWMSC21Service.VLPD()
                        {

                            VLPDId = ConversionUtility.ConvertToInt(_ooutboundDTO.VLPDId),
                            OutboundID = _ooutboundDTO.OutboundID,
                            PickedId = ConversionUtility.ConvertToInt(_ooutboundDTO.PickedId),
                            UserId = ConversionUtility.ConvertToInt(_ooutboundDTO.UserId),
                            MCode = _ooutboundDTO.SKU
                        };


                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        oVLPd = oFalcon.DeleteVLPDPickedItems(oVLPD);
                    }
                    if (oVLPd.Result == "Deleted successfully")
                    {
                        if (oVLPd.VLPDId != 0)
                        {
                            return GetVLPDPickedList(oRequest);
                        }
                        else
                        {
                            return GetOBDPickedList(oRequest);
                        }
                    }
                    else
                    {
                        throw new WMSExceptionMessage() { WMSExceptionCode = oVLPd.Result, WMSMessage = oVLPd.Result, ShowAsError = true };
                    }

                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Outbound, _lstoutbound));
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
        [Route("CheckContainerOBD")]
        public string CheckContainerOBD(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.UserDataTable userDataTable = new MRLWMSC21Service.UserDataTable();
                OutboundDTO responseDTO = new OutboundDTO();
                List<OutboundDTO> _lstinbound = new List<OutboundDTO>();
                String result = null;
                if (oRequest != null)
                {

                    OutboundDTO _oInboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_oInboundDTO != null)
                    {

                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        result = oFalcon.CheckContainerOBD(_oInboundDTO.PalletNo, _oInboundDTO.OutboundID);
                        if (result != "")
                        {
                            throw new WMSExceptionMessage() { WMSExceptionCode = result, WMSMessage = result, ShowAsError = true };
                        }
                        else
                        {
                            responseDTO.Result = "1";
                        }
                        _lstinbound.Add(responseDTO);
                    }


                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inbound, _lstinbound));
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
        [Route("GetOpenVLPDNosForSorting")]
        public string GetOpenVLPDNosForSorting(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.UserDataTable userDataTable = new MRLWMSC21Service.UserDataTable();

                List<OutboundDTO> _lstoutbound = new List<OutboundDTO>();

                if (oRequest != null)
                {

                    OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_ooutboundDTO != null)
                    {

                        MRLWMSC21Service.VLPD oVLPD = new MRLWMSC21Service.VLPD()
                        {

                            AccountId = ConversionUtility.ConvertToInt(_ooutboundDTO.AccountID)
                        };


                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        userDataTable = oFalcon.GetOpenVLPDNosForSorting(oVLPD);
                    }


                    foreach (DataRow row in userDataTable.Table.Rows)
                    {
                        OutboundDTO vlpdresponseDTO = new OutboundDTO();
                        vlpdresponseDTO.VLPDNumber = row["VLPDNumber"].ToString();
                        vlpdresponseDTO.VLPDId = row["Id"].ToString();
                        _lstoutbound.Add(vlpdresponseDTO);
                    }

                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Outbound, _lstoutbound));
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
        [Route("VLPDSortingList")]
        public string VLPDSorting(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.UserDataTable userDataTable = new MRLWMSC21Service.UserDataTable();

                List<OutboundDTO> _lstoutbound = new List<OutboundDTO>();

                if (oRequest != null)
                {

                    OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_ooutboundDTO != null)
                    {

                        MRLWMSC21Service.VLPD oVLPD = new MRLWMSC21Service.VLPD()
                        {
                            //  AccountId = ConversionUtility.ConvertToInt(_ooutboundDTO.AccountID)
                            VLPDNo = _ooutboundDTO.VLPDNumber,
                            MCode = _ooutboundDTO.MCode,
                            BatchNo = _ooutboundDTO.BatchNo,
                            SerialNo = _ooutboundDTO.SerialNo,
                            ProjectRefNo = _ooutboundDTO.ProjectNo,
                            MfgDate = _ooutboundDTO.MfgDate,
                            ExpDate = _ooutboundDTO.ExpDate,
                            PickedQty = _ooutboundDTO.PickedQty
                        };


                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        userDataTable = oFalcon.VLPDSorting(oVLPD);
                    }

                    if (userDataTable != null)
                    {

                        foreach (DataRow row in userDataTable.Table.Rows)
                        {
                            OutboundDTO vlpdresponseDTO = new OutboundDTO();
                            vlpdresponseDTO.VLPDNumber = row["VLPDNumber"].ToString();
                            vlpdresponseDTO.VLPDId = row["Id"].ToString();
                            _lstoutbound.Add(vlpdresponseDTO);
                        }


                    }



                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Outbound, _lstoutbound));
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
        [Route("GetOpenLoadsheetList")]
        public string GetOpenLoadsheetList(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.UserDataTable userDataTable = new MRLWMSC21Service.UserDataTable();

                List<OutboundDTO> _lstoutbound = new List<OutboundDTO>();

                if (oRequest != null)
                {

                    OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_ooutboundDTO != null)
                    {
                        string Tenantid = _ooutboundDTO.TenatID;
                        MRLWMSC21Service.VLPD oVLPD = new MRLWMSC21Service.VLPD()
                        {
                            AccountId = ConversionUtility.ConvertToInt(_ooutboundDTO.AccountID),
                            UserId = ConversionUtility.ConvertToInt(_ooutboundDTO.UserId)
                        };
                        //oFalcon.GetOpenLoadsheetList

                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        userDataTable = oFalcon.GetOpenLoadsheetList(Tenantid, oVLPD.AccountId.ToString());
                    }
                    //foreach (DataRow row in userDataTable.Table.Rows)
                    //{
                    //    OutboundDTO responseDTO = new OutboundDTO();
                    //    _ooutboundDTO.VLPDNumber = row["VLPDNumber"].ToString();
                    //    _ooutboundDTO.VLPDId = row["Id"].ToString();
                    //    _lstoutbound.Add(_ooutboundDTO);
                    //}

                    foreach (DataRow row in userDataTable.Table.Rows)
                    {
                        OutboundDTO vlpdresponseDTO = new OutboundDTO();
                        vlpdresponseDTO.VLPDNumber = row["LoadSheetNo"].ToString();
                        vlpdresponseDTO.VLPDId = row["LoadSheetId"].ToString();
                        _lstoutbound.Add(vlpdresponseDTO);
                    }

                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Outbound, _lstoutbound));
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
        [Route("GetPendingOBDListForLoading")]
        public string GetPendingOBDListForLoading(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.UserDataTable userDataTable = new MRLWMSC21Service.UserDataTable();

                List<OutboundDTO> _lstoutbound = new List<OutboundDTO>();

                if (oRequest != null)
                {

                    OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_ooutboundDTO != null)
                    {
                        string Tenantid = _ooutboundDTO.TenatID;
                        MRLWMSC21Service.VLPD oVLPD = new MRLWMSC21Service.VLPD()
                        {

                            AccountId = ConversionUtility.ConvertToInt(_ooutboundDTO.AccountID)
                        };
                        //oFalcon.GetOpenLoadsheetList

                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        userDataTable = oFalcon.GetPendingOBDListForLoading(Tenantid, oVLPD.AccountId.ToString());
                    }
                    //foreach (DataRow row in userDataTable.Table.Rows)
                    //{
                    //    OutboundDTO responseDTO = new OutboundDTO();
                    //    _ooutboundDTO.VLPDNumber = row["VLPDNumber"].ToString();
                    //    _ooutboundDTO.VLPDId = row["Id"].ToString();
                    //    _lstoutbound.Add(_ooutboundDTO);
                    //}

                    foreach (DataRow row in userDataTable.Table.Rows)
                    {
                        OutboundDTO vlpdresponseDTO = new OutboundDTO();
                        vlpdresponseDTO.OBDNumber = row["OBDNumber"].ToString();
                        vlpdresponseDTO.OutboundID = row["OutboundID"].ToString();
                        _lstoutbound.Add(vlpdresponseDTO);
                    }

                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Outbound, _lstoutbound));
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
        [Route("UpsertLoadCreated")]
        public string UpsertLoadCreated(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.VLPD oOutboundresponse = new MRLWMSC21Service.VLPD();
                OutboundDTO responseDTO = new OutboundDTO();
                List<OutboundDTO> _lstoutbound = new List<OutboundDTO>();

                if (oRequest != null)
                {

                    OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_ooutboundDTO != null)
                    {
                        MRLWMSC21Service.VLPD oVLPD = new MRLWMSC21Service.VLPD()
                        {
                            UserId = ConversionUtility.ConvertToInt(_ooutboundDTO.UserId),
                            TenantId = (_ooutboundDTO.TenatID),
                            Vehicle = _ooutboundDTO.Vehicle,
                            OBDNumber = _ooutboundDTO.OBDNumber,
                            DriverNo = _ooutboundDTO.DriverNo,
                            DriverName = _ooutboundDTO.DriverName,
                            LRnumber = _ooutboundDTO.LRnumber

                        };
                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        string Result = "";
                        Result = oFalcon.UpsertLoadCreated(oVLPD);
                        responseDTO.Result = Result;
                        _lstoutbound.Add(responseDTO);
                    }


                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Outbound, _lstoutbound));
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
        [Route("UpsertLoad")]
        public string UpsertLoad(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.VLPD oOutboundresponse = new MRLWMSC21Service.VLPD();
                OutboundDTO responseDTO = new OutboundDTO();
                List<OutboundDTO> _lstoutbound = new List<OutboundDTO>();

                if (oRequest != null)
                {

                    OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_ooutboundDTO != null)
                    {
                        MRLWMSC21Service.VLPD oVLPD = new MRLWMSC21Service.VLPD()
                        {
                            UserId = ConversionUtility.ConvertToInt(_ooutboundDTO.UserId),
                            MCode = (_ooutboundDTO.MCode),
                            VLPDNo = _ooutboundDTO.VLPDNumber,
                            MfgDate = _ooutboundDTO.MfgDate,
                            ExpDate = _ooutboundDTO.ExpDate,
                            BatchNo = _ooutboundDTO.BatchNo,
                            SerialNo = _ooutboundDTO.SerialNo,
                            ProjectRefNo = _ooutboundDTO.ProjectNo,
                            MRP = _ooutboundDTO.MRP,
                            PickedQty = _ooutboundDTO.PickedQty
                        };
                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        string Result = "";
                        Result = oFalcon.UpsertLoad(oVLPD);
                        responseDTO.Result = Result;
                        _lstoutbound.Add(responseDTO);
                    }
                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Outbound, _lstoutbound));
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
        [Route("LoadVerification")]
        public string LoadVerification(WMSCoreMessage oRequest)
        {
            try
            {
                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.VLPD oOutboundresponse = new MRLWMSC21Service.VLPD();
                OutboundDTO responseDTO = new OutboundDTO();
                List<OutboundDTO> _lstoutbound = new List<OutboundDTO>();

                if (oRequest != null)
                {

                    OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_ooutboundDTO != null)
                    {
                        MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient oFalcon = new MRLWMSC21Service.MRLWMSC21HHTWCFServiceClient();
                        string Result = "";
                        Result = oFalcon.LoadVerification(_ooutboundDTO.VLPDNumber, _ooutboundDTO.UserId);
                        responseDTO.Result = Result;
                        _lstoutbound.Add(responseDTO);
                    }
                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Outbound, _lstoutbound));
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
        [Route("ScanSONumberForPacking")]
        public string ScanSONumberForPacking(WMSCoreMessage oRequest)
        {
            try
            {

                OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                string SONumber = _ooutboundDTO.SONumber;
                int AccountID = Convert.ToInt32(_ooutboundDTO.AccountID);
                int UserId = Convert.ToInt32(_ooutboundDTO.TenatID);

                OutboundBL outboundBL = new OutboundBL(LoggedInUserID, ConnectionString);
                Outbound oCustomer = new Outbound();

                List<Outbound> _lstcustomers = new List<Outbound>();
                List<OutboundDTO> _lstOutboundDTO = new List<OutboundDTO>();
                OutboundDTO _oOutboundResponse = new OutboundDTO();
                List<string> _PackingMcode = new List<string>();

                _lstcustomers = outboundBL.GetItemsUnderSO(SONumber, AccountID, UserId);


                foreach (Outbound Loadsheetitem in _lstcustomers)
                {

                    OutboundDTO _outboundDTO = new OutboundDTO();
                    //_outboundDTO.MaterialMasterId = Loadsheetitem.MaterialMasterID.ToString();
                    _outboundDTO.MCode = Loadsheetitem.Mcode;
                    // _outboundDTO.SOQty = Loadsheetitem.SOQty.ToString();
                    _outboundDTO.PickedQty = Loadsheetitem.PickedQty.ToString();
                    _outboundDTO.PackedQty = Loadsheetitem.PackedQty.ToString();
                    //_outboundDTO.OutboundID = Loadsheetitem.OutboundID.ToString();
                    _outboundDTO.CustomerName = Loadsheetitem.CustomerName;
                    _outboundDTO.SOHeaderID = Loadsheetitem.SOHeaderID.ToString();
                    _outboundDTO.BusinessType = Loadsheetitem.BusinessType.ToString();
                    //_outboundDTO.OBDNumber = Loadsheetitem.OBDNumber.ToString();
                    //_outboundDTO.PSNID = Loadsheetitem.PSNID.ToString();
                    //_outboundDTO.PSNDetailsID = Loadsheetitem.PSNDetailsID.ToString();
                    _outboundDTO.MfgDate = Loadsheetitem.MFGDate;
                    _outboundDTO.ExpDate = Loadsheetitem.EXPDate;
                    _outboundDTO.SerialNo = Loadsheetitem.SerialNo;
                    _outboundDTO.ProjectNo = Loadsheetitem.ProjectRefNo;
                    _outboundDTO.BatchNo = Loadsheetitem.BatchNo;
                    _outboundDTO.MRP = Loadsheetitem.MRP;
                    _outboundDTO.HUNo = Loadsheetitem.HUNo;
                    _outboundDTO.HUSize = Loadsheetitem.HUSize;
                    //_outboundDTO.AccountID = Loadsheetitem.AccountID.ToString(); //============ Added By M.D.Prasad On 28-Aug-2020 for Account add1 ============//

                    _lstOutboundDTO.Add(_outboundDTO);


                }



                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inbound, _lstOutboundDTO));
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
        [Route("GETMSPsForPacking")]
        public string GETMSPsForPacking(WMSCoreMessage oRequest)
        {
            try
            {

                OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                string sodetailsid = _ooutboundDTO.SODetailsID;

                OutboundBL outboundBL = new OutboundBL(LoggedInUserID, ConnectionString);
                Outbound oCustomer = new Outbound();

                List<Outbound> _lstcustomers = new List<Outbound>();
                List<OutboundDTO> _lstOutboundDTO = new List<OutboundDTO>();
                OutboundDTO _oOutboundResponse = new OutboundDTO();
                List<string> _PackingMcode = new List<string>();

                _lstcustomers = outboundBL.GetMSPsForPacking(sodetailsid);


                foreach (Outbound Loadsheetitem in _lstcustomers)
                {

                    OutboundDTO _outboundDTO = new OutboundDTO();
                    _outboundDTO.MfgDate = Loadsheetitem.MFGDate;
                    _outboundDTO.ExpDate = Loadsheetitem.EXPDate;
                    _outboundDTO.SerialNo = Loadsheetitem.SerialNo;
                    _outboundDTO.ProjectNo = Loadsheetitem.ProjectRefNo;
                    _outboundDTO.BatchNo = Loadsheetitem.BatchNo;
                    _outboundDTO.MRP = Loadsheetitem.MRP;

                    _lstOutboundDTO.Add(_outboundDTO);


                }



                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inbound, _lstOutboundDTO));
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
        [Route("UpsertPackItem")]
        public string UpsertPackItem(WMSCoreMessage oRequest)
        {
            try
            {

                OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                string sodetailsid = _ooutboundDTO.SODetailsID;

                OutboundBL outboundBL = new OutboundBL(LoggedInUserID, ConnectionString);


                Outbound oOutbound = new Outbound();

                List<Outbound> _lstcustomers = new List<Outbound>();
                List<OutboundDTO> _lstOutboundDTO = new List<OutboundDTO>();
                OutboundDTO _oOutboundResponse = new OutboundDTO();
                List<string> _PackingMcode = new List<string>();

                Outbound outboundItem = new Outbound()
                {
                    Mcode = _ooutboundDTO.MCode,
                    OutboundID = ConversionUtility.ConvertToInt(_ooutboundDTO.OutboundID),
                    PSNID = ConversionUtility.ConvertToInt(_ooutboundDTO.PSNID),
                    PickedQty = ConversionUtility.ConvertToDecimal(_ooutboundDTO.PickedQty),
                    PackedQty = ConversionUtility.ConvertToInt(_ooutboundDTO.PackedQty),
                    CartonSerialNo = _ooutboundDTO.CartonSerialNo,
                    MFGDate = _ooutboundDTO.MfgDate,
                    EXPDate = _ooutboundDTO.ExpDate,
                    SerialNo = _ooutboundDTO.SerialNo,
                    ProjectRefNo = _ooutboundDTO.ProjectNo,
                    BatchNo = _ooutboundDTO.BatchNo,
                    MRP = _ooutboundDTO.MRP,
                    PSNDetailsID = ConversionUtility.ConvertToInt(_ooutboundDTO.PSNDetailsID),
                    PackType = _ooutboundDTO.PackType,
                    SoDetailsID = ConversionUtility.ConvertToInt(_ooutboundDTO.SODetailsID),
                    SOHeaderID = ConversionUtility.ConvertToInt(_ooutboundDTO.SOHeaderID),
                    SONumber = _ooutboundDTO.SONumber,
                    AccountID = Convert.ToInt32(_ooutboundDTO.AccountID),
                    HUSize = _ooutboundDTO.HUSize,
                    HUNo = _ooutboundDTO.HUNo
                };


                oOutbound = outboundBL.UpsertPackItem(outboundItem);

                OutboundDTO _outboundDTO = new OutboundDTO();

                _outboundDTO.PSNID = oOutbound.PSNID.ToString();
                _outboundDTO.PSNDetailsID = oOutbound.PSNDetailsID.ToString();
                _lstOutboundDTO.Add(_outboundDTO);







                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inbound, _lstOutboundDTO));
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
        [Route("UpdatePackComplete")]
        public string UpdatePackComplete(WMSCoreMessage oRequest)
        {
            try
            {

                OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                string sodetailsid = _ooutboundDTO.SODetailsID;

                OutboundBL outboundBL = new OutboundBL(LoggedInUserID, ConnectionString);


                Outbound oOutbound = new Outbound();

                List<Outbound> _lstcustomers = new List<Outbound>();
                List<OutboundDTO> _lstOutboundDTO = new List<OutboundDTO>();
                OutboundDTO _oOutboundResponse = new OutboundDTO();
                List<string> _PackingMcode = new List<string>();

                Outbound outboundItem = new Outbound()
                {
                    SONumber = _ooutboundDTO.SONumber,
                    AccountID = Convert.ToInt32(_ooutboundDTO.AccountID)
                };


                oOutbound = outboundBL.PackComplete(outboundItem);

                OutboundDTO _outboundDTO = new OutboundDTO();

                _outboundDTO.PackComplete = oOutbound.PackComplete.ToString();

                _lstOutboundDTO.Add(_outboundDTO);







                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inbound, _lstOutboundDTO));
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
        [Route("GenerateLoadSheet")]
        public string GenerateLoadSheet(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.VLPD oOutboundresponse = new MRLWMSC21Service.VLPD();
                OutboundDTO responseDTO = new OutboundDTO();
                List<OutboundDTO> _lstoutbound = new List<OutboundDTO>();
                Outbound _oOutbound = new Outbound();
                OutboundBL outboundBL = new OutboundBL(LoggedInUserID, ConnectionString);

                List<SalesOrderDTO> _lstsalesorders = new List<SalesOrderDTO>();

                if (oRequest != null)
                {

                    OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_ooutboundDTO != null)
                    {

                        Outbound oOutbound = new Outbound()
                        {

                            UserId = ConversionUtility.ConvertToInt(_ooutboundDTO.UserId),
                            TenantId = _ooutboundDTO.TenatID,
                            Vehicle = _ooutboundDTO.Vehicle,
                            OBDNumber = _ooutboundDTO.OBDNumber,
                            DriverNo = _ooutboundDTO.DriverNo,
                            DriverName = _ooutboundDTO.DriverName,
                            LRnumber = _ooutboundDTO.LRnumber,
                            SONumber = _ooutboundDTO.SONumber,
                            AccountID = Convert.ToInt32(_ooutboundDTO.AccountID)

                        };

                        _oOutbound = outboundBL.LoadsheetGeneration(oOutbound);

                        OutboundDTO _outboundDTO = new OutboundDTO();
                        _outboundDTO.LoadRefNo = _oOutbound.LoadRefNo;
                        _lstoutbound.Add(_outboundDTO);



                    }


                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Outbound, _lstoutbound));
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
        [Route("GetSOCountUnderLoadSheet")]
        public string GetSOCountUnderLoadSheet(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.VLPD oOutboundresponse = new MRLWMSC21Service.VLPD();
                OutboundDTO responseDTO = new OutboundDTO();
                List<OutboundDTO> _lstoutbound = new List<OutboundDTO>();
                Outbound _oOutbound = new Outbound();
                OutboundBL outboundBL = new OutboundBL(LoggedInUserID, ConnectionString);

                List<SalesOrderDTO> _lstsalesorders = new List<SalesOrderDTO>();

                if (oRequest != null)
                {

                    OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_ooutboundDTO != null)
                    {

                        Outbound oOutbound = new Outbound()
                        {

                            LoadRefNo = _ooutboundDTO.LoadRefNo


                        };

                        _oOutbound = outboundBL.GetLoadDetails(oOutbound);

                        OutboundDTO _outboundDTO = new OutboundDTO();
                        _outboundDTO.BusinessType = _oOutbound.BusinessType;
                        _outboundDTO.TotalSOCount = _oOutbound.TotalSOCount.ToString();
                        _outboundDTO.ScannedSOCount = _oOutbound.ScannedSOCount.ToString();
                        _lstoutbound.Add(_outboundDTO);



                    }


                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Outbound, _lstoutbound));
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
        [Route("UpsertLoadDetails")]
        public string UpsertLoadDetails(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.VLPD oOutboundresponse = new MRLWMSC21Service.VLPD();
                OutboundDTO responseDTO = new OutboundDTO();
                List<OutboundDTO> _lstoutbound = new List<OutboundDTO>();
                Outbound _oOutbound = new Outbound();
                OutboundBL outboundBL = new OutboundBL(LoggedInUserID, ConnectionString);

                List<SalesOrderDTO> _lstsalesorders = new List<SalesOrderDTO>();

                if (oRequest != null)
                {

                    OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_ooutboundDTO != null)
                    {

                        Outbound oOutbound = new Outbound()
                        {

                            LoadRefNo = _ooutboundDTO.LoadRefNo,
                            SONumber = _ooutboundDTO.SONumber,
                            CartonSerialNo = _ooutboundDTO.CartonSerialNo,
                            AccountID = Convert.ToInt32(_ooutboundDTO.AccountID) //============ Added By M.D.Prasad On 28-Aug-2020 for Account add2 ============//


                        };

                        _oOutbound = outboundBL.UpsertLoadDetails(oOutbound);

                        OutboundDTO _outboundDTO = new OutboundDTO();
                        _outboundDTO.CustomerName = _oOutbound.CustomerName;
                        _outboundDTO.CustomerCode = _oOutbound.CustomerCode.ToString();
                        _outboundDTO.CustomerAddress = _oOutbound.CustomerAddress.ToString();
                        _lstoutbound.Add(_outboundDTO);



                    }


                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Outbound, _lstoutbound));
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
        [Route("GetOBDNosUnderSO")]
        public string GetOBDNosUnderSO(WMSCoreMessage oRequest)
        {
            try
            {
                LoadControllerDefaults(oRequest);

                OutboundDTO responseDTO = new OutboundDTO();
                List<OutboundDTO> _lstoutbound = new List<OutboundDTO>();
                Outbound _oOutbound = new Outbound();
                OutboundBL outboundBL = new OutboundBL(LoggedInUserID, ConnectionString);

                if (oRequest != null)
                {
                    List<Outbound> outbounds = new List<Outbound>();
                    OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);

                    if (_ooutboundDTO != null)
                    {

                        Outbound oOutbound = new Outbound()
                        {
                            SONumber = _ooutboundDTO.SONumber,
                            AccountID = Convert.ToInt32(_ooutboundDTO.AccountID),
                            UserId = Convert.ToInt32(_ooutboundDTO.TenatID)
                        };

                        bool result;
                        result = outboundBL.CheckOBDSO(oOutbound);



                        outbounds = outboundBL.GetOBDNosUnderSO(oOutbound);
                        foreach (Outbound outbound in outbounds)
                        {
                            OutboundDTO _outboundDTO = new OutboundDTO();
                            _outboundDTO.OutboundID = outbound.OutboundID.ToString();
                            _outboundDTO.OBDNo = outbound.OBDNumber.ToString();


                            _lstoutbound.Add(_outboundDTO);

                        }




                    }


                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Outbound, _lstoutbound));
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
        [Route("GetPackingCartonInfo")]
        public string GetPackingCartonInfo(WMSCoreMessage oRequest)
        {
            try
            {
                LoadControllerDefaults(oRequest);

                OutboundDTO responseDTO = new OutboundDTO();
                List<OutboundDTO> _lstoutboundDTO = new List<OutboundDTO>();
                Outbound _oOutbound = new Outbound();
                OutboundBL outboundBL = new OutboundBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {
                    List<Outbound> _lstOutbound = new List<Outbound>();
                    OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);


                    if (_ooutboundDTO != null)
                    {
                        Outbound outbound = new Outbound()
                        {
                            SONumber = _ooutboundDTO.SONumber,
                            CartonSerialNo = _ooutboundDTO.CartonSerialNo,
                            TenantId = _ooutboundDTO.TenatID,
                            WareHouseID = _ooutboundDTO.WareHouseID,
                            AccountID = Convert.ToInt32(_ooutboundDTO.AccountID),
                            UserId = Convert.ToInt32(_ooutboundDTO.UserId)
                        };

                        if (outbound.SONumber != "")
                        {
                            bool SOresult;
                            SOresult = outboundBL.CheckSO(outbound);

                        }

                        if (outbound.CartonSerialNo != "")
                        {
                            bool CartonResult;
                            CartonResult = outboundBL.CheckCarton(outbound);

                        }


                        _lstOutbound = outboundBL.GetPackingCartonInfo(outbound);

                        foreach (Outbound outboundItem in _lstOutbound)
                        {
                            OutboundDTO outboundDTO = new OutboundDTO();
                            outboundDTO.CartonSerialNo = outboundItem.CartonSerialNo.ToString();
                            outboundDTO.MCode = outboundItem.Mcode.ToString();
                            outboundDTO.PickedQty = outboundItem.PickedQty.ToString();
                            outboundDTO.SONumber = outboundItem.SONumber.ToString();
                            _lstoutboundDTO.Add(outboundDTO);
                        }

                    }

                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Outbound, _lstoutboundDTO));
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
                ExceptionHandling.LogException(excp, _ClassCode + "001");
                return null;
            }
        }

        [HttpPost]
        [Route("GetRevertOBDList")]
        public string GetRevertOBDList(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.UserDataTable userDataTable = new MRLWMSC21Service.UserDataTable();

                List<OutboundDTO> _lstoutboundDTO = new List<OutboundDTO>();

                if (oRequest != null)
                {

                    OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    OutboundBL outboundBL = new OutboundBL(LoggedInUserID, ConnectionString);

                    List<Outbound> _lstOutbound = new List<Outbound>();




                    if (_ooutboundDTO != null)
                    {

                        Outbound outbound = new Outbound()
                        {
                            AccountID = ConversionUtility.ConvertToInt(_ooutboundDTO.AccountID),
                            UserId = ConversionUtility.ConvertToInt(_ooutboundDTO.UserId)
                        };

                        _lstOutbound = outboundBL.GetRevertOBDLIst(outbound);

                        foreach (Outbound outboundItem in _lstOutbound)
                        {
                            OutboundDTO outboundDTO = new OutboundDTO();
                            outboundDTO.OBDNumber = outboundItem.OBDNumber.ToString();
                            outboundDTO.OutboundID = outboundItem.OutboundID.ToString();

                            _lstoutboundDTO.Add(outboundDTO);
                        }


                    }

                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Outbound, _lstoutboundDTO));
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
                ExceptionHandling.LogException(excp, _ClassCode + "R001");
                return null;
            }

        }
        [HttpPost]
        [Route("GetRevertSOList")]
        public string GetRevertSOList(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.UserDataTable userDataTable = new MRLWMSC21Service.UserDataTable();

                List<OutboundDTO> _lstoutboundDTO = new List<OutboundDTO>();

                if (oRequest != null)
                {

                    OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    OutboundBL outboundBL = new OutboundBL(LoggedInUserID, ConnectionString);

                    List<Outbound> _lstOutbound = new List<Outbound>();




                    if (_ooutboundDTO != null)
                    {

                        Outbound outbound = new Outbound()
                        {
                            AccountID = ConversionUtility.ConvertToInt(_ooutboundDTO.AccountID),
                            UserId = ConversionUtility.ConvertToInt(_ooutboundDTO.UserId),
                            OutboundID = ConversionUtility.ConvertToInt(_ooutboundDTO.OutboundID)
                        };

                        _lstOutbound = outboundBL.GetRevertSOLIst(outbound);

                        foreach (Outbound outboundItem in _lstOutbound)
                        {
                            OutboundDTO outboundDTO = new OutboundDTO();
                            outboundDTO.SONumber = outboundItem.SONumber.ToString();
                            outboundDTO.SOHeaderID = outboundItem.SOHeaderID.ToString();

                            _lstoutboundDTO.Add(outboundDTO);
                        }


                    }

                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Outbound, _lstoutboundDTO));
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
                ExceptionHandling.LogException(excp, _ClassCode + "R002");
                return null;
            }

        }
        [HttpPost]
        [Route("GetRevertSOOBDInfo")]
        public string GetRevertSOOBDInfo(WMSCoreMessage oRequest)
        {
            try
            {

                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.UserDataTable userDataTable = new MRLWMSC21Service.UserDataTable();

                List<OutboundDTO> _lstoutboundDTO = new List<OutboundDTO>();

                if (oRequest != null)
                {

                    OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    OutboundBL outboundBL = new OutboundBL(LoggedInUserID, ConnectionString);

                    List<Outbound> _lstOutbound = new List<Outbound>();




                    if (_ooutboundDTO != null)
                    {

                        Outbound outbound = new Outbound()
                        {
                            AccountID = ConversionUtility.ConvertToInt(_ooutboundDTO.AccountID),
                            UserId = ConversionUtility.ConvertToInt(_ooutboundDTO.UserId),
                            OutboundID = ConversionUtility.ConvertToInt(_ooutboundDTO.OutboundID),
                            SONumber = _ooutboundDTO.SONumber
                        };

                        _lstOutbound = outboundBL.GetRevertSOOBDInfo(outbound);

                        foreach (Outbound outboundItem in _lstOutbound)
                        {
                            OutboundDTO outboundDTO = new OutboundDTO();
                            outboundDTO.SONumber = outboundItem.SONumber.ToString();
                            outboundDTO.SOHeaderID = outboundItem.SOHeaderID.ToString();
                            outboundDTO.OutboundID = outboundItem.OutboundID.ToString();
                            outboundDTO.OBDNumber = outboundItem.OBDNumber.ToString();
                            outboundDTO.BusinessType = outboundItem.BusinessType.ToString();
                            outboundDTO.Status = outboundItem.Status.ToString();
                            _lstoutboundDTO.Add(outboundDTO);
                        }


                    }

                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Outbound, _lstoutboundDTO));
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
                ExceptionHandling.LogException(excp, _ClassCode + "R003");
                return null;
            }

        }
        [HttpPost]
        [Route("GetRevertCartonCheck")]
        public string GetRevertCartonCheck(WMSCoreMessage oRequest)
        {
            try
            {
                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.UserDataTable userDataTable = new MRLWMSC21Service.UserDataTable();

                List<OutboundDTO> _lstoutboundDTO = new List<OutboundDTO>();

                if (oRequest != null)
                {

                    OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    OutboundBL outboundBL = new OutboundBL(LoggedInUserID, ConnectionString);
                    List<Outbound> _lstOutbound = new List<Outbound>();
                    if (_ooutboundDTO != null)
                    {

                        Outbound outbound = new Outbound()
                        {
                            OutboundID = ConversionUtility.ConvertToInt(_ooutboundDTO.OutboundID),
                            SOHeaderID = ConversionUtility.ConvertToInt(_ooutboundDTO.SOHeaderID),
                            CartonSerialNo = _ooutboundDTO.CartonSerialNo
                        };
                        _lstOutbound = outboundBL.GetRevertCartonCheck(outbound);

                        foreach (Outbound outboundItem in _lstOutbound)
                        {
                            OutboundDTO outboundDTO = new OutboundDTO();
                            outboundDTO.Status = outboundItem.Status.ToString();
                            _lstoutboundDTO.Add(outboundDTO);
                        }
                    }
                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Outbound, _lstoutboundDTO));
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
                ExceptionHandling.LogException(excp, _ClassCode + "R004");
                return null;
            }

        }
        [HttpPost]
        [Route("GetScanqtyvalidation")]
        public string GetScanqtyvalidation(WMSCoreMessage oRequest)
        {
            try
            {
                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.UserDataTable userDataTable = new MRLWMSC21Service.UserDataTable();

                List<OutboundDTO> _lstoutboundDTO = new List<OutboundDTO>();

                if (oRequest != null)
                {

                    OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    OutboundBL outboundBL = new OutboundBL(LoggedInUserID, ConnectionString);
                    List<Outbound> _lstOutbound = new List<Outbound>();
                    if (_ooutboundDTO != null)
                    {

                        Outbound outbound = new Outbound()
                        {
                            OutboundID = ConversionUtility.ConvertToInt(_ooutboundDTO.OutboundID),
                            SOHeaderID = ConversionUtility.ConvertToInt(_ooutboundDTO.SOHeaderID),
                            Mcode = _ooutboundDTO.MCode,
                            MFGDate = _ooutboundDTO.MfgDate,
                            EXPDate = _ooutboundDTO.ExpDate,
                            SerialNo = _ooutboundDTO.SerialNo,
                            ProjectRefNo = _ooutboundDTO.ProjectNo,
                            MRP = _ooutboundDTO.MRP,
                            BatchNo = _ooutboundDTO.BatchNo,
                            CartonSerialNo = _ooutboundDTO.CartonSerialNo
                        };
                        _lstOutbound = outboundBL.GetScanqtyvalidation(outbound);

                        foreach (Outbound outboundItem in _lstOutbound)
                        {
                            OutboundDTO outboundDTO = new OutboundDTO();
                            outboundDTO.Status = outboundItem.Status.ToString();
                            outboundDTO.SOQty = outboundItem.SOQty.ToString();
                            _lstoutboundDTO.Add(outboundDTO);
                        }
                    }
                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Outbound, _lstoutboundDTO));
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
                ExceptionHandling.LogException(excp, _ClassCode + "R005");
                return null;
            }

        }
        [HttpPost]
        [Route("UpsertHHTOBDRevert")]
        public string UpsertHHTOBDRevert(WMSCoreMessage oRequest)
        {
            try
            {
                LoadControllerDefaults(oRequest);
                MRLWMSC21Service.UserDataTable userDataTable = new MRLWMSC21Service.UserDataTable();

                List<OutboundDTO> _lstoutboundDTO = new List<OutboundDTO>();

                if (oRequest != null)
                {

                    OutboundDTO _ooutboundDTO = (OutboundDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    OutboundBL outboundBL = new OutboundBL(LoggedInUserID, ConnectionString);
                    List<Outbound> _lstOutbound = new List<Outbound>();
                    if (_ooutboundDTO != null)
                    {

                        Outbound outbound = new Outbound()
                        {
                            OutboundID = ConversionUtility.ConvertToInt(_ooutboundDTO.OutboundID),
                            SOHeaderID = ConversionUtility.ConvertToInt(_ooutboundDTO.SOHeaderID),
                            Mcode = _ooutboundDTO.MCode,
                            MFGDate = _ooutboundDTO.MfgDate,
                            EXPDate = _ooutboundDTO.ExpDate,
                            SerialNo = _ooutboundDTO.SerialNo,
                            ProjectRefNo = _ooutboundDTO.ProjectNo,
                            MRP = _ooutboundDTO.MRP,
                            BatchNo = _ooutboundDTO.BatchNo,
                            CartonSerialNo = _ooutboundDTO.CartonSerialNo,
                            PackedQty = Convert.ToDecimal(_ooutboundDTO.PackedQty),
                            UserId = Convert.ToInt32(_ooutboundDTO.UserId)
                        };
                        _lstOutbound = outboundBL.UpsertHHTOBDRevert(outbound);

                        foreach (Outbound outboundItem in _lstOutbound)
                        {
                            OutboundDTO outboundDTO = new OutboundDTO();
                            outboundDTO.Status = outboundItem.Status.ToString();
                            _lstoutboundDTO.Add(outboundDTO);
                        }
                    }
                    string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Outbound, _lstoutboundDTO));
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
                ExceptionHandling.LogException(excp, _ClassCode + "R006");
                return null;
            }

        }
    }
}