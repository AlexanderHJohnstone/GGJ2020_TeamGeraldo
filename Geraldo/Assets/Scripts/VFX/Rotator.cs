using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Rotator : MonoBehaviour
{
    public float _rotateSpeed = 10f;
    private float _angle;

    private void Update()
    {
        _angle += _rotateSpeed * Time.deltaTime;
        transform.localEulerAngles = new Vector3(0f, 90f, _angle);
    }
}
