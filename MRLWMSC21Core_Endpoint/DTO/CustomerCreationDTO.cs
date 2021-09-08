using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.DTO
{
    public class CustomerCreationDTO
    {
        private string _CustomerName;
        private string _CustomerCode;
        

        public string CustomerName { get => _CustomerName; set => _CustomerName = value; }
        public string CustomerCode { get => _CustomerCode; set => _CustomerCode = value; }
    }
}