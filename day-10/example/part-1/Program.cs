using System.ComponentModel;

string[] lines = File.ReadAllLines("..\\..\\..\\..\\input.txt");

var answer = 1;
var sketch = new Pipe[lines[0].Length, lines.Length];
Pipe? startingPoint = null;
for (var y = 0; y < lines.Length; y++)
{
    for (var x = 0; x < lines[0].Length; x++)
    {
        var pipe = new Pipe(lines[y][x], x, y);
        sketch[x, y] = pipe;
        if (pipe.symbol.Equals('S'))
            startingPoint = pipe;
    }
}

if (startingPoint == null)
    throw new Exception("No starting point found");

// iteratively remove incorrectly connected pipes until we end up with the main loop only
var incorrectPipesFound = false;
do
{
    incorrectPipesFound = false;

    // find incorrectly connected pipes (but don't remove them yet as to not influence connection count on adjacent pipes)
    for (var y = 0; y < sketch.GetLength(1); y++)
    {
        for (var x = 0; x < sketch.GetLength(0); x++)
        {
            if (sketch[x, y].symbol.Equals('S') || sketch[x, y].symbol.Equals('.'))
                continue;

            var connections = 0;
            if (y > 0 && sketch[x, y].north && sketch[x, y - 1].south)
                connections++;
            if (x > 0 && sketch[x, y].west && sketch[x - 1, y].east)
                connections++;
            if (y < sketch.GetLength(1) - 1 && sketch[x, y].south && sketch[x, y + 1].north)
                connections++;
            if (x < sketch.GetLength(0) - 1 && sketch[x, y].east && sketch[x + 1, y].west)
                connections++;

            if (connections != 2)
            {
                sketch[x, y].ScheduleToRemove();
                incorrectPipesFound = true;
            }
        }
    }
    // remove all unconnected pipes marked as scheduled to remove
    for (var y = 0; y < sketch.GetLength(1); y++)
        for (var x = 0; x < sketch.GetLength(0); x++)
            sketch[x, y].CleanUp();

} while (incorrectPipesFound);

// now we can start following the paths
var paths = new Position[] {
    new Position(startingPoint.x, startingPoint.y),
    new Position(startingPoint.x, startingPoint.y)
};

// start paths in a different direction each
var firstPathFound = false;
if (startingPoint.y > 0 && sketch[startingPoint.x, startingPoint.y - 1].south)
{
    paths[0].y--;
    firstPathFound = true;
}
if (startingPoint.x < sketch.GetLength(0) - 1 && sketch[startingPoint.x + 1, startingPoint.y].west)
{
    var index = 0;
    if (firstPathFound)
        index++;
    else
        firstPathFound = true;
    paths[index].x++;
}
if (startingPoint.y < sketch.GetLength(1) - 1 && sketch[startingPoint.x, startingPoint.y + 1].north)
{
    var index = 0;
    if (firstPathFound)
        index++;
    paths[index].y++;
}
if (startingPoint.x > 0 && sketch[startingPoint.x - 1, startingPoint.y].east)
{
    paths[1].x--;
}

// follow each path until we're on the same position
// this loop steps in an 'open' direction and closes it right away so it can't walk back in the wrong direction
// also we should check if we don't walk towards the starting point again, as both paths should move away from that
while (true)
{
    for (var i = 0; i < paths.Length; i++)
    {
        if (sketch[paths[i].x, paths[i].y].north && !sketch[paths[i].x, paths[i].y - 1].symbol.Equals('S'))
        {
            paths[i].y--;
            sketch[paths[i].x, paths[i].y].south = false;
        }
        else if (sketch[paths[i].x, paths[i].y].east && !sketch[paths[i].x + 1, paths[i].y].symbol.Equals('S'))
        {
            paths[i].x++;
            sketch[paths[i].x, paths[i].y].west = false;
        }
        else if (sketch[paths[i].x, paths[i].y].south && !sketch[paths[i].x, paths[i].y + 1].symbol.Equals('S'))
        {
            paths[i].y++;
            sketch[paths[i].x, paths[i].y].north = false;
        }
        else if (sketch[paths[i].x, paths[i].y].west && !sketch[paths[i].x - 1, paths[i].y].symbol.Equals('S'))
        {
            paths[i].x--;
            sketch[paths[i].x, paths[i].y].east = false;
        }
    }

    answer++;

    if (paths[0].x == paths[1].x && paths[0].y == paths[1].y)
        break;
}

Console.WriteLine(answer);

class Position
{
    public int x;
    public int y;

    public Position(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

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

    public void ScheduleToRemove()
    {
        scheduleToRemove = true;
    }

    public void CleanUp()
    {
        if (scheduleToRemove)
        {
            symbol = '.';
            north = false;
            east = false;
            south = false;
            west = false;
        }
    }

    private void SetConnections() {
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
