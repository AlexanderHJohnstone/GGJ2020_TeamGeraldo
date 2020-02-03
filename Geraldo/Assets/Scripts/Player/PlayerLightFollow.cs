using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightFollow : MonoBehaviour
{
    public Transform _follow;

    public float _speed = 0.1f;

    private Vector3 _offset;



    private void Start()
    {
        _offset = transform.position - _follow.position;
    }

    void Update()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, _follow.position + _offset, _speed);
    }
}
