using System;
using System.Collections.Generic;

namespace IBM704.Cpu
{
    public class InstructionSet
    {
        #region Declarations

        private static Dictionary<string, Instruction> _instructions;

        #endregion

        #region Instruction Set Methods

        #region Fixed-Point Arithmetic Operations

        /// <summary>
        /// Clear and Add 2    CLA Y    +0500
        ///The C(Y) replace the C(AC)S1.35. Positions Q and P of the AC are cleared. The C(Y)  are unchanged.
        /// </summary>
        /// <param name="cpu"></param>
        private static void CLA(CPU cpu)
        {
            cpu.AccumulatorReg.Bits_SignTo35 = cpu.GetCoreStorageWord(cpu.IndexedAddress);
            cpu.AccumulatorReg.PBit = "0";
            cpu.AccumulatorReg.QBit = "0";

            //Increment IR
            cpu.InstructionCounter.Increment(1);
        }

        /// <summary>
        /// This instruction algebraically adds the C(Y) to the C(AC) and 
        /// replaces the C(AC) with this sum. The C(Y) are unchanged.  Ac overflow is possible.
        /// </summary>
        /// <param name="cpu"></param>
        private static void ADD(CPU cpu)
        {
            long fromCore = Converter.ConvertToInt(cpu.GetCoreStorageWord(cpu.IndexedAddress));
            long fromAccum = Converter.ConvertToInt(cpu.AccumulatorReg.Bits_SignTo35);
            long result = fromCore + fromAccum;

            string newWord = Converter.ConvertToBinary(result, 36);

            if (newWord.Length > 36)
            {
                cpu.AccumOverflowIndicator = true;
                cpu.AccumulatorReg.Bits_SignTo35 = newWord.Substring(0, 36);
            }
            else
                cpu.AccumulatorReg.Bits_SignTo35 = newWord;

            cpu.InstructionCounter.Increment(1);
        }

        /// <summary>
        /// This instruction algebraically subtracts the C(Y) from the C(AC) and replaces the C(AC) with 
        /// this difference. The C(Y) are unchanged. Ac overflow is possible
        /// </summary>
        /// <param name="cpu"></param>
        private static void SUB(CPU cpu)
        {
            long fromCore = Converter.ConvertToInt(cpu.GetCoreStorageWord(cpu.IndexedAddress));
            long fromAccum = Converter.ConvertToInt(cpu.AccumulatorReg.Bits_SignTo35);
            long result = fromAccum - fromCore;

            string newWord = Converter.ConvertToBinary(result, 36);

            if (newWord.Length > 36)
            {
                cpu.AccumOverflowIndicator = true;
                cpu.AccumulatorReg.Bits_SignTo35 = newWord.Substring(0, 36);
            }
            else
                cpu.AccumulatorReg.Bits_SignTo35 = newWord;

            cpu.InstructionCounter.Increment(1);
        }

        /// <summary>
        /// Clear and Subtract 2    CLS Y    +0502
        ///The negative of the C(Y) replaces the C(AC)S>1_35. Positions Q and P of the 
        /// AC are cleared. The C(Y) are unchanged.
        /// </summary>
        /// <param name="cpu"></param>
        private static void CLS(CPU cpu)
        {
            long fromCore = Converter.ConvertToInt(cpu.GetCoreStorageWord(cpu.IndexedAddress));
            cpu.AccumulatorReg.Bits_SignTo35 = Converter.ConvertToBinary(fromCore * -1, 36);
            cpu.AccumulatorReg.PBit = "0";
            cpu.AccumulatorReg.QBit = "0";

            //Increment IR
            cpu.InstructionCounter.Increment(1); 
        }

        /// <summary>
        /// ADM Y    +0401
        /// This instruction algebraically adds the magnitude (absolute value) of the C(Y) to the C(AC) and replaces the 
        /// C(AC) with this sum. The C(Y) are unchanged. Ac overflow is possible
        /// </summary>
        /// <param name="cpu"></param>
        private static void ADM(CPU cpu)
        {
            long fromCore = Converter.ConvertToInt(cpu.IndexedAddress);
            fromCore = Math.Abs(fromCore);
            long fromAccum = Converter.ConvertToInt(cpu.AccumulatorReg.Bits_SignTo35);

            long result = fromAccum + fromCore;

            string newWord = Converter.ConvertToBinary(result, 36);

            if (newWord.Length > 36)
            {
                cpu.AccumOverflowIndicator = true;
                cpu.AccumulatorReg.Bits_SignTo35 = newWord.Substring(0, 36);
            }
            else
                cpu.AccumulatorReg.Bits_SignTo35 = newWord;

            cpu.InstructionCounter.Increment(1);
        }

        /// <summary>
        /// Subtract Magnitude 2    SBM Y    —0400
        /// This instruction algebraically subtracts the mag¬nitude of the C(Y) from the C(AC) 
        /// and replaces the C(AC) with this difference. The C(Y) are unchanged. Ac overflow is possible.
        /// </summary>
        /// <param name="cpu"></param>
        private static void SBM(CPU cpu)
        {
            long fromCore = Converter.ConvertToInt(cpu.IndexedAddress);
            fromCore = Math.Abs(fromCore);
            long fromAccum = Converter.ConvertToInt(cpu.AccumulatorReg.Bits_SignTo35);

            long result = fromAccum - fromCore;

            string newWord = Converter.ConvertToBinary(result, 36);

            if (newWord.Length > 36)
            {
                cpu.AccumOverflowIndicator = true;
                cpu.AccumulatorReg.Bits_SignTo35 = newWord.Substring(0, 36);
            }
            else
                cpu.AccumulatorReg.Bits_SignTo35 = newWord;

            cpu.InstructionCounter.Increment(1);
        }

        /// <summary>
        /// Multiply
        ///20    MPY Y    +0200
        ///This instruction multiplies the c (Y) by the c(MQ) . The 3 5 most significant bits of the 70-bit product 
        /// replace the C(AC)1.35 and the 3 5 least significant bits replace the c (MQ) !_35. The Q and P bits are 
        /// cleared. The sign of the AC is the algebraic sign of the product. The sign of the MQ agrees with the sign 
        /// of the AC.
        /// Placing of the binary point in the factors is com¬pletely arbitrary. A simple familiar rule to remember 
        /// with regard to placing the binary point in the result¬ing product follows.
        /// RULE: Add the number of binary bits to the right of the binary point in the first factor to the number
        /// of binary bits to the right of the binary point in the second factor. This sum is the number of bits 
        /// appear¬ing to the right of the binary point in the product.
        /// </summary>
        /// <param name="cpu"></param>
        private static void MPY(CPU cpu)
        {
            long fromCore = Converter.ConvertToInt(cpu.GetCoreStorageWord(cpu.IndexedAddress));
            long fromAccum = Converter.ConvertToInt(cpu.AccumulatorReg.Bits_SignTo35);

            long result = fromCore*fromAccum;
            //Include one extra bit for the sign
            string newWord = Converter.ConvertToBinary(result, 71);

            cpu.AccumulatorReg.PBit = "0";
            cpu.AccumulatorReg.QBit = "0";

            if (newWord.Length > 71)
            {
                cpu.AccumOverflowIndicator = true;
            }

            
            cpu.AccumulatorReg.Bits_SignTo35 = newWord.Substring(0, 36);
            cpu.MQReg.Value = "0" + newWord.Substring(36, 35);

            cpu.InstructionCounter.Increment(1);
        }

        private static void STO(CPU cpu) //Store
        {
            cpu.SetCoreStorageWord(cpu.AccumulatorReg.Bits_SignTo35, cpu.IndexedAddress);

            //Increment IR
            cpu.InstructionCounter.Increment(1);
        }

        private static void STA(CPU cpu) //Store Address
        {
            //Result stores orignal core storage word with bits 21-35 replaced by the
            //accumulator 21-35 bits
            string fromCore = cpu.GetCoreStorageWord(cpu.IndexedAddress).Substring(0, 21);
            string fromAccum = cpu.AccumulatorReg.Bits_SignTo35.Substring(21, 15);
            string result = fromCore + fromAccum;

            cpu.SetCoreStorageWord(result, cpu.IndexedAddress);

            //Increment IRC
            cpu.InstructionCounter.Increment(1);
        }

        /// <summary>
        /// Load MQ
        /// 2    LDQ Y    +0560
        /// The C(Y)  replace the C(MQ).   The C(Y)   are unchanged.
        /// </summary>
        /// <param name="cpu"></param>
        private static void LDQ(CPU cpu)
        {
            cpu.AccumulatorReg.Bits_SignTo35 = cpu.GetCoreStorageWord(cpu.IndexedAddress);
            cpu.AccumulatorReg.PBit = "0";
            cpu.AccumulatorReg.QBit = "0";

            //Increment IC
            cpu.InstructionCounter.Increment(1);
        }

        /// <summary>
        /// Store MQ 2    STQ Y -0600
        /// The C(MQ) replace the C(Y). The C(MQ) are unchanged.
        /// </summary>
        /// <param name="cpu"></param>
        private static void STQ(CPU cpu)
        {
            cpu.SetCoreStorageWord(cpu.AccumulatorReg.Bits_SignTo35, cpu.IndexedAddress);

            //Increment IC
            cpu.InstructionCounter.Increment(1);
        }

        /// <summary>
        /// Clear Magnitude
        /// 2    CLM    +0768... 000
        /// The C(AC)QP>1.35 are cleared.   The AC sign is un¬changed.
        /// </summary>
        /// <param name="cpu"></param>
        private static void CLM(CPU cpu)
        {
            string sign = cpu.AccumulatorReg.SBit;

            cpu.AccumulatorReg.Value = Settings.ZERO_WORD_38;

            cpu.AccumulatorReg.SBit = sign;

            //Increment IC
            cpu.InstructionCounter.Increment(1);
        }

        /// <summary>
        /// Change Sign
        /// 2    CHS    +0767... 002
        /// If the AC sign bit is negative, it is made positive, and vice versa.
        /// </summary>
        /// <param name="cpu"></param>
        private static void CHS(CPU cpu)
        {
            if (cpu.AccumulatorReg.SBit == "1")
                cpu.AccumulatorReg.SBit = "0";
            else
                cpu.AccumulatorReg.SBit = "1";


            //Increment IC
            cpu.InstructionCounter.Increment(1);
        }

        /// <summary>
        /// Set Sign Plus 2    
        /// SSP    +0763.
        /// A positive sign replaces the C(AC)S.
        /// </summary>
        /// <param name="cpu"></param>
        private static void SSP(CPU cpu)
        {
            cpu.AccumulatorReg.SBit = "1";

            //Increment IC
            cpu.InstructionCounter.Increment(1);
        }

        /// <summary>
        /// Set Sign Minus
        /// 2    SSM    —0760... 003
        /// A negative sign replaces the C(AC)S.
        /// </summary>
        /// <param name="cpu"></param>
        private static void SSM(CPU cpu)
        {
            cpu.AccumulatorReg.SBit = "0";

            //Increment IC
            cpu.InstructionCounter.Increment(1);
        }

        #endregion

        #region Control Operations

        /// <summary>
        /// Enter Trapping Mode 2    ETM    +0760... 007
        /// This instruction turns on the trapping indicator and also the trap light on the operator's console.  The
        /// calculator operates in the trapping mode until a leave trapping mode operation is executed or until 
        /// either the clear or reset key is pressed on the console.
        /// </summary>
        /// <param name="cpu"></param>
        private static void ETM(CPU cpu)
        {
            cpu.TrappingModeIndicator = true;

            cpu.InstructionCounter.Increment(1);
        }

        /// <summary>
        /// Leave Trapping Mode 2    LTM    —0760... 007
        /// 
        /// This instruction turns off the trapping indicator and the trap light on the operator's console. 
        /// The calculator will not operate in the trapping mode until another enter trapping mode operation is executed.
        /// NOTE: When the calculator is operating in the trapping mode, the location of every transfer 
        /// instruc¬tion (except trap transfer instructions) replaces the address part of location 0000, 
        /// whether or not the conditions for transfer of control are met. If the condition is met, the
        /// calculator takes the next in¬struction from location 0001 and proceeds from that point. The 
        /// location of each transfer instruction re¬places the address part of location 0000.

        /// </summary>
        /// <param name="cpu"></param>
        private static void LTM(CPU cpu)
        {
            cpu.TrappingModeIndicator = false;

            cpu.InstructionCounter.Increment(1);
        }

        /// <summary>
        /// Halt and Transfer 2    HTR Y    +0000
        /// This instruction stops the calculator. When the start key on the operator's console is depressed, 
        /// the calculator starts again, taking the next instruction from location Y and proceeding from there.
        /// When the calculator stops, the effective address of the HTR instruction is placed in the instruction 
        /// loca¬tion counter before executing any instruction. If TSX is manually executed, the 2's complement 
        /// of this effective address is placed in the specified index reg¬ister, and the transfer is executed.
        /// </summary>
        /// <param name="cpu"></param>
        private static void HTR(CPU cpu)
        {
            cpu.Running = false;
            cpu.InstructionCounter.Value = cpu.IndexedAddress;
        }

        private static void NOP(CPU cpu) //No Operation
        {
            //Increment IR
            cpu.InstructionCounter.Increment(1);
        }

        private static void HPR(CPU cpu) //Halt and Proceed
        {
            cpu.Running = false;

            //The start button must be pressed to proceed,
            //it can execute the proceed behavior by itself.

            //Increment IR
            cpu.InstructionCounter.Increment(1);
        }

        /// <summary>
        /// Transfer
        /// 2    TRA Y    +0020
        /// This instruction causes the calculator to take its next instruction 
        /// from location Y, and to proceed from there.
        /// 
        /// When the machine is in the trapping mode, the location of each 
        /// transfer instruction met replaces the  address  part  of location   0000.    
        /// transfers, and conditional transfers for which the condition is met, 
        /// are not executed; instead, control is transferred to location 0001
        /// </summary>
        /// <param name="cpu"></param>
        private static void TRA(CPU cpu)
        {
            //Check for Trapping
            if(cpu.TrappingModeIndicator)
            {
                cpu.Running = false;
                cpu.SetCoreStorageWordAddress(cpu.InstructionCounter.Value, "0000");
                cpu.InstructionCounter.Value = Converter.ConvertToBinary("0001", 15);
            }
            else
            {
                cpu.InstructionCounter.Value = cpu.IndexedAddress;
            }
        }

        /// <summary>
        /// Trap Transfer
        /// 2    TTR Y    +0021
        /// This instruction causes the calculator to take its next instruction from location Y and to 
        /// proceed from there whether in the trapping mode or not. This makes it possible to have an 
        /// ordinary transfer even when in the trapping mode.
        /// </summary>
        /// <param name="cpu"></param>
        private static void TTR(CPU cpu)
        {
            cpu.InstructionCounter.Value = cpu.IndexedAddress;
        }

        /// <summary>
        /// Transfer on Zero 2    TZE Y    +0100
        /// If the C(AC)QPI_35 are zero, the calculator takes its next instruction from location Y and proceeds 
        /// from there. If they are not zero, the calculator pro¬ceeds to the next instruction in sequence.
        /// </summary>
        /// <param name="cpu"></param>
        private static void TZE(CPU cpu)
        {
            if (cpu.AccumulatorReg.Value == Settings.ZERO_WORD_38)
                cpu.InstructionCounter.Value = cpu.IndexedAddress;
            else 
                cpu.InstructionCounter.Increment(1);
        }

        /// <summary>
        /// Transfer on No Zero 2    TNZ Y    —0100
        /// If the C(AC)QP1_35  are not zero,  the  calculator
        /// takes its next instruction from location Y and pro¬ceeds from there. 
        /// If they are zero, the calculator pro¬ceeds to the next instruction in sequence.
        /// </summary>
        /// <param name="cpu"></param>
        private static void TNZ(CPU cpu)
        {
            if (cpu.AccumulatorReg.Value != Settings.ZERO_WORD_38)
                cpu.InstructionCounter.Value = cpu.IndexedAddress;
            else
                cpu.InstructionCounter.Increment(1);
        }

        /// <summary>
        /// Transfer on Plus 2    TPL Y    +0120
        /// If the sign bit of the Ac is positive, the calculator takes the next instruction from 
        /// location Y and pro¬ceeds from there. If the sign bit of the AC is negative, the calculator 
        /// proceeds to the next instruction in sequence.
        /// </summary>
        /// <param name="cpu"></param>
        private static void TPL(CPU cpu)
        {
            if (cpu.AccumulatorReg.PBit == "0")
                cpu.InstructionCounter.Value = cpu.IndexedAddress;
            else
                cpu.InstructionCounter.Increment(1);
        }

        /// <summary>
        /// Transfer on Minus 2    TMI Y    —0120
        /// In the sign bit of the AC is negative, the calculator takes the next instruction from location Y 
        /// and pro¬ceeds from there. If the sign bit of the AC is positive, the calculator proceeds to the 
        /// next instruction in sequence.
        /// </summary>
        /// <param name="cpu"></param>
        private static void TMI(CPU cpu)
        {
            if (cpu.AccumulatorReg.PBit == "1")
                cpu.InstructionCounter.Value = cpu.IndexedAddress;
            else
                cpu.InstructionCounter.Increment(1);
        }

        /// <summary>
        /// Transfer on Overflow 2    TOV Y    +0140
        /// If the AC overflow indicator and light are on as the result of a previous operation, 
        /// the indicator and light are turned off and the calculator takes the next in¬struction 
        /// from location Y and proceeds from there. If the indicator and light are off, the calculator 
        /// pro¬ceeds to the next instruction in sequence.
        /// </summary>
        /// <param name="cpu"></param>
        private static void TOV(CPU cpu)
        {
            if (cpu.AccumOverflowIndicator)
                cpu.InstructionCounter.Value = cpu.IndexedAddress;
            else
                cpu.InstructionCounter.Increment(1);
        }

        /// <summary>
        /// Transfer on No Overflow 2    TNO Y    —0140
        /// If the AC overflow indicator and light are off, the calculator takes the next instruction from 
        /// location Y and proceeds from there. If the indicator and light are on, the calculator proceeds 
        /// to the next instruction in sequence after turning off the indicator and light.
        /// </summary>
        /// <param name="cpu"></param>
        private static void TNO(CPU cpu)
        {
            if (!cpu.AccumOverflowIndicator)
                cpu.InstructionCounter.Value = cpu.IndexedAddress;
            else
                cpu.InstructionCounter.Increment(1);
        }

        /// <summary>
        /// Transfer on MQ Plus 2    TQP Y    +0162
        /// If the sign bit of the MQ is positive, the calculator takes the next instruction from 
        /// location Y and pro¬ceeds from there. If the sign bit of the MQ is nega¬tive, the 
        /// calculator proceeds to the next instruction in sequence.
        /// </summary>
        /// <param name="cpu"></param>
        private static void TQP(CPU cpu)
        {
            if (cpu.MQReg.SBit == "0")
                cpu.InstructionCounter.Value = cpu.IndexedAddress;
            else
                cpu.InstructionCounter.Increment(1);
        }

        /// <summary>
        /// Transfer on Low MQ 2    TLQ Y    +0040
        /// If the C(MQ) are algebraically less than the C(AC), the calculator takes the next 
        /// instruction from location Y and proceeds from there. If the C(MQ) are alge¬braically 
        /// greater than or equal to the C(AC), the cal¬culator proceeds to the next instruction in sequence.
        /// </summary>
        /// <param name="cpu"></param>
        private static void TLQ(CPU cpu)
        {
            long fromMQ = Converter.ConvertToInt(cpu.MQReg.Value);
            long fromAC = Converter.ConvertToInt(cpu.AccumulatorReg.Value);

            if (fromMQ < fromAC)
                cpu.InstructionCounter.Value = cpu.IndexedAddress;
            else
                cpu.InstructionCounter.Increment(1);
        }

        /// <summary>
        /// Transfer on MQ Overflow 2    TQO Y    +0161
        /// If the MQ overflow indicator and light have been turned on by an overflow or underflow in the MQ 
        /// characteristic during a previous floating-point oper¬ation, the indicator and light are turned 
        /// off, the cal¬culator takes the next instruction from location Y and proceeds from there. 
        /// If the indicator and light are not on, the calculator proceeds to the next instruc¬tion in sequence.
        /// </summary>
        /// <param name="cpu"></param>
        private static void TQO(CPU cpu)
        {
            if (cpu.MQOverflowIndicator)
                cpu.InstructionCounter.Value = cpu.IndexedAddress;
            else
                cpu.InstructionCounter.Increment(1);
        }

        /// <summary>
        /// P Bit Test
        /// 2    PBT    —0761... 001
        /// If the C(AC)P is a one, the calculator skips the next 
        /// instruction and proceeds from there. If position P contains a zero, the calculator 
        /// takes the next instruc¬tion in sequence.
        /// </summary>
        /// <param name="cpu"></param>
        private static void PBT(CPU cpu)
        {
            if(cpu.AccumulatorReg.PBit == "1")
                cpu.InstructionCounter.Increment(2);
            else
                cpu.InstructionCounter.Increment(1);
        }

        /// <summary>
        /// Low Order Bit Test
        /// 2	LBT    +0765... 001
        /// If the C(AC)35 is a one, the calculator skips the next instruction and proceeds from there.  If position
        /// 3	5 contains a zero, the calculator takes the next in¬
        /// struction in sequence.
        /// </summary>
        /// <param name="cpu"></param>
        private static void LBT(CPU cpu)
        {
            if(cpu.AccumulatorReg.Value[35] == '1')
                cpu.InstructionCounter.Value = cpu.IndexedAddress;
            else
                cpu.InstructionCounter.Increment(1);
        }

        #endregion

        #region Logical Operations

        private static void CAL(CPU cpu) //Clear and Add Logical Word
        {
            //Storing core storage word to accumulator bits S,1-35
            cpu.AccumulatorReg.Bits_SignTo35 = cpu.GetCoreStorageWord(cpu.IndexedAddress);
            //Moving bit S to bit P
            cpu.AccumulatorReg.PBit = cpu.AccumulatorReg.SBit;
            cpu.AccumulatorReg.SBit = "0";
            cpu.AccumulatorReg.QBit = "0";

            //Increment IR
            cpu.InstructionCounter.Increment(1);
        }

        private static void SLW(CPU cpu) //Store Logical Word
        {
            //Result stores accumulator bits P,1-35 to the core storage word bits S,1-35
            string fromAccum = cpu.AccumulatorReg.PBit;
            string fromAccum2 = cpu.AccumulatorReg.Bits_SignTo35.Substring(1, 35);
            string result = fromAccum + fromAccum2;

            cpu.SetCoreStorageWord(result, cpu.IndexedAddress);

            //Increment IR
            cpu.InstructionCounter.Increment(1);
        }

        #endregion

        #region Floating Point Operations

        /// <summary>
        /// Floating Add
        /// 7-11    FAD Y    +0300
        /// The C(Y) are algebraically added to the C(AC), and this sum replaces the C(AC) and the C(MQ). 
        /// The C(Y) are unchanged.
        /// </summary>
        /// <param name="cpu"></param>
        private static void FAD(CPU cpu)
        {
            FloatingPointNumber fromCore = new FloatingPointNumber(cpu.GetCoreStorageWord(cpu.IndexedAddress));
            FloatingPointNumber fromAccum = new FloatingPointNumber(cpu.AccumulatorReg.Bits_SignTo35);

            fromAccum.DecimalValue = fromAccum.DecimalValue + fromCore.DecimalValue;

            cpu.AccumulatorReg.Bits_SignTo35 = fromAccum.Value;
            cpu.AccumulatorReg.PBit = "0";
            cpu.AccumulatorReg.QBit = "0";

            cpu.MQReg.Value = fromAccum.Value;

            cpu.InstructionCounter.Increment(1);
        }

        /// <summary>
        /// Floating Subtract 7-11    FSB Y    +0302
        /// </summary>
        /// <param name="cpu"></param>
        private static void FSB(CPU cpu)
        {
            FloatingPointNumber fromCore = new FloatingPointNumber(cpu.GetCoreStorageWord(cpu.IndexedAddress));
            FloatingPointNumber fromAccum = new FloatingPointNumber(cpu.AccumulatorReg.Bits_SignTo35);

            fromAccum.DecimalValue = fromAccum.DecimalValue - fromCore.DecimalValue;

            cpu.AccumulatorReg.Bits_SignTo35 = fromAccum.Value;
            cpu.AccumulatorReg.PBit = "0";
            cpu.AccumulatorReg.QBit = "0";

            cpu.MQReg.Value = fromAccum.Value;

            cpu.InstructionCounter.Increment(1);
        }

        #endregion
        
        #region Input Output Operations

        #region IO Unit Addresses
        //The addresses of the input-output units are given below.
        //COMPONENT    OCTAL   DECIMAL
        //CRT	        030	    024
        //Tapes 
        //BCD      	201-212	129-138
        //Binary	    221-232	145-154
        //Drum	        301-310	193-200
        //Card Reader	321	209
        //Card Punch	341	225
        //Printer	    361	241
        #endregion


        /// <summary>
        /// Read Select
        /// 2-111    RDS Y    +0762
        /// This instruction causes the calculator to prepare to read one record of information 
        /// from the component specified by Y. If Y specifies a tape unit, the MQ is cleared 
        /// by this instruction.
        /// </summary>
        /// <param name="cpu"></param>
        private static void RDS(CPU cpu)
        {
            string device = Converter.ConvertToOctal(cpu.IndexedAddress, 3);

            if (device == "221")
                cpu.SelectedIODevice = IODevice.TapeRead;
            else if (device == "301")
                cpu.SelectedIODevice = IODevice.DrumRead;
            else
                throw new Exception("Unknown IO Device Specified in RDS instruction.");
            
            //Increment the IRC
            cpu.InstructionCounter.Increment(1);
        }

        /// <summary>
        /// Write Select
        /// 2-111    WRS Y    +0766
        /// This instruction causes the calculator to prepare to write one record of 
        /// information on the component specified by Y.
        /// </summary>
        /// <param name="cpu"></param>
        private static void WRS(CPU cpu)
        {
            string device = Converter.ConvertToOctal(cpu.IndexedAddress, 3);

            if (device == "221")
                cpu.SelectedIODevice = IODevice.TapeWrite;
            else if (device == "301")
                cpu.SelectedIODevice = IODevice.DrumWrite;
            else if (device == "361")
                cpu.SelectedIODevice = IODevice.PrinterWrite;
            else
                throw new Exception("Unknown IO Device Specified in WRS instruction.");

            //Increment the IRC
            cpu.InstructionCounter.Increment(1);
        }

        #endregion

        #endregion

        #region Property Accessors

        public static Dictionary<string, Instruction> Instructions
        {
            get
            {
                if (_instructions == null)
                {
                    _instructions = new Dictionary<string, Instruction>();
                    

                    //Add the Fixed-Point Arithmetic Operations
                    _instructions.Add("+0500", new Instruction("CLA", "+0500", new InstructionExecute(CLA)));
                    _instructions.Add("+0400", new Instruction("ADD", "+0400", new InstructionExecute(ADD)));
                    _instructions.Add("+0402", new Instruction("SUB", "+0402", new InstructionExecute(SUB)));
                    _instructions.Add("+0401", new Instruction("ADM", "+0401", new InstructionExecute(ADM)));
                    _instructions.Add("+0502", new Instruction("CLS", "+0502", new InstructionExecute(CLS)));
                    _instructions.Add("-0400", new Instruction("SBM", "-0400", new InstructionExecute(SBM)));
                    _instructions.Add("+0200", new Instruction("MPY", "+0200", new InstructionExecute(MPY)));
                    _instructions.Add("+0601", new Instruction("STO", "+0601", new InstructionExecute(STO)));
                    _instructions.Add("+0621", new Instruction("STA", "+0621", new InstructionExecute(STA)));
                    _instructions.Add("+0560", new Instruction("LDQ", "+0560", new InstructionExecute(LDQ)));
                    _instructions.Add("+0768", new Instruction("CLM", "+0768", new InstructionExecute(CLM)));
                    _instructions.Add("+0767", new Instruction("CHS", "+0767", new InstructionExecute(CHS)));
                    _instructions.Add("+0763", new Instruction("SSP", "+0763", new InstructionExecute(SSP)));
                    _instructions.Add("+0764", new Instruction("SSM", "+0764", new InstructionExecute(SSM)));
                    _instructions.Add("-0600", new Instruction("STQ", "-0600", new InstructionExecute(STQ)));

                    //Add the Control Operations
                    _instructions.Add("+0760", new Instruction("ETM", "+0760", new InstructionExecute(ETM)));
                    _instructions.Add("-0760", new Instruction("LTM", "-0760", new InstructionExecute(LTM)));
                    _instructions.Add("+0000", new Instruction("HTR", "+0000", new InstructionExecute(HTR)));
                    _instructions.Add("+0761", new Instruction("NOP", "+0761", new InstructionExecute(NOP)));
                    _instructions.Add("+0420", new Instruction("HPR", "+0420", new InstructionExecute(HPR)));
                    _instructions.Add("+0020", new Instruction("TRA", "+0020", new InstructionExecute(TRA)));
                    _instructions.Add("+0021", new Instruction("TTR", "+0021", new InstructionExecute(TTR)));
                    _instructions.Add("+0100", new Instruction("TZE", "+0100", new InstructionExecute(TZE)));
                    _instructions.Add("-0100", new Instruction("TNZ", "-0100", new InstructionExecute(TNZ)));
                    _instructions.Add("+0120", new Instruction("TPL", "+0120", new InstructionExecute(TPL)));
                    _instructions.Add("-0120", new Instruction("TMI", "-0120", new InstructionExecute(TMI)));
                    _instructions.Add("+0140", new Instruction("TOV", "+0140", new InstructionExecute(TOV)));
                    _instructions.Add("-0140", new Instruction("TNO", "-0140", new InstructionExecute(TNO)));
                    _instructions.Add("+0162", new Instruction("TQP", "+0162", new InstructionExecute(TQP)));
                    _instructions.Add("+0161", new Instruction("TQO", "+0161", new InstructionExecute(TQO)));
                    _instructions.Add("+0040", new Instruction("TLQ", "+0040", new InstructionExecute(TLQ)));
                    _instructions.Add("-0761", new Instruction("PBT", "-0761", new InstructionExecute(PBT)));
                    _instructions.Add("+0765", new Instruction("LBT", "+0765", new InstructionExecute(LBT)));

                    //Add the Logical Operations
                    _instructions.Add("-0500", new Instruction("CAL", "-0500", new InstructionExecute(CAL)));
                    _instructions.Add("+0602", new Instruction("SLW", "+0602", new InstructionExecute(SLW)));

                    //Add the Floating Point Operations
                    _instructions.Add("+0300", new Instruction("FAD", "+0300", new InstructionExecute(FAD)));
                    _instructions.Add("+0302", new Instruction("FSB", "+0302", new InstructionExecute(FSB)));

                    //Add the Input Output Operations
                    _instructions.Add("+0762", new Instruction("RDS", "+0762", new InstructionExecute(RDS)));
                    _instructions.Add("+0766", new Instruction("WRS", "+0766", new InstructionExecute(WRS)));
                }

                return _instructions;
            }
        }

        #endregion
    }
}
