using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MRLWMSC21Common;
using System.Data;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Globalization;
using System.Text;
using System.Web.UI;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

using static MRLWMSC21.mOutbound.OutboundSearch;
//---------- Author : Durga---------
//-------- Created Date : 15/11/2017-------------//
namespace MRLWMSC21.mOutbound.BL
{
    public class OutBoundBL
    {

        CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        //------------------------ added by durga for getting tenant data on 15/11/2017----------------------//
        public List<DropDownListData> getTenantData()
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            List<DropDownListData> otenantlst = new List<DropDownListData>();
            DataSet DS = DB.GetDS("SELECT TenantID,TenantName+' - '+TenantCode name FROM TPL_Tenant where (" + cp.AccountID + "=0 or  AccountID=" + cp.AccountID + ") and IsDeleted=0 and IsActive=1" +
                                    "and TenantName is not null and TenantCode is not null and TenantName!='' and TenantCode!='' order by TenantName", false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    otenantlst.Add(new DropDownListData()
                    {
                        Id = Convert.ToInt32(row["TenantID"]),
                        Name = row["name"].ToString()
                    });
                }
            }
            return otenantlst;
        }
        //------------------------ added by durga for getting OBD Types data on 15/11/2017----------------------//
        public List<DropDownListDataFilter> getOBDTypesData()
        {
            List<DropDownListDataFilter> obdtypelst = new List<DropDownListDataFilter>();
            DataSet DS = DB.GetDS("select DocumentTypeID,DocumentType,TenantID from GEN_DocumentType where IsDeleted=0 and IsActive=1 and DocumentTypeID=1", false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    obdtypelst.Add(new DropDownListDataFilter()
                    {
                        Id = Convert.ToInt32(row["DocumentTypeID"]),
                        Name = row["DocumentType"].ToString(),
                        FilterId = Convert.ToInt32(row["TenantID"])
                    });
                }
            }
            return obdtypelst;
        }
        //------------------------ added by durga for getting OBD Types data on 15/11/2017----------------------//
        public List<DropDownListDataFilter> getCustomerData(string prefix, int tenantid)
        {
            List<DropDownListDataFilter> obdtypelst = new List<DropDownListDataFilter>();
            DataSet DS = DB.GetDS("select top 10 CustomerID,(CustomerName+' - '+CustomerCode) customer from GEN_Customer " +
                                    "where IsDeleted=0 and IsActive=1 and CustomerID>0 and (CustomerName like '" + prefix + "%' or CustomerCode like'" + prefix + "%') and TenantID=" + tenantid, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    obdtypelst.Add(new DropDownListDataFilter()
                    {
                        Id = Convert.ToInt32(row["CustomerID"]),
                        Name = row["customer"].ToString(),
                    });
                }
            }
            return obdtypelst;
        }
        //------------------------ added by durga for getting Warehouse data on 15/11/2017----------------------//
        public List<DropDownListDataFilter> getWareHouseDataData()
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            List<DropDownListDataFilter> otenantlst = new List<DropDownListDataFilter>();
            DataSet DS = DB.GetDS("select WarehouseID,WHCode from GEN_Warehouse where IsDeleted=0 and IsActive=1 and (" + cp.AccountID + "=0 or AccountID = " + cp.AccountID + ") order by WHCode ", false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    otenantlst.Add(new DropDownListDataFilter()
                    {
                        Id = Convert.ToInt32(row["WarehouseID"]),
                        Name = row["WHCode"].ToString(),

                    });

                }
            }
            return otenantlst;
        }
        public List<DropDownListDataFilter> getWareHouseDataData1(int TenantID)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            List<DropDownListDataFilter> otenantlst = new List<DropDownListDataFilter>();
            // DataSet DS = DB.GetDS("select WarehouseID,WHCode from GEN_Warehouse where IsDeleted=0 and IsActive=1 and ("+ cp.AccountID+"=0 or AccountID = " + cp.AccountID + ") order by WHCode ", false);
            DataSet DS = DB.GetDS("SELECT DISTINCT TC.WarehouseID,WH.WHCode,TC.TenantID FROM TPL_Tenant_Contract AS TC JOIN  GEN_Warehouse AS WH ON TC.WarehouseID = WH.WarehouseID AND TC.IsActive=1 AND TC.IsDeleted=0 AND WH.IsActive=1 AND WH.IsDeleted=0 AND FORMAT(GETDATE(),'yyyy-MM-dd') BETWEEN FORMAT(TC.EffectiveFrom,'yyyy-MM-dd') AND  FORMAT(TC.EffectiveTo,'yyyy-MM-dd') AND TC.TenantID =" + TenantID, false);

            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    otenantlst.Add(new DropDownListDataFilter()
                    {
                        Id = Convert.ToInt32(row["WarehouseID"]),
                        Name = row["WHCode"].ToString(),

                    });

                }
            }
            return otenantlst;
        }
        public List<DropDownListDataFilter> getWareHouseDataData(string tenantid)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            List<DropDownListDataFilter> otenantlst = new List<DropDownListDataFilter>();
            DataSet DS = DB.GetDS("select distinct WH.WarehouseID,WHCode from GEN_Warehouse WH join TPL_Tenant_Contract TC on wh.WarehouseID=TC.WarehouseID and WH.IsDeleted=0 and WH.IsActive=1 and TC.IsDeleted=0 and tc.IsActive=1 where  (" + cp.AccountID + "=0 or AccountID = " + cp.AccountID + ") and (" + tenantid + "=0 or tc.TenantID=" + tenantid + ") order by WHCode ", false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    otenantlst.Add(new DropDownListDataFilter()
                    {
                        Id = Convert.ToInt32(row["WarehouseID"]),
                        Name = row["WHCode"].ToString(),

                    });

                }
            }
            return otenantlst;
        }
        public List<DropDownListDataFilter> GetInboundDataForGateEntry(string prefix, string tenantid)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            List<DropDownListDataFilter> otenantlst = new List<DropDownListDataFilter>();
            DataSet DS = DB.GetDS("select top 10 InboundID,StoreRefNo from INB_Inbound where IsDeleted=0 and IsActive=1 and InboundStatusID in (3,5) and TenantID=" + tenantid + " and StoreRefNo like '" + prefix + "%' ", false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    otenantlst.Add(new DropDownListDataFilter()
                    {
                        Id = Convert.ToInt32(row["InboundID"]),
                        Name = row["StoreRefNo"].ToString(),

                    });

                }
            }
            return otenantlst;
        }
        public List<DropDownListDataFilter> GetMIMETypesForGateEntry()
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            List<DropDownListDataFilter> otenantlst = new List<DropDownListDataFilter>();
            DataSet DS = DB.GetDS("select YM_MST_DocumentMIME_ID,MIME from YM_MST_DocumentMIME where IsActive=1 and IsDeleted=0", false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    otenantlst.Add(new DropDownListDataFilter()
                    {
                        Id = Convert.ToInt32(row["YM_MST_DocumentMIME_ID"]),
                        Name = row["MIME"].ToString(),

                    });

                }
            }
            return otenantlst;
        }
        public List<DropDownListDataFilter> GetDocumenttypesForGateEntry(int shipmentid)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            List<DropDownListDataFilter> otenantlst = new List<DropDownListDataFilter>();
            string query = "";
            if (shipmentid == 1)
            {
                query = "select YM_MST_DocumentType_ID,DocumentType from YM_MST_DocumentTypes where isdeleted=0 and isactive=1 and YM_MST_ShipmentType_ID=1";
            }
            else
            {
                query = "select YM_MST_DocumentType_ID,DocumentType from YM_MST_DocumentTypes where isdeleted=0 and isactive=1 and YM_MST_ShipmentType_ID=2";
            }
            DataSet DS = DB.GetDS(query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    otenantlst.Add(new DropDownListDataFilter()
                    {
                        Id = Convert.ToInt32(row["YM_MST_DocumentType_ID"]),
                        Name = row["DocumentType"].ToString(),

                    });

                }
            }
            return otenantlst;
        }
        //------------------------ added by durga for outbound revert (getting OBD numbers on 20/03/2018)----------------------//
        public List<DropDownListDataFilter> getItemsForOBDRevert(string Prefix, int accountid)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            List<DropDownListDataFilter> otenantlst = new List<DropDownListDataFilter>();
            DataSet DS = DB.GetDS("select top 10 MaterialMasterID,mcode from mmt_materialmaster where IsDeleted=0 and isactive=1 and (MCode like '" + Prefix + "%' or MCode like '%" + Prefix + "%' or MCode like '%" + Prefix + "') " +
                                    "   and  tenantid in (select TenantID from TPL_Tenant where AccountID = " + accountid + ")", false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    otenantlst.Add(new DropDownListDataFilter()
                    {
                        Id = Convert.ToInt32(row["MaterialMasterID"]),
                        Name = row["mcode"].ToString(),

                    });

                }

            }
            return otenantlst;
        }
        //------------------------ added by durga for outbound revert (getting OBD numbers on 20/03/2018)----------------------//
        public List<DropDownListDataFilter> getOBDNumbersForOBDRevert(string Prefix, int accountid, int deliverystatusid)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            List<DropDownListDataFilter> otenantlst = new List<DropDownListDataFilter>();
            DataSet DS = DB.GetDS("select top 10 outboundid,obdnumber from obd_outbound where IsDeleted=0 and IsActive=1 and  tenantid in (select TenantID from TPL_Tenant where AccountID=" + accountid + ") " +
                "and deliverystatusid=" + deliverystatusid + " and (OBDNumber like '" + Prefix + "%' or OBDNumber like '%" + Prefix + "%' or OBDNumber like '%" + Prefix + "')", false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    otenantlst.Add(new DropDownListDataFilter()
                    {
                        Id = Convert.ToInt32(row["outboundid"]),
                        Name = row["obdnumber"].ToString(),

                    });

                }
            }
            return otenantlst;
        }
        //------------------------  for getting Vehicle Types data on 17/01/2018----------------------//
        public List<DropDownListDataFilter> getVehicleTypeData()
        {
            List<DropDownListDataFilter> Vctypelst = new List<DropDownListDataFilter>();
            DataSet DS = DB.GetDS("select top 10 VehicleID AS VehicleTypeID, Vehicle AS VehicleType  from GEN_Vehicle where IsDeleted=0 and IsActive=1 ", false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    Vctypelst.Add(new DropDownListDataFilter()
                    {
                        Id = Convert.ToInt32(row["VehicleTypeID"]),
                        Name = row["VehicleType"].ToString(),

                    });

                }
            }
            return Vctypelst;
        }

        //------------------------  for getting Docks data on 17/01/2018----------------------//
        public List<DropDownListDataFilter> getDocks(string WareHouseID = "0")
        {
            List<DropDownListDataFilter> doclst = new List<DropDownListDataFilter>();
            //DataSet DS = DB.GetDS("SELECT DockID, DockNo FROM GEN_Dock DOC WHERE WarehouseID = " + WareHouseID + " AND  IsDeleted=0 and IsActive=1 and DockTypeID=2 and  DOCKID NOT IN (SELECT  DISTINCT  DockID FROM OBD_VLPD WHERE  StatusId >= 2 AND 4 < StatusId) ", false);
            DataSet DS = DB.GetDS("exec [SP_GetDocks]@IsOutbound=1,@Warehouseid=" + WareHouseID, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    doclst.Add(new DropDownListDataFilter()
                    {
                        Id = Convert.ToInt32(row["DockID"]),
                        Name = row["DockNo"].ToString(),

                    });

                }
            }
            return doclst;
        }

        //------------------------ added by durga for getting SOlist with availbleQty based on  input Excel sonumbers on 15/11/2017----------------------//
        public DataTable getSOListForCreateOutbound(OutboundSerachData obj)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            DataSet ds = DB.GetDS("exec [dbo].[GET_AVAILABLESTOCK_SODETAILS] @SoNumbers='" + obj.Sonumbers + "',@TenantID=" + obj.TenantId, false);
            return ds.Tables[0];
        }

        //------------------------ added by durga for creating outbound based input selected sonumbers on 17/11/2017----------------------//
        public int CreateOutboundForSelectedSonumbers(string xml, int tenantid, int warehouseid, int deliverytypeid, int userid)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            return DB.GetSqlN("[dbo].[sp_UPSERT_BULK_CREATEDELIVERY] @XML='" + xml + "',@WAREHOUSEID=" + warehouseid + ",@TENANTID=" + tenantid + ",@CreatedBy=" + userid + ",@DOCUMENTTYPEID=" + deliverytypeid);
        }

        //------------------------ added by durga for OBD list for group OBD on 17/11/2017----------------------//
        public List<Outbound> getoUTBOUNDDataForGroupOBD(GroupOutboundSearchData obj)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            if (obj.FromDate == null)
            {
                obj.FromDate = "";
            }
            else if (obj.FromDate != "")
            {
                obj.FromDate = DateTime.ParseExact(obj.FromDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                //obj.FromDate = DateTime.ParseExact(obj.FromDate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            }

            if (obj.ToDate == null)
            {
                obj.ToDate = "";
            }
            else if (obj.ToDate != "" || obj.ToDate == null)
            {
                obj.ToDate = DateTime.ParseExact(obj.ToDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                //obj.ToDate = DateTime.ParseExact(obj.ToDate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            }

            List<Outbound> obdlist = new List<Outbound>();
            DataSet DS = DB.GetDS("exec [dbo].[sp_GetOBDListForGroupOBD] @tenantid=" + obj.TenantId + ",@customerName='" + obj.CustomerName + "',@FromDate='" + obj.FromDate + "',@ToDate='" + obj.ToDate + "',@warehouseid=" + obj.WareHouseId, false);

            if (DS.Tables[0].Rows.Count != 0)
            {
                int sno = 1;
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    DateTime abc = DateTime.ParseExact(row["OBDDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    obdlist.Add(new Outbound()
                    {
                        SNO = sno++,
                        Isselected = false,
                        TenantID = Convert.ToInt32(row["TenantID"]),
                        WarehouseID = Convert.ToInt32(row["WarehouseID"]),
                        OutboundID = Convert.ToInt32(row["OutboundID"]),
                        OBDNumber = row["OBDNumber"].ToString(),
                        OBDDate = abc.ToString("dd-MMM-yyyy"),
                        CustomerID = Convert.ToInt32(row["CustomerID"]),
                        CustomerName = row["CustomerName"].ToString(),
                        tenantname = row["tenantname"].ToString(),
                        WHCode = row["WHCode"].ToString(),
                        Volume = Convert.ToDecimal(row["Volume"]),
                        Weight = Convert.ToDecimal(row["Weight"])


                    });

                }
            }
            return obdlist;
        }
        //------------------------ added by durga for group OBD list for group OBD on 17/11/2017----------------------//
        public List<GroupOBD> getGroupOUTBOUNDDataForGroupOBD(int VLPDID, int TenantID, int WHID)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            List<GroupOBD> obdlist = new List<GroupOBD>();
            DataSet DS = DB.GetDS("exec [dbo].[OBD_GetGroupOBDList] @VLPDID=" + VLPDID + ",@AccountID_New=" + cp.AccountID + ",@TenantID_New=" + cp.TenantID + ",@UserID_New=" + cp.UserID + ",@UserTypeID_New=" + cp.UserTypeID + ",@TenantID=" + TenantID + ",@WHID=" + WHID + "", false);

            if (DS.Tables[0].Rows.Count != 0)
            {
                int sno = 1;
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    //DateTime abc = DateTime.ParseExact(row["CreatedOn"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    obdlist.Add(new GroupOBD()
                    {
                        SNO = sno++,
                        tenantName = row["tenant"].ToString(),
                        WareHouse = row["WHCode"].ToString(),
                        GRPNumber = row["VLPDNumber"].ToString(),
                        StatusId = Convert.ToInt32(row["StatusId"]),
                        CreatedDate = row["CreatedOn"].ToString(),
                        createduser = row["createdby"].ToString(),
                        Status = row["VLPDStatus"].ToString(),
                        ID = Convert.ToInt32(row["Id"])

                    });

                }
            }
            return obdlist;
        }

        //------------------------ added by durga for group OBD list for group OBD on 17/11/2017----------------------//
        public List<GroupOBDList> getOBDList(int VLPDID)
        {
            List<GroupOBDList> obdlist = new List<GroupOBDList>();
            DataSet DS = DB.GetDS("exec [SP_GetOBDListForVLPD] @VLPDId=" + VLPDID + " ", false);

            if (DS.Tables[0].Rows.Count != 0)
            {
                int sno = 1;
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    obdlist.Add(new GroupOBDList()
                    {

                        OBDNumber = row["OBDNumber"].ToString(),
                        OBDDate = row["OBDDate"].ToString(),
                        CustomerName = row["CustomerName"].ToString(),
                        AssignedQuantity = Convert.ToDecimal(row["AssignedQuantity"])


                    });

                }
            }
            return obdlist;
        }
        //------------------------ added by durga for OBD list for group OBD on 17/11/2017----------------------//
        public string createGroupobd(GroupOutboundSearchData obj, string obdids, int userid, int VehicleTypeId, string VehicleNo, string Driver, string Mobile, int DockID)
        {
            return DB.GetSqlS("DECLARE @NewUpdateOutboundID NVARCHAR(50); exec [dbo].[sp_OBD_InsertGroupOBD] @AccountID=" + cp.AccountID + ",@tenantid=" + obj.TenantId + ",@OutboundID='" + obdids + "',@CreatedBy=" + userid + ",@warehouseid=" + obj.WareHouseId + ", @VehicleTypeID=" + VehicleTypeId + ",@VehicleNumber='" + VehicleNo + "',@DriverName='" + Driver + "',@MobileNumber='" + Mobile + "',@DockId=" + DockID + ", @NewVLPDNumber_Return = @NewUpdateOutboundID OUTPUT; select @NewUpdateOutboundID AS S");
        }
        public string VerifyVLPD(int VLPDID, int userid)
        {

            string Query = " EXEC [dbo].[sp_VLPD_VERIFICATION] @VLPD = " + VLPDID + " , @USERID = " + userid + "";
            string result = "";
            int Result = DB.GetSqlN(Query);

            if (Result == -1)
            {
                result = "Quantity is Not Avaiable";
            }
            else if (Result == -5)
            {
                result = "Picking is not at done";
            }
            else if (Result == 1)
            {
                result = "Verification is done";
            }
            else
            {
                result = "Verification is not yet done";
            }
            return result;
        }
        public List<SODetails> GetSOInfo(OutboundSerachData obj)
        {
            List<SODetails> lst = new List<SODetails>();
            try
            {
                string Query = "EXEC [dbo].[GEN_ITEMWISE_AVAIABLESTOCKFORSOLIST] @SoNumbers = " + DB.SQuote(obj.Sonumbers) + ",@TenantID = " + obj.TenantId + "";
                DataSet DS = DB.GetDS(Query, false);
                if (DS.Tables.Count != 0 && DS.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        SODetails SOD = new SODetails();
                        SOD.MaterialMasterID = Convert.ToInt32(row["MaterialMasterID"]);
                        SOD.Mcode = row["Mcode"].ToString();
                        SOD.SODetailsID = Convert.ToInt32(row["SODetailsID"]);
                        SOD.SONumber = row["SONumber"].ToString();
                        SOD.MDescription = row["MDescription"].ToString();
                        SOD.SoQuantity = Convert.ToDecimal(row["SOQuantity"]);
                        SOD.PendingQty = Convert.ToDecimal(row["PendingQty"]);
                        SOD.AvailableQty = Convert.ToDecimal(row["AvailableQuantity"]);
                        SOD.InProcessQty = Convert.ToDecimal(row["InProcessQty"]);
                        SOD.LineNumber = row["LineNumber"].ToString();
                        SOD.Isselected = Convert.ToDecimal(row["AvailableQuantity"].ToString()) == 0 ? false : true;
                        SOD.BatchNo = row["BatchNo"].ToString();
                        SOD.SerialNo = row["SerialNo"].ToString();
                        SOD.ProjectRefNo = row["ProjectRefNo"].ToString();
                        SOD.ExpDate = row["ExpDate"].ToString();
                        SOD.MfgDate = row["MfgDate"].ToString();
                        lst.Add(SOD);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return lst;
        }

        public OBDResult CreateOutbound(OutboundSerachData obj)
        {
            OBDResult objrslt = new OBDResult();
            string SONumbers = obj.Sonumbers.Trim();
            List<ReleaseOBD> lst = new List<ReleaseOBD>();
            string Query = " EXEC [dbo].[sp_UPSERT_BULK_OBD_CREATION_NEW1] @SoNumbers = " + DB.SQuote(SONumbers.Trim()) + " ,@AccountID=" + cp.AccountID + ", @TenantId = " + obj.TenantId + ", @WareHouseId = " + obj.WareHouseId + ", @DeliveryTypeId = " + obj.DeliveryTypeId + ", @CreatedBy = " + cp.UserID + ", @PriorityTypeID =" + obj.PriorityTypeID;
            // string result = DB.GetSqlS(Query);
            DataSet DS = DB.GetDS(Query, false);

            if (DS.Tables.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    int Fag = Convert.ToInt32(row["Fag"]);
                    ReleaseOBD RO = new ReleaseOBD();
                    objrslt.status = 0;
                    if (Fag == -1)
                    {
                        objrslt.status = -1; // no pending quantity
                        objrslt.oOBDlst = null;
                        return objrslt;

                    }
                    else if (Fag == -0)
                    {
                        objrslt.status = -2; // Error while Creation
                        objrslt.oOBDlst = null;
                        return objrslt;
                    }
                    else
                    {
                        RO.OutboundID = Convert.ToInt32(row["OutBoundID"]);
                        RO.OBDRefNo = row["OutboundNo"].ToString();
                        RO.CustomerName = row["CustomerName"].ToString();
                        lst.Add(RO);
                    }
                    //if (Fag == -1 || Fag == -0)
                    //{
                    //    lst = null;
                    //    return lst;
                    //}

                }
                if (DS.Tables[0].Rows.Count == 0)
                {
                    objrslt.status = -1;
                    return objrslt;
                }
            }
            objrslt.oOBDlst = lst;
            return objrslt;
        }



        //------------------GET DELIVERY PICK NOTE --------------------------------------------//

        public List<DeliveryPickNote> GetVLPDItems(int VLPDID, int TransferRequestID = 0)
        {
            List<DeliveryPickNote> lst = new List<DeliveryPickNote>();
            try
            {
                string Query = "EXEC [dbo].[GEN_VLPD_ITEMS_ALLOCATED_LIST_OBDWISE_NEW] @VLPDID =" + VLPDID + ",@TransferRequestID=" + TransferRequestID;
                DataSet DS = DB.GetDS(Query, false);
                if (DS.Tables.Count != 0 && DS.Tables[0].Rows.Count != 0)
                {
                    //foreach (DataRow row in DS.Tables[0].Rows)
                    //{
                    for (int index = 0; index < DS.Tables[0].Rows.Count; index++)
                    {
                        lst.Add(new DeliveryPickNote()
                        {
                            VLPDAssignId = Convert.ToInt32(DS.Tables[0].Rows[index]["ID"].ToString()),
                            MaterialMasterID = Convert.ToInt32(DS.Tables[0].Rows[index]["MaterialMasterID"].ToString()),
                            MCode = DS.Tables[0].Rows[index]["Mcode"].ToString(),
                            MDescription = DS.Tables[0].Rows[index]["MDescription"].ToString(),
                            CartonCode = DS.Tables[0].Rows[index]["CartonCode"].ToString(),
                            AssiginQty = Convert.ToDecimal(DS.Tables[0].Rows[index]["AssignedQuantity"].ToString()),
                            PickedQty = Convert.ToDecimal(DS.Tables[0].Rows[index]["PickedQty"].ToString()),
                            PendingQty = Convert.ToDecimal(DS.Tables[0].Rows[index]["PendingQty"].ToString()),
                            Location = DS.Tables[0].Rows[index]["Location"].ToString(),
                            LocationID = Convert.ToInt32(DS.Tables[0].Rows[index]["LocationID"]),
                            BatchNo = DS.Tables[0].Rows[index]["BatchNo"].ToString(),
                            SerialNo = DS.Tables[0].Rows[index]["SerialNo"].ToString(),
                            ProjectRefNo = DS.Tables[0].Rows[index]["ProjectRefNo"].ToString(),
                            MRP = DS.Tables[0].Rows[index]["MRP"].ToString(),
                            ExpDate = DS.Tables[0].Rows[index]["ExpDate"].ToString(),
                            MfgDate = DS.Tables[0].Rows[index]["MfgDate"].ToString(),
                            AssignID = Convert.ToInt32(DS.Tables[0].Rows[index]["AssignedID"]),
                            OutboundID = DS.Tables[0].Rows[index]["OutboundID"].ToString() != "" ? Convert.ToInt32(DS.Tables[0].Rows[index]["OutboundID"]) : 0,
                            SODetailsID = DS.Tables[0].Rows[index]["SODetailsID"].ToString() != "" ? Convert.ToInt32(DS.Tables[0].Rows[index]["SODetailsID"]) : 0,
                            StorageLocationID = Convert.ToInt32(DS.Tables[0].Rows[index]["StorageLocationID"]),
                            CartonID = DS.Tables[0].Rows[index]["CartonID"].ToString() != "" ? Convert.ToInt32(DS.Tables[0].Rows[index]["CartonID"]) : 0,
                            VLPDNumber = DS.Tables[0].Rows[index]["VLPDNumber"].ToString(),
                            WHCode = DS.Tables[0].Rows[index]["WHCode"].ToString()
                            //oPickNoteInfo = GetDeliveryPickItems(VLPDID, Convert.ToInt32(DS.Tables[0].Rows[index]["MaterialMasterID"]))

                        });

                    }

                    return lst;
                }

                //}
            }

            catch (Exception ex)
            {

            }
            return lst;
        }

        public List<PickNoteInfo> GetDeliveryPickItems(int VLPDID, int MaterialId)
        {
            List<PickNoteInfo> lst = new List<PickNoteInfo>();
            try
            {
                string Query = "EXEC [dbo].[GEN_VLPD_ITEMS_ALLOCATED_LIST] @VLPDID =" + VLPDID + ", @MaterialMasterID=" + MaterialId;
                DataSet DS = DB.GetDS(Query, false);
                if (DS.Tables.Count != 0 && DS.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {

                        PickNoteInfo SOD = new PickNoteInfo();
                        SOD.MaterialMasterID = Convert.ToInt32(row["MaterialMasterID"]);

                        SOD.CartonID = Convert.ToInt32(row["CartonID"].ToString());
                        SOD.AssignID = Convert.ToInt32(row["AssignID"].ToString());
                        SOD.CartonCode = row["CartonCode"].ToString();
                        //SOD.Quantity = Convert.ToDecimal(row["Quantity"].ToString());
                        SOD.AssiginQty = string.IsNullOrEmpty(row["Assignedquantity"].ToString()) ? 0 : Convert.ToDecimal(row["Assignedquantity"].ToString());
                        //Convert.ToDecimal(row["Assignedquantity"].ToString());
                        SOD.PickedQty = string.IsNullOrEmpty(row["PickedQty"].ToString()) ? 0 : Convert.ToDecimal(row["PickedQty"].ToString());
                        SOD.PendingQty = string.IsNullOrEmpty(row["RemaingQty"].ToString()) ? 0 : Convert.ToDecimal(row["RemaingQty"].ToString());
                        SOD.Location = row["Location"].ToString();
                        //SOD.LocationID = Convert.ToInt32(row["LocationID"]);
                        SOD.BatchNo = row["BatchNo"].ToString();
                        SOD.SerialNo = row["SerialNo"].ToString();
                        SOD.ProjectRefNo = row["ProjectRefNo"].ToString();
                        SOD.ExpDate = row["ExpDate"].ToString();
                        SOD.MfgDate = row["MfgDate"].ToString();
                        lst.Add(SOD);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return lst;
        }

        //------------------GET Release Outbound  --------------------------------------------//

        public List<ReleaseOBD> GetOBDReleaseList(String OBDNumber, String FilterTenantID,string PageIndex,string PageSize)
        {
            List<ReleaseOBD> lst = new List<ReleaseOBD>();
            try
            {
                string Query = "EXEC [dbo].[sp_GET_OPENOBDSLIST] @OBDNumber ='" + OBDNumber + "',@AccountID_New=" + cp.AccountID + ",@TenantID_New=" + ((FilterTenantID == String.Empty) ? cp.TenantID.ToString() : FilterTenantID) + ",@UserID_New=" + cp.UserID + ",@UserTypeID_New=" + cp.UserTypeID + ",@PageIndex=" + PageIndex + ",@PageSize=" + PageSize;
                DataSet DS = DB.GetDS(Query, false);
                if (DS.Tables.Count != 0 && DS.Tables[0].Rows.Count != 0)
                {
                    //foreach (DataRow row in DS.Tables[0].Rows)
                    //{
                    for (int index = 0; index < DS.Tables[0].Rows.Count; index++)
                    {
                        lst.Add(new ReleaseOBD()
                        {
                            OutboundID = Convert.ToInt32(DS.Tables[0].Rows[index]["OutboundID"]),
                            OBDRefNo = DS.Tables[0].Rows[index]["OBDNumber"].ToString(),
                            TenantName = DS.Tables[0].Rows[index]["CompanyName"].ToString(),
                            CustomerName = DS.Tables[0].Rows[index]["CustomerName"].ToString(),
                            OBDDate = DS.Tables[0].Rows[index]["OBDDate"].ToString().Split()[0],
                            // SONumber = DS.Tables[0].Rows[index]["SONumber"].ToString(),
                            // SODate = DS.Tables[0].Rows[index]["SODate"].ToString().Split()[0],
                            SOQty = Convert.ToDecimal(DS.Tables[0].Rows[index]["SOQty"].ToString()),
                            ReleaseQty = Convert.ToDecimal(DS.Tables[0].Rows[index]["ReleaseQty"].ToString()),
                            DeliveryStatusID = Convert.ToInt32(DS.Tables[0].Rows[index]["DeliveryStatusID"]),
                            IstodayOBD = Convert.ToInt32(DS.Tables[0].Rows[index]["istodayobd"]),
                            WarehouseCode= DS.Tables[0].Rows[index]["WHCode"].ToString(),
                            TotalRecords = DS.Tables[0].Rows[index]["TotalRecords"].ToString(),
                        });

                    }

                    return lst;
                }

                //}
            }

            catch (Exception ex)
            {

            }
            return lst;
        }
        //------------------GET Individual OBD  --------------------------------------------//

        public List<ReleaseIndividualOBD> GetOBDwiseItem(String OBDNumber)
        {
            List<ReleaseIndividualOBD> lst = new List<ReleaseIndividualOBD>();
            try
            {
                string Query = "EXEC [dbo].[sp_GET_OPENOBDWISESLIST] @AccountID=" + cp.AccountID + ", @outboundid ='" + OBDNumber + "'";
                DataSet DS = DB.GetDS(Query, false);
                if (DS.Tables.Count != 0 && DS.Tables[0].Rows.Count != 0)
                {
                    //foreach (DataRow row in DS.Tables[0].Rows)
                    //{
                    for (int index = 0; index < DS.Tables[0].Rows.Count; index++)
                    {
                        lst.Add(new ReleaseIndividualOBD()
                        {
                            ChildKitQuantity = Convert.ToInt32(DS.Tables[0].Rows[index]["ChildKitQuantity"]),
                            KitPlannerID = Convert.ToInt32(DS.Tables[0].Rows[index]["KitPlannerID"]),
                            Ischild = Convert.ToInt32(DS.Tables[0].Rows[index]["Ischild"]),
                            OutboundID = Convert.ToInt32(DS.Tables[0].Rows[index]["OutboundID"]),
                            OBDNumber = DS.Tables[0].Rows[index]["OBDNumber"].ToString(),
                            SONumber = DS.Tables[0].Rows[index]["SONumber"].ToString(),
                            LineNumber = DS.Tables[0].Rows[index]["LineNumber"].ToString(),
                            SODetailsID = Convert.ToInt32(DS.Tables[0].Rows[index]["SODetailsID"].ToString()),
                            DispatchQty = Convert.ToDecimal(DS.Tables[0].Rows[index]["DispatchQty"].ToString()),
                            PendingQty = Convert.ToDecimal(DS.Tables[0].Rows[index]["PendingDeliveryQty"].ToString()),
                            DeliveryQty = Convert.ToDecimal(DS.Tables[0].Rows[index]["DeliveryQty"].ToString()),
                            VolumeinCBM = Convert.ToDecimal(DS.Tables[0].Rows[index]["VolumeInCBM"].ToString()),
                            MWeight = Convert.ToDecimal(DS.Tables[0].Rows[index]["MWeight"].ToString()),
                            TotalVolume = Convert.ToDecimal(DS.Tables[0].Rows[index]["Volume"].ToString()),
                            TotalWeight = Convert.ToDecimal(DS.Tables[0].Rows[index]["Weight"].ToString()),
                            MCode = DS.Tables[0].Rows[index]["MCode"].ToString(),
                            MDescription = DS.Tables[0].Rows[index]["MDescription"].ToString(),
                            SODate = DS.Tables[0].Rows[index]["SODate"].ToString().Split()[0],
                            SOQty = Convert.ToDecimal(DS.Tables[0].Rows[index]["SOQuantity"].ToString()),
                            deliverystatusID = Convert.ToInt32(DS.Tables[0].Rows[index]["DeliveryStatusID"].ToString()),
                            RevertQty = Convert.ToDecimal(DS.Tables[0].Rows[index]["RevertQty"].ToString()),
                            AvailableQty = DS.Tables[0].Rows[index]["AvailableQty"].ToString(),
                            IsSelected = false
                            //  ReleaseQty = Convert.ToDecimal(DS.Tables[0].Rows[index]["ReleaseQty"].ToString()),
                        });

                    }

                    return lst;
                }

                //}
            }

            catch (Exception ex)
            {

            }
            return lst;
        }

        public string RevertOBDLineItem(int IsKitParent, string outboundid, string SODetailsID, string quantity, int userid)
        {
            string query = "";
            if (IsKitParent == 1)
            {
                query = "[SP_OBD_StandardKIT_RREVERT] @OUTBOUNDID=" + outboundid + ",@SODETAILSID=" + SODetailsID + ",@REVERTQTY=" + quantity + ",@CreatedBy=" + userid;
            }
            else
            {
                query = "[dbo].[UPSERT_OBD_LINEITEMWISE_REVERT] @OUTBOUNDID=" + outboundid + ",@SODETAILSID=" + SODetailsID + ",@REVERTQTY=" + quantity + ",@CreatedBy=" + userid;
            }

            return DB.GetSqlS(query);
            // return "";
        }

        public string UpsertPickItem(DeliveryPickNote Pickdata, int userid, int Qty, int VLPDID, string containerid, int TransferRequestID = 0)
        {
            try
            {
                if (containerid == "")
                {
                    containerid = "0";
                }
                if ((DB.GetSqlN("select count(*) as N from OBD_VLPD_PickedItems where  IsDeleted = 0  AND IsActive = 1 AND  VLPDID=" + VLPDID) > 0))
                {
                    int IsCartonPicking = DB.GetSqlN("select count(*) as N from OBD_VLPD_PickedItems WHERE IsDeleted = 0  AND IsActive = 1  and ToCartonID!=0 and ToCartonID is not null AND VLPDID = " + VLPDID);
                    if (IsCartonPicking == 0 && containerid != "0")
                    {
                        return "-333";
                    }
                    else if (IsCartonPicking != 0 && containerid == "0")
                    {
                        return "-444";
                    }

                }

                if (Pickdata.MfgDate != "")
                {
                    Pickdata.MfgDate = DateTime.ParseExact(Pickdata.MfgDate.Replace("/", "-"), "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }
                if (Pickdata.ExpDate != "")
                {
                    Pickdata.ExpDate = DateTime.ParseExact(Pickdata.ExpDate.Replace("/", "-"), "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }
                if (containerid == "")
                {
                    containerid = "0";
                }


                StringBuilder dmlStatement = new StringBuilder();
                dmlStatement.Append("DECLARE @NewVLPDID_ID int");
                dmlStatement.Append(" EXEC  [dbo].[UPSERT_VLPD_PICKEDINFO_NEW] ");
                dmlStatement.Append("@MaterialMasterID =" + Pickdata.MaterialMasterID + ",");
                dmlStatement.Append("@AssignID=" + Pickdata.AssignID + ",");
                dmlStatement.Append("@PickQty=" + Qty + ",");
                dmlStatement.Append("@LocationID=" + Pickdata.LocationID + ",");
                dmlStatement.Append("@CartonID=" + Pickdata.CartonID + ",");
                dmlStatement.Append("@MfgDate=" + (DB.DateQuote(Pickdata.MfgDate)) + ",");
                dmlStatement.Append("@ExpDate=" + (DB.DateQuote(Pickdata.ExpDate)) + ",");
                dmlStatement.Append("@SerialNo=" + (DB.SQuote(Pickdata.SerialNo)) + ",");
                dmlStatement.Append("@BatchNo=" + (DB.SQuote(Pickdata.BatchNo)) + ",");
                dmlStatement.Append("@MRP=" + (DB.SQuote(Pickdata.MRP)) + ",");
                dmlStatement.Append("@VLPDID=" + VLPDID + ",");
                dmlStatement.Append("@TransferRequestID=" + TransferRequestID + ","); //Added By RajuNaidu
                dmlStatement.Append("@ProjectRefNo=" + (DB.SQuote(Pickdata.ProjectRefNo)) + ",");
                dmlStatement.Append("@CreatedBy=" + userid + ",");
                dmlStatement.Append("@ToCartonCode=" + containerid + ",");
                dmlStatement.Append("@OutboundID=" + Pickdata.OutboundID + ",");
                dmlStatement.Append("@SODetailsID=" + Pickdata.SODetailsID + ",");
                dmlStatement.Append("@StorageLocationID=" + Pickdata.StorageLocationID + ",");
                dmlStatement.Append("@VLPDAssignId=" + Pickdata.VLPDAssignId + ",");
                dmlStatement.Append("@NewVLPDID=@NewVLPDID_ID OUTPUT select @NewVLPDID_ID AS N;");
                int Result = DB.GetSqlN(dmlStatement.ToString());


                try
                {
                    DB.ExecuteSQL("EXEC [dbo].[sp_INV_SetFastMovementLocsFulfillOrders] @MaterialMasterID = " + Pickdata.MaterialMasterID + ", @LocationID = " + Pickdata.LocationID + "");
                }
                catch (Exception ex)
                {

                }
                //  DB.ExecuteSQL(dmlStatement.ToString());
                return Result.ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //XML Serialization for Release
        public ReleaseoutboundResult UpsertReleaseItem(List<ReleaseIndividualOBD> GetItem, int VLPDPicking)
        {
            int data = 0;
            ReleaseoutboundResult rst = new ReleaseoutboundResult();
            List<ReleaseIndividualOBD> checkdata = null;
            //checkdata = GetItem.FindAll(item => (item.MCode == "" || item.MCode == null)).ToList(); 
            string xml = null;
            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            string s = string.Join("", GetItem);
            var settings = new XmlWriterSettings();
            using (StringWriter sw = new StringWriter())
            {
                try
                {
                    XmlSerializer serialiser = new XmlSerializer(typeof(List<ReleaseIndividualOBD>));
                    serialiser.Serialize(sw, GetItem, emptyNamepsaces);
                    xml = sw.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", " ");
                }
                catch (Exception e)
                {
                    throw e;
                }
                string Query = "Exec  [dbo].[USP_TRN_AllocateAndReserveInventoryForOBDandVLPD_new] @IsVLPDPicking=" + VLPDPicking + ",@PartialFullfilmentXML='" + xml + "',@User=" + cp.UserID + " ";
                DataSet ds = DB.GetDS(Query, false);
                if (ds.Tables[0].Rows[0]["N"].ToString() == "-1")
                {
                    rst.Status = -1;

                }
                else
                {
                    rst.Status = 1;
                }
                rst.ResultData = DataTableToJSONWithJSONNet(ds.Tables[1]);
            }
            return rst;
        }

        public static string DataTableToJSONWithJSONNet(DataTable table)
        {


            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }
        public string UpsertReleaseItem(int OBDID, int userid)
        {
            try
            {

                StringBuilder dmlStatement = new StringBuilder();
                dmlStatement.Append(" EXEC  [dbo].[USP_TRN_AllocateAndReserveInventoryForOBDandVLPD_nEW] ");
                dmlStatement.Append("@OutboundID =" + OBDID + ",");
                dmlStatement.Append("@User=" + userid);
                //int Result = DB.GetSqlN(dmlStatement.ToString());
                DB.ExecuteSQL(dmlStatement.ToString());
                return "1";
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public string UpsertPickUpInitiate(int vlpdid)
        {
            try
            {

                StringBuilder dmlStatement = new StringBuilder();
                dmlStatement.Append(" EXEC  [dbo].[OBD_VLPDStatus_Update]");
                dmlStatement.Append("@VLPDID =" + vlpdid);


                //int Result = DB.GetSqlN(dmlStatement.ToString());
                DB.ExecuteSQL(dmlStatement.ToString());
                string Query = " EXEC [dbo].[GET_LocationForDeliveryPickNote] @VLPD =" + vlpdid + " ";
                string LocationStrg = DB.GetSqlS(Query);

                string loc = MRLWMSC21Common.CommonLogic.LocationStr(LocationStrg, vlpdid.ToString(), "0");
                return "1";
            }
            catch (Exception e)
            {
                // throw e;
                return "";
            }
        }

        //--------------- added by durga for getting OBD Details For revert ---------------------//
        public List<RevertOutboundData> GetOutBoundDetailsDataForRevert(RevertSearch obj, int Accountid)
        {
            try
            {

                List<RevertOutboundData> listobd = new List<RevertOutboundData>();
                string Query = "EXEC [SP_GetOBDDetailsForOBDRevert] @AccountId=" + Accountid + ",@OBDNumber =" + DB.SQuote(obj.OBDNO) + ", @RevertType=" + obj.RevertType + ",@MFGDate=" + DB.SQuote(obj.MFGDate) + ",@EXPDate=" + DB.SQuote(obj.EXPDate) + ",@SNO=" + DB.SQuote(obj.SNO) + ",@BatchNo=" + DB.SQuote(obj.BatchNo) + ",@ProjectRefNo=" + DB.SQuote(obj.ProjectRefNo) + ", @TenantID=" + obj.TenantID+",@USERID ="+cp.UserID;
                DataSet DS = DB.GetDS(Query, false);
                if (DS.Tables.Count != 0 && DS.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        if (obj.RevertType == 3)
                        {
                            listobd.Add(new RevertOutboundData()
                            {
                                OutboundID = Convert.ToInt32(row["OutboundID"].ToString()),
                                OBDNumber = row["OBDNumber"].ToString(),
                                OBDDate = row["OBDDate"].ToString(),
                                SONumber = row["SONumber"].ToString(),
                                DocumentType = row["DocumentType"].ToString(),
                                VLPDNumber = row["VLPDNumber"].ToString(),
                                PGIDoneName = row["PGIDonename"].ToString(),
                                PGIDate = row["pgidate"].ToString(),
                                Customer = row["CustomerName"].ToString()


                            });
                        }
                        else
                        {
                            listobd.Add(new RevertOutboundData()
                            {
                                OutboundID = Convert.ToInt32(row["OutboundID"].ToString()),
                                OBDNumber = row["OBDNumber"].ToString(),
                                OBDQty = Convert.ToDecimal(row["OBDQty"]),
                                soqty = Convert.ToDecimal(row["soqty"]),
                                CustPOQuantity = Convert.ToDecimal(row["CustPOQuantity"]),
                                PickedQuantity = Convert.ToDecimal(row["PickedQuantity"]),
                                SODetailsID = Convert.ToInt32(row["SODetailsID"].ToString()),
                                SOHeaderID = Convert.ToInt32(row["SOHeaderID"].ToString()),
                                MaterialMasterID = Convert.ToInt32(row["MaterialMasterID"].ToString()),
                                MaterialMaster_CustPOUoMID = Convert.ToInt32(row["MaterialMaster_CustPOUoMID"].ToString()),
                                CustPONumber = row["CustPONumber"].ToString(),
                                SONumber = row["SONumber"].ToString(),
                                MCode = row["MCode"].ToString(),
                                MfgDate = row["MfgDate"].ToString(),
                                ExpDate = row["ExpDate"].ToString(),
                                BatchNo = row["BatchNo"].ToString(),
                                SerialNo = row["SerialNo"].ToString(),
                                ProjectRefNo = row["ProjectRefNo"].ToString(),
                                RevertQty = Convert.ToDecimal(row["PickedQuantity"])
                            });
                        }



                    }
                }
                return listobd;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<ErrorcodeParent> GetErrrodes(int OutboundID)
        {
            List<ErrorcodeParent> lst = new List<ErrorcodeParent>();
            string Query = "[dbo].[OBD_Release_ErrorCode] @OutboundID = " + OutboundID + "";
            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables.Count != 0)
            {
                if (DS.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow ROW in DS.Tables[0].Rows)
                    {
                        ErrorcodeParent ECP = new ErrorcodeParent();
                        ECP.MCode = ROW["MCode"].ToString();
                        ECP.ErrorCodeMessage = ROW["ErrorMessage"].ToString();
                        ECP.SONumber = ROW["SONumber"].ToString();
                        ECP.RowID = Convert.ToInt32(ROW["ROWID"]);
                        if (DS.Tables[1].Rows.Count != 0)
                        {
                            ECP.ChildErrorCodes = GetErrorcodechild(DS.Tables[1], ECP.RowID);
                        }
                        ECP.Isselected = false;
                        lst.Add(ECP);
                    }
                }
            }
            return lst;
        }
        public List<ErrorCodeChild> GetErrorcodechild(DataTable td, int RowID)
        {
            List<ErrorCodeChild> lst = new List<ErrorCodeChild>();
            if (td.Rows.Count != 0)
            {
                foreach (DataRow ROW in td.Rows)
                {
                    ErrorCodeChild ECC = new ErrorCodeChild();
                    if (RowID == Convert.ToInt32(ROW["ROWID"]))
                    {
                        ECC.ErrorCodeMessage = ROW["ErrorMessage"].ToString();
                        ECC.BatchNo = ROW["BatchNo"].ToString();
                        ECC.MfgDate = ROW["MfgDate"].ToString();
                        ECC.ExpDate = ROW["ExpDate"].ToString();
                        ECC.SerialNo = ROW["SerialNo"].ToString();
                        ECC.ProjectRefBo = ROW["ProjectRefNo"].ToString();

                        lst.Add(ECC);
                    }
                }
            }
            return lst;
        }

        //--------------- added by durga for revert PGI Deatils on 23/03/2018 ---------------------//
        public string RevertPGI(int outboundid)
        {
            return DB.GetSqlS("[SP_RevertPGIDetails] @Outboundid=" + outboundid);

        }

        public string generatePDFData(List<PackingPDFData> _data)
        {
            string FileName = "";
            try
            {
                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                    {
                        string pdfFile;
                        string subDirectory = "";
                        //Document pdfDoc = new Document(PageSize.A7, 10f, 10f, 10f, 0f);
                        var pgSize = new iTextSharp.text.Rectangle(230, 285);
                        Document pdfDoc = new iTextSharp.text.Document(pgSize, 9f, 9f, 9f, 0f);
                        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                        //PdfWriter writer = PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
                        FileName = "ShippingLabel.pdf";//_" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second + ".pdf";
                        string fileLocation = MRLWMSC21Common.CommonLogic.SafeMapPath("~/mOutbound/PackingSlip/");

                        if (File.Exists(fileLocation + FileName))
                        {
                            try
                            {
                                File.Delete(fileLocation + FileName);
                            }
                            catch (Exception ex)
                            {
                                //Do something
                            }
                        }

                        CreateDirectory(fileLocation);
                        fileLocation += subDirectory;
                        CreateDirectory(fileLocation);

                        pdfFile = fileLocation + FileName;

                        MemoryStream memoryStream = new MemoryStream();
                        FileStream fileStream = new FileStream(pdfFile, FileMode.Create);
                        PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, fileStream);
                        pdfDoc.Open();
                        if (_data.Count > 0)
                        {
                            foreach (var dt in _data)
                            {
                                DataSet ds = DB.GetDS("SELECT * FROM OBD_Outbound WHERE IsActive=1 AND IsDeleted=0 AND OutboundID =" + dt.OutboundID, false);
                                if (ds.Tables != null)
                                {
                                    string htmlData = "";
                                    string SONUmber = ""; string Address = ""; string SODate = ""; string Notes = ""; string CusName = ""; string MobileNo = ""; string ZipCode = "";
                                    int SOHeaderID = DB.GetSqlN("SELECT TOP 1 SOHeaderID AS N FROM OBD_Outbound_ORD_CustomerPO WHERE IsActive=1 AND IsDeleted=0 AND OutboundID=" + dt.OutboundID);
                                    if (SOHeaderID != 0)
                                    {
                                        DataSet soData = DB.GetDS("SELECT SONumber,FORMAT(SODate ,'dd-MMM-yyyy') SODate FROM ORD_SOHeader WHERE IsActive=1 AND IsDeleted=0 AND SOHeaderID =" + SOHeaderID, false);
                                        if (soData != null)
                                        {
                                            if (soData.Tables[0].Rows.Count > 0)
                                            {
                                                SONUmber = soData.Tables[0].Rows[0]["SONumber"].ToString();
                                                SODate = soData.Tables[0].Rows[0]["SODate"].ToString();
                                            }
                                        }
                                        int CustomerID = DB.GetSqlN("SELECT CustomerID AS N FROM ORD_SOHeader WHERE IsActive=1 AND IsDeleted=0 AND SOHeaderID =" + SOHeaderID);
                                        DataSet data = DB.GetDS("EXEC [sp_OBD_GetDeliveryNote_Header] @OutboundID=" + dt.OutboundID, false);
                                        if (data.Tables != null)
                                        {
                                            CusName = data.Tables[0].Rows[0]["CustomerName"].ToString() == "" || data.Tables[0].Rows[0]["CustomerName"].ToString() == null ? "" : data.Tables[0].Rows[0]["CustomerName"].ToString() + " - ";
                                            Address = data.Tables[0].Rows[0]["Address1"].ToString() == "" || data.Tables[0].Rows[0]["Address1"].ToString() == null ? "" : data.Tables[0].Rows[0]["Address1"].ToString();
                                            MobileNo = data.Tables[0].Rows[0]["Phone1"].ToString() == "" || data.Tables[0].Rows[0]["Phone1"].ToString() == null ? "" : data.Tables[0].Rows[0]["Phone1"].ToString();
                                            ZipCode = data.Tables[0].Rows[0]["Zip"].ToString() == "" || data.Tables[0].Rows[0]["Zip"].ToString() == null ? "" : "<b>Kode Pos :</b> " + data.Tables[0].Rows[0]["Zip"].ToString();
                                            Address = Address.Substring(0, Math.Min(200, Address.Length));
                                        }
                                    }
                                    string AWBNo = ""; string Date = ""; string Barcode = ""; string Courier = ""; string Qrcode = "";
                                    AWBNo = ds.Tables[0].Rows[0]["AWBNo"].ToString() == "" || ds.Tables[0].Rows[0]["AWBNo"].ToString() == null || ds.Tables[0].Rows[0]["AWBNo"].ToString() == "0" ? "" : ds.Tables[0].Rows[0]["AWBNo"].ToString();
                                    Notes = ds.Tables[0].Rows[0]["Notes"].ToString() == "" || ds.Tables[0].Rows[0]["Notes"].ToString() == null ? "" : " <b>Note :</b> " + ds.Tables[0].Rows[0]["Notes"].ToString();
                                    Date = SODate == "" || SODate == null ? "" : SODate; //ds.Tables[0].Rows[0]["DueDate"].ToString() == "" || ds.Tables[0].Rows[0]["DueDate"].ToString() == null ? "" : Convert.ToDateTime(ds.Tables[0].Rows[0]["DueDate"].ToString().Trim()).ToString("dd-MMM-yyyy");                                    
                                    Barcode = AWBNo == "" || AWBNo == null ? "" : "<img width='165px' height='30px' style='text-align:right;'src='" + HttpContext.Current.Request.Url.Scheme + "://localhost/" + HttpContext.Current.Request.ApplicationPath + "/mInbound/Code39Handler.ashx?code=" + AWBNo + "'/> ";
                                    Courier = ds.Tables[0].Rows[0]["Courier"].ToString() == "" || ds.Tables[0].Rows[0]["Courier"].ToString() == null ? "" : ds.Tables[0].Rows[0]["Courier"].ToString();
                                    Qrcode = SONUmber == "" || SONUmber == null ? "" : "<img height='65px' src='" + HttpContext.Current.Request.Url.Scheme + "://localhost/" + HttpContext.Current.Request.ApplicationPath + "/mInbound/Code39Handler.ashx?code=" + SONUmber.ToString() + "DeliveryNote'/>";

                                    Notes = Notes.Substring(0, Math.Min(100, Notes.Length)); CusName = CusName.Substring(0, Math.Min(53, CusName.Length));
                                    htmlData += "<table style='font-size:7px;color:#000000;' cellspacing='0' cellpadding='2'>";
                                    htmlData += "<tr><td style='text-align:center;font-size:13px;font-weight:bold;' bgcolor='#FFF' color='#000' border='1'>" + dt.TenantCode + "</td><td colspan='3' style='text-align:right;font-size:8px;' width='100%'>" + Barcode + "<b>AWB No. : </b>" + AWBNo + "</td></td></tr>";
                                    htmlData += "<tr><td style='text-align:right;font-size:8px;' colspan='4'>" + Date + " <br/> <b>" + Courier + "</b><p style='text-align:left;vertical-align:top;font-size:9px;'><b>Order : </b>" + SONUmber + "</p></td></tr>";
                                    //htmlData += "<tr><td style='text-align:left;vertical-align:top;font-size:9px;' colspan='4'><b>Order : </b>" + SONUmber + "</td></tr>";
                                    htmlData += "<tr><td style='text-align:left;' colspan='4'><b>Ship To :  </b> <br/> " + CusName + MobileNo + "<br/>" + Address + "<br/>" + ZipCode + "<br/>" + Notes + "</td></tr>";
                                    htmlData += "<tr><td style='text-align:left;' colspan='2'>" + Qrcode + "</td><td style='text-align:right !important;' align='right' colspan='2'><span style='color:#1a40ba !important;font-size:10px;'><b>Fulfilled By : </b></span/><p></p><br/><img width='85px' height='20px' src='" + HttpContext.Current.Request.Url.Scheme + "://localhost/" + HttpContext.Current.Request.ApplicationPath + "/Images/inventrax.jpg'/></td></tr>";
                                    htmlData += "</table>";
                                    pdfDoc.NewPage();
                                    StringReader sr = new StringReader(htmlData.ToString());
                                    htmlparser.Parse(sr);
                                }
                            }
                        }
                        else
                        {
                            FileName = "No Data Found";
                        }
                        pdfDoc.Close();
                        fileStream.Close();

                        //HttpContext.Current.Response.ContentType = "application/pdf";
                        //HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".pdf");
                        //HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        //HttpContext.Current.Response.Write(pdfDoc);
                        //HttpContext.Current.Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error : " + ex;
            }

            return FileName;
        }

        public string generatePDFDataForServer(List<PackingPDFData> _data)
        {
            string FileName = "";
            try
            {
                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                    {
                        //Document pdfDoc = new Document(PageSize.A7, 10f, 10f, 10f, 0f);
                        string file = DB.GetSqlS("SELECT LogoPath AS S FROM GEN_Account WHERE AccountID=" + cp.AccountID);
                        var pgSize = new iTextSharp.text.Rectangle(230, 285);
                        Document pdfDoc = new iTextSharp.text.Document(pgSize, 9f, 9f, 9f, 0f);
                        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
                        pdfDoc.Open();
                        if (_data.Count > 0)
                        {
                            foreach (var dt in _data)
                            {
                                DataSet ds = DB.GetDS("SELECT * FROM OBD_Outbound WHERE IsActive=1 AND IsDeleted=0 AND OutboundID =" + dt.OutboundID, false);
                                if (ds.Tables != null)
                                {
                                    string htmlData = "";
                                    string SONUmber = ""; string Address = ""; string SODate = ""; string Notes = ""; string CusName = ""; string MobileNo = ""; string ZipCode = "";
                                    int SOHeaderID = DB.GetSqlN("SELECT TOP 1 SOHeaderID AS N FROM OBD_Outbound_ORD_CustomerPO WHERE IsActive=1 AND IsDeleted=0 AND OutboundID=" + dt.OutboundID);
                                    if (SOHeaderID != 0)
                                    {
                                        DataSet soData = DB.GetDS("SELECT SONumber,FORMAT(SODate ,'dd-MMM-yyyy') SODate FROM ORD_SOHeader WHERE IsActive=1 AND IsDeleted=0 AND SOHeaderID =" + SOHeaderID, false);
                                        if (soData != null)
                                        {
                                            if (soData.Tables[0].Rows.Count > 0)
                                            {
                                                SONUmber = soData.Tables[0].Rows[0]["SONumber"].ToString();
                                                SODate = soData.Tables[0].Rows[0]["SODate"].ToString();
                                            }
                                        }
                                        int CustomerID = DB.GetSqlN("SELECT CustomerID AS N FROM ORD_SOHeader WHERE IsActive=1 AND IsDeleted=0 AND SOHeaderID =" + SOHeaderID);
                                        DataSet data = DB.GetDS("EXEC [sp_OBD_GetDeliveryNote_Header] @OutboundID=" + dt.OutboundID, false);
                                        if (data.Tables != null)
                                        {
                                            CusName = data.Tables[0].Rows[0]["CustomerName"].ToString() == "" || data.Tables[0].Rows[0]["CustomerName"].ToString() == null ? "" : data.Tables[0].Rows[0]["CustomerName"].ToString() + " - ";
                                            Address = data.Tables[0].Rows[0]["Address1"].ToString() == "" || data.Tables[0].Rows[0]["Address1"].ToString() == null ? "" : data.Tables[0].Rows[0]["Address1"].ToString();
                                            MobileNo = data.Tables[0].Rows[0]["Phone1"].ToString() == "" || data.Tables[0].Rows[0]["Phone1"].ToString() == null ? "" : data.Tables[0].Rows[0]["Phone1"].ToString();
                                            ZipCode = data.Tables[0].Rows[0]["Zip"].ToString() == "" || data.Tables[0].Rows[0]["Zip"].ToString() == null ? "" : "<b>Kode Pos :</b> " + data.Tables[0].Rows[0]["Zip"].ToString();
                                            Address = Address.Substring(0, Math.Min(200, Address.Length));
                                        }
                                    }
                                    string AWBNo = ""; string Date = ""; string Barcode = ""; string Courier = ""; string Qrcode = "";
                                    AWBNo = ds.Tables[0].Rows[0]["AWBNo"].ToString() == "" || ds.Tables[0].Rows[0]["AWBNo"].ToString() == null || ds.Tables[0].Rows[0]["AWBNo"].ToString() == "0" ? "" : ds.Tables[0].Rows[0]["AWBNo"].ToString();
                                    Notes = ds.Tables[0].Rows[0]["Notes"].ToString() == "" || ds.Tables[0].Rows[0]["Notes"].ToString() == null ? "" : " <b>Note :</b> " + ds.Tables[0].Rows[0]["Notes"].ToString();
                                    Date = SODate == "" || SODate == null ? "" : SODate; //ds.Tables[0].Rows[0]["DueDate"].ToString() == "" || ds.Tables[0].Rows[0]["DueDate"].ToString() == null ? "" : Convert.ToDateTime(ds.Tables[0].Rows[0]["DueDate"].ToString().Trim()).ToString("dd-MMM-yyyy");                                    
                                    Barcode = AWBNo == "" || AWBNo == null ? "" : "<img width='165px' height='30px' style='text-align:right;'src='" + HttpContext.Current.Request.Url.Scheme + "://localhost/" + HttpContext.Current.Request.ApplicationPath + "/mInbound/Code39Handler.ashx?code=" + AWBNo + "'/> ";
                                    Courier = ds.Tables[0].Rows[0]["Courier"].ToString() == "" || ds.Tables[0].Rows[0]["Courier"].ToString() == null ? "" : ds.Tables[0].Rows[0]["Courier"].ToString();
                                    Qrcode = SONUmber == "" || SONUmber == null ? "" : "<img height='65px' src='" + HttpContext.Current.Request.Url.Scheme + "://localhost/" + HttpContext.Current.Request.ApplicationPath + "/mInbound/Code39Handler.ashx?code=" + SONUmber.ToString() + "DeliveryNote'/>";

                                    Notes = Notes.Substring(0, Math.Min(100, Notes.Length)); CusName = CusName.Substring(0, Math.Min(53, CusName.Length));
                                    htmlData += "<table style='font-size:7px;color:#000000;' cellspacing='0' cellpadding='2'>";
                                    htmlData += "<tr><td style='text-align:center;font-size:13px;font-weight:bold;' bgcolor='#FFF' color='#000' border='1'>" + dt.TenantCode + "</td><td colspan='3' style='text-align:right;font-size:8px;' width='100%'>" + Barcode + "<b>AWB No. : </b>" + AWBNo + "</td></td></tr>";
                                    htmlData += "<tr><td style='text-align:right;font-size:8px;' colspan='4'>" + Date + " <br/> <b>" + Courier + "</b><p style='text-align:left;vertical-align:top;font-size:9px;'><b>Order : </b>" + SONUmber + "</p></td></tr>";
                                    //htmlData += "<tr><td style='text-align:left;vertical-align:top;font-size:9px;' colspan='4'><b>Order : </b>" + SONUmber + "</td></tr>";
                                    htmlData += "<tr><td style='text-align:left;' colspan='4'><b>Ship To :  </b> <br/> " + CusName + MobileNo + "<br/>" + Address + "<br/>" + ZipCode + "<br/>" + Notes + "</td></tr>";
                                    htmlData += "<tr><td style='text-align:left;' colspan='2'>" + Qrcode + "</td><td style='text-align:right !important;' align='right' colspan='2'><span style='color:#1a40ba !important;font-size:10px;'><b>Fulfilled By : </b></span/><p></p><br/><img width='75px' height='20px' src='" + HttpContext.Current.Request.Url.Scheme + "://localhost/" + HttpContext.Current.Request.ApplicationPath + "/TPL/AccountLogos/" + file + "'/></td></tr>";
                                    htmlData += "</table>";
                                    pdfDoc.NewPage();
                                    StringReader sr = new StringReader(htmlData.ToString());
                                    htmlparser.Parse(sr);
                                }
                            }
                        }
                        FileName = "ShippingLabel";//_" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second;
                        pdfDoc.Close();
                        HttpContext.Current.Response.ContentType = "application/pdf";
                        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".pdf");
                        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        HttpContext.Current.Response.Write(pdfDoc);
                        HttpContext.Current.Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error : " + ex;
            }

            return "";
        }
        private bool CreateDirectory(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            return true;
        }

    }



    //------------------------ created general class by Kishore on 03/01/2017----------------------//
    public class PickNoteInfo
    {
        public int MaterialMasterID { set; get; }
        //public string MCode { set; get; }
        //public string MDescription { get; set; }
        public decimal Quantity { get; set; }
        public decimal AssiginQty { get; set; }
        public decimal PickedQty { get; set; }
        public string Location { get; set; }
        public int LocationID { get; set; }
        public string MfgDate { get; set; }
        public string ExpDate { get; set; }
        public string BatchNo { get; set; }
        public string SerialNo { get; set; }
        public string ProjectRefNo { get; set; }
        //public string Assignedquantity { get; set; }
        public decimal PendingQty { get; set; }
        public int CartonID { get; set; }

        public int AssignID { set; get; }
        public string CartonCode { get; set; }

    }
    public class SavePickNoteInfo
    {
        //public int VLPDID { set; get; }     
        public int VLPDAssignID { get; set; }
        public int MaterialMasterID { get; set; }
        public decimal PickedQty { get; set; }
        public int CartonID { get; set; }
        public int LocationID { get; set; }
        public int AssignID { set; get; }
        public int AssiginQty { get; set; }
        public int Quantity { get; set; }
        public string CartonCode { get; set; }
        public int RemaingQty { get; set; }
        public string MfgDate { get; set; }
        public string ExpDate { get; set; }
        public string BatchNo { get; set; }
        public string SerialNo { get; set; }
        public string ProjectRefNo { get; set; }
    }

    public class DeliveryPickNote
    {
        public int MaterialMasterID { set; get; }
        public string MCode { set; get; }
        public string MDescription { get; set; }
        public decimal Quantity { get; set; }
        public decimal AssiginQty { get; set; }
        public decimal PickedQty { get; set; }
        public string Location { get; set; }
        public int LocationID { get; set; }
        public string MfgDate { get; set; }
        public string ExpDate { get; set; }
        public string BatchNo { get; set; }
        public string MRP { get; set; }
        public string SerialNo { get; set; }
        public string ProjectRefNo { get; set; }
        //public string Assignedquantity { get; set; }
        public decimal PendingQty { get; set; }
        public int CartonID { get; set; }
        //public decimal Volume { get; set; }
        //public decimal Weight { get; set; }
        public int AssignID { set; get; }
        public string CartonCode { get; set; }
        public string WHCode { get; set; }
        public string VLPDNumber { get; set; }
        public int OutboundID { get; set; }
        public int SODetailsID { get; set; }
        public int StorageLocationID { get; set; }
        public int VLPDAssignId { get; set; }
        //public List<PickNoteInfo> oPickNoteInfo { get; set; }
    }
    public class SOListInfo
    {
        public List<SOInfo> soList { set; get; }
    }
    public class SOInfo
    {
        public List<KeyValue> keyValueList { set; get; }
    }
    public class KeyValue
    {
        public string Key { set; get; }
        public string Value { set; get; }
    }
    //------------------------ created general class by durga for All dropdowns on 15/11/2017----------------------//
    public class DropDownListData
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    //------------------------ created general class by durga for All dropdowns with Filters on 15/11/2017----------------------//
    public class DropDownListDataFilter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FilterId { get; set; }
    }
    //------------------------ created Revert class by durga for revert outbound on 21/03/2018---------------------//
    public class RevertSearch
    {
        public int RevertType { get; set; }
        public string OBDNO { get; set; }
        public string Mcode { get; set; }
        public string MFGDate { get; set; }
        public string EXPDate { get; set; }
        public string BatchNo { get; set; }
        public string SNO { get; set; }
        public string ProjectRefNo { get; set; }
        public int TenantID { get; set; }
    }
    public class RevertOutboundData
    {
        public int OutboundID { get; set; }
        public string OBDNumber { get; set; }
        public decimal OBDQty { get; set; }
        public decimal soqty { get; set; }
        public decimal CustPOQuantity { get; set; }
        public string CustPONumber { get; set; }
        public string SONumber { get; set; }
        public int SODetailsID { get; set; }
        public int SOHeaderID { get; set; }
        public int MaterialMasterID { get; set; }
        public int MaterialMaster_CustPOUoMID { get; set; }
        public string MCode { get; set; }
        public string MfgDate { get; set; }
        public string ExpDate { get; set; }
        public string BatchNo { get; set; }
        public string SerialNo { get; set; }
        public string ProjectRefNo { get; set; }
        public decimal PickedQuantity { get; set; }
        public string OBDDate { get; set; }
        public string DocumentType { get; set; }
        public string VLPDNumber { get; set; }
        public string PGIDoneName { get; set; }
        public string PGIDate { get; set; }
        public string Customer { get; set; }
        public decimal RevertQty { get; set; }
        public int TenantID { get; set; }
    }
    public class OutboundSerachData
    {
        public int TenantId { get; set; }
        public int WareHouseId { get; set; }
        public int DeliveryTypeId { get; set; }
        public String Sonumbers { get; set; }
        public int PriorityTypeID { get; set; }
    }
    public class GroupOutboundSearchData
    {
        public int TenantId { get; set; }
        public int WareHouseId { get; set; }
        public string CustomerName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    public class Outbound
    {
        public int SNO { get; set; }
        public Boolean Isselected { get; set; }
        public int TenantID { get; set; }
        public int WarehouseID { get; set; }
        public int OutboundID { get; set; }
        public string OBDNumber { get; set; }
        public string OBDDate { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string tenantname { get; set; }
        public string WHCode { get; set; }
        public Decimal Volume { get; set; }
        public Decimal Weight { get; set; }


    }
    public class GroupOBD
    {
        public int SNO { get; set; }
        public int ID { get; set; }
        public string tenantName { get; set; }
        public string WareHouse { get; set; }
        public string GRPNumber { get; set; }
        public string createduser { get; set; }
        public string CreatedDate { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }

    }

    public class SODetails
    {
        public int MaterialMasterID { get; set; }
        public string Mcode { get; set; }
        public string MDescription { get; set; }
        public string SONumber { get; set; }
        public string LineNumber { get; set; }
        public int SODetailsID { get; set; }
        public decimal SoQuantity { get; set; }
        public decimal InProcessQty { get; set; }
        public decimal PendingQty { get; set; }
        public decimal AvailableQty { get; set; }
        public bool Isselected { get; set; }
        public string BatchNo { get; set; }
        public string SerialNo { get; set; }
        public string ExpDate { get; set; }
        public string MfgDate { get; set; }
        public string ProjectRefNo { get; set; }
        public List<MaterialStorageParameters> Msps { set; get; }

    }
    public class MaterialStorageParameters
    {
        public string BatchNo { get; set; }
        public string SerialNo { get; set; }
        public string ExpDate { get; set; }
        public string MfgDate { get; set; }
        public string ProjectRefNo { get; set; }

    }

    public class OBDResult
    {
        public int status { get; set; }
        public List<ReleaseOBD> oOBDlst { get; set; }
    }

    public class ReleaseOBD
    {
        public string TotalRecords { get; set; }
        public int OutboundID { get; set; }
        public int IstodayOBD { get; set; }
        public string OBDRefNo { get; set; }
        public string TenantName { get; set; }
        public string CustomerName { get; set; }
        public string OBDDate { get; set; }
        public string SONumber { get; set; }
        public string SODate { get; set; }
        public decimal SOQty { get; set; }
        public decimal ReleaseQty { get; set; }
        public int DeliveryStatusID { get; set; }
        public decimal RevertQty { get; set; }

        public string WarehouseCode { get; set; }

    }

    //Release Item  public class ReleaseOBD
    public class ReleaseIndividualOBD
    {
        public int OutboundID { get; set; }
        public string SONumber { get; set; }
        public string OBDNumber { get; set; }
        public string SODate { get; set; }
        public string LineNumber { get; set; }
        public string MCode { get; set; }
        public string MDescription { get; set; }
        public decimal SOQty { get; set; }
        //public decimal ReleaseQty { get; set; }
        public int SODetailsID { get; set; }
        public decimal DispatchQty { get; set; }
        public decimal PendingQty { get; set; }
        public decimal DeliveryQty { get; set; }
        public decimal VolumeinCBM { get; set; }
        public decimal MWeight { get; set; }
        public decimal TotalVolume { get; set; }
        public decimal TotalWeight { get; set; }
        public int deliverystatusID { get; set; }
        public decimal RevertQty { get; set; }
        public int Ischild { get; set; }
        public int KitPlannerID { get; set; }
        public int ChildKitQuantity { get; set; }
        public string AvailableQty { get; set; }
        public bool IsSelected { get; set; }
    }
    public class ReleaseoutboundResult
    {
        public int Status { get; set; }
        public string ResultData { get; set; }
    }

    public class ErrorcodeParent
    {
        public string ErrorCode { get; set; }
        public string ErrorCodeMessage { get; set; }
        public string MCode { get; set; }
        public string SONumber { get; set; }
        public int RowID { get; set; }
        public bool Isselected { get; set; }
        public List<ErrorCodeChild> ChildErrorCodes { get; set; }

    }
    public class ErrorCodeChild
    {
        public string ErrorCodeMessage { get; set; }
        public string BatchNo { get; set; }
        public string MfgDate { get; set; }
        public string ExpDate { get; set; }
        public string SerialNo { get; set; }
        public string ProjectRefBo { get; set; }

    }

    public class GroupOBDList
    {
        public string OBDNumber { get; set; }
        public string OBDDate { get; set; }

        public string CustomerName { get; set; }
        public Decimal AssignedQuantity { get; set; }


    }


}