using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Tech1Spellout.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string Amount { get; set; }
        public string Outcome { get; set; }
        public string ErrorMessage { get; set; }

        public void OnPost()
        {
            // Try to parse the Amount as a decimal.
            if (decimal.TryParse(Amount, out decimal amount))
            {
                // Check if the parsed amount exceeds the maximum allowed value for dollars - 2,147,483,647 (integer cap).
                if (amount > 2147483647)
                {
                    ErrorMessage = "The amount entered exceeds the maximum allowed value of 2,147,483,647 dollars.";
                }
                else
                {
                    // Convert the valid amount to words and store it in the Outcome property.
                    Outcome = ConvertToWords(amount);
                }
            }
            else
            {
                ErrorMessage = "Invalid input. Please enter a valid amount.";
            }
        }

        // Converts the amount (decimal) to words (e.g., "one hundred dollars and fifty cents").
        public string ConvertToWords(decimal amount)
        {
            // Handle the special case where the amount is zero.
            if (amount == 0)
                return "zero dollars";

            // Separate the dollars and cents from the amount.
            int dollars = (int)amount;
            int cents = (int)((amount - dollars) * 100);

            // Convert the cents to words, if any.
            string centsString = cents > 0 ? $"{ConvertWholeNumber(cents)} cents" : string.Empty;

            // If there are cents, return the full conversion with both dollars and cents.
            if (cents > 0)
            {
                return $"{ConvertWholeNumber(dollars)} dollars and {centsString}";
            }
            else
            {
                // If no cents, just return the dollars portion.
                return $"{ConvertWholeNumber(dollars)} dollars";
            }
        }

        // Converts a whole number (up to 999) to words (e.g., 145 "one hundred forty five").
        public string ConvertWholeNumber(int number)
        {
            // Handle the special case where the number is zero.
            if (number == 0) return "zero";

            // Arrays for number words for ones, tens, and thousands.
            string[] ones = { "", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten",
                              "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            string[] tens = { "", "", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
            string[] thousands = { "", "thousand", "million", "billion", "trillion" };

            string words = "";
            int thousandCounter = 0;

            // Break the number into thousands (e.g., 1,234 -> "one thousand two hundred thirty four").
            while (number > 0)
            {
                if (number % 1000 != 0)
                {
                    words = ConvertHundreds(number % 1000) + (thousands[thousandCounter] != "" ? " " + thousands[thousandCounter] : "") + " " + words;
                }
                number /= 1000;
                thousandCounter++;
            }

            return words.Trim();
        }

        // Converts a number between 1 and 999 to words (e.g., 342 -> "three hundred forty two").
        private string ConvertHundreds(int number)
        {
            string[] ones = { "", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten",
                              "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            string[] tens = { "", "", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

            string result = "";

            // Handle the hundreds part of the number (e.g., 342 -> "three hundred").
            if (number >= 100)
            {
                result += ones[number / 100] + " hundred ";
                number %= 100;
            }

            // Handle the tens part (e.g., 342 -> "forty").
            if (number >= 20)
            {
                result += tens[number / 10] + " ";
                number %= 10;
            }

            // Handle the ones part (e.g., 342 -> "two").
            if (number > 0)
            {
                result += ones[number] + " ";
            }

            // Return the result without trailing spaces.
            return result.Trim();
        }
    }
}