using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpeedController : MonoBehaviour
{
    [SerializeField] NewController  playerController;
    [SerializeField] Image SpeedBar;
    float minSpeed;
    float maxSpeed;
    private void Start()
    {
        minSpeed = playerController.playerSettings.MinSpeed;
        maxSpeed = playerController.playerSettings.MaxSpeed;
    }

    private void Update()
    {
        if(SpeedBar)
        DisplaySpeed();   
    }
    public void DisplaySpeed()
    {
        float currentSpeed = playerController.GetSpeed();
        SpeedBar.fillAmount = currentSpeed / maxSpeed;

    }
    public void IncreaseSpeed(float amount=0.75f)
    {
        playerController.IncreaseSpeed(amount);
        float currentSpeed = playerController.GetSpeed();
        if(SpeedBar)    
        SpeedBar.fillAmount = currentSpeed/maxSpeed;
    }
}
