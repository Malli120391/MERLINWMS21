using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Data.SqlClient;

namespace MRLWMSC21Common
{
    public class GVTemplate:ITemplate 
    {
        #region ITemplate Members

        //A variable to hold the type of ListItemType.
        ListItemType _templateType;

        //A variable to hold the column name.
        string _columnName;

        //Constructor where we define the template type and column name.

        public GVTemplate(ListItemType type, string colname)
         {
            //Stores the template type.
            _templateType = type;
            //Stores the column name.
            _columnName = colname;
        }


       public void InstantiateIn(Control container)
        {
            switch (_templateType)
            {
                case ListItemType.Header:
                    //Creates a new label control and add it to the container.
                    Label lbl = new Label
                    {
                        Text = _columnName             //Assigns the name of the column in the lable.
                    };            //Allocates the new label object.
                    container.Controls.Add(lbl);        //Adds the newly created label control to the container.
                    break;

                case ListItemType.Item:
                    //Creates a new text box control and add it to the container.
                    Literal  lt1 = new Literal();                            //Allocates the new text box object.
                    
                    lt1.DataBinding += new EventHandler(lt1_DataBinding);   //Attaches the data binding event.
                   // lt1.Columns = 4;                                        //Creates a column with size 4.
                    container.Controls.Add(lt1);                            //Adds the newly created textbox to the container.
                    break;

                case ListItemType.EditItem:
                    TextBox txt1 = new TextBox
                    {
                        ID = "txt" + _columnName
                    };                            //Allocates the new text box object.
                    txt1.DataBinding += new EventHandler(txt1_DataBinding);   //Attaches the data binding event.
                     txt1.Columns = 10;                                        //Creates a column with size 4.
                    container.Controls.Add(txt1);        
                    break;

                case ListItemType.Footer:
                    CheckBox chkColumn = new CheckBox
                    {
                        ID = "Chk" + _columnName
                    };
                    container.Controls.Add(chkColumn);
                    break;

            }

        }

        #endregion

        /// <summary>
        /// This is the event, which will be raised when the binding happens.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        void lt1_DataBinding(object sender, EventArgs e)
        {
            Literal  ltdata = (Literal )sender;
            GridViewRow container = (GridViewRow)ltdata.NamingContainer;
            object dataValue = DataBinder.Eval(container.DataItem, _columnName);
            if (dataValue != DBNull.Value)
            {
                ltdata.Text = dataValue.ToString();
            }
        }

        void txt1_DataBinding(object sender, EventArgs e)
        {
            TextBox txtdata = (TextBox)sender;
            GridViewRow container = (GridViewRow)txtdata.NamingContainer;
            object dataValue = DataBinder.Eval(container.DataItem, _columnName);
            if (dataValue != DBNull.Value)
            {
                txtdata.ID = "txt" + _columnName;
                txtdata.Text = dataValue.ToString();
            }
        }

        

    }
}
