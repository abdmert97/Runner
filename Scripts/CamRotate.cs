using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Vector3 Angle;
    [SerializeField] Vector3 Distance;
    Vector3 defaultAngle;
    Vector3 defaultDistance;
    CamFollow camFollow;
    bool first = true;
    bool manuel = false;
    private void Awake()
    {

        camFollow = Camera.main.GetComponent<CamFollow>();
        defaultDistance = camFollow.distance;
        defaultAngle = camFollow.angle;

    }

    IEnumerator VectorLerp(Vector3 from, Vector3 to, int time)
    {
        Vector3 dist = (to - from) / time;
        for (int i = 0; i < time; i++)
        {

            camFollow.distance += dist;
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
        first = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        NewController controller = other.gameObject.GetComponent<NewController>();
        if (controller && controller.GetUserType() == NewController.UserType.HUMAN)
        {
       
            if (first)
            {
                Helper(controller);
            }
        }
        
    }
    public void ManuelRotate(NewController charController)
    {
       Helper(charController);
    }
   void Helper(NewController controller)
    {

            if (controller.isZipline == false&&first)
            {
                first = false;
            camFollow.angle = Angle;
            camFollow.distance = Distance;
            //    StartCoroutine(VectorLerp(camFollow.distance, Distance,10));
            //    StartCoroutine(AngleLerp(camFollow.angle, Angle,10));
            }
     
    }

}
