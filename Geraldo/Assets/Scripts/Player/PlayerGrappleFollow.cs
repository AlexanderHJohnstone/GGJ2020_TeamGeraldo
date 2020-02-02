using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrappleFollow : MonoBehaviour
{
    public GameObject _grappleTarget;
    public float _followSpeed = 0.5f;

    public bool _isGrappling;

    private Vector3 _lastPos;

    public void GrappleStart ()
    {
        _isGrappling = true;
    }

    public void GrappleEnd ()
    {
        _isGrappling = false;
    }

    public void GrappleReset()
    {
        GrappleEnd();
        transform.position = _grappleTarget.transform.position;
    }

    public void LateUpdate()
    {
        if (_isGrappling)
        {
            this.transform.position = Vector3.Lerp(_lastPos, _grappleTarget.transform.position, _followSpeed);
            _lastPos = this.transform.position;
        }
    }
}
