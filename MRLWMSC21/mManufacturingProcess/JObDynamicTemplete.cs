using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using MRLWMSC21Common;

namespace MRLWMSC21.mManufacturingProcess
{
    public class JObDynamicTemplete : ITemplate
    {
        private String columnName;
        private ListItemType itemType;
        private int materialCount;
        private decimal bomRatio;
        private bool ismaterialheader;

        public JObDynamicTemplete(ListItemType ItemType , String ColumnName,int MaterialCount)
        {
            columnName = ColumnName;
            materialCount = MaterialCount;
            itemType = ItemType;
        }

        public JObDynamicTemplete(ListItemType ItemType, String ColumnName)
        {
            columnName = ColumnName;
            itemType = ItemType;
        }
        public JObDynamicTemplete(ListItemType ItemType, String ColumnName,bool isMaterialHeader)
        {
            columnName = ColumnName;
            itemType = ItemType;
            ismaterialheader = isMaterialHeader;
        }
        public JObDynamicTemplete(ListItemType ItemType, String ColumnName,decimal BoMRatio, bool isMaterialHeader)
        {
            columnName = ColumnName;
            itemType = ItemType;
            ismaterialheader = isMaterialHeader;
            bomRatio = BoMRatio;
        }

        public void InstantiateIn(System.Web.UI.Control Container)
        {
            switch (itemType)
            {
                case ListItemType.Header:
                    {
                        if (ismaterialheader)
                        {
                            Label lbPartNumber = new Label();
                            lbPartNumber.Text = columnName;
                            lbPartNumber.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF8000");
                            Container.Controls.Add(lbPartNumber);

                            Container.Controls.Add(new LiteralControl("<br>"));
                            
                            Label lbBoMRatio = new Label();
                            lbBoMRatio.Text = "[" + bomRatio.ToString() + "]";
                            lbBoMRatio.Font.Size = 12;
                            lbBoMRatio.ForeColor = System.Drawing.ColorTranslator.FromHtml("#2E2EFE");
                            Container.Controls.Add(lbBoMRatio);
                        }
                        else
                        {
                            Literal ltliteral = new Literal();
                            ltliteral.Text = columnName;
                        }
                        break;

                    }
                case ListItemType.Item:
                    {
                        if (columnName == "MCode")
                        {
                            Literal ltMCode = new Literal();
                            ltMCode.ID = "MCode" + materialCount;
                            ltMCode.DataBinding += new EventHandler(OnDataBinding);
                            Container.Controls.Add(ltMCode);

                            /*  HiddenField ltMaterialMasterID = new HiddenField();
                              ltMaterialMasterID.ID = "MaterialMasterID" + materialCount;
                              ltMaterialMasterID.DataBinding += new EventHandler(OnDataBinding);
                              Container.Controls.Add(ltMaterialMasterID);*/
                        }
                        else if (columnName == "BatchNo")
                        {
                            Label lbBatchNo = new Label();
                            lbBatchNo.ID = "BatchNo" + materialCount;
                            lbBatchNo.DataBinding += new EventHandler(OnDataBinding);
                            Container.Controls.Add(lbBatchNo);

                            HiddenField ltTotalQty = new HiddenField();
                            ltTotalQty.ID = "TotalQty" + materialCount;
                            ltTotalQty.DataBinding += new EventHandler(OnDataBinding);
                            Container.Controls.Add(ltTotalQty);

                            HiddenField ltBoMQty = new HiddenField();
                            ltBoMQty.ID = "BoMQty" + materialCount;
                            ltBoMQty.DataBinding += new EventHandler(OnDataBinding);
                            Container.Controls.Add(ltBoMQty);

                            HiddenField ltRequireQty = new HiddenField();
                            ltRequireQty.ID = "ReqQty" + materialCount;
                            ltRequireQty.DataBinding += new EventHandler(OnDataBinding);
                            Container.Controls.Add(ltRequireQty);

                            HiddenField ltMaterialMasterID = new HiddenField();
                            ltMaterialMasterID.ID = "MaterialMasterID" + materialCount;
                            ltMaterialMasterID.DataBinding += new EventHandler(OnDataBinding);
                            Container.Controls.Add(ltMaterialMasterID);

                            Container.Controls.Add(new LiteralControl("<br>"));
                            Label lbTotalQty = new Label();
                            lbTotalQty.ID = "lbTotalQty" + materialCount;
                            lbTotalQty.Font.Size = 10;
                            lbTotalQty.ForeColor = System.Drawing.ColorTranslator.FromHtml("#5882FA");
                            lbTotalQty.DataBinding += new EventHandler(OnDataBinding);
                            Container.Controls.Add(lbTotalQty);
                        }
                        else if (columnName == "PossibleQty")
                        {
                            Literal ltPossibleQty = new Literal();
                            ltPossibleQty.ID = "PossibleQty";
                            ltPossibleQty.DataBinding += new EventHandler(OnDataBinding);
                            Container.Controls.Add(ltPossibleQty);
                        }
                        else if (columnName == "CheckBox")
                        {
                            CheckBox chkCheck = new CheckBox();
                            chkCheck.ID = "chkCheck";
                            chkCheck.Attributes.Add("onClick", "CheckCheckBox(this)");
                            chkCheck.ClientIDMode = ClientIDMode.Static;
                            Container.Controls.Add(chkCheck);
                        }
                        else if (columnName == "TextBox")
                        {
                            TextBox txtQty = new TextBox();
                            txtQty.Width = 60;
                            txtQty.ClientIDMode = ClientIDMode.Static;
                            txtQty.ID = "txtQty";
                            txtQty.Attributes.Add("onKeyPress", "return checkDec(this,event)");
                            txtQty.Attributes.Add("onblur", "CheckTextBoxValue(this)");
                            Container.Controls.Add(txtQty);
                        }
                        break;
                    }
                
            }
        }

        private void OnDataBinding(object sender, EventArgs e)
        {
            object bound_value_obj = null;
            Control ctrl = (Control)sender;
            IDataItemContainer data_item_container = (IDataItemContainer)ctrl.NamingContainer;
            //bound_value_obj = DataBinder.Eval(data_item_container.DataItem, FieldName);
           if(columnName=="MCode")
            {
                if (ctrl.ID.Substring(0, 3) == "MCo")
                {
                    bound_value_obj = DataBinder.Eval(data_item_container.DataItem, "MCode" + materialCount);
                    Literal ltMCode = (Literal)sender;
                    ltMCode.Text = bound_value_obj.ToString();
                }
                else
                {
                    bound_value_obj = DataBinder.Eval(data_item_container.DataItem, "MaterialMasterID" + materialCount);
                    HiddenField ltMaterialMaster = (HiddenField)sender;
                    ltMaterialMaster.Value = bound_value_obj.ToString();
                }
            }
           else if (columnName == "BatchNo")
           {
               if (ctrl.ID.Substring(0, 3) == "Bat")
               {
                   bound_value_obj = DataBinder.Eval(data_item_container.DataItem, "BatchNo" + materialCount);
                   Label ltMCode = (Label)sender;
                   ltMCode.Text = bound_value_obj.ToString();
               }
               else if (ctrl.ID.Substring(0, 3) == "Tot")
               {
                   bound_value_obj = DataBinder.Eval(data_item_container.DataItem, "TotalQty" + materialCount);
                   HiddenField hidTotalQty = (HiddenField)sender;
                   hidTotalQty.Value = bound_value_obj.ToString();
               }
               else if (ctrl.ID.Substring(0, 3) == "BoM")
               {
                   bound_value_obj = DataBinder.Eval(data_item_container.DataItem, "BoMQty" + materialCount);
                   HiddenField hidBoMQty = (HiddenField)sender;
                   hidBoMQty.Value = bound_value_obj.ToString();
               }
               else if (ctrl.ID.Substring(0, 3) == "Req")
               {
                   //bound_value_obj = DataBinder.Eval(data_item_container.DataItem, "TotalQty" + materialCount);
                   HiddenField hidReqQty = (HiddenField)sender;
                   hidReqQty.Value = (Convert.ToDecimal(DataBinder.Eval(data_item_container.DataItem, "BoMQty" + materialCount).ToString()) * Convert.ToDecimal(DataBinder.Eval(data_item_container.DataItem, "PossibleQty").ToString())).ToString();
               }
               else if (ctrl.ID.Substring(0, 3) == "Mat")
               {
                   bound_value_obj = DataBinder.Eval(data_item_container.DataItem, "MaterialMasterID" + materialCount);
                   HiddenField ltMaterialMaster = (HiddenField)sender;
                   ltMaterialMaster.Value = bound_value_obj.ToString();
               }
               else if (ctrl.ID.Substring(0, 3) == "lbT")
               {
                   bound_value_obj = DataBinder.Eval(data_item_container.DataItem, "TotalQty" + materialCount);
                   Label ltMaterialMaster = (Label)sender;
                   ltMaterialMaster.Text ="["+ bound_value_obj.ToString()+"]";
               }
           
           }
           else if (columnName == "PossibleQty")
           {
               bound_value_obj = DataBinder.Eval(data_item_container.DataItem, "PossibleQty");
               Literal ltPossibleQty = (Literal)sender;
               ltPossibleQty.Text = bound_value_obj.ToString();
            }
           
        }

    }
}