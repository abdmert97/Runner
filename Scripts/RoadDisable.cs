using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadDisable : MonoBehaviour
{
    // Start is called before the first frame update



    [SerializeField] GameObject road;

    

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<NewController>().GetUserType()==NewController.UserType.HUMAN)
        {
            road.SetActive(false);
            Invoke("Active", 0.4f);
        }
    }
    void Active()
    {
        road.SetActive(true);
    }

}
