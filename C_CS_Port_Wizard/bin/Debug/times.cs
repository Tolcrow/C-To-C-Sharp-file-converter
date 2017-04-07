/*
 * times.cs
 *
 * a simple file demonstrating console input/output
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
			// local declarations
			char[] buffer = new char[100];
			int table;
			int x;
			int product;

			// get the user input as to which times-table to generate ...
			System.Console.Write("Specify which TIMES-TABLE you would like to generate: ");
			buffer = System.Console.ReadLine().ToCharArray();
			table = Int32.Parse(new string(buffer));

			// now generate it !
			for (x = 1; x <= 10; x++) 
			{
				product = x * table;
				System.Console.WriteLine("{0,3} x {1,3} = {2,4}", x, table, product);
			}

			return 0;
		}
	}
}
