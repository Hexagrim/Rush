using UnityEngine;
using Unity.Cinemachine;

public class CamShake : MonoBehaviour
{
    CinemachineCamera cam;
    CinemachineBasicMultiChannelPerlin noise;
    float shakeTimer;
    float shakeTimerTotal;
    void Awake()
    {
        cam = GetComponent<CinemachineCamera>();
        noise = cam.GetCinemachineComponent(CinemachineCore.Stage.Noise) as CinemachineBasicMultiChannelPerlin;
    }
    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;

            if (shakeTimer <= 0f)
            {
                noise.AmplitudeGain = 0f;
                noise.FrequencyGain = 0f;
            }
        }
    }
    public void ShakeCam(float amplitude, float frequency, float time)
    {
        noise.AmplitudeGain = amplitude;
        noise.FrequencyGain = frequency;

        shakeTimer = time;
        shakeTimerTotal = time;
    }
}
