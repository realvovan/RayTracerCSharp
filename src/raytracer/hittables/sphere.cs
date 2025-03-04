namespace RayTracer;

class Sphere : Hittable {
    Vector3 cetner;
    Material material;
    double radius;

    public Sphere(Vector3 center, double radius, Material material) {
        this.cetner = center;
        this.radius = Math.Max(0,radius);
        this.material = material;
    }

    public override bool Hit(Ray r, Interval rayT, ref HitRecord rec) {
        Vector3 oc = this.cetner - r.Origin;
        var a = r.Direction.LengthSquared();
        var h = Vector3.Dot(r.Direction,oc);
        var c = oc.LengthSquared() - radius*radius;
        var discriminant = h*h - a*c;

        if(discriminant < 0) return false;
        
        var sqrtd = Math.Sqrt(discriminant);
        var root = (h - sqrtd) / a;
        if(!rayT.Surrounds(root)) {
            root = (h + sqrtd) / a;
            if(!rayT.Surrounds(root)) return false;
        }
        rec.T = root;
        rec.P = r.At(rec.T);
        rec.SetFaceNormal(r,(rec.P - cetner) / radius);
        rec.Material = this.material;
        return true;
    }
}