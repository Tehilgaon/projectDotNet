using System;
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
using System.Xml.Serialization;
 
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
        public static bool SendEmail(string guestMail, string hostMail)
        {
            try
            {
                var client = new SmtpClient(BE.Configuration.SMTP_Server)
                {
                    Credentials = new NetworkCredential(Configuration.MailSystem, Configuration.Password),
                    EnableSsl = true
                };
                MailMessage mailMessage = new MailMessage(hostMail,guestMail) { Body ="nothing", IsBodyHtml = true, Subject = "הגיעה אליך הזמנה "};
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception( ex.Message);
                return false;
            }
        }

        /*public static  List<DateTime> Flatten(HostingUnit hostingUnit)
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
        }*/

        public static T[] Flatten<T>(this T[,] arr)
        {
            int rows = arr.GetLength(0);
            int columns = arr.GetLength(1);
            T[] arrFlattened = new T[rows * columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    var test = arr[i, j];
                    arrFlattened[i * rows + j] = arr[i, j];
                }
            }
            return arrFlattened;
        }
        public static T[,] Expand<T>(this T[] arr, int rows)
        {
            int length = arr.GetLength(0);
            int columns = length / rows;
            T[,] arrExpanded = new T[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    arrExpanded[i, j] = arr[i * rows + j];
                }
            }
            return arrExpanded;
        }

        public static T LoadFromXML<T>(string path)
        {
            FileStream file = new FileStream(path, FileMode.Open);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            T result = (T)xmlSerializer.Deserialize(file);
            file.Close();
            return result;
        }

        public static void SaveToXML<T>(T source, string path)
        {
            FileStream file = new FileStream(path, FileMode.Create);
            XmlSerializer xmlSerializer = new XmlSerializer(source.GetType());
            xmlSerializer.Serialize(file, source); file.Close();
        }
    }
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




