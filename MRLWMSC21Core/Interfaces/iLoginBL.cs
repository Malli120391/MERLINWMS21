using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRLWMSC21Core.Entities;

namespace MRLWMSC21Core.Business.Interfaces
{
    interface ILoginBL
    {
        UserProfile UserLogin(UserProfile User);

        UserProfile ValidateUserSession(UserProfile oUser);

        bool UserLogout(UserProfile oUser);
    }
}
