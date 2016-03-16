using System;
using System.Collections.Generic;
using System.Text;

namespace IBM704.Cpu
{
    /// <summary>
    /// Instruction Location Counter. 
    /// This register, with a capacity of 12, 13, or 15 bits (for 4,096, 8,192, or 32,768 words of core storage), 
    /// determines the loca¬tion in core storage from which the central process¬ing unit takes its next instruction. 
    /// After each in¬struction has been executed, the contents of the instruction location counter are changed. After 
    /// most instructions, the contents are increased by 1, so that the calculator will go to the next sequential 
    /// location in storage for its next instruction. However, during the execution of a skip type of instruction, 
    /// the con¬tents may be increased by 1, 2, or 3, and during the execution of a transfer type, the contents may 
    /// be changed to any number in the address range. When the instruction location counter contains the largest 
    /// possible location in storage (all Ts), then the next sequential instruction is the lowest possible  (all 0's).
    ///
    ///    When operating the 704 and a stop occurs, it is necessary to know the instruction to which the in¬struction 
    /// location counter is referring. In all cases except halt and transfer, the instruction location counter contains
    ///  an address one higher than the ad¬dress of the last instruction executed. (The last in¬struction is also the 
    /// instruction appearing in the in¬struction register.) In the case of an HTR instruction, the calculator stops
    ///  with the address of the HTR in the instruction location counter.
    /// </summary>
    public class InstructionLocationCounter
    {
        private string _value;

        public InstructionLocationCounter()
        {
            Value = Settings.ZERO_WORD_15;
        }

        /// <summary>
        /// Returns a 15 "Bit" string in a binary format ("001001") that represents the current binary value of the register 
        /// </summary>
        public string Value
        {
            get
            { return _value; }
            set
            {
                if( value.Length != Settings.INT_COUNTER_WORD_SIZE)
                    throw new Exception("Attempted to put an incorrect size word in the instruction location counter.");

                _value = value;
            }
        }

        public void Increment( int incrementValue)
        {
            Value = Converter.ConvertToBinary(Converter.ConvertToInt(_value) + incrementValue, 15);
        }
    }
}
