using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Weapon _claw;
    [SerializeField] private Weapon _mallet;
    [SerializeField] private GameObject _target;
    [SerializeField] private int _yHitOfset = 15;
    [SerializeField] private int _clawSpeed = 15;

    private Weapon _activeWeapon;
    private Camera _mainCamera;

    private Vector3 _mousePos;
    private Vector3 _hitpos;
    
    private RaycastHit hit;

    void Start()
    {
        _mainCamera = Camera.main;
        _activeWeapon = _mallet;
    }
    void Update()
    {
        _mousePos = Input.mousePosition;
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(_mousePos), out hit))
        {
            return;
        }

        _hitpos = hit.point;

        if (_activeWeapon == _mallet)
        {
            _hitpos.y = hit.point.y + _yHitOfset;
        }
        else
        {
            _hitpos.y = _activeWeapon.transform.position.y;
        }

        Vector3 newPosTarget = _activeWeapon.transform.position;
        newPosTarget.y = _yHitOfset;
        //_target.transform.position = newPosTarget;
    }
    public void OnFire()
    {
        _activeWeapon.Fire();
    }
    public void OnSwitchWeapon()
    {
        if (_activeWeapon == _mallet)
        {
            _mallet.gameObject.SetActive(false);
            _claw.gameObject.SetActive(true);
            _activeWeapon = _claw;
        }
        else if (_activeWeapon == _claw)
        {
            _mallet.gameObject.SetActive(true);
            _claw.gameObject.SetActive(false);
            _activeWeapon = _mallet;
        }
    }
}
