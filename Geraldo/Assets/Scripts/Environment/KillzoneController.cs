using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(BoxCollider))]
public class KillzoneController : MonoBehaviour
{
    public Transform _leftBlock;
    public Transform _rightBlock;
    public BoxCollider _collider;
    public LineRenderer _killLine;

    [Range(5f, 100f)]
    public float _width = 10f;

    private void Start()
    {
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
    }

    private void OnValidate()
    {
        AdjustToNewWidth();
    }
}
