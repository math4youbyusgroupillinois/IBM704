using System;
using System.Collections.Generic;
using System.Text;

namespace IBM704.SecondaryStorage
{
    public static class SecondaryStorage
    {
        /// <summary>
        /// Returns a List of instructions in octal
        /// </summary>
        /// <returns></returns>
        public static List<string> TestProgram()
        {
            List<string> program = new List<string>();

            for (int i = 0; i < 4096; i++)
            {
                program.Add("+000000000000");
            }


            //Instruction Area: Words 0 - 511
            program[0] = "+056000001000";  //LDQ: Load MQ with word 512
            program[1] = "+056000001001";  //LDQ: Load MQ with word 512
            program[2] = "+056000001002";  //LDQ: Load MQ with word 512
            program[3] = "+056000001003";  //LDQ: Load MQ with word 512
            program[4] = "+056000001004";  //LDQ: Load MQ with word 512
            program[5] = "+056000001005";  //LDQ: Load MQ with word 512
            program[6] = "+056000001006";  //LDQ: Load MQ with word 512
            program[7] = "+056000001007";  //LDQ: Load MQ with word 512
            program[8] = "+056000001008";  //LDQ: Load MQ with word 512
            program[9] = "+056000001009";  //LDQ: Load MQ with word 512
            program[10] = "+056000001010";  //LDQ: Load MQ with word 512



            //Data Area: Words 512 - 4095
            program[512] = "+000000000001"; // 5
            program[513] = "+000000000003"; // 10
            program[514] = "+000000000005"; // 14
            program[515] = "+000000000007"; // 14
            program[516] = "+000000000011"; // 14
            program[517] = "+000000000013"; // 14
            program[518] = "+000000000015"; // 14
            program[519] = "+000000000017"; // 14
            program[520] = "+000000000021"; // 14
            program[521] = "+000000000023"; // 14
            program[522] = "+000000000025"; // 14
            program[523] = "+000000000027"; // 14

            program[777] = "-377777777777";

            return program;
        }
    }
}
