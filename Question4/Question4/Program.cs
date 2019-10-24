using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question4
{
    class Program
    {
        static void Main(string[] args)
        {
            const int n = 59;
            int[] arrPrimeNum = new int[n];

            int status = 1;
            int num = 3;
            int sum_of_prime = 0;

            Console.WriteLine("First " + n + " prime numbers are:");
            Console.WriteLine(2);

            for (int k = 2; k <= n;)
            {
                for (int i = 2; i <= Math.Sqrt(num); i++)
                {
                    if (num % i == 0)
                    {
                        status = 0;
                        break;
                    }
                }
                if (status != 0)
                {
                    Console.WriteLine(num);
                    arrPrimeNum[k - 2] = k;
                    sum_of_prime += k;
                    k++;
                }
                status = 1;
                num++;
            }

            Console.WriteLine("The sum of the first " + n + " prime numbers = " + sum_of_prime);

            Console.Read();
        }
    }
}
