﻿using System;
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
    class SystemRender3D : ISystem
    {
        private const ComponentTypes MASK = (ComponentTypes.COMP_TRANSFORM | ComponentTypes.COMP_GEOMETRY_3D);

        public string Name => "SystemRender3D";

        int shaderProgramID;
        public SystemRender3D()
        {
            shaderProgramID = ShaderManager.CreateShaderProgram("Shaders/basicVertex3D.glsl", "Shaders/basicFragment3D.glsl");
        }

        public void Action(Entity entity)
        {
            if ((entity.ComponentMask & MASK) != MASK)
                return;

            List<IComponent> entityComponents = entity.Components;

            IComponent geometryComp = entityComponents.Find(c => c.ComponentType == ComponentTypes.COMP_GEOMETRY_3D);
            int vertBuffer = (geometryComp as ComponentShape).VertexBuffer;
            int elBuffer = (geometryComp as ComponentShape).ElementBuffer;
            int elementCount = (geometryComp as ComponentShape).ElementCount;

            IComponent transformComp = entityComponents.Find(c => c.ComponentType == ComponentTypes.COMP_TRANSFORM);
            ComponentTransform transform = (ComponentTransform)transformComp;

            Matrix4 modelMat = Matrix4.Identity;
            modelMat *= Matrix4.CreateRotationX(transform.Rotation.X);
            modelMat *= Matrix4.CreateRotationY(transform.Rotation.Y);
            modelMat *= Matrix4.CreateRotationZ(transform.Rotation.Z);
            modelMat *= Matrix4.CreateScale(transform.Scale);
            modelMat *= Matrix4.CreateTranslation(transform.Position);

            IComponent colourComp = entityComponents.Find(c => c.ComponentType == ComponentTypes.COMP_COLOUR);
            Vector4 colour = new Vector4(1, 1, 1, 1);
            if (colourComp != null)
                colour = (colourComp as ComponentColour).Colour;

            Draw(modelMat, colour, vertBuffer, elBuffer, elementCount);
        }

        private void Draw(Matrix4 modelMat, Vector4 colour, int vertBuffer, int elBuffer, int elementCount)
        {
            GL.UseProgram(shaderProgramID);
            
            int uniModelMat = GL.GetUniformLocation(shaderProgramID, "ModelMat");
            GL.UniformMatrix4(uniModelMat, false, ref modelMat);

            Matrix4 mvpMat = modelMat * DefaultScene.instance.camera.view * DefaultScene.instance.camera.projection;
            GL.UniformMatrix4(GL.GetUniformLocation(shaderProgramID, "ModelViewProjectionMat"), false, ref mvpMat);

            int uniColour = GL.GetUniformLocation(shaderProgramID, "Colour");
            GL.Uniform4(uniColour, colour);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertBuffer);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elBuffer);

            int vPositionLocation = GL.GetAttribLocation(shaderProgramID, "vPosition");
            GL.EnableVertexAttribArray(vPositionLocation);
            GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            int vNormalLocation = GL.GetAttribLocation(shaderProgramID, "vNormal");
            GL.EnableVertexAttribArray(vNormalLocation);
            GL.VertexAttribPointer(vNormalLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));

            GL.DrawElements(PrimitiveType.Triangles, elementCount, DrawElementsType.UnsignedInt, 0);
            GL.UseProgram(0);

        }

    }
}
