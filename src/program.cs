using RayTracer;

namespace Program;

class Program {
    static void Main() {
        HittableList world = new();

        var groundMaterial = new Lambertian(new Color(0.5,0.5,0.5));
        world.Add(new Sphere(new Vector3(0,-1000,0),1000,groundMaterial));

        for(int i = -6; i < 6; i++) {
            for(int j = -6; j < 6; j++) {
                var chooseMat = Utils.RandomDouble();
                Vector3 center = new(i + 0.9*Utils.RandomDouble(),0.2,j + 0.9*Utils.RandomDouble());

                if((center - new Vector3(4,0.2,0)).Length() > 0.9) {
                    if(chooseMat < 0.8) {
                        //diffuse
                        var albedo = Color.FromVector(Vector3.GetRandom()*Vector3.GetRandom());
                        var sphereMaterial = new Lambertian(albedo);
                        world.Add(new Sphere(center,0.2,sphereMaterial));
                    } else if(chooseMat < 0.95) {
                        //metal
                        var albedo = Color.FromVector(Vector3.GetRandom(0.5,1));
                        var fuzz = Utils.RandomDouble(0,0.5);
                        var sphereMaterial = new Metal(albedo,fuzz);
                        world.Add(new Sphere(center,0.2,sphereMaterial));
                    } else {
                        //glass
                        var sphereMaterial = new Dielectric(1.5);
                        world.Add(new Sphere(center,0.2,sphereMaterial));
                    }
                }
            }
        }

        var material1 = new Dielectric(1.5);
        world.Add(new Sphere(new Vector3(0,1,0),1,material1));

        var material2 = new Lambertian(new Color(0.4,0.2,0.1));
        world.Add(new Sphere(new Vector3(-4,1,0),1,material2));

        var material3 = new Metal(new Color(0.7,0.6,0.5),0);
        world.Add(new Sphere(new Vector3(4,1,0),1,material3));

        Camera cam = new() {
            AspectRatio = 16.0 / 9.0,
            ImageWidth = 1200,
            SamplesPerPixel = 500,
            MaxDepth = 50,
            vFOV = 20,
            LookFrom = new Vector3(13,2,3),
            LookAt = new Vector3(0,0,0),
            vUp = new Vector3(0,1,0),
            DefocusAngle = 0.6,
            FocusDistance = 10
        };
        cam.Render(world);
    }
}