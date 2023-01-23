using System;
using System.Numerics;
using Easel;
using Easel.Entities;
using Easel.Entities.Components;
using Easel.Graphics;
using Easel.Graphics.Materials;
using Easel.Scenes;
using Pie.Windowing;

namespace EaselDemo.Engine;

public class MyScene : Scene
{
    protected override void Initialize()
    {
        base.Initialize();

        Bitmap right = Content.Load<Bitmap>("right");
        Bitmap left = Content.Load<Bitmap>("left");
        Bitmap top = Content.Load<Bitmap>("top");
        Bitmap bottom = Content.Load<Bitmap>("bottom");
        Bitmap front = Content.Load<Bitmap>("front");
        Bitmap back = Content.Load<Bitmap>("back");

        Camera.Main.Skybox = new Skybox(right, left, top, bottom, front, back);
        Camera.Main.Transform = new Transform()
        {
            Position = new Vector3(-2.2681346f, 1.3556243f, -0.6261671f),
            Rotation = new Quaternion(-0.12515905f, -0.799447f, -0.17877193f, 0.55969656f)
        };
        
        Mesh mesh = Content.Load<Mesh[]>("Cube")[0];

        Texture2D albedo = Content.Load<Texture2D>("metalgrid2_basecolor");
        Texture2D normal = Content.Load<Texture2D>("metalgrid2_normal-dx");
        Texture2D metallic = Content.Load<Texture2D>("metalgrid2_metallic");
        Texture2D roughness = Content.Load<Texture2D>("metalgrid2_roughness");
        Texture2D ao = Content.Load<Texture2D>("metalgrid2_AO");

        MaterialMesh mMesh = new MaterialMesh(mesh, new StandardMaterial(albedo, normal, metallic, roughness, ao));
        
        Entity cube = new Entity(new Transform()
        {
            Scale = new Vector3(5, 1, 5)
        });
        cube.AddComponent(new MeshRenderer(mMesh));
        AddEntity("cube", cube);
    }

    protected override void Update()
    {
        base.Update();

        if (Input.KeyDown(Key.Escape))
            Game.Close();

        GetEntity("cube").Transform.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, 0.1f * Time.DeltaTime);
    }
}