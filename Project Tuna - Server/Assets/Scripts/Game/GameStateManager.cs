using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;

public enum GameState
{
    Running,
    Waiting
}

public class GameStateManager : MonoBehaviour
{

    #region Singleton
    private static GameStateManager instance;

    public static GameStateManager Instance
    {
        get
        {
            if (instance is null)
                Debug.LogError($"{nameof(GameStateManager)}-Instance has not yet been created.");
            return instance;
        }
        set
        {
            if (instance is not null)
            {
                Debug.LogWarning($"{nameof(GameStateManager)}-Instance has already been created. Destroying new one.");
                Destroy(value);

                return;
            }
            instance = value;
        }
    }
    #endregion

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField]
    private GameState gameState = GameState.Waiting;
    public GameState CurrentGameState
    {
        get { return gameState; }
        private set { gameState = value; }
    }

    public bool isGameJoinable()
    {
        if (CurrentGameState == GameState.Running)
            return false;
        if (CurrentGameState == GameState.Waiting)
            return true;
        return false;
    }

    public string GetGameJoinabilityReason()
    {
        return GameStateToReason(CurrentGameState);
    }

    private static readonly Dictionary<GameState, string> mappingDict = new Dictionary<GameState, string>{
            {GameState.Running, "Game is currently running."},
            {GameState.Waiting, ""}
        };

    private string GameStateToReason(GameState state)
    {
        return mappingDict[state];
    }

}
