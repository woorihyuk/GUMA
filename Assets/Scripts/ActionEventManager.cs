using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class ActionEventManager : MonoBehaviour
{
    public static ActionEventManager Instance;

    private CinemachineVirtualCamera _vCam01;
    private CinemachineCameraOffset _vCam01Offset;
    public CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void CloseUpToPlayer(float duration)
    {
        if (!_vCam01) _vCam01 = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        if (!_vCam01Offset) _vCam01Offset = _vCam01.GetComponent<CinemachineCameraOffset>();
        DOVirtual.Float(_vCam01Offset.m_Offset.z, 1.25f, duration, value => _vCam01Offset.m_Offset.z = value)
            .SetEase(Ease.Linear);
        // DOVirtual.Float(_vCam01.m_Lens.OrthographicSize, 5, 0.5f, value => _vCam01.m_Lens.OrthographicSize = value);
    }
    
    public void ResetCamSize(float duration)
    {
        if (!_vCam01) _vCam01 = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        if (!_vCam01Offset) _vCam01Offset = _vCam01.GetComponent<CinemachineCameraOffset>();
        DOVirtual.Float(_vCam01Offset.m_Offset.z, -10, duration, value => _vCam01Offset.m_Offset.z = value).SetEase(Ease.Linear);
        // DOVirtual.Float(_vCam01.m_Lens.OrthographicSize, 7.5f, 0.5f, value => _vCam01.m_Lens.OrthographicSize = value);
    }

    public void ImpulseRandom()
    {
        impulseSource.GenerateImpulse(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
    }
}