using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static MusicStore.Web.Infrastructure.Common.WebConstants;

namespace MusicStore.Web.Areas.Admin.Controllers
{
    [Area(AdminArea)]
    [Authorize(Roles = AdministratingRole)]
    public class BaseAdminController : Controller
    {
    }
}
