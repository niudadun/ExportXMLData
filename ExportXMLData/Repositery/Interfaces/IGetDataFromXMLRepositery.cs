using ExportXMLData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExportXMLData.Repositery.Interfaces
{
    public interface IGetDataFromXMLRepositery
    {
        /// <summary>
        /// Method to get all available data from XML format text.
        /// </summary>
        /// <returns>Data from XML tags</returns>
        XMLResult GetTotalExpense(string alltext);
    }
}
