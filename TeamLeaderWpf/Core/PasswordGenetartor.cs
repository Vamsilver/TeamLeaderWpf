using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamLeaderWpf.Core
{
    public class PasswordGenetartor
    {
        const string LOWER_CASE = "abcdefghijklmnopqursuvwxyz";
        const string UPPER_CAES = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string NUMBERS = "123456789";
        const string SPECIALS = @"!@£$%^&*()#€";


        public string GeneratePassword(bool useLowercase, bool useUppercase, bool useNumbers, bool useSpecial,
            int passwordSize)
        {
            char[] _password = new char[passwordSize];
            string charSet = "";
            System.Random _random = new Random();
            int counter;

            if (useLowercase) charSet += LOWER_CASE;

            if (useUppercase) charSet += UPPER_CAES;

            if (useNumbers) charSet += NUMBERS;

            if (useSpecial) charSet += SPECIALS;

            for (counter = 0; counter < passwordSize; counter++)
            {
                _password[counter] = charSet[_random.Next(charSet.Length - 1)];
            }

            return String.Join(null, _password);
        }
    }
}
