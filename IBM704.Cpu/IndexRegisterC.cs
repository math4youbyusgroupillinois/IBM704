using System;
using System.Collections.Generic;
using System.Text;

namespace IBM704.Cpu
{
    public class IndexRegisterC
    {
        private string _value;

        public IndexRegisterC()
        {
            _value = Settings.ZERO_WORD_15;
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
                if( value.Length != Settings.INDEX_C_SIZE)
                    throw new Exception("Attempted to put an incorrect size word in Index Register C.");

                _value = value;
            }
        }
    }
}
