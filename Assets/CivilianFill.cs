using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianFill : MonoBehaviour
{
    [SerializeField] private int _civilianInside;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Civilian"))
        {
            _civilianInside++;
        }
    }

    public int GetCivilianCount()
    {
        return _civilianInside;
    }
}
