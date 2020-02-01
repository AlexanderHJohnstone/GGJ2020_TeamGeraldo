using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputManager))]
public class PlayerMovementController : MonoBehaviour
{
    public Transform _playerRoot;
    private PlayerInputManager _inputManager;

    private Vector3 _velocity;
    public enum _grappleStates { _idle, _shooting, _retracting, _attached};
    public _grappleStates _currentGrappleState;

    //public movement fields
    public float _acceleration = 0.1f;
    [Range(0f, 1f)]
    public float _horizontalDrag = 0.95f;
    public float _maxHorizontalSpeed = 1f;
    public float _maxVerticalSpeed = 10f;
    public float _gravity = 10f;

    //grapple fields
    private float _radius;
    public float _angle;
    private Vector3 _attachPoint;
    public GameObject _grappleSlot;
    public GameObject _grapple;
    private Vector3 _targetPosition;
    public float _grappleSpeed = 10f;
    public float _maxGrappleDistance = 5f;

    //debugging fields
    [Header("DEBUGGING FIELDS")]
    public bool _movePlayer = true;

    private void Start()
    {
        _inputManager = GetComponent<PlayerInputManager>();
        _velocity = new Vector3(0f, 10f, 0f);
        _currentGrappleState = _grappleStates._idle;
    }

    private void Update()
    {
        //poll input
        float hDir = _inputManager.GetHorizontal();
        bool grapple = _inputManager.GetGrapple();

        switch(_currentGrappleState)
        {
            case _grappleStates._idle:
                AirMovement(hDir);

                if (grapple)
                {
                    Vector3 mousePosition = Input.mousePosition;
                    mousePosition.z = -Camera.main.transform.position.z;
                    Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                    _targetPosition = mouseWorldPosition;
                    _currentGrappleState = _grappleStates._shooting;
                }
                break;
            case _grappleStates._shooting:
                AirMovement(hDir);
                ShootGrapple();

                if(grapple)
                {
                    _currentGrappleState = _grappleStates._retracting;
                }
                break;
            case _grappleStates._retracting:
                AirMovement(hDir);
                RetractGrapple();
                break;
            case _grappleStates._attached:
                GrappleMovement(hDir);

                if(grapple)
                {
                    float currentSpeed = _velocity.magnitude;
                    Vector3 dir = _attachPoint - transform.position;
                    Vector3 newDirection = Vector3.Cross(dir, Vector3.forward).normalized;
                    _velocity = new Vector3(newDirection.x, newDirection.y, 0f).normalized * currentSpeed;
                    _currentGrappleState = _grappleStates._retracting;
                }
                break;
            default:
                break;
        }
    }

    private void AirMovement(float hDir)
    {
        if(!_movePlayer)
        {
            return;
        }

        //air control
        if (Mathf.Abs(hDir) > 0f)
        {
            _velocity.x += hDir * _acceleration;
        }
        else
        {
            _velocity.x *= _horizontalDrag;
        }
        _velocity.x = Mathf.Clamp(_velocity.x, -_maxHorizontalSpeed, _maxHorizontalSpeed);

        //add gravity
        _velocity.y -= _gravity;
        _velocity.y = Mathf.Clamp(_velocity.y, -_maxVerticalSpeed, 100f);

        //apply movement
        transform.position += _velocity * Time.deltaTime;
    }

    private void GrappleMovement(float hDir)
    {
        _angle += _velocity.magnitude / _radius;
        if(_angle > 360f)
        {
            _angle -= 360f;
        }
        if(_angle < 0f)
        {
            _angle += 360f;
        }
        float x = _radius * Mathf.Cos(_angle * Mathf.Deg2Rad);
        float y = _radius * Mathf.Sin(_angle * Mathf.Deg2Rad);
        transform.position = _attachPoint + new Vector3(x, y, 0f);

        _grapple.transform.position = new Vector3(_attachPoint.x, _attachPoint.y, 0f);
    }

    private void ShootGrapple()
    {
        _grapple.transform.parent = _playerRoot;

        float distanceFromPlayer = Vector3.Distance(transform.position, _grapple.transform.position);
        float distanceToTarget = Vector3.Distance(_grapple.transform.position, _targetPosition);
        if (distanceFromPlayer < _maxGrappleDistance && distanceToTarget > 0.1f)
        {
            _grapple.transform.position = Vector3.MoveTowards(_grapple.transform.position, _targetPosition, _grappleSpeed * Time.deltaTime);
        }
        else
        {
            _currentGrappleState = _grappleStates._retracting;
        }
    }

    private void RetractGrapple()
    {
        _grapple.transform.position = Vector3.MoveTowards(_grapple.transform.position, _grappleSlot.transform.position, _grappleSpeed * Time.deltaTime);
        if(Vector3.Distance(_grapple.transform.position, _grappleSlot.transform.position) < 0.1f)
        {
            _grapple.transform.parent = _grappleSlot.transform;
            _grapple.transform.position = _grappleSlot.transform.position;
            _currentGrappleState = _grappleStates._idle;
        }
    }

    public void AttachGrapple(Vector3 attachPoint)
    {
        _attachPoint = attachPoint;
        if (_currentGrappleState == _grappleStates._shooting)
        {
            _grapple.transform.position = _attachPoint;
            _radius = Vector3.Distance(transform.position, _attachPoint);
            Vector3 dir = (transform.position - _attachPoint).normalized;
            _angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            _currentGrappleState = _grappleStates._attached;
        }
    }
}
