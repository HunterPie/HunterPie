using HunterPie.Core.Architecture;

namespace HunterPie.Core.Settings.Types;

public class Position : Bindable
{
    public double X
    {
        get;
        set => SetValue(ref field, value);
    }
    public double Y
    {
        get;
        set => SetValue(ref field, value);
    }

    public Position(double x, double y)
    {
        X = x;
        Y = y;
    }

    public bool Equals(Position other) => X == other.X && Y == other.Y;
}