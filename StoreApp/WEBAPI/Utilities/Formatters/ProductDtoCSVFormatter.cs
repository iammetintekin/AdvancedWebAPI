using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Project.Entity.DTOs.Product;
using System.Text;

namespace WEBAPI.Utilities.Formatters
{
    public class ProductDtoCSVFormatter : TextOutputFormatter
    {
        public ProductDtoCSVFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        // nesne bazlı csv desteği
        protected override bool CanWriteType(Type? type)
        {
            // type formata uygun mu kontrolü?
            if (typeof(ProductDto).IsAssignableFrom(type) || typeof(IEnumerable<ProductDto>).IsAssignableFrom(type))
            {
                return base.CanWriteType(type);
            }
            return false;
        }
        private static void FormatCsv(StringBuilder sb, ProductDto dto)
        {
            sb.AppendLine($"{dto.Id}, {dto.Name}, {dto.Price}");

        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            var buffer = new StringBuilder();
            // tekil obje mi
            if(context.Object is ProductDto listProductDto)
            {
                FormatCsv(buffer, listProductDto); 
            }
            // yoksa çoğul obje mi
            else
            {
                var data = (IEnumerable<ProductDto>)context.Object;

                foreach (var item in data)
                {
                    FormatCsv(buffer, item);
                }
            }
            await response.WriteAsync(buffer.ToString());
        }
    }
}
