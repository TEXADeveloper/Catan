using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static event Action ToggleDice;
    public static event Action ToggleBuild;

    [SerializeField] private GameData gameData;
    [SerializeField] private WorldGeneration worldGeneration;
    [SerializeField] private Transform resourcesParent;
    [SerializeField] private Transform progressParent;
    [SerializeField] private PlayerBuilding playerBuilding;
    [SerializeField] private Button endTurnButton;
    private Coroutine cycle;
    private int firstInRound;
    private byte round = 0;
    private int diceValue = 0;
    private bool playerBuilt = false, endTurn = false;

    void Start()
    {
        worldGeneration.CreateTerrain(gameData.TerrainOrder, gameData.TerrainNumbers);
        gameData.AddPlayer(new Player());
        foreach (ResourcesDisplay i in resourcesParent.GetComponentsInChildren<ResourcesDisplay>())
            i.SetPlayer(gameData.Players[0]);
        foreach (ProgressDisplay i in progressParent.GetComponentsInChildren<ProgressDisplay>())
            i.SetPlayer(gameData.Players[0]);

        cycle = StartCoroutine(gameCycle());
    }

    private IEnumerator gameCycle()
    {
        yield return StartCoroutine(firstTurn());

        int i = firstInRound;
        while (checkVictoryPoints() != null)
        {
            yield return StartCoroutine(Game(gameData.Players[i]));
            i++;
            if (i >= gameData.Players.Count)
                i = 0;
        }
        /**
            En cualquier momento del turno:
            ** 1. Utilizar carta de progreso.
        */
    }

    private Player checkVictoryPoints()
    {
        for (int i = 0; i < gameData.Players.Count; i++)
            if (gameData.Players[i].VictoryPoints >= 10)
                return gameData.Players[i];
        return null;
    }

    private IEnumerator firstTurn()
    {
        DiceManager.DiceResult += diceResult;
        bool isDecided = false;
        int i = 0;
        List<(Player, int)> playerList = new List<(Player, int)>();
        foreach (Player p in gameData.Players)
        {
            playerList.Add((p, 0));
        }
        while (!isDecided)
        {
            //? 1. tirar dados.
            ToggleDice?.Invoke();
            yield return new WaitUntil(() => diceValue != 0);//! //FIXME: Se queda tirando los dados
            playerList[i] = (playerList[i].Item1, diceValue);
            ToggleDice?.Invoke();

            i++;
            if (i >= playerList.Count)
            {
                //? 2. Decidir quien empieza.
                int maxValue = playerList[0].Item2;
                for (int j = 1; j < playerList.Count; j++)
                    if (playerList[j].Item2 > maxValue)
                        maxValue = playerList[j].Item2;
                foreach ((Player, int) dupla in playerList)
                    if (dupla.Item2 < maxValue)
                        playerList.Remove(dupla);
                if (playerList.Count == 1)
                    isDecided = true;
                i = 0;
            }
            diceValue = 0;
        }
        DiceManager.DiceResult -= diceResult;
        //? 3. firstInRound = highestDiceNumber.
        for (int j = 0; j < gameData.Players.Count; j++)
            if (gameData.Players[j] == playerList[0].Item1)
                firstInRound = j;


        i = firstInRound;
        int multiplier = 1;
        //? 1. comienza el de mayor numero y se sigue en sentido horario.
        while (i == firstInRound && multiplier == -1)
        {
            //? Coloca 1 poblado y 1 carretera juntos.
            //construir poblado
            PlayerBuilding.PlayerBuilt += playerBuiltListener;
            while (!playerBuilt)
            {
                playerBuilding.SetBuildingType(1);
                yield return new WaitForSeconds(.1f);
            }
            playerBuilt = false;
            //construir carretera
            while (!playerBuilt)
            {
                playerBuilding.SetBuildingType(3);
                yield return new WaitForSeconds(.1f);
            }
            playerBuilt = false;
            playerBuilding.SetBuildingType(0);
            round++;
            PlayerBuilding.PlayerBuilt -= playerBuiltListener;

            //? lo mismo pero en sentido antihorario.
            i += multiplier;
            if (i >= gameData.Players.Count)
                i = 0;
            if (i == firstInRound)
            {
                multiplier = -1;
                i--;
            }
            if (i < 0)
                i = gameData.Players.Count - 1;
        }

        //* 3. Reclamar Materiales;
        foreach (Terrain t in worldGeneration.Terrains)
        {
            t.GiveInitialMaterials();
        }
    }

    private void playerBuiltListener()
    {
        playerBuilt = true;
    }

    private void diceResult(int result)
    {
        diceValue = result;
    }

    private IEnumerator Game(Player player)
    {
        ToggleDice?.Invoke();
        yield return new WaitUntil(() => diceValue != 0);
        ToggleDice?.Invoke();

        //TODO: Habilitar intercambio
        endTurnButton.interactable = true;
        ToggleBuild?.Invoke();
        yield return new WaitUntil(() => endTurn);
        ToggleBuild?.Invoke();
        endTurn = false;
        endTurnButton.interactable = false;
    }

    public void EndTurn()
    {
        endTurn = true;
    }
}
