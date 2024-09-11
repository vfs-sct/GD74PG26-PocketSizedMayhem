using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private CivilianManager _civilianManager;

    [SerializeField] public List<RegularCriminalBehaviour> _regularEnemiesList;
    private void Start()
    {
        _regularEnemiesList = new List<RegularCriminalBehaviour>();
    }
    private void Update()
    {
        foreach (RegularCriminalBehaviour criminal in _regularEnemiesList)
        {
            if(!criminal.HasTarget())
            {
                criminal.SetTarget(ClosestCivilian(criminal));
            }
        }
    }

    public GameObject ClosestCivilian(RegularCriminalBehaviour enemy)
    {
        float distance = enemy.GetDetectionRadius();
        GameObject closest = null;
        foreach(GameObject civilian in _civilianManager._civilians)
        {
            float compareDist;
            compareDist = Vector3.Distance(enemy.gameObject.transform.position, civilian.gameObject.transform.position);
            if(compareDist < distance)
            {
                distance = compareDist;
                closest = civilian.gameObject;
            }
        }
        return closest;
    }
    public void AddToEnemyList(GameObject enemy)
    {
        enemy.GetComponent<EnemyDeath>().OnKilled += RemoveEnemy;
        _regularEnemiesList.Add(enemy.GetComponent<RegularCriminalBehaviour>());
    }

    public void RemoveEnemy(object sender, GameObject enemy)
    {
        if (!_regularEnemiesList.Contains(enemy.GetComponent<RegularCriminalBehaviour>()))
        {
            return;
        }
        _regularEnemiesList.Remove(enemy.GetComponent<RegularCriminalBehaviour>());
        enemy.GetComponent<EnemyDeath>().OnKilled -= RemoveEnemy;
    }
}
