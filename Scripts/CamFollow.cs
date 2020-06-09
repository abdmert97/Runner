using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Vector3 distance;
    public Vector3 angle;
    [SerializeField] Transform obj;
    [SerializeField] float xOffset;
    [SerializeField] float followSpeed;
    
    Vector3 vel= new Vector3();

    private Vector3 currentDistance;

    private void Awake()
    {
        currentDistance = distance;
    }
    private void LateUpdate()
    {

  

        transform.localRotation = Quaternion.Lerp(transform.localRotation,Quaternion.Euler(angle),2* Time.deltaTime);

    
        float time = Time.deltaTime * followSpeed;

        currentDistance = Vector3.Lerp(currentDistance, distance, followSpeed * Time.deltaTime);

       transform.localPosition = new Vector3(Mathf.Lerp(transform.position.x, currentDistance.x + obj.position.x,
                                       followSpeed / 10 / Mathf.Abs(transform.position.x - currentDistance.x - obj.position.x)),

                                        Mathf.Lerp(transform.position.y, currentDistance.y + obj.position.y, 
                                       followSpeed/3 / Mathf.Abs(transform.position.y- currentDistance.y - obj.position.y)), 
                                       Mathf.Lerp(transform.position.z, currentDistance.z + obj.position.z, 
                                       followSpeed / Mathf.Abs(transform.position.z - currentDistance.z - obj.position.z)));
       // transform.position = Vector3.SmoothDamp(transform.position, obj.position+distance,ref vel,time);
        //transform.position = new Vector3(distance.x + obj.position.x , transform.position.y,transform.position.z);
           // distance + obj.position;
    }
}
