using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbController : MonoBehaviour
{
    public Animator anim;
    public NewController control;

    private float upForce;

    public float moveSpeed;
    public float jumpForce;
    public bool applyRootMotion;

    public bool isClimbActive;

    bool useGravity = true;



    bool isClimbing;

    void Start()
    {
        anim.applyRootMotion = true;

    }

    public void StartClimbControl()
    {
        isClimbActive = true;
        useGravity = true;

        Jump();
    }


    void FixedUpdate()
    {
        if (isClimbActive == false)
            return;

        Move();
        Gravity();

    
    }

    void Move()
    {
        if (applyRootMotion == false)
        {
            Vector3 nextPosition = (new Vector3(0, 0, 0.02f) * control.GetSpeedMultiplier());
            control.fixedMove = nextPosition;

        }
     
    }

    void Jump()
    {
        //rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        upForce = jumpForce;
        anim.SetTrigger("WallJump");
    }

    void Gravity()
    {
        if (useGravity == false)
            return;

        
        upForce += Physics.gravity.y * Time.fixedDeltaTime;
        control.fixedMove.y = upForce * Time.fixedDeltaTime;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Wall") && isClimbing == false && isClimbActive == true)
        {

            isClimbing = true;
            anim.SetTrigger("OnWall");

            control.fixedMove = Vector3.zero;

            float targetHeight = hit.gameObject.GetComponent<GetWallHeight>().wallHeight;
            StartCoroutine(JumpAtWall(targetHeight));
        }
    }

    IEnumerator JumpAtWall(float targetHeight)
    {
        control.fixedMove = Vector3.down * 0.05f;
        yield return new WaitForSeconds(0.10f);
        // rigid.useGravity = true;



        float jumpVel = CalculateWallForce(targetHeight - 0.00f);

        upForce = jumpVel;
        //control.fixedMove = Vector3.up * jumpVel;


        StartCoroutine(WaitCatchExit(targetHeight));
    }

    IEnumerator WaitCatchExit(float targetHeight)
    {
        //character scale
        targetHeight -= 0.2f;
        yield return new WaitUntil(() => transform.position.y >= targetHeight);

        control.fixedMove = Vector3.zero;
        useGravity = false;


        anim.SetTrigger("ExitWall");

        StartCoroutine(WaitUntilExit(0.75f));
    }

    IEnumerator WaitUntilExit(float animExitTime)
    {
        applyRootMotion = true;
        yield return new WaitForSeconds(animExitTime);
        applyRootMotion = false;
        isClimbing = false;
        isClimbActive = false;

        control.ClimbControlEnd();

        useGravity = true;
    }




    private void OnAnimatorMove()
    {
        if (!applyRootMotion)
            return;

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsTag("Motion"))
        {
            var deltaMove = anim.deltaPosition;

            deltaMove.z *= 4;
            deltaMove.y *= 1.15f;

            control.fixedMove = deltaMove;
        }

    }

    float CalculateWallForce(float targetHeight)
    {
        float heightDif = targetHeight - transform.position.y;
        float gravity = Physics.gravity.y;

        float duration = Mathf.Sqrt(Mathf.Abs((heightDif * 2) / gravity));

        return Mathf.Abs(duration * gravity);
    }

}
