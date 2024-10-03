using System.Collections.Generic;
using Assets.Scripts.HarashSuperVillains.Player;
using UnityEngine;

public interface IEnemyDetector
{
    void TryDetect(GameObject player, List<Transform> detectSpots);
}
