using FinanczeskaServerApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinanczeskaServerApp.Controllers
{
    public class ApiController : Controller
    {
        private readonly S4HBIService _s4HBIService;

        public ApiController(S4HBIService s4HBIService)
        {
            _s4HBIService = s4HBIService ?? throw new ArgumentNullException(nameof(s4HBIService));
        }

        [HttpPost]
        public async Task<IActionResult> LoadData(string serializeData)
        {
            string requestBody = await new StreamReader(Request.Body).ReadToEndAsync();
            bool result = _s4HBIService.LoadData(requestBody);
            if (result)
                return Ok(result);
            else
                return BadRequest(false);
        }
    }
}
