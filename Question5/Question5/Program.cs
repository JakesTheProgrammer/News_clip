using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question5
{
    class Program
    {
        static void Main(string[] args)
        {
            long seq_num1 = 1;
            long seq_num2 = 1; 
            long seq_num3 = 0;

            Console.WriteLine("Fibonacci sequence = ");
            Console.Write(seq_num1 + " : " + seq_num2);

            for (int k = 1; k <= 48; k++)
            {
                seq_num3 = seq_num1 + seq_num2;
                seq_num1 = seq_num2;
                seq_num2 = seq_num3;

                Console.Write(" : " + seq_num3);
            }

            Console.Read();

        }
    }
}
