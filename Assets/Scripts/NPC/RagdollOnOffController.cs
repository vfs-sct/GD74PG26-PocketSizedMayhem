using CharacterMovement;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class RagdollOnOffController : MonoBehaviour
{ 
    [SerializeField] private Animator _npcAnimator;
    [SerializeField] private Collider[] _ragdollColliders;
    [SerializeField] private Rigidbody[] _ragdollRigidbodies;
    [SerializeField] private CharacterMovement3D _characterMovement;

    [SerializeField] private int _bounceForce = 10;

    public void RagdollModeOn()
    {
        _npcAnimator.enabled = false;

        foreach (Collider collider in _ragdollColliders)
        {
            collider.enabled = true;
        }

        foreach (Rigidbody rigid in _ragdollRigidbodies)
        {
            rigid.isKinematic = false;
        }
        GetComponent<Rigidbody>().isKinematic = true;
        _characterMovement.enabled = false;
    }

    public void RagdollModeOff()
    {
        _npcAnimator.enabled = true;

        foreach (Collider collider in _ragdollColliders)
        {
            collider.enabled = false;
        }

        foreach (Rigidbody rigid in _ragdollRigidbodies)
        {
            rigid.isKinematic = true;
        }
        GetComponent<Rigidbody>().isKinematic = false;
        _characterMovement.enabled = true;
    }

    public void DeathBounce()
    {
        foreach (Rigidbody rigid in _ragdollRigidbodies)
        {
            rigid.AddForce(Vector3.up * _bounceForce, ForceMode.Impulse);
        }
    }
}
