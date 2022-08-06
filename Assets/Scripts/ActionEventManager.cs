﻿using Cinemachine;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class ActionEventManager : PrefabSingleton<ActionEventManager>
{
    private CinemachineVirtualCamera _vCam01;
    private CinemachineFramingTransposer _vCam01Framing;
    private CinemachineImpulseSource _impulseSource;

    protected override void Awake()
    {
        base.Awake();
        _vCam01 = LevelPropertiesManager.Instance.playerCam;
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _vCam01Framing = _vCam01.GetCinemachineComponent<CinemachineFramingTransposer>();
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
}