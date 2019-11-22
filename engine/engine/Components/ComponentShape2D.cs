using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
namespace engine.Components
{
    class ComponentShape2D : IComponent
    {
        public ComponentTypes ComponentType => ComponentTypes.COMP_GEOMETRY;

        private bool centerIsZero;

        private float[] vertices;
        private uint[] indices;

        private int[] mVertexBufferObjectIDArray = new int[2];
        public int VertexBuffer => mVertexBufferObjectIDArray[0];
        public int ElementBuffer => mVertexBufferObjectIDArray[1];
        public int ElementCount => indices.Length;
        public ComponentShape2D(ShapeTypes shape, Vector2 size, bool centerIsZero = true)
        {
            this.centerIsZero = centerIsZero;
            switch (shape)
            {
                case ShapeTypes.Square:
                    MakeSquare(size);
                    break;
                case ShapeTypes.Triangle:
                    MakeTriangle(size);
                    break;
            }
            LoadShape();
        }

        private void LoadShape()
        {
            GL.GenBuffers(2, mVertexBufferObjectIDArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, mVertexBufferObjectIDArray[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * sizeof(float)), vertices,
            BufferUsageHint.StaticDraw);
            int size;
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (vertices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
            }
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mVertexBufferObjectIDArray[1]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Length * sizeof(uint)),
            indices, BufferUsageHint.StaticDraw);
            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out
            size);
            if (indices.Length * sizeof(uint) != size)
            {
                throw new ApplicationException("Index data not loaded onto graphics card correctly");
            }
        }
        private void MakeSquare(Vector2 size)
        {
            //h and v represent half the vertical and horizontal size - half to make 
            //the vertice writing easier, and centered around 0.
            float h = size.X / 2f;
            float v = size.Y / 2f;

            float top = centerIsZero ? size.Y / 2f : size.Y;
            float bottom = centerIsZero ? -size.Y/ 2f : 0;
            float left = centerIsZero ? -size.X / 2f : 0;
            float right = centerIsZero ? size.X / 2f : size.X;
            vertices = new float[] {
                left,   top,
                right,  top,
                right,  bottom,
                left,   bottom
            };
            indices = new uint[] { 1, 0, 3, 2, 1, 3 };
        }
        private void MakeTriangle(Vector2 size)
        {
            float h = size.X / 2f;
            float v = size.Y / 2f;
            vertices = new float[] {
                0f,v,
                -h,-v,
                h,-v,
            };
            indices = new uint[] { 1, 2, 0};
        }
    }
    enum ShapeTypes
    {
        Square,
        Triangle
    }
}
