using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace engine.Managers
{
    internal static class ShaderManager
    {
        /// <summary>
        /// Create a shader program comprised of a vertex and fragment shader.
        /// </summary>
        /// <param name="vertexShader"></param>
        /// <param name="fragmentShader"></param>
        /// <returns>ID of the shader program</returns>
        internal static int CreateShaderProgram(string vertexShader, string fragmentShader)
        {
            int programID = GL.CreateProgram();
            GL.AttachShader(programID, LoadShader(vertexShader, ShaderType.VertexShader));
            GL.AttachShader(programID, LoadShader(fragmentShader, ShaderType.FragmentShader));
            GL.LinkProgram(programID);
            Console.WriteLine($"Shader program created. Info:\r\n {GL.GetShaderInfoLog(programID)}");
            return programID;
        }
        /// <summary>
        /// Load a shader.
        /// </summary>
        /// <param name="shaderFile"></param>
        /// <param name="shaderType"></param>
        /// <returns>The address of the shader</returns>
        private static int LoadShader(string shaderFile, ShaderType shaderType)
        {
            int shaderAddr = GL.CreateShader(shaderType);
            GL.ShaderSource(shaderAddr, System.IO.File.ReadAllText(shaderFile));
            GL.CompileShader(shaderAddr);
            Console.WriteLine($"Shader {shaderFile} loaded. Info:\r\n {GL.GetShaderInfoLog(shaderAddr)}");
            return shaderAddr;
        }
    }
}
