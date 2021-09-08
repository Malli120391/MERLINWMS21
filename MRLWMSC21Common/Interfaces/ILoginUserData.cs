using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21Common.Interfaces
{
  public interface ILoginUserData
    {
        Boolean Login(String Email, String Password, String LoginIPAddress);
       
    }
}
