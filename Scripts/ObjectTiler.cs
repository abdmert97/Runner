using UnityEngine;

public class ObjectTiler : MonoBehaviour
{
    public Transform objectPrefab;
    public float tileDistance;

    public int tileCount;

    private void Start()
    {
        for (int i = 0; i < tileCount; i++)
        {
            var objectClone = GameObject.Instantiate(objectPrefab);

            objectClone.transform.SetParent(this.transform);
            objectClone.transform.localRotation = Quaternion.identity;
            objectClone.transform.localScale = objectPrefab.localScale;

            float zPosition = tileDistance / 2f + tileDistance * i;
            objectClone.transform.localPosition = new Vector3(0, 0, zPosition);
        }
    }
}
