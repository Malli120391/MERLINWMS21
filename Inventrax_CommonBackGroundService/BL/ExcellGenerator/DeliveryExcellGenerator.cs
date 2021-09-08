using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventrax_CommonBackGroundService.BL.ExcellGenerator
{
    public class DeliveryExcellGenerator : IExcellGenerator
    {
        public void Create(DataTable dt, string tenantName, string warehouseName, string fileName)
        {
            try
            {

                string strOperationNumber = string.Empty;
                ExcelPackage objExcelPackage = new ExcelPackage();

               

                //Create the worksheet    
                ExcelWorksheet objWorksheet = objExcelPackage.Workbook.Worksheets.Add("Delivery");

                #region  Region for  Header Name

                int val = dt.Columns.Count;
                using (ExcelRange Rng = objWorksheet.Cells[1, 1, 2, val])
                {
                    Rng.Merge = true;
                    Rng.Style.Font.Bold = true;
                    Rng.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    Rng.Style.Font.SetFromFont(new System.Drawing.Font("Calibri", 25));

                }
                objWorksheet.Cells[1, 1, 2, val].RichText.Text = "Delivery Note Notification from Shipper";
                #endregion

                #region  Region for  Warehouse and Tenant Name
                using (ExcelRange Rng = objWorksheet.Cells[3, 1, 3, val])
                {
                    Rng.Merge = true;
                    Rng.Style.Font.Bold = true;
                    Rng.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    Rng.Style.Font.SetFromFont(new System.Drawing.Font("Calibri", 15));

                }
                objWorksheet.Cells[3, 1, 3, val].RichText.Text = "Warehouse :   " + warehouseName;


                using (ExcelRange Rng = objWorksheet.Cells[4, 1, 4, val])
                {
                    Rng.Merge = true;
                    Rng.Style.Font.Bold = true;
                    Rng.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    Rng.Style.Font.SetFromFont(new System.Drawing.Font("Calibri", 15));

                }
                objWorksheet.Cells[4, 1, 4, val].RichText.Text = "Tenant:  " + tenantName;
                #endregion


                objWorksheet.Cells.AutoFitColumns();









                // The below line will handle writing data table  starting from A6
                objWorksheet.Cells["A6"].LoadFromDataTable(dt, true);


                objWorksheet.Cells.Style.Font.SetFromFont(new System.Drawing.Font("Calibri", 12));
                objWorksheet.Cells.AutoFitColumns();



                objExcelPackage.SaveAs(new System.IO.FileInfo(fileName));



            }
            catch (Exception ex)
            {
               LogWriter.WriteLog("Exception in method DeliveryExcellGenerator.Create()  " + ex.Message);
            }

        }
    }
}
