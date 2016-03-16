using System;
using System.Collections.Generic;
using System.Text;

namespace IBM704.Cpu
{
    public class FloatingPointNumber
    {
        #region Declarations

        private string _value;
        private int _decimalValue;

        #endregion

        #region Setup
        public FloatingPointNumber(string binary)
        {
            Value = binary;
        }

        public FloatingPointNumber()
        {
            Value = Settings.ZERO_WORD_36;
        }
        #endregion

        #region Property Accessors

        public string Characteristic
        {
            get { return Value.Substring(1, 8); }
        }

        public string Fraction
        {
            get { return Value.Substring(9); }
        }

        public string Value
        {
            get { return _value; }
            set
            {
                if (value.Length != Settings.STD_WORD_SIZE)
                    throw new Exception(
                        "Attempted to input an incorretly sized word into a floating point representation.");

                _value = value;
            }
        }

        public decimal DecimalValue
        {
            get
            {
                long charactAdj = Converter.ConvertToInt(Characteristic) - 128;
                long fractionAdj = Converter.ConvertToInt(Fraction);
                string fractionTemp = "." + Convert.ToString(fractionAdj);
                decimal fraction = Convert.ToDecimal(fractionTemp);
                decimal result = fraction * Convert.ToDecimal(Math.Pow(10, charactAdj));

                if (Value[0] == '1')
                    result *= -1;

                return result;
            }
            set
            {
                int workingCharact=0;
                decimal  workingFraction = Math.Abs(value);

                if( value >= 1)
                {
                    for( workingCharact = 0; workingFraction >= 1; workingCharact++)
                    {
                        workingFraction = workingFraction/10;
                    }
                }
                else if( value < 1)
                {
                    for (workingCharact = 0; workingFraction < 1; workingCharact--)
                    {
                        workingFraction = workingFraction * 10;
                    }

                    workingCharact++;
                    workingFraction = workingFraction/10;
                }

                string characteristic = Converter.ConvertToBinary(workingCharact + 128, 8);

                long fractionDec = Convert.ToInt64( Convert.ToString(workingFraction).Substring(2));
                string fraction = Converter.ConvertToBinary(fractionDec, 27);

                string sign = (value >= 0 ? "0" : "1");

                Value = sign + characteristic + fraction;
            }
        }

        #endregion
        }
    }
