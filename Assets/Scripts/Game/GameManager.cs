using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private WorldGeneration worldGeneration;
    private Coroutine cycle;
    private byte firstInRound;
    private byte phase;

    void Start()
    {
        worldGeneration.CreateTerrain(gameData.TerrainOrder);
        cycle = StartCoroutine(gameCycle());
    }

    private IEnumerator gameCycle()
    {
        yield return null; //? Wait until all players are connected
        yield return new WaitUntil(() => firstTurn());
        byte i = firstInRound;
        while (checkVictoryPoints() != null)
        {
            yield return new WaitUntil(() => Game(gameData.Players[i]));
            i++;
            if (i >= gameData.Players.Length)
                i = 0;
        }
        /**
            En cualquier momento del turno:
            ** 1. Utilizar carta de progreso.
        */
    }

    private Player checkVictoryPoints()
    {
        for (int i = 0; i < gameData.Players.Length; i++)
            if (gameData.Players[i].VictoryPoints >= 10)
                return gameData.Players[i];
        return null;
    }

    private bool firstTurn()
    {
        /**
            Setup:
            ** 1. decidir quien empieza.
                *! 1. tirar dados.
                *! 2. si hay empate tirar dados de nuevo.
                *! firstInRound =  higherDiceNumber;
            ** 2. colocar en orden los pueblos y carreteras.
                *! 1. comienza el de mayor numero y se sigue en sentido horario.
                *! 2. Coloca 1 poblado y 1 carretera juntos.
                *! 3. lo mismo pero en sentido antihorario.
            ** 3. Reclamar los materiales.
        */
        return true;
    }

    private bool Game(Player player)
    {
        /**
            Ciclo de juego (por jugador):
            ** 1. Tirar dados.
             ** 2. Otorgar materiales.
            ** 3. Intercambio y compra.
                *! Al comprar posicionar las estructuras
            ** 4. 
       */
        return true;
    }
}
