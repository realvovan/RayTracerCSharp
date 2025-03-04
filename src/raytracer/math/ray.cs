namespace RayTracer;

class Ray {
    public Vector3 Origin {get;}
    public Vector3 Direction {get;}

    public Ray() {
        this.Origin = new Vector3();
        this.Direction = new Vector3();
    }

    public Ray(Vector3 origin, Vector3 direction) {
        this.Origin = origin;
        this.Direction = direction;
    }

    public Vector3 At(double t) => this.Origin + t*this.Direction;
}