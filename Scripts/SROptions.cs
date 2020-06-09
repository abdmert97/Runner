using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;

public partial class SROptions
{

    [Category("Game Settings")]
    public bool SpeedEffectEnabled
    {
        get => GameManager.instance.speedEffectController.EffectEnabled;
        set => GameManager.instance.speedEffectController.EffectEnabled = value;
    }

    [Category("Game Settings")]
    public bool BoosterEnabled
    {
        get => GameManager.instance.IsBoostEnabled;
        set => GameManager.instance.SwitchCollectables();
    }

    [Category("Player Settings")]
    public float PlayerDefaultSpeed
    {
        get { return GameManager.instance.playerSettings.DefaultSpeed; }
        set { GameManager.instance.playerSettings.DefaultSpeed = value; }
    }

    [Category("Player Settings")]
    public float PlayerMaxSpeed
    {
        get { return GameManager.instance.playerSettings.MaxSpeed; }
        set { GameManager.instance.playerSettings.MaxSpeed = value; }
    }
    [Category("Player Settings")]
    public float PlayerMinSpeed
    {
        get { return GameManager.instance.playerSettings.MinSpeed; }
        set { GameManager.instance.playerSettings.MinSpeed = value; }
    }
    [Category("Player Settings")]
    public float PlayerMaxRunAnimationSpeed
    {
        get { return GameManager.instance.playerSettings.MaxRunAnimationSpeed; }
        set { GameManager.instance.playerSettings.MaxRunAnimationSpeed = value; }
    }
    [Category("Player Settings")]
    public float PlayerMinRunAnimationSpeed
    {
        get { return GameManager.instance.playerSettings.MinRunAnimationSpeed; }
        set { GameManager.instance.playerSettings.MinRunAnimationSpeed = value; }
    }
    [Category("Player Settings")]
    public float PlayerSwitchVerticalDistance
    {
        get { return GameManager.instance.playerSettings.SwitchVerticalDistance; }
        set { GameManager.instance.playerSettings.SwitchVerticalDistance = value; }
    }
    [Category("Player Settings")]
    public float PlayerSwitchHorizontalDistance
    {
        get { return GameManager.instance.playerSettings.SwitchHorizontalDistance; }
        set { GameManager.instance.playerSettings.SwitchHorizontalDistance = value; }
    }

    [Category("AI Settings")]
    [NumberRange(0, 100)]
    public int Ai1Power
    {
        get => GameManager.instance.botControllers[0].GetComponent<BotMoveRoutine>().desicionSuccess;
        set => GameManager.instance.botControllers[0].GetComponent<BotMoveRoutine>().desicionSuccess = value;
    }

    [Category("AI Settings")]
    [NumberRange(0, 100)]
    public int Ai2Power
    {
        get => GameManager.instance.botControllers[1].GetComponent<BotMoveRoutine>().desicionSuccess;
        set => GameManager.instance.botControllers[1].GetComponent<BotMoveRoutine>().desicionSuccess = value;
    }

    [Category("Level Management")]
    public void NextLevel()
    {
        GameManager.instance.NextLevel();
    }

    [Category("Level Management")]
    public void RestartLevel()
    {
        GameManager.instance.RestartLevel();
    }
}
