using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _config;

        public string ApiResponse { get; set; }
        public string ApiUrl { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public async Task<ActionResult> OnGet()
        {
            string apiUrl = _config.GetValue<string>("WEATHER_FORE_CAST_API");
            if (string.IsNullOrWhiteSpace(apiUrl))
            {
                return Page();
            }
            
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                ApiUrl = $"{apiUrl}/WeatherForecast";
                ApiResponse = await client.GetStringAsync(ApiUrl);
                return Page();
            }
        }
    }
}
