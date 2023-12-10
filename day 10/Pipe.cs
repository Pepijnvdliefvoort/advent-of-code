using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day_10
{
    class Pipe
    {
        public char symbol;
        public int x;
        public int y;

        public bool north;
        public bool east;
        public bool south;
        public bool west;

        public bool scheduleToRemove;

        public Pipe(char symbol, int x, int y)
        {
            this.symbol = symbol;
            this.x = x;
            this.y = y;
            SetConnections();
        }

        private void SetConnections()
        {
            switch (symbol)
            {
                case 'S':
                    north = true;
                    east = true;
                    south = true;
                    west = true;
                    break;
                case 'F':
                    north = false;
                    east = true;
                    south = true;
                    west = false;
                    break;
                case '-':
                    north = false;
                    east = true;
                    south = false;
                    west = true;
                    break;
                case '7':
                    north = false;
                    east = false;
                    south = true;
                    west = true;
                    break;
                case '|':
                    north = true;
                    east = false;
                    south = true;
                    west = false;
                    break;
                case 'J':
                    north = true;
                    east = false;
                    south = false;
                    west = true;
                    break;
                case 'L':
                    north = true;
                    east = true;
                    south = false;
                    west = false;
                    break;
            }
        }
    }
}
