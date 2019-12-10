using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace engine.Managers
{
    static class ShapeManager
    {
        private static Dictionary<IPolyKey,PolyMeta> polygons = new Dictionary<IPolyKey,PolyMeta>();
        internal struct PolyMeta
        {
            internal int elementCount;
            internal int elementBuffer;
            internal int vertexBuffer;
        }
        private interface IPolyKey
        {
            int sides { get; set; }
            bool centerIsZero { get; set; }
        }
        private struct PolyKey2D : IPolyKey
        {
            internal Vector2 size;
            public int sides { get; set; }
            public bool centerIsZero { get; set; }
        }
        private struct PolyKey3D : IPolyKey
        {
            internal Vector3 size;
            public bool centerIsZero { get; set; }
            public int sides { get; set; }
        }
        internal static PolyMeta MakeRegularPolygon(int sides, Vector2 size, bool centerIsZero)
        {
            PolyKey2D polyKey = new PolyKey2D()
            {
                sides = sides,
                size = size,
                centerIsZero = centerIsZero
            };
            if (polygons.ContainsKey(polyKey)) //Just return an already made and loaded polygon if an identical one exists.
                return polygons[polyKey];

            float[] vertices = new float[2 + sides * 2];
            vertices[0] = 0; vertices[1] = 0;
            float count = 0; //Count is used to count the number of iterations to then get the correct angle to step to. the variable 'i' steps wrong and from too far so doesn't work.
            if (sides == 4)//Offset count for 4 sided shapes so they are oriented correctly.
                count = .5f;
            for (uint i = 2; i < 2 + sides * 2; i += 2)
            {
                double theta = ((2f * Math.PI) / sides) * count;
                count++;
                double X = size.X/2f * Math.Sin(-theta);
                double Y = size.Y/2f * Math.Cos(-theta);

                if (!centerIsZero)
                {
                    vertices[0] = size.X / 2f; vertices[1] = size.Y / 2f;
                    X += size.X / 2f;
                    Y += size.Y / 2f;
                }


                vertices[i] = (float)X;
                vertices[i + 1] = (float)Y;
            }

            uint[] indices = new uint[sides + 2];
            for (uint i = 0; i < sides + 1; i++)
                indices[i] = i;
            indices[indices.Length - 1] = indices[1];

            LoadShape(vertices, indices, out int vertBufferID, out int elBufferID);

            PolyMeta polyMeta = new PolyMeta()
            {
                elementCount = indices.Length,
                elementBuffer = elBufferID,
                vertexBuffer = vertBufferID
            };

            polygons.Add(polyKey, polyMeta);

            return polyMeta;
            
        }

        internal static PolyMeta MakeCube(Vector3 size)
        {
            PolyKey3D polyKey = new PolyKey3D()
            {
                sides = 12,
                size = size,
                centerIsZero = true
            };
            if (polygons.ContainsKey(polyKey))
                return polygons[polyKey];

            float X = size.X / 2f;
            float Y = size.Y / 2f;
            float Z = size.Z / 2f;
            float[] vertices = new float[]
            {
                //front
                -X,-Y, Z, 0,0,1,
                X,-Y,Z, 0,0,1,
                X,Y,Z, 0,0,1,
                -X,Y,Z, 0,0,1,

                //right
                X, -Y, -Z, 1,0,0,
                X, Y, -Z, 1,0,0,
                X, Y, Z, 1,0,0,
                X, -Y, Z, 1,0,0,

                //back
                -X,-Y,-Z, 0, 0, -1,
                -X,Y,-Z, 0, 0, -1,
                X,Y,-Z, 0, 0, -1,
                X,-Y,-Z, 0, 0, -1,

                //left
                -X,-Y,-Z,-1,0,0,
                -X,-Y,Z,-1,0,0,
                -X,Y,Z,-1,0,0,
                -X,Y,-Z,-1,0,0,

                //top
                X,Y,Z,0,1,0,
                -X,Y,-Z,0,1,0,
                -X,Y,Z,0,1,0,
                X,Y,-Z,0,1,0,

                //bottom
                -X,-Y,-Z,0,-1,0,
                X,-Y,-Z,0,-1,0,
                X,-Y,Z,0,-1,0,
                -X,-Y,Z,0,-1,0,
            };
            uint[] indices = new uint[]
            {
                0,1,2,0,2,3,//front
                4,5,6,4,6,7,//right
                8,9,10,8,10,11,//back
                12,13,14,12,14,15,//left
                16,17,18,16,19,17,//top
                20,21,22,20,22,23,//bottom
            };
            LoadShape(vertices, indices, out int vertBufferID, out int elBufferID);

            PolyMeta polyMeta = new PolyMeta()
            {
                elementCount = indices.Length,
                elementBuffer = elBufferID,
                vertexBuffer = vertBufferID
            };

            polygons.Add(polyKey, polyMeta);

            return polyMeta;
        }

        private static void LoadShape(float[] vertices, uint[] indices, out int vertexBuffer, out int elementBuffer)
        {
            int[] buffers = new int[2];
            GL.GenBuffers(2, buffers); 
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffers[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * sizeof(float)), vertices,
            BufferUsageHint.StaticDraw);
            int size;
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (vertices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
            }
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, buffers[1]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Length * sizeof(uint)),
            indices, BufferUsageHint.StaticDraw);
            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out
            size);
            if (indices.Length * sizeof(uint) != size)
            {
                throw new ApplicationException("Index data not loaded onto graphics card correctly");
            }

            vertexBuffer = buffers[0];
            elementBuffer = buffers[1];
        }
    }
}
