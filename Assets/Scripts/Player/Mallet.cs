using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Mallet : Weapon
{
    [SerializeField] private GameObject _impact;
    [SerializeField] private GameObject _target;
    [SerializeField] private float _originalStartY;
    [field:SerializeField] public EventReference AttackSFX {  get; set; } 

    private Animator _malletAnimator;

    private Vector3 _mousePos;
    private Vector3 hitpoint;
    private RaycastHit _hit;

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
        
    }

    private void Update()
    {
        _mousePos = Input.mousePosition;

        if (!Physics.Raycast(Camera.main.ScreenPointToRay(_mousePos), out _hit))
        {
            return;
        }

        _target.transform.position = _hit.point;

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
    public void Smoke()
    {
        {
            GameObject impact = Instantiate(_impact, this.gameObject.transform.position, this.gameObject.transform.rotation);
            impact.GetComponent<VisualEffect>().Play();
        }
    }
}

