using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;
    public List<Pool> poolsList;
    public Dictionary<poolObjName, Queue<GameObject>> poolDictionary;

    [System.Serializable]
    public class Pool
    {
        public poolObjName tag;
        public GameObject prefab;
        public BasePiece type;
        public int size;
    }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        poolDictionary = new Dictionary<poolObjName, Queue<GameObject>>();

        foreach (var pool in poolsList)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject prefab = Instantiate(pool.prefab);
                prefab.SetActive(false);
                objectPool.Enqueue(prefab);
                prefab.name = pool.tag.ToString();
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(poolObjName tag, Vector3 pos, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag)) return null;
        GameObject obj = poolDictionary[tag].Dequeue();

        obj.SetActive(true);
        obj.transform.position = pos;
        obj.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(obj);
        return obj;
    }

    public BasePiece SpawnPiece(poolObjName tag)
    {
        if (!poolDictionary.ContainsKey(tag)) return null;
        GameObject prefabObj = poolDictionary[tag].Dequeue();
        BasePiece type = prefabObj.GetComponent<BasePiece>();

        prefabObj.SetActive(true);
        poolDictionary[tag].Enqueue(prefabObj);
        return type;
    }

    public void DespawnAll()
    {
        foreach (var pool in poolDictionary)
        {
            for (int i = 0; i < pool.Value.Count; i++)
            {
                GameObject pieceToDespawn = pool.Value.Dequeue();
                pieceToDespawn.SetActive(false);
                pool.Value.Enqueue(pieceToDespawn);
            }
        }
    }

    [System.Serializable]
    public enum poolObjName
    {
        BlackPawn,
        WhitePawn,
        BlackTower,
        WhiteTower,
        BlackKnight,
        WhiteKnight,
        BlackBishop,
        WhiteBishop,
        BlackQueen,
        WhiteQueen,
        BlackKing,
        WhiteKing,
    }
}
