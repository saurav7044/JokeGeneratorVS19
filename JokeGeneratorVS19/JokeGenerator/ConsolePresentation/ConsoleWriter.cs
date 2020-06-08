using System;

namespace JokeGenerator.ConsolePresentation
{
    internal sealed class ConsoleWriter
    {

        internal void WriteLine(string key, ConsoleColor? color = null, bool chooseFlag = false, params object[] args)
        {
            var result =
                chooseFlag
                    ? string.Format(key, args).Insert(0, "")
                    : string.Format(key, args);
            WriteLine(result, color);
        }

        private void WriteLine(string text = null, ConsoleColor? color = null)
        {
            if (color.HasValue)
            {
                Console.ForegroundColor = color.Value;
                if (text == null)
                    Console.WriteLine();
                else
                    Console.WriteLine(text, color.Value);

                Console.ResetColor();
            }
            else
            {
                if (text == null)
                    Console.WriteLine();
                else
                    Console.WriteLine(text);
            }
        }
    }
}