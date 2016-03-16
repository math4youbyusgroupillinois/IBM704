using System;
using System.Collections.Generic;
using System.Text;

namespace IBM704.Cpu
{
    public class Instruction
    {
        private InstructionExecute _executeMethod;
        private string _octal;
        private string _name;
    
        /// <summary>
        /// Will Convert an instruction in Binary format to Octal
        /// </summary>
        /// <param name="binary">The binary instruction of length 36</param>
        /// <returns>The octal representation of the instruction. 13 Digit length.</returns>
        public static string ConvertToOctal(string binary)
        {
            if(binary.Length != Settings.STD_WORD_SIZE)
                throw new Exception("Attempted to Convert an invalid length octal instruction to a Binary Format");

            string result = Converter.ConvertToOctal(binary.Substring(1), 12);

            if (binary[0] == '0')
                result = "+" + result;
            else if (binary[1] == '1')
                result = "-" + result;
            else
                throw new Exception("Invalid Sign character. Failed when trying to Convert Binary Instruction to Octal");

            return result;
        }
        
        /// <summary>
        /// Will Convert an instruction in Octal format to binary.
        /// </summary>
        /// <param name="octal">The octal instruction of length 13 (sign + twelve octal digits). </param>
        /// <returns>The binary representation of the instruction. 36 Bit length.</returns>
        public static string ConvertToBinary(string octal)
        {
            if (octal.Length != 13)
                throw new Exception("Attempted to Convert an invalid length octal instruction to a Binary Format");

            string wordNoSign = octal.Substring(1);

            string result = Converter.ConvertToBinary(wordNoSign, 35);

            if (octal[0] == '+')
                result = "0" + result;
            else if (octal[0] == '-')
                result = "1" + result;
            else
                throw new Exception("Invalid Sign character. Failed when trying to Convert Octal Instruction to Binary");

            return result;
        }

        public Instruction( string name, string octal, InstructionExecute executeMethod)
        {
            _name = name;
            _octal = octal;
            _executeMethod = executeMethod;

        }

        public string Name
        {
            get{ return _name; }
        }

        public string OctalRepresentation
        {
            get{ return _octal; }
        }

        public void Execute(CPU cpu)
        {
            _executeMethod.Invoke(cpu);
        }
    }

    public delegate void InstructionExecute( CPU cpu);
}
