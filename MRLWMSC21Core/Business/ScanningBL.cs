using System;
using System.Collections.Generic;
using MRLWMSC21Core.Entities;
using MRLWMSC21Core.Library;
using MRLWMSC21Core.DataAccess;
using MRLWMSC21Core.Business.Interfaces;
using System.Linq;
using System.Text;
using MRLWMSC21Core.DataAccess.Utilities;
using MRLWMSC21Core.Business;
using WMSCore.Interfaces;
using WMSCore_BusinessEntities.Entities;
using Newtonsoft.Json.Linq;
using WMSCore_DataAccess.DataAccess.SqlServerImpl;


namespace WMSCore.Business
{
    public class ScanningBL : BaseBL, iScanningBL
    {
        private string _ClassCode = "WMSCore_BL_0012_";


        private ScanningDAL _oScanningdDAL = null;

        public ScanningBL(int LoginUser, string ConnectionString) : base(LoginUser, ConnectionString)
        {
            _oScanningdDAL = new ScanningDAL(LoginUser, ConnectionString);

            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_BusinessLayer.ScanningBL);
        }

        public ScannedItem ValidateLocation(ScannedItem obj)
        {
            try
            {
                return _oScanningdDAL.ValidateLocation(obj);
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("obj", obj);

                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public ScannedItem ValidatePallet(ScannedItem obj)
        {
            try
            {
                return _oScanningdDAL.ValidatePallet(obj);
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("obj", obj);

                ExceptionHandling.LogException(excp, _ClassCode + "002", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }

        }

        public ScannedItem ValidateSKU(ScannedItem obj)
        {
            try
            {

                if (obj.ScanInput.Split('|').Length == 9)
                {
                    obj.SkuCode = obj.ScanInput.Split('|')[0];
                    obj.Batch = obj.ScanInput.Split('|')[1];
                    obj.SerialNumber = obj.ScanInput.Split('|')[2];
                    obj.MfgDate = obj.ScanInput.Split('|')[3];
                    obj.ExpDate = obj.ScanInput.Split('|')[4];
                    obj.PrjRef = obj.ScanInput.Split('|')[5];
                    obj.KitID = obj.ScanInput.Split('|')[6];
                    obj.Mrp = obj.ScanInput.Split('|')[7];
                    obj.LineNumber = obj.ScanInput.Split('|')[8];
                    obj.HUSize = 0;
                    obj.HUNo = 0;


                }
                else if (obj.ScanInput.Split('|').Length == 11)
                {
                    obj.SkuCode = obj.ScanInput.Split('|')[0];
                    obj.Batch = obj.ScanInput.Split('|')[1];
                    obj.SerialNumber = obj.ScanInput.Split('|')[2];
                    obj.MfgDate = obj.ScanInput.Split('|')[3];
                    obj.ExpDate = obj.ScanInput.Split('|')[4];
                    obj.PrjRef = obj.ScanInput.Split('|')[5];
                    obj.KitID = obj.ScanInput.Split('|')[6];
                    obj.Mrp = obj.ScanInput.Split('|')[7];
                    obj.LineNumber = obj.ScanInput.Split('|')[8];
                    obj.HUNo = Convert.ToInt32(obj.ScanInput.Split('|')[9]);
                    obj.HUSize = Convert.ToInt32(obj.ScanInput.Split('|')[10]);


                }
                else if (obj.ScanInput.Split('|').Length == 5)
                {
                    obj.SkuCode = obj.ScanInput.Split('|')[0];
                    obj.Batch = obj.ScanInput.Split('|')[1];
                    obj.SerialNumber = obj.ScanInput.Split('|')[2];
                    obj.KitID = obj.ScanInput.Split('|')[3];
                    obj.LineNumber = obj.ScanInput.Split('|')[4];
                    obj.MfgDate = string.Empty;
                    obj.ExpDate = string.Empty;
                    obj.PrjRef = string.Empty;
                    obj.Mrp = string.Empty;
                    obj.HUSize = 0;
                    obj.HUNo = 0;
                }
                else if (obj.ScanInput.Split('|').Length == 1)
                {
                    obj.SkuCode = obj.ScanInput.Split('|')[0];



                    obj.Batch = string.Empty;
                    obj.SerialNumber = string.Empty;
                    obj.MfgDate = string.Empty;
                    obj.ExpDate = string.Empty;
                    obj.PrjRef = string.Empty;
                    obj.KitID = string.Empty;
                    obj.Mrp = string.Empty;
                    obj.LineNumber = string.Empty;
                    obj.HUSize = 0;
                    obj.HUNo = 0;


                }
                else if (obj.InboundID != "0" && obj.InboundID != "")
                {
                    if (obj.ScanInput.Split('|').Length == 2)
                    {
                        obj.SkuCode = obj.ScanInput.Split('|')[0];
                        obj.SupplierInvoiceDetailsId = obj.ScanInput.Split('|')[1];

                        obj.Batch = string.Empty;
                        obj.SerialNumber = string.Empty;
                        obj.MfgDate = string.Empty;
                        obj.ExpDate = string.Empty;
                        obj.PrjRef = string.Empty;
                        obj.KitID = string.Empty;
                        obj.Mrp = string.Empty;
                        obj.LineNumber = string.Empty;
                        obj.HUSize = 0;
                        obj.HUNo = 0;

                    }
                    else
                    {
                        throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_DAL_SCAN_0003", WMSMessage = ErrorMessages.WMC_DAL_SCAN_0003, ShowAsError = true };
                    }
                }

                else
                {
                    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_DAL_SCAN_0003", WMSMessage = ErrorMessages.WMC_DAL_SCAN_0003, ShowAsError = true };
                }

                return _oScanningdDAL.ValidateItem(obj);


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("obj", obj);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }


        public ScannedItem ValidateSO(ScannedItem obj)
        {
            try
            {
                return _oScanningdDAL.ValidateSO(obj);
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("obj", obj);

                ExceptionHandling.LogException(excp, _ClassCode + "004", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }

        }


        public ScannedItem ValidateCarton(ScannedItem obj)
        {
            try
            {
                return _oScanningdDAL.ValidateCarton(obj);
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("obj", obj);

                ExceptionHandling.LogException(excp, _ClassCode + "004", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }

        }
    }
}
