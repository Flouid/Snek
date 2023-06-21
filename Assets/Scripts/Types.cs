

public enum Direction
{
    North,
    East,
    South,
    West
}

public class GridPosition
{
    public int x, y;
    public Direction dir;

    public GridPosition(int x, int y, Direction dir = Direction.East)
    {
        this.x = x;
        this.y = y;
        this.dir = dir;
    }

    public GridPosition Behind()
    {
        switch (dir)
        {
            case (Direction.North): return new GridPosition(x, y - 1, dir);
            case (Direction.East): return new GridPosition(x - 1, y, dir);
            case (Direction.South): return new GridPosition(x, y + 1, dir);
            case (Direction.West): return new GridPosition(x + 1, y, dir);
        }
        return this;
    }

    public static bool InALine(GridPosition a, GridPosition b)
    {
        return (a.x == b.x || a.y == b.y);
    }
}
