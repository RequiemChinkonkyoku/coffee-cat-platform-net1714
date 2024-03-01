using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeCatPlatform.Pages
{
    public class Index1Model : PageModel
    {
        [BindProperty]
        public string Text { get; set; }

        public void OnGet(string text)
        {
        }
        public IActionResult OnPost()
        {
            return RedirectToPage("/Index1", new { text = Text });
        }
    }
}
