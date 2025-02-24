using System;

public class Vec
{
    public double x, y, z;

    // Constructors
    public Vec() => x = y = z = 0;
    public Vec(double x, double y, double z) { this.x = x; this.y = y; this.z = z; }

    // Approximate Comparison for two doubles
    public static bool Approx(double a, double b, double absTol = 1e-3, double relTol = 1e-3)
    {
        double absDiff = Math.Abs(a - b);
        if (absDiff < absTol) return true;

        double relDiff = Math.Abs(a - b) / (Math.Abs(a) + Math.Abs(b));
        if (relDiff < relTol) return true;

        return false;
    }

    // Approximate comparison for Vec objects
    public bool Approx(Vec other, double absTol = 1e-3, double relTol = 1e-3)
    {
        return Approx(this.x, other.x, absTol, relTol) &&
               Approx(this.y, other.y, absTol, relTol) &&
               Approx(this.z, other.z, absTol, relTol);
    }

    // Static Approximate Comparison for Vec objects
    public static bool Approx(Vec u, Vec v) => u.Approx(v);

    // Operator Overloads
    public static Vec operator +(Vec v1, Vec v2) => new Vec(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
    public static Vec operator -(Vec v1, Vec v2) => new Vec(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
    public static Vec operator *(Vec v, double scalar) => new Vec(v.x * scalar, v.y * scalar, v.z * scalar);
    public static Vec operator /(Vec v, double scalar) => new Vec(v.x / scalar, v.y / scalar, v.z / scalar);

    // Dot Product
    public double Dot(Vec other) => this.x * other.x + this.y * other.y + this.z * other.z;

    // Override ToString
    public override string ToString() => $"({x} {y} {z})";
}
