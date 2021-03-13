// ---------------------------------------------------------------------------- 
// Author: Dimitry, PixeyeHQ
// Project : UNITY FOLDOUT
// https://github.com/PixeyeHQ/InspectorFoldoutGroup
// Contacts : Pix - ask@pixeye.games
// Website : http://www.pixeye.games
// ----------------------------------------------------------------------------

using System;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Editor.Attributes.FieldsAccessibility
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SpoilerAttribute : PropertyAttribute
    {
        public readonly string name;
        public readonly bool foldEverything;

        /// <summary>Adds the property to the specified foldout group.</summary>
        /// <param name="name">Name of the foldout group.</param>
        /// <param name="foldEverything">Toggle to put all properties to the specified group</param>
        public SpoilerAttribute(string name, bool foldEverything = false)
        {
            this.foldEverything = foldEverything;
            this.name = name;
        }
    }
}