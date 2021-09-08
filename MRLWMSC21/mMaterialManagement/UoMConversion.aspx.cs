using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using MRLWMSC21Common;

namespace MRLWMSC21.mMaterialManagement
{
    public partial class UoMConversion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Build_FromMeasurement(1.ToString());
            }
        }

        protected void ddlMeadureType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Build_FromMeasurement(ddlMeadureType.SelectedValue);
        }

        protected void ddlFromMeasurement_SelectedIndexChanged(object sender, EventArgs e)
        {
               

            String[] FromMeasurements = ddlFromMeasurement.SelectedValue.Split(',');
            ddlToMeasurement.Items.Clear();
            StringBuilder stGetToConversion = new StringBuilder();
            stGetToConversion.Append("select	ISNULL(MTD.ConvesionValue,1)*IIF(ISNULL(MPM1.PowerOf,0)-ISNULL(MPM.PowerOf,0)<0,1/POWER(convert(float,10),ISNULL(MPM.PowerOf,0)-ISNULL(MPM1.PowerOf,0)),POWER(convert(float,10),ISNULL(MPM1.PowerOf,0)-ISNULL(MPM.PowerOf,0))) ConversionValue, "
                                    +"IIF(MPM.MetricPrifixID IS NOT NULL, MPM.MetricPrifixName+' '+MTM.MeasurementName,MTM.MeasurementName) Measurement "
                                    +"from MES_MeasurementMaster MTM "
                                    +"LEFT JOIN MES_MatricPrefixMaster MPM ON MTM.ConversionTypeID=1 "
                                    + "left join MES_MatricPrefixMaster MPM1 ON MPM1.MetricPrifixID=" + FromMeasurements[0]
                                    + "LEFT join MES_MeasurementDetails MTD on MTD.MeasurementID=" + FromMeasurements [1]+ " AND MTD.ToMesurementID=MTM.MeasurementID "
                                    + "where MTM.MeasurementTypeID=" + ddlMeadureType.SelectedValue + " ORDER BY Measurement");
            IDataReader rsGetToMeasurement = DB.GetRS(stGetToConversion.ToString());
            ddlToMeasurement.Items.Add(new ListItem("Select"));
            while (rsGetToMeasurement.Read())
            {
                ddlToMeasurement.Items.Add(new ListItem(DB.RSField(rsGetToMeasurement, "Measurement"), DB.RSFieldDouble(rsGetToMeasurement, "ConversionValue").ToString()));
            }
            rsGetToMeasurement.Close();

        }

        public void Build_FromMeasurement(String MeasurementType)
        {
            ddlFromMeasurement.Items.Clear();
            IDataReader rsGetFromMeasurement = DB.GetRS("select convert(nvarchar,isnull(MPM.MetricPrifixID,0))+','+convert(nvarchar,MTM.MeasurementID)MeasurementID,IIF(MPM.MetricPrifixID IS NOT NULL, MPM.MetricPrifixName+' '+MTM.MeasurementName,MTM.MeasurementName) Measurement from MES_MeasurementMaster MTM LEFT JOIN MES_MatricPrefixMaster MPM ON MTM.ConversionTypeID=1 where MeasurementTypeID=" + MeasurementType + " ORDER BY Measurement");
            ddlFromMeasurement.Items.Add(new ListItem("Select"));
            while (rsGetFromMeasurement.Read())
            {
                ddlFromMeasurement.Items.Add(new ListItem(DB.RSField(rsGetFromMeasurement, "Measurement"), DB.RSField(rsGetFromMeasurement, "MeasurementID")));
            }
            rsGetFromMeasurement.Close();
            ddlToMeasurement.Items.Clear();
        }
    }
}