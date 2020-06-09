using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    private bool pressed;
    public bool action = false;
    private Vector3 oldPosMouse;
    public Vector3 oldPosChar;
    private Camera cam;
    private Vector3 currentPosMouse;
    NewController controller;

    private void Awake()
    {
        cam = Camera.main;
        oldPosChar = transform.position;
        controller = GetComponent<NewController>();
    }
    public void SetPlayerPosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pressed = true;
            oldPosMouse = cam.ScreenToViewportPoint(Input.mousePosition);
        }
        if (pressed && Input.GetMouseButton(0))
        {
            currentPosMouse = cam.ScreenToViewportPoint(Input.mousePosition);
            Move(oldPosMouse, currentPosMouse);
        }
        if (Input.GetMouseButtonUp(0))
        {
            pressed = false;
            action = false;
        }
    }
    public void Move(Vector3 oldPos, Vector3 currPos)
    {
        if (oldPos != null)
        {
            Vector3 distance = currPos - oldPos;
            float degree = Mathf.Rad2Deg * Mathf.Atan2(distance.y, distance.x);
            controller.isIncline = false;
            controller.SelectAction(degree, distance);
        }
        else
        {
            oldPos = new Vector3();
            oldPos = currentPosMouse;
            return;
        }
    }
}
