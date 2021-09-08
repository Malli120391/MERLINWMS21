using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.DTO
{
    public class MastersDTO
    {

     
        

       





        private string _StartNum;

        private string _EndNum;

        private string _UpdatedDate;

        public string StartNum { get => _StartNum; set => _StartNum = value; }
        public string EndNum { get => _EndNum; set => _EndNum = value; }
        public string UpdatedDate { get => _UpdatedDate; set => _UpdatedDate = value; }
    }

 
}