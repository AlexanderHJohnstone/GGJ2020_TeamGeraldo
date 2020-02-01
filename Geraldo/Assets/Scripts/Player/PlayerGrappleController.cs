using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrappleController : MonoBehaviour
{
    public PlayerMovementController _movementController;

    private void OnTriggerEnter(Collider other)
    {
        _movementController.AttachGrapple(new Vector3(transform.position.x, transform.position.y, 0f));
    }
}
