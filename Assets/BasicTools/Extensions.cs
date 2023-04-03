using System;
using System.Collections.Generic;
using UnityEngine;

namespace BasicTools
{
    public static class Extensions
    {
        public static Vector2 GetVectorXZ(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.z);
        }
        public static Vector3 GetVector3(this Vector2 vector)
        {
            return new Vector3(vector.x, 0, vector.y);
        }

        public static void RemoveRange<T>(this List<T> list,IEnumerable<T> range)
        {
            foreach(T obj in range)
            {
                list.Remove(obj);
            }
        }
    }
}
