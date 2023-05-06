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
        public string Id { get; private set; }
        public string Country { get; private set; }
        // regex in this user file is for protection not for validation in first place
        //reg  /^([a-z\d\.-]+)@([a-z\d-]+)\.([a-z]{2,8})(\.[a-z]{2,8})?$/g
        public string Email { get; private set; }
        // check
        public string Password { get; private set; }
        // israel +972 [0-9]{9}
        public string PhoneNumber { get; private set; }
        public string Profile_img{ get; private set; }
        //private string ResetUrlPar { get; set; }

        public WebUser(string first, string last, string id, string country, string email, string password, string phoneNumber, string profile_img)
        {
            First = first;
            Last = last;
            Id = id;
            Country = country;
            Email = email;
            Password = password;
            PhoneNumber = phoneNumber;
            Profile_img = profile_img;
            //ResetUrlPar = generateOneTimeResetUrl();
            //Task.Run(() => sendEmail(email , this));
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
        public static WebUser? checkIfKeyCorrect(string key, string id)
        {
            DBservices dBservices = new DBservices();
            return dBservices.checkIfKeyCorrect(key, id);
        }
        public async static Task<WebUser?> GetByemail(string email)
        {
            DBservices dBservices = new DBservices();
            WebUser user = dBservices.GetByemail(email);
            await SetKeyAndEmail(user);
            return user;
        }

        public async static Task<bool> SetKeyAndEmail(WebUser user)
        {
            DBservices dBservices = new DBservices();
            string key = generateOneTimeResetUrl();
            DateTime date = DateTime.Now;
            await user.sendEmail(user.Email, user, key);
            return dBservices.SetKeyAndDate(key,date, user.Id);
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
        /*
        public async Task<bool> resetPassword(string uniqueUrlPar, string newPassword , string key, WebUser user)
        {
            if (uniqueUrlPar == this.ResetUrlPar)
            {
                this.Password = newPassword;
                this.ResetUrlPar = key;
                await sendEmail(user.Email, user);
                return true;
            }
            return false;
        }
        */
        private static string generateOneTimeResetUrl()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 6);
        }
        private async Task sendEmail(string email, WebUser user, string key)
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
            string copyCodeScript = @"
            <script>
            function copyCode() {
                const input = document.querySelector('#myInput');
                input.select();
                document.execCommand('copy');
            }
            </script>";
            string message = $@"
            <!DOCTYPE html>
            <html lang=""en"">
            <body style=""background-color: rgba(0, 0, 0, 0.761);"">
            <div style=""text-align: center;"">
                <img src=""https://cdn.discordapp.com/attachments/1104395876857819199/1104396286314160218/cdnlogo.com_airbnb.png"" alt="""" style=""object-fit: contain; max-height: 250px;"">
                <h1 style=""color:white; text-align: center;"">Hello {user.First} {user.Last}, This is the 6 digits code that you can use for
                    reseting your password</h1>
                <p style=""color: white;"">select the numbers to copy</p>
                <div style=""display:flex; justify-content: center; align-items: center;"">
                    <div style="" margin:0 auto; display: inline-flex; justify-content: center; align-items: center; background-color: #fe7d7dA2;  font-size: 26px; color: white; padding: 10px; box-shadow: inset 0 0 10px white; border-radius: 5px;"">
                        &#128203;
                        <input type=""text"" id=""myInput"" value=""{key}"" readonly
                            style="" cursor: copy; border-style: none;  background-color: transparent; font-size: 26px; letter-spacing: 8px; color: whitepadding: 10px;""
                            size=""5"" onclick=""copyCode()"" >
                    </div>
                </div>
                <p style=""color: white;"">This code will be useable for 30 minutes if you didnt use it in the comming 30 minutes
                    it will expeared.
                </p>
            </div>
            {copyCodeScript}
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
