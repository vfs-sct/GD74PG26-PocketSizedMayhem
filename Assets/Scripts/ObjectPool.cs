
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    [SerializeField] private GameObject _pooledObject;

    private List<GameObject> pooledObjects = new List<GameObject>();
    [SerializeField] private int _poolAmount;


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
            GameObject obj = Instantiate(_pooledObject);
            obj.transform.parent = gameObject.transform;
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (pooledObjects[i].GetComponent<VisualEffect>().aliveParticleCount != 0)
            {
                continue;
            }
            else
            {
                pooledObjects[i].SetActive(true);
                return pooledObjects[i];
            }
        }
        return null;
    }
}
