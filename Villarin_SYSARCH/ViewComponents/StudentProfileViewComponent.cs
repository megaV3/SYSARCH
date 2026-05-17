using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Villarin_SYSARCH.Models;

namespace Villarin_SYSARCH.ViewComponents
{
    public class StudentProfileViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var modelJson = HttpContext.Session.GetString("AccountModel");
            var model = modelJson == null ? null : JsonConvert.DeserializeObject<Account>(modelJson);

            return View(model); // Returns the Default.cshtml view
        }
    }

}
