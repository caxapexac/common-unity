using System;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Runtime.Extensions.Unity
{
    public static class Vector3IntExtensions
    {
        public static Vector3 ToVector3(this Vector3Int self)
        {
            return new Vector3(Convert.ToSingle(self.x), Convert.ToSingle(self.y), Convert.ToSingle(self.z));
        }
    }
}