/*
 * fileOut.cs
 *
 * a simple source file demonstrating file output
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace hello
{
	class Program
	{
		static int Main(string[] args)
		{
			FileStream fp;

			fp = File.OpenWrite("testOutput.txt");
			if (fp == null ) 
			{
				System.Console.WriteLine("**Error: Unable to open file : testOutput.txt for writing");
				return 1;
			}

			fp.Write(Encoding.ASCII.GetBytes("hello world\n"), 0, Encoding.ASCII.GetBytes("hello world\n").Length);
			fp.Close();

			System.Console.WriteLine("File testOutput.txt has been created");

			return 0;
		}
	}
}
