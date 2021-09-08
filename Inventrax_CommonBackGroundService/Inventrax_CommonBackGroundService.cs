using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using MRLWMSC21Common;
using Inventrax_CommonBackGroundService.BL.EmailHandler;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace Inventrax_CommonBackGroundService
{
    public partial class Inventrax_CommonBackGroundService : ServiceBase
    {
    

        private Timer grnTimer = null;
        private Timer pgiTimer = null;



        private bool isGrnMailServiceInProgress = false;
        private bool isPGIMailServiceInprogress = false;
        private bool isOBDSummaryMailServiceInprogress = false;
        private bool isWarehouseSummaryMailServiceInprogress = false;
        private bool isStockConsolidationServiceInProgress = false;
        private bool isDaywiseClosingServiceInProgress = false;
        private bool isWebHooksServiceInProgress = false;


        public Inventrax_CommonBackGroundService()
        {
            InitializeComponent();


        }

        protected override void OnStart(string[] args)
        {

            try
            {
                LogWriter.WriteLog("On start");
                LogWriter.WriteLog("On Test");


                #region GRN EMail Service Section starts here 


                        LogWriter.WriteLog("On Test");
                        try
                        {
                            LogWriter.WriteLog("Try Block enter");
                            LogWriter.WriteLog(CommonLogic.Application("IsGRNEmailRequired"));
                   
                            if (CommonLogic.Application("IsGRNEmailRequired") == "1")
                            {
                                LogWriter.WriteLog("Try Block enter");

                                LogWriter.WriteLog("==============================================  GRN Email Service  Started    ==============================================");

                                grnTimer = new Timer();
                                grnTimer.Interval = Convert.ToInt32(ConfigurationSettings.AppSettings["TimeForGRNEmail"].ToString());
                                this.grnTimer.Elapsed += new System.Timers.ElapsedEventHandler(PushGRNEmail);
                                grnTimer.Enabled = true;
                            }
                            else
                            {
                                LogWriter.WriteLog("Else");
                            }
                        }
                        catch (Exception ex)
                        {
                            LogWriter.WriteLog("GRN EMail  Service not statrted");
                            ex.Source = "GRN EMail  Service not statrted";
                            MailUtility mail = new MailUtility();
                            mail.SendExceptionEmail(ex);
                        }
                #endregion 


                #region PGI EMail Service Section starts here 
                        try
                        {
                            if (CommonLogic.Application("IsPGIEmailRequired") == "1")
                            {

                                LogWriter.WriteLog("==============================================  PGI Email Service  Started    ==============================================");

                                pgiTimer = new Timer();
                                pgiTimer.Interval = Convert.ToInt32(ConfigurationSettings.AppSettings["TimeForPGIEmail"].ToString());
                                this.pgiTimer.Elapsed += new System.Timers.ElapsedEventHandler(PushPGIEmail);
                                pgiTimer.Enabled = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            LogWriter.WriteLog("PGI EMail  Service not statrted");
                            ex.Source = "PGI EMail  Service not statrted";
                            MailUtility mail = new MailUtility();
                            mail.SendExceptionEmail(ex);
                        }
                #endregion



                #region OBD SUMMARY REPORT EMail Service Section starts here 
                try
                {
                    if (CommonLogic.Application("IsOBDSummmaryRequired") == "1")
                    {

                        LogWriter.WriteLog("==============================================  OBD Summary Email Service  Started    ==============================================");

                        pgiTimer = new Timer();
                        pgiTimer.Interval = Convert.ToInt32(ConfigurationSettings.AppSettings["TimeForOBDSummary"].ToString());
                        this.pgiTimer.Elapsed += new System.Timers.ElapsedEventHandler(PushOBDSummaryEmail);
                        pgiTimer.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    LogWriter.WriteLog("OBD Summary EMail  Service not statrted");
                    ex.Source = "OBD Summary  Service not statrted";
                    MailUtility mail = new MailUtility();
                    mail.SendExceptionEmail(ex);
                }
                #endregion










                #region OBD SUMMARY REPORT EMail Service Section starts here 
                try
                {
                    if (CommonLogic.Application("IsWHSummaryReportRequired") == "1")
                    {

                        LogWriter.WriteLog("==============================================  Warehouse Summary Email Service  Started    ==============================================");

                        pgiTimer = new Timer();
                        pgiTimer.Interval = Convert.ToInt32(ConfigurationSettings.AppSettings["TimeForWHSummary"].ToString());
                        this.pgiTimer.Elapsed += new System.Timers.ElapsedEventHandler(PushWarehouseSummaryEmail);
                        pgiTimer.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    LogWriter.WriteLog("Warehouse Summary EMail  Service not statrted");
                    ex.Source = "Warehouse Summary  Service not statrted";
                    MailUtility mail = new MailUtility();
                    mail.SendExceptionEmail(ex);
                }
                #endregion




                #region  Stock Consolidation Service starts here
                        try
                        {
                            if (CommonLogic.Application("IsStockConsolidationRequired") == "1")
                            {

                                LogWriter.WriteLog("============================================== Stock Consolidation Service  Started    ==============================================");

                                pgiTimer = new Timer();
                                pgiTimer.Interval = Convert.ToInt32(ConfigurationSettings.AppSettings["TimeForStockConsolidation"].ToString());
                                this.pgiTimer.Elapsed += new System.Timers.ElapsedEventHandler(ConsolidateGoodsMovementData);
                                pgiTimer.Enabled = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            LogWriter.WriteLog("Stock Consolidation Service starts here");
                            ex.Source = "Stock Consolidation Service starts here";
                            MailUtility mail = new MailUtility();
                            mail.SendExceptionEmail(ex);
                        }
                #endregion



                #region  DayWise Closing Stock Service Starts Here
                try
                {
                    if (CommonLogic.Application("IsDayWiseClosingStockRequired") == "1")
                    {

                        LogWriter.WriteLog("============================================== DayWise Closing Stock Service  Started    ==============================================");

                        pgiTimer = new Timer();
                        pgiTimer.Interval = Convert.ToInt32(ConfigurationSettings.AppSettings["IsDayWiseClosingStockRequired"].ToString());
                        this.pgiTimer.Elapsed += new System.Timers.ElapsedEventHandler(ExecuteDaywiseClosingStock);
                        pgiTimer.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    LogWriter.WriteLog("DayWise Closing Stock Service starts here");
                    ex.Source = "DayWise Closing Stock  Service starts here";
                    MailUtility mail = new MailUtility();
                    mail.SendExceptionEmail(ex);
                }
                #endregion





                #region WebHooks Service Section Starts Here
                try
                {
                    if (CommonLogic.Application("isWebHooksServiceRequired") == "1")
                    {

                        LogWriter.WriteLog("==============================================  WebHooks API Servicer Started    ==============================================");

                        pgiTimer = new Timer();
                        pgiTimer.Interval = Convert.ToInt32(ConfigurationSettings.AppSettings["TimeForWebHooksService"].ToString());
                        this.pgiTimer.Elapsed += new System.Timers.ElapsedEventHandler(PostDataToWebHookAPI);
                        pgiTimer.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    LogWriter.WriteLog("Warehouse Summary EMail  Service not statrted");
                    ex.Source = "Warehouse Summary  Service not statrted";
                    MailUtility mail = new MailUtility();
                    mail.SendExceptionEmail(ex);
                }
                #endregion



            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(ex.Message);

            }
        }

        protected override void OnStop()
        {
        }




     


        /// <summary>
        /// 1. This service will responcible Sending GRN Notigifications
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private void PushGRNEmail(object obj, ElapsedEventArgs e)
        {
            if (!isGrnMailServiceInProgress)
            {
                try
                {
                    LogWriter.WriteLog("New Request");
                    isGrnMailServiceInProgress = true;
                   ICommonService service = new GRNEmailService();
                    service.Execute();
                    isGrnMailServiceInProgress = false;

                }
                catch (Exception ex)
                {
                    isGrnMailServiceInProgress = false;
                    LogWriter.WriteLog("Exception PushGRNEmail " + ex.Message);

                }
            }
            else
            {
                LogWriter.WriteLog(" PushGRNEmail Previous request in progress");
            }
          

        }


        /// <summary>
        /// 2.  This service will responcible Sending PGI Notigifications
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private void PushPGIEmail(object obj, ElapsedEventArgs e)
        {



           
                if (!isPGIMailServiceInprogress)
                {
                    try
                    {

                        isPGIMailServiceInprogress = true;
                        ICommonService service = new PGIEmailService();
                        service.Execute();
                        isPGIMailServiceInprogress = false;

                    }
                    catch (Exception ex)
                    {
                        isPGIMailServiceInprogress = false;
                        LogWriter.WriteLog("Exception PushPGIEmail " + ex.Message);

                    }
                }
                else
                {
                    LogWriter.WriteLog(" PushPGIEmail Previous request in progress");
                }

   


        }



        /// <summary>
        /// 3.  This service will responcible Sending  OBD Summary Notigifications
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private void PushOBDSummaryEmail(object obj, ElapsedEventArgs e)
        {
              if (!isOBDSummaryMailServiceInprogress)
                {
                    try
                    {

                        isOBDSummaryMailServiceInprogress = true;
                        ICommonService service = new OBDSummaryEmailService();
                        service.Execute();
                        isOBDSummaryMailServiceInprogress = false;

                    }
                    catch (Exception ex)
                    {
                         isOBDSummaryMailServiceInprogress = false;
                        LogWriter.WriteLog("Exception PushOBDSummaryEmail " + ex.Message);

                    }
                }
                else
                {
                    LogWriter.WriteLog(" PushOBDSummaryEmail Previous request in progress");
                }



    }





        /// <summary>
        /// 4.  This service will responcible Sending  Warehouse Summary Notigifications
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private void PushWarehouseSummaryEmail(object obj, ElapsedEventArgs e)
        {
            if (!isWarehouseSummaryMailServiceInprogress)
            {
                try
                {

                    isWarehouseSummaryMailServiceInprogress = true;
                    ICommonService service = new WHStockEMailSerice();
                    service.Execute();
                    isWarehouseSummaryMailServiceInprogress = false;

                }
                catch (Exception ex)
                {
                    isWarehouseSummaryMailServiceInprogress = false;
                    LogWriter.WriteLog("Exception PushOBDSummaryEmail " + ex.Message);

                }
            }
            else
            {
                LogWriter.WriteLog(" PushOBDSummaryEmail Previous request in progress");
            }



        }



        /// <summary>
        /// 5.  This service will responcible Sending Consolidating Goodsmovement data into ActiveStockSetails
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private void ConsolidateGoodsMovementData(object obj, ElapsedEventArgs e)
        {
            if (!isStockConsolidationServiceInProgress)
            {
                try
                {

                    isStockConsolidationServiceInProgress = true;

                    LogWriter.WriteLog("ConsolidateGoodsMovementData executed at" + DateTime.Now.ToLongTimeString());
                    DB.ExecuteSQL("[dbo].[SP_INV_UpdateActiveStockDetails]");


                    isStockConsolidationServiceInProgress = false;

                }
                catch (Exception ex)
                {
                    isStockConsolidationServiceInProgress = false;
                    LogWriter.WriteLog("ConsolidateGoodsMovementData" + ex.Message);

                }
            }
            else
            {
                LogWriter.WriteLog(" ConsolidateGoodsMovementData");
            }



        }


        /// <summary>
        /// 5.  This service will responcible for Posting Daywise closing stock
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private void ExecuteDaywiseClosingStock(object obj, ElapsedEventArgs e)
        {
            if (!isDaywiseClosingServiceInProgress)
            {
                try
                {

                    isDaywiseClosingServiceInProgress = true;

                    LogWriter.WriteLog("ExecuteDaywiseClosingStock executed at" + DateTime.Now.ToLongTimeString());

                    DataSet dsResult= DB.GetDS("SELECT WarehouseID FROM GEN_Warehouse WHERE IsActive=1 AND IsDeleted=0", false);

                    if (dsResult != null && dsResult.Tables.Count != 0 && dsResult.Tables[0].Rows.Count != 0)
                    {
                        foreach (DataRow dr in dsResult.Tables[0].Rows)
                        {
                            string warehouseID = "0";
                            warehouseID = dr["WarehouseID"].ToString();

                            DB.ExecuteSQL("EXEC [dbo].[SP_INV_UpdateDayWiseClosingStock] @WarehouseID=" + warehouseID);

                        }

                    }

                    isDaywiseClosingServiceInProgress = false;

                }
                catch (Exception ex)
                {
                    isDaywiseClosingServiceInProgress = false;
                    LogWriter.WriteLog("ExecuteDaywiseClosingStock" + ex.Message);

                }
            }
            else
            {
                LogWriter.WriteLog(" ExecuteDaywiseClosingStock");
            }



        }

        /// <summary>
        /// 6.  This service will responcible Sending  WEBhooks API Call
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private void PostDataToWebHookAPI(object obj, ElapsedEventArgs e)
        {
            LogWriter.WriteLog("PostDataToWebHookAPI");
            string webHookID = "";
            string APIEndPoint = "";
            string APIType = "";
            if (!isWebHooksServiceInProgress)
            {
                try
                {
                    isWebHooksServiceInProgress = true;
                    string username = CommonLogic.Application("WebHookUserName");
                    string password = CommonLogic.Application("WebHookPassword");
                    string strQuery = "select  JsonObject,WebHookID,APIEndPoint,APIType from API_TRN_WebHook  where isnull(PostStatus,0)=0 and isnull(NoOfAttempts,0)<4";

                    DataSet dsResult = DB.GetDS(strQuery, false);

                    if (dsResult != null && dsResult.Tables.Count != 0 && dsResult.Tables[0].Rows.Count != 0)
                    {
                        foreach (DataRow dr in dsResult.Tables[0].Rows)
                        {

                            string jsonBody = dr["JsonObject"].ToString();
                            //jsonBody = "{\"Data\":" + jsonBody + "}";
                            webHookID = dr["WebHookID"].ToString();
                            APIEndPoint = dr["APIEndPoint"].ToString();
                            APIType = dr["APIType"].ToString();

                            //LogWriter.WriteLog(jsonBody);
                            //LogWriter.WriteLog(APIType);
                            //LogWriter.WriteLog(APIEndPoint);
                            //LogWriter.WriteLog(username);
                            //LogWriter.WriteLog(password);

                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(APIEndPoint);
                            request.Method = APIType;
                            string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));

                            request.Headers.Add("Authorization", "Basic " + svcCredentials);



                            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                            {

                                streamWriter.Write(jsonBody);
                                streamWriter.Flush();
                                streamWriter.Close();

                                var httpResponse = (HttpWebResponse)request.GetResponse();
                                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                                {
                                    var result = streamReader.ReadToEnd();
                                    BL.WebHooks.Root _oObject = (BL.WebHooks.Root)JsonConvert.DeserializeObject<BL.WebHooks.Root>(result);
                                    if (_oObject.metadata.status == "OK")
                                    {
                                        DB.ExecuteSQL("update  API_TRN_WebHook set PostStatus=1 where WebHookID=" + webHookID);
                                    }
                                    else
                                    {
                                        LogWriter.WriteLog(_oObject.ToString());
                                        DB.ExecuteSQL("update  API_TRN_WebHook set PostStatus=0,NoOfAttempts=isnull(NoOfAttempts,0)+1 where WebHookID=" + webHookID);
                                    }
                                }
                            }


                        }
                    }

                    isWebHooksServiceInProgress = false;
                }
                catch (Exception ex)
                {
                    DB.ExecuteSQL("update  API_TRN_WebHook set PostStatus=0,NoOfAttempts=isnull(NoOfAttempts,0)+1 where WebHookID=" + webHookID);
                    isWebHooksServiceInProgress = false;
                    LogWriter.WriteLog("Exception PostDataToWebHookAPI " + ex.Message);
                    MailUtility mail = new MailUtility();
                    mail.SendExceptionEmail(ex);
                }
            }
            else
            {
                LogWriter.WriteLog(" PostDataToWebHookAPI Previous request in progress");
            }



        }
    }
}
