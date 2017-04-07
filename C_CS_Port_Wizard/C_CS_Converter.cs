/**
* \file Filename.extention
* \short Class-ProjectName
* \author Name, Name, Name
* \date YYYY-MM-DD
* \brief Description
*/




using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace C_CS_Port_Wizard {
    static class C_CS_Converter {

        /**
* \class ClassName
* \breif <b>Description :</b> Description
* \author Name
*/

        /**
        * \brief <b>Convert</b> - 
        * \details 
        * \param ref string code - string containing C code that will be converted to C# code
        * \return nothing
        */
        public static bool Convert(ref string code) {

            ConvertExtention(ref code);
            ConvertInclude(ref code);
            ConvertFopen(ref code);
            ConvertMain(ref code);
            ConvertPrintf(ref code);
            ConvertCharArray(ref code);
            ConvertGets(ref code);
            ConvertAtoi(ref code);
            ConvertFile(ref code);
            ConvertFgets(ref code);
            ConvertNULL(ref code);
            ConvertStrlen(ref code);
            ConvertFclose(ref code);
            ConvertFprintf(ref code);

            return true;
        }


        /**
        * \brief <b>ConvertExtention</b> - converts .c to .cs
        * \details changes any occurrences of the c file extention to the c# extention
        * \param ref string code - string containing C code that will be converted to C# code
        * \return nothing
        */
        static void ConvertExtention(ref string code) {

            code = Regex.Replace(code, @"\.c", ".cs");
        }



        /**
        * \brief <b>ConvertInclude</b> - changes c's include to c#'s using
        * \details converts the standard stdio.h include to the standard c# using statements
        *           stdlib.h and string.h are removed as their c# replacements don't require using statements
        * \param ref string code - string containing C code that will be converted to C# code
        * \return nothing
        */
        static void ConvertInclude(ref string code) {

            code = Regex.Replace(code, @"#include <stdio.h>", "using System;\r\nusing System.Collections.Generic;\r\nusing System.IO;\r\nusing System.Text;");
            code = Regex.Replace(code, @"\r\n#include <stdlib.h>", "");
            code = Regex.Replace(code, @"\r\n#include <string.h>", "");
        }



        /**
        * \brief <b>ConvertMain</b> - converts a C main to a C# main
        * \details replaces main with C# equivalent and places main inside a class and namespace required by C#
        *           also fixes indentation
        * \param ref string code - string containing C code that will be converted to C# code
        * \return nothing
        */
        static void ConvertMain(ref string code) {

            code = Regex.Replace(code, @"\r\n\t", "\r\n\t\t\t");
            code = Regex.Replace(code, @"int main \(void\)\r\n{", "namespace hello\r\n{\r\n\tclass Program\r\n\t{\r\n\t\tstatic int Main(string[] args)\r\n\t\t{");
            
            Regex rgx = new Regex(@"}", RegexOptions.RightToLeft);
            code = rgx.Replace(code, "\t\t}\r\n\t}\r\n}", 1);
        }



        /**
        * \brief <b>Convert</b> - 
        * \details 
        * \param ref string code - string containing C code that will be converted to C# code
        * \return nothing
        */
        static void ConvertPrintf(ref string code) {

            string[,] searchReplace = new string[2, 2] { { @"\\n", "WriteLine" }, 
                                                            { "", "Write" } };
            // convert printfs with \n first then other printfs
            for (int i = 0; i < 2; i++) {

                Match match = Regex.Match(code, @"(printf\s*\("")(.+)(" + searchReplace[i,0] + @""")(.*)(\);)");
                while (match.Success) {

                    string str = match.Groups[2].Value;
                    // find c format placeholders
                    Match stringVar = Regex.Match(match.Groups[2].Value, @"(\%)(\d*)([ds])");
                    for (int j = 0; stringVar.Success; j++) {
                        // replace c format placeholders with c# format placeholders
                        Regex regx = new Regex(@"\%\d*[ds]");
                        string replaceWith = string.Empty;
                        if (stringVar.Groups[2].Value.Length == 0) {

                            replaceWith = @"{" + j.ToString() + "}";
                        }
                        else {

                            replaceWith = @"{" + j.ToString() + "," + stringVar.Groups[2].Value + "}";
                        }
                        str = regx.Replace(str, replaceWith, 1);

                        stringVar = stringVar.NextMatch();
                    }

                    string parameters = match.Groups[4].Value;

                    if (parameters.Length > 3) {


                        string[] para = parameters.Split(',');
                        for (int j = 0; j < para.Length; ++j) {
                            if (para[j].Length > 0) {
                                string st = (@"char\s*" + para[j].Substring(1) + @"\[\d+\];");
                                Match m = Regex.Match(code, st);
                                if (m.Success) {
                                    string s = para[j] + @"";
                                    Regex regx = new Regex(s);
                                    parameters = regx.Replace(parameters, @" new string(" + para[j] + @")", 1);
                                    
                                }
                            }
                        }
                    }




                    Regex rgx = new Regex(@"printf\s*\("".+" + searchReplace[i, 0] + @""".*\);");
                    code = rgx.Replace(code, @"System.Console." + searchReplace[i, 1] + @"(""" + str + @"""" + parameters + ");", 1);
                    match = match.NextMatch();
                }
            }
        }



        /**
        * \brief <b>Convert</b> - 
        * \details 
        * \param ref string code - string containing C code that will be converted to C# code
        * \return nothing
        */
        static void ConvertCharArray(ref string code) {
            
            Match match = Regex.Match(code, @"(char\s+)(\w+)(\[)(\d+)(\];)");
            while (match.Success) {
                Regex rgx = new Regex(@"char\s+.+\[\d+\];");
                code = rgx.Replace(code, @"char[] " + match.Groups[2].Value + " = new char[" + match.Groups[4].Value + "];", 1);
                match = match.NextMatch();
            }
        }



        /**
        * \brief <b>Convert</b> - 
        * \details 
        * \param ref string code - string containing C code that will be converted to C# code
        * \return nothing
        */
        static void ConvertGets(ref string code) {

            Match match = Regex.Match(code, @"(gets \()(.+)(\);)");
            while (match.Success) {
                Regex rgx = new Regex(@"gets \(.+\);");
                code = rgx.Replace(code, match.Groups[2].Value + @" = System.Console.ReadLine().ToCharArray();", 1);
                match = match.NextMatch();
            }
        }



        /**
        * \brief <b>Convert</b> - 
        * \details 
        * \param ref string code - string containing C code that will be converted to C# code
        * \return nothing
        */
        static void ConvertAtoi(ref string code) {

            Match match = Regex.Match(code, @"(\w+)( = atoi \()(\w+)(\);)");
            while (match.Success) {
                Regex rgx = new Regex(@"\w+ = atoi \(\w+\);");
                code = rgx.Replace(code, match.Groups[1].Value + @" = Int32.Parse(new string(" + match.Groups[3].Value + "));", 1);
                match = match.NextMatch();
            }
        }



        /**
        * \brief <b>Convert</b> - 
        * \details 
        * \param ref string code - string containing C code that will be converted to C# code
        * \return nothing
        */
        static void ConvertFile(ref string code) {

            code = Regex.Replace(code, @"FILE.*\*", "FileStream ");
        }



        /**
        * \brief <b>Convert</b> - 
        * \details 
        * \param ref string code - string containing C code that will be converted to C# code
        * \return nothing
        */
        static void ConvertFopen(ref string code) {

            Match match = Regex.Match(code, @"(\w+)(\s=\sfopen\s\()(""\w+\.\w+"")(\,\s"")(\w)(""\);)");
            while (match.Success) {

                Regex rgx = new Regex(@"\w+\s=\sfopen\s\(""\w+\.\w+""\,\s""\w""\);");
                if(match.Groups[5].Value == "r") {

                    code = rgx.Replace(code, match.Groups[1].Value + " = File.OpenRead(" + match.Groups[3].Value + ");\r\n\tStreamReader sr = new StreamReader(" + match.Groups[1].Value + ");", 1);
                }
                else if (match.Groups[5].Value == "w") {

                    code = rgx.Replace(code, match.Groups[1].Value + " = File.OpenWrite(" + match.Groups[3].Value + ");", 1);
                }

                match = match.NextMatch();
            }
            
        }



        /**
        * \brief <b>Convert</b> - 
        * \details 
        * \param ref string code - string containing C code that will be converted to C# code
        * \return nothing
        */
        static void ConvertFgets(ref string code) {

            Match match = Regex.Match(code, @"(fgets\()(\w+)(\,\s*)(\d+)(\,\s*)(\w+)(\))");
            while (match.Success) {

                Regex rgx = new Regex(@"fgets\(\w+\,\s*\d+\,\s*\w+\) != NULL");
                code = rgx.Replace(code, "sr.Read(" + match.Groups[2].Value + ", 0, " + match.Groups[4].Value + ") != 0", 1);

                // sr.Read(inBuff, 0, 100)
                match = match.NextMatch();
            }
        }



        /**
        * \brief <b>Convert</b> - 
        * \details 
        * \param ref string code - string containing C code that will be converted to C# code
        * \return nothing
        */
        static void ConvertNULL(ref string code) {

            code = Regex.Replace(code, @"NULL", "null ");
        }



        /**
        * \brief <b>Convert</b> - 
        * \details 
        * \param ref string code - string containing C code that will be converted to C# code
        * \return nothing
        */
        static void ConvertStrlen(ref string code) {

            Match match = Regex.Match(code, @"(strlen\()(\w+)(\))");
            while (match.Success) {

                Regex rgx = new Regex(@"strlen\(\w+\)");
                code = rgx.Replace(code, match.Groups[2].Value + ".Length", 1);
                match = match.NextMatch();
            }
        }



        /**
        * \brief <b>Convert</b> - 
        * \details 
        * \param ref string code - string containing C code that will be converted to C# code
        * \return nothing
        */
        static void ConvertFclose(ref string code) {

            Match match = Regex.Match(code, @"(fclose\s*\()(\w+)(\);)");
            while (match.Success) {

                Regex rgx = new Regex(@"fclose\s*\(\w+\);");
                code = rgx.Replace(code, match.Groups[2].Value + ".Close();", 1);
                match = match.NextMatch();
            }
        }



        /**
        * \brief <b>Convert</b> - 
        * \details 
        * \param ref string code - string containing C code that will be converted to C# code
        * \return nothing
        */
        static void ConvertFprintf(ref string code) {

            Match match = Regex.Match(code, @"(fprintf\s*\()(\w+)(\,\s*"")(.+)(""\);)");
            while (match.Success) {

                Regex rgx = new Regex(@"fprintf\s*\(\w+\,\s*"".+""\);");
                code = rgx.Replace(code, match.Groups[2].Value + @".Write(Encoding.ASCII.GetBytes(""" + match.Groups[4].Value + @"""), 0, Encoding.ASCII.GetBytes(""" + match.Groups[4].Value + @""").Length);", 1);
                match = match.NextMatch();
            }
        }
        
    }
}
