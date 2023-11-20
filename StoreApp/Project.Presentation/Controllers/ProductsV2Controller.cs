using AutoMapper;  
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project.Services.ServiceManager;  
using WEBAPIFramework.ActionFilters;

namespace WEBAPIFramework.Controllers
{
    //2. sürümü desteği keseceğiz bilgisi
    // headersa deprecated version uyarısı ekler
    //[ApiVersion("2.0", Deprecated = true)] 
    //[ServiceFilter(typeof(LogFilterAttribute))]
    [Route("api/products/[action]")]
    [ResponseCache(CacheProfileName = "5mins")] // program cs de detaylı 
    [ApiExplorerSettings(GroupName = "v2")]
    public class ProductsV2Controller : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public ProductsV2Controller(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        [HttpGet(Name = "GetAllAsync")]
       // [HttpHead]
        //[ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetAllAsync()
        {
            var paged_data = await _serviceManager.ProductService.GetAllAsync(false);  
            return Ok(paged_data);
        }
    } 
}
