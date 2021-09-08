using MRLWMSC21_Endpoint.Security;

namespace MRLWMSC21_Endpoint.Interfaces
{
    interface ILogin
    {
        string UserLogin(WMSCoreMessage oRequest);

        WMSCoreMessage ValidateUserSession(WMSCoreMessage oRequest);

        WMSCoreMessage UserLogout(WMSCoreMessage oRequest);
        string Checktest(WMSCoreMessage oRequest);

    }
}
