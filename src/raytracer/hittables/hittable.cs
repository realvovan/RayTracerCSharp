namespace RayTracer;

class HitRecord {
    public Vector3 P;
    public Vector3 Normal;
    public Material Material;
    public double T;
    public bool FrontFace;

    public void SetFaceNormal(Ray r, Vector3 outwardNoraml) {
        this.FrontFace = Vector3.Dot(r.Direction, outwardNoraml) < 0;
        this.Normal = this.FrontFace ? outwardNoraml : -outwardNoraml;
    }
}

abstract class Hittable {
    public abstract bool Hit(Ray r, Interval rayT, ref HitRecord rec);
}