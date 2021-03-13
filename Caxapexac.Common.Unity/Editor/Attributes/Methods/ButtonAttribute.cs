// ---------------------------------------------------------------------------- 
// Author: Kaynn, Yeo Wen Qin
// https://github.com/Kaynn-Cahya
// Date:   26/02/2019
// ----------------------------------------------------------------------------

using System;
using UnityEngine;


namespace MyBox
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonAttribute : PropertyAttribute
    {
        public readonly ButtonMethodDrawOrder DrawOrder;

        public ButtonAttribute(ButtonMethodDrawOrder drawOrder = ButtonMethodDrawOrder.AfterInspector)
        {
            DrawOrder = drawOrder;
        }
    }


    public enum ButtonMethodDrawOrder
    {
        BeforeInspector,
        AfterInspector
    }
}