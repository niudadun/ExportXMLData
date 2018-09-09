using ExportXMLData.Models;
using ExportXMLData.Repositery.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace ExportXMLData.Repositery
{
    public class GetDataFromXMLRepositery: IGetDataFromXMLRepositery
    {
        /// <summary>
        /// Get all data from xml tags
        /// </summary>
        /// <param name="alltext">input text</param>
        /// <returns></returns>
        public XMLResult GetTotalExpense(string alltext)
        {
            XMLResult totalCost = new XMLResult {
                XMLDataPair = "",
                Error = "",
                CostCentre = "UNKNOWN"
            };
            if (!CheckValidation(alltext, totalCost))
            {
                return totalCost;
            }
            string xml = "<Result>" + alltext + "</Result>";
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(xml);
            }
            catch (XmlException e)
            {
                totalCost.Error = e.Message;

                if (e.Message.Contains("does not match the end tag"))
                {
                    totalCost.Error = e.Message.Substring(0,e.Message.IndexOf("start tag") + 9) + " has no corresponding closing tag. Cannot process this request.";
                }
                return totalCost;
            }

            foreach (var node in new List<XmlNode>(doc.FirstChild.ChildNodes
                                        .OfType<XmlNode>()).Where(
                                        x => x.ParentNode != null && x.NodeType == XmlNodeType.Text))
            {
                node.ParentNode.RemoveChild(node);
            }
            BusinessLogic(doc,totalCost);
            string result = JsonConvert.SerializeXmlNode(doc.FirstChild);
            totalCost.XMLDataPair = result;

            return totalCost;
        }

        /// <summary>
        /// Check if recieved data meet business reqiurements
        /// </summary>
        /// <param name="doc">parsed text</param>
        /// <param name="totalCost">response message</param>
        private void BusinessLogic(XmlDocument doc, XMLResult totalCost)
        {
            XmlNode totalNode = doc.GetElementsByTagName("total")[0];
            bool parsed = double.TryParse(totalNode.InnerText, out double numValue);

            if (!parsed)
            {
                totalCost.TotalExcludingGST = "Invalid number format";
                totalCost.GST = "Invalid number format";
                totalCost.Error = totalNode.InnerText + " is not an valid number.";
            }
            else
            {
                totalCost.TotalExcludingGST = Math.Round((numValue / 1.1),2).ToString();
                totalCost.GST = Math.Round((numValue / 11),2).ToString();
            }
            XmlNode costCentreNode = doc.GetElementsByTagName("cost_centre")[0];
            if (costCentreNode == null || costCentreNode.InnerText == "")
            {
                totalCost.CostCentre = "UNKNOWN";
            }
            else
            {
                totalCost.CostCentre = costCentreNode.InnerText;
            }
        }

        /// <summary>
        /// Check input data validation
        /// </summary>
        /// <param name="data">input text</param>
        /// <param name="totalCost">response message</param>
        /// <returns>true or false to return</returns>
        private bool CheckValidation(string data, XMLResult totalCost)
        {
            bool result = true;
            if (!data.Contains("<total>"))
            {
                totalCost.Error = "'total' section is reqiured. Cannot process this request.";
                result = false;
            }
            return result;
        }
    }
}
