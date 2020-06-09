using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using System;

public class NewController : MonoBehaviour
{

    #region PublicVars
    public enum UserType { HUMAN, BOT };
    public bool isRunning = true;
    public bool isRamp = false;
    public float FallingDistance = 0;
    public float characterVerticalOffset = 0;
    public bool isJumped = false;
    public bool isFall = false;
    public float characterHorizontalOffset = 0;
    public Animator anim;
    public PSettings playerSettings;
    public List<GameObject> collectables = new List<GameObject>();
    public Vector3 roadOffset;
    public bool isZipline = false;
    public Vector3 fixedMove = new Vector3();
    public bool trambolinJump = false;
    public bool stopVerticalMove = false;
    public BotMoveRoutine botMove;
    public CharacterController characterController;
    public Vector3 oldRoadOffset = new Vector3();
    public GameObject apart;
    public bool isIncline = false;
    public bool playGame = true;
    public ParticleSystem speedEffect;
    public bool StopInputGather = false;
    public bool Hang = false;
    #endregion
    #region PrivateVars
    private AnimationCurve animCurve;
    InputManager inputManager;
    private WaitForFixedUpdate waitFrame = new WaitForFixedUpdate();
    private float botActionTime = 0;
    private float speedMultiplier = 1;
    private float oldCharacterHorizontalOffset = 0;
    private float oldCharacterVerticalOffset = 0;
    #endregion
    #region SerializedFields
    [SerializeField] UserType user;
    [SerializeField] GameObject roadEffect;
    [SerializeField] GameObject obstacleEffect;
    [SerializeField] GameObject fallEffect;
    ParticleSystem obstacleParticleSystem;
    [SerializeField] CamRotate camRotate;
    #endregion
    #region Strings
    private string runningString = "Running";
    private string jumpString = "Jump";
    private string RunSpeedString = "RunSpeed";
    private string JumpLoop = "JumpLoop";
    private string Grounded = "Grounded";

    #endregion
    // Start is called before the first frame update
    private void Awake()
    {
        roadOffset.y = transform.position.y;   
        inputManager = GetComponent<InputManager>();
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        obstacleParticleSystem =obstacleEffect.GetComponent<ParticleSystem>();
        obstacleParticleSystem.Stop();
        speedMultiplier = 1;
        if (user == UserType.BOT)
        {
            botMove = GetComponent<BotMoveRoutine>();
        }
        else // HUMAN
        {
            speedEffect.Stop();
            Application.targetFrameRate = 60;
        }
       
    }
   
    // Update is called once per frame
    void Update()
    {
        if (!playGame|| StopInputGather)
        {
            roadEffect.SetActive(false);
            return;
        }
           
        if (Mathf.Abs(characterHorizontalOffset) < 0.01f)
            characterHorizontalOffset = 0;

        if (Mathf.Abs(transform.position.y - roadOffset.y) < 0.05f && isRunning)
        {
            roadEffect.SetActive(true);
        }
        else
        {
            roadEffect.SetActive(false);
        }

        if (user == UserType.HUMAN)
           {
              inputManager.SetPlayerPosition(); 
           }
        else
           {
               BotAction();
           }


    }
    private void FixedUpdate()
    {

        if (!playGame ) return;
    
        SetSpeed();
        
        MoveCharacter();

    //   SetGround();
    }
    private void SetGround()
    {
   
     if (transform.position.y - roadOffset.y < 0.2f) 
             anim.SetBool("GroundedJump", true);
        else
              anim.SetBool("GroundedJump", false);
    }

    private void SetSpeed()
    {
        if (stopVerticalMove) return;
        if (isRunning) anim.speed = 1;
        if(speedMultiplier >playerSettings.MaxSpeed&&!isZipline&&!trambolinJump&&!stopVerticalMove)
        {
            speedMultiplier = playerSettings.MaxSpeed;
        }
        if (playerSettings.DefaultSpeed < speedMultiplier)
        {
            speedMultiplier -= Time.fixedDeltaTime * playerSettings.SpeedDecreaseAmount;
           
        }
        else
        {
            if (playerSettings.DefaultSpeed < speedMultiplier + Time.fixedDeltaTime)
            {
                speedMultiplier = playerSettings.DefaultSpeed;
            }
            else
                speedMultiplier += Time.fixedDeltaTime;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName(runningString)&&isRunning)
        {
            float speed = playerSettings.MaxRunAnimationSpeed / playerSettings.MaxSpeed * speedMultiplier;
            if (speed < playerSettings.MinRunAnimationSpeed) speed = playerSettings.MinRunAnimationSpeed;
            anim.SetFloat(RunSpeedString, speed);
        }
    }

    private void BotAction()
    {
        if (botActionTime < 0)
        {
            Vector3 move = botMove.SetBotPosition();
            if (move != Vector3.zero)
            {
                botActionTime = 0.6f;
            }
            inputManager.Move(Vector3.zero, move);
            inputManager.action = false;
        }
        else
            botActionTime -= Time.deltaTime;
    }

    public void SelectAction(float degree, Vector3 distance)
    {
        if (degree < 0)
        {
            degree *= -1;
           
            isIncline = true;
        }
        if (degree < playerSettings.SwitchVerticalToHorizontalDegree || 180 - degree < playerSettings.SwitchVerticalToHorizontalDegree)
        {
            if (Mathf.Abs(distance.x) >= playerSettings.SwitchHorizontalDistance && !inputManager.action)
            {
                if (!isZipline && distance.x > 0 && !Mathf.Approximately(characterHorizontalOffset, playerSettings.RoadSize))
                {
                    inputManager.action = true;
                    StartCoroutine(MoveHorizontal(playerSettings.RoadSize));
                }
                else if (!isZipline && distance.x < 0 && !Mathf.Approximately(characterHorizontalOffset, -playerSettings.RoadSize))
                {
                    inputManager.action = true;
                    StartCoroutine(MoveHorizontal(-playerSettings.RoadSize));
                }
            }
        }
        else if (!inputManager.action && Mathf.Abs(distance.y) >= playerSettings.SwitchHorizontalDistance)
        {
            
            inputManager.action = true;
           
            if (isIncline)
            {
          
                Incline();
            }
            else if(!isZipline)
            {
                if (stopVerticalMove) return;
                Jump();
            }
            
        }
    }
    IEnumerator MoveHorizontal(float amount)
    {
        if(isRunning&&!stopVerticalMove)
        {
            if (amount > 0)
                anim.Play("Right");
            else
                anim.Play("Left");
        }
       
        int iteration = playerSettings.DirectionChangeTime;
        float change = amount / iteration;
        for (int i = 0; i < iteration; i++)
        {
            characterHorizontalOffset += change;
            if (characterHorizontalOffset > playerSettings.RoadSize)
            {
                characterHorizontalOffset = playerSettings.RoadSize;
                break;
            }
            if (characterHorizontalOffset < -playerSettings.RoadSize)
            {
                characterHorizontalOffset = -playerSettings.RoadSize;
                break;
            }
            yield return waitFrame;
        }
    }
    private void MoveCharacter()
    {
        if (isZipline||stopVerticalMove)
        {
            if (stopVerticalMove&&characterHorizontalOffset != transform.position.x)
            {
                fixedMove += Vector3.right * (characterHorizontalOffset - transform.position.x);
            }

     
            characterController.Move(fixedMove);

            return;
        }
        if(characterController.isGrounded) isFall = false;
        Vector3 moveVector = new Vector3(0,0,0.02f*speedMultiplier);

        moveVector += Vector3.up* (characterVerticalOffset- oldCharacterVerticalOffset);
 
           
        if (!characterController.isGrounded && !isJumped)
        {
             FallingDistance = 1;
            if (!isFall)
            {
                FallingDistance = transform.position.y - roadOffset.y;
            }
            if (FallingDistance > 0)
            {
               
                if (FallingDistance > 1.75f)
                {
                    moveVector += Vector3.down * Time.fixedDeltaTime * 10 * FallingDistance;
                }
                else
                    moveVector += Vector3.down * Time.fixedDeltaTime * 3 * FallingDistance;
            }
        }
        if(characterHorizontalOffset != transform.position.x)
        {
            moveVector += Vector3.right * (characterHorizontalOffset - transform.position.x);
        }
      if (!isJumped)
      {
           if ((roadOffset.y - oldRoadOffset.y) > 0)
           {           
               moveVector += Vector3.up * (roadOffset.y - oldRoadOffset.y) * Time.fixedDeltaTime;
           }
           else
               moveVector += Vector3.up * (roadOffset.y - oldRoadOffset.y);
        }
     
        oldCharacterHorizontalOffset = characterHorizontalOffset;
        oldCharacterVerticalOffset   = characterVerticalOffset;
        
        characterController.Move(moveVector);
        inputManager.oldPosChar = transform.position;
        oldRoadOffset = roadOffset;
      

    }
    private void Incline()
    {
       
        isRunning = false;
        if (characterVerticalOffset > 0)
            StartCoroutine(InclineRoutine());
        if(isZipline)
        {
            apart.SetActive(false);
            if (isZipline && user == UserType.HUMAN)
            {
                isZipline = false;
             camRotate.ManuelRotate(this);

                anim.Play("Falling");
            }
        }
        else if (isJumped|| stopVerticalMove )
        {
         
          
            anim.Play("Flip");
            isJumped = false;
            if (stopVerticalMove)
            stopVerticalMove = false;
        }
        else
        {
            if (user == UserType.HUMAN)
            {
                speedEffect.Stop();
                speedEffect.gameObject.SetActive(false);
                anim.Play("Slide", -1, 0);
                Invoke("OpenEffect", 0.75f);
            }
            else
                anim.Play("Slide");


        }
        float animLength = anim.GetCurrentAnimatorStateInfo(0).length;
        Invoke("MakeFalse", animLength);
    }
    void OpenEffect()
    {
        speedEffect.gameObject.SetActive(true);
        speedEffect.Stop();
    }
    IEnumerator InclineRoutine()
    {
        float amount = characterVerticalOffset / 5;
        while (characterVerticalOffset > 0)
        {
            characterVerticalOffset -= amount;
            yield return waitFrame;
        }

        characterVerticalOffset = 0;
    }
    private void MakeFalse()
    {
        isIncline = false;
        isRunning = true;
    }

    private void Jump()
    {
        Vector3 center = transform.position + (Vector3.up * 0.5f);

        if (Physics.Raycast(center,transform.forward, out RaycastHit hit))
        {
            float distance = hit.distance;

            float minDistance = 1.8f;
            float distanceRange = 1f;
            if (speedMultiplier > 4)
            {
                minDistance += 0.5f;
                distanceRange += 0.2f;
            }


            if (hit.collider.CompareTag("Wall") && distance > minDistance && distance < (minDistance + distanceRange))
            {

                stopVerticalMove = true;
                GetComponent<ClimbController>().StartClimbControl();
            }
            else
            { 
            stopVerticalMove = false;

            }
        }
        else stopVerticalMove = false;

        if (stopVerticalMove == false)
        {
            if (!isJumped && (transform.position.y - roadOffset.y < 0.15f || isRamp))
            {
                StartCoroutine(JumpCurveRoutine());
            }
        }
    
    }

    public void ClimbControlEnd()
    {

        stopVerticalMove = false;
        speedMultiplier = 2;
    }

    public float GetSpeedMultiplier()
    {
        return speedMultiplier;
    }

   public IEnumerator JumpCurveRoutine(bool trambolin = false)
    {
      
        anim.SetBool(JumpLoop, false);
        anim.SetBool(Grounded, false);
        anim.speed = 1;
        anim.Play(jumpString,-1,0);
        bool rJump = false;
        float additionalLength = 0;
        if (isRamp)
        {
            additionalLength = transform.position.y - roadOffset.y;
            rJump = true;
        
        }
        isRunning = false;
        isJumped = true;
        animCurve = playerSettings.ShortJump;
        bool stillJump = false;
        float curveTime = 0;
        float curveAmount = animCurve.Evaluate(curveTime);
        float animLength = anim.GetCurrentAnimatorClipInfo(0).Length;
        float curveLength = animCurve[animCurve.length - 1].time+additionalLength/2;
        float animSpeed = animLength / curveLength;
        anim.speed = animSpeed;
        
        while ((curveAmount > 0 || curveTime == 0) && isJumped&&!isZipline&&!Hang)
        {
            curveTime += Time.fixedDeltaTime;
            curveAmount = animCurve.Evaluate(curveTime);
            characterVerticalOffset = curveAmount;
            if (CalculateHeight() > 1.3f&&!rJump)
            {
                anim.SetBool(JumpLoop, true);
                stillJump = true;
            }
            yield return waitFrame;
           
        }
        bool effect = false;
        if (stillJump && !isZipline)
        {

            while (!anim.GetBool(Grounded) && !isZipline)
            {
             
                characterController.Move(Vector3.down * Time.fixedDeltaTime * 4);
                if (transform.position.y - roadOffset.y < .6f) 
                {
                    anim.SetBool(Grounded, true);
                    isJumped = false;
                    isRunning = true;

                }
                yield return waitFrame;
                if (effect == false && transform.position.y - roadOffset.y < .25f)
                {
                    Vector3 pos = gameObject.transform.position;
                    GameObject eff = Instantiate(fallEffect, new Vector3(pos.x, roadOffset.y + 0.065f, pos.z + .15f*speedMultiplier), Quaternion.Euler(-90, 0, 0));
                    eff.transform.localScale = Vector3.one / 6;

                    effect = true;
                    StartCoroutine(DestroyEffect(eff, 1f));
                    if (speedEffect.isPlaying)
                        speedEffect.Stop();
                }
            }
        }// This works when player miss zipline
     
    

        if (characterVerticalOffset < 0|| characterController.isGrounded)
            characterVerticalOffset = 0;
        isJumped = false;
        isRunning = true;
        anim.speed = 1;

    }
    IEnumerator DestroyEffect(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(obj);
    }
    public int GetCharLine()
    {
        if (Mathf.Approximately(characterHorizontalOffset, 0)) return 0;
        if (characterHorizontalOffset < 0) return -1;

        return 1;
    }
    public void Restart()
    {
        playGame = false;
        transform.position = new Vector3(0, 0, 0);
        foreach (GameObject obj in collectables)
        {
            obj.SetActive(true);
        }
        collectables.Clear(); 
        speedMultiplier = 1;
        roadOffset = Vector3.zero;
        characterVerticalOffset = 0;
        isIncline = false;
        isRunning = true;
        isZipline = false;
        trambolinJump = false;
        if(apart != null)
             apart.SetActive(false);
        isRamp = false;
        speedMultiplier = playerSettings.MinSpeed;
        FallingDistance = 0;
        characterVerticalOffset = 0;
        isJumped = false;
        isFall = false;
        characterHorizontalOffset = 0;
        trambolinJump = false;
        stopVerticalMove = false;
        anim.Play(runningString);
        anim.SetBool("Win", false);
        Invoke("StartGame", .1f);
    }
    public void StartGame()
    {
        playGame = true;
    }
    public float GetSpeed()
    {
        return speedMultiplier;
    }
    public void IncreaseSpeed(float amount)
    {
        if (speedMultiplier + amount > playerSettings.MaxSpeed&&!isZipline&&!trambolinJump)
            speedMultiplier = playerSettings.MaxSpeed;
        else
            speedMultiplier += amount;
    }
    public void IncreaseSpeedLimitless(float amount)
    {
        speedMultiplier += amount;
    }
    public UserType GetUserType()
    {
        return user;
    }
    public void ObstacleBlock()
    {
        if(!obstacleParticleSystem.isPlaying)
           obstacleParticleSystem.Play();
        //obstacleEffect.SetActive(true);
        Invoke("StopParticle", 1.5f);
        speedMultiplier = playerSettings.MinSpeed;
    }
    void StopParticle()
    {
        if (obstacleParticleSystem.isPlaying)
            obstacleParticleSystem.Stop();
        //obstacleEffect.SetActive(false);
    }

    public float CalculateHeight()
    {
        return transform.position.y - roadOffset.y;
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.CompareTag("Ramp"))
        {
            isRamp = true;
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("TrambolinLoop"))
                anim.Play(runningString);
        }
        else
        {
            isRamp = false;
        }
       if(hit.transform.gameObject.layer != 12 )
        {
            if(Mathf.Abs(hit.normal.x)>0.1f&& (hit.normal.x*characterController.velocity.x)<0)
            {
                if (transform.position.y < 0.05 || isJumped)
                {
                    if (hit.normal.x > 0)
                    {
                        ObstacleBlock();
                        StartCoroutine(MoveHorizontal(playerSettings.RoadSize));
                    }
                    else
                    {
                        ObstacleBlock();
                        StartCoroutine(MoveHorizontal(-playerSettings.RoadSize));
                    }
                }
            }
        }
    }
}
