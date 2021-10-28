using UnityEngine;

namespace Tetris.Extensions
{
    public static class GameObjctExtensions
    {
        public static void SetSelfAndChildrenLayer(this GameObject go, int layer)
        {
            go.layer = layer;
            for (var i = 0; i < go.transform.childCount; i++)
            {
                var child = go.transform.GetChild(i);
                child.gameObject.layer = layer;
            }
        }
    }
}
