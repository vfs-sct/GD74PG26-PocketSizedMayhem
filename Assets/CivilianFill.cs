using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CivilianFill : MonoBehaviour
{
    [SerializeField] private int _civilianInside = 0;
    [SerializeField] private int _civilianMaxCount = 5;
    [SerializeField] private TextMeshPro _civilianCountText;
    LayerMask _layerMask;

    private void Start()
    {
        _layerMask |= (1 << LayerMask.NameToLayer("EasyCivilian"));
        _layerMask |= (1 << LayerMask.NameToLayer("MediumCivilian"));
        _layerMask |= (1 << LayerMask.NameToLayer("HardCivilian"));
        _layerMask |= (1 << LayerMask.NameToLayer("NegativeCivilian"));
        _civilianCountText.text = _civilianInside + "/" + _civilianMaxCount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if((_layerMask.value & (1 << other.transform.gameObject.layer)) != 0 && _civilianInside < _civilianMaxCount)
        {
            _civilianInside++;
            _civilianCountText.text = _civilianInside + "/" + _civilianMaxCount;
            other.gameObject.SetActive(false);
        }
    }

    public int GetCivilianCount()
    {
        return _civilianInside;
    }

    public void ResetFill()
    {
        _civilianInside = 0;
        _civilianCountText.text = _civilianInside + "/" + _civilianMaxCount;
    }
}
