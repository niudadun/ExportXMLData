using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExportXMLData.Models;
using ExportXMLData.Repositery.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExportXMLData.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class GetXMLController : ControllerBase
    {
        private readonly IGetDataFromXMLRepositery _GetDatarepository;

        public GetXMLController(IGetDataFromXMLRepositery GetDatarepository)
        {
            _GetDatarepository = GetDatarepository;
        }

        [Route("~/api/GetXMLData")]
        [HttpPost]
        public ActionResult<XMLResult> GetXMLData([FromBody]Pobj alltext)
        {
            try
            {
                if (alltext == null)
                {
                    return BadRequest("Owner object is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }

                return _GetDatarepository.GetTotalExpense(alltext.Alltext); ;
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
