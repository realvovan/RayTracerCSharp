namespace RayTracer;

class Camera {
    public double AspectRatio = 1.0;
    public int ImageWidth = 100;
    public int SamplesPerPixel = 10;
    public int MaxDepth = 10;
    public double vFOV = 90;
    public Vector3 LookFrom = new Vector3(0,0,0);
    public Vector3 LookAt = new Vector3(0,0,-1);
    public Vector3 vUp = new Vector3(0,1,0);
    public double DefocusAngle = 0;
    public double FocusDistance = 10;

    int imageHeight;
    double pixelSampleScale;
    Vector3 center;
    Vector3 pixel00Location;
    Vector3 pixelDeltaU;
    Vector3 pixelDeltaV;
    Vector3 u;
    Vector3 v;
    Vector3 w;
    Vector3 defocusDiskU;
    Vector3 defocusDiskV;

    public void Render(Hittable world) {
        this.init();
        Console.Write($"P3\n{this.ImageWidth} {this.imageHeight}\n255\n");
        for(int j = 0; j < this.imageHeight; j++) {
            Console.Error.Write($"\rScanlines remaining: {imageHeight - j} ");
            Console.Error.Flush();
            for(int i = 0; i < this.ImageWidth; i++) {
                Color pixelColor = new(0,0,0);
                for(int sample = 0; sample < this.SamplesPerPixel; sample++) {
                    Ray r = getRay(i,j);
                    pixelColor += rayColor(r,this.MaxDepth,world);
                }
                Color.WriteColor(this.pixelSampleScale * pixelColor);
            }
        }
        Console.Error.Write("\rDone.                 \n");
    }

    void init() {
        this.imageHeight = Math.Max(1,(int)(this.ImageWidth/this.AspectRatio));
        this.pixelSampleScale = 1.0/this.SamplesPerPixel;
        this.center = this.LookFrom;
        //determine viewport dimensions
        var viewportHeight = 2 * Math.Tan(Utils.ToRad(this.vFOV)/2) * this.FocusDistance;
        var viewportWidth = viewportHeight * ((double)this.ImageWidth/imageHeight);
        //calculate u,v,w unit basis vectors for the camera coordinate frame
        this.w = Vector3.ToUnit(this.LookFrom - this.LookAt);
        this.u = Vector3.ToUnit(Vector3.Cross(this.vUp,this.w));
        this.v = Vector3.Cross(this.w,this.u);
        //calculate the vectors across the horizontal and down the vertical viewport edges
        var viewport_u = viewportWidth * u;
        var viewport_v = viewportHeight * (-v);
        //calculate the horizontal and vertical delta vectors from pixel to pixel
        this.pixelDeltaU = viewport_u / this.ImageWidth;
        this.pixelDeltaV = viewport_v / this.imageHeight;
        //calculate the location of the upper left pixel
        var viewportUpperLeft = this.center - (this.FocusDistance * w) - viewport_u/2 - viewport_v/2;
        this.pixel00Location = viewportUpperLeft + 0.5 * (pixelDeltaU + pixelDeltaV);
        //calculate the camera defocus disk basis vectos
        var defocusRadius = this.FocusDistance * Math.Tan(Utils.ToRad(this.DefocusAngle/2));
        this.defocusDiskU = this.u * defocusRadius;
        this.defocusDiskV = this.v * defocusRadius;
    }

    Vector3 sampleSquare() => new Vector3(Utils.RandomDouble() - 0.5,Utils.RandomDouble() - 0.5,0);

    Ray getRay(int i,int j) {
        var offset = this.sampleSquare();
        var pixelSample = this.pixel00Location
            + ((i + offset.X) * this.pixelDeltaU)
            + ((j + offset.Y) * this.pixelDeltaV);
        var rayOrigin = (this.DefocusAngle <= 0) ? this.center : defocusDiskSample();
        var rayDir = pixelSample - rayOrigin;
        return new Ray(rayOrigin,rayDir);
    }

    Vector3 defocusDiskSample() {
        var p = Vector3.RandomInUnitDisk();
        return this.center + (p.X * this.defocusDiskU) + (p.Y * this.defocusDiskV);
    }

    Color rayColor(Ray r,int depth,Hittable world) {
        if(depth <= 0) return new Color(0,0,0);

        HitRecord rec = new();
        if(world.Hit(r,new Interval(0.001,double.PositiveInfinity),ref rec)) {
            Ray scattered = new();
            Color attenuation = new();
            if(rec.Material.Scatter(r,rec,ref attenuation,ref scattered))
                return attenuation * rayColor(scattered,depth-1,world);
            return new Color(0,0,0);
        }
        Vector3 unit = Vector3.ToUnit(r.Direction);
        var a = 0.5*(unit.Y + 1.0);
        return (1.0-a)*new Color(1,1,1) + a*new Color(0.5,0.7,1.0);
    }
}