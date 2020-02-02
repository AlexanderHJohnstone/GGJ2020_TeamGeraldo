using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    private float _horizontal;
    private bool _grappleHeld;
    private bool _grappleReleased;

    private void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _grappleHeld = Input.GetButton("Grapple");
        _grappleReleased = Input.GetButtonUp("Grapple");
    }

    public float GetHorizontal()
    {
        return _horizontal;
    }

    public bool GetGrapple()
    {
        return _grappleHeld;
    }

    public bool GrappleReleased()
    {
        return _grappleReleased;
    }
}
