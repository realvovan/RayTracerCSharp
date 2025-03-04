namespace RayTracer;

class Vector3 {
    double[] e;
    //getters
    public double X => this.e[0];
    public double Y => this.e[1];
    public double Z => this.e[2];
    //constructors
    public Vector3() => this.e = [0d,0d,0d];
    public Vector3(double x, double y, double z) => this.e = [x,y,z];
    //methods
    public double LengthSquared() => this.e[0]*this.e[0] + this.e[1]*this.e[1] + this.e[2]*this.e[2];
    public double Length() => Math.Sqrt(this.LengthSquared());
    public bool IsNearZero() {
        var s = 1e-8;
        return (Math.Abs(this.e[0]) < s) && (Math.Abs(this.e[1]) < s) && (Math.Abs(this.e[2]) < s);
    }
    public override string ToString() => $"{this.e[0]} {this.e[1]} {this.e[2]}";
    //static methods
    public static double Dot(Vector3 u, Vector3 v) => u.e[0] * v.e[0] + u.e[1] * v.e[1] + u.e[2] * v.e[2];
    public static Vector3 Cross(Vector3 u, Vector3 v) => new Vector3(
        u.e[1] * v.e[2] - u.e[2] * v.e[1],
        u.e[2] * v.e[0] - u.e[0] * v.e[2],
        u.e[0] * v.e[1] - u.e[1] * v.e[0]
    );
    public static Vector3 GetRandom() => new Vector3(Utils.RandomDouble(),Utils.RandomDouble(),Utils.RandomDouble());
    public static Vector3 GetRandom(double min, double max) => new Vector3(Utils.RandomDouble(min,max),Utils.RandomDouble(min,max),Utils.RandomDouble(min,max));
    public static Vector3 GetRandomUnitVector() {
        while(true) {
            var p = Vector3.GetRandom(-1,1);
            var lensq = p.LengthSquared();
            if(1e-160 < lensq && lensq <= 1) return p/Math.Sqrt(lensq);
        }
    }
    public static Vector3 RandomOnHemisphere(Vector3 normal) {
        Vector3 onUnitSphere = Vector3.GetRandomUnitVector();
        if(Vector3.Dot(onUnitSphere,normal) > 0.0) return onUnitSphere;
        return -onUnitSphere;
    }
    public static Vector3 RandomInUnitDisk() {
        while(true) {
            var p = new Vector3(Utils.RandomDouble(-1,1),Utils.RandomDouble(-1,1),0);
            if(p.LengthSquared() < 1) return p;
        }
    }
    public static Vector3 ToUnit(Vector3 v) => v / v.Length();
    public static Vector3 Reflect(Vector3 v, Vector3 u) => v - 2*Vector3.Dot(v,u)*u;
    public static Vector3 Refract(Vector3 uv, Vector3 n, double etaiOverEtat) {
        var cosTheta = Math.Min(Vector3.Dot(-uv,n),1.0);
        Vector3 rOutPerp = etaiOverEtat * (uv + cosTheta*n);
        Vector3 rOutParallel = -Math.Sqrt(Math.Abs(1.0-rOutPerp.LengthSquared())) * n;
        return rOutPerp + rOutParallel;
    }
    //operator overloading
    public static Vector3 operator -(Vector3 v) => new Vector3(-v.e[0],-v.e[1],-v.e[2]);
    public static Vector3 operator +(Vector3 u,Vector3 v) => new Vector3(
        v.e[0] + u.e[0],
        v.e[1] + u.e[1],
        v.e[2] + u.e[2]
    );
    public static Vector3 operator -(Vector3 u, Vector3 v) => new Vector3(
        u.e[0] - v.e[0],
        u.e[1] - v.e[1],
        u.e[2] - v.e[2]
    );
    public static Vector3 operator *(Vector3 v, double scalar) => new Vector3(
        v.e[0] * scalar,
        v.e[1] * scalar,
        v.e[2] * scalar
    );
    public static Vector3 operator *(double scalar, Vector3 v) => v * scalar;
    public static Vector3 operator *(Vector3 u, Vector3 v) => new Vector3(u.e[0] * v.e[0], u.e[1] * v.e[1], u.e[2] * v.e[2]);
    public static Vector3 operator /(Vector3 v, double scalar) => (1/scalar) * v;
}