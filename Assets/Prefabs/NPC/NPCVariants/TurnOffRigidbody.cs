using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffRigidbody : MonoBehaviour
{
    [SerializeField] GameObject ragdoll;
    private void Start()
    {
        StartCoroutine(DisableRagdoll());
    }
    IEnumerator DisableRagdoll()
    {
        yield return new WaitForSeconds(3f);
        ragdoll.SetActive(false);
    }
}
