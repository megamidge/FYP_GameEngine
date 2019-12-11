using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace engine.Managers
{
    static class TextureManager
    {
        private static Dictionary<string, int> textures = new Dictionary<string, int>();
        internal static int LoadTexture(string textureLocation)
        {
            if (textures.ContainsKey(textureLocation))
                return textures[textureLocation];
            int texId = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, texId);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            Bitmap bmp = new Bitmap(textureLocation);
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmpData.Width, bmpData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);

            bmp.UnlockBits(bmpData);

            textures.Add(textureLocation, texId);
            return texId;
        }
    }
}
