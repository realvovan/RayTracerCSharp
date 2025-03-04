namespace RayTracer;

class Color {
    static readonly Interval intensity = new Interval(0.000,0.999);
    double[] colors;
    public double R => this.colors[0];
    public double G => this.colors[1];
    public double B => this.colors[2];

    public Color() => this.colors = [0,0,0];
    public Color(double r, double g, double b) => this.colors = [r,g,b];

    public static void WriteColor(Color color) {
        var r = LinearToGamma(color.colors[0]);
        var g = LinearToGamma(color.colors[1]);
        var b = LinearToGamma(color.colors[2]);
        int rbyte = (int)(255.999 * intensity.Clamp(r));
        int gbyte = (int)(255.999 * intensity.Clamp(g));
        int bbyte = (int)(255.999 * intensity.Clamp(b));
        Console.Write($"{rbyte} {gbyte} {bbyte}\n");
    }
    public static double LinearToGamma(double linearComponent) => linearComponent > 0 ? Math.Sqrt(linearComponent) : 0;
    public static Color FromVector(Vector3 vec) => new Color(vec.X,vec.Y,vec.Z);
    public static Color operator *(double scalar, Color c) => new Color(
        c.colors[0] * scalar,
        c.colors[1] * scalar,
        c.colors[2] * scalar
    );
    public static Color operator *(Color c1, Color c2) => new Color(
        c1.colors[0] * c2.colors[0],
        c1.colors[1] * c2.colors[1],
        c1.colors[2] * c2.colors[2]
    );
    public static Color operator +(Color c1, Color c2) => new Color(
        c1.colors[0] + c2.colors[0],
        c1.colors[1] + c2.colors[1],
        c1.colors[2] + c2.colors[2]
    );
}