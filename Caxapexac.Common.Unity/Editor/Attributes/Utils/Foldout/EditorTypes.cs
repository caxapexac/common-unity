using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Caxapexac.Common.Sharp.Editor.Attributes.Utils;


namespace MyBox.Internal
{
    internal static class EditorTypes
    {
        public static Dictionary<int, List<FieldInfo>> fields = new Dictionary<int, List<FieldInfo>>(FastComparable.Default);

        public static int Get(object target, out List<FieldInfo> objectFields)
        {
            var t = target.GetType();
            var hash = t.GetHashCode();

            if (!fields.TryGetValue(hash, out objectFields))
            {
                var typeTree = t.GetTypeTree();
                objectFields = target.GetType()
                    .GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.NonPublic)
                    .OrderByDescending(x => typeTree.IndexOf(x.DeclaringType))
                    .ToList();
                fields.Add(hash, objectFields);
            }

            return objectFields.Count;
        }
    }
}