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
        if (enemies[0] == null && enemies[1] != null && enemies[2] != null)
        {
            setObjective("Reach the castle");
        }

        if (enemies[0] == null && enemies[1] == null && enemies[2] == null )
        {
            setObjective("Build the castle");
            soundManager.PlaySoundEffect(Sound.SUCCESS, true);

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
