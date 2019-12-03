using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using engine.Components;
using engine.Managers;
using OpenTK;
using OpenTK.Graphics.OpenGL;
namespace engine.Systems
{
    class SystemRender2D : ISystem
    {
        private const ComponentTypes MASK = (ComponentTypes.COMP_TRANSFORM | ComponentTypes.COMP_GEOMETRY);

        int shaderProgramID;
        public SystemRender2D()
        {
            shaderProgramID = ShaderManager.CreateShaderProgram("Shaders/basicVertex.glsl", "Shaders/basicFragment.glsl");
        }

        public string Name => "SystemRender2D";

        public void Action(Entity entity)
        {
            if ((entity.ComponentMask & MASK) != MASK)
                return;
            
            List<IComponent> entityComponents = entity.Components;

            IComponent geometryComp = entityComponents.Find(c => c.ComponentType == ComponentTypes.COMP_GEOMETRY);
            int vertBuffer = (geometryComp as ComponentShape2D).VertexBuffer;
            int elBuffer = (geometryComp as ComponentShape2D).ElementBuffer;
            int elementCount = (geometryComp as ComponentShape2D).ElementCount;

            IComponent transformComp = entityComponents.Find(c => c.ComponentType == ComponentTypes.COMP_TRANSFORM);
            ComponentTransform transform = (ComponentTransform)transformComp;

            Matrix4 modelMat = Matrix4.Identity;
            modelMat *= Matrix4.CreateRotationX(transform.Rotation.X);
            modelMat *= Matrix4.CreateRotationY(transform.Rotation.Y);
            modelMat *= Matrix4.CreateRotationZ(transform.Rotation.Z);
            modelMat *= Matrix4.CreateTranslation(transform.Position);
            modelMat *= Matrix4.CreateScale(transform.Scale);

            IComponent colourComp = entityComponents.Find(c => c.ComponentType == ComponentTypes.COMP_COLOUR);
            Vector4 colour = new Vector4(1, 1, 1, 1);
            if (colourComp != null)
                colour = (colourComp as ComponentColour).Colour;

            Draw(modelMat, colour, vertBuffer, elBuffer, elementCount);
        }

        private void Draw(Matrix4 modelMat, Vector4 colour, int vertBuffer, int elBuffer, int elementCount)
        {
            GL.UseProgram(shaderProgramID);

            Matrix4 viewMat = Matrix4.Identity;
            viewMat *= Matrix4.CreateTranslation(new Vector3(-SceneManager.Instance.Width/2f, -SceneManager.Instance.Height/2f, 0));
            int uniViewMat = GL.GetUniformLocation(shaderProgramID, "ViewMat");
            GL.UniformMatrix4(uniViewMat, false, ref viewMat);
                       
            int uniModelMat = GL.GetUniformLocation(shaderProgramID, "ModelMat");
            GL.UniformMatrix4(uniModelMat, false, ref modelMat);

            Matrix4 projectionMat = Matrix4.CreateOrthographic(SceneManager.Instance.Width, SceneManager.Instance.Height, 0, 50);

            Matrix4 mvpMat = modelMat * viewMat * projectionMat;
            GL.UniformMatrix4(GL.GetUniformLocation(shaderProgramID,"ModelViewProjectionMat"), false, ref mvpMat);

            int uniColour = GL.GetUniformLocation(shaderProgramID, "Colour");
            GL.Uniform4(uniColour, colour);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertBuffer);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elBuffer);

            int vPositionLocation = GL.GetAttribLocation(shaderProgramID, "vPosition");
            GL.EnableVertexAttribArray(vPositionLocation);
            GL.VertexAttribPointer(vPositionLocation, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
            
            GL.DrawElements(PrimitiveType.TriangleFan, elementCount, DrawElementsType.UnsignedInt, 0);
            GL.UseProgram(0);
        }
    }
}
