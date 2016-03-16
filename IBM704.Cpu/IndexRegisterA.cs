using System;
using System.Collections.Generic;
using System.Text;

namespace IBM704.Cpu
{
    /// <summary>
    /// Index Registers. There are three registers, each with a capacity of 12, 13, or 15 bits
    ///  (for 4,096, 8,192, or 32,768 words of core storage), called index registers A, B, and
    ///  C. These registers make possible the auto¬matic counting and address modification features
    ///  of the 704.
    ///With respect to the index registers, the 704 instruc¬tions fall into two classes,
    ///  non-indexable and indexable.
    ///The non-indexable instructions are the five Type A instructions Tix, TNX, TXH, TXL,
    ///  TXI and seven of the Type B instructions, namely, TSX, LXA, LXD, SXD, PXD, PAX,
    ///  and PDX. (Notice that these are the only instructions with an X in the operation
    ///  code.) Instructions of this class are used to test and manipu¬late the contents of
    ///  the index register specified in their tag field.
    ///All other instructions are indexable instructions in their normal form. They are
    ///  recognizable by the fact that position 8 or 9, or both, contain a zero. If an indexable
    ///  instruction specifies an index register (that is, one of the three bits in its tag
    ///  field is a 1), it is executed as if its address field had contained its stated address
    ///  minus the contents of the specified index register. Suppose, for example, that index
    ///  register B contains 01178 and that the instruction CLA B 21178, contained in location
    ///  1000, is executed. After the execution, the accumulator will contain the contents of
    ///  core storage location 20008. However, the con¬tents of location 1000 are still CLA B 
    /// 21178. This is called effective address modification; that is, the address of the
    ///  instruction is modified in the control unit for execution purposes but is unaltered in
    ///  storage.
    ///If an instruction specifies no index register (all three bits in its tag field are zeros),
    ///  it is executed as if the index registers did not exist. Thus CLA 21178 will place the
    ///  contents of core storage location 21178 in the accumulator, regardless of the contents
    ///  of the index registers.
    ///Note   that   in   the   case   of   the   fourteen   sense-type instructions, effective
    ///  address modification may   I actually cause operation modification, because the last
    ///eight bits of these instructions are part of their opera¬tions. For example, if index register
    ///  A contains 00018, then SSP is executed as SSP, but SSP A is exe¬cuted as CHS. (See instructions
    ///  for octal code of SSP and CHS.)
    ///An instruction may refer to more than one index register by placing multiple l's in the tag field,
    ///  such as Oil (when programming, this number must be written in octal form). An instruction
    ///  (except a fixed instruction) with this tag is executed as if there were a single index register,
    ///  equivalent to index registers A and B connected in logical OR fashion. For example, if index
    ///  registers A and B contain 32048 and 36318, respectively, the instruction CLA 3 65218 is executed
    ///  with an effective address 65218 — 36358 = 26648. Similarly, the instruction LXD 3 16418
    ///  causes the contents of both index regis¬ters A and B to be replaced by the contents of the
    ///  decrement part of core storage location 16418.
    ///The tag field specifies one or more of the three index registers or no index register as follows:

    ///TAG FIELD BINARY    OCTAL	INDEX REGISTER (s) SPECIFIED
    ///000	0		None
    ///001	1		A
    ///010	2		B
    ///100	4		C
    ///011	3		A OR B
    ///101	5		A OR C
    ///110	6		B OR C
    ///111	7	A	OR B OR C
    ///A non-indexable instruction with a zero tag is exe¬cuted as if there were an imaginary index
    ///  register always containing zeros. For example, PXD with a tag of zero clears the entire AC;
    ///  SXD with a tag of zero clears the decrement field of the storage location to which it refers.
    /// </summary>
    public class IndexRegisterA
    {
        private string _value;

        public IndexRegisterA()
        {
            _value = Settings.ZERO_WORD_12;
        }

        /// <summary>
        /// Returns a 12 "Bit" string in a binary format ("001001") that represents the current binary value of the register 
        /// </summary>
        public string Value
        {
            get
            { return _value; }
            set
            {
                if( value.Length != Settings.INDEX_A_SIZE)
                    throw new Exception("Attempted to put an incorrect size word in Index Register A.");

                _value = value;
            }
        }
    }
}
