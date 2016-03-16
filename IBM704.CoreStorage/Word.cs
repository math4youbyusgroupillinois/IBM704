using System;
using System.Collections.Generic;
using System.Text;

namespace IBM704.CoreStorage
{
    public class Word
    {
        private string _value;

        public Word()
        {
            _value = Settings.ZERO_WORD_36;
        }

        /// <summary>
        /// Gets or Sets a 36 "Bit" string that represents the value of the Core Storage word
        /// </summary>
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value.Length != 36)
                    throw new Exception("Attepted to store a word without 36 bits.");

                _value = value;
            }
        }

        /// <summary>
        /// Gets or Sets a  15 bit string representing the address section of the core storage word
        /// </summary>
        public string Address
        {
            get { return Value.Substring(Settings.ADDRESS_INDEX); }
            set
            {
                if (value.Length != Settings.STD_WORD_SIZE - Settings.ADDRESS_INDEX)
                    throw new Exception(
                        "Attempted to modify the Address of a core storage word with an incorretly sized string");

                _value = _value.Substring(0, Settings.ADDRESS_INDEX) + value;
            }
        }
    }
}
