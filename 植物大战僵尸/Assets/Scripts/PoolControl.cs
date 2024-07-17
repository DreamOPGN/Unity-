using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolControl : MonoBehaviour
{
    public static PoolControl Instance;

    private Dictionary<string,List<GameObject>> Pools;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        Pools = new Dictionary<string,List<GameObject>>();
    }

    private GameObject InstantiateGameObject(string name, GameObject prefab)
    {
        GameObject GO = Instantiate(prefab);
        //enemy.transform.position = EnemySpawn.Instance.SelectSpawnPoint();
        GO.name = name;
        return GO;
    }

    public T GetFromPool<T>(string name, GameObject prefab) where T : Object
    {
        List<GameObject> Pool;
        if (!Pools.ContainsKey(name))
        {
            Pool = new List<GameObject>();
            Pools[name] = Pool;
        }
        else
        {
            Pool = Pools[name];
        }

        T gameObjectToOutput;
        if (Pool.Count == 0)
        {
            gameObjectToOutput = InstantiateGameObject(name, prefab) as T;
        }
        else
        {
            gameObjectToOutput = Pool[0] as T;
            Pool.RemoveAt(0);
        }

        return gameObjectToOutput;
    }

    public void PlaceInPool(GameObject prefab)
    {

        prefab.gameObject.SetActive(false);
        if (Pools.ContainsKey(prefab.name))
        {
            Pools[prefab.name].Add(prefab);
        }
        else
        {
            Debug.LogWarning("Trying to place an enemy in a non-existent pool: " + prefab.name);
        }
    }
}

