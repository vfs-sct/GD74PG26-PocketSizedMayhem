using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Weapon _claw;
    [SerializeField] private Weapon _mallet;

    [SerializeField] private GameObject _target;

    [SerializeField] private Material _clawTargetMaterial;
    [SerializeField] private Material _malletTargetMaterial;

    private Weapon _activeWeapon;
    private Camera _mainCamera;

    private Vector3 _mousePos;
    private RaycastHit hit;
    private LayerMask _layerMask;

    void Start()
    {
        _mainCamera = Camera.main;
        _activeWeapon = _mallet;
        _layerMask = LayerMask.GetMask("Floor");
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
            _target.GetComponent<MeshRenderer>().material = _clawTargetMaterial;
        }
        else if (_activeWeapon == _claw)
        {
            _mallet.gameObject.SetActive(true);
            _claw.gameObject.SetActive(false);
            _activeWeapon = _mallet;
            _target.GetComponent<MeshRenderer>().material = _malletTargetMaterial;
        }
    }

    public void OnSelectMallet()
    {
        _mallet.gameObject.SetActive(true);
        _claw.gameObject.SetActive(false);
        _activeWeapon = _mallet;
        _target.GetComponent<MeshRenderer>().material = _malletTargetMaterial;
    }

    public void OnSelectClaw()
    {
        _mallet.gameObject.SetActive(false);
        _claw.gameObject.SetActive(true);
        _activeWeapon = _claw;
        _target.GetComponent<MeshRenderer>().material = _clawTargetMaterial;
    }
}
