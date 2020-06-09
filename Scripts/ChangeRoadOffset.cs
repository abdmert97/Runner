using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRoadOffset : MonoBehaviour
{
    [SerializeField] bool isFall;
    [SerializeField] GameObject fallEffect;
    private void OnTriggerEnter(Collider other)
    {
        NewController controller;
        controller = other.GetComponent<NewController>();
        if(controller)
        {

            if (isFall)
            {
                StartCoroutine(Fall(controller.roadOffset, transform.position, 60, controller));
            }
            else
            {
                StartCoroutine(Fall(controller.roadOffset, transform.position, 20, controller));
            }
            if (!controller.isJumped&&isFall&&!controller.isZipline&&!controller.trambolinJump)
            {
                controller.isRunning = false;
                controller.anim.SetFloat("FallSpeed", 0.4f);
                controller.anim.Play("Falling");
            }
        }
    }

    IEnumerator Fall(Vector3 old,Vector3 curr,int iteration,NewController controller)
    {
        float distance = curr.y - old.y;
        for(int i = 1; i<= iteration; i++)
        {
            controller.roadOffset.y = old.y + (distance / iteration) * i;
            yield return new WaitForFixedUpdate();
        }
        if (isFall&&controller.anim.GetCurrentAnimatorStateInfo(0).IsName("Falling"))
        {
           Vector3 pos = controller.gameObject.transform.position;
           GameObject eff = Instantiate(fallEffect, new Vector3(pos.x, controller.roadOffset.y, pos.z + .3f), Quaternion.Euler(-90, 0, 0));
           eff.transform.localScale = Vector3.one / 7;
           StartCoroutine(DestroyEffect(eff, 1f));       
        }
            
    }
    IEnumerator DestroyEffect(GameObject obj,float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(obj);
    }
}
