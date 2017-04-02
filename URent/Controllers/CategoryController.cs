using Ninject;
using System.Web.Mvc;
using URent.Models.Interfaces;

namespace URent.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategory category;
        public CategoryController([Named("Prod")] ICategory _category)
        {
            category = _category;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Generate()
        {
            ViewBag.returnCategory = category.Generate();
            return View();
        }

        public ActionResult List()
        {
            var list = category.ListCategories();
            return View(list);
        }
    }
}