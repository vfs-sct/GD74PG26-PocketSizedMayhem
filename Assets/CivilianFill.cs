using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianFill : MonoBehaviour
{
    private int _civilianInside = 0;

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
