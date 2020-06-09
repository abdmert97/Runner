using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedEffectController : MonoBehaviour
{
    [SerializeField] ParticleSystem speedEffect;
    public bool EffectEnabled = true;
    public float time = 0;
    float maxTime = .75f;
    float stopTime = 0;
    bool enabledEffect = false;
    private void Update()
    {
        if(!EffectEnabled)
        {
            if (enabledEffect)
            {
                enabledEffect = false;
                if (speedEffect.isPlaying)
                    speedEffect.Stop();
            }
            else
                return;
        }
        if (time > 0&&EffectEnabled)
        {
            if (stopTime > 0.25f)
                speedEffect.Clear();
            enabledEffect = true;
            time -= Time.deltaTime;
            if(speedEffect.isStopped)
            speedEffect.Play();
            stopTime = 0;

        }
        else if(enabledEffect)
        {
            
            enabledEffect = false;
            if (speedEffect.isPlaying)
                speedEffect.Stop();

        }
        else
        {
            stopTime += Time.deltaTime;
        }
    }
    public void IncreaseTime(float duration = 1)
    {
        time += duration;
        if (time >= maxTime)
            time = maxTime;
    }
}
