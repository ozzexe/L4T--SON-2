using UnityEngine;
using Unity.Netcode;

public class Goal : NetworkBehaviour
{
    public string team; // "Red" or "Blue"
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            gameManager.AddScore(team, 1);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            gameManager.RemoveScore(team, 1);
        }
    }
}
