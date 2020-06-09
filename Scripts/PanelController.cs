using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PanelController : MonoBehaviour
{
    [SerializeField] Slider AILevelLeft;
    [SerializeField] Slider AILevelRight;
    [SerializeField] BotMoveRoutine leftBot;
    [SerializeField] BotMoveRoutine rightBot;
    [SerializeField] SpeedEffectController speedEffectController;
    [SerializeField] NewController player;
    [SerializeField] NewController botLeft;
    [SerializeField] NewController botRight;
    public void ChangeLeft()
    {
       int level = (int)AILevelLeft.value;
        leftBot.desicionSuccess = level;
    }
    public void ChangeRight()
    {
        int level = (int)AILevelRight.value;
        rightBot.desicionSuccess = level;
    }
    public void ChangeEffect()
    {
        speedEffectController.EffectEnabled = !speedEffectController.EffectEnabled;
    }

    public void Quit()
    {
       gameObject.SetActive(false);
       player.playGame = true;
       botLeft.playGame = true;
       botRight.playGame = true;
    }
    public void Open()
    {
        gameObject.SetActive(true);
        player.playGame = false;
        botLeft.playGame = false;
        botRight.playGame = false;
    }
}
