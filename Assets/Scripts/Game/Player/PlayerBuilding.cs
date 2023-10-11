using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum BuildingType
{
    none = 0,
    town = 1,
    city = 2,
    road = 3
};

public class PlayerBuilding : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    public static event Action PlayerBuilt;

    Vector2 mousePos = Vector2.zero;
    [SerializeField] private Button buildingButton;
    [SerializeField] private Animator cardAnimator;
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private float maxDistance;
    [SerializeField] private float tapThreshold;
    float timePressed = 0, timeReleased = 0;
    BuildingType type;

    Camera mainCamera;

    int turn = 1;

    void Start()
    {
        mainCamera = Camera.main;
        GameManager.ToggleBuild += toggleButton;
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
                build(); //TODO: Obtener turno
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
            result = hit.collider.GetComponent<Terrain>().Build(type, hit.point, turn, gameData.Players[0]);
        else
            type = BuildingType.none;
        if (result)
        {
            if (BuildingType.town == type)
                turn++;
            type = BuildingType.none;
            PlayerBuilt?.Invoke();
        }
    }

    private void toggleButton()
    {
        if (buildingButton.interactable)
            cardAnimator.SetTrigger("Out");
        buildingButton.interactable = !buildingButton.interactable;
    }

    void OnDisable()
    {
        GameManager.ToggleBuild -= toggleButton;
    }
}
