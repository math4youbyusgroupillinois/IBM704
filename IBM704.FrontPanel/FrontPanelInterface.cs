using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using IBM704.Cpu;

namespace IBM704.FrontPanel
{
    public class FrontPanelInterface
    {
        #region I/O Declarations

        const int PORT1_A0 = 0x000;
        const int PORT1_B0 = PORT1_A0 + 1;
        const int PORT1_C0 = PORT1_A0 + 2;
        const int PORT1_Control0 = PORT1_A0 + 3;

        const int PORT1_A1 = PORT1_A0 + 4;
        const int PORT1_B1 = PORT1_A0 + 5;
        const int PORT1_C1 = PORT1_A0 + 6;
        const int PORT1_Control1 = PORT1_A0 + 7;

        const int PORT2_A0 = 0x200;
        const int PORT2_B0 = PORT2_A0 + 1;
        const int PORT2_C0 = PORT2_A0 + 2;
        const int PORT2_Control0 = PORT2_A0 + 3;

        const int PORT2_A1 = PORT2_A0 + 4;
        const int PORT2_B1 = PORT2_A0 + 5;
        const int PORT2_C1 = PORT2_A0 + 6;
        const int PORT2_Control1 = PORT2_A0 + 7;

        const int PORT3_A0 = 0x300;
        const int PORT3_B0 = PORT3_A0 + 1;
        const int PORT3_C0 = PORT3_A0 + 2;
        const int PORT3_Control0 = PORT3_A0 + 3;

        const int PORT3_A1 = PORT3_A0 + 4;
        const int PORT3_B1 = PORT3_A0 + 5;
        const int PORT3_C1 = PORT3_A0 + 6;
        const int PORT3_Control1 = PORT3_A0 + 7;

        #endregion

        #region External Methods

        [DllImport("inpout32.dll", EntryPoint = "Out32")]
        public static extern void outp(int adress, int value);

        [DllImport("inpout32.dll", EntryPoint = "Inp32")]
        public static extern int inp(int adress);

        #endregion

        #region Initialization & Setup

        public FrontPanelInterface()
        {
            outp(PORT1_Control0, 0x80);
            outp(PORT1_Control1, 0x80);
            outp(PORT2_Control0, 0x80);
            outp(PORT2_Control1, 0x80);
            outp(PORT3_Control0, 0x9B);
            outp(PORT3_Control1, 0x9B);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This will Set the lights on the Physical Front Panel based on the passed in instance of the Front Panel
        /// </summary>
        /// <param name="frontPanel"></param>
        public void SetLights(FrontPanel frontPanel)
        {
            int adder = 0;

            //Set Indicator Lights
            if (frontPanel.ProgramStopLight)
                adder = adder + 1;
            if (frontPanel.AccumulatorOverflowLight)
                adder = adder + 2;
            if (frontPanel.DivideCheckLight)
                adder = adder + 4;
            //if (ReadWriteSelectLight)
            //adder = adder + 8;
            if (frontPanel.MQOverflowLight)
                adder = adder + 16;
            if (frontPanel.ReadWriteCheckLight)
                adder = adder + 32;

            //Begin to set Index Register Lights
            if (frontPanel.IndexRegisterDisplay[0] == '1')
                adder = adder + 64;
            if (frontPanel.IndexRegisterDisplay[1] == '1')
                adder = adder + 128;

            outp(PORT1_A0, adder);
            adder = 0;

            //Continue to set Index Register Lights
            if (frontPanel.IndexRegisterDisplay[2] == '1')
                adder = adder + 1;
            if (frontPanel.IndexRegisterDisplay[3] == '1')
                adder = adder + 2;
            if (frontPanel.IndexRegisterDisplay[4] == '1')
                adder = adder + 4;
            if (frontPanel.IndexRegisterDisplay[5] == '1')
                adder = adder + 8;
            if (frontPanel.IndexRegisterDisplay[6] == '1')
                adder = adder + 16;
            if (frontPanel.IndexRegisterDisplay[7] == '1')
                adder = adder + 32;
            if (frontPanel.IndexRegisterDisplay[8] == '1')
                adder = adder + 64;
            if (frontPanel.IndexRegisterDisplay[9] == '1')
                adder = adder + 128;

            outp(PORT1_B0, adder);
            adder = 0;

            //Continue to set Index Register Lights
            if (frontPanel.IndexRegisterDisplay[10] == '1')
                adder = adder + 1;
            if (frontPanel.IndexRegisterDisplay[11] == '1')
                adder = adder + 2;
            if (frontPanel.IndexRegisterDisplay[12] == '1')
                adder = adder + 4;
            if (frontPanel.IndexRegisterDisplay[13] == '1')
                adder = adder + 8;
            if (frontPanel.IndexRegisterDisplay[14] == '1')
                adder = adder + 16;

            // Begin to set State Lights
            if (frontPanel.PowerLight)
                adder = adder + 32;
            //if (ReadyLight)
            //adder = adder + 64;
            if (frontPanel.AutomaticLight)
                adder = adder + 128;

            outp(PORT1_C0, adder);
            adder = 0;

            // Begin to set Accumulator Lights
            if (frontPanel.AccumDisplay[0] == '1')//////////////////ACCUMULATOR
                adder = adder + 1;
            if (frontPanel.AccumDisplay[1] == '1')
                adder = adder + 2;
            if (frontPanel.AccumDisplay[2] == '1')
                adder = adder + 4;
            if (frontPanel.AccumDisplay[3] == '1')
                adder = adder + 8;
            if (frontPanel.AccumDisplay[4] == '1')
                adder = adder + 16;
            if (frontPanel.AccumDisplay[5] == '1')
                adder = adder + 32;
            if (frontPanel.AccumDisplay[6] == '1')
                adder = adder + 64;
            if (frontPanel.AccumDisplay[7] == '1')
                adder = adder + 128;

            outp(PORT1_A1, adder);
            adder = 0;

            // Continue to set Accumulator Lights
            if (frontPanel.AccumDisplay[8] == '1')
                adder = adder + 1;
            if (frontPanel.AccumDisplay[9] == '1')
                adder = adder + 2;
            if (frontPanel.AccumDisplay[10] == '1')
                adder = adder + 4;
            if (frontPanel.AccumDisplay[11] == '1')
                adder = adder + 8;
            if (frontPanel.AccumDisplay[12] == '1')
                adder = adder + 16;
            if (frontPanel.AccumDisplay[13] == '1')
                adder = adder + 32;
            if (frontPanel.AccumDisplay[14] == '1')
                adder = adder + 64;
            if (frontPanel.AccumDisplay[15] == '1')
                adder = adder + 128;

            outp(PORT1_B1, adder);
            adder = 0;

            // Continue to set Accumulator Lights
            if (frontPanel.AccumDisplay[16] == '1')
                adder = adder + 1;
            if (frontPanel.AccumDisplay[17] == '1')
                adder = adder + 2;
            if (frontPanel.AccumDisplay[18] == '1')
                adder = adder + 4;
            if (frontPanel.AccumDisplay[19] == '1')
                adder = adder + 8;
            if (frontPanel.AccumDisplay[20] == '1')
                adder = adder + 16;
            if (frontPanel.AccumDisplay[21] == '1')
                adder = adder + 32;
            if (frontPanel.AccumDisplay[22] == '1')
                adder = adder + 64;
            if (frontPanel.AccumDisplay[23] == '1')
                adder = adder + 128;

            outp(PORT1_C1, adder);
            adder = 0;

            // Continue to set Accumulator Lights
            if (frontPanel.AccumDisplay[24] == '1')
                adder = adder + 1;
            if (frontPanel.AccumDisplay[25] == '1')
                adder = adder + 2;
            if (frontPanel.AccumDisplay[26] == '1')
                adder = adder + 4;
            if (frontPanel.AccumDisplay[27] == '1')
                adder = adder + 8;
            if (frontPanel.AccumDisplay[28] == '1')
                adder = adder + 16;
            if (frontPanel.AccumDisplay[29] == '1')
                adder = adder + 32;
            if (frontPanel.AccumDisplay[30] == '1')
                adder = adder + 64;
            if (frontPanel.AccumDisplay[31] == '1')
                adder = adder + 128;

            outp(PORT2_A0, adder);
            adder = 0;

            // Continue to set Accumulator Lights
            if (frontPanel.AccumDisplay[32] == '1')
                adder = adder + 1;
            if (frontPanel.AccumDisplay[33] == '1')
                adder = adder + 2;
            if (frontPanel.AccumDisplay[34] == '1')
                adder = adder + 4;
            if (frontPanel.AccumDisplay[35] == '1')
                adder = adder + 8;
            if (frontPanel.AccumDisplay[36] == '1')
                adder = adder + 16;
            if (frontPanel.AccumDisplay[37] == '1')
                adder = adder + 32;

            // Begin to set Instruction Counter Lights
            if (frontPanel.InstructionCounterDisplay[0] == '1')
                adder = adder + 64;
            if (frontPanel.InstructionCounterDisplay[1] == '1')
                adder = adder + 128;

            outp(PORT2_B0, adder);
            adder = 0;

            // Continue to set Instruction Counter Lights
            if (frontPanel.InstructionCounterDisplay[2] == '1')
                adder = adder + 1;
            if (frontPanel.InstructionCounterDisplay[3] == '1')
                adder = adder + 2;
            if (frontPanel.InstructionCounterDisplay[4] == '1')
                adder = adder + 4;
            if (frontPanel.InstructionCounterDisplay[5] == '1')
                adder = adder + 8;
            if (frontPanel.InstructionCounterDisplay[6] == '1')
                adder = adder + 16;
            if (frontPanel.InstructionCounterDisplay[7] == '1')
                adder = adder + 32;
            if (frontPanel.InstructionCounterDisplay[8] == '1')
                adder = adder + 64;
            if (frontPanel.InstructionCounterDisplay[9] == '1')
                adder = adder + 128;

            outp(PORT2_C0, adder);
            adder = 0;

            // Begin to set Instruction Counter Lights
            if (frontPanel.InstructionCounterDisplay[10] == '1')
                adder = adder + 1;
            if (frontPanel.InstructionCounterDisplay[11] == '1')
                adder = adder + 2;
            if (frontPanel.InstructionCounterDisplay[12] == '1')
                adder = adder + 4;
            if (frontPanel.InstructionCounterDisplay[13] == '1')
                adder = adder + 8;
            if (frontPanel.InstructionCounterDisplay[14] == '1')
                adder = adder + 16;

            // Set the Trapping Indicator Light
            if (frontPanel.TrapIndicator)
                adder = adder + 32;

            // Begin to set Instruction Register Lights
            if (frontPanel.InstructionRegisterDisplay[0] == '1')
                adder = adder + 64;
            if (frontPanel.InstructionRegisterDisplay[1] == '1')
                adder = adder + 128;

            outp(PORT2_A1, adder);
            adder = 0;

            // Continue to set Instruction Register Lights
            if (frontPanel.InstructionRegisterDisplay[2] == '1')
                adder = adder + 1;
            if (frontPanel.InstructionRegisterDisplay[3] == '1')
                adder = adder + 2;
            if (frontPanel.InstructionRegisterDisplay[4] == '1')
                adder = adder + 4;
            if (frontPanel.InstructionRegisterDisplay[5] == '1')
                adder = adder + 8;
            if (frontPanel.InstructionRegisterDisplay[6] == '1')
                adder = adder + 16;
            if (frontPanel.InstructionRegisterDisplay[7] == '1')
                adder = adder + 32;
            if (frontPanel.InstructionRegisterDisplay[8] == '1')
                adder = adder + 64;
            if (frontPanel.InstructionRegisterDisplay[9] == '1')
                adder = adder + 128;

            outp(PORT2_B1, adder);
            adder = 0;

            // Begin to set Instruction Register Lights
            if (frontPanel.InstructionRegisterDisplay[10] == '1')
                adder = adder + 1;
            if (frontPanel.InstructionRegisterDisplay[11] == '1')
                adder = adder + 2;
            if (frontPanel.InstructionRegisterDisplay[12] == '1')
                adder = adder + 4;
            if (frontPanel.InstructionRegisterDisplay[13] == '1')
                adder = adder + 8;
            if (frontPanel.InstructionRegisterDisplay[14] == '1')
                adder = adder + 16;
            if (frontPanel.InstructionRegisterDisplay[15] == '1')
                adder = adder + 32;
            if (frontPanel.InstructionRegisterDisplay[16] == '1')
                adder = adder + 64;
            if (frontPanel.InstructionRegisterDisplay[17] == '1')
                adder = adder + 128;

            outp(PORT2_C1, adder);
        }

        /// <summary>
        /// This will check the status of the switches and buttons on the physical front panel
        /// and will update the passed in logical front panel accordingly
        /// </summary>
        /// <param name="frontPanel"></param>
        public void ReadKeys(ref FrontPanel frontPanel)
        {
            string incoming;
            char[] keyedBuffer = new char[36];

            //Read Input Keys. These keys must be read in BEFORE the 'Enter MQ' or 'Enter Instruction' buttons are read. 
            incoming = Converter.ConvertToBinary(inp(PORT3_B0), 8);
            keyedBuffer[0] = incoming[3];
            keyedBuffer[1] = incoming[4];
            keyedBuffer[2] = incoming[5];
            keyedBuffer[3] = incoming[6];
            keyedBuffer[4] = incoming[7];

            incoming = Converter.ConvertToBinary(inp(PORT3_C0), 8);
            keyedBuffer[5] = incoming[0];
            keyedBuffer[6] = incoming[1];
            keyedBuffer[7] = incoming[2];
            keyedBuffer[8] = incoming[3];
            keyedBuffer[9] = incoming[4];
            keyedBuffer[10] = incoming[5];
            keyedBuffer[11] = incoming[6];
            keyedBuffer[12] = incoming[7];

            incoming = Converter.ConvertToBinary(inp(PORT3_A1), 8);
            keyedBuffer[13] = incoming[0];
            keyedBuffer[14] = incoming[1];
            keyedBuffer[15] = incoming[2];
            keyedBuffer[16] = incoming[3];
            keyedBuffer[17] = incoming[4];
            keyedBuffer[18] = incoming[5];
            keyedBuffer[19] = incoming[6];
            keyedBuffer[20] = incoming[7];

            incoming = Converter.ConvertToBinary(inp(PORT3_B1), 8);
            keyedBuffer[21] = incoming[0];
            keyedBuffer[22] = incoming[1];
            keyedBuffer[23] = incoming[2];
            keyedBuffer[24] = incoming[3];
            keyedBuffer[25] = incoming[4];
            keyedBuffer[26] = incoming[5];
            keyedBuffer[27] = incoming[6];
            keyedBuffer[28] = incoming[7];

            incoming = Converter.ConvertToBinary(inp(PORT3_B1), 8);
            keyedBuffer[29] = incoming[0];
            keyedBuffer[30] = incoming[1];
            keyedBuffer[31] = incoming[2];
            keyedBuffer[32] = incoming[3];
            keyedBuffer[33] = incoming[4];
            keyedBuffer[34] = incoming[5];
            keyedBuffer[35] = incoming[6];

            frontPanel.KeyedInWord = new string(keyedBuffer);

            // Read Automatic/Manual Switch. This must be read be any buttons, otherwise users will be able to switch the machine to automatic
            //        and still use buttons that should only be used in manual mode.
            if (incoming[7] == '1')
            {
                if (frontPanel.Mode == RunMode.Manual)
                    frontPanel.ChangeToAutomatic();
            }
            else
            {
                if (frontPanel.Mode == RunMode.Automatic)
                    frontPanel.ChangeToManual();
            }

            //Read From Buttons
            incoming = Converter.ConvertToBinary(inp(PORT3_A0), 8);
            if (incoming[0] == '1')
                frontPanel.SetDisplayIndex(IndexDisplay.IndexA);
            if (incoming[1] == '1')
                frontPanel.SetDisplayIndex(IndexDisplay.IndexB);
            if (incoming[2] == '1')
                frontPanel.SetDisplayIndex(IndexDisplay.IndexC);
            if (incoming[3] == '1')
                frontPanel.MultiStep();
            if (incoming[4] == '1')
                frontPanel.LoadDrum();
            if (incoming[5] == '1')
                frontPanel.SingleStep();
            if (incoming[6] == '1')
                frontPanel.Reset();
            if (incoming[7] == '1')
                frontPanel.Start();

            incoming = Converter.ConvertToBinary(inp(PORT3_B0), 8);
            if (incoming[0] == '1')
                frontPanel.Clear();
            if (incoming[1] == '1')
                frontPanel.EnterMQ();
            if (incoming[2] == '1')
                frontPanel.EnterInstruction();


        }

        #endregion

    }
}
