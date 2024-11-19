using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static EnemySpawner;
using static NewNpcBehavior;

public class SwitchBuilding : MonoBehaviour
{
    [SerializeField] private GameObject _unShattered;
    [SerializeField] private GameObject _shattered;
    [SerializeField] private CivilianFill _civilianFillTracker;
    private void OnTriggerEnter(Collider other)
    {
        if(_unShattered.activeInHierarchy && other.tag == "Mallet")
        {
            _unShattered.SetActive(false);
            _shattered.SetActive(true);
        }
    }

    
}
