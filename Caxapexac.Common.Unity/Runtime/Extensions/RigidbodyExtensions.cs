using UnityEngine;


namespace Caxapexac.Common.Sharp.Runtime.Extensions.Unity
{
    public static class RigidbodyExtensions
    {
        /// <summary>
        /// Changes the direction of a rigidbody without changing its speed.
        /// </summary>
        /// <param name="self">Rigidbody.</param>
        /// <param name="direction">New direction.</param>
        public static void ChangeDirection(this Rigidbody self, Vector3 direction)
        {
            self.velocity = direction * self.velocity.magnitude;
        }
    }
}