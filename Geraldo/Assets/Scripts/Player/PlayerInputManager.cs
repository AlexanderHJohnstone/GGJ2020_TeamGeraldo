using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    private float _horizontal;
    private bool _grapple;

    private void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _grapple = Input.GetButton("Grapple");
    }

    public float GetHorizontal()
    {
        return _horizontal;
    }

    public bool GetGrapple()
    {
        return _grapple;
    }
}
