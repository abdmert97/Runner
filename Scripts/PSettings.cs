using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PSettings", menuName = "PlayerSetting", order = 55)]
public class PSettings : ScriptableObject
{
    [SerializeField] float roadSize;
    [SerializeField] float switchHorizontalDistance;
    [SerializeField] float switchVerticalDistance;
    [SerializeField] float switchVerticaltoHorizontalDegree;
    [SerializeField] float jumpDistance;
    [SerializeField] int jumpTime;
    [SerializeField] float maxSpeed;
    [SerializeField] float minSpeed;
    [SerializeField] float followSpeedChange;
    [SerializeField] int directionChangeTime;
    [SerializeField] float playerCameraShakeSpeed;
    [SerializeField] float playerCameraShakeDistance;
    [SerializeField] AnimationCurve trambolinJump;
    [SerializeField] AnimationCurve shortJump;
    [SerializeField] float maxRunAnimationSpeed;
    [SerializeField] float minRunAnimationSpeed;
    [SerializeField] float speedDecreaseAmount;
    [SerializeField] float defaultSpeed;
    [SerializeField] float flipDistance;
    public float RoadSize
    {
        get
        {
            return roadSize;
        }
    }
    public float SwitchHorizontalDistance
    {
        get
        {
            return switchHorizontalDistance;
        }
        set => switchHorizontalDistance = value;
    }
    public float SwitchVerticalToHorizontalDegree
    {
        get
        {
            return switchVerticaltoHorizontalDegree;
        }
    }
    public float SwitchVerticalDistance
    {
        get
        {
            return switchVerticalDistance;
        }
        set => switchVerticalDistance = value;
    }
    public float JumpDistance
    {
        get
        {
            return jumpDistance;
        }
    }
    public int JumpTime
    {
        get
        {
            return jumpTime;
        }
    }
    public float MaxSpeed
    {
        get
        {
            return maxSpeed;
        }
        set => maxSpeed = value;

    }

    public float MinSpeed
    {
        get
        {
            return minSpeed;
        }
        set => minSpeed = value;
    }
    public float FollowSpeedChange
    {
        get
        {
            return followSpeedChange;
        }
    }
    public int DirectionChangeTime
    {
        get
        {
            return directionChangeTime;
        }
    }
    public float PlayerCameraShakeSpeed
    {
        get
        {
            return playerCameraShakeSpeed;
        }
    }
    public float PlayerCameraShakeDistance
    {
        get
        {
            return playerCameraShakeDistance;
        }
    }
    public AnimationCurve TrambolinJmup
    {
        get
        {
            return trambolinJump;
        }
    }
    public AnimationCurve ShortJump
    {
        get
        {
            return shortJump;
        }
    }
    public float MaxRunAnimationSpeed
    {
        get
        {
            return maxRunAnimationSpeed;
        }
        set => maxRunAnimationSpeed = value;
    }
    public float MinRunAnimationSpeed
    {
        get
        {
            return minRunAnimationSpeed;
        }
        set => minRunAnimationSpeed = value;
    }
    public float SpeedDecreaseAmount
    {
        get
        {
            return speedDecreaseAmount;
        }
    }
    public float DefaultSpeed
    {
        get
        {
            return defaultSpeed;
        }
        set => defaultSpeed = value;
    }
    public float FlipDistance
    {
        get
        {
            return flipDistance;
        }
    }

}
