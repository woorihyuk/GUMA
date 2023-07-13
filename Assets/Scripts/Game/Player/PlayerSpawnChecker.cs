using System;
using UnityEngine;

namespace Game.Player
{
    public class PlayerSpawnChecker : MonoBehaviour
    {
        private void Awake()
        {
            if (FindAnyObjectByType<Player>() == null)
            {
                Instantiate(Resources.Load<Player>("Player"));
            }
        }
    }
}
