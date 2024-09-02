using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class RagdollOnOffController : MonoBehaviour
{ 
    [SerializeField] private GameObject _hips;
    [SerializeField] private int _bounceForce = 10;
    [SerializeField] private GameObject _bloodEffect;
    private Animator _npcAnimator;
    private Collider[] _ragdollColliders;
    private Rigidbody[] _ragdollRigidbodies;
    private BoxCollider _boxCollider;


    
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
            rigid.AddForce(Vector3.up * _bounceForce, ForceMode.Impulse);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Mallet")
        {  
            RagdollModeOn();
            this.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
            GameObject blood =  Instantiate(_bloodEffect,this.gameObject.transform.position, this.gameObject.transform.rotation);
            blood.GetComponent<VisualEffect>().Play();
        }
    }
}
