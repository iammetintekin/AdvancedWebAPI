using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Project.Entity.LinkModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBAPIFramework.ActionFilters;

namespace WEBAPIFramework.Controllers
{ 
    /// <summary>
    /// Using for root documentation. level 3 api
    /// </summary>
    [ApiController]
    [Route("api")]
    [ApiExplorerSettings(GroupName = "v1")]

    public class RootController:ControllerBase
    {
        private readonly LinkGenerator _linkGenerator;

        public RootController(LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
        }
        [HttpGet(Name ="GetRoot")]
        // EĞER ROOT İSTEDİİM ACCEPT MEDİA TYPE GÖNDERİRSE LİNK LİSTESİ DÖNECEĞİZ.
        public async Task<IActionResult> GetRoot([FromHeader(Name ="Accept")] string MediaType)
        {
            if (MediaType.Contains("application/usr.metintekin.apiroot"))
            {
                //
                var list = new List<Link>()
                {
                    new Link()
                    {
                        Href = _linkGenerator.GetUriByName(HttpContext, nameof(GetRoot), new{}),
                        Method = "GET",
                        Rel = "_self"
                    },
                     new Link()
                    {
                        Href = _linkGenerator.GetUriByName(HttpContext, nameof(ProductsController.GetAllAsync), new{}),
                        Method = "GET",
                        Rel = "products"
                    },
                     new Link()
                    {
                        Href = _linkGenerator.GetUriByName(HttpContext, nameof(ProductsController.CreateAsync), new{}),
                        Method = "POST",
                        Rel = "products"
                    }
                };

                return Ok(list);
            }
            return NoContent();
        }
    }
}
