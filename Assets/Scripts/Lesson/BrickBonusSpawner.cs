using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBonusSpawner : MonoBehaviour
{
    [SerializeField] private BonusAtSpawnTime _bonus;
    [SerializeField] private GameObject _prefab;

    public void OnSpawnFinished()
    {
        _bonus.OnSpawnFinished();
    }
}
