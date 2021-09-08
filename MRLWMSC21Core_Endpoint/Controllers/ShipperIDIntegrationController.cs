using MRLWMSC21Core.Business;
using MRLWMSC21Core.Entities;
using MRLWMSC21Core.Library;
using MRLWMSC21_Endpoint.DTO;
using MRLWMSC21_Endpoint.Interfaces;
using MRLWMSC21_Endpoint.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Xml;
using System.Xml.Serialization;
using WMSCore_BusinessEntities.Entities;
using static MRLWMSC21_Endpoint.DTO.MastersDTO;

namespace MRLWMSC21_Endpoint.Controllers
{
    [RoutePrefix("WMSAPI")]
    public class ShipperIDIntegrationController : BaseController, iIntegration
    {
        private string _ClassCode = string.Empty;



        [HttpPost]
        [Route("CreateOutbound")]
        public object CreateOutbound(WMSCoreMessage oRequest)
        {

            try
            {
                LoadControllerDefaults(oRequest);

                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {
                    APISOCreateRequestDTO aPISORequestDTO = (APISOCreateRequestDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);


                    string authtoken = oRequest.AuthToken.AuthKey;

                    List<APISOCreation> aPISOCreations = new List<APISOCreation>();
                    APISOCreation aPISOCreation = new APISOCreation();

                    bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);


                    if (result == true)
                    {

                        //APISOCreateRequestDTO aPISOCreateRequestDTO = new APISOCreateRequestDTO();
                        List<SOCreationDTO> _lstAPISORequestDTO = aPISORequestDTO.OrderReuest;

                        string finalresult = "";
                        var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

                        emptyNamepsaces.Add("", "");
                        var settings = new XmlWriterSettings();
                        string xmlString = null;
                        using (StringWriter sw = new StringWriter())
                        {
                            XmlSerializer serialiser = new XmlSerializer(typeof(List<SOCreationDTO>));
                            serialiser.Serialize(sw, _lstAPISORequestDTO, emptyNamepsaces);

                            xmlString = sw.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", " ");


                        }


                        finalresult = oshipperIDIntergrationBL.UpsertBulkOrders(xmlString, authtoken);


                        string json = finalresult;
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }

                }

                else
                {
                    return null;
                }
            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return myCleanJsonObject;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "001");
                return null;
            }
        }


        [HttpPost]
        [Route("CancelSalesOrder")]
        public object CancelSalesOrder(WMSCoreMessage oRequest)
        {

            try
            {
                LoadControllerDefaults(oRequest);

                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {
                    APISOCreateRequestDTO aPISORequestDTO = (APISOCreateRequestDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);


                    string authtoken = oRequest.AuthToken.AuthKey;

                    List<APISOCreation> aPISOCreations = new List<APISOCreation>();
                    APISOCreation aPISOCreation = new APISOCreation();

                    bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);


                    if (result == true)
                    {

                        //APISOCreateRequestDTO aPISOCreateRequestDTO = new APISOCreateRequestDTO();
                        List<SOCreationDTO> _lstAPISORequestDTO = aPISORequestDTO.OrderReuest;

                        string finalresult = "";
                        var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

                        emptyNamepsaces.Add("", "");
                        var settings = new XmlWriterSettings();
                        string xmlString = null;
                        using (StringWriter sw = new StringWriter())
                        {
                            XmlSerializer serialiser = new XmlSerializer(typeof(List<SOCreationDTO>));
                            serialiser.Serialize(sw, _lstAPISORequestDTO, emptyNamepsaces);

                            xmlString = sw.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", " ");


                        }


                        finalresult = oshipperIDIntergrationBL.CancelSalesOrder(xmlString, authtoken);


                        string json = finalresult;
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }

                }

                else
                {
                    return null;
                }
            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return myCleanJsonObject;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "001");
                return null;
            }
        }


        [HttpPost]
        [Route("CreatePurchaseOrder")]
        public object CreatePurchaseOrder(WMSCoreMessage oRequest)
        {

            try
            {
                LoadControllerDefaults(oRequest);

                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {
                    List<InboundDataDTO> aPISORequestDTO = (List<InboundDataDTO>)WMSCoreEndpointSecurity.ValidateRequest(oRequest);


                    string authtoken = oRequest.AuthToken.AuthKey;

                    List<MasterDataCreation> aPISOCreations = new List<MasterDataCreation>();
                    MasterDataCreation aPISOCreation = new MasterDataCreation();

                    bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);

                    // bool result = true;
                    if (result == true)
                    {

                        //APISOCreateRequestDTO aPISOCreateRequestDTO = new APISOCreateRequestDTO();
                        // List<SupplierCreation> _lstAPISORequestDTO = aPISORequestDTO.OrderReuest;

                        string finalresult = "";
                        var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

                        emptyNamepsaces.Add("", "");
                        var settings = new XmlWriterSettings();
                        string xmlString = null;
                        using (StringWriter sw = new StringWriter())
                        {
                            XmlSerializer serialiser = new XmlSerializer(typeof(List<InboundDataDTO>));
                            serialiser.Serialize(sw, aPISORequestDTO, emptyNamepsaces);

                            xmlString = sw.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", " ");


                        }


                        finalresult = oshipperIDIntergrationBL.CreatePurchaseOrder(xmlString, authtoken);


                        string json = finalresult;
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }

                }

                else
                {
                    return null;
                }
            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return myCleanJsonObject;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "001");
                return null;
            }
        }


        [HttpPost]
        [Route("CreateSalesOrder")]
        public object CreateSalesOrder(WMSCoreMessage oRequest)
        {

            try
            {
                LoadControllerDefaults(oRequest);

                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {
                    APISOCreateRequestDTO aPISORequestDTO = (APISOCreateRequestDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);


                    string authtoken = oRequest.AuthToken.AuthKey;

                    List<APISOCreation> aPISOCreations = new List<APISOCreation>();
                    APISOCreation aPISOCreation = new APISOCreation();

                    bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);


                    if (result == true)
                    {

                        //APISOCreateRequestDTO aPISOCreateRequestDTO = new APISOCreateRequestDTO();
                        List<SOCreationDTO> _lstAPISORequestDTO = aPISORequestDTO.OrderReuest;

                        string finalresult = "";
                        var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

                        emptyNamepsaces.Add("", "");
                        var settings = new XmlWriterSettings();
                        string xmlString = null;
                        using (StringWriter sw = new StringWriter())
                        {
                            XmlSerializer serialiser = new XmlSerializer(typeof(List<SOCreationDTO>));
                            serialiser.Serialize(sw, _lstAPISORequestDTO, emptyNamepsaces);

                            xmlString = sw.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", " ");


                        }


                        finalresult = oshipperIDIntergrationBL.CreateSalesOrder(xmlString, authtoken);


                        string json = finalresult;
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }

                }

                else
                {
                    return null;
                }
            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return myCleanJsonObject;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "001");
                return null;
            }
        }


        //============ posting Inbound =========================================//
        [HttpPost]
        [Route("PostInboundData")]
        public Object InboundData(WMSCoreMessage oRequest)
        {

            try
            {
                LoadControllerDefaults(oRequest);

                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {
                    List<InboundDataDTO> aPISORequestDTO = (List<InboundDataDTO>)WMSCoreEndpointSecurity.ValidateRequest(oRequest);


                    string authtoken = oRequest.AuthToken.AuthKey;

                    List<MasterDataCreation> aPISOCreations = new List<MasterDataCreation>();
                    MasterDataCreation aPISOCreation = new MasterDataCreation();

                    bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);

                    // bool result = true;
                    if (result == true)
                    {

                        //APISOCreateRequestDTO aPISOCreateRequestDTO = new APISOCreateRequestDTO();
                        // List<SupplierCreation> _lstAPISORequestDTO = aPISORequestDTO.OrderReuest;

                        string finalresult = "";
                        var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

                        emptyNamepsaces.Add("", "");
                        var settings = new XmlWriterSettings();
                        string xmlString = null;
                        using (StringWriter sw = new StringWriter())
                        {
                            XmlSerializer serialiser = new XmlSerializer(typeof(List<InboundDataDTO>));
                            serialiser.Serialize(sw, aPISORequestDTO, emptyNamepsaces);

                            xmlString = sw.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", " ");


                        }


                        finalresult = oshipperIDIntergrationBL.UpsertBulkInbound(xmlString, authtoken);


                        string json = finalresult;
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }

                }

                else
                {
                    return null;
                }
            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return myCleanJsonObject;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "001");
                return null;
            }
        }


        //============================ End of Posting Inbound =========================//

        [HttpPost]
        [Route("CreateCustomer")]
        public object CreateCustomer(WMSCoreMessage oRequest)
        {

            try
            {
                LoadControllerDefaults(oRequest);

                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {
                    //APISOCreateRequestDTO aPISORequestDTO = (APISOCreateRequestDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    List<CustomerCreationDTO> aPISORequestDTO = (List<CustomerCreationDTO>)WMSCoreEndpointSecurity.ValidateRequest(oRequest);


                    string authtoken = oRequest.AuthToken.AuthKey;

                    List<APISOCreation> aPISOCreations = new List<APISOCreation>();
                    APISOCreation aPISOCreation = new APISOCreation();

                    bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);


                    if (result == true)
                    {

                        //APISOCreateRequestDTO aPISOCreateRequestDTO = new APISOCreateRequestDTO();
                        // List<SOCreationDTO> _lstAPISORequestDTO = aPISORequestDTO.OrderReuest;

                        string finalresult = "";
                        var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

                        emptyNamepsaces.Add("", "");
                        var settings = new XmlWriterSettings();
                        string xmlString = null;
                        using (StringWriter sw = new StringWriter())
                        {
                            XmlSerializer serialiser = new XmlSerializer(typeof(List<CustomerCreationDTO>));
                            serialiser.Serialize(sw, aPISORequestDTO, emptyNamepsaces);

                            xmlString = sw.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", " ");


                        }


                        finalresult = oshipperIDIntergrationBL.CreateCustomer(xmlString, authtoken);


                        string json = finalresult;
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }

                }

                else
                {
                    return null;
                }
            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return myCleanJsonObject;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "001");
                return null;
            }
        }


        [HttpPost]
        [Route("CreateSupplier")]
        public object CreateSupplier(WMSCoreMessage oRequest)
        {

            try
            {
                LoadControllerDefaults(oRequest);

                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {
                    //APISOCreateRequestDTO aPISORequestDTO = (APISOCreateRequestDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    List<SupplierCreationDTO> aPISORequestDTO = (List<SupplierCreationDTO>)WMSCoreEndpointSecurity.ValidateRequest(oRequest);


                    string authtoken = oRequest.AuthToken.AuthKey;

                    List<APISOCreation> aPISOCreations = new List<APISOCreation>();
                    APISOCreation aPISOCreation = new APISOCreation();

                    bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);


                    if (result == true)
                    {

                        //APISOCreateRequestDTO aPISOCreateRequestDTO = new APISOCreateRequestDTO();
                        // List<SOCreationDTO> _lstAPISORequestDTO = aPISORequestDTO.OrderReuest;

                        string finalresult = "";
                        var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

                        emptyNamepsaces.Add("", "");
                        var settings = new XmlWriterSettings();
                        string xmlString = null;
                        using (StringWriter sw = new StringWriter())
                        {
                            XmlSerializer serialiser = new XmlSerializer(typeof(List<SupplierCreationDTO>));
                            serialiser.Serialize(sw, aPISORequestDTO, emptyNamepsaces);

                            xmlString = sw.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", " ");


                        }


                        finalresult = oshipperIDIntergrationBL.CreateSupplier(xmlString, authtoken);


                        string json = finalresult;
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }

                }

                else
                {
                    return null;
                }
            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return myCleanJsonObject;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "001");
                return null;
            }
        }

        [HttpPost]
        [Route("PostCustomerReturnData")]
        public Object PostCustomerReturnDatas(WMSCoreMessage oRequest)
        {

            try
            {
                LoadControllerDefaults(oRequest);

                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {
                    List<CustomerReturnDTO> aPISORequestDTO = (List<CustomerReturnDTO>)WMSCoreEndpointSecurity.ValidateRequest(oRequest);


                    string authtoken = oRequest.AuthToken.AuthKey;

                    List<CustomerReturnDTO> aPISOCreations = new List<CustomerReturnDTO>();
                    CustomerReturnDTO aPISOCreation = new CustomerReturnDTO();

                    bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);

                    // bool result = true;
                    if (result == true)
                    {

                        //APISOCreateRequestDTO aPISOCreateRequestDTO = new APISOCreateRequestDTO();
                        // List<SupplierCreation> _lstAPISORequestDTO = aPISORequestDTO.OrderReuest;

                        string finalresult = "";
                        var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

                        emptyNamepsaces.Add("", "");
                        var settings = new XmlWriterSettings();
                        string xmlString = null;
                        using (StringWriter sw = new StringWriter())
                        {
                            XmlSerializer serialiser = new XmlSerializer(typeof(List<CustomerReturnDTO>));
                            serialiser.Serialize(sw, aPISORequestDTO, emptyNamepsaces);

                            xmlString = sw.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", " ");


                        }


                        finalresult = oshipperIDIntergrationBL.UpsertCustomerReturn(xmlString, authtoken);


                        string json = finalresult;
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }

                }

                else
                {
                    return null;
                }
            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return myCleanJsonObject;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "001");
                return null;
            }
        }




        [HttpPost]
        [Route("PostSupplierReturn")]
        public Object PostSupplierReturnData(WMSCoreMessage oRequest)
        {

            try
            {
                LoadControllerDefaults(oRequest);

                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {
                    List<SupplierReturnDTO> aPISORequestDTO = (List<SupplierReturnDTO>)WMSCoreEndpointSecurity.ValidateRequest(oRequest);


                    string authtoken = oRequest.AuthToken.AuthKey;

                    //List<MasterDataCreation> aPISOCreations = new List<MasterDataCreation>();
                    //MasterDataCreation aPISOCreation = new MasterDataCreation();

                    bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);

                    // bool result = true;
                    if (result == true)
                    {

                        //APISOCreateRequestDTO aPISOCreateRequestDTO = new APISOCreateRequestDTO();
                        // List<SupplierCreation> _lstAPISORequestDTO = aPISORequestDTO.OrderReuest;

                        string finalresult = "";
                        var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

                        emptyNamepsaces.Add("", "");
                        var settings = new XmlWriterSettings();
                        string xmlString = null;
                        using (StringWriter sw = new StringWriter())
                        {
                            XmlSerializer serialiser = new XmlSerializer(typeof(List<SupplierReturnDTO>));
                            serialiser.Serialize(sw, aPISORequestDTO, emptyNamepsaces);

                            xmlString = sw.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", " ");


                        }


                        finalresult = oshipperIDIntergrationBL.UpsertSupplierReturn(xmlString, authtoken);


                        string json = finalresult;
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }

                }

                else
                {
                    return null;
                }
            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return myCleanJsonObject;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "001");
                return null;
            }
        }






        //======================== Supplier Posting=====================//
        [HttpPost]
        [Route("PostSupplierData")]
        public Object SupplierData(WMSCoreMessage oRequest)
        {

            try
            {
                LoadControllerDefaults(oRequest);

                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {
                    List<SupplierImportDTO> aPISORequestDTO = (List<SupplierImportDTO>)WMSCoreEndpointSecurity.ValidateRequest(oRequest);


                    string authtoken = oRequest.AuthToken.AuthKey;

                    //List<MasterDataCreation> aPISOCreations = new List<MasterDataCreation>();
                    //MasterDataCreation aPISOCreation = new MasterDataCreation();

                    bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);

                    // bool result = true;
                    if (result == true)
                    {

                        //APISOCreateRequestDTO aPISOCreateRequestDTO = new APISOCreateRequestDTO();
                        // List<SupplierCreation> _lstAPISORequestDTO = aPISORequestDTO.OrderReuest;

                        string finalresult = "";
                        var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

                        emptyNamepsaces.Add("", "");
                        var settings = new XmlWriterSettings();
                        string xmlString = null;
                        using (StringWriter sw = new StringWriter())
                        {
                            XmlSerializer serialiser = new XmlSerializer(typeof(List<SupplierImportDTO>));
                            serialiser.Serialize(sw, aPISORequestDTO, emptyNamepsaces);

                            xmlString = sw.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", " ");


                        }


                        finalresult = oshipperIDIntergrationBL.UpsertBulkSupplier(xmlString, authtoken);


                        string json = finalresult;
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }

                }

                else
                {
                    return null;
                }
            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return myCleanJsonObject;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "001");
                return null;
            }
        }

        //======================= End of Supplier Posting=============//


        //==================== Start of Item master Posting =============//

        [HttpPost]
        [Route("PostItemMasterData")]
        public object ItemMasterData(WMSCoreMessage oRequest)
        {

            try
            {
                LoadControllerDefaults(oRequest);

                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {
                    List<ItemMasterDTO> aPISORequestDTO = (List<ItemMasterDTO>)WMSCoreEndpointSecurity.ValidateRequest(oRequest);


                    string authtoken = oRequest.AuthToken.AuthKey;

                    //List<MasterDataCreation> aPISOCreations = new List<MasterDataCreation>();
                    //MasterDataCreation aPISOCreation = new MasterDataCreation();

                    bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);

                    // bool result = true;
                    if (result == true)
                    {

                        //APISOCreateRequestDTO aPISOCreateRequestDTO = new APISOCreateRequestDTO();
                        // List<SupplierCreation> _lstAPISORequestDTO = aPISORequestDTO.OrderReuest;

                        string finalresult = "";
                        var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

                        emptyNamepsaces.Add("", "");
                        var settings = new XmlWriterSettings();
                        string xmlString = null;
                        using (StringWriter sw = new StringWriter())
                        {
                            XmlSerializer serialiser = new XmlSerializer(typeof(List<ItemMasterDTO>));
                            serialiser.Serialize(sw, aPISORequestDTO, emptyNamepsaces);

                            xmlString = sw.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", " ");


                        }


                        finalresult = oshipperIDIntergrationBL.UpsertBulkItemmaster(xmlString, authtoken);


                        string json = finalresult;
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }

                }

                else
                {
                    return null;
                }
            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return myCleanJsonObject;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "001");
                return null;
            }
        }



        //========== Enn of Item master Posting =========================//


        [HttpPost]
        [Route("PutItemMasterData")]
        public Object PutItemMasterData(WMSCoreMessage oRequest)
        {

            try
            {
                LoadControllerDefaults(oRequest);

                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {
                    List<UpdateItemMasterDTO> aPISORequestDTO = (List<UpdateItemMasterDTO>)WMSCoreEndpointSecurity.ValidateRequest(oRequest);


                    string authtoken = oRequest.AuthToken.AuthKey;

                    //List<MasterDataCreation> aPISOCreations = new List<MasterDataCreation>();
                    //MasterDataCreation aPISOCreation = new MasterDataCreation();

                    //bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);

                     bool result = true;
                    if (result == true)
                    {

                        //APISOCreateRequestDTO aPISOCreateRequestDTO = new APISOCreateRequestDTO();
                        // List<SupplierCreation> _lstAPISORequestDTO = aPISORequestDTO.OrderReuest;

                        string finalresult = "";
                        var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

                        emptyNamepsaces.Add("", "");
                        var settings = new XmlWriterSettings();
                        string xmlString = null;
                        using (StringWriter sw = new StringWriter())
                        {
                            XmlSerializer serialiser = new XmlSerializer(typeof(List<UpdateItemMasterDTO>));
                            serialiser.Serialize(sw, aPISORequestDTO, emptyNamepsaces);

                            xmlString = sw.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", " ");


                        }


                        finalresult = oshipperIDIntergrationBL.UpsertPutItemMasterData(xmlString, authtoken);


                        string json = finalresult;
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }

                }

                else
                {
                    return null;
                }
            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return myCleanJsonObject;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "001");
                return null;
            }
        }


        [HttpPost]
        [Route("PutSupplierData")]
        public Object PutSupplierDatas(WMSCoreMessage oRequest)
        {

            try
            {
                LoadControllerDefaults(oRequest);

                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {
                    List<UpdateSupplierDTO> aPISORequestDTO = (List<UpdateSupplierDTO>)WMSCoreEndpointSecurity.ValidateRequest(oRequest);


                    string authtoken = oRequest.AuthToken.AuthKey;

                    //List<MasterDataCreation> aPISOCreations = new List<MasterDataCreation>();
                    //MasterDataCreation aPISOCreation = new MasterDataCreation();

                    bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);

                    // bool result = true;
                    if (result == true)
                    {

                        //APISOCreateRequestDTO aPISOCreateRequestDTO = new APISOCreateRequestDTO();
                        // List<SupplierCreation> _lstAPISORequestDTO = aPISORequestDTO.OrderReuest;

                        string finalresult = "";
                        var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

                        emptyNamepsaces.Add("", "");
                        var settings = new XmlWriterSettings();
                        string xmlString = null;
                        using (StringWriter sw = new StringWriter())
                        {
                            XmlSerializer serialiser = new XmlSerializer(typeof(List<UpdateSupplierDTO>));
                            serialiser.Serialize(sw, aPISORequestDTO, emptyNamepsaces);

                            xmlString = sw.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", " ");


                        }


                        finalresult = oshipperIDIntergrationBL.UpsertPutSupplierData(xmlString, authtoken);


                        string json = finalresult;
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }

                }

                else
                {
                    return null;
                }
            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return myCleanJsonObject;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "001");
                return null;
            }
        }








        [HttpGet]
        [Route("FetchWarehouseInventory")]
        public object FetchWarehouseInventory(WMSCoreMessage oRequest)
        {
            try
            {
                LoadControllerDefaults(oRequest);
                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {

                    APIInventoryDTO oAPIInventoryDTO = (APIInventoryDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    string authtoken = oRequest.AuthToken.AuthKey;

                    bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);

                    if (result == true)
                    {

                        List<APIInventoryDTO> _lstinventoryDTOs = new List<APIInventoryDTO>();

                        List<APIInventory> inventories = new List<APIInventory>();

                        APIInventory inventoryItem = new APIInventory()
                        {
                            StartDate = oAPIInventoryDTO.StartDate.ToString(),
                            WareHouseCode = oAPIInventoryDTO.WareHouseCode,
                            PaginationID = oAPIInventoryDTO.PaginationID,
                            PageSize = oAPIInventoryDTO.PageSize,
                            UpdatedDate = oAPIInventoryDTO.UpdatedDate.ToString()
                        };

                        inventories = oshipperIDIntergrationBL.GetWarehouseInventoryData(inventoryItem, authtoken);


                        foreach (APIInventory inventoryItems in inventories)
                        {
                            APIInventoryDTO _oInventoryDTO = new APIInventoryDTO();
                            _oInventoryDTO.WareHouseCode = inventoryItems.WareHouseCode.ToString();
                            _oInventoryDTO.TenantCode = inventoryItems.TenantCode.ToString();
                            _oInventoryDTO.MaterialCode = inventoryItems.MaterialCode.ToString();
                            _oInventoryDTO.MaterialDescription = inventoryItems.MaterialDescription.ToString();
                            _oInventoryDTO.UoMQty = inventoryItems.UoMQty.ToString();
                            _oInventoryDTO.MfgDate = inventoryItems.MfgDate.ToString();
                            _oInventoryDTO.ExpDate = inventoryItems.ExpDate.ToString();
                            _oInventoryDTO.BatchNo = inventoryItems.BatchNo.ToString();
                            _oInventoryDTO.SerialNo = inventoryItems.SerialNo.ToString();
                            _oInventoryDTO.ProjectRefNo = inventoryItems.ProjectRefNo.ToString();
                            _oInventoryDTO.MRP = inventoryItems.MRP.ToString();
                            _oInventoryDTO.AvailableQuantity = inventoryItems.AvailableQuantity;
                            _oInventoryDTO.OnHandQty = inventoryItems.OnHandQty;
                            _oInventoryDTO.AllocatedQty = inventoryItems.AllocatedQty;
                            _oInventoryDTO.DamagedQty = inventoryItems.DamagedQty;
                            _oInventoryDTO.TotalRecords = inventoryItems.TotalRecords.ToString();



                            _lstinventoryDTOs.Add(_oInventoryDTO);
                        }


                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, _lstinventoryDTOs));
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }


                }
                else
                {
                    return null;
                }



            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return json;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "002");
                return null;
            }
        }




        [HttpGet]
        [Route("GetReleaseOBDStatus")]
        public Object GetReleaseOutboundStatus(WMSCoreMessage oRequest)
        {
            try
            {
                LoadControllerDefaults(oRequest);
                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {

                    ReleaseOutbound oMastersDTO = (ReleaseOutbound)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    string authtoken = oRequest.AuthToken.AuthKey;

                    bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);
                    // bool result = true;
                    if (result == true)
                    {



                        string GetOBDData;

                        //APIInventory inventoryItem = new APIInventory()
                        //{
                        //    StartDate = oAPIInventoryDTO.StartDate.ToString(),
                        //    WareHouseCode = oAPIInventoryDTO.WareHouseCode,
                        //    PaginationID = oAPIInventoryDTO.PaginationID,
                        //    PageSize = oAPIInventoryDTO.PageSize
                        //};

                        GetOBDData = oshipperIDIntergrationBL.GetReleaseOBD(oMastersDTO.StartDate, oMastersDTO.EndDate, oMastersDTO.WarehouseCode, oMastersDTO.PaginationID, oMastersDTO.PageSize, authtoken);

                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(GetOBDData);
                        return myCleanJsonObject;


                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }


                }
                else
                {
                    return null;
                }



            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return myCleanJsonObject;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "002");
                return null;
            }
        }





        [HttpGet]
        [Route("GetTenantList")]
        public Object GetTenantList(WMSCoreMessage oRequest)
        {
            try
            {
                LoadControllerDefaults(oRequest);
                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {

                    MastersDTO oMastersDTO = (MastersDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    string authtoken = oRequest.AuthToken.AuthKey;

                    bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);
                    // bool result = true;
                    if (result == true)
                    {



                        string tenantData;

                        //APIInventory inventoryItem = new APIInventory()
                        //{
                        //    StartDate = oAPIInventoryDTO.StartDate.ToString(),
                        //    WareHouseCode = oAPIInventoryDTO.WareHouseCode,
                        //    PaginationID = oAPIInventoryDTO.PaginationID,
                        //    PageSize = oAPIInventoryDTO.PageSize
                        //};

                        tenantData = oshipperIDIntergrationBL.GetTenatData(oMastersDTO.StartNum, oMastersDTO.EndNum, authtoken);

                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(tenantData);
                        return myCleanJsonObject;
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }


                }
                else
                {
                    return null;
                }



            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return myCleanJsonObject;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "002");
                return null;
            }
        }





        [HttpGet]
        [Route("GetSupplierList")]
        public object GetSupplierList(WMSCoreMessage oRequest)
        {
            try
            {
                LoadControllerDefaults(oRequest);
                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {

                    MastersDTO oMastersDTO = (MastersDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    string authtoken = oRequest.AuthToken.AuthKey;

                    bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);
                    //bool result = true;
                    if (result == true)
                    {



                        string Supplierdata;

                        //APIInventory inventoryItem = new APIInventory()
                        //{
                        //    StartDate = oAPIInventoryDTO.StartDate.ToString(),
                        //    WareHouseCode = oAPIInventoryDTO.WareHouseCode,
                        //    PaginationID = oAPIInventoryDTO.PaginationID,
                        //    PageSize = oAPIInventoryDTO.PageSize
                        //};

                        Supplierdata = oshipperIDIntergrationBL.GetSupplierList(oMastersDTO.StartNum, oMastersDTO.EndNum, oMastersDTO.UpdatedDate, authtoken);

                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(Supplierdata);
                        return myCleanJsonObject;
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }


                }
                else
                {
                    return null;
                }



            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return myCleanJsonObject;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "002");
                return null;
            }
        }



        [HttpGet]
        [Route("GetSupplierListForSupplier")]
        public Object GetSupplierListForSpecificSupplier(WMSCoreMessage oRequest, [FromUri] int SupplierID)
        {
            try
            {
                LoadControllerDefaults(oRequest);
                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {

                    MastersDTO oMastersDTO = (MastersDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    string authtoken = oRequest.AuthToken.AuthKey;

                    bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);
                    //bool result = true;
                    if (result == true)
                    {



                        string Supplierdata;

                        //APIInventory inventoryItem = new APIInventory()
                        //{
                        //    StartDate = oAPIInventoryDTO.StartDate.ToString(),
                        //    WareHouseCode = oAPIInventoryDTO.WareHouseCode,
                        //    PaginationID = oAPIInventoryDTO.PaginationID,
                        //    PageSize = oAPIInventoryDTO.PageSize
                        //};

                        Supplierdata = oshipperIDIntergrationBL.GetSupplierSpecList(oMastersDTO.StartNum, oMastersDTO.EndNum, authtoken, SupplierID);
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(Supplierdata);
                        return myCleanJsonObject;
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }


                }
                else
                {
                    return null;
                }



            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return myCleanJsonObject;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "002");
                return null;
            }
        }



        [HttpGet]
        [Route("GetItemMasterListForSpec")]
        public Object GetItemMasterListForSpecItem(WMSCoreMessage oRequest, [FromUri] int MaterialMasterID)
        {
            try
            {


                LoadControllerDefaults(oRequest);
                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {

                    MastersDTO oMastersDTO = (MastersDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    string authtoken = oRequest.AuthToken.AuthKey;

                    bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);
                    //bool result = true;
                    if (result == true)
                    {



                        string ItemData;

                        //APIInventory inventoryItem = new APIInventory()
                        //{
                        //    StartDate = oAPIInventoryDTO.StartDate.ToString(),
                        //    WareHouseCode = oAPIInventoryDTO.WareHouseCode,
                        //    PaginationID = oAPIInventoryDTO.PaginationID,
                        //    PageSize = oAPIInventoryDTO.PageSize
                        //};

                        ItemData = oshipperIDIntergrationBL.GetMaterialSpecList(oMastersDTO.StartNum, oMastersDTO.EndNum, authtoken, MaterialMasterID);

                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(ItemData);
                        return myCleanJsonObject;
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }


                }
                else
                {
                    return null;
                }



            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return myCleanJsonObject;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "002");
                return null;
            }
        }






        [HttpGet]
        [Route("GetInboundListForSpec")]
        public Object GetInboundListForSpecs(WMSCoreMessage oRequest, [FromUri] int InboundID)
        {
            try
            {
                LoadControllerDefaults(oRequest);
                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {

                    MastersDTO oMastersDTO = (MastersDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    string authtoken = oRequest.AuthToken.AuthKey;

                    bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);
                    //bool result = true;
                    if (result == true)
                    {



                        string InboundList;

                        //APIInventory inventoryItem = new APIInventory()
                        //{
                        //    StartDate = oAPIInventoryDTO.StartDate.ToString(),
                        //    WareHouseCode = oAPIInventoryDTO.WareHouseCode,
                        //    PaginationID = oAPIInventoryDTO.PaginationID,
                        //    PageSize = oAPIInventoryDTO.PageSize
                        //};

                        InboundList = oshipperIDIntergrationBL.GetInboundSpecList(oMastersDTO.StartNum, oMastersDTO.EndNum, authtoken, InboundID);

                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(InboundList);
                        return myCleanJsonObject;
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));

                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }


                }
                else
                {
                    return null;
                }



            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return myCleanJsonObject;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "002");
                return null;
            }
        }

        [HttpGet]
        [Route("GetSLocToSLOCList")]
        public Object GetSLocToSLOCTransfer(WMSCoreMessage oRequest)
        {
            try
            {
                LoadControllerDefaults(oRequest);
                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {

                    SLOCToSLOCDTO oMastersDTO = (SLOCToSLOCDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    string authtoken = oRequest.AuthToken.AuthKey;

                    bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);
                    //bool result = true;
                    if (result == true)
                    {



                        string Sloc;

                        //APIInventory inventoryItem = new APIInventory()
                        //{
                        //    StartDate = oAPIInventoryDTO.StartDate.ToString(),
                        //    WareHouseCode = oAPIInventoryDTO.WareHouseCode,
                        //    PaginationID = oAPIInventoryDTO.PaginationID,
                        //    PageSize = oAPIInventoryDTO.PageSize
                        //};

                        Sloc = oshipperIDIntergrationBL.GetSLocToSLocList(oMastersDTO.StartDate, oMastersDTO.EndDate, oMastersDTO.WareHouseCode, oMastersDTO.PaginationId, oMastersDTO.Pagesize, authtoken);

                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(Sloc);
                        return myCleanJsonObject;
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }


                }
                else
                {
                    return null;
                }



            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return myCleanJsonObject;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "002");
                return null;
            }
        }


        [HttpGet]
        [Route("GetBinToBinList")]
        public Object GetBinToBinTransfer(WMSCoreMessage oRequest)
        {
            try
            {
                LoadControllerDefaults(oRequest);
                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {

                    BinToBinDTO oMastersDTO = (BinToBinDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    string authtoken = oRequest.AuthToken.AuthKey;

                    bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);
                    //bool result = true;
                    if (result == true)
                    {



                        string BintoBin;

                        //APIInventory inventoryItem = new APIInventory()
                        //{
                        //    StartDate = oAPIInventoryDTO.StartDate.ToString(),
                        //    WareHouseCode = oAPIInventoryDTO.WareHouseCode,
                        //    PaginationID = oAPIInventoryDTO.PaginationID,
                        //    PageSize = oAPIInventoryDTO.PageSize
                        //};

                        BintoBin = oshipperIDIntergrationBL.GetBinToBinList(oMastersDTO.StartDate, oMastersDTO.EndDate, oMastersDTO.WareHouseCode, oMastersDTO.PaginationId, oMastersDTO.Pagesize, oMastersDTO.TenantID, authtoken);

                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(BintoBin);
                        return myCleanJsonObject;
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));

                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }


                }
                else
                {
                    return null;
                }



            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return myCleanJsonObject;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "002");
                return null;
            }
        }




        [HttpGet]
        [Route("GetItemasterList")]
        public object GetItemMasterList(WMSCoreMessage oRequest)
        {
            try
            {
                LoadControllerDefaults(oRequest);
                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {

                    MastersDTO oMastersDTO = (MastersDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    string authtoken = oRequest.AuthToken.AuthKey;

                    bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);
                    // bool result = true;
                    if (result == true)
                    {



                        string ItemMasterdata;

                        //APIInventory inventoryItem = new APIInventory()
                        //{
                        //    StartDate = oAPIInventoryDTO.StartDate.ToString(),
                        //    WareHouseCode = oAPIInventoryDTO.WareHouseCode,
                        //    PaginationID = oAPIInventoryDTO.PaginationID,
                        //    PageSize = oAPIInventoryDTO.PageSize
                        //};

                        ItemMasterdata = oshipperIDIntergrationBL.GetItemMasterList(oMastersDTO.StartNum, oMastersDTO.EndNum, authtoken);

                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(ItemMasterdata);
                        return myCleanJsonObject;
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;

                    }


                }
                else
                {
                    return null;
                }



            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return myCleanJsonObject;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "002");
                return null;
            }
        }




        [HttpGet]
        [Route("GetInboundList")]
        public Object GetInboundList(WMSCoreMessage oRequest)
        {
            try
            {
                LoadControllerDefaults(oRequest);
                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {

                    MastersDTO oMastersDTO = (MastersDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    string authtoken = oRequest.AuthToken.AuthKey;

                    bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);
                    //  bool result = true;
                    if (result == true)
                    {



                        string Inbounddata;

                        //APIInventory inventoryItem = new APIInventory()
                        //{
                        //    StartDate = oAPIInventoryDTO.StartDate.ToString(),
                        //    WareHouseCode = oAPIInventoryDTO.WareHouseCode,
                        //    PaginationID = oAPIInventoryDTO.PaginationID,
                        //    PageSize = oAPIInventoryDTO.PageSize
                        //};

                        Inbounddata = oshipperIDIntergrationBL.GetInboundList(oMastersDTO.StartNum, oMastersDTO.EndNum, oMastersDTO.UpdatedDate, authtoken);

                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(Inbounddata);
                        return myCleanJsonObject;
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }


                }
                else
                {
                    return null;
                }



            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return myCleanJsonObject;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "002");
                return null;
            }
        }




        [HttpGet]
        [Route("GetInboundListForCustomerReturn")]
        public Object GetInboundListForCustomerReturns(WMSCoreMessage oRequest)
        {
            try
            {
                LoadControllerDefaults(oRequest);
                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {

                    MastersDTO oMastersDTO = (MastersDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    string authtoken = oRequest.AuthToken.AuthKey;

                    bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);
                    //  bool result = true;
                    if (result == true)
                    {



                        string CustomerReturns;

                        //APIInventory inventoryItem = new APIInventory()
                        //{
                        //    StartDate = oAPIInventoryDTO.StartDate.ToString(),
                        //    WareHouseCode = oAPIInventoryDTO.WareHouseCode,
                        //    PaginationID = oAPIInventoryDTO.PaginationID,
                        //    PageSize = oAPIInventoryDTO.PageSize
                        //};

                        CustomerReturns = oshipperIDIntergrationBL.GetInboundCustomerReturn(oMastersDTO.StartNum, oMastersDTO.EndNum, authtoken);

                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(CustomerReturns);
                        return myCleanJsonObject;
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }


                }
                else
                {
                    return null;
                }



            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return myCleanJsonObject;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "002");
                return null;
            }
        }


        [HttpGet]
        [Route("GetOutboundListForSupplierReturn")]
        public Object GetOutboundListForSupplierReturns(WMSCoreMessage oRequest)
        {
            try
            {
                LoadControllerDefaults(oRequest);
                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {

                    SupplierReturnOBDDTO oMastersDTO = (SupplierReturnOBDDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    string authtoken = oRequest.AuthToken.AuthKey;

                    bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);
                    //  bool result = true;
                    if (result == true)
                    {



                        string SupOBDData;

                        //APIInventory inventoryItem = new APIInventory()
                        //{
                        //    StartDate = oAPIInventoryDTO.StartDate.ToString(),
                        //    WareHouseCode = oAPIInventoryDTO.WareHouseCode,
                        //    PaginationID = oAPIInventoryDTO.PaginationID,
                        //    PageSize = oAPIInventoryDTO.PageSize
                        //};

                        SupOBDData = oshipperIDIntergrationBL.GetOutboundSupplierReturn(oMastersDTO.StartDate, oMastersDTO.EndDate, oMastersDTO.PaginationID, oMastersDTO.PageSize, oMastersDTO.WareHouseCode, authtoken);

                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(SupOBDData);
                        return myCleanJsonObject;
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }


                }
                else
                {
                    return null;
                }



            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return myCleanJsonObject;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "002");
                return null;
            }
        }






        [HttpGet]
        [Route("GetwarehouseInventory")]
        public Object warehouseInventory([FromBody]WMSCoreMessage oRequest, [FromUri] int WareHouseID)
        {
            try
            {
                LoadControllerDefaults(oRequest);
                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {

                    MastersDTO oMastersDTO = (MastersDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    string authtoken = oRequest.AuthToken.AuthKey;

                    bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);
                    // bool result = true;
                    if (result == true)
                    {



                        string WHData;

                        //APIInventory inventoryItem = new APIInventory()
                        //{
                        //    StartDate = oAPIInventoryDTO.StartDate.ToString(),
                        //    WareHouseCode = oAPIInventoryDTO.WareHouseCode,
                        //    PaginationID = oAPIInventoryDTO.PaginationID,
                        //    PageSize = oAPIInventoryDTO.PageSize
                        //};

                        WHData = oshipperIDIntergrationBL.GetIventoryWH(oMastersDTO.StartNum, oMastersDTO.EndNum, authtoken, WareHouseID);

                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(WHData);
                        return myCleanJsonObject;
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }


                }
                else
                {
                    return null;
                }



            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return myCleanJsonObject;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "002");
                return null;
            }
        }


        [HttpGet]
        [Route("GetwarehouseInventorydetails")]
        public Object warehouseInventorydetails([FromBody]WMSCoreMessage oRequest, [FromUri] int MaterialMasterID)
        {
            try
            {
                LoadControllerDefaults(oRequest);
                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {

                    MastersDTO oMastersDTO = (MastersDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    string authtoken = oRequest.AuthToken.AuthKey;

                    bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);
                    //  bool result = true;
                    if (result == true)
                    {



                        string WHDatas;

                        //APIInventory inventoryItem = new APIInventory()
                        //{
                        //    StartDate = oAPIInventoryDTO.StartDate.ToString(),
                        //    WareHouseCode = oAPIInventoryDTO.WareHouseCode,
                        //    PaginationID = oAPIInventoryDTO.PaginationID,
                        //    PageSize = oAPIInventoryDTO.PageSize
                        //};

                        WHDatas = oshipperIDIntergrationBL.GetIventoryIMWH(oMastersDTO.StartNum, oMastersDTO.EndNum, authtoken, MaterialMasterID);

                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(WHDatas);
                        return myCleanJsonObject;
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }


                }
                else
                {
                    return null;
                }



            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return myCleanJsonObject;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "002");
                return null;
            }
        }




        [HttpGet]
        [Route("FetchOutboundOrders")]
        public Object FetchOutboundOrders([FromBody]WMSCoreMessage oRequest)
        {
            try
            {
                LoadControllerDefaults(oRequest);
                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {

                    OutboundOrdersDTO oMastersDTO = (OutboundOrdersDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    string authtoken = oRequest.AuthToken.AuthKey;

                    bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);
                    //  bool result = true;
                    if (result == true)
                    {



                        string ObdOrders;

                        //APIInventory inventoryItem = new APIInventory()
                        //{
                        //    StartDate = oAPIInventoryDTO.StartDate.ToString(),
                        //    WareHouseCode = oAPIInventoryDTO.WareHouseCode,
                        //    PaginationID = oAPIInventoryDTO.PaginationID,
                        //    PageSize = oAPIInventoryDTO.PageSize
                        //};

                        ObdOrders = oshipperIDIntergrationBL.FetOutboundOrder(oMastersDTO.StartDate, oMastersDTO.EndDate, oMastersDTO.PaginationID, oMastersDTO.PageSize, oMastersDTO.WareHouseCode, oMastersDTO.UpdatedDate, authtoken);

                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(ObdOrders);
                        return myCleanJsonObject;
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }


                }
                else
                {
                    return null;
                }



            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return myCleanJsonObject;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "002");
                return null;
            }
        }




        //[HttpGet]
        //[Route("FetchOutboundOrders")]
        //public string FetchOutboundOrders(WMSCoreMessage oRequest)
        //{
        //    try
        //    {
        //        LoadControllerDefaults(oRequest);
        //        ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);

        //        if (oRequest != null)
        //        {
        //            OutboundOrdersDTO oAPIInventoryDTO = (OutboundOrdersDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
        //            string authtoken = oRequest.AuthToken.AuthKey;

        //            bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);
        //            if (result == true)
        //            {

        //                List<OutboundOrdersDTO> outboundOrdersDTOs = new List<OutboundOrdersDTO>();
        //                List<OutboundOrders> outboundOrders = new List<OutboundOrders>();

        //                OutboundOrders outboundOrderItem = new OutboundOrders()
        //                {

        //                    StartDate = oAPIInventoryDTO.StartDate,
        //                    EndDate = oAPIInventoryDTO.EndDate,
        //                    PaginationID = oAPIInventoryDTO.PaginationID,
        //                    PageSize = oAPIInventoryDTO.PageSize,
        //                    WareHouseCode = oAPIInventoryDTO.WareHouseCode

        //                };
        //                outboundOrders = oshipperIDIntergrationBL.GetOutboundOrders(outboundOrderItem, authtoken);


        //                foreach(OutboundOrders orders in outboundOrders)
        //                {
        //                    OutboundOrdersDTO ordersDTO = new OutboundOrdersDTO();

        //                    ordersDTO.TenantCode = orders.TenantCode.ToString();
        //                    ordersDTO.WareHouseCode = orders.WareHouseCode.ToString();
        //                    ordersDTO.DeliveryDocNumber = orders.DeliveryDocNumber.ToString();
        //                    ordersDTO.InvoiceNo = orders.InvoiceNo.ToString();
        //                    ordersDTO.SONumber = orders.SONumber.ToString();
        //                    ordersDTO.AWBNo = orders.AWBNo.ToString();
        //                    ordersDTO.Courier = orders.Courier.ToString();
        //                    ordersDTO.DeliveryDocType = orders.DeliveryDocType.ToString();
        //                    ordersDTO.DeliveryDocDate = orders.DeliveryDocDate.ToString();
        //                    ordersDTO.PickingDate = orders.PickingDate.ToString();
        //                    ordersDTO.PackingDate = orders.PackingDate.ToString();
        //                    ordersDTO.LoadGeneratedDate = orders.LoadGeneratedDate.ToString();
        //                    ordersDTO.PGIDate = orders.PGIDate.ToString();
        //                    ordersDTO.DeliveryDate = orders.DeliveryDate.ToString();
        //                    ordersDTO.DeliveryStatus = orders.DeliveryStatus.ToString();
        //                    ordersDTO.TotalRecords = orders.TotalRecords.ToString();


        //                    outboundOrdersDTOs.Add(ordersDTO);
        //                }



        //                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.OutboundOrdersDTO, outboundOrdersDTOs));
        //                return json;
        //            }
        //            else
        //            {
        //                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));
        //                return json;
        //            }
        //        }
        //        else
        //        {
        //            return null;
        //        }



        //    }
        //    catch (WMSExceptionMessage ex)
        //    {
        //        List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
        //        _lstwMSExceptionMessage.Add(ex);
        //        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
        //        return json;


        //    }
        //    catch (Exception excp)
        //    {
        //        ExceptionHandling.LogException(excp, _ClassCode + "003");
        //        return null;
        //    }
        //}








        [HttpGet]
        [Route("FetchOutboundOrdersByOBD")]
        public Object FetchOutboundOrdersByOutBound(WMSCoreMessage oRequest, [FromUri] int OutBoundID)
        {
            try
            {
                LoadControllerDefaults(oRequest);
                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {

                    OutboundOrdersDTO oMastersDTO = (OutboundOrdersDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    string authtoken = oRequest.AuthToken.AuthKey;

                    bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);
                    // bool result = true;
                    if (result == true)
                    {



                        string OBDdata;

                        //APIInventory inventoryItem = new APIInventory()
                        //{
                        //    StartDate = oAPIInventoryDTO.StartDate.ToString(),
                        //    WareHouseCode = oAPIInventoryDTO.WareHouseCode,
                        //    PaginationID = oAPIInventoryDTO.PaginationID,
                        //    PageSize = oAPIInventoryDTO.PageSize
                        //};

                        OBDdata = oshipperIDIntergrationBL.FetchOutBoundByOBD(authtoken, OutBoundID);

                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(OBDdata);
                        return myCleanJsonObject;
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }


                }
                else
                {
                    return null;
                }



            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return myCleanJsonObject;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "002");
                return null;
            }
        }



        [HttpPost]
        [Route("RevertInbound")]
        public Object RevertParticularInbound(WMSCoreMessage oRequest, [FromUri] int InboundID)
        {
            try
            {
                LoadControllerDefaults(oRequest);
                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {

                    OutboundOrdersDTO oMastersDTO = (OutboundOrdersDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    string authtoken = oRequest.AuthToken.AuthKey;

                    bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);
                    // bool result = true;
                    if (result == true)
                    {



                        string RevertInbound;

                        //APIInventory inventoryItem = new APIInventory()
                        //{
                        //    StartDate = oAPIInventoryDTO.StartDate.ToString(),
                        //    WareHouseCode = oAPIInventoryDTO.WareHouseCode,
                        //    PaginationID = oAPIInventoryDTO.PaginationID,
                        //    PageSize = oAPIInventoryDTO.PageSize
                        //};

                        RevertInbound = oshipperIDIntergrationBL.RevertParticularInbounds(authtoken, InboundID);

                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(RevertInbound);
                        return myCleanJsonObject;
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }


                }
                else
                {
                    return null;
                }



            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return myCleanJsonObject;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "002");
                return null;
            }
        }

        [HttpPost]
        [Route("RevertOutbound")]
        public Object RevertRevertOutbound(WMSCoreMessage oRequest, [FromUri] int OutBound)
        {
            try
            {
                LoadControllerDefaults(oRequest);
                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {

                    OutboundOrdersDTO oMastersDTO = (OutboundOrdersDTO)WMSCoreEndpointSecurity.ValidateRequest(oRequest);
                    string authtoken = oRequest.AuthToken.AuthKey;

                    bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);
                    // bool result = true;
                    if (result == true)
                    {



                        string OBDRevert;

                        //APIInventory inventoryItem = new APIInventory()
                        //{
                        //    StartDate = oAPIInventoryDTO.StartDate.ToString(),
                        //    WareHouseCode = oAPIInventoryDTO.WareHouseCode,
                        //    PaginationID = oAPIInventoryDTO.PaginationID,
                        //    PageSize = oAPIInventoryDTO.PageSize
                        //};

                        OBDRevert = oshipperIDIntergrationBL.RevertOutbounds(authtoken, OutBound);

                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(OBDRevert);
                        return myCleanJsonObject;
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }


                }
                else
                {
                    return null;
                }



            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return myCleanJsonObject;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "002");
                return null;
            }
        }
        [HttpPost]
        [Route("MiscReceipt")]
        public object MiscReceipt(WMSCoreMessage oRequest)
        {

            try
            {
                LoadControllerDefaults(oRequest);

                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {
                    List<MisslleniousReceiptDTO> aPIMRRequestDTO = (List<MisslleniousReceiptDTO>)WMSCoreEndpointSecurity.ValidateRequest(oRequest);


                    string authtoken = oRequest.AuthToken.AuthKey;

                    List<MisslleniousReceiptDTO> aPISOCreations = new List<MisslleniousReceiptDTO>();
                    MisslleniousReceiptDTO aPISOCreation = new MisslleniousReceiptDTO();

                    bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);


                    if (result == true)
                    {

                        //APISOCreateRequestDTO aPISOCreateRequestDTO = new APISOCreateRequestDTO();
                      //  List<MisslleniousReceiptDTO> _lstAPISORequestDTO = aPIMRRequestDTO;

                        string finalresult = "";
                        var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

                        emptyNamepsaces.Add("", "");
                        var settings = new XmlWriterSettings();
                        string xmlString = null;
                        using (StringWriter sw = new StringWriter())
                        {
                            XmlSerializer serialiser = new XmlSerializer(typeof(List<MisslleniousReceiptDTO>));
                            serialiser.Serialize(sw, aPIMRRequestDTO, emptyNamepsaces);

                            xmlString = sw.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", " ");


                        }
                        finalresult = oshipperIDIntergrationBL.MiscReceipt(xmlString, authtoken);


                        string json = finalresult;
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }

                }

                else
                {
                    return null;
                }
            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return myCleanJsonObject;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "001");
                return null;
            }
        }


        [HttpPost]
        [Route("MiscIssue")]
        public object MiscIssue(WMSCoreMessage oRequest)
        {

            try
            {
                LoadControllerDefaults(oRequest);

                ShipperIDIntergrationBL oshipperIDIntergrationBL = new ShipperIDIntergrationBL(LoggedInUserID, ConnectionString);


                if (oRequest != null)
                {
                    List<MisslleniousIssueDTO> aPIMRRequestDTO = (List<MisslleniousIssueDTO>)WMSCoreEndpointSecurity.ValidateRequest(oRequest);


                    string authtoken = oRequest.AuthToken.AuthKey;

                    List<MisslleniousIssueDTO> aPISOCreations = new List<MisslleniousIssueDTO>();
                    MisslleniousIssueDTO aPISOCreation = new MisslleniousIssueDTO();

                    bool result = oshipperIDIntergrationBL.CheckAUthToken(authtoken);


                    if (result == true)
                    {

                        //APISOCreateRequestDTO aPISOCreateRequestDTO = new APISOCreateRequestDTO();
                        //  List<MisslleniousReceiptDTO> _lstAPISORequestDTO = aPIMRRequestDTO;

                        string finalresult = "";
                        var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

                        emptyNamepsaces.Add("", "");
                        var settings = new XmlWriterSettings();
                        string xmlString = null;
                        using (StringWriter sw = new StringWriter())
                        {
                            XmlSerializer serialiser = new XmlSerializer(typeof(List<MisslleniousIssueDTO>));
                            serialiser.Serialize(sw, aPIMRRequestDTO, emptyNamepsaces);

                            xmlString = sw.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", " ");


                        }
                        finalresult = oshipperIDIntergrationBL.MiscIssue(xmlString, authtoken);


                        string json = finalresult;
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Inventory, "Response : Invalid Auth Key."));
                        var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                        return myCleanJsonObject;
                    }

                }

                else
                {
                    return null;
                }
            }
            catch (WMSExceptionMessage ex)
            {
                List<WMSExceptionMessage> _lstwMSExceptionMessage = new List<WMSExceptionMessage>();
                _lstwMSExceptionMessage.Add(ex);
                string json = JsonConvert.SerializeObject(WMSCoreEndpointSecurity.PrepareResponse(oRequest.AuthToken, Constants.EndpointConstants.DTO.Exception, _lstwMSExceptionMessage));
                var myCleanJsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                return myCleanJsonObject;


            }
            catch (Exception excp)
            {
                ExceptionHandling.LogException(excp, _ClassCode + "001");
                return null;
            }
        }

    }
}