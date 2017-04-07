/*
 * fileIn.cs
 *
 * a simple source file demonstrating file input
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
			// local variables
			int	fileLine = 1;
			FileStream fp;
			char[] inBuff = new char[100];

			// open the file
			fp = File.OpenRead("test.txt");
			StreamReader sr = new StreamReader(fp);
			if (fp == null ) 
			{
				System.Console.WriteLine("**Error: Unable to open file : test.txt for reading");
				return 1;
			}

			/* Walk through the file line by line counting and displaying
			   the file contents */
			System.Console.WriteLine("The following are the lines read from file \"test.txt\"");
			while (sr.Read(inBuff, 0, 100) != 0)
			{
				System.Console.Write("{0,2}) [Length : {1,3}] {2}", fileLine, inBuff.Length, new string( inBuff));
				fileLine++;
			}
			System.Console.WriteLine("<<< EOF");

			fp.Close();

			return 0;
		}
	}
}
