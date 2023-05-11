using UnityEngine;

namespace Photon
{
    public static class DataExtensions
    {
        public static Vector3 ToDirectionVector3(this Vector2 vector2)
        {
            return new Vector3(vector2.x, 0, vector2.y);
        }
    }
}