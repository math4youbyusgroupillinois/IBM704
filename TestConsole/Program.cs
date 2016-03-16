using System;
using System.Collections.Generic;
using System.Text;
using IBM704.Cpu;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //string binary = Convert.ToString(12, 2);
           // int dec = Convert.ToInt32(binary, 10);
            //string binary = Converter.ConvertToBinary(7);
            //int dec = Converter.ConvertToInt(binary);
            //string oct = Converter.ConvertToOctal("000101001010");


            //CPU testCpu = new CPU();
            //testCpu.InstructionReg.Value = "000000000000000000111000000000000000";

            //testCpu.IndexRegA.Value =  "000100001000";
            //testCpu.IndexRegB.Value = "0000011111111";

            //string test = testCpu.TagIndex;

            //testCpu.SetCoreStorageWord(Converter.ConvertToBinary(11,36),0);
            //testCpu.ExecuteSingleStep();

            //string result = Converter.ConvertToOctal("111");

            FloatingPointNumber testFloat = new FloatingPointNumber("010000000000000000000000000000000001");

            decimal testDec = testFloat.DecimalValue;

            testFloat.DecimalValue = (decimal)0.1 * -1;

        }
    }
}
