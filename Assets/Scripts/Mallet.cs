using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mallet : Weapon
{
    [SerializeField] private GameObject _target;
    [SerializeField] private float _originalStartY;

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
        GetComponent<Collider>().enabled = false;
    }

    private void EnableColldiers()
    {
        GetComponent<Collider>().enabled = true;
    }
}

