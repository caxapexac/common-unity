using System;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Runtime.Extensions.Unity
{
    public static class TransformExtensions
    {
        public static void Reset(this Transform self)
        {
            self.localPosition = Vector3.zero;
            self.localRotation = Quaternion.identity;
            self.localScale = Vector3.one;
        }

        public static void ResetPosition(this Transform self)
        {
            self.localPosition = Vector3.zero;
        }

        public static void LookAtTopDown(this Transform self, Vector3 target)
        {
            self.LookAt(target, false, true, false);
        }

        public static void LookAt(this Transform self, Vector3 target, bool ignoreX, bool ignoreY, bool ignoreZ)
        {
            Vector3 position = self.position;
            Vector3 result = new Vector3(ignoreX ? position.x : target.x, ignoreY ? position.y : target.y, ignoreZ ? position.z : target.z);
            self.LookAt(result);
        }


        public static void LookAt2D(this Transform self, Vector2 target)
        {
            Vector2 dir = target - (Vector2)self.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            self.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        public static void LookAt2D(this Transform self, Transform target)
        {
            self.LookAt2D(target.position);
        }

        public static void LookAt2D(this Transform self, GameObject target)
        {
            self.LookAt2D(target.transform.position);
        }

        public static void AddChildren(this Transform self, GameObject[] children)
        {
            Array.ForEach(children, child => child.transform.parent = self);
        }

        /// <summary>
        /// Sets the position of a transform's children to zero.
        /// </summary>
        /// <param name="self">Parent transform.</param>
        /// <param name="recursive">Also reset ancestor positions?</param>
        public static void ResetChildrenPositions(this Transform self, bool recursive = false)
        {
            foreach (Transform child in self)
            {
                child.position = Vector3.zero;

                if (recursive)
                {
                    child.ResetChildrenPositions(true);
                }
            }
        }

        /// <summary>
        /// Sets the x component of the transform's position.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="x">Value of x.</param>
        public static void SetX(this Transform self, float x)
        {
            var position = self.position;
            self.position = new Vector3(x, position.y, position.z);
        }

        /// <summary>
        /// Sets the y component of the transform's position.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="y">Value of y.</param>
        public static void SetY(this Transform self, float y)
        {
            var position = self.position;
            self.position = new Vector3(position.x, y, position.z);
            ;
        }

        /// <summary>
        /// Sets the z component of the transform's position.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="z">Value of z.</param>
        public static void SetZ(this Transform self, float z)
        {
            var position = self.position;
            self.position = new Vector3(position.x, position.y, z);
        }

        /// <summary>
        /// Find GameObject with name in recursive hierarchy.
        /// </summary>
        /// <returns>Transform of found GameObject.</returns>
        /// <param name="self">Root of search.</param>
        /// <param name="name">Name to search.</param>
        public static Transform FindRecursive(this Transform self, string name)
        {
            if ((object)self == null || string.CompareOrdinal(self.name, name) == 0)
            {
                return self;
            }
            Transform retVal = null;
            for (var i = self.childCount - 1; i >= 0; i--)
            {
                retVal = self.GetChild(i).FindRecursive(name);
                if ((object)retVal != null)
                {
                    break;
                }
            }
            return retVal;
        }

        /// <summary>
        /// Find GameObject with tag in recursive hierarchy.
        /// </summary>
        /// <returns>Transform of found GameObject.</returns>
        /// <param name="self">Root of search.</param>
        /// <param name="tag">Tag to search.</param>
        public static Transform FindRecursiveByTag(this Transform self, string tag)
        {
            if ((object)self == null || self.CompareTag(tag))
            {
                return self;
            }
            Transform retVal = null;
            for (var i = self.childCount - 1; i >= 0; i--)
            {
                retVal = self.GetChild(i).FindRecursiveByTag(tag);
                if ((object)retVal != null)
                {
                    break;
                }
            }
            return retVal;
        }
    }
}