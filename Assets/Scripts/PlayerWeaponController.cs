using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Weapon _claw;
    [SerializeField] private Weapon _mallet;

    private Weapon _activeWeapon;
    private Camera _mainCamera;

    private Vector3 _mousePos;
    private RaycastHit hit;

    void Start()
    {
        _mainCamera = Camera.main;
        _activeWeapon = _mallet;
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
