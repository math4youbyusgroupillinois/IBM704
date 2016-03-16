using System;
using System.Collections.Generic;
using System.Text;

namespace IBM704.Cpu
{
    /// <summary>
    /// Multiplier-Quotient Register (MQ). The MQ is a register with a capacity of 35 bits plus sign. It has five major uses:
    ///1.	During the execution of every CPY instruction, the MQ is used as a buffer between core storage and any of the other storage media or input-ouput devices.
    ///2.	The multiplier must be placed in the MQ be¬fore the execution of a multiplication instruc¬tion.
    ///3.	After a division instruction is executed, the quotient appears in the MQ (the remainder appears in the AC). In fixed point division, the MQ contains the least significant half of the dividend.
    ///4.
    ///After a multiplication instruction is executed, the MQ contains the less significant half of the product. In this connection, the MQ may be regarded as the right-hand extension of the AC; see Figure 9.
    ///5.
    ///The least significant 3 5 bits of the results of FAD, UFA, FSB, and UFS instructions are in the MQ.
    /// </summary>
    public class MultiplierQuotientRegister
    {
        private string _value;

        public MultiplierQuotientRegister()
        {
            _value = Settings.ZERO_WORD_36;
        }

        /// <summary>
        /// Returns a 36 "Bit" string in a binary format ("001001") that represents the current binary value of the register 
        /// </summary>
        public string Value
        {
            get { return _value; }
            set
            {
                if( value.Length != Settings.STD_WORD_SIZE)
                    throw new Exception("Attempted to assign a string that was not the correct size to the MQ Reg");

                _value = value;
            }
        }

        /// <summary>
        /// Gets or Sets the sign bit
        /// </summary>
        public string SBit
        {
            get
            {
                return Value[0].ToString();
            }
            set
            {
                if (value.Length != 1)
                    throw new Exception("Attempted to assign a string that was not one character to the MQ SBit.");

                char[] temp = Value.ToCharArray();
                temp[0] = value[0];

                Value = new string(temp);
            }
        }
    }
}
