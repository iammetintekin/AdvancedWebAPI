using WEBAPI.Utilities.Formatters;

namespace WEBAPI.Extensions
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder AddCustomCsvFormatter(this IMvcBuilder builder)
        {
            builder.AddMvcOptions(options =>
            {
                options.OutputFormatters.Add(new ProductDtoCSVFormatter());
            });
            return builder;
        }
    }
}
