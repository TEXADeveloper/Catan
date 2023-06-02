using UnityEngine;
using UnityEngine.InputSystem;

public enum BuildingType
{
    none = 0,
    town = 1,
    city = 2,
    road = 3
};

public class PlayerBuilding : MonoBehaviour
{
    Vector2 mousePos = Vector2.zero;
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private float maxDistance;
    [SerializeField] private float tapThreshold;
    float timePressed = 0, timeReleased = 0;
    BuildingType type;

    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    public void SetBuildingType(int type)
    {
        this.type = (BuildingType)type;
    }


    public void ReadPrimary(InputAction.CallbackContext ctx)
    {
        if (type == BuildingType.none)
            return;
        mousePos = ctx.ReadValue<Vector2>();
    }

    public void ReadPrimaryContact(InputAction.CallbackContext ctx)
    {
        if (type == BuildingType.none)
            return;
        if (ctx.performed)
            timePressed = Time.time;
        else
        {
            timeReleased = Time.time;
            if (timePressed != 0 && timeReleased != 0 && timeReleased - timePressed < tapThreshold)
            {
                build();
                timePressed = 0;
                timeReleased = 0;
            }
        }
    }

    private void build()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        bool result = false;
        if (Physics.Raycast(ray, out hit, maxDistance, terrainLayer) && hit.collider.CompareTag("Terrain"))
            result = hit.collider.GetComponent<Terrain>().Build(type, hit.point);
        else
            type = BuildingType.none;
        if (result)
            type = BuildingType.none;
    }
}
