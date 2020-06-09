using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public LevelManager levelManager;
    public GameObject[] Collectables;
    public NewController playerController;
    public NewController[] botControllers;
    public GameObject SpeedBar;
    public PSettings playerSettings;

    public SpeedEffectController speedEffectController;
    public bool IsBoostEnabled { get; private set; } = true;
    private void Awake()
    {
        instance = this;
      
        playerSettings = Instantiate(playerSettings);

        playerController.playerSettings = playerSettings;

        for (int i = 0; i < botControllers.Length; i++)
        {
            botControllers[i].playerSettings = playerSettings;
        }
    }

    private void Start()
    {
        SRDebug.Instance.PanelVisibilityChanged += visible => { Time.timeScale = visible ? 0 : 1; };
    }

    public void SwitchCollectables()
    {
        if (IsBoostEnabled)
        {
            Collectables = GameObject.FindGameObjectsWithTag("Collectable");
            IsBoostEnabled = false;
            for (int i = 0; i < Collectables.Length; i++)
            {
                Collectables[i].SetActive(false);
            }
            SpeedBar.SetActive(false);
        }
        else
        {
            for (int i = 0; i < Collectables.Length; i++)
            {

                Collectables[i].SetActive(true);
            }
            IsBoostEnabled = true;
            SpeedBar.SetActive(true);
        }
    }

    public void RestartLevel()
    {
        playerController.Restart();
        
        for(int i = 0; i < botControllers.Length; i++)
        {
            botControllers[i].Restart();

            var botMoveRoutine = botControllers[i].gameObject.GetComponent<BotMoveRoutine>();
            botMoveRoutine.SetStartLine();
        }
    }

    public void NextLevel()
    {
        RestartLevel();
        levelManager.NextLevel();
    }
}
