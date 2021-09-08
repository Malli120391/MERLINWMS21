using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21Common.Interfaces
{
   public interface ICustomerSession
    {
        void Age();

        void Clear();

        void ClearVal(String SessionName);

        void ResyncToDB();

        void SetVal(String SessionName, String SessionValue, System.DateTime ExpiresOn);

        void SetVal(String SessionName, int NewValue, System.DateTime ExpiresOn);


        void SetVal(String SessionName, Single NewValue, System.DateTime ExpiresOn);


        void SetVal(String SessionName, decimal NewValue, System.DateTime ExpiresOn);


        void SetVal(String SessionName, System.DateTime NewValue, System.DateTime ExpiresOn);


        String Session(String SessionName);

        bool SessionBool(String paramName);

        int SessionUSInt(String paramName);


        long SessionUSLong(String paramName);


        Single SessionUSSingle(String paramName);


        Decimal SessionUSDecimal(String paramName);


        DateTime SessionUSDateTime(String paramName);


        int SessionNativeInt(String paramName);


        long SessionNativeLong(String paramName);


        Single SessionNativeSingle(String paramName);


        Decimal SessionNativeDecimal(String paramName);


        DateTime SessionNativeDateTime(String paramName);
        

    }
}
