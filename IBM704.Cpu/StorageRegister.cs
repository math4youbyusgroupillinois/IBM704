using System;
using System.Collections.Generic;
using System.Text;

namespace IBM704.Cpu
{
    /// <summary>
    /// From the 704 Manual:
    /// 
    /// One special register, which will be referred to as the SR, is used for both arithmetic and control func¬tions. 
    /// Its operation is entirely automatic and will rarely concern the programmer. The SR has a capacity of 36 bits (one word) 
    /// and serves as a buffer between core storage and the central processing unit. Some of the interpretation of an 
    /// instruction is performed in the SR. It is also used in the execution of floating¬point instructions.
    /// </summary>
    public sealed class StorageRegister
    {
        private string _value;

        public StorageRegister()
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
                if (value.Length != Settings.STD_WORD_SIZE)
                    throw new Exception("Attempted to assign an incorrect size word to the StorageRegister");

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
    }
}
