using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZ : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<NewNpcBehavior>( out NewNpcBehavior hehe))
            other.gameObject.SetActive(false);
    }
}
