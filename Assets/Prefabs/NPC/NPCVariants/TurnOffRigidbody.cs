using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class TurnOffRigidbody : MonoBehaviour
{
    [SerializeField] GameObject ragdoll;
    [SerializeField] GameObject ragdoll2;
    [SerializeField] GameObject ragdoll3;
    [SerializeField] CapsuleCollider _collider;
    [SerializeField] BoxCollider _boxcollider;
    private void Start()
    {
        StartCoroutine(DisableRagdoll());
    }
    IEnumerator DisableRagdoll()
    {
        yield return new WaitForSeconds(3f);
        ragdoll.SetActive(false);
        ragdoll2.SetActive(false);
        ragdoll3.SetActive(false);
        _boxcollider.enabled = false;
        _collider.enabled = true;
    }
}
