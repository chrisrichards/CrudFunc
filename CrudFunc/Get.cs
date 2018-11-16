using System.Globalization;
using System.Threading.Tasks;
using CrudFunc.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace CrudFunc
{
    public static class Get
    {
        private static AppDbContext _context;

        [FunctionName("Get")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "blogs/{id:int}")]
            HttpRequest req,
            int id,
            ILogger log)
        {
            log.LogInformation($"Get - blogs/{id}");

            if (_context == null)
                _context = AppDbContextHelper.CreateContext();

            var blog = await _context.Blogs
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.BlogId == id);

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            settings.Converters.Add(new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ",
                DateTimeStyles = DateTimeStyles.AdjustToUniversal
            });
            return new JsonResult(blog, settings);
        }
    }
}