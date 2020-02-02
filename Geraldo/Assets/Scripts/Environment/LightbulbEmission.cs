using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightbulbEmission : MonoBehaviour
{
    public float _startIllumination = 0f;

    public MeshRenderer _fillaments;
    public MeshRenderer _vents;
    public Light _light;

    public float _completeThreshold = 0.95f;

    [ColorUsage(true, true)]
    public Color _color = Color.yellow;

    [ColorUsage(true, true)]
    public Color _inactiveColor = Color.red;

    [ColorUsage(true, true)]
    public Color _activeColor = Color.green;

    public Vector2 _lightIntensity = new Vector2(0f, 1.5f);

    public void Start()
    {
        _fillaments.material.EnableKeyword("_EmissionColor");
        _vents.materials[1].EnableKeyword("_EmissionColor");

        SetLightLevel(_startIllumination);
    }

    public void SetLightLevel (float value)
    {
        Color fillamentColor = Color.Lerp(Color.black, _color, value);

        _fillaments.material.SetColor("_EmissionColor", fillamentColor);

        if (value > _completeThreshold) _vents.materials[1].SetColor("_EmissionColor", _activeColor);
        else  _vents.materials[1].SetColor("_EmissionColor", _inactiveColor);

        _light.intensity = Mathf.Lerp(_lightIntensity.x, _lightIntensity.y, value);
    }

}
