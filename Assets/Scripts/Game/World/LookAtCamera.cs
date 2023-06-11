using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Camera main;

    void Start()
    {
        main = Camera.main;
        CameraController.MOVE_CAMERA += changeRotation;
        changeRotation();
    }

    private void changeRotation()
    {
        transform.LookAt(main.transform, Vector3.down);
    }

    void OnDisable()
    {
        CameraController.MOVE_CAMERA -= changeRotation;
    }
}
