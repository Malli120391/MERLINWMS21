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

namespace MRLWMSC21Common
{
    public class DynamicallyTemplatedGridViewHandler : ITemplate
    {
        ListItemType ItemType;
        string FieldName;
        string ControlType;
        String Query;
        LinkButton button;
        Literal header_ltrl;
        CheckBox checkbox;
        Literal field_lbl;
        TextBox field_txtbox;
        DropDownList dropdown;
        Boolean RequireValidation = false;
        Boolean enableRequireValidation = true;
        String Groupvalidation;
        RequiredFieldValidator validation;
        RadioButton radioButton;
        EventHandler Handler;
        String Text;
        String DataType;
        String MinTolerance;
        String MaxTolerance;
        EventHandler EditEvent;
        EventHandler UpdateEvent;
        EventHandler CancelEvent;

        //int ColumnType;
        public DynamicallyTemplatedGridViewHandler(ListItemType item_type, LinkButton _button)
        {
            ItemType = item_type;
            button = _button;

        }
        public DynamicallyTemplatedGridViewHandler(ListItemType item_type, EventHandler handler, string field_name, string controlType, String text)
        {
            ItemType = item_type;
            // button = _button;
            FieldName = field_name;
            Handler = handler;
            ControlType = controlType;
            Text = text;
        }


        public DynamicallyTemplatedGridViewHandler(ListItemType item_type, string field_name, string controlType, String dataType)
        {
            ItemType = item_type;
            FieldName = field_name;
            ControlType = controlType;
            DataType = dataType;
        }
        public DynamicallyTemplatedGridViewHandler(ListItemType item_type, string field_name, string controlType)
        {
            ItemType = item_type;
            FieldName = field_name;
            ControlType = controlType;
           
        }


        public DynamicallyTemplatedGridViewHandler(ListItemType item_type, string field_name, string controlType, String dataType, String _Query, String minTolerance,String maxTolerance, String groupvalidation, Boolean enablerequireValidation)
        {
            ItemType = item_type;
            FieldName = field_name;
            ControlType = controlType;
            //ColumnType = columnType;
            Query = _Query;
            RequireValidation = true;
            DataType = dataType;
            enableRequireValidation = enablerequireValidation;
            Groupvalidation = groupvalidation;
            MinTolerance = minTolerance;
            MaxTolerance = maxTolerance;

        }
        public DynamicallyTemplatedGridViewHandler(ListItemType item_type, string field_name, string controlType, String dataType, String _Query, String minTolerance, String maxTolerance)
        {
            ItemType = item_type;
            FieldName = field_name;
            ControlType = controlType;
            //ColumnType = columnType; 
            Query = _Query;
            
            DataType = dataType;
            MinTolerance = minTolerance;
            MaxTolerance = maxTolerance;

        }


        public DynamicallyTemplatedGridViewHandler(ListItemType item_type, string field_name, string controlType, String dataType, String _Query)
        {
            ItemType = item_type;
            FieldName = field_name;
            ControlType = controlType;
            Query = _Query;
            DataType = dataType;
        }


        public DynamicallyTemplatedGridViewHandler(ListItemType item_type, string field_name, string controlType, String dataType, String _Query, String groupvalidation, Boolean enablerequireValidation)
        {
            ItemType = item_type;
            FieldName = field_name;
            ControlType = controlType;
            Query = _Query;
            RequireValidation = true;
            DataType = dataType;
            enableRequireValidation = enablerequireValidation;
            Groupvalidation = groupvalidation;
        }

        public DynamicallyTemplatedGridViewHandler(ListItemType item_type, string controlType, EventHandler EditEvent)
        {
            this.ItemType = item_type;
            this.ControlType = controlType;
            this.EditEvent = EditEvent;
           
        }

        public DynamicallyTemplatedGridViewHandler(ListItemType item_type, string controlType, EventHandler UpdateEvent, EventHandler CancelEvent)
        {
            this.ItemType = item_type;
            this.ControlType = controlType;
            this.UpdateEvent = UpdateEvent;
            this.CancelEvent = CancelEvent;
        }

        public void InstantiateIn(System.Web.UI.Control Container)
        {
            switch (ItemType)
            {
                case ListItemType.Header:
                    {

                        header_ltrl = new Literal
                        {
                            Text = "<b>" + FieldName + "</b>"
                        };

                        Container.Controls.Add(header_ltrl);

                        break;
                    }
                case ListItemType.Item:
                    {
                    switch (ControlType)
                    {
                        case "Command":
                            {
                                    LinkButton EditButton = new LinkButton
                                    {
                                        Text = "<nobr> Edit <img src='../Images/redarrowright.gif' border='0' /></nobr>",
                                        CssClass = "ButnEmpty"
                                    };
                                    EditButton.Click += EditEvent;
                                Container.Controls.Add(EditButton);
                                //ImageButton edit_button = new ImageButton();
                                //edit_button.ID = "edit_button";
                                //edit_button.ImageUrl = "~/images/edit.gif";
                                //edit_button.CommandName = "Edit";
                                ////edit_button.Click += new ImageClickEventHandler(edit_button_Click);
                                //edit_button.ToolTip = "Edit";
                                //Container.Controls.Add(edit_button);

                                //ImageButton delete_button = new ImageButton();
                                //delete_button.ID = "delete_button";
                                //delete_button.ImageUrl = "~/images/delete.gif";
                                //delete_button.CommandName = "Delete";
                                //delete_button.ToolTip = "Delete";
                                //delete_button.OnClientClick = "return confirm('Are you sure to delete the record?')";
                                //Container.Controls.Add(delete_button);

                                ///* Similarly add button for insert.
                                // * It is important to know when 'insert' button is added 
                                // * its CommandName is set to "Edit"  like that of 'edit' button 
                                // * only because we want the GridView enter into Edit mode, 
                                // * and this time we also want the text boxes for corresponding fields empty*/
                                //ImageButton insert_button = new ImageButton();
                                //insert_button.ID = "insert_button";
                                //insert_button.ImageUrl = "~/images/insert.bmp";
                                //insert_button.CommandName = "Edit";
                                //insert_button.ToolTip = "Insert";
                                ////insert_button.Click += new ImageClickEventHandler(insert_button_Click);
                                //Container.Controls.Add(insert_button);
                                break;
                            }
                        case "TextBox":
                            {


                                    TextBox txtTextBox = new TextBox
                                    {
                                        ID = FieldName
                                    };
                                    txtTextBox.DataBinding += new EventHandler(OnDataBinding);
                                txtTextBox.Width = 80;
                                txtTextBox.Attributes.Add("onKeyPress", "return checkDec(this,event)");
                                txtTextBox.Attributes.Add("onblur", "CheckDecimal(this)");
                                // if (RequireValidation)
                                /* if(true)
                                 {
                                     validation = new RequiredFieldValidator();
                                     validation.ValidationGroup = Groupvalidation;
                                     validation.ID = "rfv" + FieldName;
                                     validation.Display = ValidatorDisplay.Dynamic;
                                     validation.ErrorMessage = "*";
                                     validation.Enabled = enableRequireValidation;
                                     validation.ControlToValidate = txtTextBox.ID;
                                     Container.Controls.Add(validation);
                                 }*/
                                Container.Controls.Add(txtTextBox);
                                break;
                            }

                        case "CheckBox":
                            {
                                    checkbox = new CheckBox
                                    {
                                        ID = FieldName
                                    };
                                    checkbox.Attributes.Add("runat", "server");
                                //checkbox.DataBinding += new EventHandler(OnDataBinding);
                                Container.Controls.Add(checkbox);
                                break;
                            }
                        case "RadioButton":
                            {
                                    radioButton = new RadioButton
                                    {
                                        ID = FieldName,
                                        CausesValidation = false
                                    };
                                    radioButton.Attributes.Add("runat", "server");
                                radioButton.Attributes.Add("onclick", "RadioCheck(this);");
                                //radioButton.Checked = true;
                                Container.Controls.Add(radioButton);
                                break;
                            }
                        case "CheckImage":
                            {

                                Image img = new Image();
                                img.DataBinding += new EventHandler(OnDataBinding);
                                Container.Controls.Add(img);

                                break;
                            }
                        case "LinkButton":
                            {
                                Container.Controls.Add(new LiteralControl("<nobr>"));
                                    LinkButton button = new LinkButton
                                    {
                                        ID = "lnk" + FieldName,
                                        Text = Text
                                    };
                                    //button.Click += Handler;
                                    button.Font.Underline = false;
                                // button.DataBinding += new EventHandler(OnDataBinding);
                                Container.Controls.Add(button);
                                    Image img = new Image
                                    {
                                        ImageUrl = "../Images/redarrowright.gif"
                                    };
                                    Container.Controls.Add(new LiteralControl("&nbsp;"));
                                Container.Controls.Add(img);
                                Container.Controls.Add(new LiteralControl("</nobr>"));
                                break;
                            }

                        case "LiteralWithID":
                            {
                                    field_lbl = new Literal
                                    {
                                        ID = FieldName,

                                        Text = String.Empty //we will bind it later through 'OnDataBinding' event
                                    };
                                    field_lbl.DataBinding += new EventHandler(OnDataBinding);
                                Container.Controls.Add(field_lbl);
                                field_lbl = new Literal();
                                if (FieldName == "Plant")
                                    field_lbl.ID = "M" + FieldName + "ID";
                                else
                                    field_lbl.ID = FieldName + "ID";
                                //field_lbl.ID = FieldName;
                                field_lbl.Visible = false;
                                field_lbl.Text = String.Empty; //we will bind it later through 'OnDataBinding' event
                                field_lbl.DataBinding += new EventHandler(OnDataBinding);
                                Container.Controls.Add(field_lbl);
                                break;
                            }

                        default:
                                field_lbl = new Literal
                                {
                                    ID = FieldName,

                                    Text = String.Empty //we will bind it later through 'OnDataBinding' event
                                };
                                field_lbl.DataBinding += new EventHandler(OnDataBinding);
                            Container.Controls.Add(field_lbl);
                            break;

                    }
                    break;
            }
                case ListItemType.EditItem:

                    if (RequireValidation)
                    {
                        validation = new RequiredFieldValidator
                        {
                            ValidationGroup = Groupvalidation,
                            ID = "rfv" + FieldName,
                            Display = ValidatorDisplay.Dynamic,
                            ErrorMessage = "*",
                            Enabled = enableRequireValidation
                        };

                    }

                    if (ControlType == "Command")
                    {
                        Button UpdateButton = new Button
                        {
                            Text = "Update",
                            OnClientClick = "this.disabled = true; this.value = 'Submitting...';",
                            UseSubmitBehavior = false,
                            CssClass = "ButnEmpty"
                        };
                        UpdateButton.Click += UpdateEvent;
                        Container.Controls.Add(UpdateButton);

                        Button CancelButton = new Button
                        {
                            Text = "Cancel",
                            CssClass = "ButnEmpty"
                        };
                        CancelButton.Click += CancelEvent;
                        Container.Controls.Add(CancelButton);

                        //ImageButton update_button = new ImageButton();
                        //update_button.ID = "update_button";
                        //update_button.CommandName = "Update";
                        //update_button.ImageUrl = "~/images/update.gif";
                        //if ((int)new Page().Session["InsertFlag"] == 1)
                        //    update_button.ToolTip = "Add";
                        //else
                        //    update_button.ToolTip = "Update";
                        //update_button.OnClientClick = "return confirm('Are you sure to update the record?')";
                        //Container.Controls.Add(update_button);

                        //ImageButton cancel_button = new ImageButton();
                        //cancel_button.ImageUrl = "~/images/cancel.gif";
                        //cancel_button.ID = "cancel_button";
                        //cancel_button.CommandName = "Cancel";
                        //cancel_button.ToolTip = "Cancel";

                        //Container.Controls.Add(cancel_button);

                    }
                    else if (ControlType == "TextBox")
                    {
                        field_txtbox = new TextBox
                        {
                            ID = "txt" + FieldName,
                            ClientIDMode = ClientIDMode.Static,
                            Width = 80
                        };

                        if (DataType == "Nvarchar")
                            field_txtbox.Attributes.Add("onKeypress", "return checkSpecialChar(event)");
                        else if (DataType == "Integer")
                            field_txtbox.Attributes.Add("onKeypress", "return checkNum(event)");
                        else if (DataType == "Decimal")
                            field_txtbox.Attributes.Add("onKeypress", "return checkDec(this,event)");
                        else if (DataType == "DateTime")
                        {
                            field_txtbox.EnableTheming = false;
                            field_txtbox.CssClass = "DateBoxCSS_small";
                        }
                        field_txtbox.DataBinding += new EventHandler(OnDataBinding);
                        if (RequireValidation)
                        {
                            validation.ControlToValidate = field_txtbox.ID;
                            Container.Controls.Add(validation);
                        }
                        Container.Controls.Add(field_txtbox);

                    }
                   
                    else if (ControlType == "CheckBox")
                    {
                        checkbox = new CheckBox
                        {
                            ID = "chk" + FieldName
                        };
                        checkbox.DataBinding += new EventHandler(OnDataBinding);
                        Container.Controls.Add(checkbox);
                    }

                    else if (ControlType == "DropDownList")
                    {
                        dropdown = new DropDownList
                        {
                            ID = "ddl" + FieldName,
                            ClientIDMode = ClientIDMode.Static,
                            Width = 80
                        };
                        setDropDown(dropdown, Query);
                        dropdown.DataBinding += new EventHandler(OnDataBinding);
                        if (RequireValidation)
                        {
                            validation.ControlToValidate = dropdown.ID;
                            Container.Controls.Add(validation);
                        }
                        Container.Controls.Add(dropdown);
                    }
                    else if (ControlType == "Empty")
                    {
                        //Container.Controls.Add(null);
                    }
                    break;
                case ListItemType.Footer:
                    {

                        Container.Controls.Add(button);

                        break;
                    }

            }

        }


        private void OnDataBinding(object sender, EventArgs e)
        {

            object bound_value_obj = null;
            Control ctrl = (Control)sender;
            IDataItemContainer data_item_container = (IDataItemContainer)ctrl.NamingContainer;
            if (FieldName == null)
                return;
            bound_value_obj = DataBinder.Eval(data_item_container.DataItem, FieldName);

            switch (ItemType)
            {
                case ListItemType.Item:
                    {
                        if (ControlType == "CheckImage")
                        {
                            if (bound_value_obj.ToString() == "1")
                            {
                                Image img = (Image)sender;
                                img.ImageUrl = "../Images/blue_menu_icons/check_mark.png";
                            }
                        }
                        else if (ControlType == "CheckBox")
                        {
                            CheckBox checkBox = (CheckBox)sender;
                            checkBox.Checked = Convert.ToBoolean(Convert.ToInt16(bound_value_obj.ToString()));
                        }
                        else if (ControlType == "LinkButton")
                        {
                            LinkButton lnkbutton = ((LinkButton)sender);
                            lnkbutton.CommandName = FieldName;
                            lnkbutton.CommandArgument = bound_value_obj.ToString();
                        }
                        else if (ControlType == "LiteralWithID")
                        {
                            Literal field_ltrl = (Literal)sender;
                            bound_value_obj = DataBinder.Eval(data_item_container.DataItem, field_ltrl.ID);
                            field_ltrl.Text = bound_value_obj.ToString();

                        }
                        else if (ControlType == "TextBox")
                        {
                            TextBox field_ltrl = (TextBox)sender;
                            bound_value_obj = DataBinder.Eval(data_item_container.DataItem, field_ltrl.ID);
                            field_ltrl.Text = bound_value_obj.ToString();

                        }


                        else
                        {
                            Literal field_ltrl = (Literal)sender;
                            field_ltrl.Text = bound_value_obj.ToString();
                        }
                        break;
                    }
                case ListItemType.EditItem:
                    if (ControlType == "CheckBox")
                    {
                        CheckBox checkBox = (CheckBox)sender;
                        checkBox.Checked = Convert.ToBoolean(Convert.ToInt16(bound_value_obj.ToString()));
                    }
                    else if (ControlType == "DropDownList")
                    {
                        DropDownList dropdown = (DropDownList)sender;
                        //dropdown.SelectedValue = bound_value_obj.ToString();
                        dropdown.SelectedIndex = dropdown.Items.IndexOf(dropdown.Items.FindByText(bound_value_obj.ToString()));
                    }
                    else
                    {
                        TextBox field_txtbox = (TextBox)sender;
                        field_txtbox.Text = bound_value_obj.ToString();
                    }
                    break;
            }


        }


        private void setDropDown(DropDownList ddlist, String sql)
        {

            ddlist.Items.Clear();
            IDataReader reader = DB.GetRS(sql);
            ddlist.Items.Add(new ListItem("Select", ""));
            while (reader.Read())
            {

                ddlist.Items.Add(new ListItem(reader[0].ToString(), reader[1].ToString()));
            }
            reader.Close();

        }
    }
}
