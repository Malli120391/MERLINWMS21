using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRLWMSC21Common.Interfaces;
using System.Xml.Serialization;
namespace MRLWMSC21Common.PrintCommon
{
    [Serializable]
   public sealed class PrintContainerLabel : IPrintContainerLabel
    {
        public string PrintLable(string Containercode, string IP)
        {
            if (Containercode != "")
            {
                ZPL zpl = new ZPL();
                zpl.addBarcode(100, 20, Containercode, 100, BarcodeType.CODE128, false, 2);
                zpl.drawText(100, 180, 30, 30, Containercode);
                // string data = zpl.getZPLString();
                zpl.printUsingIP(IP, 9100, 1, 0);

            }
            return "";
        }

        public string PrintLable_Container(PrintBO bo, string IP)
        {
            if (bo.BarcodeString != "")
            {

                ZPL zpl = new ZPL();
                switch (bo.PrinterDPI)
                {
                    case 203:
                        zpl.addBarcode(100, 20, bo.BarcodeString, 100, BarcodeType.CODE128, false, 2);
                        zpl.drawText(100, 180, 30, 30, bo.BarcodeString);
                        break;
                    case 300:
                        zpl.addBarcode(150, 50, bo.BarcodeString, 200, BarcodeType.CODE128, false, 5);
                        zpl.drawText(250, 270, 100, 100, bo.BarcodeString);
                        break;
                    default:
                        zpl.addBarcode(150, 50, bo.BarcodeString, 200, BarcodeType.CODE128, false, 5);
                        zpl.drawText(250, 270, 100, 100, bo.BarcodeString);
                        break;

                }
                zpl.printUsingIP(IP, 9100, 1, 1);

            }
            return "";
        }

        public string PrintLable_ZPL(PrintBO bo, string IP)
        {
            string ZPL = "";
            if (bo.BarcodeString != "")
            {

                ZPL zpl = new ZPL();
                switch (bo.PrinterDPI)
                {
                    case 203:
                        zpl.drawText(105, 10, 30, 30, bo.AccountName);
                        zpl.addBarcodeForQRCodeContainer(130, 40, bo.BarcodeString, 2, BarcodeType.QRCODE, false,3);
                        zpl.drawText(70,155,30, 30, bo.BarcodeString);
                        break;
                    case 300:
                        zpl.drawText(250, 20, 70, 70, bo.AccountName);
                        zpl.addBarcodeForQRCodeContainer(100, 125, bo.BarcodeString, 200, BarcodeType.QRCODE, false, 4);
                        zpl.drawText(250, 350, 70, 70, bo.BarcodeString);
                        break;
                    default:
                        zpl.drawText(20, 20, 40, 40, bo.AccountName);
                        zpl.addBarcodeForQRCodeContainer(20, 50, bo.BarcodeString, 150, BarcodeType.QRCODE, false, 2);
                        zpl.drawText(25, 220, 40,40, bo.BarcodeString);
                        break;

                }
                ZPL = zpl.printUsingIP_ZPL(IP, 9100, 1, 1);

            }
            return ZPL;
        }

        //==================== Added By M.D.Prasad On 02-Jan-2021 For Container Data Loaded From DB ====================//
        public string PrintLable_ZPL_FromDB(PrintBO bo, string IP)
        {
            string ZPL = "";
            if (bo.BarcodeString != "")
            {
                ZPL zpl = new ZPL();
                string zplData = DB.GetSqlS("SELECT ZPLScript AS S FROM TPL_Tenant_BarcodeType WHERE Isactive=1 AND IsDeleted=0 AND LabelType='Container'");
                string ConData = zplData.Replace("ContainerName", bo.BarcodeString);  
                ZPL = ConData.Replace("AccountName", bo.AccountName);
            }
            return ZPL;
        }
    }
}
