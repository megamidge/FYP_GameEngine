using engine;
using engine.Components;
using engine.Managers;
using engine.Systems;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
namespace engine
{
    class DefaultScene : Scene
    {
        private EntityManager entityManager;
        private SystemManager systemManager;
        private float clearRed, clearGreen, clearBlue;
        public DefaultScene(SceneManager sceneManager) : base(sceneManager)
        {
            sceneType = SceneType.INIT;

            entityManager = new EntityManager();
            systemManager = new SystemManager();

            sceneManager.RenderEvent = Render;
            sceneManager.UpdateEvent = Update;
            
            clearRed = 1f;
            clearBlue = 0f;
            clearGreen = 0f;

            CreateEntities();
            CreateSystems();
        }
        public override void Close()
        {
        }
        private void CreateEntities()
        {
            Entity entity = new Entity("Bouncing_Square");
            entity.AddComponent(new ComponentTransform(new Vector3(100,100,-1f), new Vector3(0,0,0), new Vector3(1f,1f,1)));
            entity.AddComponent(new ComponentShape2D(ShapeTypes.Square, new Vector2(100, 100)));
            entity.AddComponent(new ComponentColour(new Vector4(1,0.8f,0.5f,1)));
            entityManager.AddEntity(entity);

            entity = new Entity("Triangle");
            entity.AddComponent(new ComponentTransform(new Vector3(200, 259, -2f), new Vector3(0, 0, (float)Math.PI/4), new Vector3(1f, 1f, 1)));
            entity.AddComponent(new ComponentShape2D(ShapeTypes.Triangle, new Vector2(250, 200)));
            entity.AddComponent(new ComponentColour(new Vector4(0, 1f, 0.2f, 1)));
            entityManager.AddEntity(entity);

            entity = new Entity("Rectangle");
            entity.AddComponent(new ComponentTransform(new Vector3(400, 0, 0), new Vector3(0, 0, 0), new Vector3(1f, 1f, 1)));
            entity.AddComponent(new ComponentShape2D(ShapeTypes.Square, new Vector2(200, 100)));
            entity.AddComponent(new ComponentColour(new Vector4(0, 0f, 0.7f, 1)));
            entityManager.AddEntity(entity);

            entity = new Entity("Square");
            entity.AddComponent(new ComponentShape2D(ShapeTypes.Square, new Vector2(100, 100), false));
            entity.AddComponent(new ComponentTransform(new Vector3(0, 0, 0)));
            entityManager.AddEntity(entity);

            entity = new Entity("Circle");
            entity.AddComponent(new ComponentShape2D(10, new Vector2(100, 100)));
            entity.AddComponent(new ComponentTransform(new Vector3(sceneManager.Width / 2f, sceneManager.Height / 2f, 0)));
            entityManager.AddEntity(entity);
        }
        private void CreateSystems()
        {
            ISystem system;
            system = new SystemRender2D();
            systemManager.AddSystem(system);
        }

        public override void Render(FrameEventArgs e)
        {
            GL.Viewport(0, 0, sceneManager.Width, sceneManager.Height);

            GL.ClearColor(clearRed, clearGreen, clearBlue, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            systemManager.ActionSystems(entityManager);

        }

        private float time = 0;
        public override void Update(FrameEventArgs e)
        {
            time += (float)e.Time;
            clearRed = 0.2f + (0.2f * (float)(Math.Sin(time * 0.5f)));
            clearBlue = 0.2f + (0.2f * (float)(Math.Sin(time * 0.8f)));
            clearGreen = 0.2f + (0.2f * (float)(Math.Sin(time * 1.1f)));

            //Animate bouncing square
            Entity entity = entityManager.Entities.Find(ent => ent.Name == "Bouncing_Square");
            ComponentTransform compTransform = (ComponentTransform)entity.Components.Find(c => c.ComponentType == ComponentTypes.COMP_TRANSFORM);
            Vector3 rotation = compTransform.Rotation;
            //rotation.Z += (float)Math.PI / 10 * (float)e.Time;
            //compTransform.Rotation = rotation;

            Vector3 position = compTransform.Position;
            if (upping)
                position.Y += 100f * (float)e.Time;
            else
                position.Y -= 100f * (float)e.Time;

            if (position.Y >= SceneManager.Instance.Height -50)
                upping = false;
            if (position.Y <= 50)
                upping = true;

            if (horizontalling)
                position.X += 100f * (float)e.Time;
            else
                position.X -= 100f * (float)e.Time;

            if (position.X >= SceneManager.Instance.Width - 50)
                horizontalling = false;
            if (position.X <= 50)
                horizontalling = true;
            compTransform.Position = position;

            //Animate triangle
            entity = entityManager.Entities.Find(ent => ent.Name == "Triangle");
            compTransform = (ComponentTransform)entity.Components.Find(c => c.ComponentType == ComponentTypes.COMP_TRANSFORM);
            rotation = compTransform.Rotation;
            rotation.Z -= (float)Math.PI / 10 * (float)e.Time;
            compTransform.Rotation = rotation;
        }
        private bool upping = true;
        private bool horizontalling = true;
    }
}
