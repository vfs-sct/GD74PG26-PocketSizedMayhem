using PrimeTween;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static EnemySpawner;

public class NPCObjectPool : MonoBehaviour
{
    public static NPCObjectPool instance;

    [Header("Civilian Prefab Referances")]
    [SerializeField] private GameObject _easyCivilian;
    [SerializeField] private GameObject _mediumCivilian;
    [SerializeField] private GameObject _hardCivilian;
    [SerializeField] private GameObject _negativeCivilian;

    [Header("Civilian Pool Amounts")]
    [SerializeField]private int _easyPoolAmount ;
    [SerializeField]private int _mediumPoolAmount ;
    [SerializeField]private int _hardPoolAmount ;
    [SerializeField]private int _negativePoolAmount;

    private List<GameObject> _easyPooledObjects;
    private List<GameObject> _mediumPooledObjects;
    private List<GameObject> _hardPooledObjects;
    private List<NewNpcBehavior> _negativePooledObjects;
    [SerializeField]private List<GameObject> _activeNPC;
    [SerializeField] private GameObject _pointPopUp;
    [SerializeField] private GameObject _negativePopUp;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameObject _pointLocation;
    [SerializeField] Image _comboBar;
    [SerializeField]private float combo = 0;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        _easyPooledObjects = new List<GameObject>();
        _mediumPooledObjects = new List<GameObject>();
        _hardPooledObjects = new List<GameObject>();
        _negativePooledObjects = new List<NewNpcBehavior>();
        _activeNPC = new List<GameObject>();

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
            _negativePooledObjects.Add(obj.GetComponent<NewNpcBehavior>());
            obj.SetActive(false);
        }
    }

    private void Update()
    {
        foreach (NewNpcBehavior negativeCivilian in _negativePooledObjects)
        {
            if (!negativeCivilian.HasTarget() && _activeNPC.Count!=0)
            {
                negativeCivilian.SetTarget(_activeNPC[UnityEngine.Random.Range(0, _activeNPC.Count)]);
            }
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
                            AddToCivilianList(_easyPooledObjects[i]);
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
                            AddToCivilianList(_mediumPooledObjects[i]);
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
                            AddToCivilianList(_hardPooledObjects[i]);
                            return _hardPooledObjects[i];
                        }
                    }
                    break;
                }
            case NPCType.NEGATIVE:
                {
                    for (int i = 0; i < _negativePooledObjects.Count; i++)
                    {
                        if (!_negativePooledObjects[i].gameObject.activeInHierarchy)
                        {
                            return _negativePooledObjects[i].gameObject;
                        }
                    }
                    break;
                }
        }
        return null;
    }

    public void RemoveCivilian(object sender, GameObject civilian)
    {
        if (!_activeNPC.Contains(civilian))
        {
            return;
        }
        _activeNPC.Remove(civilian);
        if (civilian.GetComponent<CivilianDeath>()._pointGiven)
        {
            int value = civilian.GetComponent<NewNpcBehavior>().GetPoint();
            Vector3 pointPos = Camera.main.WorldToScreenPoint(civilian.transform.position);
            pointPos.y += 300;
            pointPos.x += 100;
            GameObject point;
            if (value>0)
            {
                point = Instantiate(_pointPopUp, pointPos, _pointPopUp.transform.rotation, _canvas.transform);
                combo++;
            }
            else
            {
                point = Instantiate(_negativePopUp, pointPos, _pointPopUp.transform.rotation, _canvas.transform);
                combo = 0;
            }    
            PlayerStats.Points += value;
            point.GetComponent<TextMeshProUGUI>().text = "" + value;
            Tween.Position(point.transform, _pointLocation.transform.position, duration: 2, ease: Ease.InOutSine);
            Tween.Scale(point.transform, Vector3.zero, duration: 2, ease: Ease.InOutSine);
        }
        civilian.GetComponent<CivilianDeath>().OnKilled -= RemoveCivilian;
        _comboBar.fillAmount = Mathf.Clamp(combo*5,0,100)/100;
        Debug.Log(Mathf.Clamp(combo * 5, 0, 100));
    }

    public void AddToCivilianList(GameObject civilian)
    {
        civilian.GetComponent<CivilianDeath>().OnKilled += RemoveCivilian;
        _activeNPC.Add(civilian);
    }
}
