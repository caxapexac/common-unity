using System;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Editor.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class RegexAttribute : PropertyAttribute
    {
        public readonly string pattern;
        public readonly string helpMessage;

        public RegexAttribute(string pattern, string helpMessage)
        {
            this.pattern = pattern;
            this.helpMessage = helpMessage;
        }
    }
}