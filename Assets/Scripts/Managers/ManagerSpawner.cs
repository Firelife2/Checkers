using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerSpawner : MonoBehaviour
{
    [SerializeField] GameObject localGameManagerPrefab, networkManagerPrefab;

    public void SpawnLocalGameManager()
    {
        Instantiate(localGameManagerPrefab);
    }

    public void SpawnNetworkManager()
    {
        Instantiate(networkManagerPrefab);
    }
}
