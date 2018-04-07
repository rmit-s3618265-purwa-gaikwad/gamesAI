using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GamesAI
{
    public static class Helper
    {
        /// <summary>
        /// Computes the average of a sequence of Vector3 values, possibly normalized
        /// </summary>
        /// <param name="source"></param>
        /// <param name="normalize"></param>
        /// <returns>
        /// The average of the sequence of values, normalized if <paramref name="normalize"/> is true
        /// Vector3.zero if the sequence is empty
        /// </returns>
        public static Vector3 Average(this IEnumerable<Vector3> source, bool normalize)
        {
            return source.Aggregate(new {vector = Vector3.zero, count = 0},
                (acc, vec) => new {vector = acc.vector + vec, count = acc.count + 1},
                acc =>
                {
                    Vector3 vec = acc.vector;
                    if (acc.count > 0)
                    {
                        vec /= acc.count;
                        if (normalize)
                        {
                            vec.Normalize();
                        }
                    }
                    return vec;
                });
        }

        /// <summary>
        /// Computes the sum of a sequence of Vector3 values, possibly normalized
        /// </summary>
        /// <param name="source"></param>
        /// <param name="normalize"></param>
        /// <returns>
        /// The sum of the sequence of values, normalized if <paramref name="normalize"/> is true
        /// Vector3.zero if the sequence is empty
        /// </returns>
        public static Vector3 Sum(this IEnumerable<Vector3> source, bool normalize)
        {
            return source.Aggregate(new { vector = Vector3.zero, count = 0 },
                (acc, vec) => new { vector = acc.vector + vec, count = acc.count + 1 },
                acc =>
                {
                    Vector3 vec = acc.vector;
                    if (acc.count > 0 && normalize)
                    {
                        vec.Normalize();
                    }
                    return vec;
                });
        }

        public static ClosestResult ClosestTo(this IEnumerable<Character> source, Vector3 pos, float sqrRange)
        {
            return source.Aggregate(new ClosestResult(character: null, sqrDistance: sqrRange),
                (closest, character) =>
                {
                    float sqrDistance = (character.transform.position - pos).sqrMagnitude;
                    return sqrDistance <= closest.SqrDistance
                        ? new ClosestResult(character, sqrDistance)
                        : closest;
                });
        }

        public class ClosestResult
        {
            public readonly Character Character;
            public readonly float SqrDistance;

            internal ClosestResult(Character character, float sqrDistance)
            {
                Character = character;
                SqrDistance = sqrDistance;
            }
        }

        public static Vector3 IgnoreY(this Vector3 vec) => new Vector3(vec.x, 0, vec.z);

        public static Vector2Int Sign(this Vector2Int vec) => new Vector2Int(Math.Sign(vec.x), Math.Sign(vec.y));
        public static Vector2Int Sign(this Vector2 vec) => new Vector2Int(Math.Sign(vec.x), Math.Sign(vec.y));

        public static Vector3 RandomRange(Vector3 min, Vector3 max)
            => new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
    }
}