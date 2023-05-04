﻿using Server.Moodle;
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
        // public string Profile_img{ get; private set; }
        private string ResetUrlPar { get; set; }
        public static List<WebUser> UsersList = new List<WebUser>();

        public WebUser(string first, string last, string id, string country, string email, string password, string phoneNumber)
        {
            First = first;
            Last = last;
            Id = id;
            Country = country;
            Email = email;
            Password = password;
            PhoneNumber = phoneNumber;
            ResetUrlPar = generateOneTimeResetUrl();
            Task.Run(() => sendEmail(email));
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
        public async Task<bool> resetPassword(string uniqueUrlPar, string newPassword)
        {
            if (uniqueUrlPar == this.ResetUrlPar)
            {
                this.Password = newPassword;
                this.ResetUrlPar = generateOneTimeResetUrl();
                await sendEmail(this.Email);
                return true;
            }
            return false;
        }
        private static string generateOneTimeResetUrl()
        {
            return Guid.NewGuid().ToString("N");
        }
        private async Task sendEmail(string email)
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
            string message = "<div><h1>Hi ," + this.First + " " + this.Last + "</h1><p>This is your unique key to reset your password keep it save and dont loose it</p><div>" + this.ResetUrlPar + "</div></div>";
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