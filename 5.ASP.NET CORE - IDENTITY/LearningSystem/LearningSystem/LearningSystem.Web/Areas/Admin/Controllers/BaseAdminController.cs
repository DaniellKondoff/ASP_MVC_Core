using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static LearningSystem.Web.Common.WebConstants;

namespace LearningSystem.Web.Areas.Admin.Controllers
{
    [Area(AdminArea)]
    [Authorize(Roles = AdministratingRole)]
    public abstract class BaseAdminController : Controller
    {
    }
}
