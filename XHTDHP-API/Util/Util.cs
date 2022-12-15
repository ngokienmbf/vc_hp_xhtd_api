using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Globalization;
using XHTDHP_API.Models;

namespace XHTDHP_API.Utils
{
    public class Util
    {
        public static bool IsEmailOrPhone(string input)
        {
            if (IsEmail(input)) return true;
            if (IsPhoneNumber(input)) return true;
            return false;
        }
        public static bool IsEmail(string email)
        {
            try
            {
                //var addr = new System.Net.Mail.MailAddress(email);
                //return addr.Address == email;
                string pattern = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-\.]{1,}$";
                var regex = new Regex(pattern, RegexOptions.IgnoreCase);

                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }
        public static bool IsPhoneNumber(string number)
        {
            if (String.IsNullOrEmpty(number)) return false;
            string RegexE;
            List<string> lst = PhoneList();
            foreach (var p in lst)
            {
                RegexE = @"^" + p + @"([0-9]{1,7})$";
                if (Regex.Match(number, RegexE).Success)
                {
                    return true;
                }
            }

            return false;
        }

        public static List<string> PhoneList()
        {
            List<string> lst = new List<string>();
            //Viettel
            lst.Add("032");
            lst.Add("033");
            lst.Add("034");
            lst.Add("035");
            lst.Add("036");
            lst.Add("037");
            lst.Add("038");
            lst.Add("039");
            lst.Add("086");
            lst.Add("096");
            lst.Add("097");
            lst.Add("098");
            //MobilePhone
            lst.Add("070");
            lst.Add("079");
            lst.Add("077");
            lst.Add("076");
            lst.Add("078");
            lst.Add("089");
            lst.Add("090");
            lst.Add("093");
            //VinaPhone
            lst.Add("083");
            lst.Add("084");
            lst.Add("085");
            lst.Add("081");
            lst.Add("082");
            lst.Add("088");
            lst.Add("091");
            lst.Add("094");
            //VietNamMobile
            lst.Add("092");
            lst.Add("056");
            lst.Add("058");
            //GPhone
            lst.Add("099");
            lst.Add("059");
            return lst;

        }
        
        public static string GenerateJsonContentToppic(string Title, string Body, string Content, string NotifyType, string Topic, string FormId, int? ParameterId, string FullUrl, string FullUrlImage)
        {
            string Result = "";
            return Result;
        }

        public static void SendMail(string FromName, string ToEmail, string ToName, string Subject, string Body, EmailConfig setting)
        {


            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = setting.EmailSenderSmtp;
            smtpClient.Port = int.Parse(setting.EmailSenderPort);
            smtpClient.EnableSsl = setting.EmailSenderSsl.Value;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            smtpClient.Credentials = new NetworkCredential(setting.EmailSender, setting.EmailSenderPassword);
            smtpClient.Timeout = 20000;

            MailAddress FromAddress = new MailAddress(setting.EmailSender, setting.AdminName);
            MailAddress ToAddress = new MailAddress(ToEmail, ToName);
            MailMessage Mailer = new MailMessage(FromAddress, ToAddress);
            Mailer.IsBodyHtml = true;
            Mailer.BodyEncoding = System.Text.Encoding.UTF8;
            Mailer.Subject = Subject;
            Mailer.Body = Body;
            smtpClient.Send(Mailer);
        }


        public static string ShowNumber(object Number, int NumberDecimalDigits)
        {
            string NumberString = "0";
            if (Number != null && Number.ToString() != string.Empty)
            {
                NumberFormatInfo myNumberFormat = new CultureInfo("vi-VN").NumberFormat;
                myNumberFormat.NumberGroupSeparator = ".";
                myNumberFormat.NumberDecimalDigits = NumberDecimalDigits;

                NumberString = double.Parse(Number.ToString()).ToString("N", myNumberFormat);
            }
            return NumberString;
        }


    }
}
