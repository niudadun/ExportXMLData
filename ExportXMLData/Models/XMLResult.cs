using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExportXMLData.Models
{
    /// <summary>
    /// response message structure
    /// </summary>
    public class XMLResult
    {
        public string XMLDataPair { get; set; }

        public string TotalExcludingGST { get; set; }

        public string GST { get; set; }

        public string CostCentre { get; set; }

        public string Error { get; set; }
    }

    /// <summary>
    /// recieve message structure
    /// </summary>
    public class Pobj
    {
        public string Alltext { get; set; }
    }
}
