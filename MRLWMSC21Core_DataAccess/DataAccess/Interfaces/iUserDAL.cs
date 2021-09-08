using System;
using System.Collections.Generic;
using System.Text;
using MRLWMSC21Core.Entities;

namespace MRLWMSC21Core.DataAccess.Interfaces
{
   interface IUserDAL
    {
        List<User> GetAllUsersUnderAccount(Account oAccount);

        List<User> GetAllUsersUnderAccount(int iAccountID);
        
        List<User> GetOnlyAccountUsers(Account oAccount);

        List<User> GetOnlyAccountUsers(int iAccountID);

        List<User> GetAllUsersUnderTenant(Tenant oTenant);

        List<User> GetAllUsersUnderTenant(int iTenant);

        User GetUserByID(int UserID);

        List<User> GetUsersByID(string sUserIDs, string Delimiter);

        bool SaveUser(User oUser);

        UserProfile LoginUser(UserProfile oUserProfile);
        

    }
}
