using System;
using System.Collections.Generic;
using System.Text;

namespace IBM704.Cpu
{
    public enum InstructionType
    {
        TypeA,
        TypeB,
    }

    public enum WordType
    {
        Instruction,
        FixedPoint,
        FloatingPoint,
    }

    public enum RunMode
    {
        Automatic,
        Manual,
    }

    public enum ExecutionType
    {
        SingleStep,
        MultiStep,
        Automatic,
    }

    public enum RunState
    {
        Running,
        NotRunning,
    }

    public enum IODevice
    {
        TapeRead,
        TapeWrite,
        DrumRead,
        DrumWrite,
        PrinterWrite,
    }
}
