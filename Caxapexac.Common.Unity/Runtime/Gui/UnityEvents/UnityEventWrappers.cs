using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;


namespace Caxapexac.Common.Sharp.Runtime.Gui.UnityEvents
{
    /// <summary>
    /// Because generic UnityEvent won't show in inspector 
    /// </summary>
    [Serializable]
    public class UnityEventInt : UnityEvent<int>
    {
    }


    [Serializable]
    public class UnityEventFloat : UnityEvent<float>
    {
    }


    [Serializable]
    public class UnityEventString : UnityEvent<string>
    {
    }


    [Serializable]
    public class UnityEventPointerEventData : UnityEvent<PointerEventData>
    {
    }
}