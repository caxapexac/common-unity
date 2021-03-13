using System;


namespace Caxapexac.Common.Sharp.Runtime.Patterns.Service
{
    /// <summary>
    /// Attribute for limit usage of MonoBehaviourService-classes at specified scenes only.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public sealed class MonoBehaviourServiceFilterAttribute : Attribute
    {
        public string Name;

        public MonoBehaviourServiceFilterAttribute(string name)
        {
            Name = name;
        }
    }
}