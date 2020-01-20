﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    [Serializable]
    public class Configuration
    {
        private static long guestRequestKey= 1000000;
        private static long hostingUnitKey=1000000;
        private static long orderKey=2005000;
        private static int fee = 10;
        private static string mng="0000";
        private static int orderValidity=14;
        public static string TypeDAL = "XML";
        private string sMTP_Server="smtp.gmail.com";

        internal static long GuestRequestKey { get => guestRequestKey; set => guestRequestKey = value; }
        internal static long HostingUnitKey { get => hostingUnitKey; set => hostingUnitKey+=1; }
        internal static long OrderKey { get => orderKey; set => orderKey+=1; }
        public static int Fee { get => fee; set => fee = value; }
        public static string Mng { get => mng;  }
        public static int OrderValidity  { get => orderValidity; set => orderValidity = value; }
        public string SMTP_Server { get => sMTP_Server; set => sMTP_Server = value; }
    }
}
