using Autodesk.Fbx;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    [SerializeField] private GameObject _malletSmashVFX;

    private List<GameObject> pooledObjects = new List<GameObject>();
    private int _poolAmount = 3;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        for(int i = 0; i < _poolAmount; i++)
        {
            GameObject obj = Instantiate(_malletSmashVFX);
            obj.transform.parent = gameObject.transform;
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            return pooledObjects[i];
        }
        return null;
    }
}
