using Assets.Scripts.HarashSuperVillains.Player;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.SpawnPlayer();
    }

    public GameObject GetCurrentPlayer()
    {
        return GameManager.Instance.GetCurrentPlayer();
    }
}