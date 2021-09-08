using MRLWMSC21Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

//  ====================== Created by M.D.Prasad =========================//

namespace MRLWMSC21.mReports.BL
{
    public class ChartsBL
    {
        public MRLWMSC21.mReports.Models.ChartsModel.TotalWHData GET_AllInventoryBinsData()
        {
            MRLWMSC21.mReports.Models.ChartsModel.TotalWHData lstBinsData = new Models.ChartsModel.TotalWHData();
            string query = "EXEC [dbo].[USP_GenerateKPIDashboards]";
            DataSet ds = DB.GetDS(query, false);
            if (ds!=null && ds.Tables!=null)
            {
                if (ds.Tables[1].Rows.Count != 0)
                {
                    MRLWMSC21.mReports.Models.ChartsModel.TotalBins TBinData = new Models.ChartsModel.TotalBins()
                       {
                           TotalBinLocations = ds.Tables[1].Rows[0]["TotalBinLocations"].ToString(),
                           NoOfOccupiedLocations = ds.Tables[1].Rows[0]["NoOfOccupiedLocations"].ToString(),
                           NoOfEmptyLocations = ds.Tables[1].Rows[0]["NoOfEmptyLocations"].ToString(),

                           TotalWarehouseBinVolume = ds.Tables[1].Rows[0]["TotalWarehouseBinVolume"].ToString(),
                           InventoryVolumeOccupied = ds.Tables[1].Rows[0]["InventoryVolumeOccupied"].ToString(),
                           EmptyVolume = ds.Tables[1].Rows[0]["EmptyVolume"].ToString(),
                           UtilByVolumePercent = ds.Tables[1].Rows[0]["UtilByVolumePercent"].ToString(),

                           TotalWHCapacityByWeight = ds.Tables[1].Rows[0]["TotalWHCapacityByWeight"].ToString(),
                           WightOfAvailableStock = ds.Tables[1].Rows[0]["WightOfAvailableStock"].ToString(),
                           EmptyCapacityByWeight = ds.Tables[1].Rows[0]["EmptyCapacityByWeight"].ToString(),
                           UtilByWeight = ds.Tables[1].Rows[0]["UtilByWeight"].ToString()
                       };
                    lstBinsData.objTotalBins = TBinData;
                }

                if (ds.Tables[2].Rows.Count != 0)
                {
                    MRLWMSC21.mReports.Models.ChartsModel.TotalInwardReceipts TReceiptData = new Models.ChartsModel.TotalInwardReceipts()
                    {
                        TotalInward = ds.Tables[2].Rows[0]["TotalInward"].ToString(),
                        WorkItemsCompleted = ds.Tables[2].Rows[0]["WorkItemsCompleted"].ToString(),
                        ReceiptsToDo = ds.Tables[2].Rows[0]["ReceiptsToDo"].ToString()
                    };
                    lstBinsData.objTotalInwardReceipts = TReceiptData;
                }

                if (ds.Tables[3].Rows.Count != 0)
                {
                    MRLWMSC21.mReports.Models.ChartsModel.TotalInwardPutaways TPutawayData = new Models.ChartsModel.TotalInwardPutaways()
                    {
                        TotalInwardWorkLines = ds.Tables[3].Rows[0]["TotalInwardWorkLines"].ToString(),
                        WorkItemsCompleted = ds.Tables[3].Rows[0]["WorkItemsCompleted"].ToString(),
                        WorkItemsPanding = ds.Tables[3].Rows[0]["WorkItemsPanding"].ToString()
                    };
                    lstBinsData.objTotalInwardPutaways = TPutawayData;
                }

                if (ds.Tables[4].Rows.Count != 0)
                {
                    MRLWMSC21.mReports.Models.ChartsModel.TotalInwardPicking TPickingData = new Models.ChartsModel.TotalInwardPicking()
                    {
                        PickingLines = ds.Tables[4].Rows[0]["PickingLines"].ToString(),
                        CompletedLines = ds.Tables[4].Rows[0]["CompletedLines"].ToString(),
                        PendingLines = ds.Tables[4].Rows[0]["PendingLines"].ToString()
                    };
                    lstBinsData.objTotalInwardPicking = TPickingData;
                }


                if (ds.Tables[5].Rows.Count != 0)
                {
                    MRLWMSC21.mReports.Models.ChartsModel.TotalInboundData TInboundData = new Models.ChartsModel.TotalInboundData()
                    {
                        TimeTakenForCompletion = ds.Tables[5].Rows[0]["TimeTakenForCompletion"].ToString(),
                        QuantityReceived = ds.Tables[5].Rows[0]["QuantityReceived"].ToString(),
                        NormalizedTime = ds.Tables[5].Rows[0]["NormalizedTime"].ToString(),
                        SOQtyNormalizationFactor = ds.Tables[5].Rows[0]["SOQtyNormalizationFactor"].ToString()
                    };
                    lstBinsData.objTotalInboundData = TInboundData;
                }

                if (ds.Tables[6].Rows.Count != 0)
                {
                    MRLWMSC21.mReports.Models.ChartsModel.TotalOutBoundData TOutboundData = new Models.ChartsModel.TotalOutBoundData()
                    {
                        TimeTakenForCompletion = ds.Tables[6].Rows[0]["TimeTakenForCompletion"].ToString(),
                        QuantitySold = ds.Tables[6].Rows[0]["QuantitySold"].ToString(),
                        NormalizedTime = ds.Tables[6].Rows[0]["NormalizedTime"].ToString(),
                        SOQtyNormalizationFactor = ds.Tables[6].Rows[0]["SOQtyNormalizationFactor"].ToString()
                    };
                    lstBinsData.objTotalOutBoundData = TOutboundData;
                }

                //if (ds.Tables[7].Rows.Count != 0)
                //{
                //    MRLWMSC21.mReports.Models.ChartsModel.TotalBays TBaysData = new Models.ChartsModel.TotalBays()
                //    {
                //        TotalLoadingBays = ds.Tables[7].Rows[0]["TotalLoadingBays"].ToString(),
                //        BaysCurrentlyUsed = ds.Tables[7].Rows[0]["BaysCurrentlyUsed"].ToString()
                //    };
                //    lstBinsData.objTotalBays = TBaysData;
                //}

                if (ds.Tables[7].Rows.Count != 0)
                {
                    MRLWMSC21.mReports.Models.ChartsModel.InventoryAgeing TInventoryData = new Models.ChartsModel.InventoryAgeing()
                    {
                        VolumeAgeGT6Months = ds.Tables[7].Rows[0]["VolumeAgeGT6Months"].ToString(),
                        VolumeAgeGT12Months = ds.Tables[7].Rows[0]["VolumeAgeGT12Months"].ToString(),
                        VolumeAgeGT24Months = ds.Tables[7].Rows[0]["VolumeAgeGT24Months"].ToString(),
                        NoSKUsGT6Months = ds.Tables[7].Rows[0]["NoSKUsGT6Months"].ToString(),
                        NoSKUsGT12Months = ds.Tables[7].Rows[0]["NoSKUsGT12Months"].ToString(),
                        NoSKUsGT24Months = ds.Tables[7].Rows[0]["NoSKUsGT24Months"].ToString()
                    };
                    lstBinsData.objInventoryAgeing = TInventoryData;
                }
                List<MRLWMSC21.mReports.Models.ChartsModel.ItemLinesAgeing> oItemLinesAgeinglist = new List<Models.ChartsModel.ItemLinesAgeing>();
                if (ds.Tables[8].Rows.Count != 0)
                {
                    foreach (DataRow row in ds.Tables[8].Rows)
                    {
                       oItemLinesAgeinglist.Add(new Models.ChartsModel.ItemLinesAgeing()
                        {
                            DimCode = row["DimCode"].ToString(),
                            DimName = row["DimName"].ToString(),
                            MonthID = row["MonthID"].ToString(),
                            MonthName = row["MonthName"].ToString(),
                            YearID = row["YearID"].ToString(),
                            MovementType = row["MovementType"].ToString(),
                            GoodsMovementType = row["GoodsMovementType"].ToString(),
                            Quantity = row["Quantity"].ToString(),
                            Lines = row["Lines"].ToString()
                        });
                       
                    }                    
                }
                lstBinsData.objItemLinesAgeing = oItemLinesAgeinglist;
            }
            return lstBinsData;
        }

        //public List<MRLWMSC21.mReports.Models.ChartsModel.TotalInwardReceipts> GETInwardData()
        //{
        //    List<MRLWMSC21.mReports.Models.ChartsModel.TotalInwardReceipts> lstInwardData = new List<Models.ChartsModel.TotalInwardReceipts>();
        //    string query = "EXEC [dbo].[USP_GenerateKPIDashboards]";
        //    DataSet ds = DB.GetDS(query, false);
           
        //    if (ds.Tables[2].Rows.Count != 0)
        //    {
        //        foreach (DataRow row in ds.Tables[2].Rows)
        //        {
        //            MRLWMSC21.mReports.Models.ChartsModel.TotalInwardReceipts IData = new Models.ChartsModel.TotalInwardReceipts();
        //            IData.TotalInward = row["TotalInward"].ToString();
        //            IData.WorkItemsCompleted = row["WorkItemsCompleted"].ToString();
        //            IData.ReceiptsToDo = row["ReceiptsToDo"].ToString();
        //            lstInwardData.Add(IData);
        //        }
        //    }

        //    return lstInwardData;
        //}


        //======================== New Charts ==============================//

        public MRLWMSC21.mReports.Models.ChartsModel.TotalOperatorEfficiency GET_AllInbounData()
        {
            MRLWMSC21.mReports.Models.ChartsModel.TotalOperatorEfficiency lstEfficiencyData = new Models.ChartsModel.TotalOperatorEfficiency();
            string query = "EXEC [dbo].[GEN_OperationEfficiency]";
            DataSet ds = DB.GetDS(query, false);
            if (ds != null && ds.Tables != null)
            {
                List<MRLWMSC21.mReports.Models.ChartsModel.TotalUser> TUData = new List<Models.ChartsModel.TotalUser>();
                if (ds.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        TUData.Add(new Models.ChartsModel.TotalUser()
                        {
                            Totalusers = row["Totalusers"].ToString(),
                            Month = row["Month"].ToString(),
                            Year = row["Year"].ToString(),

                        });
                    }
                }
                lstEfficiencyData.objTotalUsers = TUData;


                List<MRLWMSC21.mReports.Models.ChartsModel.TotalInbound> TINData = new List<Models.ChartsModel.TotalInbound>();
                if (ds.Tables[1].Rows.Count != 0)
                {
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        TINData.Add(new Models.ChartsModel.TotalInbound()
                        {
                            TotalInbounds = row["TotalInbounds"].ToString(),
                            Month = row["Month"].ToString(),
                            Year = row["Year"].ToString()

                        });
                    }
                }
                lstEfficiencyData.objTotalInbds = TINData;

                List<MRLWMSC21.mReports.Models.ChartsModel.TotalLineNo> TLData = new List<Models.ChartsModel.TotalLineNo>();
                if (ds.Tables[2].Rows.Count != 0)
                {
                    foreach (DataRow row in ds.Tables[2].Rows)
                    {
                        TLData.Add(new Models.ChartsModel.TotalLineNo()
                        {
                            TotalLineNO = row["TotalLineNO"].ToString(),
                            Month = row["Month"].ToString(),
                            Year = row["Year"].ToString()

                        });
                    }
                }
                lstEfficiencyData.objTotalLineno = TLData;


                List<MRLWMSC21.mReports.Models.ChartsModel.TotalInbQty> TInbQty = new List<Models.ChartsModel.TotalInbQty>();
                if (ds.Tables[3].Rows.Count != 0)
                {
                    foreach (DataRow row in ds.Tables[3].Rows)
                    {
                        TInbQty.Add(new Models.ChartsModel.TotalInbQty()
                        {
                            Qty = row["Qty"].ToString(),
                            Month = row["Month"].ToString(),
                            Year = row["Year"].ToString()

                        });
                    }
                }
                lstEfficiencyData.objTotalInbQty = TInbQty;


                List<MRLWMSC21.mReports.Models.ChartsModel.TotalUserOBD> TOBDUserData = new List<Models.ChartsModel.TotalUserOBD>();
                if (ds.Tables[4].Rows.Count != 0)
                {
                    foreach (DataRow row in ds.Tables[4].Rows)
                    {
                        TOBDUserData.Add(new Models.ChartsModel.TotalUserOBD()
                        {
                            Totalusers = row["Totalusers"].ToString(),
                            Month = row["Month"].ToString(),
                            Year = row["Year"].ToString()

                        });
                    }
                }
                lstEfficiencyData.objTotalUsersObd = TOBDUserData;

                List<MRLWMSC21.mReports.Models.ChartsModel.TotalOutbound> TObdData = new List<Models.ChartsModel.TotalOutbound>();
                if (ds.Tables[5].Rows.Count != 0)
                {
                    foreach (DataRow row in ds.Tables[5].Rows)
                    {
                        TObdData.Add(new Models.ChartsModel.TotalOutbound()
                        {
                            TotalOutbounds = row["TotalOutbounds"].ToString(),
                            Month = row["Month"].ToString(),
                            Year = row["Year"].ToString()

                        });
                    }
                }
                lstEfficiencyData.objTotalObbds = TObdData;

                List<MRLWMSC21.mReports.Models.ChartsModel.TotalLineNoOBD> TObdLData = new List<Models.ChartsModel.TotalLineNoOBD>();
                if (ds.Tables[6].Rows.Count != 0)
                {
                    foreach (DataRow row in ds.Tables[6].Rows)
                    {
                        TObdLData.Add(new Models.ChartsModel.TotalLineNoOBD()
                        {
                            TotalLineNO = row["TotalLineNO"].ToString(),
                            Month = row["Month"].ToString(),
                            Year = row["Year"].ToString()

                        });
                    }
                }
                lstEfficiencyData.objTotalLinenoObd = TObdLData;

                List<MRLWMSC21.mReports.Models.ChartsModel.TotalObdQty> TObdQty = new List<Models.ChartsModel.TotalObdQty>();
                if (ds.Tables[7].Rows.Count != 0)
                {
                    foreach (DataRow row in ds.Tables[7].Rows)
                    {
                        TObdQty.Add(new Models.ChartsModel.TotalObdQty()
                        {
                            Qty = row["Qty"].ToString(),
                            Month = row["Month"].ToString(),
                            Year = row["Year"].ToString()

                        });
                    }
                }
                lstEfficiencyData.objTotalObdQty = TObdQty; 

                //if (ds.Tables[0].Rows.Count != 0)
                //{
                //    MRLWMSC21.mReports.Models.ChartsModel.TotalUser TUData = new Models.ChartsModel.TotalUser()
                //    {
                //        Totalusers = ds.Tables[0].Rows[0]["Totalusers"].ToString(),
                //        Month = ds.Tables[0].Rows[0]["Month"].ToString(),
                //        Year = ds.Tables[0].Rows[0]["Year"].ToString(),

                      
                //    };
                //    lstEfficiencyData.objTotalUsers = TUData;
                //}

                //if (ds.Tables[1].Rows.Count != 0)
                //{
                //    MRLWMSC21.mReports.Models.ChartsModel.TotalInbound TINData = new Models.ChartsModel.TotalInbound()
                //    {
                //        TotalInbounds = ds.Tables[1].Rows[0]["TotalInbounds"].ToString(),
                //        Month = ds.Tables[1].Rows[0]["Month"].ToString(),
                //        Year = ds.Tables[1].Rows[0]["Year"].ToString()
                //    };
                //    lstEfficiencyData.objTotalInbds = TINData;
                //}

                //if (ds.Tables[2].Rows.Count != 0)
                //{
                //    MRLWMSC21.mReports.Models.ChartsModel.TotalLineNo TLData = new Models.ChartsModel.TotalLineNo()
                //    {
                //        TotalLineNO = ds.Tables[2].Rows[0]["TotalLineNO"].ToString(),
                //        Month = ds.Tables[2].Rows[0]["Month"].ToString(),
                //        Year = ds.Tables[2].Rows[0]["Year"].ToString()
                //    };
                //    lstEfficiencyData.objTotalLineno = TLData;
                //}                
            }
            return lstEfficiencyData;
        }
    }
}