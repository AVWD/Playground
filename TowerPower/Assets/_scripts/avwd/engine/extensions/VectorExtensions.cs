using System;
using UnityEngine;
using System.Collections;

namespace AVWD.Engine.Services
{
    public static class VectorExtensions
    {
        public static Vector3 CrossLeft(this Vector3 v) { return new Vector3(-v.y, v.x, v.z); }
        public static Vector3 CrossRight(this Vector3 v) { return new Vector3(v.y, -v.x, v.z); }

        public static Vector2 CrossLeft(this Vector2 v) { return new Vector2(-v.y, v.x); }
        public static Vector2 CrossRight(this Vector2 v) { return new Vector2(v.y, -v.x); }

        public static Vector2 Clamp(this Vector2 v, float length) { return new Vector2(Math.Max(Math.Min(length, v.x), 0), Math.Max(Math.Min(length, v.y), 0)); }
        public static Vector3 Clamp(this Vector3 v, float length) { return new Vector2(Math.Max(Math.Min(length, v.x), 0), Math.Max(Math.Min(length, v.y), 0)); }

        public static float ToAngle(this Vector2 v) { return (float)Math.Atan2(v.y, v.x); }
        public static Vector2 ScaleTo(this Vector2 v, float length) { return v * (length / v.magnitude); }

        public static Vector2 TruncateLength(this Vector2 v, float maxLength)
        {
            if (v.sqrMagnitude > maxLength * maxLength)
                v = v.normalized * maxLength;
            return v;
        }

    }
}

