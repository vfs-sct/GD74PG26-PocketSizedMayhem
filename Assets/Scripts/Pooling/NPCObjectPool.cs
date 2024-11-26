using PrimeTween;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;
using UnityEngine.AI;
using static NewNpcBehavior;

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
    [Header("Point Pop Up Random Range")]
    [SerializeField] private int _xMin;
    [SerializeField] private int _xMax;
    [SerializeField] private int _yMin;
    [SerializeField] private int _yMax;
    [SerializeField] Color _color;
    [SerializeField] Color _color2;
    [SerializeField] Color _color3;
    [SerializeField] Color _color4;
    [SerializeField] Image image;
    private List<GameObject> _easyPooledObjects;
    private List<GameObject> _mediumPooledObjects;
    private List<GameObject> _hardPooledObjects;
    private List<GameObject> _negativePooledObjects;
    [SerializeField]private List<NewNpcBehavior> _activeNPC;
    [SerializeField]private List<CivilianFill> _doors;
    [SerializeField]private List<GameObject> _emptyBuildings;
    [SerializeField] private GameObject _pointPopUp;
    [SerializeField] private GameObject _negativePopUp;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameObject _pointLocation;
    [SerializeField] Image _comboBar;
    [SerializeField]private int combo = 0;
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        _textMeshProUGUI.text = "Combo X" + combo;
        NavMesh.pathfindingIterationsPerFrame = 250;
    }

    void Start()
    {
        _emptyBuildings = new List<GameObject>();
        _easyPooledObjects = new List<GameObject>();
        _mediumPooledObjects = new List<GameObject>();
        _hardPooledObjects = new List<GameObject>();
        _negativePooledObjects = new List<GameObject>();
        _activeNPC = new List<NewNpcBehavior>();

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

        foreach (CivilianFill door in _doors)
        {
            door.Empty += AddToDoorList;
            door.Full += RemoveFromDoorList;
            _emptyBuildings.Add(door.gameObject);
        }

    }

    private void Update()
    {
        foreach (NewNpcBehavior civilian in _activeNPC)
        {
            if (civilian.IsGrounded &&!civilian.HasTarget() && _activeNPC.Count!=0)
            {
                civilian.SetTarget(_emptyBuildings[UnityEngine.Random.Range(0, _emptyBuildings.Count)]);
            }
        }
    }

    public GameObject GetPooledObject(TypeDifficulty npcType)
    {
        switch (npcType)
        {
            case TypeDifficulty.EASY:
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
            case TypeDifficulty.NORMAL:
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
            case TypeDifficulty.HARD:
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
        }
        return null;
    }

    public void RemoveCivilian(object sender, GameObject civilian)
    {
        if (!_activeNPC.Contains(civilian.GetComponent<NewNpcBehavior>()))
        {
            return;
        }
        
        if (!civilian.GetComponent<CivilianDeath>()._pointGiven)
        {
            _activeNPC.Remove(civilian.GetComponent<NewNpcBehavior>());
            int value = civilian.GetComponent<NewNpcBehavior>().GetPoint();
            Vector3 pointPos = Camera.main.WorldToScreenPoint(civilian.transform.position);
            pointPos += new Vector3(UnityEngine.Random.Range(_xMin,_xMax), UnityEngine.Random.Range(_yMin,_yMax),0);
            StartCoroutine(CreatePoint(pointPos, value));
            civilian.GetComponent<CivilianDeath>().OnKilled -= RemoveCivilian;
            civilian.GetComponent<CivilianDeath>()._pointGiven = true;
        }
    }
    IEnumerator CreatePoint(Vector3 civilianPos, int point)
    {
        GameObject pointPopUp;
        if (point > 0)
        {
            pointPopUp = Instantiate(_pointPopUp, civilianPos, _pointPopUp.transform.rotation, _canvas.transform);
            combo++;
            point *= combo;
        }
        else
        {
            pointPopUp = Instantiate(_negativePopUp, civilianPos, _pointPopUp.transform.rotation, _canvas.transform);
            combo = 0;
        }
        PlayerStats.Points += point;
        _textMeshProUGUI.text = "Combo X" + combo;
        pointPopUp.transform.localScale += new Vector3(combo,combo,combo)/10;
        pointPopUp.GetComponent<TextMeshProUGUI>().text = "" + point;
        Tween.Scale(pointPopUp.transform, Vector3.zero, duration: 1, ease: Ease.InOutSine);
        Tween.Position(pointPopUp.transform, _pointLocation.transform.position, duration: 1, ease: Ease.InOutSine);
        combo = Mathf.Clamp(combo, 0, 20);
        yield return null;
    }
    public void AddToCivilianList(GameObject civilian)
    {
        civilian.GetComponent<CivilianDeath>().OnKilled += RemoveCivilian;
        _activeNPC.Add(civilian.GetComponent<NewNpcBehavior>());
    }
    public void AddToDoorList(object sender, GameObject door)
    {
        _emptyBuildings.Add(door);
    }
    public void RemoveFromDoorList(object sender, GameObject door)
    {
        _emptyBuildings.Remove(door);
    }
}
