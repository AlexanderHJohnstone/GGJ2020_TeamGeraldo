﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputManager))]
public class PlayerMovementController : MonoBehaviour
{
    [Header("COMPONENET REFERENCES")]
    public Transform _playerRoot;
    private PlayerInputManager _inputManager;
    public PlayerGrappleFollow _grappleMeshFollower;
    public GameObject _grappleSlot;
    public GameObject _grappleTarget;
    public GameObject _visualGrappleSlot;
    public LineRenderer _armLine;

    private float _hDir;
    private float _lookDir;
    private Vector3 _velocity;
    private enum _grappleStates { _idle, _shooting, _retracting, _attached};
    private _grappleStates _currentGrappleState;

    //public movement fields
    [Header("MOVEMENT PROPERTIES")]
    public float _acceleration = 0.1f;
    [Range(0f, 1f)]
    public float _horizontalDrag = 0.95f;
    public float _maxHorizontalSpeed = 1f;
    public float _maxVerticalSpeed = 10f;
    public float _gravity = 10f;

    //grapple fields
    [Header("GRAPPLE PROPERTIES")]
    public float _rotationDirection;
    private float _radius;
    public float _angle;
    private Vector3 _attachPoint;
    private Vector3 _targetPosition;
    public float _grappleSpeed = 10f;
    public float _maxGrappleDistance = 5f;
    private float _currentRotationSpeed;
    public float _minimumRotationSpeed = 3f;
    private bool _canLaunchGrapple = true;
    public float _grappleTargetingCheckRadius = 5f;
    public LayerMask _attachablesLayerMask;

    [Header("DEBUGGING PROPERTIES")]
    public bool _playerMovement;

    private void Start()
    {
        _inputManager = GetComponent<PlayerInputManager>();
        _velocity = new Vector3(0f, 10f, 0f);
        _currentGrappleState = _grappleStates._idle;
        _armLine.enabled = false;
    }

    private void Update()
    {
        //poll input
        _hDir = _inputManager.GetHorizontal();
        bool grapple = _inputManager.GetGrapple();
        bool grappleReleased = _inputManager.GrappleReleased();

        if(grappleReleased)
        {
            _canLaunchGrapple = true;
        }

        if(Mathf.Abs(_hDir) > 0f)
        {
            _lookDir = Mathf.Sign(_hDir);
        }

        switch(_currentGrappleState)
        {
            case _grappleStates._idle:
                AirMovement();
                LookRotationFree();

                if (_canLaunchGrapple && grapple)
                {
                    _armLine.enabled = true;
                    _canLaunchGrapple = false;
                    Vector3 mousePosition = Input.mousePosition;
                    mousePosition.z = -Camera.main.transform.position.z;
                    Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                    _targetPosition = mouseWorldPosition;
                    _currentGrappleState = _grappleStates._shooting;
                    FindTarget();
                }
                break;
            case _grappleStates._shooting:
                AirMovement();
                LookRotationFree();
                ShootGrapple();

                if(!grapple)
                {
                    _currentGrappleState = _grappleStates._retracting;
                }
                break;
            case _grappleStates._retracting:
                AirMovement();
                LookRotationFree();
                RetractGrapple();
                break;
            case _grappleStates._attached:
                GrappleMovement();
                LookRotationGrapple();

                if(!grapple)
                {
                    Vector3 dir = (transform.position - _attachPoint).normalized;
                    if(_rotationDirection > 0f)
                    {
                        _velocity = new Vector3(-dir.y, dir.x, 0f).normalized * _currentRotationSpeed;
                    }
                    else
                    {
                        _velocity = new Vector3(dir.y, -dir.x, 0f).normalized * _currentRotationSpeed;
                    }
                    _currentGrappleState = _grappleStates._retracting;
                }
                break;
            default:
                break;
        }

        _armLine.SetPosition(0, _visualGrappleSlot.transform.position);
        _armLine.SetPosition(1, _grappleTarget.transform.position);
    }

    private void AirMovement()
    {
        if(!_playerMovement)
        {
            return;
        }

        //air control
        if (Mathf.Abs(_hDir) > 0f)
        {
            _velocity.x += _hDir * _acceleration;
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

    private void GrappleMovement()
    {
        if(_currentRotationSpeed < _minimumRotationSpeed)
        {
            _currentRotationSpeed = Mathf.Lerp(_currentRotationSpeed, _minimumRotationSpeed, Time.deltaTime * 3f);
        }

        _angle += (_currentRotationSpeed / _radius) * _rotationDirection;
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

        _grappleTarget.transform.position = new Vector3(_attachPoint.x, _attachPoint.y, 0f);
    }

    private void FindTarget()
    {
        float distance = _grappleTargetingCheckRadius;
        Vector3 direction = (_targetPosition - transform.position).normalized;
        while(distance < _maxGrappleDistance)
        {
            Vector3 testPosition = transform.position + direction * distance;
            Collider[] attachables = Physics.OverlapSphere(testPosition, _grappleTargetingCheckRadius, _attachablesLayerMask);
            if(attachables.Length > 0)
            {
                _targetPosition = attachables[0].gameObject.transform.position;
                for(int i = 1; i < attachables.Length; i++)
                {
                    float distanceToOld = Vector3.Distance(transform.position, _targetPosition);
                    float distanceToNew = Vector3.Distance(transform.position, attachables[i].gameObject.transform.position);
                    if(distanceToNew < distanceToOld)
                    {
                        _targetPosition = attachables[i].gameObject.transform.position;
                    }
                }
                return;
            }
            else
            {
                distance += _grappleTargetingCheckRadius;
            }
        }
    }

    private void ShootGrapple()
    {
        _grappleMeshFollower.GrappleStart();
        _grappleTarget.transform.parent = _playerRoot;

        float distanceFromPlayer = Vector3.Distance(transform.position, _grappleTarget.transform.position);
        float distanceToTarget = Vector3.Distance(_grappleTarget.transform.position, _targetPosition);
        if (distanceFromPlayer < _maxGrappleDistance && distanceToTarget > 0.1f)
        {
            _grappleTarget.transform.position = Vector3.MoveTowards(_grappleTarget.transform.position, _targetPosition, _grappleSpeed * Time.deltaTime);
        }
        else
        {
            _currentGrappleState = _grappleStates._retracting;
        }
    }

    public void RetractGrapple()
    {
        _grappleTarget.transform.position = Vector3.MoveTowards(_grappleTarget.transform.position, _grappleSlot.transform.position, _grappleSpeed * Time.deltaTime);
        if(Vector3.Distance(_grappleTarget.transform.position, _grappleSlot.transform.position) < 0.5f)
        {
            _armLine.enabled = false;
            _grappleMeshFollower.GrappleEnd();
            _grappleTarget.transform.parent = _grappleSlot.transform;
            _grappleTarget.transform.position = _grappleSlot.transform.position;
            _currentGrappleState = _grappleStates._idle;
        }
    }

    public void AttachGrapple(Vector3 attachPoint)
    {
        _attachPoint = attachPoint;
        if (_currentGrappleState == _grappleStates._shooting)
        {
            _grappleTarget.transform.position = _attachPoint;
            Vector3 dir = (transform.position - _attachPoint).normalized;
            _radius = Vector3.Distance(transform.position, _attachPoint);
            _angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            _currentRotationSpeed = _velocity.magnitude;

            //get angle from attach point
            float incomingAngle = Vector3.SignedAngle((attachPoint - transform.position).normalized, _velocity.normalized, Vector3.forward);

            if(Mathf.Sign(incomingAngle) > 0)
            {
                _rotationDirection = -1;
            }
            else
            {
                _rotationDirection = 1;
            }

            if(transform.position.x > _attachPoint.x)
            {
                _lookDir = -1;
            }
            else
            {
                _lookDir = 1;
            }
            _currentGrappleState = _grappleStates._attached;
        }
    }

    public void ReleaseGrapple()
    {
        _currentGrappleState = _grappleStates._retracting;
    }

    private void LookRotationFree()
    {
        //rotate to movement direction
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, 0f);
        if (_lookDir < 0f)
        {
            targetRotation = Quaternion.Euler(0f, 180f, 0f);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
    }

    private void LookRotationGrapple()
    {
        Quaternion targetRotation;
        if(_lookDir > 0f)
        {
            targetRotation = Quaternion.Euler(0f, 0f, _angle + 180f);
        }
        else
        {
            targetRotation = Quaternion.Euler(0f, 180f, -_angle);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
    }

    private void ResetPlayer()
    {
        transform.position = Vector3.zero;
        _velocity = new Vector3(0f, 10f, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Killzone"))
        {
            ResetPlayer();
        }
    }
}
