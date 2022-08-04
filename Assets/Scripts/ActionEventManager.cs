using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class ActionEventManager : MonoBehaviour
{
    public static ActionEventManager Instance;

    private CinemachineVirtualCamera _vCam01;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void CloseUpToPlayer()
    {
        if (_vCam01 == null) _vCam01 = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        DOVirtual.Float(_vCam01.m_Lens.OrthographicSize, 5, 0.5f, value => _vCam01.m_Lens.OrthographicSize = value);
    }
    
    public void ResetCamSize()
    {
        if (_vCam01 == null) _vCam01 = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        DOVirtual.Float(_vCam01.m_Lens.OrthographicSize, 7.5f, 0.5f, value => _vCam01.m_Lens.OrthographicSize = value);
    }
}