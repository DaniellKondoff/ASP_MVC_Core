using CameraBazaar.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CameraBazaar.Web.ViewComponents
{
    [ViewComponent]
    public class Menu : ViewComponent
    {
        private readonly UserManager<User> userManager;

        public Menu(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task <IViewComponentResult> InvokeAsync()
        {
            string userName = this.User.Identity.Name;

            var user = await this.userManager.FindByNameAsync(userName);
            string userId = user.Id;
            return await Task.FromResult((IViewComponentResult)View("Default", userId));
        }
    }
}
