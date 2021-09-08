using SAPIntegration.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRLWMSC21Common;

namespace SAPIntegration.INOUT
{
    

    public class OrderManager
    {
        private InvSalesOrderInfo InvSO;
        private Object order;
        private bool isUpdate;
        private bool isCreate;
        private bool isDelete;

        public OrderManager(object objOrder) 
        {
            order = objOrder;
            isUpdate = false;
            isDelete = false;
            isCreate = true;
        }

        
        public bool saveOrUpdate() 
        {
            return true;

        
        }


        public bool deleteOrder() 
        {
            return true;
        }

    }
}
