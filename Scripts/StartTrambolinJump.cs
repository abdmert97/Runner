using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTrambolinJump : MonoBehaviour
{
    NewController controller;
    [SerializeField] GameObject fallEffect;
    private void OnTriggerEnter(Collider other)
    {
        controller = other.GetComponent<NewController>();
        controller.trambolinJump = true;
        StartCoroutine(TrambolinJump());
    }
    IEnumerator TrambolinJump()
    {
        bool effect = false;
        controller.anim.SetBool("Grounded", false);
        controller.stopVerticalMove = true;
        controller.anim.Play("TrambolinJump"); 
        controller.speedEffect.Clear();
        controller.speedEffect.Play();
     
        for (int i = 0; i <60; i++)
        {
            controller.fixedMove= Vector3.up/12-Vector3.up*i/(60*12)+ Vector3.forward * controller.playerSettings.MaxSpeed * 0.025f;

            yield return new WaitForFixedUpdate();
        }
        float downSpeed =0;
        bool isLand = false;
        while (!controller.characterController.isGrounded)
        {
            controller.fixedMove = (Vector3.down/48)*(downSpeed)+Vector3.forward*controller.playerSettings.MaxSpeed*0.025f;
            if(downSpeed<4.5f)
                 downSpeed += 0.18f;   
            if (controller.transform.position.y - controller.roadOffset.y < .7f&&!isLand)
            { 
                isLand = true;
                controller.anim.SetBool("Grounded", true);      
            }
            if (effect == false && controller.transform.position.y - controller.roadOffset.y < 0.25f)
            {
                Vector3 pos = controller.gameObject.transform.position;
                GameObject eff = Instantiate(fallEffect, new Vector3(pos.x, controller.roadOffset.y + 0.075f, pos.z + .35f), Quaternion.Euler(-90, 0, 0));
                eff.transform.localScale = Vector3.one / 6;

                effect = true;
                StartCoroutine(DestroyEffect(eff, 1f));
                if (controller.speedEffect.isPlaying)
                    controller.speedEffect.Stop();
            }
            yield return new WaitForFixedUpdate();
        }
        controller.anim.SetBool("Grounded", true);
        controller.stopVerticalMove = false;
        controller.trambolinJump = false;
    }

    IEnumerator DestroyEffect(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(obj);
    }
}
