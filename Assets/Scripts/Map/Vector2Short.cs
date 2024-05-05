using System;

[Serializable]
public struct Vector2Short 
{
    public short x;
    public short y;

    public Vector2Short(short _x, short _y)
    {
        x = _x;
        y = _y;
    }

    public Vector2Short(int _x, int _y)
    {
        x = (short)_x;
        y = (short)_y;
    }

    public void SetPos(short _x, short _y)
    {
        x = _x;
        y = _y;
    }

    public bool IsValid => x >= 0 && y >= 0;
    public bool IsDefault => x == 0 && y == 0;

    public static Vector2Short operator +(Vector2Short sPos, Vector2Short tPos)
    {
        return new Vector2Short()
        {
            x = (short)(sPos.x + tPos.x),
            y = (short)(sPos.y + tPos.y)
        };
    }
    public static Vector2Short operator -(Vector2Short sPos, Vector2Short tPos)
    {
        return new Vector2Short()
        {
            x = (short)(sPos.x - tPos.x),
            y = (short)(sPos.y - tPos.y)
        };
    }

    public static Vector2Short operator *(Vector2Short sPos, short k)
    {
        sPos.x *= k;
        sPos.y *= k;
        return sPos;
    }
    public static bool operator >(Vector2Short sPos, Vector2Short tPos)
    {
        return sPos.x > tPos.x && sPos.y > tPos.y;
    }
    public static bool operator <(Vector2Short sPos, Vector2Short tPos)
    {
        return sPos.x < tPos.x && sPos.y < tPos.y;
    }
    public static bool operator >=(Vector2Short sPos, Vector2Short tPos)
    {
        return sPos.x >= tPos.x && sPos.y >= tPos.y;
    }
    public static bool operator <=(Vector2Short sPos, Vector2Short tPos)
    {
        return sPos.x <= tPos.x && sPos.y <= tPos.y;
    }
    public static bool operator ==(Vector2Short sPos, Vector2Short tPos)
    {
        return sPos.x == tPos.x && sPos.y == tPos.y;
    }
    public static bool operator !=(Vector2Short sPos, Vector2Short tPos)
    {
        return !(sPos == tPos);
    }

    public static float Distance(Vector2Short sPos, Vector2Short tPos)
    {
        return (float)Math.Sqrt(Math.Pow((sPos.x - tPos.x), 2) + Math.Pow((sPos.y - tPos.y), 2));
    }

    public override bool Equals(object obj)
    {
        return obj is Vector2Short @short &&
               x == @short.x &&
               y == @short.y;
    }

    public override int GetHashCode()
    {
        int hashCode = 1502939027;
        hashCode = hashCode * -1521134295 + x.GetHashCode();
        hashCode = hashCode * -1521134295 + y.GetHashCode();
        return hashCode;
    }


    public static Vector2Short GridPosZero()
    {
        return new Vector2Short(0, 0);
    }

    public override string ToString()
    {
        return $"({x}, {y})";
    }

    public static Vector2Short One => new Vector2Short(1, 1);
    public static Vector2Short Zero => new Vector2Short(0, 0);
    public static Vector2Short Negative => new Vector2Short(-1, -1);
    public static Vector2Short Hide => new Vector2Short(-99, -99);
}
