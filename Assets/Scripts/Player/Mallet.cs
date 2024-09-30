using System;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using PrimeTween;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class Mallet : Weapon
{
    [field: SerializeField] public EventReference AttackSFX { get; set; }

    [SerializeField] private GameObject _impact;
    [SerializeField] private GameObject _target;
    [SerializeField] private float _originalStartY;
    [SerializeField] private float _targetOffset;

    [SerializeField] private Animator _malletAnimator;
    [SerializeField] private GameObject _impactPos;
    [SerializeField] private GameObject _malletHandle;
    [SerializeField] private GameObject _floor;
    private Vector3 _mousePos;
    private Vector3 hitpoint;
    private RaycastHit _hit;
    private LayerMask _layerMask;
    private Vector3 _hitTargetpos;
    [SerializeField] private float _impactRadius;
    [SerializeField] private float _malletMovementSpeed ;
    private bool _isAttacking = false;
    public override void Fire()
    {
        if(!_isAttacking)
        {
            if (!AttackSFX.IsNull)
            {
                RuntimeManager.PlayOneShot(AttackSFX, this.gameObject.transform.position);
            }
            _malletAnimator.SetTrigger("Swing");
            _layerMask = LayerMask.GetMask("Floor");
        }
    }

    private void Update()
    {
        _mousePos = Input.mousePosition;

        if (!Physics.Raycast(Camera.main.ScreenPointToRay(_mousePos), out _hit, Mathf.Infinity, _layerMask))
        {
            return;
        }
        _hitTargetpos = _hit.point;
        _target.transform.position = _hitTargetpos;

        this.gameObject.transform.position = _hit.point;
        hitpoint = _hit.point;
        hitpoint.z -= _targetOffset;
        hitpoint.y = _originalStartY;
        _malletHandle.gameObject.transform.position = Vector3.MoveTowards(_malletHandle.gameObject.transform.position, hitpoint, _malletMovementSpeed);
    }

    public void DisableColliders()
    {
        GetComponentInChildren<Collider>().enabled = false;
        _isAttacking = false;
    }

    public void EnableColldiers()
    {
        GetComponentInChildren<Collider>().enabled = true;
        _isAttacking = true;
    }
    public void ImpactEffects()
    {
        GameObject impact = Instantiate(_impact, _impactPos.transform.position+Vector3.up, _impact.transform.rotation);
        impact.GetComponent<VisualEffect>().Play();
    }
}

