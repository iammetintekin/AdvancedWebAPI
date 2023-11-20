using AutoMapper;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Project.Entity.DTOs.Product; 
using Project.Entity.RequestFeatures;
using Project.Services.ServiceManager; 
using System.Text.Json; 
using WEBAPIFramework.ActionFilters;

namespace WEBAPIFramework.Controllers
{
    //[ApiVersion("1.0")]
    [ServiceFilter(typeof(LogFilterAttribute))] 
    [ApiController]
    [Route("api/products/[action]")]
    [ResponseCache(CacheProfileName = "5mins")] // program cs de detaylı
    [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 90)] // şeklinde override edilebilir.
    [ApiExplorerSettings(GroupName = "v1")]
    public class ProductsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager; 
        public ProductsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        //[ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        //[ValidationAttributeFilter] 
        // [ResponseCache(Duration = 60)]
        //program cs den yeni tanımlandı ve artık controllerin tüm
        //endpointler 5 mins profilini kullanıyor GetAll methodu hariç.
        //1 kere istek attıktan sonra 60 saniye boyunca ne kadar istek gelirse gelsin datayı yenilemez cahdeki datayı gösterir.
        //60 saniye sonra güncellenen datalar listelenir.
        // headerstaki age özelliği ile isteğin kaç saniye beklediği yazıyor
   
        [HttpGet]
        [ActionName("GetAllAsync")]
        [HttpHead]
        [Authorize]
        public async Task<IActionResult> GetAllAsync([FromQuery]ProductRequestParameters Parameters)
        { 
            var paged_data = await _serviceManager.ProductService.GetAllAsync(Parameters,false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paged_data.meta));
            // xml veya json formatında config yapıldı.
            return StatusCode(201, paged_data.products);
        }
        [Authorize] 
        [HttpGet("{id:int}",Name ="GetbyIdAsync")]
        public async Task<IActionResult> GetAsync([FromRoute(Name = "id")] int id)
        {
            var data = await _serviceManager.ProductService.GetByIdAsync(id, false);  
            return StatusCode(201, data);
        }
        /// <summary>
        /// Ürün oluuşturma yetkili admin
        /// </summary>
        /// <param name="CreateProductDto"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Personel")]
        [HttpPost(Name = "CreateAsync")]
        [ServiceFilter(typeof(ValidationFilterAttribute))] 
        public async Task<IActionResult> CreateAsync([FromBody] CreateProductDto CreateProductDto)
        {
            var create_result = await _serviceManager.ProductService.CreateSingleAsync(CreateProductDto);
            return StatusCode(201, create_result);
        }
        [Authorize(Roles = "Admin,Personel")] 
        [HttpPut("{id:int}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult UpdateAsync([FromRoute(Name = "id")] int id, [FromBody] UpdateProductDto UpdateProductDto)
        {
            _serviceManager.ProductService.UpdateSingleAsync(id, UpdateProductDto, true);
            return NoContent(); 
        }
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteByIdAsync([FromRoute(Name = "id")] int id)
        {
            _serviceManager.ProductService.DeleteByIdAsync(id, false);
            return NoContent(); // 204 
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PartialUpdateOneProductAsync([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<UpdateProductDto> ProductPatch)
        {
            if (ProductPatch is null)
                return BadRequest(ModelState);

            var result =await _serviceManager.ProductService.GetOneProductForPatchAsync(id, true);
            
            ProductPatch.ApplyTo(result.Item1, ModelState);

            TryValidateModel(result.Item1);

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState); // hata olursa 422 ile dönüyor.

            // değişecek sonra
            await _serviceManager.ProductService.SaveChangesForPatch(result.Item1, result.Item2);
            return NoContent();
        }

        [HttpOptions]
        [Authorize] 
        public IActionResult GetProductsOptions()
        {
            Response.Headers.Add("Allow", "GET,PUT,POST,PATCH,DELETE,HEAD,OPTIONS");
            return Ok();
        }
    }
}
