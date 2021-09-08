using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using MRLWMSC21Common;
using System.Data;
using System.IO;

namespace MRLWMSC21
{
    public partial class FalconErrorList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["FromDate"] = "";
                ViewState["ToDate"] = "";
                LoadGridData(LoadGridData());
            }
        }

        private DataTable LoadGridData()
        {

            try
            {
                DataSet ds = new DataSet();
                string path = Server.MapPath("~/WMSErrorList.xml");
                if (File.Exists(path))
                    ds.ReadXml(path);
                DataTable dtCloned = null;
                if (ds.Tables.Count!=0)
                {
                    DataTable dt = ds.Tables[0];
                    dtCloned = dt.Clone();
                    dtCloned.Columns[5].DataType = typeof(DateTime);
                    for (int dr = 0; dr < dt.Rows.Count; dr++)
                    {
                        DataRow _dr = dtCloned.NewRow();
                        for (int cc = 0; cc < dt.Rows[dr].ItemArray.Count(); cc++)
                        {
                            if (cc == 5 && dt.Rows[dr][cc].ToString() != "")
                            {
                                _dr[cc] = DateTime.ParseExact(dt.Rows[dr][cc].ToString(), "dd/MM/yyyy HH:mm:ss.fff", null);//This Formate is Stored in CommonLogic when ever we change formate both commonlogin and this page also.
                            }
                            else
                                _dr[cc] = dt.Rows[dr][cc].ToString();
                        }
                        dtCloned.Rows.Add(_dr);
                    }
                }

                ds.Dispose();

                return dtCloned;
            }
            catch (Exception ex)
            {
                resetError("Error while loading", true);
                return null;
            }
        }

        private void LoadGridData(DataTable dt) 
        {
            try
            {
                if (dt != null && dt.Rows.Count != 0)
                {
                    lblCount.Text = "Errors Count [ " + dt.Rows.Count + " ]";
                    if (ViewState["FromDate"].ToString() != "" && ViewState["ToDate"].ToString() != "")
                    {
                        DataTable filteredRows = dt.Clone();
                        var rows = dt.AsEnumerable().Where(r => r.Field<DateTime?>("E_Time").HasValue && r.Field<DateTime?>("E_Time").Value.Date >= DateTime.ParseExact(ViewState["FromDate"].ToString(), "dd/MM/yyyy", null).Date && r.Field<DateTime?>("E_Time").Value.Date <= DateTime.ParseExact(ViewState["ToDate"].ToString(), "dd/MM/yyyy", null).Date);
                        if (rows.Any())
                            filteredRows = rows.CopyToDataTable();
                        gvErrorsList.DataSource = filteredRows;
                    }
                    else
                        gvErrorsList.DataSource = dt;

                    gvErrorsList.DataBind();
                    dt.Dispose();
                }
                else
                {
                    lblCount.Text = "";
                    gvErrorsList.DataSource = null;
                    gvErrorsList.DataBind();
                }
            }
            catch (Exception ex)
            {
                resetError("Error while loading", true);
                return;
            }
        }

        protected void lnkgenerateerror_Click(object sender, EventArgs e)
        {
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {

            try
            {

                string gvIDs = "";
                bool chkBox = false;
                String[] ids;
                foreach (GridViewRow gv in gvErrorsList.Rows)
                {
                    CheckBox deleteChkBxItem = (CheckBox)gv.FindControl("deleteError");
                    if (deleteChkBxItem.Checked)
                    {
                        chkBox = true;
                        // Concatenate GridView items with comma for SQL Delete
                        gvIDs += ((Label)gv.FindControl("RecID")).Text.ToString() + ",";
                    }
                }
                if (chkBox)
                {
                    gvIDs = gvIDs.Substring(0, gvIDs.LastIndexOf(","));
                    ids = gvIDs.Split(',');
                    DataSet ds = new DataSet();
                    ds.ReadXml(Server.MapPath("~/WMSErrorList.xml"));
                    DataRow _dr;
                    foreach (string id in ids)
                    {
                        //ds.Tables[0].Rows.RemoveAt(Convert.ToInt16(id));
                        _dr = ds.Tables[0].Rows[Convert.ToInt16(id)];
                        _dr.Delete();
                    }
                    ds.Tables[0].AcceptChanges();
                    //ds.Tables[0].Rows.RemoveAt(e.RowIndex);
                    ds.WriteXml(Server.MapPath("~/WMSErrorList.xml"));
                    LoadGridData(LoadGridData());
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void lbtndateSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtfromdate.Text != "")
                {
                    ViewState["FromDate"] = txtfromdate.Text;
                }
                else
                    ViewState["FromDate"] = "";

                if (txttodate.Text != "")
                {
                    ViewState["ToDate"] = txttodate.Text;
                }
                else
                    ViewState["ToDate"] = "";
                LoadGridData(LoadGridData());
            }
            catch (Exception ex)
            {
                resetError("Error while searching", true);
                return;
            }
        }

        protected void gvErrorsList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvErrorsList.PageIndex = e.NewPageIndex;
                gvErrorsList.EditIndex = -1;
                LoadGridData(LoadGridData());
            }
            catch (Exception ex) {
                resetError("Error while paging", true);
                return;
            }
        }

        protected void gvErrorsList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
        }
        
        protected void resetError(string error, bool isError)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }

        protected void lnkdelete_Click(object sender, EventArgs e)
        {
            try
            {
                string ErrorRow = ((HiddenField)((GridViewRow)(((LinkButton)sender).Parent.Parent)).FindControl("hifrowid")).Value;
                CommonLogic.DeleteErrorNode(ErrorRow);

                LoadGridData(LoadGridData());
            }
            catch (Exception ex) {
                resetError("Error while deleting node", true);
                return;
            }
        }
    }
}