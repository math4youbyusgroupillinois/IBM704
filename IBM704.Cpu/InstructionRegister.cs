using System;
using System.Collections.Generic;
using System.Text;

namespace IBM704.Cpu
{
    /// <summary>
    /// Instruction Register. When the central processing unit is ready to accept another instruction,
    ///  the word in the core storage location specified by the instruction location counter is brought
    ///  into the SR. In the SR, positions 1 and 2 are tested to determine whether the instruction is
    ///  Type A or Type B. Depending upon the outcome, the 18 bit positions of the instruction register
    ///  are filled with the required portions of the instruction word for further interpretation and
    ///  exe¬cution. The instruction register then contains the operation part of the instruction being
    ///  executed, while the rest of the instruction, i.e., address, tag, and decre¬ment parts, are
    ///  interpreted in the SR.

    ///With Type A instructions, positions S, 8, 9 of the instruction register contain the contents of
    ///  positions S, 1, 2 of the instruction. The prefix is the entire operation part of Type A
    ///  instructions. The remain¬ing positions of the instruction register contain zeros.

    ///With Type B instructions, positions S, 1-9 of the instruction register contain the contents of
    ///  positions S, 3-11 of the instruction. The remaining positions of the instruction register
    ///  contain ones with the exception of input-output, shifting and sense instruc¬tions. The contents
    ///  of positions 28-35 of these in¬structions are placed in positions 10-17 of the instruc¬tion
    ///  register where they are interpreted as part of the operation part of the instruction.

    /// </summary>
    public class InstructionRegister
    {
        private string _value;

        public InstructionRegister()
        {
            _value = Settings.ZERO_WORD_36;
        }

        /// <summary>
        /// Returns a 36 "Bit" string in a binary format ("001001") that represents the current binary value of the register 
        /// </summary>
        public string Value
        {
            get
            { return _value; }
            set
            {
                if( value.Length != Settings.STD_WORD_SIZE)
                    throw new Exception("Attempted to put an incorrect size word in the instruction register.");

                _value = value;
            }
        }

        public string AddressTypeA
        {
            get
            {
                return Value.Substring(Settings.ADDRESS_INDEX);
            }
        }

        /// <summary>
        /// The Tag of the instruction contained in bits 18-20
        /// </summary>
        public string Tag
        {
            get
            {
                return Value.Substring(18, 3);
            }
        }

        public string OperationPart_Octal
        {
            get
            {
                //Pass the operation part without the sign bit
                string sign = Value[0].ToString();
                string operation = Converter.ConvertToOctal(Value.Substring(1,11));
                
                //Check the length of the string
                if (operation.Length != 4)
                    operation = Converter.PadOctalString(operation, 4);

                //Check the sign status
                if (sign == "0")
                    sign = "+";
                else
                    sign = "-";

                //Combine the sign with the operation
                return String.Concat(sign, operation);
            }
            
        }
    }
}
