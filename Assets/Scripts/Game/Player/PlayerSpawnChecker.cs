using Game.Level;
using UnityEngine;

namespace Game.Player
{
    public class PlayerSpawnChecker : MonoBehaviour
    {
        private void Awake()
        {
            var player = FindAnyObjectByType<Player>();
            if (player == null)
            {
                player = Instantiate(Resources.Load<Player>("Player"));
            }

            LevelPropertiesManager.Instance.playerCam.Follow = player.transform;
            player.SetPositionFromLevelProperties();
        }
    }
}
