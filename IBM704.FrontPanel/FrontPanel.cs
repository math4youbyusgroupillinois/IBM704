using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using IBM704.Cpu;

namespace IBM704.FrontPanel
{
    public class FrontPanel
    {
        #region Declarations 

        private CPU _cpu; 

        //Lights
        private bool _autoLight;
        private bool _readWriteCheckLight;
        private string _indexRegDisplay;

        private IndexDisplay _currentIndexReg;

        private string _keyedInWord;


        #endregion

        #region Initialization & Setup

        public FrontPanel()
        {
            _cpu = new CPU();
        }

        #endregion

        #region Public Methods

        public void ChangeToManual()
        {
            _cpu.Mode = RunMode.Manual;
            _autoLight = false;
        }

        public void ChangeToAutomatic()
        {
            _cpu.Mode = RunMode.Automatic;
            _autoLight = true;
        }

        public void Clear()
        {
            if(_cpu.Mode != RunMode.Automatic)
                return;

            _cpu.Clear();

            Reset();
        }

        /// <summary>
        /// MANUAL mode only.
        /// </summary>
        public void SingleStep()
        {
            if (_cpu.Mode != RunMode.Manual)
                return;

            _cpu.ExecuteSingleStep();
        }

        /// <summary>
        /// MANUAL mode only.
        /// </summary>
        public void MultiStep()
        {
            if (_cpu.Mode != RunMode.Manual)
                return;

            _cpu.ExecuteMultistep();
        }

        /// <summary>
        /// Will set the Front Panel to display the contents of index register A, B, or C
        /// 
        /// MANUAL mode only.
        /// </summary>
        public void SetDisplayIndex(IndexDisplay newIndex)
        {
            if (_cpu.Mode != RunMode.Manual)
                return;

            _currentIndexReg = newIndex;
        }

        public void LoadTape()
        {
            _cpu.LoadProgram();
        }

        public void LoadDrum()
        {
            _cpu.LoadProgram();
        }

        public void Reset()
        {
            _cpu.Reset();
        }

        /// <summary>
        /// AUTOMATIC mode only.
        /// </summary>
        public void Start()
        {
            ///SHOULD this be automatic only?
            if (_cpu.Mode != RunMode.Automatic)
                return;

            ReadWriteCheckLight = false;

            _cpu.ExecuteContinuous();
        }

        /// <summary>
        /// Enter MQ Key. If the operator manually keys a 
        /// given word of information into the panel input switches and if the enter
        /// MQ key is pressed while the calculator is on MANUAL, then the keyed-in 
        /// word replaces the contents of the MQ. The contents of the SR are destroyed 
        /// by this operation.
        /// 
        /// MANUAL mode only.
        /// </summary>
        public void EnterMQ()
        {
            if (_cpu.Mode != RunMode.Manual)
                return;

            if (KeyedInWord == null)
                throw new Exception("Enter MQ Error: No word has been keyed in.");

            _cpu.MQReg.Value = KeyedInWord;
            _cpu.StorageReg.Value = Settings.ZERO_WORD_36;
        }

        /// <summary>
        /// Enter Instruction Key. If the operator presses the enter instruction key 
        /// under the same conditions as above, then the operation part of the keyed-in 
        /// word goes into the instruction register, the full word is placed in the SR, and 
        /// the instruction is executed.
        /// 
        /// MANUAL mode only.
        /// </summary>
        public void EnterInstruction()
        {
            if (_cpu.Mode != RunMode.Manual)
                return;

            if (KeyedInWord == null)
                throw new Exception("Enter Instruction Error: No word has been keyed in.");

            _cpu.InstructionReg.Value = KeyedInWord;
            _cpu.StorageReg.Value = KeyedInWord;
            SingleStep();
        }

        /// <summary>
        /// Display Storage Key. If, while the calculator is on MANUAL, the 
        /// operator keys the location into the address part of the panel input 
        /// switches, and presses the display storage key, the contents of the 
        /// keyed-in location go into the SR where they may be read from the SR lights.
        /// 
        /// MANUAL mode only.
        /// </summary>
        public void DisplayStorage()
        {
            if (_cpu.Mode != RunMode.Manual)
                return;

            if (KeyedInWord == null)
                throw new Exception("Display Storage Error: No word has been keyed in.");

            string fromCore = _cpu.GetCoreStorageWord(KeyedInWord.Substring(Settings.ADDRESS_INDEX));

            _cpu.StorageReg.Value = fromCore;
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
        /// MANUAL mode only.
        /// </summary>
        public void DisplayEffectiveAddress()
        {
            if (_cpu.Mode != RunMode.Manual)
                return;

            _cpu.SetSRegEffectiveAddress();
        }

        #endregion

        #region PropertyAccessors

        /// <summary>
        /// Gets the string representing the current binary value of the Index Register Display
        /// </summary>
        public string IndexRegisterDisplay
        {
            get
            {
               switch(_currentIndexReg)
                {
                    case IndexDisplay.IndexA:
                        _indexRegDisplay = Converter.PadBinaryString(_cpu.IndexRegA.Value, 15);
                        break;
                    case IndexDisplay.IndexB:
                        _indexRegDisplay = Converter.PadBinaryString(_cpu.IndexRegB.Value, 15);
                        break;
                    case IndexDisplay.IndexC:
                        _indexRegDisplay = Converter.PadBinaryString(_cpu.IndexRegC.Value, 15);
                        break;
                }
                
                return _indexRegDisplay;
            }
        }

        public bool TrapIndicator
        {
            get
            {
                return _cpu.TrappingModeIndicator;
            }
        }

        public bool ProgramStopLight
        {
            get{ return !_cpu.Running; }
        }

        public bool AccumulatorOverflowLight
        {
            get { return _cpu.AccumOverflowIndicator; }
        }

        public bool MQOverflowLight
        {
            get { return _cpu.MQOverflowIndicator; }
        }

        public bool DivideCheckLight
        {
            get { return _cpu.DivideCheckIndicator; }
        }

        public int ReadWriteSelectLight
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public bool ReadWriteCheckLight
        {
            get
            {
                return _readWriteCheckLight;
            }
            set
            {
                _readWriteCheckLight = value;
            }
        }

        public int TapeCheckLight
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public int ReadyLight
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public bool PowerLight
        {
            get{ return true; }
        }

        public bool AutomaticLight
        {
            get
            {
                return _autoLight;
            }
            set
            {
                _autoLight = value;
            }
        }

        public string KeyedInWord
        {
            get
            {
                return _keyedInWord;
            }
            set
            {
                if (value.Length != Settings.STD_WORD_SIZE)
                    throw new Exception(
                        "Attempted to input a value into the \"KeyedInWord\" that was not of standard word size");

                _keyedInWord = value;
            }
        }

        public string KeyedInWordOctal
        {
            get{ return Instruction.ConvertToOctal(KeyedInWord); }
            set
            {
                if(value.Length != 13)
                    throw new Exception(
                        "Attempted to input a value into the \"KeyedInWord\" that was not of standard word size");

                KeyedInWord = Instruction.ConvertToBinary(value);
            }

        }

        public string AccumDisplay
        {
            get { return _cpu.AccumulatorReg.Value; }
        }

        public string InstructionCounterDisplay
        {
            get { return _cpu.InstructionCounter.Value; }
        }

        public string InstructionRegisterDisplay
        {
            get { return _cpu.InstructionReg.Value; }
        }

        public string MQRegisterDisplay
        {
            get { return _cpu.MQReg.Value; }
        }

        public string StorageRegisterDisplay
        {
            get { return _cpu.StorageReg.Value; }
        }

        public RunMode Mode
        {
            get { return _cpu.Mode; }
        }

        public bool IsAutomatic
        {
            get
            {
                if(Mode == RunMode.Automatic)
                    return true;
                return false;
            }
        }

        #endregion
    }
}
