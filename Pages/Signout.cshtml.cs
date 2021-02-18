using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace myapp.Pages {
  public class SignoutModel : PageModel {
    public async Task OnGet () {
      await HttpContext.SignOutAsync (CookieAuthenticationDefaults.AuthenticationScheme);
      await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
    }
  }
}