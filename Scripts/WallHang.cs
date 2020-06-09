using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHang : MonoBehaviour
{
    NewController controller;
    float time = 0;
    bool first = true;
    bool jumpActive = false;
    float force = 3.25f;
    private void OnTriggerEnter(Collider other)
    {

        controller = other.GetComponent<NewController>();



    }
    private void OnTriggerStay(Collider other)
    {
      
        float distance = transform.position.z - other.transform.position.z;
        if (distance < 0) return;
       
        if (distance < 0.4f&&first&& !controller.isJumped&&!controller.isRamp && controller.stopVerticalMove == false)
        {

            controller.StopInputGather = true;
            controller.stopVerticalMove = true;
            controller.anim.SetBool("Hang", true);
            first = false;
            time = 0;
        }
        if(!first)
        {
            if (time == 0)
            { Vector3 pos = other.transform.position;
                other.transform.position = new Vector3(pos.x,controller.roadOffset.y, pos.z);
                    
                    }
            if (time > 0.25f && time < 0.50f)
            {
                controller.fixedMove = Vector3.up * Time.fixedDeltaTime * force + Vector3.forward * Time.fixedDeltaTime * 2.2f;
                force -= Time.deltaTime * 3;
            }
            else if (time > 0.65f && time < 1.67f)
                controller.fixedMove = Vector3.up * Time.fixedDeltaTime / 3.1f;
            else if (time >= 1.67f)
                controller.fixedMove = Vector3.forward * Time.fixedDeltaTime;
            else if(time <3f)
                controller.fixedMove = Vector3.zero;
            else
                controller.fixedMove = Vector3.up * Time.fixedDeltaTime * 0.1f;
            time += Time.deltaTime;
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        time = 0;
        force = 3.25f;
        controller.StopInputGather = false;
        controller.stopVerticalMove = false;
        controller.anim.SetBool("Hang", false);
        controller.anim.SetBool("RunHang", false);
        first = true;
        jumpActive = false;
        controller.Hang = false;
        controller.IncreaseSpeed(-1);
        Invoke("OpenPlayerMove", .1f);
    }
    void OpenPlayerMove()
    {
        controller.StopInputGather = false;
    }
}
