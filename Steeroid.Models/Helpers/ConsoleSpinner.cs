using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steeroid.Models.Helpers
{
    public sealed class ConsoleSpinner
    {
        private static Lazy<ConsoleSpinner> lazy =
            new Lazy<ConsoleSpinner>(() => new ConsoleSpinner());

        public static void Reset()
        {
            lazy = new Lazy<ConsoleSpinner>(() => new ConsoleSpinner());
        }

        public static ConsoleSpinner Instance { get { return lazy.Value; } }

        private int _consoleX;
        private int _consoleY;
        private readonly char[] _frames = { '|', '/', '-', '\\' };
        private int _current;

        int _delay;

        private ConsoleSpinner()
        {
            try
            {
                _current = 0;
                _consoleX = Console.CursorLeft;
                _consoleY = Console.CursorTop;
            }
            catch (Exception)
            {

                // throw;
            }

            _delay = 100;
        }

        public void Update()
        {

            _consoleX = 50;//Console.CursorLeft;
            _consoleY = Console.CursorTop;


            _delay += 100;
            if (_delay >= 300)
            {
                Console.SetCursorPosition(_consoleX, _consoleY);
                Console.Write(_frames[_current]);
                _delay = 100;

                if (++_current >= _frames.Length)
                    _current = 0;
            }


        }
    }
}
