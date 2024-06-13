using CourseApp.UI.Resources;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;

namespace CourseApp.UI.Controllers
{
    public class GroupController : Controller
    {

        public async Task<IActionResult> Index(int page = 1)
        {
            using (HttpClient client = new HttpClient())
            {
                using (var response = await client.GetAsync("https://localhost:44392/api/Groups?page=" + page + "&size=2"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var bodyStr = await response.Content.ReadAsStringAsync();

                        var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                        PaginatedResponseResource<GroupListItemGetResource> data = JsonSerializer.Deserialize<PaginatedResponseResource<GroupListItemGetResource>>(bodyStr, options);
                        return View(data);
                    }
                    else
                    {
                        return RedirectToAction("error", "home");
                    }
                }
            }
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GroupCreateDto dto)
        {
            if (ModelState.IsValid)
            {
                using (HttpClient client = new HttpClient())
                {
                    var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("https://localhost:44392/api/Groups", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Validation Error!");
                    }
                }
            }
            return View(dto);
        }

    }
}