using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    public GameManager instance;
    public Player player;
    public InputManager inputManager;
    public StatsTextManager statsTextManager;
    public ObjectiveManager objectiveManager;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        inputManager = GetComponent<InputManager>();
        statsTextManager = GetComponent<StatsTextManager>();
    }
}