using Microsoft.AspNetCore.Mvc;

namespace TraversalCoreProje.ViewComponents.AdminDashboard
{
    public class _AdminRole : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
