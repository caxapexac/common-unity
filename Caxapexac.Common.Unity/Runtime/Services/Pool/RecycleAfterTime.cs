// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using UnityEngine;


namespace Caxapexac.Common.Sharp.Runtime.Services.Pool
{
    /// <summary>
    /// Recycle instance after time (if it was spawned from pool).
    /// Dont use it on swarm spawns of prefab (use custom recycling instead).
    /// </summary>
    public sealed class RecycleAfterTime : MonoBehaviour
    {
        [SerializeField]
        private float _timeout = 1f;

        private float _endTime;

        private void OnEnable()
        {
            _endTime = Time.time + _timeout;
        }

        private void LateUpdate()
        {
            if (Time.time >= _endTime)
            {
                OnRecycle();
            }
        }

        private void OnRecycle()
        {
            var po = GetComponent<IPoolObject>();
            if ((object)po != null)
            {
                po.PoolRecycle();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}