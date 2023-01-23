using System;
using System.Diagnostics;
using System.Numerics;
using Easel.Graphics;
using Easel.Graphics.Lighting;
using Easel.Graphics.Materials;
using Easel.Graphics.Renderers;
using Easel.Graphics.Renderers.Structs;
using Easel.Math;
using Pie;
using Pie.Windowing;

namespace EaselDemo.Graphics;

public class MyApp : IDisposable
{
    private WindowSettings _settings;
    
    private Window _window;
    private EaselGraphics _graphics;

    private Renderable _renderable;
    private Quaternion _rotation;

    public MyApp(WindowSettings settings)
    {
        _settings = settings;
    }

    public void Initialize()
    {
        Mesh mesh = Mesh.FromFile("Content/Cube.obj")[0];

        Texture2D albedo = new Texture2D("Content/metalgrid2_basecolor.png");
        Texture2D normal = new Texture2D("Content/metalgrid2_normal-dx.png");
        Texture2D metallic = new Texture2D("Content/metalgrid2_metallic.png");
        Texture2D roughness = new Texture2D("Content/metalgrid2_roughness.png");
        Texture2D ao = new Texture2D("Content/metalgrid2_AO.png");

        MaterialMesh mMesh = new MaterialMesh(mesh, new StandardMaterial(albedo, normal, metallic, roughness, ao));

        _renderable = Renderable.CreateFromMesh(mMesh);

        Matrix4x4 projection = Matrix4x4.CreatePerspectiveFieldOfView(EaselMath.ToRadians(70),
            _window.Size.Width / (float) _window.Size.Height, 0.1f, 1000f);

        Vector3 camPos = new Vector3(-2.2681346f, 1.3556243f, -0.6261671f);
        Quaternion camRot = new Quaternion(-0.12515905f, -0.799447f, -0.17877193f, 0.55969656f);
        Vector3 forward = Vector3.Transform(-Vector3.UnitZ, camRot);
        Vector3 up = Vector3.Transform(Vector3.UnitY, camRot);

        Matrix4x4 view = Matrix4x4.CreateLookAt(camPos, camPos + forward, up);
        
        Bitmap right = new Bitmap("Content/right.jpg");
        Bitmap left = new Bitmap("Content/left.jpg");
        Bitmap top = new Bitmap("Content/top.jpg");
        Bitmap bottom = new Bitmap("Content/bottom.jpg");
        Bitmap front = new Bitmap("Content/front.jpg");
        Bitmap back = new Bitmap("Content/back.jpg");

        Skybox skybox = new Skybox(right, left, top, bottom, front, back);

        _graphics.Renderer.Camera = new CameraInfo(projection, view)
        {
            ClearColor = Color.Black,
            Skybox = skybox,
            Position = camPos
        };
        
        _graphics.Renderer.DirectionalLight = new DirectionalLight(new Vector2(EaselMath.ToRadians(0), EaselMath.ToRadians(75)), Color.White);
        
        _rotation = Quaternion.Identity;
    }

    public void Update(float dt, InputState state)
    {
        if (state.IsKeyDown(Key.Escape))
            _window.ShouldClose = true;

        _rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, 0.1f * dt);
    }

    public void Draw(float dt)
    {
        _graphics.Renderer.AddOpaque(_renderable,
            Matrix4x4.CreateFromQuaternion(_rotation) * Matrix4x4.CreateScale(5, 1, 5));
        
        _graphics.Renderer.Perform3DPass();
    }

    public void Run()
    {
        _window = Window.CreateWithGraphicsDevice(_settings, out GraphicsDevice device);
        _graphics = new EaselGraphics(device, new RenderOptions());

        Initialize();
        
        Stopwatch sw = Stopwatch.StartNew();

        while (!_window.ShouldClose)
        {
            InputState state = _window.ProcessEvents();

            _graphics.Renderer.NewFrame();

            float dt = (float) sw.Elapsed.TotalSeconds;
            sw.Restart();

            Update(dt, state);
            Draw(dt);
            
            _graphics.Renderer.DoneFrame();
            
            _graphics.Present();
        }
    }

    public void Dispose()
    {
        _graphics.Dispose();
        _window.Dispose();
    }
}