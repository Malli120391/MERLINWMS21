using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRLWMSC21Core.Library;
 
namespace MRLWMSC21Core.Business
{
    public class GenericBL : BaseBL
    {
        private string _ClassCode = "WMSCore_BL_008_";

        public GenericBL(int LoginUser, string ConnectionString) : base(LoginUser, ConnectionString)
        {
            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_BusinessLayer.GenericBL);
        }

        public bool LogException(WMSExceptionMessage oWMSExcp)
        {
            
            try
            {

                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oWMSExcp", oWMSExcp);

                ExceptionHandling.LogException(oWMSExcp, _ClassCode + "001", oExcpData);

                return true;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oWMSExcp", oWMSExcp);

                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }
    }
}
