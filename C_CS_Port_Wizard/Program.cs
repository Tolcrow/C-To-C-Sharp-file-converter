using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace C_CS_Port_Wizard {
    class Program {
        static void Main(string[] args) {

            string inputFile = string.Empty;
            string outputFile = string.Empty;

            if (args.Length != 4) {

                Console.WriteLine("Wrong number of arguments");
            }
            else {

                for (int i = 0; i < args.Length; i++) {

                    if (args[i] == "-i") {

                        inputFile = args[i + 1];
                    }
                    if (args[i] == "-o") {

                        outputFile = args[i + 1];
                    }
                }

                string fileContents = string.Empty;

                if ((inputFile != string.Empty) && (outputFile != string.Empty)) {

                    try {
                        
                        fileContents = File.ReadAllText(inputFile);

                        C_CS_Converter.Convert(ref fileContents);

                        File.WriteAllText(outputFile, fileContents);
                    }
                    catch(Exception e) {
						
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }
    }
}
