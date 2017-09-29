using System;

[Serializable]
public class GridCoordinates
{
    public GridCoordinates(int _x, int _y)
    {
        X = _x;
        Y = _y;
    }

    public override bool Equals(System.Object obj)
    {
        if (obj == null)
        {
            return false;
        }

        // If parameter cannot be cast to GridCoordinates return false:
        GridCoordinates p = obj as GridCoordinates;
        if ((System.Object)p == null)
        {
            return false;
        }

        return (X == p.X) && (Y == p.Y);
    }

    public bool Equals(GridCoordinates p)
    {
        // If parameter is null return false:
        if ((object)p == null)
        {
            return false;
        }

        return (X == p.X) && (Y == p.Y);
    }

    public override int GetHashCode()
    {
        return X ^ Y;
    }

    public static bool operator ==(GridCoordinates _a, GridCoordinates _b)
    {
        // If both are null, or both are same instance, return true.
        if (System.Object.ReferenceEquals(_a, _b))
        {
            return true;
        }

        // If one is null, but not both, return false.
        if (((object)_a == null) || ((object)_b == null))
        {
            return false;
        }

        return (_a.X == _b.X) && (_a.Y == _b.Y);
    }

    public static bool operator !=(GridCoordinates _a, GridCoordinates _b)
    {
        return !(_a == _b);
    }

    public override string ToString()
    {
        return "(" + X + ", " + Y + ")";
    }

    public void SetCoords(int _x, int _y)
    {
        X = _x;
        Y = _y;
    }

    public GridCoordinates GetUp()
    {
        if (Up == null)
        {
            Up = new GridCoordinates(X, Y + 1);
        }
        return Up;
    }

    public GridCoordinates GetDown()
    {
        if (Down == null)
        {
            Down = new GridCoordinates(X, Y - 1);
        }
        return Down;
    }

    public GridCoordinates GetRight()
    {
        if (Right == null)
        {
            Right = new GridCoordinates(X + 1, Y);
        }
        return Right;
    }

    public GridCoordinates GetLeft()
    {
        if (Left == null)
        {
            Left = new GridCoordinates(X - 1, Y);
        }
        return Left;
    }

    public bool CheckValidCoords(float _width, float _height)
    {
        return ((X >= 0) && (X <= _width) && (Y >= 0) && (Y <= _height));
    }

    public int X;
    public int Y;

    private GridCoordinates Up = null;
    private GridCoordinates Down = null;
    private GridCoordinates Right = null;
    private GridCoordinates Left = null;
}
