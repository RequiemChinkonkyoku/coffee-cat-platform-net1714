using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using Repositories.Impl;
using System.Net.Mail;
using System.Net;

namespace CoffeeCatPlatform.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly IRepositoryBase<Customer> _customerRepo;

        private string _email;
        private string _password;

        private string _verificationToken;

        [BindProperty]
        public string? Message { get; set; }

        [BindProperty]
        public string? Email { get; set; }

        public ForgotPasswordModel()
        {
            _customerRepo = new CustomerRepository();
        }

        public void OnGet(string message)
        {
            Message = message;
        }

        public IActionResult OnPost(string message)
        {
            /*if (!ModelState.IsValid)
            {
                return Page();
            }*/

            var existingEmail = _customerRepo.GetAll().FirstOrDefault(c => c.Email.Equals(Email));
            if (existingEmail == null)
            {
                ModelState.AddModelError("Customer.Email", "This email has not registered!");
                return Page();
            }

            _email = GetEmail();
            _password = GetPassword();

            if (_customerRepo.GetAll() == null || Email == null)
            {
                return Page();
            }

            string token = Email + DateTime.Today.ToString();
            _verificationToken = BCrypt.Net.BCrypt.HashString(token);

            string verify = SendVerificationEmail(_email, Email, "Verification Email",
                "http://localhost:5117/ChangeCustomerPassword?_verificationToken=" + _verificationToken);
            return RedirectToPage("/ForgotPassword", new { message = verify });
        }

        private string SendVerificationEmail(string _from, string _to, string _subject, string _body)
        {
            MailMessage message = new MailMessage(_from, _to, _subject, _body);

            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            message.ReplyToList.Add(new MailAddress(_from));
            message.Sender = new MailAddress(_from);

            using var smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(_email, _password);

            try
            {
                smtpClient.Send(message);
                return "An verification email has been sent to your inbox. The link will only be valid for today.";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Email cannot be sent.";
            }
        }

        private string GetEmail()
        {
            IConfiguration config = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", true, true)
        .Build();
            var data = config["GmailSender:Email"];

            return data;
        }

        private string GetPassword()
        {
            IConfiguration config = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", true, true)
        .Build();
            var data = config["GmailSender:Password"];

            return data;
        }
    }
}
