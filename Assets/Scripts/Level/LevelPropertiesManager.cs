using System;
using Cinemachine;
using UnityEngine;

public class LevelPropertiesManager : MonoBehaviour
{
    public static LevelPropertiesManager Instance;
    
    public Transform[] positionSpots;
    public Collider2D playerCamCollider;
    public CinemachineVirtualCamera playerCam;

    private void Awake()
    {
        Instance = this;
    }

    public bool TryGetPositionOfLevel(out Vector3 position)
    {
        print(GameManager.Instance.positionFlags);
        if (GameManager.Instance.positionFlags != -1)
        {
            position = positionSpots[GameManager.Instance.positionFlags].position;
            return true;
        }

        position = Vector3.zero;
        return false;
    }
}