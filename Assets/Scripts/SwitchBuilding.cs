using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBuilding : MonoBehaviour
{
    [SerializeField] private GameObject _unShattered;
    [SerializeField] private GameObject _shattered;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Mallet")
        {
            _unShattered.SetActive(false);
            _shattered.SetActive(true);
        }
    }
}
