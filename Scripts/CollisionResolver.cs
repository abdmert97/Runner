using System.Collections;
using System.Collections.Generic;
using ToolScripts;
using ToolScripts.Explosion3D;
using UnityEngine;
using UnityEngine.UI;
public class CollisionResolver : MonoBehaviour
{
    #region SerializeFields
    [SerializeField] NewController playerController;
    [SerializeField] SpeedController sController;
    [SerializeField] GameObject particleEffect;
    [SerializeField] RectTransform SpeedBar;
    [SerializeField] Camera cam;
    [SerializeField] CamFollow camFollow;
    [SerializeField] Material Opakmat;
    [SerializeField] Material TransparentMat;
    [SerializeField] SpeedEffectController speedEffectController;
    #endregion SerializeFields
    #region Strings
    private string Jump = "Jump";
    private string Slide = "Slide";
    private string JumpOrSlide = "JumpOrSlide";
    private string Obstacle = "Obstacle";
    private string Collectable = "Collectable";
    private string Ramp = "Ramp";
    private string Ground = "Ground";
    private string FallSpeed = "FallSpeed";
    private string Falling = "Falling";
    #endregion
        float time = 0;
    bool first = true;
    bool jumpActive = false;
    float force = 3.25f;
    NewController controller;
    bool firstFall = true;
    private void Awake()
    {
        controller = GetComponent<NewController>();
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag(Jump) || other.CompareTag(Slide) || other.CompareTag(JumpOrSlide) || other.CompareTag(Obstacle))
        {
            playerController.ObstacleBlock();

            if (playerController.GetUserType() == NewController.UserType.HUMAN)
            {

                var explosionData = new ExplosionManager.ExplosionData
                {
                    ExplosionPosition = other.transform.position,
                    ExplosionVelocity = 1.2f,
                    Gravity = Vector3.down * 20,
                    AdditionalVelocityToDirections = -Physics.gravity*.3f,
                    ParticleCount = 20,
                    SizeRange = new Vector2(0.03f, 0.08f),
                    ExplosionColors = new List<Color32>{Color.white, Color.red}
                };
                
                ExplosionManager.Instance.CreateExplosion(explosionData);

                var shakeScript = Camera.main.transform.parent.GetComponent<Shake>();
                shakeScript.ShakeTrigger(0.15f, .05f);

                BasicCoroutineHandler.Instance.WaitForSeconds(0.03f,
                    () => { other.GetComponent<MeshRenderer>().enabled = false; });

                BasicCoroutineHandler.Instance.WaitForSeconds(1,
                    () => { other.GetComponent<MeshRenderer>().enabled = true; });
            }
        }

        else if (other.CompareTag(Collectable))
        {
           
            sController.IncreaseSpeed();
            playerController.collectables.Add(other.gameObject);
           
            if (playerController.GetUserType() == NewController.UserType.HUMAN)
            {
                GameObject particle = Instantiate(particleEffect, transform.position + Vector3.forward * 0.65f, Quaternion.identity);
                particle.transform.localScale = Vector3.one / 3;
                speedEffectController.IncreaseTime(1);
                StartCoroutine(WaitAndDestroy(particle, 1));
                StartCoroutine(ScaleDown(other.gameObject));
            }
            else
            {
                other.gameObject.SetActive(false);
            }
        }

        if(other.CompareTag("FallObstacle")&&firstFall)
        {
            controller.stopVerticalMove = true;
            controller.StopInputGather = true;
            firstFall = false;
           
            FallObstacle fallObstacle = other.GetComponent<FallObstacle>();
            StartCoroutine(FallRoutine(fallObstacle));
        }

    }
   
    IEnumerator FallRoutine(FallObstacle fallObstacle)
    {
        
        controller.anim.Play("Falling");
        controller.anim.SetFloat("FallSpeed", 0.6f);
        controller.fixedMove = Vector3.down * Time.fixedDeltaTime*2 + Vector3.forward * Time.fixedDeltaTime * 0.5f;

        yield return new  WaitForSeconds(1f);
        Vector3 pos = controller.transform.position;
        controller.fixedMove = Vector3.zero;
        controller.transform.position = new Vector3(pos.x, fallObstacle.spawnPoint.position.y, fallObstacle.spawnPoint.position.z);
        Invoke("OpenController", 0.1f);
      
    }
    void OpenController()
    {
        firstFall = true;
        controller.anim.Play("Running");
        controller.stopVerticalMove = false;
        controller.StopInputGather = false;
    }
    IEnumerator SetTransparency(GameObject obj)
    {
        Renderer rend = obj.GetComponent<Renderer>();
        rend.sharedMaterial = TransparentMat;
        yield return new WaitForSeconds(0.75f);
        rend.sharedMaterial = Opakmat;

    }
    IEnumerator ScaleDown(GameObject obj )
    {
        if(playerController.GetUserType()==NewController.UserType.BOT)
        {
            obj.gameObject.SetActive(false);
        }
        else
        {
            float distance = 0;
            Collider col = obj.GetComponent<Collider>();
            col.enabled = false;

            Vector3 firstPos = obj.transform.position;
            while (obj.transform.localScale.x > 0.2f)
            {
                Vector3 objPos = obj.transform.position;
                Vector3 pos = cam.ScreenToWorldPoint(SpeedBar.position);

                pos += Vector3.right + Vector3.up * 0.5f + Vector3.back * (camFollow.distance.z - 1 + distance);
                distance += 0.01f;
                obj.transform.position = Vector3.Lerp(objPos, pos, 0.1f);
                obj.transform.localScale -= Vector3.one * 0.075f;
                yield return new WaitForFixedUpdate();
            }

            obj.gameObject.SetActive(false);
         
            obj.transform.position = firstPos;
            obj.transform.localScale = Vector3.one;
            col.enabled = true;
        }
       

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Ramp))
        {
            float distance = transform.position.y-playerController.roadOffset.y;

            if (distance > 0.15f && !playerController.isJumped && !playerController.characterController.isGrounded)
            {
                playerController.anim.SetFloat(FallSpeed, 1f);
                playerController.anim.Play(Falling);
                playerController.isFall = true;
                playerController.FallingDistance = distance;

            }

        }
        if (other.CompareTag("Wall"))
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
    }
    IEnumerator WaitAndDestroy(GameObject obj,float time)
    {
       
        yield return new WaitForSeconds(time);
        Destroy(obj);
    }
    private void OnCollisionEnter(Collision collision)
    {
      if(collision.collider.CompareTag(Ground))
        {
            playerController.isRamp = false;
            playerController.isFall = true;
           
        }
        if (collision.collider.CompareTag(Ramp))
        {
            playerController.isJumped = false;
            playerController.isRamp = true;

        }
    }

    void OpenPlayerMove()
    {
        controller.StopInputGather = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            float distance = transform.position.z - transform.position.z;
            if (distance < 0) return;

            if (distance < 0.4f && first && !controller.isJumped && !controller.isRamp && controller.stopVerticalMove == false)
            {

                controller.StopInputGather = true;
                controller.stopVerticalMove = true;
                controller.anim.SetBool("Hang", true);
                first = false;
                time = 0;
            }
            if (!first)
            {
                if (time == 0)
                {
                    Vector3 pos = transform.position;
                    transform.position = new Vector3(pos.x, controller.roadOffset.y, pos.z);

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
                else if (time < 3f)
                    controller.fixedMove = Vector3.zero;
                else
                    controller.fixedMove = Vector3.up * Time.fixedDeltaTime * 0.1f;
                time += Time.deltaTime;
            }

        }
    }

  

}
