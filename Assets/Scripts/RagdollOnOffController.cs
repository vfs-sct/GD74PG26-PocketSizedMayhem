using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RagdollOnOffController : MonoBehaviour
{ 
    [SerializeField] private GameObject _hips;

    private Animator _npcAnimator;
    private Collider[] _ragdollColliders;
    private Rigidbody[] _ragdollRigidbodies;
    private BoxCollider _boxCollider;
    private bool _isragdoll = false;
    
    void Start()
    {
        _npcAnimator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider>();
        GetRagdollParts();
        RagdollModeOff();
    }

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
        _boxCollider.isTrigger = true;
    }

    public void RagdollModeOff()
    {
        foreach (Collider collider in _ragdollColliders)
        {
            collider.enabled = false;
        }
        foreach (Rigidbody rigid in _ragdollRigidbodies)
        {
            rigid.isKinematic = true;
        }
        // transform.position = get.transform.position;
        _boxCollider.enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        _npcAnimator.enabled = true;
        transform.position = _hips.transform.position;
    }

    private void GetRagdollParts()
    {
        _ragdollColliders = _hips.GetComponentsInChildren<Collider>();
        _ragdollRigidbodies = _hips.GetComponentsInChildren<Rigidbody>();
    }

    public void GravityOff()
    {
        foreach (Rigidbody rigid in _ragdollRigidbodies)
        {
            rigid.useGravity = false;
        }
    }

    public void GravityOn()
    {
        foreach (Rigidbody rigid in _ragdollRigidbodies)
        {
            rigid.useGravity = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (Rigidbody rigid in _ragdollRigidbodies)
        {
            rigid.AddForce(Vector3.up * 5, ForceMode.Impulse);
        }
    }
}
