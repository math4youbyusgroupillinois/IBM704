using System;
using System.Collections.Generic;
using System.Text;

namespace IBM704.Cpu
{
    public static class Settings
    {
        public static readonly int ADDRESS_INDEX = 21;
        public static readonly string ZERO_WORD_12 = "000000000000";
        public static readonly string ZERO_WORD_13 = "0000000000000";
        public static readonly string ZERO_WORD_15 = "000000000000000";
        public static readonly string ZERO_WORD_36 = "000000000000000000000000000000000000";
        public static readonly string ZERO_WORD_38 = "00000000000000000000000000000000000000";
        public static readonly int ACC_REG_SIZE = 38;
        public static readonly int STD_WORD_SIZE = 36;
        public static readonly int INT_COUNTER_WORD_SIZE = 15;

        public static readonly int INDEX_A_SIZE = 12;
        public static readonly int INDEX_B_SIZE = 13;
        public static readonly int INDEX_C_SIZE = 15;

        /// <summary>
        /// This is the number of instructions that will be executed when the multistep key is pressed.
        /// </summary>
        public static readonly int MULTISTEP_NUM = 5;
    }
}
