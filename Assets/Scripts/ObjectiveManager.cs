using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ObjectiveManager : MonoBehaviour, ISubject
{
    public TextMeshProUGUI textArea;
    public TextMeshProUGUI victoryText;
    public GameObject prefab;
    public GameObject westSpawnPoint, eastSpawnPoint;
    public GameObject nextRoundButton;
    public int stage;
    public Castle castle;
    public bool gateHouseUpgraded;
    public int[] enemyCount;
    public List<Enemy> enemies;

    private readonly List<IObserver> observers = new();

    // Start is called before the first frame update
    void Start()
    {
        nextRoundButton.SetActive(false);
        SetObjective("Build the castle");
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (enemies.Count == 0 && castle.gateHouse.level > 0)
        {
            nextRoundButton.SetActive(true);
            SetObjective("Start the wave");
        }
    }

    public void NextStage()
    {
        for (var i = 0; i < enemyCount[stage]; i++)
        {
            var isLeft = Random.value < 0.5f;
            var enemy = Instantiate(prefab,
                isLeft ? westSpawnPoint.transform.position : eastSpawnPoint.transform.position, Quaternion.identity);
            enemies.Add(enemy.GetComponent<Enemy>());
        }

        stage++;
        
        SetObjective("Defend the castle.");
        
        nextRoundButton.SetActive(false);
        
        Notify();
    }

    private void SetObjective(params string[] objectives)
    {
        if (objectives.Length == 0)
        {
            objectives = new string[] { "All completed" };
        }

        StringBuilder sb = new();
        sb.Append("Stage: " + stage + "\n");
        foreach (var item in objectives)
        {
            sb.Append("• " + item + "\n");
        }
        textArea.text = sb.ToString();
    }

    public void Attach(IObserver observer)
    {
        observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void Notify()
    {
        foreach (var observer in observers)
        {
            observer.Refresh();
        }
    }
}