using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21Common.Interfaces
{
   public interface ICustomPrincipal
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        bool IsInRole(string role);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="warehouse"></param>
        /// <returns></returns>
        bool IsInWarehouse(string warehouse);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sitecode"></param>
        /// <returns></returns>
        bool IsInSiteCode(string sitecode);


        // Checks whether a principal is in all of the specified set of   roles
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        bool IsInAllRoles(params string[] roles);


        // Checks whether a principal is in any of the specified set of roles
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        bool IsInAnyRoles(params string[] roles);


        // Checks whether a principal is in all of the specified set of warehouses
        /// <summary>
        /// 
        /// </summary>
        /// <param name="warehouses"></param>
        /// <returns></returns>
        bool IsInAllWarehouses(params string[] warehouses);



        // Checks whether a principal is in any of the specified set of   warehouses
        /// <summary>
        /// 
        /// </summary>
        /// <param name="warehouses"></param>
        /// <returns></returns>
        bool IsInAnyWarehouses(params string[] warehouses);


        // Checks whether a principal is in all of the specified set of   sitecodes
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sitecodes"></param>
        /// <returns></returns>`
        bool IsInAllSitecodes(params string[] sitecodes);



        // Checks whether a principal is in any of the specified set of   sitecodes
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sitecodes"></param>
        /// <returns></returns>
        bool IsInAnySitecodes(params string[] sitecodes);
        

    }
}
