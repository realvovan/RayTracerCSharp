namespace RayTracer;

class Interval {
    public static readonly Interval EMPTY = new(double.PositiveInfinity,double.NegativeInfinity);
    public static readonly Interval UNIVERSE = new(double.NegativeInfinity,double.PositiveInfinity);

    public double Min;
    public double Max;

    public Interval() {
        this.Min = double.NegativeInfinity;
        this.Max = double.PositiveInfinity;
    }
    public Interval(double min,double max) {
        this.Min = min;
        this.Max = max;
    }
    public double Size() => this.Max - this.Min;
    public bool Contains(double x) => this.Min <= x && x <= this.Max;
    public bool Surrounds(double x) => this.Min < x && x < this.Max;
    public double Clamp(double x) {
        if(x < this.Min) return this.Min;
        if(x > this.Max) return this.Max;
        return x;
    }
}