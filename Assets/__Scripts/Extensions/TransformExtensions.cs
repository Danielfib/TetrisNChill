using UnityEngine;
using System.Linq;

namespace Tetris.Extensions
{
    public static class TransformExtensions
    {
        public static Transform[] GetChildren(this Transform t)
        {
            return t.GetComponentsInChildren<Transform>().Where(c => c != t).ToArray();
        }
    }
}
