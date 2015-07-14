using UnityEngine;
using System.Collections;
public class CubeRotator : MonoBehaviour {
    public float rotationSpeed = 180.0f;
     
    void Update () {
        gameObject.transform.Rotate(Vector3.up, Time.smoothDeltaTime * rotationSpeed);
    }
}