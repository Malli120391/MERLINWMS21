using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.DTO
{
    public class SalesOrderDTO
    {
        private string _SOHeaderID;
        private string _SONumber;

        public string SOHeaderID { get => _SOHeaderID; set => _SOHeaderID = value; }
        public string SONumber { get => _SONumber; set => _SONumber = value; }
    }
}