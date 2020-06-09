using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZiplineEnter : MonoBehaviour
{

  
    enum ZipType { ENTER,EXIT};
    CamFollow   camFollow;
    NewController controller;
    [SerializeField] ZipType zipType;
    [SerializeField] Transform zipEnd;
    [SerializeField] Transform zipStart;
    [SerializeField] Vector3 Angle;
    [SerializeField] Vector3 Distance;
    Vector3 defaultAngle;
    Vector3 defaultDistance;
   
    bool zip = false;
    Vector3 distanceZip;
    float tangent;

    // 2 sininde girme durumunu düzenle !!!!!!
    private void Awake()
    {
      
        camFollow = Camera.main.GetComponent<CamFollow>();
        tangent = Mathf.Tan(11.3f * Mathf.Deg2Rad);
 
    }
    private void Start()
    {
        if (zipType == ZipType.ENTER)
        {
            distanceZip = zipEnd.position - zipStart.position;
 
        }
      
        defaultDistance = camFollow.distance;
        defaultAngle = camFollow.angle;
    }
    private void OnTriggerEnter(Collider other)
    {
        controller = other.GetComponent<NewController>();
      
       
        if (zipType == ZipType.EXIT)
        {
            if (controller.isZipline)
                controller.anim.Play("ZipEnd");

            Invoke("ZipExit", .3f);
            if (controller.apart)
                controller.apart.gameObject.SetActive(false);
        }
      
    }

    private void OnTriggerStay(Collider other)
    {
      // if (controller.GetUserType() == NewController.UserType.BOT)
       //     return;
        if (zip|| zipType == ZipType.EXIT) return;
        float currentDistance = zipEnd.position.z - other.transform.position.z;

        float posx = zipEnd.position.y + currentDistance * tangent;
        if (posx - other.transform.position.y-0.385f>=0&& posx - other.transform.position.y-0.385f<0.1f)
        {
            controller.characterVerticalOffset = 0;
             
            Vector3 oldPos = other.gameObject.transform.position;
           // other.gameObject.transform.position = new Vector3(oldPos.x, posx - other.transform.position.y - 0.385f, oldPos.z);
            zip = true;

            /* if (coll.transform.position.z + 2.5f < other.transform.position.z)
              {
                  Debug.Log(coll.transform.position.z + " " + other.transform.position.z);
                  Invoke("ZipExit", .3f);
                  return;
              }
              */
            // Bot için düzelt
            Invoke("OpenApart", 0.25f);
            if (controller.GetUserType() == NewController.UserType.HUMAN)
            {
                Invoke("CamChange", 0.1f);
                controller.speedEffect.Clear();
                controller.speedEffect.Play();
            }
                controller.isZipline = true;
                controller.anim.Play("ZipEnter");
                controller.IncreaseSpeed(3);
                Vector3 pos = controller.gameObject.transform.position;
                controller.characterVerticalOffset = 0;
                controller.fixedMove = Quaternion.Euler(Vector3.right * 11.3f) * ((Vector3.forward * Time.fixedDeltaTime * 9));
           
        
        }
    


    }
    private void OnTriggerExit(Collider other)
    {
        if (zipType == ZipType.ENTER&&controller.GetUserType() == NewController.UserType.HUMAN&&controller.isZipline == false)
        {
            Invoke("ZipExit", .1f);

        }
       
        zip = false;
    }

    private void OpenApart()
    {

        if(controller.apart)
        controller.apart.gameObject.SetActive(true);
    }
    private void ZipExit()
    {
        controller.isZipline = false;
        if (controller.GetUserType() == NewController.UserType.HUMAN && controller.speedEffect.isPlaying)
        {
            controller.speedEffect.Stop();
            camFollow.distance = defaultDistance;
            camFollow.angle = defaultAngle;
        }
 

    }
    private void CamChange()
    {
        if (controller.GetUserType() == NewController.UserType.HUMAN)
        {
            camFollow.distance = Distance;
            camFollow.angle = Angle;
        }
      //  StartCoroutine(VectorLerp(camFollow.distance, Distance,20));
      //  StartCoroutine(AngleLerp(camFollow.angle, Angle, 20));

    }
    IEnumerator VectorLerp( Vector3 from,Vector3 to,int time)
    {
        Vector3 dist = (to - from) / time;
        for(int i = 0; i< time;i++)
        {

            camFollow.distance +=dist;
            yield return new WaitForFixedUpdate();

        }
    }
    IEnumerator AngleLerp(Vector3 from, Vector3 to, int time)
    {
        Vector3 dist = (to - from) / time;
        for (int i = 0; i < time; i++)
        {

            camFollow.angle += dist;
            yield return new WaitForFixedUpdate();

        }
    }
}
