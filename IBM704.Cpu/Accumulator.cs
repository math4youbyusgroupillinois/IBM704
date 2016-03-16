using System;
using System.Collections.Generic;
using System.Text;

namespace IBM704.Cpu
{
    /// <summary>
    /// Accumulator (AC). The accumulator is a register with a capacity of 37 bits and a sign. 
    /// Nearly every arithmetic operation involves the accumulator. In some operations (for instance, addi¬tion, shifting left) 
    /// it is possible that the contents of the accumulator will overflow positions 1-35. When an overflow occurs, with the exception 
    /// of overflow caused  by the  ACL instruction,  the  AC OVERFLOW indicator is turned on. Certain instructions permit the program 
    /// to sense the condition of the overflow indicator while the program is being performed. The programmer may preserve some of the 
    /// overflow in¬formation if he wishes. For this purpose, two extra bit positions, or overflow positions, are provided. These are 
    /// designated the P and Q positions. When two numbers having different signs but the same magnitude are added algebraically in 
    /// the AC, it is important to know if the result is + 0 or — 0, since + 0 is considered larger than — 0. In this case, the sign 
    /// of the result is identical to the sign of the number in the AC before the addition took place.
    /// 
    /// the order of the word bits is S Q P 1-35.
    /// </summary>
    public class Accumulator
    {
        private string _value;

        public Accumulator()
        {
            Value = Settings.ZERO_WORD_38;
        }

        public string PBit
        {
            get
            {
                return _value[2].ToString();
            }
            set
            {
                if (value.Length != 1)
                    throw new Exception("Attempted to assign a string that was not one character to the ACC PBit.");

                char[] temp = Value.ToCharArray();
                temp[2] = value[0];

        

                Value = new string(temp); 
            }
        }

        public string QBit
        {
            get
            {
                return Value[1].ToString();
            }
            set
            {
                if (value.Length != 1)
                    throw new Exception("Attempted to assign a string that was not one character to the ACC QBit.");

                char[] temp = Value.ToCharArray();
                temp[1] = value[0];

                Value = new string(temp);
            }
        }

        public string SBit
        {
            get
            {
                return Value[0].ToString();
            }
            set
            {
                if (value.Length != 1)
                    throw new Exception("Attempted to assign a string that was not one character to the ACC SBit.");

                char[] temp = Value.ToCharArray();
                temp[0] = value[0];

                Value = new string(temp);
            }
        }

        /// <summary>
        /// Returns a 36 "Bit" string
        /// </summary>
        public string Bits_SignTo35
        {
            get
            {
                string signBit = Value.Substring(0, 1);
                string OneTo35 = Value.Substring(3, 35);

                return signBit + OneTo35;
            }
            set
            {
                if (value.Length != Settings.STD_WORD_SIZE)
                    throw new Exception("Attempted to assign a string that was not the correct size to the ACC Reg");

                char[] temp = Value.ToCharArray();
                Value = value[0].ToString() + temp[1].ToString() + temp[2].ToString() + value.Substring(1,35);
            }
        }

        /// <summary>
        /// Returns a 38 "Bit" string in a binary format ("001001") that represents the current binary value of the register 
        /// </summary>
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value.Length != Settings.ACC_REG_SIZE)
                    throw new Exception("Attempted to put an incorrect size word in the accumulator");

                _value = value;
            }
        }
    }
}
