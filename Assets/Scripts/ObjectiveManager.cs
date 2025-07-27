using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class ObjectiveManager : MonoBehaviour
{
    public GameObject[] enemies;
    [SerializeField] private TextMeshProUGUI textArea;
    public TextMeshProUGUI victoryText;
    public SoundManager soundManager;
    public Player player;
    public int objectivesCompleted;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (enemies[0] == null && enemies[1] == null && enemies[2] == null && objectivesCompleted == 0)
        {
            setObjective("Reach the castle");
            objectivesCompleted++;
        }

        if (enemies[0] == null && enemies[1] == null && enemies[2] == null && objectivesCompleted == 1)
        {
            setObjective("Build the castle");
            soundManager.PlaySoundEffect(Sound.Success, true);
            objectivesCompleted++;

        }
    }

    public void setObjective (params string[] objectives)
    {
        if (objectives.Length == 0)
        {
            setObjective("All completed");
            return;
        }

        StringBuilder sb = new();
        sb.Append("Objectives\n");
        foreach (var item in objectives)
        {
            sb.Append("• " + item + "\n");
        }

        textArea.text = sb.ToString();
    }
}
