﻿using System;

namespace CysteinePaymentGateway.VPOS.Iyzico
{
    internal class StringHelper
    {
        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
