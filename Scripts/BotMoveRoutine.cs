using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotMoveRoutine : MonoBehaviour
{
    NewController pController;
    BotController botController;
    [SerializeField] int startLine;
    public int desicionSuccess;
    public Vector3[] moveList = {Vector3.left,Vector3.right,Vector3.up,Vector3.down*5,Vector3.zero };
    // Start is called before the first frame update
    void Start()
    {

        botController = GetComponent<BotController>();
        pController = GetComponent<NewController>();

        if(startLine == -1)
        {
            transform.position -= Vector3.right * pController.playerSettings.RoadSize;
            pController.characterHorizontalOffset = -pController.playerSettings.RoadSize;
        }
        if (startLine == 1)
        {
            transform.position += Vector3.right * pController.playerSettings.RoadSize;
            pController.characterHorizontalOffset = pController.playerSettings.RoadSize;
        }
    }
    public void SetStartLine()
    {
        if (startLine == -1)
        {
            if (pController == null)
            {          
                pController = GetComponent<NewController>();
            }
            pController.characterHorizontalOffset = -pController.playerSettings.RoadSize;
            
        }
        if (startLine == 1)
        {
            pController.characterHorizontalOffset = pController.playerSettings.RoadSize;
        }
    }



    public Vector3 SetBotPosition()
    {
            BotController.DesicionType desicion = botController.GetDesicion(desicionSuccess);         
            switch (desicion)
            {
            case BotController.DesicionType.LEFT:
                return moveList[0];
            case BotController.DesicionType.RIGHT:
                return moveList[1];
            case BotController.DesicionType.JUMP:
                    return moveList[2];
            case BotController.DesicionType.SLIDE:                
                    return moveList[3]; 
            case BotController.DesicionType.RUN:
                    return moveList[4];
            }
        return moveList[4];
    }
  
}
