using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class LightFlicker : MonoBehaviour
{
    public float minIntensity = 0.25f;
    public float maxIntensity = 0.5f;
    //private float _random;
    private Light2D _light;

    public float accelerateTime = 0.15f;
    private float _targetIntensity = 1.0f;
    private float _lastIntensity = 1.0f;
    private float _timePassed;
    private const double Tolerance = 0.0001;

    private void Start()
    {
        _light = GetComponent<Light2D>();
        _lastIntensity = _light.intensity;
        // _random = Random.Range(0.0f, 65535.0f);
    }

    private void Update()
    {
        _timePassed += Time.deltaTime;
        _light.intensity = Mathf.Lerp(_lastIntensity, _targetIntensity, _timePassed/accelerateTime);
 
        if (Mathf.Abs(_light.intensity - _targetIntensity) < Tolerance) {
            _lastIntensity = _light.intensity;
            _targetIntensity = Random.Range(minIntensity, maxIntensity);
            _timePassed = 0.0f;
        }
        
        /*var noise = Mathf.PerlinNoise(_random, Time.time);
        _light.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);*/
    }
}