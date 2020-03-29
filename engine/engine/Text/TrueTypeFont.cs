using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace engine.Text
{
    internal class Point
    {
        internal bool onCurve;
        internal int X;
        internal int Y;
    }
    internal class Component
    {
        internal struct Matrix
        {
            internal int a;
            internal int b;
            internal int c;
            internal int d;
            internal int e;
            internal int f;
        }
        internal ushort glyphIndex;
        internal Matrix matrix;
        internal int destPointIndex;
        internal int srcPointIndex;
    }
    class TrueTypeFont
    {
        struct Table
        {
            internal uint checksum;
            internal uint offset;
            internal uint length;
        }

        struct Glyph
        {
            internal short numberOfContours;
            internal short xMin;
            internal short yMin;
            internal short xMax;
            internal short yMax;
            internal string type;
            internal List<ushort> contourEnds;
            internal List<Point> points;
            internal List<Component> components;
        }
        private BinaryReader file;
        private Dictionary<string, Table> tables;
        internal uint length;

        private uint scalarType;
        private ushort searchRange;
        private ushort entrySelector;
        private ushort rangeShift;

        //Head table:
        internal int version;
        private int fontRevision;
        private uint checksumAdjustment;
        private uint magicNumber;
        private ushort flags;
        private ushort unitsPerEm;
        private DateTime created;
        private DateTime modified;
        internal short xMin;
        internal short yMin;
        internal short xMax;
        internal short yMax;
        private ushort macStyle;
        private ushort lowestRecPPEM;
        private short fontDirectionHint;
        private short indexToLocFormat;
        private short glyphDataFormat;

        internal TrueTypeFont(byte[] arrayBuffer)
        {
            this.file = new BinaryReader(arrayBuffer);
            this.tables = this.ReadOffsetTables(this.file);
            ReadHeadTable(this.file);
            //TO-DO: Read name table to extract name info.
            this.length = this.GlyphCount();
        }
        private Dictionary<string, Table> ReadOffsetTables(BinaryReader file)
        {
            Dictionary<string, Table> tables = new Dictionary<string, Table>();
            this.scalarType = file.GetUint();
            ushort numTables = file.GetUShort();
            this.searchRange = file.GetUShort();
            this.entrySelector = file.GetUShort();
            this.rangeShift = file.GetUShort();

            for (uint i = 0; i < numTables; i++)
            {
                string tag = file.GetString(4);
                tables.Add(
                    tag,
                    new Table()
                    {
                        checksum = file.GetUint(),
                        offset = file.GetUint(),
                        length = file.GetUint()
                    }
                );

                if (tag != "head")
                {
                    System.Diagnostics.Debug.Assert(this.CalculateTableChecksum(file, tables[tag].offset, tables[tag].length) == tables[tag].checksum);
                }
            }
            return tables;
        }

        private uint CalculateTableChecksum(BinaryReader file, uint offset, uint length)
        {
            uint old = file.Seek(offset);
            uint sum = 0;
            int nlongs = (((int)length + 3) / 4) | 0;
            while (nlongs-- > 0)
            {
                uint val = sum + file.GetUint() & 0xffffffff;
                sum = (uint)((uint)val >> 0); //requires testing.
            }
            file.Seek(old);
            return sum;
        }
        /// <summary>
        /// As defined in https://docs.microsoft.com/en-us/typography/opentype/spec/head
        /// </summary>
        /// <param name="file"></param>
        private void ReadHeadTable(BinaryReader file)
        {
            System.Diagnostics.Debug.Assert(this.tables.ContainsKey("head"));
            file.Seek(this.tables["head"].offset);

            this.version = file.GetFixed();
            this.fontRevision = file.GetFixed();
            this.checksumAdjustment = file.GetUint();
            this.magicNumber = file.GetUint();
            System.Diagnostics.Debug.Assert(this.magicNumber == 0x5f0f3cf5);
            this.flags = file.GetUShort();
            this.unitsPerEm = file.GetUShort();
            this.created = file.GetDate();
            this.modified = file.GetDate();
            this.xMin = file.GetFword();
            this.yMin = file.GetFword();
            this.xMax = file.GetFword();
            this.yMax = file.GetFword();
            this.macStyle = file.GetUShort();
            this.lowestRecPPEM = file.GetUShort();
            this.fontDirectionHint = file.GetShort();
            this.indexToLocFormat = file.GetShort();
            this.glyphDataFormat = file.GetShort();
        }
        private uint GlyphCount()
        {
            System.Diagnostics.Debug.Assert(this.tables.ContainsKey("maxp"));
            uint old = this.file.Seek(this.tables["maxp"].offset + 4);
            ushort count = this.file.GetUShort();
            this.file.Seek(old);
            return count;
        }
        private uint GetGlyphOffset(uint index)
        {
            System.Diagnostics.Debug.Assert(this.tables.ContainsKey("loca"));
            Table table = this.tables["loca"];
            BinaryReader file = this.file;
            uint offset, old;
            if (this.indexToLocFormat == 1)
            {
                old = file.Seek(table.offset + index * 4);
                offset = file.GetUint();
            }
            else
            {
                old = file.Seek(table.offset + index * 2);
                offset = (uint)(file.GetUShort() * 2);
            }

            file.Seek(old);

            return offset + this.tables["glyf"].offset;
        }
        private Glyph? ReadGlyph(uint index)
        {
            uint offset = this.GetGlyphOffset(index);
            BinaryReader file = this.file;

            if (offset >= this.tables["glyf"].offset + this.tables["glyf"].length)
                return null;
            System.Diagnostics.Debug.Assert(offset >= this.tables["glyf"].offset);
            System.Diagnostics.Debug.Assert(offset < this.tables["glyf"].offset + this.tables["glyf"].length);

            file.Seek(offset);
            Glyph glyph = new Glyph()
            {
                numberOfContours = file.GetShort(),
                xMin = file.GetFword(),
                yMin = file.GetFword(),
                xMax = file.GetFword(),
                yMax = file.GetFword()
            };

            System.Diagnostics.Debug.Assert(glyph.numberOfContours >= -1);

            if (glyph.numberOfContours == -1)
                this.ReadCompoundGlyph(file, ref glyph);
            else
                this.ReadSimpleGlyph(file, ref glyph);

            return glyph;
        }

        private void ReadSimpleGlyph(BinaryReader file, ref Glyph glyph)
        {
            uint ON_CURVE = 1;
            uint X_IS_BYTE = 2;
            uint Y_IS_BYTE = 4;
            uint REPEAT = 8;
            uint X_DELTA = 16;
            uint Y_DELTA = 32;

            glyph.type = "simple";
            glyph.contourEnds = new List<ushort>();
            List<Point> points = glyph.points = new List<Point>();
            for (uint i = 0; i < glyph.numberOfContours; i++)
            {
                glyph.contourEnds.Add(file.GetUShort());
            }

            //skip over instructions
            file.Seek(file.GetUShort() + file.CurrentPos());

            if (glyph.numberOfContours == 0)
                return;

            int numPoints = glyph.contourEnds.Max() + 1;

            List<byte> flags = new List<byte>();
            for (uint i = 0; i < numPoints; i++)
            {
                byte flag = file.GetByte();
                flags.Add(flag);
                points.Add(new Point() { onCurve = (flag & ON_CURVE) > 0 });
                if ((flag & REPEAT) != 0)
                {
                    byte repeatCount = file.GetByte();
                    System.Diagnostics.Debug.Assert(repeatCount > 0);
                    i += repeatCount;
                    while (repeatCount-- > 0)
                    {
                        flags.Add(flag);
                        points.Add(new Point() { onCurve = (flag & ON_CURVE) > 0 });
                    }
                }
            }

            void ReadCoords(string name, uint byteFlag, uint deltaFlag, short min, short max)
            {
                int value = 0;
                for (int i = 0; i < numPoints; i++)
                {
                    byte flag = flags[i];
                    if ((flag & byteFlag) != 0)
                    {
                        if ((flag & deltaFlag) != 0)
                            value += file.GetByte();
                        else
                            value -= file.GetByte();
                    }
                    else if ((~flag & deltaFlag) != 0)
                    {
                        value += file.GetShort();
                    }
                    else
                    {
                        //value unchanged
                    }

                    if (name == "x")
                        points[i].X = value;
                    else if (name == "y")
                        points[i].Y = value;
                }
            }

            ReadCoords("x", X_IS_BYTE, X_DELTA, glyph.xMin, glyph.xMax);
            ReadCoords("y", Y_IS_BYTE, Y_DELTA, glyph.yMin, glyph.yMax);

            glyph.points = points;//Just making sure points is updated as not confident points is a reference seeing as not allowed to modify the list directly.
        }


        private void ReadCompoundGlyph(BinaryReader file, ref Glyph glyph)
        {
            uint ARG_1_AND_2_ARE_WORDS = 1;
            uint ARGS_ARE_XY_VALUES = 2;
            uint ROUND_XY_TO_GRID = 4;
            uint WE_HAVE_A_SCALE = 8;
            //RESERVED = 16;
            uint MORE_COMPONENTS = 32;
            uint WE_HAVE_AN_X_AND_Y_SCALE = 64;
            uint WE_HAVE_A_TWO_BY_TWO = 128;
            uint WE_HAVE_INSTRUCTIONS = 256;
            uint USE_MY_METRICS = 512;
            uint OVERLAP_COMPONENT = 1024;

            glyph.type = "compound";
            glyph.components = new List<Component>();

            uint flags = MORE_COMPONENTS;
            while ((flags & MORE_COMPONENTS) != 0)
            {
                int arg1, arg2;

                flags = file.GetUShort();
                Component component = new Component()
                {
                    glyphIndex = file.GetUShort(),
                    matrix = new Component.Matrix()
                    {
                        a = 1,
                        b = 0,
                        c = 0,
                        d = 1,
                        e = 0,
                        f = 0
                    }
                };

                if ((flags & ARG_1_AND_2_ARE_WORDS) != 0)
                {
                    arg1 = file.GetShort();
                    arg2 = file.GetShort();
                }
                else
                {
                    arg1 = file.GetByte();
                    arg2 = file.GetByte();
                }

                if ((flags & ARGS_ARE_XY_VALUES) != 0)
                {
                    component.matrix.e = arg1;
                    component.matrix.f = arg2;
                }
                else
                {
                    component.destPointIndex = arg1;
                    component.srcPointIndex = arg2;
                }

                if ((flags & WE_HAVE_A_SCALE) != 0)
                {
                    component.matrix.a = file.Get2Dot14();
                    component.matrix.d = component.matrix.a;
                }
                else if ((flags & WE_HAVE_AN_X_AND_Y_SCALE) != 0)
                {
                    component.matrix.a = file.Get2Dot14();
                    component.matrix.d = file.Get2Dot14();
                }
                else if ((flags & WE_HAVE_A_TWO_BY_TWO) != 0)
                {
                    component.matrix.a = file.Get2Dot14();
                    component.matrix.b = file.Get2Dot14();
                    component.matrix.c = file.Get2Dot14();
                    component.matrix.d = file.Get2Dot14();
                }

                glyph.components.Add(component);
            }

            if ((flags & WE_HAVE_INSTRUCTIONS) != 0)
            {
                file.Seek(file.GetUShort() + file.CurrentPos());
            }
        }

        internal bool DrawGlyph(uint index)
        {

            Glyph? glyph = ReadGlyph(index);
            if(glyph == null || glyph?.type != "simple") //Only draws simple glyphs i suppose.
                return false;
            if (glyph?.xMax - glyph?.xMin <= 0)
                return false;
            if (glyph?.yMax - glyph?.yMin <= 0)
                return false;
            int width = (int)(glyph?.xMax - glyph?.xMin);
            int height = (int)(glyph?.yMax - glyph?.yMin);
            Bitmap bmp = new Bitmap(width,height);
            Graphics bmpGfx = Graphics.FromImage(bmp);
            bmpGfx.TranslateTransform((int)-glyph?.xMin, (int)-glyph?.yMin);
            int p = 0, c = 0, first = 1;
            Pen pen = new Pen(Color.White, 4);
            Brush brush = System.Drawing.Brushes.Black;
            List<GraphicsPath> paths = new List<GraphicsPath>();
            GraphicsPath gp = new GraphicsPath(FillMode.Winding);
            int firstX = 0, firstY = 0;
            int contextX=0, contextY=0; //X and Y pos to draw a line from.
            while (p < glyph?.points.Count)
            {
                Point point = glyph?.points[p];
                if(first == 1)
                {
                    firstX = point.X;
                    firstY = point.Y;
                    contextX = point.X;
                    contextY = point.Y;
                    first = 0;
                }
                else
                {
                    gp.AddLine(contextX, contextY, point.X, point.Y);
                    contextX = point.X;
                    contextY = point.Y;
                }

                if(p == glyph?.contourEnds[c])
                {
                    c += 1;
                    first = 1;
                    gp.AddLine(contextX, contextY, firstX, firstY);
                    paths.Add(gp);
                    gp = new GraphicsPath(FillMode.Winding);
                }

                p++;
            }
            GraphicsPath finalPath = paths[0];
            for (int i = 1; i < paths.Count; i++)
                finalPath.AddPath(paths[i], false);
            bmpGfx.FillPath(brush, finalPath);
            bmpGfx.Flush();
            bmpGfx.Save();
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            bmp.Save($"Glyphs/{index}.jpg");
            bmpGfx.Dispose();
            bmp.Dispose();
            return true;
        }
    }
}
