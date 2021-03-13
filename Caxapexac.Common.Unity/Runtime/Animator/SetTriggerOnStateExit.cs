// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using UnityEngine;


namespace Caxapexac.Common.Sharp.Runtime.Animator
{
    /// <summary>
    /// Set Animator trigger parameter to new state on node exit.
    /// </summary>
    public sealed class SetTriggerOnStateExit : StateMachineBehaviour
    {
        [SerializeField]
        private string TriggerName = "";

        [SerializeField]
        private bool TriggerValue = false;

        private int _fieldHash = -1;

        public override void OnStateExit(UnityEngine.Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            if (_fieldHash == -1)
            {
#if UNITY_EDITOR
                if (string.IsNullOrEmpty(TriggerName))
                {
                    Debug.LogWarning("Trigger field name is empty", animator);
                    return;
                }
#endif
                _fieldHash = UnityEngine.Animator.StringToHash(TriggerName);
            }

            if (TriggerValue)
            {
                animator.SetTrigger(_fieldHash);
            }
            else
            {
                animator.ResetTrigger(_fieldHash);
            }
        }
    }
}