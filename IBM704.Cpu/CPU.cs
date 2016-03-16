using System;
using System.Collections.Generic;
using System.Text;
using IBM704.CoreStorage;


namespace IBM704.Cpu
{
    /// <summary>
    /// Class that represents the CPU of the IBM704. This class contains all of the components, such as registers, that are needed to 
    /// operate properly.
    /// </summary>
    public class CPU
    {
        #region Declarations

        /// <summary>
        /// Either Automatic or Manual: Determined by the Front Panel Switch
        /// </summary>
        private RunMode _runMode;
        private bool _running;
        private int _senseSwitches;
        private int _senseLights;
        private CoreStorage.CoreStorage _coreStorage;

        //Indicators
        private bool _mqOverflowIndicator = false;
        private bool _accumOverflowIndicator = false;
        private bool _divideCheckIndicator = false;
        private bool _tapeCheckIndicator = false;
        private bool _trappingModeIndicator = false;
        private bool _readWriteCheckIndicator = false;
        
        //Registers
        private InstructionRegister _instructionRegister;
        private Accumulator _accumulator;
        private IndexRegisterA _indexRegA;
        private IndexRegisterB _indexRegB;
        private IndexRegisterC _indexRegC;
        private InstructionLocationCounter _instructionCounter;
        private MultiplierQuotientRegister _mqRegister;
        private StorageRegister _storageRegister;

        /// <summary>
        /// Stores the selected IO device
        /// </summary>
        private IODevice _selectedIODevice;

        #endregion

        #region Instantiation & Setup

        public CPU( )
        {
            _runMode = RunMode.Manual;
            _running = false;
            _trappingModeIndicator = false;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method will execute instructions until some form of "HALT" is processed. 
        /// This method is called when the 704 is switched to automatic and the start key is pressed.
        /// </summary>
        public void ExecuteContinuous()
        {
            Running = true;

            while( Running )
            {
                if (Mode != RunMode.Automatic)
                    throw new Exception("Attempted to execute continuously while in Manual");

                Execute();
            }
        }

        public void ExecuteMultistep()
        {
            if (Mode != RunMode.Manual)
                throw new Exception("Attempted to execute a multistep while in Automatic");

            Running = true;

            for (int i = 0; i < Settings.MULTISTEP_NUM; i++)
            {
                if( !Running)
                    break;

                Execute();
            }

            Running = false;
        }

        public void ExecuteSingleStep()
        {
            if (Mode != RunMode.Manual)
                throw new Exception("Attempted to execute a single step while in Automatic");

            Running = true;

            Execute();

            Running = false;
        }
        
        /// <summary>
        /// From the IBM 704 Manual:
        /// 
        /// Reset Key. Pressing the reset key resets all registers and indicators in the logical 
        /// section of the machine. That is, the SR, AC, MQ, instruction location counter, instruction 
        /// register, and index registers are set to zero and all indicators are turned off. 
        /// The panel lights are all turned off with the exception of those marked POWER and READY. 
        /// Core storage is not affected by the reset key.
        /// </summary>
        public void Reset()
        {
            StorageReg.Value = Settings.ZERO_WORD_36;
            AccumulatorReg.Value = Settings.ZERO_WORD_38;
            MQReg.Value = Settings.ZERO_WORD_36;
            InstructionCounter.Value = Settings.ZERO_WORD_15;
            InstructionReg.Value = Settings.ZERO_WORD_36;
            IndexRegA.Value = Settings.ZERO_WORD_12;
            IndexRegB.Value = Settings.ZERO_WORD_13;
            IndexRegC.Value = Settings.ZERO_WORD_15;

            MQOverflowIndicator = false;
            AccumOverflowIndicator = false;
            DivideCheckIndicator = false;
            TapeCheckIndicator = false;
            TrappingModeIndicator = false;
        }

        /// <summary>
        /// From the IBM 704 Manual:
        /// 
        /// Clear Key. If the calculator is on AUTOMATIC, pressing the clear key resets all the 
        /// registers in core storage. The entire logical section is reset, just as if the reset 
        /// key had been depressed also. The clear key is ineffective when the calculator is on MANUAL.
        /// 
        /// Also know as the DEATH key.
        /// </summary>
        public void Clear()
        {
            if (Mode != RunMode.Automatic)
                throw new Exception("Attempted to execute a Clear command while in Manual");

            _coreStorage = new CoreStorage.CoreStorage();

            Reset();
        }

        public void LoadProgram()
        {
            List<string> program = SecondaryStorage.SecondaryStorage.TestProgram();

            for (int i = 0; i < program.Count; i++)
            {
                Core.Words[i].Value = Converter.ConvertToBinary(program[i], 36);
            }

            InstructionReg.Value = Core.Words[0].Value;
        }

        public string GetCoreStorageWord(string binary)
        {
            return Core.Words[Converter.ConvertToInt(binary)].Value;
        }

        public void SetCoreStorageWord(string newValue, string index)
        {
            Core.Words[Converter.ConvertToInt(index)].Value = newValue;
        }

        public void SetCoreStorageWordAddress(string newAddress, string index)
        {
            Core.Words[Converter.ConvertToInt(index)].Address = newAddress;
        }

        /// <summary>
        /// Display Effective Address Key. Assume the calculator is on MANUAL and the display effective address key 
        /// is pressed. The difference between the contents of the address field of the instruction in the SR and those 
        /// of the index register tagged in that instruction (if one is tagged) will appear in the address field of the 
        /// SR where it may be read from the SR lights. If any index registers have been displayed prior to dis¬playing 
        /// effective address, put the automatic-manual switch on AUTOMATIC and then back on MANUAL before pressing the 
        /// display effective address key.
        ///
        /// The circuitry for displaying the effective address does not distinguish between instruction types; hence even 
        /// the address of type A instructions will appear as an "effective address."
        /// 
        /// NEED TO VERIFY THIS IS CORRECT
        /// </summary>
        public void SetSRegEffectiveAddress()
        {
            //Return the address contained in the storage register (bits 21-35) and subtract the TagIndex (determined by the logical OR of Index Registers specified by the Tag (bits 18-20).
            string instructAddress = StorageReg.AddressTypeA;
            long modifiedAddress = Converter.ConvertToInt(instructAddress) - Converter.ConvertToInt(TagIndexSR);

            StorageReg.Value = Converter.ConvertToBinary(modifiedAddress, 36);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This method will execute the instruction currently loaded into the IR and upon completion it will load the 
        /// instruction specified by the Instruction Location Counter into the IR. 
        /// </summary>
        private void Execute()
        {
            //Find the instructions octal representation 
            //Use the octal representation to find the actual instruction and execute it
            InstructionSet.Instructions[InstructionReg.OperationPart_Octal].Execute(this);

            //Grab the next instruction from Core Storage and place in the instruction register
            long index = Converter.ConvertToInt(InstructionCounter.Value);
            InstructionReg.Value = Core.Words[index].Value;
        }

        /// <summary>
        /// This performs a Logical OR of the two binary strings passed in.
        /// </summary>
        /// <param name="binaryInput1">A string in a binary format. Example "011001"</param>
        /// <param name="binaryInput2">A string in a binary format. Example "011001"</param>
        /// <returns>A binary string (ex. "110011") that is the same length as the longest of the inputs. For example, if binaryInput1.Length > binaryInput2.Length
        /// then result.Length = binaryInput1.Length</returns>
        private static string LogicalOR(string binaryInput1, string binaryInput2)
        {
            if (binaryInput1.Length > binaryInput2.Length)
                binaryInput2 = Converter.PadBinaryString(binaryInput2, binaryInput1.Length);
            else if (binaryInput2.Length > binaryInput1.Length)
                binaryInput1 = Converter.PadBinaryString(binaryInput1, binaryInput2.Length);

            char[] result = binaryInput1.ToCharArray();

            for (int i = 0; i < binaryInput2.Length; i++)
            {
                if (binaryInput2[i] == '1')
                    result[i] = '1';
            }

            return new string(result);
        }



        #endregion

        #region Property Accessors

        /// <summary>
        /// Gets and Sets the SelectedIODevice
        /// </summary>
        public IODevice SelectedIODevice
        {
            get { return _selectedIODevice; }
            set { _selectedIODevice = value; }
        }

        /// <summary>
        /// Gets the Accumulator Register
        /// </summary>
        public Accumulator AccumulatorReg
        {
            get
            {
                if (_accumulator == null)
                    _accumulator = new Accumulator();

                return _accumulator;
            }
        }

        /// <summary>
        /// Gets the Storage Register
        /// </summary>
        public StorageRegister StorageReg
        {
            get
            {
                if(_storageRegister == null)
                    _storageRegister = new StorageRegister();

                return _storageRegister;
            }
        }

        /// <summary>
        /// Gets the Instruction Location Counter
        /// </summary>
        public InstructionLocationCounter InstructionCounter
        {
            get
            {
                if (_instructionCounter == null)
                    _instructionCounter = new InstructionLocationCounter();

                return _instructionCounter;
            }
        }

        /// <summary>
        /// Gets the Multiplier Quotient Register
        /// </summary>
        public MultiplierQuotientRegister MQReg
        {
            get
            {
                if (_mqRegister == null)
                    _mqRegister = new MultiplierQuotientRegister();

                return _mqRegister;
            }
        }

        public IndexRegisterA IndexRegA
        {
            get
            {
                if (_indexRegA == null)
                    _indexRegA = new IndexRegisterA();

                return _indexRegA;
            }
        }

        public IndexRegisterB IndexRegB
        {
            get
            {
                if (_indexRegB == null)
                    _indexRegB = new IndexRegisterB();

                return _indexRegB;
            }
        }

        public IndexRegisterC IndexRegC
        {
            get
            {
                if (_indexRegC == null)
                    _indexRegC = new IndexRegisterC();

                return _indexRegC;
            }
        }

        public InstructionRegister InstructionReg
        {
            get
            {
                if (_instructionRegister == null)
                    _instructionRegister = new InstructionRegister();

                return _instructionRegister;
            }
        }

        /// <summary>
        /// Returns the logical OR of the registers indicated in the tag field of the instruction register (bit 18-20).
        /// Bit 18 set to "1" represents IndexRegC. Bit 19 set to "1" represents IndexRegB. Bit 20 set to "1" represents IndexRegA.
        /// </summary>
        public string TagIndex
        {
            get
            {
                string result = "0";
                //TODO: TagIndex (determined by the logical OR of Index Registers specified by the Tag (bits 18-20)
                switch(Converter.ConvertToInt(InstructionReg.Tag))
                {
                    case 0:
                        result = "0";
                        break;
                    case 1:
                        result = IndexRegA.Value;
                        break;
                    case 2:
                        result = IndexRegB.Value;
                        break;
                    case 3:
                        result = LogicalOR(IndexRegA.Value, IndexRegB.Value);
                        break;
                    case 4:
                        result = IndexRegC.Value;
                        break;
                    case 5:
                        result = LogicalOR(IndexRegA.Value, IndexRegC.Value);
                        break;
                    case 6:
                        result = LogicalOR(IndexRegB.Value, IndexRegC.Value);
                        break;
                    case 7:
                        result = LogicalOR(IndexRegA.Value, IndexRegB.Value);
                        result = LogicalOR(result, IndexRegC.Value);
                        break;
                }

                return result;
            }
        }

        /// <summary>
        /// Returns the logical OR of the registers indicated in the tag field of the storage register (bit 18-20).
        /// Bit 18 set to "1" represents IndexRegC. Bit 19 set to "1" represents IndexRegB. Bit 20 set to "1" represents IndexRegA.
        /// </summary>
        public string TagIndexSR
        {
            get
            {
                string result = "0";
                //TODO: TagIndex (determined by the logical OR of Index Registers specified by the Tag (bits 18-20)
                switch (Converter.ConvertToInt(StorageReg.Tag))
                {
                    case 0:
                        result = "0";
                        break;
                    case 1:
                        result = IndexRegA.Value;
                        break;
                    case 2:
                        result = IndexRegB.Value;
                        break;
                    case 3:
                        result = LogicalOR(IndexRegA.Value, IndexRegB.Value);
                        break;
                    case 4:
                        result = IndexRegC.Value;
                        break;
                    case 5:
                        result = LogicalOR(IndexRegA.Value, IndexRegC.Value);
                        break;
                    case 6:
                        result = LogicalOR(IndexRegB.Value, IndexRegC.Value);
                        break;
                    case 7:
                        result = LogicalOR(IndexRegA.Value, IndexRegB.Value);
                        result = LogicalOR(result, IndexRegC.Value);
                        break;
                }

                return result;
            }
        }

        /// <summary>
        ///  //Return the address contained in the instruction register (bits 21-35) and subtract the TagIndex 
        /// (determined by the logical OR of Index Registers specified by the Tag (bits 18-20). The format is a binary string "010001001" and will be of length 15.
        /// </summary>
        public string IndexedAddress
        {
            get
            {
                //Return the address contained in the instruction register (bits 21-35) and subtract the TagIndex (determined by the logical OR of Index Registers specified by the Tag (bits 18-20).
                string instructAddress = InstructionReg.AddressTypeA;
                long modifiedAddress = Converter.ConvertToInt(instructAddress) - Converter.ConvertToInt(TagIndex);

                return Converter.ConvertToBinary(modifiedAddress, 15);
            }
        }

        public bool MQOverflowIndicator
        {
            get { return _mqOverflowIndicator; }
            set { _mqOverflowIndicator = value; }
        }

        public bool AccumOverflowIndicator
        {
            get { return _accumOverflowIndicator; }
            set { _accumOverflowIndicator = value; }
        }

        public bool DivideCheckIndicator
        {
            get { return _divideCheckIndicator; }
            set { _divideCheckIndicator = value; }
        }

        public bool TapeCheckIndicator
        {
            get { return _tapeCheckIndicator; }
            set { _tapeCheckIndicator = value; }
        }



        /// <summary>
        /// Gets or Sets the Mode for the IBM 704. This should be set via the Front Panel Layer. Either "Automatic" or  "Manual"
        /// </summary>
        public RunMode Mode
        {
            get { return _runMode; }
            set
            {
                if(value == RunMode.Manual)
                    Running = false;

                _runMode = value;
            }
        }

        /// <summary>
        /// Gets or Sets the current Trapping State for the CPU
        /// </summary>
        public bool TrappingModeIndicator
        {
            get { return _trappingModeIndicator; }
            set { _trappingModeIndicator = value; }
        }

        /// <summary>
        /// Gets or Sets the current Running State for the CPU
        /// </summary>
        public bool Running
        {
            get { return _running; }
            set { _running = value; }
        }

        public CoreStorage.CoreStorage Core
        {
            get
            {
                if( _coreStorage == null)
                    _coreStorage = new CoreStorage.CoreStorage();
                
                return _coreStorage;
            }
        }

        public bool ReadWriteCheckIndicator
        {
            get { return _readWriteCheckIndicator; }
            set { _readWriteCheckIndicator = value; }
        }

        #endregion
    }
}
