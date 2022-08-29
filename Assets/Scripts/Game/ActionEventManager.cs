using Cinemachine;
using DG.Tweening;
using Game;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class ActionEventManager : PrefabSingleton<ActionEventManager>
{
    private CinemachineVirtualCamera _vCam01;
    private CinemachineFramingTransposer _vCam01Framing;
    private CinemachineImpulseSource _impulseSource;
    private Transform _eggObj;

    protected override void Awake()
    {
        base.Awake();
        _vCam01 = LevelPropertiesManager.Instance.playerCam;
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _vCam01Framing = _vCam01.GetCinemachineComponent<CinemachineFramingTransposer>();
        _eggObj = GameObject.Find("dinamic_egg_0").transform;
    }

    public void CloseUpToPlayer(float duration)
    {
        DOVirtual.Float(_vCam01Framing.m_TrackedObjectOffset.z, 1.25f, duration, value => _vCam01Framing.m_TrackedObjectOffset.z = value)
            .SetEase(Ease.Linear);
        // DOVirtual.Float(_vCam01.m_Lens.OrthographicSize, 5, 0.5f, value => _vCam01.m_Lens.OrthographicSize = value);
    }
    
    public void ResetCamSize(float duration)
    {
        DOVirtual.Float(_vCam01Framing.m_TrackedObjectOffset.z, -10, duration, value => _vCam01Framing.m_TrackedObjectOffset.z = value).SetEase(Ease.Linear);
        // DOVirtual.Float(_vCam01.m_Lens.OrthographicSize, 7.5f, 0.5f, value => _vCam01.m_Lens.OrthographicSize = value);
    }

    public void ImpulseRandom()
    {
        _impulseSource.GenerateImpulse(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
    }

    public void CloseToEgg()
    {
        _eggObj.GetComponent<Animator>().enabled = true;
        _vCam01.gameObject.SetActive(false);
    }

    public void FadeInImage()
    {
        GameUIManager.Instance.endCg.gameObject.SetActive(true);
        GameUIManager.Instance.endCg.alpha = 0;
        GameUIManager.Instance.endCg.interactable = false;
        GameUIManager.Instance.endCg.DOFade(1, 2)
            .OnComplete(() =>
            {
                GameUIManager.Instance.endCg.interactable = true;
            }).SetUpdate(true).Play();
    }
}