using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrictPosition : MonoBehaviour
{
    [SerializeField] GameObject FollowedObject;
    [SerializeField] Vector3 distance;
    private void LateUpdate()
    {
        transform.position = FollowedObject.transform.position+distance;
    }
}
