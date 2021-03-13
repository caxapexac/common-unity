// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using UnityEngine;


namespace Caxapexac.Common.Sharp.Runtime.Animator
{
    /// <summary>
    /// Set Animator bool parameter to new state on node exit.
    /// </summary>
    public sealed class SetBoolOnStateExit : StateMachineBehaviour
    {
        [SerializeField]
        private string BoolName = "";

        [SerializeField]
        private bool BoolValue = false;

        private int _fieldHash = -1;

        public override void OnStateExit(UnityEngine.Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            if (_fieldHash == -1)
            {
#if UNITY_EDITOR
                if (string.IsNullOrEmpty(BoolName))
                {
                    Debug.LogWarning("Bool field name is empty", animator);
                    return;
                }
#endif
                _fieldHash = UnityEngine.Animator.StringToHash(BoolName);
            }
            animator.SetBool(_fieldHash, BoolValue);
        }
    }
}