using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(BoxCollider))]
public class KillzoneController : MonoBehaviour
{
    [Header("COMPONENT REFERENCES")]
    public Transform _leftBlock;
    public Transform _rightBlock;
    public BoxCollider _collider;
    public LineRenderer _killLine;

    [Header("ELECTRICITY PROPERTIES")]
    public float _segmentDistance;
    private int _segmentCount;
    private Material _lineMaterial;
    

    [Range(5f, 100f)]
    public float _width = 10f;

    private void Start()
    {
        _lineMaterial = _killLine.sharedMaterial;
        AdjustToNewWidth();
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
}
