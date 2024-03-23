using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCatPlatform.Pages.Shared
{
    public class CustomerAuthModel : PageModel
    {
        private const string SessionKeyName = "_Name";
        private const string SessionKeyId = "_Id";
        private const string SessionKeyType = "_Type";

        public IActionResult CustomerAuthorize()
        {
            if (SessionCheck() == false)
            {
                return RedirectToPage("/ErrorPages/NotLoggedInError");
            }
            else if (CustomerCheck() == false)
            {
                return RedirectToPage("/ErrorPages/NotAuthorizedError");
            }
            return null;
        }

        private bool SessionCheck()
        {
            bool result = true;
            if (String.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName))
                && String.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyId))
                && String.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyType)))
            {
                result = false;
            }
            return result;
        }

        private bool CustomerCheck()
        {
            bool result = false;
            if (HttpContext.Session.GetString(SessionKeyType) == "Customer"
                || HttpContext.Session.GetString(SessionKeyType) == "Admin")
            {
                result = true;
            }
            return result;
        }
    }
}
