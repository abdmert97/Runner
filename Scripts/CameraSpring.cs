using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpring : MonoBehaviour
{
    private float upper;
    private float lower;
    [SerializeField] PSettings playerSettings;

    bool up = true;
    // Start is called before the first frame update
    void Start()
    {
        upper = transform.localPosition.y + playerSettings.PlayerCameraShakeDistance;
        lower = transform.localPosition.y +  playerSettings.PlayerCameraShakeDistance;
    }

    // Update is called once per frame
    void Update()
    {

        if(true)
        {
            CameraShake();
        }
      


    }
    private void CameraShake()
    {
        if (up)
        {
            if (transform.localPosition.y < upper)
            {
                transform.localPosition += Vector3.up * Time.deltaTime * playerSettings.PlayerCameraShakeSpeed;
                transform.Rotate(Vector3.left * 10 * Time.deltaTime * playerSettings.PlayerCameraShakeSpeed);
            }
            else
                up = false;
        }
        else
        {
            if (transform.localPosition.y > lower)
            {
                transform.localPosition -= Vector3.up * Time.deltaTime * playerSettings.PlayerCameraShakeSpeed;
                transform.Rotate(Vector3.right * Time.deltaTime * 10 * playerSettings.PlayerCameraShakeSpeed);
            }
            else
                up = true;
        }
    }
}
