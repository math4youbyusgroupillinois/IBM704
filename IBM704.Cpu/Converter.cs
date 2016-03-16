using System;
using System.Collections.Generic;
using System.Text;

namespace IBM704.Cpu
{
    public static class Converter
    {
        #region Declarations
        
        private const int base10 = 10;
        private static char[] cHexa = new char[] { 'A', 'B', 'C', 'D', 'E', 'F' };
        private static int[] iHexaNumeric = new int[] { 10, 11, 12, 13, 14, 15 };
        private static int[] iHexaIndices = new int[] { 0, 1, 2, 3, 4, 5 };
        private const int asciiDiff = 48;

        #endregion

        #region Public Methods

        /// <summary>
        /// Will convert an integer into a Binary String.
        /// </summary>
        /// <param name="number"></param>
        /// <returns>Returns a string in a format similar to "01001"</returns>
        public static string ConvertToBinary(long number)
        {
            return Convert.ToString(number, 2);
        }

        /// <summary>
        /// Pads a string wilh 0's at the beginning in order to reach a desired length, i.e. number of bits.
        /// For Example: Passing ("1", 3) will return "001".
        /// </summary>
        /// <param name="input">The binary string to pad. Example "011001"</param>
        /// <param name="desiredLength">The length, in number of bits that should be returned</param>
        /// <returns>The padded string</returns>
        public static string PadBinaryString(string input, int desiredLength)
        {
            int difference = desiredLength - input.Length;

            for (int i = 0; i < difference; i++)
            {
                input = "0" + input;
            }

            return input;
        }

        /// <summary>
        /// Pads a string wilh 0's at the beginning in order to reach a desired length, i.e. number of bits.
        /// For Example: Passing ("1", 3) will return "001".
        /// </summary>
        /// <param name="input">The octal string to pad. Example "011001"</param>
        /// <param name="desiredLength">The length, in number of bits that should be returned</param>
        /// <returns>The padded string</returns>
        public static string PadOctalString(string input, int desiredLength)
        {
            int difference = desiredLength - input.Length;

            for (int i = 0; i < difference; i++)
            {
                input = "0" + input;
            }

            return input;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="desiredLength">The number of minimum bit the result should have. This will pad the result with 0's</param>
        /// <returns></returns>
        public static string ConvertToBinary(long number, int desiredLength)
        {
            string result = Convert.ToString(number, 2);

            result = PadBinaryString(result, desiredLength);

            return result;
        }

        public static string ConvertToBinary(string octal, int desiredLength)
        {
            long intResult = BaseToDecimal(octal, 8);
            string binaryResult = ConvertToBinary(intResult, desiredLength);

            return binaryResult;
        }

        /// <summary>
        /// Converts a binary string to an octal string.
        /// </summary>
        /// <param name="binary"></param>
        /// <returns></returns>
        public static string ConvertToOctal( string binary)
        {
            //TODO: Rewrite to take the sign into effect

            long  result = BaseToDecimal(binary, 2);
            return DecimalToBase(result, 8);
        }

        public static string ConvertToOctal( string binary, int desiredLength)
        {
            long decResult = BaseToDecimal(binary, 2);
            string result = DecimalToBase(decResult, 8);

            return PadOctalString(result, desiredLength);
        }

        /// <summary>
        /// Converts a binary string to an integer value.
        /// </summary>
        /// <param name="binary">The string, in a binary format "010011", that needs to be converted. 
        /// If the word is 36 bits long, then bit 0 is considered a sign bit with a value of "1" meaning "-".</param>
        /// <returns></returns>
        public static long ConvertToInt(string binary)
        {
            //If the word is 36 bits long, then the first bit is a sign bit.
            string sign = "0";

            if(binary.Length == Settings.STD_WORD_SIZE)
            {
                sign = binary[0].ToString();
                binary = binary.Substring(1);
            }

            long result = BaseToDecimal(binary, 2);

            if (sign == "1")
                result *= -1;

            return result;
        }

        #endregion

        #region Private Methods

        private static string DecimalToBase(long iDec, int numbase)
        {
            string strBin = "";
            long[] result = new long[64];
            int MaxBit = 64;
            for (; iDec > 0; iDec /= numbase)
            {
                long rem = iDec % numbase;
                result[--MaxBit] = rem;
            }
            for (int i = 0; i < result.Length; i++)
                if ((long)result.GetValue(i) >= base10)
                    strBin += cHexa[(int)result.GetValue(i) % base10];
                else
                    strBin += result.GetValue(i);
            strBin = strBin.TrimStart(new char[] { '0' });
            return strBin;
        }

        private static long BaseToDecimal(string sBase, int numbase)
        {
            long dec = 0;
            long b;
            int iProduct = 1;
            string sHexa = "";
            if (numbase > base10)
                for (int i = 0; i < cHexa.Length; i++)
                    sHexa += cHexa.GetValue(i).ToString();
            for (int i = sBase.Length - 1; i >= 0; i--, iProduct *= numbase)
            {
                string sValue = sBase[i].ToString();
                if (sValue.IndexOfAny(cHexa) >= 0)
                    b = iHexaNumeric[sHexa.IndexOf(sBase[i])];
                else
                    b = sBase[i] - asciiDiff;
                dec += (b * iProduct);
            }
            return dec;
        }

        #endregion 
    }
}
