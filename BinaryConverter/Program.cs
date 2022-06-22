using System;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BinaryConverter
{
    class Program
    {
        #region other stuff

        public enum OpCodes : byte
        {
            //NOP
            NAH = (0x00), //NAH(nah|its|ok) Does Nothing

            //Math
            ADD = (0x10), //ADD(Rdest|R1|R2) Adds R1 & R2 to Rdest
            SUB = (0x11), //SUB(Rdest|R1|R2)  
            MUL = (0x12), //MUL(Rdest|R1|R2)
            DIV = (0x13), //DIV(Rdest|R1|R2)
            MOD = (0x14), //MOD(Rdest|R1|R2)

            //Logic
            AND = (0x20), //NOT(Rdest|R1|R2) 
            OR = (0x21),  //OR(Rdest|R1|R2) Places the result in Rdest register 
            XOR = (0x22), //XNOR(Rdest|R1|R2) 
            NOT = (0x23), //NOT(Rdest|R1|nah) 
            LTE = (0x24),
            EQ = (0x25),

            //Flow Control
            JMP = (0x30), //JMP(Addrh|Addrl|nah) Jumps to a memory location denoted by Addrh & Addrl bytes 
            JMPT = (0x31),
            JMPZ,
            JMPi,
            JMPZi,

            //Memory
            SET = (0x40),
            COPY = (0x41), //MOV
            LOAD = (0x42),
            STORE = (0x43),
            PUSH = (0x44),
            POP = (0x45),
            LOADi,
            STOREi,
        }

        public static short[] Reg = new short[5];
        public static int[] RAM = new int[10]; //instructions

        const int IP = 4;
        const int R0 = 0;
        const int R1 = 1;
        const int R2 = 2;
        const int R3 = 2;

        //MY solution
        static string DecimalToBinary(int number)
        {
            //12->1100
            int counter = 10;

            string binaryNumb = "";

            while (counter >= 0)
            {
                if ((int)(number / Math.Pow(2, counter)) != 0)
                {
                    binaryNumb = $"{binaryNumb}1";
                    number = number - (int)(Math.Pow(2, counter));
                }
                else
                {
                    if (binaryNumb.Contains('1'))
                    {
                        binaryNumb = $"{binaryNumb}0";
                    }
                }
                counter--;
            }

            return binaryNumb;
        }

        //Better solution
        static string BetterDecimalToBinary(int number)
        {
            StringBuilder builder = new StringBuilder();

            while (number != 0)
            {
                int lastDigit = number % 2; //Reads last digit of number
                builder.Insert(0, lastDigit); //Adds to string
                number >>= 1; //Shift all digits once to the right, same as number /= 2;
                //1000 => 0100
            }

            return builder.ToString();
        }

        static bool IsPowerOfTwo(int number)
        {
            int oneCounter = 0;

            while (number > 0)
            {
                int lastDigit = number & 1;

                if (lastDigit == 1) //&& binaryNumber != 1)
                {
                    oneCounter++;
                }

                if (oneCounter > 1)
                {
                    return false;
                }

                number >>= 1;
            }

            return true;
        }

        //static int RotateLeft(int number)
        //{
        //    //int lastDigit = number & 1;
        //    number <<= 2;

        //    if(lastDigit == 1)
        //    {
        //        number += 1;
        //    }

        //    return number;
        //}

        static int ClosestPowerOfTwo(int number) //Returns Lower
        {
            int oneCounter = 0;

            while (number > 0)
            {
                int lastDigit = number & 1;

                if (lastDigit == 1) //&& binaryNumber != 1)
                {
                    oneCounter++;
                }

                if (oneCounter == 2)
                {

                }

                number >>= 1;
            }

            return -1;
        }

        static bool IsNthBitOne(int number, int bitIndex)
        {
            number >>= bitIndex;

            int lastDigit = number & 1;

            return lastDigit == 1;
        }

        static int PullOutNthByte(int number, int byteIndex)
        {
            int NthByte = 0;

            number >>= byteIndex * 8;

            NthByte = number & 0b_11111111;

            return NthByte;
        }

        static byte GetNthByte(int item, int index)
        {
            return (byte)((item >> (index * 8)) & 0xFF);
        }

        static int FlipBit(int number, int bitIndex)
        {
            int tempNumber = number;

            int numberAfterBit = tempNumber >>= bitIndex + 1;
            int bit;

            if (IsNthBitOne(number, bitIndex))
            {
                bit = 0;
            }
            else
            {
                bit = 1;
            }

            return number >> bitIndex + bit + numberAfterBit;  // + bit + tempNumber;

            //split it up into two numbers, before and after the bit. flip the bit, add the before, bit, after
        }

        static int Calculate(int instruction)
        {
            //1st & 2nd byte for numbers, 3rd byte for instruction, 4th just hanging around
            //1 = add, 2 = sub, 3 = mul, 4 = div

            int a = PullOutNthByte(instruction, 2);
            int b = PullOutNthByte(instruction, 1);
            int opperation = PullOutNthByte(instruction, 0);
            ;

            if (opperation == 1)
            {
                //add
                return a + b;
            }
            else if (opperation == 2)
            {
                //sub
                return a - b;
            }
            else if (opperation == 3)
            {
                //mul
                return a * b;
            }
            else if (opperation == 4)
            {
                //div
                return a / b;
            }

            return -1;
        }

        static void DoWork(int instruction)
        {
            //destination = source
            //0x00|FF|00|00
            byte upCode = GetNthByte(instruction, 3);
            byte Rdest = GetNthByte(instruction, 2);
            byte R1 = GetNthByte(instruction, 1);
            byte R2 = GetNthByte(instruction, 0);

            if (upCode == (byte)OpCodes.ADD)
            {
                Reg[Rdest] = (byte)(R1 + R2);
            }
            else if (upCode == (byte)OpCodes.SUB)
            {
                Reg[Rdest] = (byte)(R1 - R2);
            }
            else if (upCode == (byte)OpCodes.MUL)
            {
                Reg[Rdest] = (byte)(R1 * R2);
            }
            else if (upCode == (byte)OpCodes.DIV)
            {
                Reg[Rdest] = (byte)(R1 / R2);
            }
            else if (upCode == (byte)OpCodes.MOD)
            {
                Reg[Rdest] = (byte)(R1 % R2);
            }

            else if (upCode == (byte)OpCodes.AND)
            {
                Reg[Rdest] = (short)(Reg[R1] & Reg[R2]);
            }
            else if (upCode == (byte)OpCodes.NOT)
            {
                Reg[Rdest] = (short)(~Reg[R1]);
            }
            else if (upCode == (byte)OpCodes.LTE)
            {
                if (Reg[R1] <= Reg[R2])
                {
                    Reg[Rdest] = 1;
                }
                else
                {
                    Reg[Rdest] = 0;
                }
            }

            else if (upCode == (byte)OpCodes.JMP)
            {
                Reg[IP] = R2;
            }
            else if (upCode == (byte)OpCodes.JMPT)
            {
                //int hi = ((byte)OpCodes.JMP + 00 + 00 + R1);

                if (Reg[Rdest] == 1)
                {
                    Reg[IP] = R2;
                }
            }

            else if (upCode == (byte)OpCodes.SET)
            {
                Reg[R1] = R2;
            }
        }

        static void Loop(int interations)
        {
        restartLoop:

            interations--;

            if (interations > 0)
            {
                goto restartLoop;
            }

            return;
        }

        /*AssemblyCode

        0x00|FF|00|00
        EX
               (00)
        0x40|00|FF|00 <- SET R0 0;
               (00)
        0x40|01|FF|01 <- SET R1 1;

        0x10|00|01|00 <- ADD R0 R1 R0;
        0x30|FF|00|02 <- JMP LOOP;
      Jump to line 2^

        //infinite counter
        
        SET r1 0
        SET r2 1
        setItBack:    
            ADD r1 r2 r1
            goTo setItBack

        */

        static unsafe void LoopThrough(int[] array, int start, int end)
        {
            int index = 0;
            fixed (int* ptr = array)
            {
                while (index < end - start)
                {
                    int* copy = ptr + start + index;
                    index++;
                    Console.WriteLine(*copy);
                }
            }
        }

        //*(ptr + 1) is the item itself
        //*item is the value of the pointer, int* is declaring the pointer

        static unsafe void BubbleSort(int[] array, int start = 0, int end = 0)
        {
            if (end == 0) end = array.Length;

            int bucket = 0;
            int index = 0;

            int* ptrA;
            int* ptrB;

            fixed (int* ptr = array)
            {
                ptrA = ptr;
                ptrB = ptr + 1;
                while (index < end - start - 1)
                {
                    bucket = *ptrA;

                    if (*ptrA > *ptrA)
                    {
                        *ptrA = *ptrA;
                        *ptrA = bucket;
                    }

                    ptrA = ptrA++;
                    ptrB = ptrB++;
                    index++;
                }
            }
        }

        static int Subtraction(int a, int b)
        {
            b = ~b;
            b += 1;

            return a + b;
        }

        class LinkedListNode
        {
            public int Value { get; set; } //Either zero or one
            public LinkedListNode Next { get; set; }

            public LinkedListNode(int value, LinkedListNode next = null)
            {
                Value = value;
                Next = next;
            }
        }

        static LinkedListNode Add(LinkedListNode head1, LinkedListNode head2)
        {
            LinkedListNode newListHead = new LinkedListNode(-1);
            LinkedListNode movinOut = newListHead;

            bool carryOne = false;

            while (!(head1 == null && head2 == null))
            {
                int currentValue;
                if (head2 == null)
                {
                    currentValue = head1.Value;
                }
                else if (head1 == null)
                {
                    currentValue = head2.Value;
                }
                else
                {
                    currentValue = head1.Value + head2.Value;
                }

                if (carryOne)
                {
                    currentValue++;
                }

                if (currentValue > 1)
                {
                    carryOne = true;
                    currentValue = 0;
                }

                movinOut.Value = currentValue;

                if (head1 != null) head1 = head1.Next;
                if (head2 != null) head2 = head2.Next;
                movinOut.Next = new LinkedListNode(-1);
                movinOut = movinOut.Next;
            }

            if (carryOne == true)
            {
                movinOut.Value = 1;
            }

            return newListHead;
        }
        //010101
        //111

        static string[] matches = new string[3] { "(ADD|SUB|MUL|DIV|MOD) (R([3][0-1]|[1-2][0-9]|[0-9]) ){3}", "", "" };
        //replace this guy ^

        //Match match = Regex.Match(input: "ADD R0 R1 R2", pattern: "(ADD|SUB|MUL|DIV|MOD) (R([3][0-1]|[1-2][0-9]|[0-9]) ){3}");

        /*
        uint eh(string assebmly)
        {
            cheating = 0;
            string RDest = "";
            string R1 = "";
            string R2 = "";

            byte instruc = 0;
            byte byteDest = 0;
            byte byteOne = 0; //padding
            byte byteTwo = 0;

            string instruction = seperateViaSpace(assebmly, 0);
            RDest = seperateViaSpace(assebmly, cheating);
            R1 = seperateViaSpace(assebmly, cheating);
            if (cheating != assebmly[assebmly.Length-1])
            {
                R2 = seperateViaSpace(assebmly, cheating);
            }

            if(R2 == "")
            {
                //padding
                byteOne = 0xFF;
            }

            if (instruction == "ADD")
            {
                instruc = 0x10;

            }
            else if (instruction == "SUB")
            {
                instruc = 0x11;
            }
            else if (instruction == "SET")
            {
                instruc = 0x40;
            }

            //return 0x10030102

            return 1;
        }
        */
        static uint assemble(string instruction)
        {
            string[] parts = instruction.Split(' ');
            string instruct = parts[0];
            string firstRegister = parts[1];
            byte firstRegisterIndex = byte.Parse(firstRegister.Substring(1));

            byte opCode = 0;

            if (instruct == "SET")
            {
                opCode = 0x40;

                ushort data = ushort.Parse(parts[2]);

                //string value = parts[2];
                //ushort data = ushort.Parse(value);

                //check if the short is a byte of a short (i.e its bigger than a byte, put some to the padding)
                return (uint)((opCode << 24) | (firstRegisterIndex << 16) | data);
            }
            else if (instruct == "ADD")
            {
                opCode = 0x10;

                string secRegister = parts[2];
                byte secRegisterIndex = byte.Parse(firstRegister.Substring(1));

                string thirdRegister = parts[3];
                byte thirdRegisterIndex = byte.Parse(firstRegister.Substring(1));

                // return (uint)(());
            }

            return 0;
        }

        //Dictionary ;

        static uint eh(string instruction)
        {
            Match match = null;

            foreach (var strMatch in matches)
            {
                match = Regex.Match(instruction, strMatch);
                if (match.Success)
                {
                    break;
                }
            }

            if (!match.Success)
            {
                throw new Exception("Syntax Error nerd");
            }

            string name = match.Groups[1].Value;
            byte opCode;
            byte firstRegisterIndex = byte.Parse(match.Groups[2].Value);

            if (name == "ADD")
            {
                opCode = 0x10;

                byte secondRegisterIndex = byte.Parse(match.Groups[3].Value);
                byte thirdRegisterIndex = byte.Parse(match.Groups[4].Value);

                return (uint)((opCode << 24) | (firstRegisterIndex << 16) | (secondRegisterIndex << 8) | thirdRegisterIndex);
            }
            if (name == "SUB")
            {
                opCode = 0x11;


            }
            if (name == "SET")
            {
                opCode = 0x40;

                //short data = 
            }

            return 0;
        }

        string[] LoadAssemblyProgram()
        {
            return new string[]
            {
                //placeholder
                "SET R0 5",
                "SET R1 10",
                "ADD R0 R1 R2",
            };
        }

        string seperateViaSpace(string fullWord, int startingIndex)
        {
            string instruction = "";
            int counter = startingIndex;

            while (counter != fullWord.Length - 1 && fullWord[counter] != ' ')
            {
                instruction = fullWord[counter] + instruction;
                counter++;
            }
            return instruction;
        }

        #endregion


        public abstract class Animal
        {

            public Animal()
            {
                Type type = this.GetType();



                MethodInfo[] methods = type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.GetProperty);


                MethodInfo[] methods2 = methods.SkipWhile();
                //int count = 0;
                //foreach (var method in methods)
                //{
                //    if (method.Attributes != MethodAttributes.SpecialName)
                //    {
                //        count++;
                //    }
                //}

                if (methods.Length < 3)
                {
                    throw new Exception("Don't bring me down");
                }
                //type.getMethod
            }
        }

        public class Cat : Animal
        {
            public int Age => 4;
            public int Legs => 4;



            public int ELO => 6;

            private void Meow()
            {
                Console.WriteLine("hello");
            }
        }

        public class Dog : Animal
        {

        }

        static void Main(string[] args)
        {
            Cat cat = new Cat();//Fail at run time if the cat does not have atleast two public methods

            ;

            string test = "ADD R0 R1 R2";
            string test2 = "SUB R1 R2 R3";

            ADD e = (ADD)Instruction.Parse(test);
            SUB a = (SUB)Instruction.Parse(test2);
            ;




            /*

            Random gen = new Random();

            //int[] test = new int[7] { 5, 7, 11, 34, 17, 2112, 23 };

            //LoopThrough(test, 2, 5);

            //Console.WriteLine(Subtraction(9, 4));

            LinkedListNode abba = new LinkedListNode(0, new LinkedListNode(1, new LinkedListNode(0, new LinkedListNode(1, new LinkedListNode(0, new LinkedListNode(1))))));
            LinkedListNode baab = new LinkedListNode(1, new LinkedListNode(1, new LinkedListNode(1)));

            //LinkedListNode abba = new LinkedListNode(1, new LinkedListNode(0));
            //LinkedListNode baab = new LinkedListNode(1, new LinkedListNode(0));

            LinkedListNode cbbc = Add(abba, baab);
            ;
            Reg[IP] = 0;

            //R0 = 0, R1 = 1 (counter), R2 = 5, R3 = gte result

            Reg[R0] = 0;
            Reg[R1] = 0;
            Reg[R2] = 0;
            Reg[R3] = 0;

            RAM[0] = 0x40FF0000; //0
            RAM[1] = 0x40FF0101; //1 counter
            RAM[2] = 0x40FF0205; //till _

            RAM[4] = 0x24030002; //check
            RAM[3] = 0x10000001; //add
            RAM[5] = 0x31030003; //loop back

            for (; Reg[IP] < RAM.Length; Reg[IP]++)
            {
                DoWork(RAM[Reg[IP]]);

                if (Reg[R0] == 5)
                {
                    Console.WriteLine("5");
                }
                else if (Reg[R0] == 6)
                {
                    Console.WriteLine("Failed, gt 5");
                }
            }

            //Loop through and call do work on each instruction

            //Console.WriteLine(PullOutNthByte(1234567, 0));
            //Console.WriteLine(FlipBit(5, 0));
            //Console.WriteLine(Calculate(0b_0000_0101_0000_0101_0000_0001));
            //1 0010 1101 0110 1000 0111
            //0 = 1000 0111 OR 135
            */
        }
    }
}
