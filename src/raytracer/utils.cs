namespace RayTracer;

static class Utils {
    static Random rng = new();

    public const double Pi = 3.1415926535897932385;
    public static double ToRad(double degrees) => degrees * Pi / 180.0;
    public static double RandomDouble() => rng.NextDouble();
    public static double RandomDouble(double min,double max) => rng.NextDouble() * (max-min) + min;
}