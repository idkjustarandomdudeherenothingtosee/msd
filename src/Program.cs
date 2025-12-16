using System;
using System.IO;
using MoonsecDeobfuscator.Deobfuscation;
using MoonsecDeobfuscator.Deobfuscation.Bytecode;

namespace MoonsecDeobfuscator
{
    public static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 5 || args[1] != "-i" || args[3] != "-o")
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("Devirtualize and dump bytecode to file:\n\t-dev -i <input> -o <output>");
                Console.WriteLine("Devirtualize and dump bytecode disassembly to file:\n\t-dis -i <input> -o <output>");
                return;
            }

            string command = args[0];
            string input = args[2];
            string output = args[4];

            if (!File.Exists(input))
            {
                Console.WriteLine("Invalid input path!");
                return;
            }

            Deobfuscator deob = new Deobfuscator();
            string source = File.ReadAllText(input);

            if (command == "-dev")
            {
                BytecodeChunk result = deob.Deobfuscate(source);

                FileStream stream = new FileStream(output, FileMode.Create, FileAccess.Write);
                Serializer serializer = new Serializer(stream);

                serializer.Serialize(result);

                serializer.Dispose();
                stream.Dispose();
            }
            else if (command == "-dis")
            {
                BytecodeChunk result = deob.Deobfuscate(source);
                Disassembler dis = new Disassembler(result);

                File.WriteAllText(output, dis.Disassemble());
            }
            else
            {
                Console.WriteLine("Invalid command!");
            }
        }
    }
}
