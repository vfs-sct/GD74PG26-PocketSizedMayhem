using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.VFX;

public class Mallet : Weapon
{
    [field: SerializeField] public EventReference AttackSFX { get; set; }

    [SerializeField] private GameObject _impact;
    [SerializeField] private GameObject _target;
    [SerializeField] private float _originalStartY;
    [SerializeField] private float _targetOffset;

    private Animator _malletAnimator;

    private Vector3 _mousePos;
    private Vector3 hitpoint;
    private RaycastHit _hit;
    private LayerMask _layerMask;
    private Vector3 _hitTargetpos;
    private void Start()
    {
        _malletAnimator = GetComponent<Animator>();
    }

    public override void Fire()
    {
        if(!AttackSFX.IsNull)
        {
            RuntimeManager.PlayOneShot(AttackSFX, this.gameObject.transform.position);
        }
        _malletAnimator.SetTrigger("Swing");
        _layerMask = LayerMask.GetMask("Floor");
    }

    private void Update()
    {
        _mousePos = Input.mousePosition;

        if (!Physics.Raycast(Camera.main.ScreenPointToRay(_mousePos), out _hit, Mathf.Infinity, _layerMask))
        {
            return;
        }
        _hitTargetpos = _hit.point;
        _hitTargetpos.z += _targetOffset;
        _target.transform.position = _hitTargetpos;

        this.transform.position = _hit.point;
        hitpoint = _hit.point;
        hitpoint.y = _originalStartY;
        transform.position = hitpoint;
    }

    private void DisableColliders()
    {
        GetComponentInChildren<Collider>().enabled = false;
    }

    private void EnableColldiers()
    {
        GetComponentInChildren<Collider>().enabled = true;
    }
    public void ImpactEffects()
    {
        {
            GameObject impact = Instantiate(_impact, this.gameObject.transform.position, this.gameObject.transform.rotation);
            impact.GetComponent<VisualEffect>().Play();
        }
    }
}

