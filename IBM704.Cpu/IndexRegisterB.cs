using System;
using System.Collections.Generic;
using System.Text;

namespace IBM704.Cpu
{
    public class IndexRegisterB
    {
        private string _value;

        public IndexRegisterB()
        {
            _value = Settings.ZERO_WORD_13;
        }

        /// <summary>
        /// Returns a 13 "Bit" string in a binary format ("001001") that represents the current binary value of the register 
        /// </summary>
        public string Value
        {
            get
            { return _value; }
            set
            {
                if( value.Length != Settings.INDEX_B_SIZE)
                    throw new Exception("Attempted to put an incorrect size word in Index Register B.");

                _value = value;
            }
        }
    }
}
