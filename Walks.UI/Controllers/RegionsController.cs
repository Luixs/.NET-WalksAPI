using Microsoft.AspNetCore.Mvc;
using Walks.UI.Models.DTO;

namespace Walks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public RegionsController(IHttpClientFactory httpclientFactory)
        {
            this._httpClientFactory = httpclientFactory;
        }
        public async Task<IActionResult> Index()
        {
            List<RegionDto> responseB = new List<RegionDto>();
            try
            {
                // --- GET REGIONS INFORMATION 
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync("https://localhost:7224/api/regions");

                response.EnsureSuccessStatusCode(); // Return error if is not success status code.

                responseB.AddRange(await response.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());

            }catch (Exception ex)
            {

            }

            return View(responseB);

        }
    }
}
