using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question1
{
    class Program
    {
        static void Main(string[] args)
        {
            string givenSentence = "Please replace all characters equals to the letter “A” with an underscore (_)";
            string new_sentence = "";

            Console.WriteLine("The given sentence: \n" + givenSentence);


            new_sentence = givenSentence.Replace('a', '_');
            Console.WriteLine("\n\nUsing the given C# method of .replace()");
            Console.WriteLine("New sentence: \n" + new_sentence.Replace('A', '_'));


            Console.WriteLine("\n\nUsing a foreach loop with a if statement to check each character in the sentence \nNew sentence:");
            foreach (char c in givenSentence)
            {
                if (c.Equals('a') || c.Equals('A'))
                    Console.Write('_');
                else
                    Console.Write(c);
            }

            Console.Read();
        }
    }
}
