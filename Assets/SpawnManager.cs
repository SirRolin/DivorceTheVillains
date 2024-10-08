using Assets.Scripts.HarashSuperVillains.Player;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject playerPrefab;
    private GameObject currentPlayer;
    public GameObject getCurrentPlayer()
    {
        return currentPlayer;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        currentPlayer = Instantiate(playerPrefab, new Vector3(0,1,0), Quaternion.identity);
        currentPlayer.GetComponent<DeathManager>().OnDeath += OnDeath;
    }

    private void OnDeath(){
        Destroy(currentPlayer);
        SpawnPlayer();
    }
}
