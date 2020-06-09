using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampCollision : MonoBehaviour
{
    NewController playerController;
    bool start = true;
    Animator anim;

    private void OnTriggerEnter(Collider other)
    {
        playerController = other.GetComponentInParent<NewController>();
        anim = other.GetComponent<Animator>();

        playerController.isFall = true;
       
    }
   


}
