using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminApp.Pages.AuthorizePage
{
    [Authorize]
    public class AuthPageModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
