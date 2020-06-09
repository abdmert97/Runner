using MapGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject[] LevelList;
    int level = 0;
    [SerializeField] GameManager gameManager;

    [SerializeField] MapGenerator mapGenerator;
    [SerializeField] Transform waterTransform;

    private void Awake()
    {
        UpdateMapGeneratorHeight();
    }

    public void NextLevel()
    {
        gameManager.SwitchCollectables();
        gameManager.Collectables = null;
        int length = LevelList.Length;
        for (int i = 0; i < length; i++)
            LevelList[i].SetActive(false);
        level++;
        level =level % length;
        LevelList[level].SetActive(true);

        UpdateMapGeneratorHeight();
    }

    private void UpdateMapGeneratorHeight()
    {
        if (level == 0)
        {
            Vector3 mapGeneratorPosition = mapGenerator.transform.position;
            mapGeneratorPosition.y = -17.5f;
            mapGenerator.transform.position = mapGeneratorPosition;
        }
        else if (level == 1)
        {
            Vector3 mapGeneratorPosition = mapGenerator.transform.position;
            mapGeneratorPosition.y = -10.5f;
            mapGenerator.transform.position = mapGeneratorPosition;
        }

        Vector3 waterPosition = waterTransform.position;
        waterPosition.y = mapGenerator.transform.position.y - 7.5f;
        waterTransform.transform.position = waterPosition;
    }
}
