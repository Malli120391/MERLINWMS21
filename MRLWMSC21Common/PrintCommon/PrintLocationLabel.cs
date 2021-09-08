using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRLWMSC21Common.Interfaces;
using System.Xml.Serialization;
namespace MRLWMSC21Common
{
  [Serializable]
  public class PrintLocationLabel : IPrintLocationLabel
    {
        public string PrintLable(string location,string IP)
        {
            if (location != "")
            {

                ZPL zpl = new ZPL();
                zpl.addBarcode(100, 20, location, 100, BarcodeType.CODE128, false, 2);
                zpl.drawText(100, 180, 30, 30, location);
               // string data = zpl.getZPLString();
                zpl.printUsingIP(IP, 9100, 1, 0);
            }
            return "Successfully Printed";
        }

        //Commeneted by M.D Prasad on 2-1-2019 //
        public string PrintLable_ZPL(string location, string IP)
        {
            string dt = "";
            if (location != "")
            {

                ZPL zpl = new ZPL();
                zpl.addBarcode(80, 50, location, 150, BarcodeType.CODE128, false, 2);
                zpl.drawText(100, 240, 30, 30, location);
                // string data = zpl.getZPLString();
                dt = zpl.printUsingIP_ZPL(IP, 9100, 1, 0);
            }
            return dt;
        }

        //Commeneted by M.D Prasad on 2-1-2019 //
        //============= Added By M.D.Prasad On 24-Apr-2020 For Display Location Code ==================//       
        public string PrintLable_ZPL_DisplayLocCode(string location, string IP)   
        {
            string dt = "";
            if (location != "")
            {               
                ZPL zpl = new ZPL();
                //string displayLocation = DB.GetSqlS("SELECT DisplayLocationCode S FROM INV_Location WHERE IsActive=1 AND IsDeleted=0 AND Location='" + location + "'");
                zpl.addBarcodeForQRCodeLocation(30, 10, location, 2, BarcodeType.QRCODE, false, 3);
                zpl.drawText(30, 160, 20, 20, location);
                dt = zpl.printUsingIP_ZPL(IP, 9100, 1, 0);    
            }
            return dt;
        }

        //==================== Added By M.D.Prasad On 01-Jan-2021 For Location Data Loaded From DB ====================//
        public string PrintLable_ZPL_FromDB(string location, string IP)
        {
            string dt = "";
            if (location != "")
            {
                ZPL zpl = new ZPL();
                string zplData = DB.GetSqlS("SELECT ZPLScript AS S FROM TPL_Tenant_BarcodeType WHERE Isactive=1 AND IsDeleted=0 AND LabelType='Location'");
                dt = zplData.Replace("LocationName", location);
            }
            return dt;
        }
    }
}
