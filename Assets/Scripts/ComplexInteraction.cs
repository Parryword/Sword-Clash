using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ComplexInteraction : MonoBehaviour
{
    private ComplexSpawner spawner;

    private void Awake()
    {
        spawner = GetComponent<ComplexSpawner>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spawner.ShowUpgradePanel(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spawner.ShowUpgradePanel(false);
        }
    }
}