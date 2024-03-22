using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using Repositories.Impl;
using System.Net;
using System.Net.Mail;

namespace CoffeeCatPlatform.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public Customer Customer { get; set; }

        [BindProperty]
        public string? Message { get; set; }

        private readonly IRepositoryBase<Customer> _customerRepo;

        private string _email;
        private string _password;

        private string _verificationToken;

        public RegisterModel()
        {
            _customerRepo = new CustomerRepository();
        }

        public void OnGet(string message)
        {
            Message = message;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingEmail = _customerRepo.GetAll().FirstOrDefault(c => c.Email.Equals(Customer.Email));
            if (existingEmail != null)
            {
                ModelState.AddModelError("Customer.Email", "Email is already in use.");
                return Page();
            }

            _email = GetEmail();
            _password = GetPassword();

            if (_customerRepo.GetAll() == null || Customer == null)
            {
                return Page();
            }
            Customer.Status = 0;
            _customerRepo.Add(Customer);

            string token = Customer.CustomerId + Customer.Email + Customer.Password;
            _verificationToken = BCrypt.Net.BCrypt.HashString(token);

            string verify = SendVerificationEmail(_email, Customer.Email, "Verification Email",
                "http://localhost:5117/AccountVerification?_verificationToken=" + _verificationToken);
            return RedirectToPage("/Register", new { message = verify });
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
                return "An verification email has been sent to your inbox.";
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
