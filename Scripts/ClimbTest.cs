using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbTest : MonoBehaviour
{

    public Animator anim;
    public Rigidbody rigid;


    public float moveSpeed;
    public float jumpForce;
    public bool applyRootMotion;

    float currentMoveSpeed;

    bool isClimbing;

    void Start()
    {
        anim.applyRootMotion = true;
        currentMoveSpeed = moveSpeed;
    }

   
    void Update()
    {
        Move();


        if (Input.GetMouseButtonDown(0) && isClimbing == false)
        {
            Jump();
        }
    }

    void Move()
    {
        Vector3 nextPosition = transform.position + (transform.forward * currentMoveSpeed * Time.deltaTime);
        rigid.MovePosition(nextPosition);
    }

    void Jump()
    {
        rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        anim.SetTrigger("WallJump");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isClimbing = true;
            anim.SetTrigger("OnWall");
            currentMoveSpeed = 0;
            rigid.velocity = Vector3.zero;

            float targetHeight = collision.gameObject.GetComponent<GetWallHeight>().wallHeight;
            StartCoroutine(JumpAtWall(targetHeight));
        }
    }

    IEnumerator JumpAtWall(float targetHeight)
    {
        rigid.velocity = Vector3.down * 0.05f;
        yield return new WaitForSeconds(0.10f);
       // rigid.useGravity = true;


       
        float jumpVel = CalculateWallForce(targetHeight - 0.10f);


        rigid.velocity = Vector3.up * jumpVel;


        StartCoroutine(WaitCatchExit(targetHeight));
    }       

    IEnumerator WaitCatchExit(float targetHeight)
    {
        //character scale
        targetHeight -= 0.2f;
        yield return new WaitUntil(() => transform.position.y >= targetHeight);

        rigid.velocity = Vector3.zero;
        rigid.useGravity = false;

       
        anim.SetTrigger("ExitWall");

        StartCoroutine(WaitUntilExit(0.75f));
    }

    IEnumerator WaitUntilExit(float animExitTime)
    {
        applyRootMotion = true;
        yield return new WaitForSeconds(animExitTime);
        applyRootMotion = false;
        isClimbing = false;

        StartCoroutine(MoveSpeedBoots());
        rigid.useGravity = true;
    }

    IEnumerator MoveSpeedBoots()
    {
        while (currentMoveSpeed < moveSpeed)
        {
            currentMoveSpeed += 6 * Time.deltaTime;
            yield return new WaitForEndOfFrame();

        }

        currentMoveSpeed = moveSpeed;
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

            rigid.transform.position += deltaMove;
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
