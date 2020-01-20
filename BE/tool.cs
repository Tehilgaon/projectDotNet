﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Mail;
using System.Net;
using System.Xml.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.Mail;
namespace BE
{
    public static class tool
    {
        static DateTime today = DateTime.Today;
         static DateTime aYearFromNow = DateTime.Today.AddYears(1);
       
        public static T Clone<T>(this T source)
        {
            var isNotSerializable = !typeof(T).IsSerializable;
            if (isNotSerializable)
                throw new ArgumentException("The type must be serializable.", "source");
            var sourceIsNull = ReferenceEquals(source, null);
            if (sourceIsNull)
                return default(T);
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        public static  List<DateTime> Flatten(HostingUnit hostingUnit)
        {
            List<DateTime> DairySer = new List<DateTime>();
            if (hostingUnit[today] == true)
                DairySer.Add(today);
            for (DateTime day = DateTime.Today; day < aYearFromNow; day = day.AddDays(1))
            {
                if (hostingUnit[day] == false && hostingUnit[day.AddDays(1)] == true)
                    DairySer.Add(day.AddDays(1));
                if (hostingUnit[day] == true && hostingUnit[day.AddDays(1)] == false)
                    DairySer.Add(day);
            }
            if (hostingUnit[aYearFromNow] == true)
                DairySer.Add(aYearFromNow);
            return DairySer;
        }

        public static 
        /* public static bool SendingEmail(string recipients, string subject, string body)
         {
             try
             {
                 var client = new SmtpClient(BE.Configuration.SMTP_Server, BE.Configuration.SMTP_Port)
                 {
                     Credentials = new NetworkCredential(Configuration.SenderEmailAddress, Configuration.EmailServerPasword),
                     EnableSsl = true
                 };
                 MailMessage mailMessage = new MailMessage(recipients, recipients) { Body = body, IsBodyHtml = true, Subject = subject, };
                 client.Send(mailMessage);
                 return true;
             }
             catch (Exception)
             {
                 return false;
             }
         }*/

    }



}
