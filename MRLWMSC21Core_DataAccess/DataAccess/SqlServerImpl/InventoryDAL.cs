using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRLWMSC21Core.Library;
using MRLWMSC21Core.Entities;
using System.Data.SqlClient;
using System.Data;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using MRLWMSC21Core.DataAccess.Interfaces;

namespace MRLWMSC21Core.DataAccess
{
    public class InventoryDAL : BaseDAL, Interfaces.IInventoryDAL
    {

        private string _ClassCode = "WMSCore_DA_003_";

        public InventoryDAL(int LoginUser, string ConnectionString) : base(LoginUser, ConnectionString)
        {

            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_DataLayer.InventoryDAL);
        }
        public Inventory TransferPallettoLocation(Inventory obj)
        {

            try
            {
                // Method block to Fetch the Entity List from Database.
                StringBuilder _sbSQL = new StringBuilder();
                List<SqlParameter> lSQLParams = new List<SqlParameter>();

                _sbSQL.Append("EXEC [dbo].[sp_INV_TransferPalletToBin]");

                if (obj.ContainerCode != null && obj.ContainerCode!=string.Empty)
                {
                    SqlParameter oParam = new SqlParameter("@CartonCode", SqlDbType.NVarChar, 100);
                    oParam.Value = obj.ContainerCode;
                    lSQLParams.Add(oParam);


                }


                if ( obj.TenantID !=0)
                {
                    SqlParameter oParam1 = new SqlParameter("@TenantID", SqlDbType.Int);
                    oParam1.Value = obj.TenantID;
                    lSQLParams.Add(oParam1);

                }


                if (obj.WarehouseID!=0)
                {
                    SqlParameter oParam1 = new SqlParameter("@WarehouseID", SqlDbType.Int);
                    oParam1.Value = obj.WarehouseID;
                    lSQLParams.Add(oParam1);

                }

                if (obj.@LocationCode != null && obj.@LocationCode != string.Empty)
                {

                    SqlParameter oParam2 = new SqlParameter("@LocationCode", SqlDbType.NVarChar, 100);
                    oParam2.Value = obj.LocationCode;
                    lSQLParams.Add(oParam2);

                }

                if (LoggedInUserID != 0)
                {

                    SqlParameter oParam2 = new SqlParameter("@CreatedBy", SqlDbType.NVarChar, 100);
                    oParam2.Value = LoggedInUserID;
                    lSQLParams.Add(oParam2);

                }


                DataSet _dsResults = base.FillDataSet(_sbSQL.ToString(), lSQLParams);
                if (_dsResults != null && _dsResults.Tables.Count != 0 && _dsResults.Tables[0].Rows.Count != 0)
                {
                   if(_dsResults.Tables[0].Rows[0][0].ToString()=="1")
                    {
                        return obj;
                    }else if (_dsResults.Tables[0].Rows[0][0].ToString() == "-111")
                    {
                        throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_DAL_INV_0001", WMSMessage = ErrorMessages.WMC_DAL_INV_0001, ShowAsError = true };
                    }
                    else if (_dsResults.Tables[0].Rows[0][0].ToString() == "-222")
                    {
                        throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_DAL_INV_0002", WMSMessage = ErrorMessages.WMC_DAL_INV_0002, ShowAsError = true };
                    }
                    else if (_dsResults.Tables[0].Rows[0][0].ToString() == "-333")
                    {
                        throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_DAL_INV_0003", WMSMessage = ErrorMessages.WMC_DAL_INV_0003, ShowAsError = true };
                    }
                    else if (_dsResults.Tables[0].Rows[0][0].ToString() == "-444")
                    {
                        throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_DAL_INV_0004", WMSMessage = ErrorMessages.WMC_DAL_INV_0004, ShowAsError = true };
                    }
                    else if (_dsResults.Tables[0].Rows[0][0].ToString() == "-1")
                    {
                        throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_DAL_INV_0005", WMSMessage = ErrorMessages.WMC_DAL_INV_0005, ShowAsError = true };

                    }
                    else if (_dsResults.Tables[0].Rows[0][0].ToString() == "-555")
                    {
                        throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_DAL_INV_0006", WMSMessage = ErrorMessages.WMC_DAL_INV_0006, ShowAsError = true };
                    }
                    else
                    {
                        throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_DAL_INV_0007", WMSMessage = ErrorMessages.WMC_DAL_INV_0007, ShowAsError = true };
                    }
                }
                else
                {
                    
                    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_DAL_INV_0005", WMSMessage = ErrorMessages.WMC_DAL_INV_0005, ShowAsError = true };
                }

                return obj;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("ScannedItem", obj);

                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }

        }

        public List<Inventory> GetSlocWiseActiveStock (Inventory obj)
        {
            try
            {
                List<Inventory> inventories = new List<Inventory>();

                StringBuilder _sbSQL = new StringBuilder();
                List<SqlParameter> lSQLParams = new List<SqlParameter>();

                _sbSQL.Append("EXEC [dbo].[USP_API_INV_Get_SLOCWiseActiveStockDetails_MaterialTransfer]");

                if (obj.ContainerCode != null && obj.ContainerCode != string.Empty)
                {
                    SqlParameter oParam = new SqlParameter("@CartonCode", SqlDbType.NVarChar, 100);
                    oParam.Value = obj.ContainerCode;
                    lSQLParams.Add(oParam);


                }
                
                if (obj.TenantID != 0)
                {
                    SqlParameter oParam1 = new SqlParameter("@TenantID", SqlDbType.Int);
                    oParam1.Value = obj.TenantID;
                    lSQLParams.Add(oParam1);

                }


                if (obj.WarehouseID != 0)
                {
                    SqlParameter oParam1 = new SqlParameter("@WarehouseID", SqlDbType.Int);
                    oParam1.Value = obj.WarehouseID;
                    lSQLParams.Add(oParam1);

                }

                if (obj.LocationCode != null && obj.LocationCode != string.Empty)
                {

                    SqlParameter oParam2 = new SqlParameter("@LocationCode", SqlDbType.NVarChar, 100);
                    oParam2.Value = obj.LocationCode;
                    lSQLParams.Add(oParam2);

                }

                if (obj.MaterialCode != null && obj.MaterialCode != string.Empty)
                {

                    SqlParameter oParam2 = new SqlParameter("@MCode", SqlDbType.NVarChar, 100);
                    oParam2.Value = obj.MaterialCode;
                    lSQLParams.Add(oParam2);

                }


                if (obj.BatchNumber != null && obj.BatchNumber != string.Empty)
                {

                    SqlParameter oParam2 = new SqlParameter("@BATCHNO", SqlDbType.NVarChar, 100);
                    oParam2.Value = obj.BatchNumber;
                    lSQLParams.Add(oParam2);

                }

                if (obj.Mfg_Date != null && obj.Mfg_Date.ToString() != string.Empty) 
                {

                    SqlParameter oParam2 = new SqlParameter("@MFGDate", SqlDbType.NVarChar, 100);
                    oParam2.Value = obj.Mfg_Date;
                    lSQLParams.Add(oParam2);

                }


                if (obj.ExpDate != null && obj.ExpDate.ToString() != string.Empty) 
                {

                    SqlParameter oParam2 = new SqlParameter("@EXPDate", SqlDbType.NVarChar, 100);
                    oParam2.Value = obj.ExpDate;
                    lSQLParams.Add(oParam2);

                }

                if (obj.SerialNo != null && obj.SerialNo != string.Empty) 
                {

                    SqlParameter oParam2 = new SqlParameter("@SerialNo", SqlDbType.NVarChar, 100);
                    oParam2.Value = obj.SerialNo;
                    lSQLParams.Add(oParam2);

                }

                if (obj.ProjectRefNo != null && obj.ProjectRefNo != string.Empty) 
                {

                    SqlParameter oParam2 = new SqlParameter("@ProjectRefNo", SqlDbType.NVarChar, 100);
                    oParam2.Value = obj.ProjectRefNo;
                    lSQLParams.Add(oParam2);

                }
                if (obj.MRP != 0 )
                {

                    SqlParameter oParam2 = new SqlParameter("@MRP", SqlDbType.NVarChar, 100);
                    oParam2.Value = obj.MRP;
                    lSQLParams.Add(oParam2);

                }


                DataSet _dsResults = base.FillDataSet(_sbSQL.ToString(), lSQLParams);


                if (_dsResults != null)
                {
                    if(_dsResults.Tables.Count > 0)
                    {
                        foreach(DataRow dr in _dsResults.Tables[0].Rows)
                        {
                            Inventory inventory = new Inventory()
                            {
                                StorageLocation = dr["Code"].ToString(),
                                AvailableQuantity= ConversionUtility.ConvertToDecimal(dr["AvlQty"].ToString())
                            };
                            inventories.Add(inventory);
                        }
                    }
                }

                return inventories;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("ScannedItem", obj);

                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public List<Inventory> UpdateMaterialTrasnfer(Inventory obj)
        {
            try
            {
                List<Inventory> inventories = new List<Inventory>();

                StringBuilder _sbSQL = new StringBuilder();
                List<SqlParameter> lSQLParams = new List<SqlParameter>();

                _sbSQL.Append("EXEC [dbo].[USP_API_INV_MaterialTransfer]");

                if (obj.ContainerCode != null && obj.ContainerCode != string.Empty)
                {
                    SqlParameter oParam = new SqlParameter("@CartonCode", SqlDbType.NVarChar, 100);
                    oParam.Value = obj.ContainerCode;
                    lSQLParams.Add(oParam);


                }

                if (obj.TenantID != 0)
                {
                    SqlParameter oParam1 = new SqlParameter("@Tenat_ID", SqlDbType.Int);
                    oParam1.Value = obj.TenantID;
                    lSQLParams.Add(oParam1);

                }


                if (obj.WarehouseID != 0)
                {
                    SqlParameter oParam1 = new SqlParameter("@WarehouseId", SqlDbType.Int);
                    oParam1.Value = obj.WarehouseID;
                    lSQLParams.Add(oParam1);

                }

                if (obj.LocationCode != null && obj.LocationCode != string.Empty)
                {

                    SqlParameter oParam2 = new SqlParameter("@LocationCode", SqlDbType.NVarChar, 100);
                    oParam2.Value = obj.LocationCode;
                    lSQLParams.Add(oParam2);

                }

                if (obj.MaterialCode != null && obj.MaterialCode != string.Empty)
                {

                    SqlParameter oParam2 = new SqlParameter("@Mcode", SqlDbType.NVarChar, 100);
                    oParam2.Value = obj.MaterialCode;
                    lSQLParams.Add(oParam2);

                }


                if (obj.BatchNumber != null && obj.BatchNumber != string.Empty)
                {

                    SqlParameter oParam2 = new SqlParameter("@BatchNo", SqlDbType.NVarChar, 100);
                    oParam2.Value = obj.BatchNumber;
                    lSQLParams.Add(oParam2);

                }

                if(obj.UserId != 0)
                {
                    SqlParameter oParam2 = new SqlParameter("@UserID", SqlDbType.Int);
                    oParam2.Value = obj.UserId;
                    lSQLParams.Add(oParam2);
                }

                if (obj.StorageLocation != null && obj.StorageLocation != string.Empty)
                {

                    SqlParameter oParam2 = new SqlParameter("@FromSL", SqlDbType.NVarChar, 100);
                    oParam2.Value = obj.StorageLocation;
                    lSQLParams.Add(oParam2);

                }

                if (obj.ToStorageLocation != null && obj.ToStorageLocation != string.Empty)
                {

                    SqlParameter oParam2 = new SqlParameter("@ToSL", SqlDbType.NVarChar, 100);
                    oParam2.Value = obj.ToStorageLocation;
                    lSQLParams.Add(oParam2);

                }

                if (obj.Quantity != 0)
                {
                    SqlParameter oParam2 = new SqlParameter("@Quantity", SqlDbType.Int);
                    oParam2.Value = obj.Quantity;
                    lSQLParams.Add(oParam2);
                }

                DataSet _dsResults = base.FillDataSet(_sbSQL.ToString(), lSQLParams);


                if (_dsResults != null)
                {
                    if (_dsResults.Tables.Count > 0)
                    {
                        foreach (DataRow dr in _dsResults.Tables[0].Rows)
                        {
                            Inventory inventory = new Inventory()
                            {
                                Result =ConversionUtility.ConvertToInt(dr["N"].ToString())
                               
                            };
                            inventories.Add(inventory);
                        }
                    }
                }

                return inventories;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("ScannedItem", obj);

                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public Inventory ReceiveInventory(Inventory oInventory)
        {
            try
            {
                List<Inventory> lstInventory = new List<Inventory>();
                lstInventory.Add(oInventory);
                ReceiveInventory(lstInventory);
                return lstInventory[0];

            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception ex)
            {

                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oInventory", oInventory);

                ExceptionHandling.LogException(ex, _ClassCode + "001", oExcpData);

                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public List<Inventory> ReceiveInventory(List<Inventory> lInventory)
        {
            //Method block to Fetch the Entity List from Database.
            try
            {
                var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

                emptyNamepsaces.Add("", "");
                var settings = new XmlWriterSettings();
                string xmlString = null;
                using (StringWriter sw = new StringWriter())
                {
                    XmlSerializer serialiser = new XmlSerializer(typeof(List<Inventory>));
                    serialiser.Serialize(sw, lInventory, emptyNamepsaces);

                    xmlString = sw.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", " ");
                    string strQuery = " EXEC [dbo].[sp_API_INV_ReciveItem] ";
                    strQuery += " @InventoryData=" + base.SQuote(xmlString);
                    strQuery += ",@CreatedBy=" + base.LoggedInUserID;
                    DataSet dsResult = base.FillDataSet(strQuery, new List<SqlParameter>());


                    if (dsResult == null)
                        throw new Exception("Move Item Procedure Returned NULL DataSet.");
                    else if (dsResult.Tables.Count == 0)
                        throw new Exception("Move Item Procedure returned 0 Data Tables.");



                    if (dsResult != null && dsResult.Tables.Count != 0 && dsResult.Tables[0].Rows.Count != 0)
                    {

                        lInventory[0].DockGoodsMovementID = Convert.ToInt32(dsResult.Tables[0].Rows[0]["DockGoodsMovementID"].ToString());
                        lInventory[0].ColorID = Convert.ToInt32(dsResult.Tables[0].Rows[0]["ColorAttributeValueID"].ToString());
                        lInventory[0].MRP = Convert.ToDecimal(dsResult.Tables[0].Rows[0]["MRP"].ToString());
                        lInventory[0].MOP = Convert.ToInt32(dsResult.Tables[0].Rows[0]["MOP"].ToString());
                        lInventory[0].IsReceived = true;
                        lInventory[0].IsFinishedGoods = ConversionUtility.ConvertToBool(dsResult.Tables[0].Rows[0]["IsFG"].ToString());
                        lInventory[0].IsRawMaterial = ConversionUtility.ConvertToBool(dsResult.Tables[0].Rows[0]["IsRM"].ToString());
                        lInventory[0].IsConsumables = ConversionUtility.ConvertToBool(dsResult.Tables[0].Rows[0]["IsConsummables"].ToString());
                        lInventory[0].Color = dsResult.Tables[0].Rows[0]["Color"].ToString();
                        lInventory[0].DocumentQuantity = ConversionUtility.ConvertToDecimal(dsResult.Tables[0].Rows[0]["DocumentQuantity"].ToString());
                        lInventory[0].DocumentProcessedQuantity = ConversionUtility.ConvertToDecimal(dsResult.Tables[0].Rows[0]["DocumentReceivedQty"].ToString());
                        lInventory[0].MaterialShortDescription = dsResult.Tables[0].Rows[0]["MDescription"].ToString();
                        return lInventory;
                    }
                    else
                    {
                        lInventory[0].IsReceived = false;
                        return lInventory;

                    }


                }


                //Method block to Fetch the Entity List from Database.
                //throw new NotImplementedException();

                return lInventory;

            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception ex)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("lInventory", lInventory);

                ExceptionHandling.LogException(ex, _ClassCode + "002", oExcpData);

                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public GoodsMovement MoveInventory(GoodsMovement oGoodsMovement)
        {
            try
            {
                //Method block to Fetch the Entity List from Database.

                List<GoodsMovement> _lGoodsMovement = new List<GoodsMovement>();

                _lGoodsMovement.Add(oGoodsMovement);

                _lGoodsMovement = MoveInventory(_lGoodsMovement);

                return _lGoodsMovement[0];


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception ex)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oGoodsMovement", oGoodsMovement);

                ExceptionHandling.LogException(ex, _ClassCode + "003", oExcpData);

                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public List<GoodsMovement> MoveInventory(List<GoodsMovement> lGoodsMovement)
        {
            try
            {
                //Method block to Fetch the Entity List from Database.

                if (lGoodsMovement == null)
                    throw new WMSExceptionMessage() { };

                var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

                emptyNamepsaces.Add("", "");
                var settings = new XmlWriterSettings();
                string xmlString = null;
                using (StringWriter sw = new StringWriter())
                {
                    XmlSerializer serialiser = new XmlSerializer(typeof(List<GoodsMovement>));
                    serialiser.Serialize(sw, lGoodsMovement, emptyNamepsaces);

                    xmlString = sw.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", " ");

                    List<GoodsMovement> oGoodsMovements = new List<GoodsMovement>();

                    string _oSQL = "EXEC [dbo].[sp_API_INV_MoveItem]";
                    _oSQL += " @GoodsMovementData=" + base.SQuote(xmlString);
                    _oSQL += ",@CreatedBy=" + base.LoggedInUserID;
                    DataSet _dsResult = base.FillDataSet(_oSQL, new List<SqlParameter>());

                    if (_dsResult == null)
                        throw new Exception("Move Item Procedure Returned NULL DataSet.");
                    else if (_dsResult.Tables.Count == 0)
                        throw new Exception("Move Item Procedure returned 0 Data Tables.");
                    // else if(_dsResult.Tables[0].Rows.Count)

                    List<GoodsMovement> _oGoodsMovement = MoveInventoryResult(_dsResult.Tables[1], lGoodsMovement);

                    return _oGoodsMovement;

                }
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception ex)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("lGoodsMovement", lGoodsMovement);

                ExceptionHandling.LogException(ex, _ClassCode + "004", oExcpData);

                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        //Developed
        public List<Inventory> GetActiveStock(SearchCriteria oCriteria)
        {
            try
            {
                //Method block to Fetch the Entity List from Database.

                List<Inventory> lInventory = new List<Inventory>();

                //StringBuilder sbQuery = new StringBuilder();
                //sbQuery.Append("SELECT *, (SELECT Code FROM StorageLocation WHERE ID = StorageLocationID) AS StorageLocation FROM [dbo].[vAvailableStock] WHERE (ISNULL(AvailableQty, 0) + ISNULL(IBOHQuantity,0) + ISNULL(OBOHQuantity,0)>0)  ");
                //sbQuery.Append((oCriteria.LocationID != 0 ? " AND LocationID=" + oCriteria.LocationID : " "));
                //sbQuery.Append(((oCriteria.MaterialCode != "" && oCriteria.MaterialCode != null) ? " AND MCode= " + base.SQuote(oCriteria.MaterialCode.ToString()) + "" : ""));
                //sbQuery.Append(((oCriteria.ContainerID != 0 ? " AND CartonID= " + oCriteria.ContainerID + "" : "")));
                //sbQuery.Append((oCriteria.ContainerCode != "" && oCriteria.ContainerCode != null) ? " AND CartonCode= " + base.SQuote(oCriteria.ContainerCode) + "" : "");
                //sbQuery.Append((oCriteria.MaterialRSN != null && oCriteria.MaterialRSN != "") ? " AND SerialNumber = '" + oCriteria.MaterialRSN + "'" : "");
                //DataSet _dsResults = base.FillDataSet(sbQuery.ToString());


                string _sSQL = "EXEC DBO.USP_API_FetchActiveStock ";

                List<SqlParameter> _lParams = new List<SqlParameter>();
                
                if (oCriteria.MaterialRSN != null)
                {
                    SqlParameter oParam = new SqlParameter("@RSN", SqlDbType.NVarChar, 50);
                    oParam.Value = oCriteria.MaterialRSN;

                    _lParams.Add(oParam);
                }


                if (oCriteria.LocationID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@LocationID", SqlDbType.Int);
                    oParam.Value = oCriteria.LocationID;

                    _lParams.Add(oParam);
                }

                if (oCriteria.LocationCode != null)
                {
                    SqlParameter oParam = new SqlParameter("@LocationCode", SqlDbType.NVarChar, 50);
                    oParam.Value = oCriteria.LocationCode;

                    _lParams.Add(oParam);
                }
                

                if (oCriteria.ContainerID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@ContainerID", SqlDbType.Int);
                    oParam.Value = oCriteria.ContainerID;

                    _lParams.Add(oParam);
                }

                if (oCriteria.ContainerCode != null)
                {
                    SqlParameter oParam = new SqlParameter("@ContainerCode", SqlDbType.NVarChar, 50);
                    oParam.Value = oCriteria.ContainerCode;

                    _lParams.Add(oParam);
                }


                if (oCriteria.MaterialCode != null)
                {
                    SqlParameter oParam = new SqlParameter("@MCode", SqlDbType.NVarChar, 50);
                    oParam.Value = oCriteria.MaterialCode;

                    _lParams.Add(oParam);
                }

                if (oCriteria.MaterialMasterID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@MaterialMasterID", SqlDbType.Int);
                    oParam.Value = oCriteria.MaterialMasterID;

                    _lParams.Add(oParam);
                }



                


                DataSet _dsResults = FillDataSet(_sSQL, _lParams);

                if (_dsResults != null)
                {
                    if (_dsResults.Tables.Count > 0)
                    {
                        foreach (DataRow _drInentory in _dsResults.Tables[0].Rows)
                        {
                            Inventory _TempInventory;
                            _TempInventory = BindInventoryMap(_drInentory);

                            lInventory.Add(_TempInventory);
                        }
                    }
                }

                return lInventory;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception ex)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCriteria", oCriteria);

                ExceptionHandling.LogException(ex, _ClassCode + "005", oExcpData);

                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }


        public List<Inventory> GetActiveStockWithOutRSN(SearchCriteria oCriteria)
        {
            try
            {
                //Method block to Fetch the Entity List from Database.

                List<Inventory> lInventory = new List<Inventory>();

                //StringBuilder sbQuery = new StringBuilder();
                //sbQuery.Append("SELECT *, (SELECT Code FROM StorageLocation WHERE ID = StorageLocationID) AS StorageLocation FROM [dbo].[vAvailableStock] WHERE (ISNULL(AvailableQty, 0) + ISNULL(IBOHQuantity,0) + ISNULL(OBOHQuantity,0)>0)  ");
                //sbQuery.Append((oCriteria.LocationID != 0 ? " AND LocationID=" + oCriteria.LocationID : " "));
                //sbQuery.Append(((oCriteria.MaterialCode != "" && oCriteria.MaterialCode != null) ? " AND MCode= " + base.SQuote(oCriteria.MaterialCode.ToString()) + "" : ""));
                //sbQuery.Append(((oCriteria.ContainerID != 0 ? " AND CartonID= " + oCriteria.ContainerID + "" : "")));
                //sbQuery.Append((oCriteria.ContainerCode != "" && oCriteria.ContainerCode != null) ? " AND CartonCode= " + base.SQuote(oCriteria.ContainerCode) + "" : "");
                //sbQuery.Append((oCriteria.MaterialRSN != null && oCriteria.MaterialRSN != "") ? " AND SerialNumber = '" + oCriteria.MaterialRSN + "'" : "");
                //DataSet _dsResults = base.FillDataSet(sbQuery.ToString());


                string _sSQL = "EXEC DBO.USP_API_FetchActiveStock_WithoutRSN ";

                List<SqlParameter> _lParams = new List<SqlParameter>();

                if (oCriteria.MaterialRSN != null)
                {
                    SqlParameter oParam = new SqlParameter("@RSN", SqlDbType.NVarChar, 50);
                    oParam.Value = oCriteria.MaterialRSN;

                    _lParams.Add(oParam);
                }


                if (oCriteria.LocationID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@LocationID", SqlDbType.Int);
                    oParam.Value = oCriteria.LocationID;

                    _lParams.Add(oParam);
                }

                if (oCriteria.LocationCode != null)
                {
                    SqlParameter oParam = new SqlParameter("@LocationCode", SqlDbType.NVarChar, 50);
                    oParam.Value = oCriteria.LocationCode;

                    _lParams.Add(oParam);
                }


                if (oCriteria.ContainerID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@ContainerID", SqlDbType.Int);
                    oParam.Value = oCriteria.ContainerID;

                    _lParams.Add(oParam);
                }

                if (oCriteria.ContainerCode != null)
                {
                    SqlParameter oParam = new SqlParameter("@ContainerCode", SqlDbType.NVarChar, 50);
                    oParam.Value = oCriteria.ContainerCode;

                    _lParams.Add(oParam);
                }


                if (oCriteria.MaterialCode != null)
                {
                    SqlParameter oParam = new SqlParameter("@MCode", SqlDbType.NVarChar, 50);
                    oParam.Value = oCriteria.MaterialCode;

                    _lParams.Add(oParam);
                }

                if (oCriteria.MaterialMasterID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@MaterialMasterID", SqlDbType.Int);
                    oParam.Value = oCriteria.MaterialMasterID;

                    _lParams.Add(oParam);
                }


                if (oCriteria.BatchNumber!=null)
                {
                    SqlParameter oParam = new SqlParameter("@BatchNumber", SqlDbType.NVarChar);
                    oParam.Value = oCriteria.BatchNumber;

                    _lParams.Add(oParam);
                }





                DataSet _dsResults = FillDataSet(_sSQL, _lParams);

                if (_dsResults != null)
                {
                    if (_dsResults.Tables.Count > 0)
                    {
                        foreach (DataRow _drInentory in _dsResults.Tables[0].Rows)
                        {
                            Inventory _TempInventory;
                            _TempInventory = BindInventoryMap(_drInentory);

                            lInventory.Add(_TempInventory);
                        }
                    }
                }

                return lInventory;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception ex)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCriteria", oCriteria);

                ExceptionHandling.LogException(ex, _ClassCode + "009", oExcpData);

                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        //Developed for GOODS MOVEMENT data Binding
        private GoodsMovement BindGoodsMovement(DataRow _drGoodsMovement, DataTable _dtInventory)
        {
            GoodsMovement _oGoodsMovement = new GoodsMovement
            {
                GoodsMovementDetailsID = ConversionUtility.ConvertToInt(_drGoodsMovement["GoodsMovementDetailsID"].ToString()),
                TransactionDocumentID = ConversionUtility.ConvertToInt(_drGoodsMovement["TransactionDocID"].ToString()),
                POSODetailsID = ConversionUtility.ConvertToInt(_drGoodsMovement["POSODetailsID"].ToString()),
                MaterialMasterID = ConversionUtility.ConvertToInt(_drGoodsMovement["MaterialMasterID"].ToString()),
                KitPlannerID = ConversionUtility.ConvertToInt(_drGoodsMovement["KitPlannerID"].ToString()),

                //DockGoodsMovementDetailsID = ConversionUtility.ConvertToInt(_drGoodsMovement[""].ToString()),

                //MaterialTransactionID = ConversionUtility.ConvertToInt(_drGoodsMovement[""].ToString()),
                //PickedFromSLoc = ConversionUtility.ConvertToInt(_drGoodsMovement[""].ToString()),
                //PickedContainerID = ConversionUtility.ConvertToInt(_drGoodsMovement[""].ToString()),
                //PutawayAtContainerID = ConversionUtility.ConvertToInt(_drGoodsMovement[""].ToString()),
                //PutawayAtLocationID = ConversionUtility.ConvertToInt(_drGoodsMovement[""].ToString()),
                //BUoMID = ConversionUtility.ConvertToInt(_drGoodsMovement[""].ToString()),

                //AutoPickFromCurrentLocation = ConversionUtility.ConvertToBool(_drGoodsMovement[""].ToString()),                

                //PickedFromLocationCode = _drGoodsMovement[""].ToString(),                
                //PickedContainerCode = _drGoodsMovement[""].ToString(),                
                //PutawayAtLocationCode = _drGoodsMovement[""].ToString(),                
                //PutawayAtContainerCode = _drGoodsMovement[""].ToString(),

            };
            _oGoodsMovement = BindInventory(_oGoodsMovement, _dtInventory);

            return _oGoodsMovement;
        }

        private GoodsMovement BindInventory(GoodsMovement oGoodsMovement, DataTable _dtInventory)
        {

            if (_dtInventory != null)
            {
                if (_dtInventory.Rows.Count > 0)
                {
                    List<Inventory> _lInventory = new List<Inventory>();

                    foreach (DataRow _drInventory in _dtInventory.Rows)
                    {
                        Inventory oInventory = new Inventory
                        {
                            MaterialCode = _drInventory["MCode"].ToString(),
                            LocationCode = _drInventory["LOCATION"].ToString(),

                            //ContainerCode=_drInventory[""].ToString(),
                            //ReferenceDocumentNumber=_drInventory[""].ToString(),
                            //RSN=_drInventory[""].ToString(),
                            //VehicleNumber=_drInventory[""].ToString(),

                            MaterialMasterID = ConversionUtility.ConvertToInt(_drInventory["MaterialMasterID"].ToString()),
                            MaterialTransactionID = ConversionUtility.ConvertToInt(_drInventory["MaterialTransactionID"].ToString()),
                            MOP = ConversionUtility.ConvertToInt(_drInventory["MOP"].ToString()),
                            ColorID = ConversionUtility.ConvertToInt(_drInventory["ColorAttributeValueID"].ToString()),

                            BatchNumber =_drInventory["BatchNumber"].ToString(),

                            //VehicleID=ConversionUtility.ConvertToInt(_drInventory[""].ToString()),
                            //ReferenceDocumentID=ConversionUtility.ConvertToInt(_drInventory[""].ToString()),
                            //ContainerID=ConversionUtility.ConvertToInt(_drInventory[""].ToString()),
                            //LocationID=ConversionUtility.ConvertToInt(_drInventory[""].ToString()),
                            //ReceivedInUoM=ConversionUtility.ConvertToInt(_drInventory[""].ToString()),
                            //PosoDetailsID=ConversionUtility.ConvertToInt(_drInventory[""].ToString()),
                            //StorageLocationID=ConversionUtility.ConvertToInt(_drInventory[""].ToString()),
                            //DockGoodsMovementID=ConversionUtility.ConvertToInt(_drInventory[""].ToString()),

                            MRP = ConversionUtility.ConvertToDecimal(_drInventory["MRP"].ToString()),
                            Quantity = ConversionUtility.ConvertToDecimal(_drInventory["AvailableQty"].ToString()),
                            IBOHQuantity = ConversionUtility.ConvertToDecimal(_drInventory["IBOHQuantity"].ToString()),
                            OBOHQuantity = ConversionUtility.ConvertToDecimal(_drInventory["OBOHQuantity"].ToString())
                        };

                        _lInventory.Add(oInventory);
                    }
                }
            }
            return oGoodsMovement;
        }

        //Developed INVENTORY data binding.
        private Inventory BindInventoryMap(DataRow _drInventory)
        {

            Inventory oInventory = new Inventory
            {
                MaterialCode = _drInventory["MCode"].ToString(),
                LocationCode = _drInventory["LOCATION"].ToString(),
                DisplayLocationCode = _drInventory["DisplayLocationCode"].ToString(),

                HomeLevelID = ConversionUtility.ConvertToInt(_drInventory["HomeLevelID"].ToString()),
                HomeZoneHeaderID = ConversionUtility.ConvertToInt(_drInventory["HomeZoneHeaderID"].ToString()),
                GenericMaterialID = ConversionUtility.ConvertToInt(_drInventory["SKUGenericCodeID"].ToString()),
                MaterialRangeID = ConversionUtility.ConvertToInt(_drInventory["MaterialRangeID"].ToString()),

                BatchNumber = _drInventory["BatchNumber"].ToString(),
                StorageLocationID = ConversionUtility.ConvertToInt(_drInventory["StorageLocationID"].ToString()),
                StorageLocation = _drInventory["StorageLocation"].ToString(),

                //ContainerCode=_drInventory[""].ToString(),
                //ReferenceDocumentNumber=_drInventory[""].ToString(),
                RSN = _drInventory["SerialNumber"].ToString(),

                ItemVolume = ConversionUtility.ConvertToDecimal(_drInventory["MaterialVolume"].ToString()),

                MaterialMasterID = ConversionUtility.ConvertToInt(_drInventory["MaterialMasterID"].ToString()),
                MaterialTransactionID = ConversionUtility.ConvertToInt(_drInventory["MaterialTransactionID"].ToString()),
                MOP = ConversionUtility.ConvertToInt(_drInventory["MOP"].ToString()),
                ColorID = ConversionUtility.ConvertToInt(_drInventory["ColorAttributeValueID"].ToString()),
                ContainerID = ConversionUtility.ConvertToInt(_drInventory["CartonID"].ToString()),

                //VehicleID=ConversionUtility.ConvertToInt(_drInventory[""].ToString()),
                //ReferenceDocumentID=ConversionUtility.ConvertToInt(_drInventory[""].ToString()),
                // ContainerID=ConversionUtility.ConvertToInt(_drInventory["CartonID"].ToString()),
                //LocationID=ConversionUtility.ConvertToInt(_drInventory[""].ToString()),
                //ReceivedInUoM=ConversionUtility.ConvertToInt(_drInventory[""].ToString()),
                //PosoDetailsID=ConversionUtility.ConvertToInt(_drInventory[""].ToString()),
                //StorageLocationID=ConversionUtility.ConvertToInt(_drInventory[""].ToString()),
                //DockGoodsMovementID=ConversionUtility.ConvertToInt(_drInventory[""].ToString()),

                MRP = ConversionUtility.ConvertToDecimal(_drInventory["MRP"].ToString()),
                Quantity = ConversionUtility.ConvertToDecimal(_drInventory["TotalQuantity"].ToString()),
                AvailableQuantity= ConversionUtility.ConvertToDecimal(_drInventory["AvailableQty"].ToString()),
                IBOHQuantity = ConversionUtility.ConvertToDecimal(_drInventory["IBOHQuantity"].ToString()),
                OBOHQuantity = ConversionUtility.ConvertToDecimal(_drInventory["OBOHQuantity"].ToString()),
                LocationID = ConversionUtility.ConvertToInt(_drInventory["LocationID"].ToString()),

                IsLost = ConversionUtility.ConvertToBool(_drInventory["IsLost"].ToString()),
                IsFinishedGoods = ConversionUtility.ConvertToBool(_drInventory["IsFG"].ToString()),
                IsRawMaterial = ConversionUtility.ConvertToBool(_drInventory["IsRM"].ToString()),
                IsConsumables = ConversionUtility.ConvertToBool(_drInventory["IsConsummables"].ToString()),

                Color = _drInventory["Color"].ToString(),
                ContainerCode = _drInventory["ContainerCode"].ToString(),

                IsDamaged = ConversionUtility.ConvertToBool(_drInventory["IsDamaged"].ToString()),
                MfgDate = ConversionUtility.ConvertToDateTime(_drInventory["MfgDate"].ToString()),
                MaterialShortDescription = _drInventory["MDescription"].ToString(),

                IsMaterialParent = ConversionUtility.ConvertToBool(_drInventory["IsMaterialParent"].ToString()),
                MonthOfMfg = ConversionUtility.ConvertToInt(_drInventory["MFGMonth"].ToString()),
                YearOfMfg = ConversionUtility.ConvertToInt(_drInventory["MFGYear"].ToString())

            };

            return oInventory;
        }

        private List<GoodsMovement> MoveInventoryResult(DataTable _dtMoveInventoryResult,List<GoodsMovement> oMovement)
        {
            try
            {
                foreach (GoodsMovement Item in oMovement)
                {

                    foreach (DataRow _drResult in _dtMoveInventoryResult.Rows)
                    {
                        if (_drResult["Result"].ToString().Contains("No Stock Available"))
                        {
                            throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_IN_DAL_001", WMSMessage = ErrorMessages.EMC_IN_DAL_001, ShowAsError = true };
                        }
                        if (Item.RSNNumber.Equals(_drResult["RSNNumber"].ToString()))
                        {
                            Item.IsMaterialOldMRP = ConversionUtility.ConvertToBool(_drResult["IsOldMRP"].ToString());
                            Item.PutawayMRP = ConversionUtility.ConvertToDecimal(_drResult["PutawayMRP"].ToString());
                            Item.PutawayAtSLoc = _drResult["PutawayAtSLoc"].ToString();
                            Item.MaterialDescription = _drResult["MaterialDescription"].ToString();                            
                         }
                    }

                }
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception ex)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCriteria", _dtMoveInventoryResult);

                ExceptionHandling.LogException(ex, _ClassCode + "006", oExcpData);

                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }

            return oMovement;
        }



        public List<Inventory> UpdateInventory(List<Inventory> oInventory)
        {


            try
            {
                var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

                emptyNamepsaces.Add("", "");
                var settings = new XmlWriterSettings();
                string xmlString = null;
                using (StringWriter sw = new StringWriter())
                {
                    XmlSerializer serialiser = new XmlSerializer(typeof(List<Inventory>));
                    serialiser.Serialize(sw, oInventory, emptyNamepsaces);

                    xmlString = sw.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", " ");
                    string strQuery = " EXEC [dbo].[sp_API_INV_UpdateItem] ";
                    strQuery += " @InventoryData=" + base.SQuote(xmlString);
                    strQuery += ",@CreatedBy=" + base.LoggedInUserID;
                    base.ExecuteNonQuery(strQuery, new List<SqlParameter>());

                    return oInventory;
                }

            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception ex)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oInventory", oInventory);

                ExceptionHandling.LogException(ex, _ClassCode + "006", oExcpData);

                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }

        }

        public Inventory ReceiveExcessInventory(Inventory oInventory)
        {
            try
            {
                var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

                emptyNamepsaces.Add("", "");
                var settings = new XmlWriterSettings();
                string xmlString = null;
                using (StringWriter sw = new StringWriter())
                {
                    XmlSerializer serialiser = new XmlSerializer(typeof(List<Inventory>));
                    serialiser.Serialize(sw, oInventory, emptyNamepsaces);


                    string _sQuery = " EXEC [dbo].[sp_API_INV_ReciveExcessItem] ";

                    List<SqlParameter> lSQLParams = new List<SqlParameter>();

                    SqlParameter oParam1 = new SqlParameter("@InventoryData", SqlDbType.Xml);
                    oParam1.Value = xmlString;
                    lSQLParams.Add(oParam1);



                    SqlParameter oParam2 = new SqlParameter("@CreatedBy", SqlDbType.Int, 50);
                    oParam2.Value = LoggedInUserID;
                    lSQLParams.Add(oParam2);


                    DataSet dsResult = FillDataSet(_sQuery, lSQLParams);

                    if (dsResult == null)
                        throw new Exception("Receive Excess Item Procedure Returned NULL DataSet.");
                    else if (dsResult.Tables.Count == 0)
                        throw new Exception("Receive Excess Item Procedure returned 0 Data Tables.");


                    if (dsResult != null && dsResult.Tables.Count != 0 && dsResult.Tables[0].Rows.Count != 0)
                    {


                        oInventory.ColorID = Convert.ToInt32(dsResult.Tables[0].Rows[0]["ColorAttributeValueID"].ToString());
                        oInventory.MRP = Convert.ToDecimal(dsResult.Tables[0].Rows[0]["MRP"].ToString());
                        oInventory.MOP = Convert.ToInt32(dsResult.Tables[0].Rows[0]["MOP"].ToString());
                        oInventory.IsReceived = true;
                        oInventory.IsFinishedGoods = ConversionUtility.ConvertToBool(dsResult.Tables[0].Rows[0]["IsFG"].ToString());
                        oInventory.IsRawMaterial = ConversionUtility.ConvertToBool(dsResult.Tables[0].Rows[0]["IsRM"].ToString());
                        oInventory.IsConsumables = ConversionUtility.ConvertToBool(dsResult.Tables[0].Rows[0]["IsConsummables"].ToString());
                        oInventory.Color = dsResult.Tables[0].Rows[0]["Color"].ToString();
                        oInventory.DocumentQuantity = ConversionUtility.ConvertToDecimal(dsResult.Tables[0].Rows[0]["DocumentQuantity"].ToString());
                        oInventory.DocumentProcessedQuantity = ConversionUtility.ConvertToDecimal(dsResult.Tables[0].Rows[0]["DocumentReceivedQty"].ToString());
                        oInventory.IsExcessInventory = true;

                        return oInventory;
                    }
                    else
                    {
                        oInventory.IsReceived = false;
                        return oInventory;

                    }
                }
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception ex)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oInventory", oInventory);

                ExceptionHandling.LogException(ex, _ClassCode + "007", oExcpData);

                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }

        }

        public GoodsMovement ChangeMRP(GoodsMovement oMovement)
        {
            try
            {
                string _sQuery = " EXEC [dbo].[sp_API_INV_UpdateMRP] ";

                List<SqlParameter> lSQLParams = new List<SqlParameter>();

                SqlParameter oParam1 = new SqlParameter("@RSNNumber", SqlDbType.NVarChar);
                oParam1.Value = oMovement.RSNNumber;
                lSQLParams.Add(oParam1);



                SqlParameter oParam2 = new SqlParameter("@CreatedBy", SqlDbType.Int, 50);
                oParam2.Value = LoggedInUserID;
                lSQLParams.Add(oParam2);


                DataSet dsResult = FillDataSet(_sQuery, lSQLParams);
                if(dsResult!=null)
                {
                    oMovement.PutawayMRP = Convert.ToDecimal(dsResult.Tables[0].Rows[0]["MRP"]);
                    return oMovement;
                }
                else
                {
                    throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
                }
                
               

            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception ex)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oMovement", oMovement);

                ExceptionHandling.LogException(ex, _ClassCode + "008", oExcpData);

                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public bool IsRSNExisted(string RSNNumber)
        {


            
            try
            {
                //Method block to Fetch the Entity List from Database.
                    string _sSQL = "SELECT  count(ActiveStockDetailsID) as N FROM INV_ActiveStockDetails WHERE SerialNumber='"+ RSNNumber+"'";

                    DataSet _dsResults = FillDataSet(_sSQL, null);
              
                    if (Convert.ToInt32( _dsResults.Tables[0].Rows[0][0]) > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
              
              
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception ex)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("RSNNumber", RSNNumber);

                ExceptionHandling.LogException(ex, _ClassCode + "009", oExcpData);

                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }

            

        }
    }
}
