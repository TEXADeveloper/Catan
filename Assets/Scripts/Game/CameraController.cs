using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed;
    private Vector2 primaryPos = Vector2.zero;
    private bool primaryTouching = false;

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float mouseZoomSpeed;
    [SerializeField] private float minFOV;
    [SerializeField] private float maxFOV;
    private Vector2 secondaryPos = Vector2.zero;
    private bool secondarytouching = false;

    public void ReadPrimary(InputAction.CallbackContext ctx)
    {
        Vector2 delta = ctx.ReadValue<Vector2>() - primaryPos;
        primaryPos = ctx.ReadValue<Vector2>();
        if (primaryTouching && !secondarytouching)
            rotate(delta.y);
    }

    private void rotate(float delta)
    {
        Vector3 rotation = transform.rotation.eulerAngles + new Vector3(-delta * rotationSpeed, 0f, 0f);
        rotation.x = Mathf.Clamp(rotation.x, 20, 90);
        transform.eulerAngles = rotation;
    }

    public void ReadPrimaryContact(InputAction.CallbackContext ctx)
    {
        primaryTouching = ctx.performed;
    }


    public void ReadSecondary(InputAction.CallbackContext ctx)
    {
        secondaryPos = ctx.ReadValue<Vector2>();
    }

    private Coroutine zoomCoroutine;
    public void ReadSecondaryContact(InputAction.CallbackContext ctx)
    {
        secondarytouching = ctx.performed;

        if (secondarytouching)
            zoomCoroutine = StartCoroutine(zoom());
        else if (!secondarytouching)
            StopCoroutine(zoomCoroutine);
    }

    private IEnumerator zoom()
    {
        float prevDistance = 0, distance = 0, distanceDelta = 0;
        while (true)
        {
            distance = Vector2.Distance(primaryPos, secondaryPos);
            distanceDelta = prevDistance - distance;
            if (distance > prevDistance)
            {
                Camera.main.fieldOfView += distanceDelta * zoomSpeed;
                Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, minFOV, maxFOV);
            }
            else if (distance < prevDistance)
            {
                Camera.main.fieldOfView -= distanceDelta * zoomSpeed;
                Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, minFOV, maxFOV);
            }
            prevDistance = distance;
            yield return null;
        }
    }

    public void ReadPCZoom(InputAction.CallbackContext ctx)
    {
        float delta = ctx.ReadValue<float>();
        Camera.main.fieldOfView -= delta * mouseZoomSpeed;
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, minFOV, maxFOV);
    }
}
