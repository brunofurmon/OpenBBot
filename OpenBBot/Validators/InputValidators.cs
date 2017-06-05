using System;


namespace OpenBBot.Validators
{
    public static class InputValidators
    {
        public static bool ValidateAgainstNumber(string text)
        {
            if (string.IsNullOrEmpty(text) || !char.IsDigit(text, text.Length - 1))
            {
                return false;
            }
            return true;
        }
        public static bool IsInsideLimits(Int32 number, int min = Int32.MinValue, int max = Int32.MaxValue)
        {
            bool isInsideLimits = number <= min && number <= max;
            return isInsideLimits;
        }
    }
}
