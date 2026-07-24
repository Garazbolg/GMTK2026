using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevCore.Core
{
    /// <summary>
    /// Helper extensions to manipulate random values
    /// </summary>
    public static class RandomExtensions
    {
        public static float RandomRange(this Vector2 minMax) {
            return Random.Range(minMax.x, minMax.y);
        }

        public static T GetRandom<T>(this T[] array) {
            return array[Random.Range(0, array.Length)];
        }
    }
}
