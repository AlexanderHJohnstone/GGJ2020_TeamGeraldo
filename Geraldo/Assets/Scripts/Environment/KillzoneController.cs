using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(BoxCollider))]
public class KillzoneController : MonoBehaviour
{
    [Range(5f, 100f)]
    public float _width = 10f;

    [Header("COMPONENT REFERENCES")]
    public Transform _leftBlock;
    public Transform _rightBlock;
    public BoxCollider _collider;
    public LineRenderer _killLine;

    [Header("ELECTRICITY PROPERTIES")]
    public float _segmentDistance;
    private int _segmentCount;
    private Material _lineMaterial;

    [Header("ANIMATION PROPERTIES")]
    public bool _animRotate;
    public float _rotateSpeed = 10f;
    private float _angle;
    public bool _animToggle;
    private bool _electricityOn = true;
    public float _onTime = 1f;
    public float _offTime = 1f;
    public bool _animMove;
    public float _moveDistance = 5f;
    public float _moveSpeed = 10f;
    public bool _startMovingRight;
    private Vector3 _startPosition;
    private bool _movingRight;
    private Vector3 _rightPosition;
    private Vector3 _leftPosition;

    private void Start()
    {
        _lineMaterial = _killLine.sharedMaterial;
        AdjustToNewWidth();
        _angle = transform.eulerAngles.z;

        if(_animToggle)
        {
            StartCoroutine(ElectricityToggle());
        }

        _startPosition = transform.position;

        _movingRight = _startMovingRight;
        _rightPosition = transform.position + transform.right * _moveDistance;
        _leftPosition = transform.position - transform.right * _moveDistance;
    }

    private void Update()
    {
        if(transform.hasChanged)
        {
            if(transform.localScale != Vector3.one)
            {
                transform.localScale = Vector3.one;
            }
            if(transform.position.z != 0f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
            }
            if(transform.eulerAngles.x != 0f || transform.eulerAngles.y != 0f)
            {
                transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z);
            }

            AdjustToNewWidth();
        }

        if (Application.isPlaying)
        {
            if (_animRotate)
            {
                _angle += _rotateSpeed * Time.deltaTime;
                transform.eulerAngles = new Vector3(0f, 0f, _angle);
            }
            if(_animMove)
            {
                if(_movingRight)
                {
                    transform.position = Vector3.MoveTowards(transform.position, _rightPosition, Time.deltaTime * _moveSpeed);
                    if(Vector3.Distance(transform.position, _rightPosition) < 0.1f)
                    {
                        _movingRight = false;
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, _leftPosition, Time.deltaTime * _moveSpeed);
                    if (Vector3.Distance(transform.position, _leftPosition) < 0.1f)
                    {
                        _movingRight = true;
                    }
                }
            }
        }
    }

    private void AdjustToNewWidth()
    {
        _leftBlock.localPosition = new Vector3(-_width / 2f, 0f, 0f);
        _rightBlock.localPosition = new Vector3(_width / 2, 0f, 0f);
        _collider.size = new Vector3(_width, 1f, 1f);
        _killLine.SetPosition(0, _leftBlock.position);
        _killLine.SetPosition(1, _rightBlock.position);

        float lineLength = Vector3.Distance(_leftBlock.position, _rightBlock.position);
        _segmentCount = Mathf.RoundToInt(lineLength / _segmentCount);

        if (_lineMaterial == null)
        {
            _lineMaterial = _killLine.sharedMaterial;
        }

        _lineMaterial.SetFloat("_Width", _width);
    }

    private void OnValidate()
    {
        AdjustToNewWidth();
    }

    private IEnumerator ElectricityToggle()
    {
        float delayTime;
        if (_electricityOn)
        {
            delayTime = _onTime;
        }
        else
        {
            delayTime = _offTime;
        }

        yield return new WaitForSeconds(delayTime);

        if(_electricityOn)
        {
            _electricityOn = false;
            _collider.enabled = false;
            _killLine.enabled = false;
        }
        else
        {
            _electricityOn = true;
            _collider.enabled = true;
            _killLine.enabled = true;
        }

        StartCoroutine(ElectricityToggle());
    }
}
