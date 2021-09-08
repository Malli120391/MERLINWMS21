using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing ;
using System.IO;

namespace MRLWMSC21Common
{
   
    public class NetworkPrinterClass: PrintDocument 
    {
        protected override void OnBeginPrint(System.Drawing.Printing.PrintEventArgs ev)
        {
            base.OnBeginPrint(ev);
            this.PrinterSettings.PrinterName = "SCLaser2";
            //setup rest of stuff....   
        }
    }

}
