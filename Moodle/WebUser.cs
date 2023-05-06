using Server.Moodle;
using System.Runtime.InteropServices;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using Server.Email;
using Microsoft.Extensions.Options;
using Server.Moodle.DAL;

namespace Server.Moodle
{
    public class WebUser
    {
        public string First { get; private set; }
        public string Last { get; private set; }
        //unique
        public int Id { get; private set; }
        public string Country { get; private set; }
        // regex in this user file is for protection not for validation in first place
        //reg  /^([a-z\d\.-]+)@([a-z\d-]+)\.([a-z]{2,8})(\.[a-z]{2,8})?$/g
        public string Email { get; private set; }
        // check
        public string Password { get; private set; }
        // israel +972 [0-9]{9}
        public string PhoneNumber { get; private set; }
        // public string Profile_img{ get; private set; }
        private string ResetUrlPar { get; set; }
        public static List<WebUser> UsersList = new List<WebUser>();

        public WebUser(string first, string last, int id, string country, string email, string password, string phoneNumber)
        {
            First = first;
            Last = last;
            Id = id;
            Country = country;
            Email = email;
            Password = password;
            PhoneNumber = phoneNumber;
            ResetUrlPar = generateOneTimeResetUrl();
            // Task.Run(() => sendEmail(email));
            //sendEmail(email);
        }
        public static WebUser? GetById(string id)
        {
            DBservices dBservices = new DBservices();
            return dBservices.getUserById(id);
        }

        public static List<WebUser> Read()
        {
            DBservices dBservices = new DBservices();
            return dBservices.GetAll();
        }
        public static WebUser? GetByemail(string email)
        {
            DBservices dBservices = new DBservices();
            return dBservices.GetByemail(email);
        }
        public static int SetKeyAndDate(int id)
        {
            DBservices dBservices = new DBservices();
            string key = generateOneTimeResetUrl();
            DateTime date = DateTime.Now;
            return dBservices.SetKeyAndDate(key,date,id);
        }

        public bool checkPassowrdValdition(string password)
        {
            return password == this.Password;
        }
        public bool Registration()
        {
            DBservices dBservices = new DBservices();
            return dBservices.InsertUser(this);
        }

        public static WebUser LogInPost(string email, string password)
        {
            DBservices dBservices = new DBservices();
            return dBservices.LogInPost(email, password);
        }
        public async Task<bool> resetPassword(string uniqueUrlPar, string newPassword, WebUser user)
        {

            this.Password = newPassword;
            this.ResetUrlPar = generateOneTimeResetUrl();
            await sendEmail(this.Email , user);
            return true;

        }
        private static string generateOneTimeResetUrl()
        {
            Random random = new Random();
            int randomNumber = random.Next(100000, 999999); 
            return randomNumber.ToString();
        }
        private async Task sendEmail(string email, WebUser user)
        {
            // your email format regex pattern
            string pattern = @"^([a-z\d\.-]+)@([a-z\d-]+)\.([a-z]{2,8})(\.[a-z]{2,8})?$";

            if (!Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase))
            {
                return ;
            }

            var smtpSettings = new SmtpSettings("smtp.elasticemail.com", 587, true, "amit.khaled.airbnb@gmail.com", "6BA88EB97CC6AE035885DC0CD3A95BB30CC8");
            var emailSender = new EmailSender(Options.Create(smtpSettings));
            string subject = "Your unique key";
            string message = $@"
            <!DOCTYPE html>
            <html lang=""en"">
            <body style=""background-color: rgba(0, 0, 0, 0.761);"">
            <div style=""text-align: center;"">
                <img src=""https://cdn.freebiesupply.com/logos/large/2x/airbnb-2-logo-svg-vector.svg"" alt=""
                    style=""object-fit: contain; max-height: 250px;"">
                <h1 style=""color:white; text-align: center;"">Hello {user.First} {user.Last}, This is the 6 digits code that you can use for
                    reseting your password</h1>
                <p style=""color: white;"">select the numbers to copy</p>
                <div style=""display:flex; justify-content: center; align-items: center;"">
                    <div
                        style=""display: inline-flex; justify-content: center; align-items: center; background-color: #fe7d7dA2;  font-size: 26px; color: white; padding: 10px; box-shadow: inset 0 0 10px white; border-radius: 5px;"">
                        &#128203;
                        <input type=""text"" id=""myInput"" value=""{user.ResetUrlPar}"" readonly
                            style=""cursor: copy; border-style: none;  background-color: transparent; font-size: 26px; letter-spacing: 8px; color: whitepadding: 10px;""
                            size=""5"" onselect='document.execCommand(""copy"")' >
                    </div>
                </div>
                <p style=""color: white;"">This code will be useable for 30 minutes if you didnt use it in the comming 30 minutes
                    it will expeared.
                </p>
            </div>
            </body>
            </html>";
            await emailSender.SendEmailAsync(email, subject, message);
            /*
            var smtpClient = new SmtpClient("smtp.elasticemail.com")
            {
                Credentials = new NetworkCredential("amit.khaled.airbnb@gmail.com", "6BA88EB97CC6AE035885DC0CD3A95BB30CC8"),
                EnableSsl = true,
               
            };
            //create the mail message
            MailMessage mail = new MailMessage();

            //set the addresses
            mail.From = new MailAddress("amit.khaled.airbnb@gmail.com");
            mail.To.Add(email);
            mail.Subject = "Your unique key";
            string s = "<div><h1>Hi ,"+ this.First + " " + this.Last +"</h1><p>This is your unique key to reset your password keep it save and dont loose it</p><div>" + this.ResetUrlPar + "</div></div>";
            mail.Body = s;
            mail.IsBodyHtml = true;
            // after that use your SmtpClient code to send the email
            smtpClient.SendMailAsync(mail);
            */
        }
        public static bool Update(WebUser user)
        {
            DBservices dBservices = new DBservices();
            return dBservices.UpdateUser(user);
        }
        public static bool Delete(string email)
        {
            DBservices dBservices = new DBservices();
            return dBservices.DeleteUser(email);
        }
    }
}
