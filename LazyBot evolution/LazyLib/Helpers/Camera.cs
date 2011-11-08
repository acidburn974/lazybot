
﻿/*
This file is part of LazyBot - Copyright (C) 2011 Arutha

    LazyBot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    LazyBot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with LazyBot.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using LazyLib.Wow;

namespace LazyLib.Helpers
{
    internal class Camera
    {
        internal class World2Screen
        {
            [DllImport("user32.dll")]
            private static extern bool GetClientRect(IntPtr hWnd, ref Rect rect);
            private const Single Deg2Rad = (Single)Math.PI / 180;
            // ReSharper disable InconsistentNaming
            #pragma warning disable 649
            private struct Rect
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }
            #pragma warning restore 649
            // ReSharper restore InconsistentNaming

            internal static Point WorldToScreen(Location position, bool realPos)
            {
                return WorldToScreen(position.X, position.Y, position.Z, realPos);
            }

            internal static Point WorldToScreen(float x, float y, float z, bool realPos)
            {

                var Result = new Point
                {
                    X = 0,
                    Y = 0
                };

                Vector pseudoVec = new Vector(x, y, z);

                Vector Pos = new Vector(X, Y, Z);

                Vector diff = pseudoVec - Pos;

                Vector view = diff * Matrix.Inverse();
                Vector cam = new Vector(-view.Y, -view.Z, view.X);

                Single WowWidth = Convert.ToSingle((uint)InterfaceHelper.WindowWidth);
                Single WowHeight = Convert.ToSingle((uint)InterfaceHelper.WindowHeight);

                Single fHorizontalAdjust = ((WowWidth / WowHeight >= 1.6f) ? 55.0f : 44.0f);

                Single fScreenX = WowWidth / 2.0f;
                Single fScreenY = WowHeight / 2.0f;

                Rect rect = new Rect();
                GetClientRect(Memory.WindowHandle, ref rect);

                float modifier = 1.0f;
                float modifier2 = 1.08f;
                if (1.0 * rect.right / rect.bottom > 1.5)
                {
                    modifier *= 1.15f;
                    modifier2 = 1.0f;
                }

                Single fTmpX = fScreenX / (Single)Math.Tan((((WowWidth / WowHeight) * fHorizontalAdjust) * modifier2 * modifier / 2.0f) * Deg2Rad);
                Single fTmpY = fScreenY / (Single)Math.Tan((((WowWidth / WowHeight) * 35) / 2.0f) * Deg2Rad);

                Result.X = (int)(fScreenX + cam.X * fTmpX / cam.Z);
                Result.Y = (int)(fScreenY + cam.Y * fTmpY / cam.Z);

                if (Result.X < 0 || Result.Y < 0)
                {
                    Result.X = 0;
                    Result.Y = 0;
                }
                else
                {
                    if (realPos)
                    {

                        Point p = new Point();
                        Frame.ScreenToClient(Memory.WindowHandle, ref p);
                        Result.X += Math.Abs(p.X);
                        Result.Y += Math.Abs(p.Y);

                    }
                    Result.Y -= 20;
                }
                return Result;
            }
        }

        internal static UInt32 BaseAddress
        {
            get
            {
                return Memory.Read<uint>(Memory.ReadRelative<uint>((uint)Pointers.CgWorldFrameGetActiveCamera.CameraPointer) + (uint)Pointers.CgWorldFrameGetActiveCamera.CameraOffset);
            }
        }

        internal static Single X
        {
            get
            {
                return Memory.Read<float>(BaseAddress + (uint)Pointers.CgWorldFrameGetActiveCamera.CameraX);
            }
        }

        internal static Single Y
        {
            get
            {
                return Memory.Read<float>(BaseAddress + (uint)Pointers.CgWorldFrameGetActiveCamera.CameraY);
            }
        }

        internal static Single Z
        {
            get
            {
                return Memory.Read<float>(BaseAddress + (uint)Pointers.CgWorldFrameGetActiveCamera.CameraZ);
            }
        }

        internal static Matrix Matrix
        {
            get
            {

                byte[] bCamera = Memory.ReadBytes(BaseAddress + (uint)Pointers.CgWorldFrameGetActiveCamera.CameraMatrix, 36);

                return new Matrix(BitConverter.ToSingle(bCamera, 0), BitConverter.ToSingle(bCamera, 4), BitConverter.ToSingle(bCamera, 8),
                    BitConverter.ToSingle(bCamera, 12), BitConverter.ToSingle(bCamera, 16), BitConverter.ToSingle(bCamera, 20),
                    BitConverter.ToSingle(bCamera, 24), BitConverter.ToSingle(bCamera, 28), BitConverter.ToSingle(bCamera, 32));
            }
        }

    }

    public class Matrix
    {
        private float _x1, _x2, _x3, _y1, _y2, _y3, _z1, _z2, _z3;
        public Matrix(float x1, float x2, float x3, float y1, float y2, float y3, float z1, float z2, float z3)
        {
            _x1 = x1;
            _x2 = x2;
            _x3 = x3;
            _y1 = y1;
            _y2 = y2;
            _y3 = y3;
            _z1 = z1;
            _z2 = z2;
            _z3 = z3;
        }
        public Matrix()
        { }
        public Vector GetFirstColumn
        {
            get { return new Vector(_x1, _y1, _z1); }
        }
        public Matrix Inverse()
        {
            var d = 1 / Det();
            var inv = new Matrix(d * (_y2 * _z3 - _y3 * _z2), d * (_x3 * _z2 - _x2 * _z3), d * (_x2 * _y3 - _x3 * _y2),
                                    d * (_y3 * _z1 - _y1 * _z3), d * (_x1 * _z3 - _x3 * _z1), d * (_x3 * _y1 - _x1 * _y3),
                                    d * (_y1 * _z2 - _y2 * _z1), d * (_x2 * _z1 - _x1 * _z2), d * (_x1 * _y2 - _x2 * _y1));
            return inv;
        }
        public float Det()
        {
            float det = (_x1 * _y2 * _z3) + (_x2 * _y3 * _z1) + (_x3 * _y1 * _z2)
                            - (_x3 * _y2 * _z1) - (_x2 * _y1 * _z3) - (_x1 * _y3 * _z2);
            return det;
        }

        public static Vector operator *(Vector v, Matrix m)
        {
            var res = new Vector(m._x1 * v.X + m._y1 * v.Y + m._z1 * v.Z,
                                    m._x2 * v.X + m._y2 * v.Y + m._z2 * v.Z,
                                    m._x3 * v.X + m._y3 * v.Y + m._z3 * v.Z);
            return res;
        }

    }

    public class Vector
    {
        private float _x, _y, _z;
        public Vector()
        {
            _x = 0;
            _y = 0;
            _z = 0;
        }
        public Vector(float x, float y, float z)
        {
            _x = x;
            _y = y;
            _z = z;
        }
        public Vector(Vector v)
        {
            _x = v._x;
            _y = v._y;
            _z = v._z;
        }

        public void SetVec(Vector v)
        {
            _x = v._x;
            _y = v._y;
            _z = v._z;
        }
        public void SetVec(float x, float y, float z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public float X
        {
            get { return _x; }
            set { _x = value; }
        }
        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }
        public float Z
        {
            get { return _z; }
            set { _z = value; }
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            var v3 = new Vector((v1._x + v2._x), (v1._y + v2._y), (v1._z + v2._z));
            return v3;
        }
        public static Vector operator -(Vector v1, Vector v2)
        {
            var v3 = new Vector((v1._x - v2._x), (v1._y - v2._y), (v1._z - v2._z));
            return v3;
        }
        public static float operator *(Vector v1, Vector v2)
        {
            var f = v1._x * v2._x + v1._y * v2._y + v1._z * v2._z;
            return f;
        }
    }
}
