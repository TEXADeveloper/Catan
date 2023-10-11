using UnityEngine;
using TMPro;

public class PlayerDisplay : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private GameData gameData;
    [SerializeField] private int i;
    [Header("Components")]
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text materialCards;
    [SerializeField] private TMP_Text progressCards;
    [SerializeField] private TMP_Text longestRoad;
    [SerializeField] private TMP_Text usedKnights;
    [SerializeField] private TMP_Text victoryPoints;

    private Player p;


    void Start()
    {
        p = gameData.GetPlayer(i);
        if (p == null)
            Destroy(this.gameObject);
        else
        {
            updateInfo();

            p.ResourcesUpdated += updateInfo;
            p.CardsUpdated += updateInfo;
            p.PointsUpdated += updateInfo;
        }
    }

    private void updateInfo()
    {
        playerName.text = p.Name;

        int progress = 0, materials = 0;
        for (int j = 0; j < 5; j++)
        {
            materials += p.GetResources(j);
            progress += p.GetCards(j);
        }

        materialCards.text = materials.ToString();
        progressCards.text = progress.ToString();
        longestRoad.text = "0";
        usedKnights.text = "0";
        victoryPoints.text = p.VictoryPoints.ToString();
    }

    void OnDisable()
    {
        if (p == null)
            return;
        p.ResourcesUpdated -= updateInfo;
        p.CardsUpdated -= updateInfo;
        p.PointsUpdated -= updateInfo;
    }
}
