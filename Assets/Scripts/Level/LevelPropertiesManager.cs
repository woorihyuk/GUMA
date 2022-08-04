using System;
using Camera;
using UnityEngine;

namespace Level
{
    public class LevelPropertiesManager : MonoBehaviour
    {
        [Header("Camera", order = 3)]
        public Vector2 maxPosition;
        public Vector2 minPosition;
        public bool lockToPlayer = true;

        private TargetCamera _targetCamera;
        private Player _player;
        
        private void Start()
        {
            _targetCamera = FindObjectOfType<TargetCamera>();
            _player = FindObjectOfType<Player>();
        }

        private void Update()
        {
            if (lockToPlayer) _targetCamera.UpdatePositionToTarget(_player.transform.position, maxPosition, minPosition);
        }
    }
}