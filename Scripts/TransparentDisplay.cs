using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentDisplay : MonoBehaviour
{
    [SerializeField]Material mat;
    private void Start()
    {
        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 1);
    }
    private void OnTriggerEnter(Collider other)
    {
        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, .1f);
    }
    private void OnTriggerExit(Collider other)
    {
        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 1);
    }
}
