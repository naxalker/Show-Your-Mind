using UnityEngine;

public class AdaptiveCamera : MonoBehaviour
{
    private const float ReferenceWidth = 1284f;
    private const float ReferenceHeight = 2778f;
    private const float ReferenceOrthographicSize = 10.18f;
    private const float MinCamSize = 8.38f;

    private Camera _camera;

    void Start()
    {
        _camera = GetComponent<Camera>();

        UpdateCameraSize();
    }

    void UpdateCameraSize()
    {
        float targetAspect = ReferenceWidth / ReferenceHeight;
        float currentAspect = (float)Screen.width / Screen.height;

        float sizeAdjustment = targetAspect / currentAspect;
        _camera.orthographicSize = Mathf.Max(ReferenceOrthographicSize * sizeAdjustment, MinCamSize);
    }
}
