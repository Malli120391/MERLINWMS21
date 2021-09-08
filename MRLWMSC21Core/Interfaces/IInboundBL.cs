using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRLWMSC21Core.Entities;
using MRLWMSC21Core.Library;

namespace MRLWMSC21Core.Business.Interfaces
{
    interface IInboundBL 
    {
        /// <summary>
        ///  Validate the Location Code which is scanned.
        /// </summary>
        /// <param name="oLocation">Location Object.</param>
        /// <returns>Boolean value identifying if the Scanned Location code is valid.</returns>
        bool ValidateLocationCode(Location oLocation, Constants.LocationType cLocationType, bool DepricateExceptionsAndReturnFlags = false);

        /// <summary>
        /// Validate the Scanned Location Code and Return Inventory at that location.
        /// </summary>
        /// <param name="oLocation">Location Object</param>
        /// <returns>Inventory List</returns>
        List<Inventory> ValidateLocationAndReturnInventory(Location oLocation, Constants.LocationType cLocationType, bool DepricateExceptionsAndReturnFlags = false);

        /// <summary>
        ///  Fetch the list of inbounds that are to be Unloaded based on a search criteria (User, Vehicle).
        /// </summary>
        /// <param name="oSearchCriteria">Search Criteria object to filter the list on Inbounds.</param>
        /// <returns>Inbound List with Vehicle information.</returns>
        List<Inbound> GetInboundsForUnloading(SearchCriteria oSearchCriteria);


        /// <summary>
        ///  Fetch the list of vehicle that are to be Unloaded based on a search criteria (User, Vehicle).
        /// </summary>
        /// <param name="oSearchCriteria">Search Criteria object to filter the list on Inbounds.</param>
        /// <returns>Inbound List with Vehicle information.</returns>
        List<Vehicle> GetVehiclesForUnloading(SearchCriteria oSearchCriteria);


        /// <summary>
        ///  Pass the RSN Number of the Material received in the shipment, to validate if the material is part of the shipment, receive it as the desingated dock location into the pallet.
        /// </summary>
        /// <param name="oInventory">Inventory Object of the SKU Received.</param>
        /// <returns>Boolean Value identifying if the Material is Received or not.</returns>
        Inventory ValidateRSNAndReceive(Inventory oInventory, bool IsDeNesting = false);

        /// <summary>
        /// Generates Pallet Suggestions based on the Material to Identify the Zone to putaway.
        /// </summary>
        /// <param name="oSearchCriteria">Pallet Code or Material Code to generate the Zoning Suggestion to putaway.</param>
        /// <returns>Returns the Location Entity identifying the Zone to Putaway.</returns>
        Location GeneratePalletGateSuggestion(SearchCriteria oSearchCriteria);

        /// <summary>
        /// Generates putaway suggestions for the Material / Pallet.
        /// </summary>
        /// <param name="oCriteria">Material ID or Pallet ID for Putaway Suggestions.</param>
        /// <returns>Returns the Location where the material needs to be putaway.</returns>
     //   List<Suggestion> GeneratePutawaySuggestion(SearchCriteria oCriteria);
        List<Suggestion> GeneratePutawaySuggestion(SearchCriteria oCriteria);

        /// <summary>
        /// Putaway Inventory in the warehouse.
        /// </summary>
        /// <param name="lInventory">List of Inventory Entity to putaway.</param>
        /// <returns>Boolean Value identifying status of Putaway Process.</returns>
        //bool PutawayInventory(List<GoodsMovement> lMovement);

        /// <summary>
        /// Putaway Inventory in the Warehouse
        /// </summary>
        /// <param name="oInventory">Inventory Object to Putaway the material.</param>
        /// <returns>Boolean Value identifying the status of the Putaway Process.</returns>
        List<GoodsMovement> PutawayInventory(GoodsMovement oInventory);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="oPallet"></param>
        /// <returns></returns>
        bool ValidatePalletCode(Pallet oPallet);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oPallet"></param>
        /// <returns></returns>
        List<Inventory> ValidatePalletAndReturnInventory(Pallet oPallet);


        /// <summary>
        /// Flags the current location as full and re-generates suggestion for the balance inventory in the pallet.
        /// </summary>
        /// <param name="oLocation">Object with Location code to identify the location to be marked full.</param>
        /// <param name="oPallet">Object with Pallet Code to identify the inventory for which suggestions are to be re-generated.</param>
        /// <returns>List of Inventory with the location re-generated for putaway.</returns>
        List<Suggestion> MarkBinFull(Location oLocation, Pallet oPallet);

        /// <summary>
        /// Marks the inventory difference in the pallet as Lost / Found and confirms items putaway.
        /// </summary>
        /// <param name="oPallet"></param>
        /// <returns></returns>
        bool PalletPutawayComplete(Pallet oPallet);

        Suggestion FetchNextSuggestion(SearchCriteria oCriteria);
        /// <summary>
        /// Update or Delete incase of modificaions required
        /// </summary>
        /// <param name="oCriteria"></param>
        /// <returns></returns>



        List<Suggestion> FetchAllPutawaySuggestion(SearchCriteria oCriteria);
        /// <summary>
        /// Fetch all the suggestions for a pallet.
        /// </summary>
        /// <param name="oCriteria">PAllet Code & Current location of the pallet.</param>
        /// <returns></returns>

        Inventory UpdateInventory(Inventory oInventory);


    }
}
