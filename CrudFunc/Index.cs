using System.Globalization;
using System.Linq;
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
    public static class Index
    {
        private static AppDbContext _context;

        [FunctionName("Index")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "blogs")]
            HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Index - blogs");

            if (_context == null)
                _context = AppDbContextHelper.CreateContext();

            var blogs = await _context.Blogs
                .AsNoTracking()
                .OrderByDescending(blog => blog.BlogId)
                .ToListAsync();

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            settings.Converters.Add(new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ",
                DateTimeStyles = DateTimeStyles.AdjustToUniversal
            });
            return new JsonResult(blogs, settings);
        }
    }
}