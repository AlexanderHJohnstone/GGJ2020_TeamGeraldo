using UnityEngine;

public class LightBulbController : MonoBehaviour
{
    [SerializeField]
    [Range(0, 1)]
    private float intensityScale;

    public LightbulbEmission _emissionController;

    public void SetIntensityScale(float rotationPercent, int playerDirection)
    {
        if (playerDirection == -1)
            intensityScale = 1 - Mathf.InverseLerp(0.5f, 1f, rotationPercent);
        else
            intensityScale = 1 - Mathf.InverseLerp(0.5f, 0f , rotationPercent);

        if (_emissionController != null) _emissionController.SetLightLevel(intensityScale);
    }
}
