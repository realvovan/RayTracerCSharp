namespace RayTracer;

class HittableList : Hittable {
    public List<Hittable> Objects = [];

    public HittableList() {}
    public HittableList(Hittable obj) => this.Add(obj);

    public void Clear() => this.Objects.Clear();
    public void Add(Hittable obj) => this.Objects.Add(obj);

    public override bool Hit(Ray r, Interval rayT, ref HitRecord rec) {
        HitRecord temp = new();
        bool hitAnything = false;
        var closest = rayT.Max;

        foreach(var obj in this.Objects) {
            if(obj.Hit(r,new Interval(rayT.Min,closest),ref temp)) {
                hitAnything = true;
                closest = temp.T;
                rec = temp;
            }
        }
        return hitAnything;
    }
}