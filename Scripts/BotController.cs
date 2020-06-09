using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour
{
    [SerializeField] Vector3 overlapSize;
    [SerializeField] float actionDistance;
     NewController playerController;
    [SerializeField] string[] obstacleTag;
    public List<GameObject> obstacles = new List<GameObject>();
    public int[] collectables = new int[3];
    public  enum DesicionType{LEFT,RIGHT,JUMP,SLIDE,RUN};
    public  DesicionType currentDesicion = DesicionType.RUN;
    string left = "Left";
    string right = "Right";
    string collectableString = "Collectable";
    string Zipline = "Zipline";
    public  int line;
    float roadSize;
    public bool []safeLine = { true,true,true};
    
    // Start is called before the first frame update
    void Awake()
    {
        playerController = GetComponentInParent<NewController>();
        roadSize = playerController.playerSettings.RoadSize;
    }

    // Update is called once per frame
    public DesicionType GetDesicion(int desicionLevel)
    {
        Collider[] colliders = Overlap(transform.position);
        line = playerController.GetCharLine();
        obstacles.Clear();


        CreateObstacleList(colliders, line);
        obstacles.Sort(SortByXPosition);
        Decide();
        int success = Random.Range(0, 100);
        if(success>desicionLevel&&currentDesicion!=DesicionType.RUN)
        {
            ChangeDesicion();
        }
  
        return currentDesicion;
    }
    private void ChangeDesicion()
    {
        int x = (int)currentDesicion;
        int rand = Random.Range(0, 2);
        if(rand == 2 )
        {
            currentDesicion = DesicionType.RUN;
        }
        else
             currentDesicion = (DesicionType)(Random.Range(0,4));
    }
    private void Decide()
    {
        bool selected = false;
        currentDesicion = DesicionType.RUN;
        int length = obstacles.Count;
        safeLine[0] = true;
        safeLine[1] = true;
        safeLine[2] = true;
        int selectedLine = line;

        for (int i = 0; i < length; i++)
        {
            //    Debug.Log(transform.parent.name + " " + obstacles[i].tag);
            int obsLine = GetLineFromPosition(obstacles[i]);
            if (obsLine == line)
            {
                if (!selected && obstacles[i].transform.position.z - transform.position.z < actionDistance)
                {
                    if (obstacles[i].CompareTag(obstacleTag[1])|| obstacles[i].CompareTag(Zipline) || obstacles[i].CompareTag(obstacleTag[5])) // JUMP
                    {
                        selected = true;
                        currentDesicion = DesicionType.JUMP;
                        break;
                    }
                    if (obstacles[i].CompareTag(obstacleTag[2])) // Slide
                    {
                        selected = true;
                        currentDesicion = DesicionType.SLIDE;
                        break;
                    }
                    if (obstacles[i].CompareTag(obstacleTag[3])) // JumpOrSlide
                    {
                        selected = true;
                        if (Random.Range(0, 2) == 0)
                            currentDesicion = DesicionType.JUMP;
                        else
                            currentDesicion = DesicionType.SLIDE;
                    }
                }

            }
            else
                safeLine[obsLine + 1] = false;
        }

        selectedLine = FindSelectedLine(selectedLine);
        if (currentDesicion != DesicionType.RUN)
            for (int j = 0; j < 3; j++)
            {
                if (safeLine[j])
                {
                    int random = Random.Range(0, 2);
                    if (random == 1)
                    {

                        selectedLine = j - 1;
                        break;
                    }
                }
            }
        if (Mathf.Abs(selectedLine - line) == 1)
        {
            if (selectedLine > line)
            {
                currentDesicion = DesicionType.RIGHT;
            }
            else
                currentDesicion = DesicionType.LEFT;
        }
    }

    private int FindSelectedLine(int selectedLine)
    {
        int maxCollectable = 0;
        int valuableLine = -2;
        for (int i = 0; i < 3; i++)
        {
            if (maxCollectable < collectables[i]&&(safeLine[i]||collectables[i]==10))
            {
                valuableLine = i;
                maxCollectable = collectables[i];

            }
        }
        selectedLine = valuableLine == -2 ? selectedLine : valuableLine - 1;
        return selectedLine;
    }

    int SortByXPosition(GameObject o1,GameObject o2)
    {
        return o1.transform.position.x.CompareTo(o2.transform.position.x);
    }
    private void CreateObstacleList(Collider [] colliders,int line)
    {
    
        int length = colliders.Length;
        int tagLength = obstacleTag.Length;
        collectables[0] = 0;
        collectables[1] = 0;
        collectables[2] = 0;
        for (int i = 0; i < length; i++)
        {
            for(int j = 0; j < tagLength;j++)
            {
                if(colliders[i].CompareTag(obstacleTag[j]))
                {
                    //Obstacle Detected
                    obstacles.Add(colliders[i].gameObject);
                  int obsline = GetLineFromPosition(colliders[i].gameObject);
                   
                 
                }
               
            }
            if (colliders[i].CompareTag(collectableString))
            {
                int colline = GetLineFromPosition(colliders[i].gameObject);
               
               collectables[colline+1]++;
            }
            if (colliders[i].CompareTag(Zipline))
            {
                int colline = GetLineFromPosition(colliders[i].gameObject);

                collectables[colline + 1]+=10;
            }
        }

    }
    int GetLineFromPosition(GameObject obj)
    {
        if (Mathf.Abs(obj.transform.position.x) < roadSize / 2)
            return 0;
        if (obj.transform.position.x < 0)
            return -1;
        return 1;

    }
    private void OnDrawGizmosSelected()
    {
  
        Gizmos.DrawWireCube(transform.position+Vector3.forward *overlapSize.z/2,overlapSize); 
    }
    private Collider[] Overlap(Vector3 center)
    {
        Collider[] Colliders = Physics.OverlapBox(transform.position + Vector3.forward * overlapSize.z / 2, overlapSize/2);
      
        return Colliders;
    }
    private int GetLine(GameObject obj)
    {
        if(obj.transform.parent.name.Contains(left))
        {
            return -1;
        }
        if (obj.transform.parent.name.Contains(right))
        {
            return 1;
        }

        return 0;
    }
}
