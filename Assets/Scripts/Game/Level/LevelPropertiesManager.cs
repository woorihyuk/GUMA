using Cinemachine;
using UnityEngine;

namespace Game.Level
{
    public class LevelPropertiesManager : MonoBehaviour
    {
        public static LevelPropertiesManager Instance;
        public Transform[] positionSpots;
        public Transform[] savePoints;
        public CinemachineVirtualCamera playerCam;

        private void Awake()
        {
            Instance = this;
        }

        public bool TryGetPositionOfLevel(out Vector3 position)
        {
            if (GameManager.Instance.positionFlags != -1)
            {
                position = positionSpots[GameManager.Instance.positionFlags].position;
                return true;
            }

            position = Vector3.zero;
            return false;
        }
    }
}