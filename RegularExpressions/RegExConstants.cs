namespace Utilities.RegularExpressions
{
    public class RegExConstants
    {
        #region Postal Codes

        public const string PostalCodePattern = @"^[A-Z]\d[A-Z] \d[A-Z]\d$";
        public const string PostalCodePlaceholder = "R2A 3B4";
        public const string PostalCodeErrorMessage = "Postal code must be formatted R2A 3B4";

        #endregion

        #region Phone Numbers

        public const string PhoneNumberPattern = @"^\d{3}-\d{3}-\d{4}$";
        public const string PhoneNumberPlaceholder = "204-234-5678";
        public const string PhoneNumberErrorMessage = "Phone number must be formatted 204-234-5678";

        #endregion
    }
}
