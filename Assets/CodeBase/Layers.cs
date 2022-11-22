using UnityEngine;

namespace CodeBase
{
    public static class Layers
    {
        private static readonly int ArrowsLayerMask = LayerMask.GetMask("Arrows");
        private static readonly int WorldBoundsLayerMask = LayerMask.GetMask("WorldBoundaries");

        public static readonly int ArrowsMask = ~(ArrowsLayerMask | WorldBoundsLayerMask);
    }
}