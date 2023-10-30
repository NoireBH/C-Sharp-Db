using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaporStore.Data.Common
{
    public static class ValidationConstants
    {
        //User
        public const int UserUserNameMinLength = 3;
        public const int UserUserNameMaxLength = 20;
        public const string UserFullNameRegex = @"[A-Z][a-z]+ [A-Z][a-z]+";
        public const int UserAgeMin = 3;
        public const int UserAgeMax = 103;

        //Card
        public const string CardNumberRegex = @"^[\d]{4} [\d]{4} [\d]{4} [\d]{4}$";
        public const string CvcRegex = @"^[\d]{3}$";

        //Purchase
        public const string ProductKeyRegex = @"^[A-Z\d]{4}-[A-Z\d]{4}-[A-Z\d]{4}$";
    }
}
