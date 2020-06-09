using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGame : MonoBehaviour
{
    [SerializeField] NewController player;
    [SerializeField] NewController botLeft;
    [SerializeField] NewController botRight;
    [SerializeField] BotMoveRoutine moveLeft;
    [SerializeField] BotMoveRoutine moveRight;
    int finishRank = 1;




    private void OnTriggerEnter(Collider other)
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<NewController>();
        GameObject[] bots = GameObject.FindGameObjectsWithTag("Bot");
        botRight = bots[0].GetComponent<NewController>();
        botLeft = bots[1].GetComponent<NewController>();
        moveLeft = bots[0].GetComponent<BotMoveRoutine>();
        moveRight = bots[1].GetComponent<BotMoveRoutine>();
        if (finishRank == 1)
        {
            NewController cont = other.GetComponent<NewController>();
            other.GetComponent<Animator>().Play("TurnAround");
            other.GetComponent<Animator>().SetBool("Win",true);
            cont.playGame = false;
            cont.isRunning = false;
            Vector3 pos = other.gameObject.transform.position;
            other.gameObject.transform.position = new Vector3(pos.x,cont.roadOffset.y,pos.z);

        }
        else
        {
            other.GetComponent<Animator>().Play("TurnAround");
            other.GetComponent<Animator>().SetBool("Win", false);
           
            other.GetComponent<NewController>().playGame = false;
            other.GetComponent<NewController>().isRunning = false;


        }
        finishRank++;

        if (other.CompareTag("Player"))
        {
            Invoke("StartGame", 6f);
          
        }
    }


    void StartGame()
    {
        player.Restart();
        botLeft.Restart();
        botRight.Restart();
        moveLeft.SetStartLine();
        moveRight.SetStartLine();
        finishRank = 1;

    }
}
