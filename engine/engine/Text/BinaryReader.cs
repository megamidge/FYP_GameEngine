using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Based on/taken from http://stevehanov.ca/blog/TrueType.js
/// </summary>
namespace engine.Text
{
    
    
    class BinaryReader
    {
        uint pos;
        byte[] data;
        internal BinaryReader(byte[] arrayBuffer)
        {
            this.pos = 0;
            this.data = arrayBuffer;
        }

        internal uint Seek(uint pos)
        {
            if (pos < 0 || pos > this.data.Length) throw new Exception("Seek position out of bounds.");
            uint oldPos = this.pos;
            this.pos = pos;
            return oldPos;
        }
        internal uint CurrentPos()
        {
            return this.pos;
        }
        internal byte GetByte()
        {
            if (this.pos >= this.data.Length) throw new Exception("Position out of bounds.");
            return this.data[this.pos++];
        }
        /// <summary>
        /// This needs testing.
        /// </summary>
        /// <returns></returns>
        internal ushort GetUShort()
        {
            short s = (short)(this.GetByte() << 8 | this.GetByte());
            return (ushort)((ushort)s >> 0); //fuck knows if this is correct. Neds t
        }
        internal uint GetUint()
        {
            return (uint)this.GetInt() >> 0;
        }
        internal short GetShort()
        {
            short result = (short)this.GetUShort();
            if ((result & 0x8000) != 0)
                unchecked
                {
                    result -= (short)(1 << 16);
                }
            return result;
        }
        internal int GetInt()
        {
            return ((this.GetByte() << 24) |
                    (this.GetByte() << 16) |
                    (this.GetByte() << 8) |
                    (this.GetByte()));
        }

        internal short GetFword()
        {
            return this.GetShort();
        }

        internal short Get2Dot14()
        {
            return (short)(this.GetShort() / (1 << 14));
        }
        internal int GetFixed()
        {
            return this.GetInt() / (1 << 16);
        }
        internal string GetString(uint length)
        {
            string result = "";
            for (uint i = 0; i < length; i++)
                result += (char)this.GetByte();
            return result;
        }

        internal DateTime GetDate()
        {
            long macTime = this.GetUint() * 0x100000000 + this.GetUint();
            long utcTime = (macTime * 1000) + (long)(DateTime.UtcNow - new DateTime(1904, 1, 1)).TotalMilliseconds;
            return new DateTime(utcTime);
        }
    }
}
