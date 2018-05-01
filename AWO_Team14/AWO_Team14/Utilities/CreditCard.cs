using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AWO_Team14.Utilities
{
    public class CreditCard
    {
        public static String GetCreditCardType (String creditcard)
        {
            if (creditcard.Length == 15)
            {
                return "Amex";
            }
            else if (creditcard[0] == '4')
            {
                return "Visa";
            }
            else if (creditcard.Substring(0,2) == "54")
            {
                return "MasterCard";
            }
            else if (creditcard[0] == '6')
            {
                return "Discover";
            }
            else
            {
                return "Invalid";
            }
        }
    }
}