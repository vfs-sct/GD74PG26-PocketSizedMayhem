using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static EnemySpawner;

public class NPCObjectPool : MonoBehaviour
{
    public static NPCObjectPool instance;

    [SerializeField] private GameObject _easyCivilian;
    [SerializeField] private GameObject _mediumCivilian;
    [SerializeField] private GameObject _hardCivilian;
    [SerializeField] private GameObject _negativeCivilian;

    private List<GameObject> _easyPooledObjects = new List<GameObject>();
    private List<GameObject> _mediumPooledObjects = new List<GameObject>();
    private List<GameObject> _hardPooledObjects = new List<GameObject>();
    private List<GameObject> _negativePooledObjects = new List<GameObject>();

    [SerializeField]private int _easyPoolAmount ;
    [SerializeField]private int _mediumPoolAmount ;
    [SerializeField]private int _hardPoolAmount ;
    [SerializeField]private int _negativePoolAmount;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        for (int i = 0; i < _easyPoolAmount; i++)
        {
            GameObject obj = Instantiate(_easyCivilian);
            obj.transform.parent = gameObject.transform;
            _easyPooledObjects.Add(obj);
            obj.SetActive(false);
        }

        for (int i = 0; i < _mediumPoolAmount; i++)
        {
            GameObject obj = Instantiate(_mediumCivilian);
            obj.transform.parent = gameObject.transform;
            _mediumPooledObjects.Add(obj);
            obj.SetActive(false);
        }

        for (int i = 0; i < _hardPoolAmount; i++)
        {
            GameObject obj = Instantiate(_hardCivilian);
            obj.transform.parent = gameObject.transform;
            _hardPooledObjects.Add(obj);
            obj.SetActive(false);
        }

        for (int i = 0; i < _negativePoolAmount; i++)
        {
            GameObject obj = Instantiate(_negativeCivilian);
            obj.transform.parent = gameObject.transform;
            _negativePooledObjects.Add(obj);
            obj.SetActive(false);
        }

    }

    public GameObject GetPooledObject(NPCType npcType)
    {
        switch (npcType)
        {
            case NPCType.EASY:
                {
                    for (int i = 0; i < _easyPooledObjects.Count; i++)
                    {
                        if (!_easyPooledObjects[i].activeInHierarchy)
                        {
                            return _easyPooledObjects[i];
                        }

                    }
                    break;
                }
            case NPCType.MEDIUM:
                {
                    for (int i = 0; i < _mediumPooledObjects.Count; i++)
                    {
                        if (!_mediumPooledObjects[i].activeInHierarchy)
                        {
                            return _mediumPooledObjects[i];
                        }

                    }
                    break;
                }
            case NPCType.HARD:
                {
                    for (int i = 0; i < _hardPooledObjects.Count; i++)
                    {
                        if (!_hardPooledObjects[i].activeInHierarchy)
                        {
                            return _hardPooledObjects[i];
                        }

                    }
                    break;
                }
            case NPCType.NEGATIVE:
                {
                    for (int i = 0; i < _negativePooledObjects.Count; i++)
                    {
                        if (!_negativePooledObjects[i].activeInHierarchy)
                        {
                            return _negativePooledObjects[i];
                        }

                    }
                    break;
                }
        }

        return null;
    }
}
