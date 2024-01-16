using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Player player;
    [SerializeField]
    public static InputManager inputManager;
    [SerializeField]
    public static StatsTextManager statsTextManager;
    public static ObjectiveManager objectiveManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        inputManager = InputManager.instance;
        statsTextManager = GetComponent<StatsTextManager>();

    }
}