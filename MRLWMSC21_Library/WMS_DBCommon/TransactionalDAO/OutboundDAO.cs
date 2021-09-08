using MRLWMSC21Common;
using MRLWMSC21_Library.WMS_ServiceObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21_Library.WMS_DBCommon.TransactionalDAO
{

    internal class OutboundDAO : DBServiceController
    {
        delegate Object OutboundDataAccess(Object input, int DB_ACTION);

        public object processDBCall(string DB_CALL_CODE, object input, int IN_MSG_TYPE)
        {
            try
            {
                if (DBServicePool.DBServiceImplementRef.ContainsKey(DB_CALL_CODE.Trim()) || DBServicePool.DBServiceImplementRef[DB_CALL_CODE].Trim() != "")
                {
                    MethodInfo methodInfo = typeof(OutboundDAO).GetMethod(DBServicePool.DBServiceImplementRef[DB_CALL_CODE]);
                    OutboundDataAccess dataAccess = (OutboundDataAccess)Delegate.CreateDelegate(typeof(OutboundDataAccess), null, methodInfo);
                    return dataAccess(input, IN_MSG_TYPE);
                }
                else
                {
                    throw new NotImplementedException("the requested DB execution call not implemented under InvoiceDAO. [" + DB_CALL_CODE + "]");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public object GetDeliveryPickInfoByOutboundId(object input, int IN_MSG_TYPE)
        {
            //getting data through procedure call
            AvailableStockData pickableItems = null;
            if (input != null && IN_MSG_TYPE == 2)
            {
                IDictionary<string, object> criteria = (IDictionary<string, object>)input;
                int OutboundId = Convert.ToInt32(criteria["OUTBOUNDID"].ToString());
                string drlStatement = "[dbo].[SP_SUGGESTED_PICKING_DATA_FOR_PICKNOTE] @OutboundID=" + Convert.ToInt32(criteria["OUTBOUNDID"].ToString()) + ",@MCode='" + criteria["MATERIALCODE"].ToString() + "'";

                DataSet StockInfo = DB.GetDS(drlStatement, false);
                if (StockInfo.Tables.Count == 3)
                {
                    pickableItems = new AvailableStockData();
                    DataTable orderLineTable = StockInfo.Tables[0];
                    DataTable stockInfoTable = StockInfo.Tables[1];
                    DataTable mspTable = StockInfo.Tables[2];

                    pickableItems.orders = new Dictionary<int, SalesOrder>();

                    foreach (DataRow row in orderLineTable.Rows)
                    {

                        int SOId = Convert.ToInt32(row["SOHeaderID"]);

                        LineItem line = new LineItem();
                        line.ConversionFactor = Convert.ToDecimal(row["ConvesionValue"]);
                        line.SO_QTY = Convert.ToDecimal(row["SOQuantity"]);

                        line.SO_UOM = row["IUoM"].ToString();
                        line.SO_UOM_QTY = Convert.ToDecimal(row["IUoMQty"]);

                        line.SO_UOMID = Convert.ToInt32(row["MaterialMaster_SUoMID"]);
                        line.Base_UOM = row["BUoM"].ToString();
                        line.LineNo = row["LineNumber"].ToString();
                        line.LineSeqId = Convert.ToInt32(row["LineNumber"]);

                        line.MaterialCode = row["MCode"].ToString();
                        line.MaterialID = Convert.ToInt32(row["MaterialMasterID"].ToString());
                        line.MTypeId = Convert.ToInt16(row["MTypeID"].ToString());
                        line.MDesc = row["MDescription"].ToString();

                        line.KitPlannerId = Convert.ToInt32(row["KitPlannerID"].ToString());
                        line.QTY = Convert.ToDecimal(row["SOQuantity"]);
                        line.SODetailsID = Convert.ToInt32(row["SODetailsID"].ToString());
                        line.WarehouseId = Convert.ToInt32(row["WarehouseID"].ToString());

                        int SoDetailId = Convert.ToInt32(row["SODetailsID"].ToString());


                        if (row["MSPValues"] != null && row["MSPValues"].ToString() != "")
                        {
                            line.MSPS = strToMSPDictionary(row["MSPValues"].ToString());
                            line.IsMSPRequest = true;
                        }


                        decimal processedQty = 0.0m;
                        decimal processedDocQty = 0.0m;
                        line.AvailStock = new List<MaterialStockInfo>();

                        foreach (DataRow stockRow in stockInfoTable.Rows)
                        {
                            if ((SoDetailId != Convert.ToInt32(stockRow["SODetailsID"])) || (line.WarehouseId != Convert.ToInt32(stockRow["WarehouseID"])))
                            {
                                continue;
                            }
                            else
                            {
                                MaterialStockInfo stockInfo = new MaterialStockInfo();
                                processedQty += Convert.ToDecimal(stockRow["PickedQty"]);
                                stockInfo.UOMId = Convert.ToInt32(stockRow["MaterialMaster_IUoMID"]);

                                if (line.SO_UOMID == stockInfo.UOMId)
                                    processedDocQty += Convert.ToDecimal(stockRow["PickedQty"]);
                                else
                                    processedDocQty += Convert.ToDecimal(stockRow["PickedQty"]) * line.ConversionFactor;

                                stockInfo.BatchNo = stockRow["Batch"].ToString();
                               // stockInfo.InvoiceSysLineId = Convert.ToInt32(stockRow["POSODetailsID"].ToString());
                                stockInfo.IsDamaged = Convert.ToInt16(stockRow["IsDamaged"]) == 1;
                                stockInfo.HasDiscrepancy = Convert.ToInt16(stockRow["HasDiscrepancy"]) == 1;
                                stockInfo.InitiatedQTY = Convert.ToDecimal(stockRow["PickedQty"]);
                                stockInfo.MaterialId = Convert.ToInt32(stockRow["MaterialMasterID"]);
                                stockInfo.LocationSysId = Convert.ToInt32(stockRow["LocationID"]);
                                stockInfo.KitPlannerId = Convert.ToInt32(stockRow["KitPlannerID"]);

                                stockInfo.LocationCode = stockRow["Location"].ToString();
                                stockInfo.QTY = Convert.ToDecimal(stockRow["Quantity"]);
                                stockInfo.LocationGenId = GenerateLocationSerialNo(stockInfo.LocationCode);

                                //Good MSP values
                                if (stockRow["MSPValues"] != null && stockRow["MSPValues"].ToString() != "")
                                {
                                    stockInfo.MSPS = strToMSPList(stockRow["MSPValues"].ToString());
                                    bool foundExpiry = false;
                                    foreach (MSP mspChecker in stockInfo.MSPS)
                                    {
                                        if (mspChecker.MSPSysId == 2)
                                        {
                                            foundExpiry = true;
                                            stockInfo.dummyExpiry = ((DateTime)mspChecker.Value).Date;
                                        }
                                    }
                                    if (!foundExpiry)
                                        stockInfo.dummyExpiry = DateTime.Now.Date.AddDays(180);
                                }
                                else 
                                {
                                    stockInfo.dummyExpiry = DateTime.Now.Date.AddDays(180);
                                }
                                
                                line.AvailStock.Add(stockInfo);
                            }

                        }
                        line.processedDocQty = processedDocQty;
                        line.processedQty = processedQty;

                        if (!pickableItems.orders.ContainsKey(SOId))
                        {
                            SalesOrder order = new SalesOrder();
                            order.SOId = SOId;
                            order.SONumber = row["SONumber"].ToString();
                            order.SOSysId = Convert.ToInt32(row["SOHeaderID"]);
                            order.WarehouseId = Convert.ToInt16(row["WarehouseID"]);
                            order.OutboundId = OutboundId;

                            order.SOLineItems = new List<LineItem>();
                            order.SOLineItems.Add(line);
                            pickableItems.orders.Add(SOId, order);
                        }
                        else
                        {
                            pickableItems.orders[SOId].SOLineItems.Add(line);
                        }
                    }

                    pickableItems.MSP = new Dictionary<int, MSP>();
                    foreach (DataRow row in mspTable.Rows)
                    {
                        pickableItems.MSP.Add(Convert.ToInt16(row["MaterialStorageParameterID"]), new MSP()
                        {
                            MSPSysId = Convert.ToInt16(row["MaterialStorageParameterID"]),
                            DisplayName = row["DisplayName"].ToString()
                        });
                    }
                }
                else
                {
                    throw new Exception("required information is not initialized yet");
                }
            }
            return pickableItems;
        }



        private IList<MSP> strToMSPList(string inputStr)
        {
            IList<MSP> msps = new List<MSP>();
            foreach (string str in inputStr.Split(','))
            {
                if (str.Length > 1)
                {
                    MSP msp = new MSP();
                    msp.NativeValue = str.Split('|')[1];
                    msp.Value = str.Split('|')[1].Trim();
                    msp.MSPSysId = Convert.ToInt16(str.Split('|')[0]);
                    if (msp.MSPSysId == 1 || msp.MSPSysId == 2)
                    {
                        msp.Value = DateTime.ParseExact(msp.NativeValue.Trim(), "dd/MM/yyyy", null);
                        //msp.Value = Convert.ToDateTime(msp.NativeValue.Trim());
                        //msp.Value = DateTime.Parse(msp.Value.ToString());
                    }

                    msps.Add(msp);
                }
            }
            return msps;
        }

        private IDictionary<int, MSP> strToMSPDictionary(string inputStr)
        {
            IDictionary<int, MSP> msps = new Dictionary<int, MSP>();
            foreach (string str in inputStr.Split(','))
            {
                if (str.Length > 1)
                {
                    MSP msp = new MSP();
                    msp.NativeValue = str.Split('|')[1].Trim();
                    msp.Value = str.Split('|')[1].Trim();
                    msp.MSPSysId = Convert.ToInt16(str.Split('|')[0]);
                    if (msp.MSPSysId == 1 || msp.MSPSysId == 2)
                    {
                        msp.Value = DateTime.ParseExact(msp.Value.ToString(), "dd/MM/yyyy", null);
                        //msp.Value = DateTime.Parse(msp.Value.ToString());
                        //msp.Value = Convert.ToDateTime(msp.Value.ToString());
                    }

                    msps.Add(msp.MSPSysId, msp);
                }
            }
            return msps;
        }

        private int GenerateLocationSerialNo(string code)
        {
            //S1 01 D 01 
            if (code.Length == 7)
            {
                int zone = (int)Convert.ToChar(code.Substring(0, 1));
                int rack = Convert.ToInt16(code.Substring(2, 2));
                int bay = (int)Convert.ToChar(code.Substring(4, 1));
                int bin = Convert.ToInt32(code.Substring(5, 2));
                return (zone * 100000) + (rack * 1000) + (bay * 100) + (bin * 10);
            }
            return 0;
        }


    }
}
