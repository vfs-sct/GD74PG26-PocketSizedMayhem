using PrimeTween;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CivilianFill : MonoBehaviour
{
    [SerializeField] private int _civilianInside = 0;
    [SerializeField] private int _civilianMaxCount = 5;
    [SerializeField] private TextMeshPro _civilianCountText;
    public event EventHandler<GameObject> Full;
    public event EventHandler<GameObject> Empty;
    LayerMask _layerMask;
    private bool _full;

    private void Start()
    {
        _full = false; 
        _layerMask |= (1 << LayerMask.NameToLayer("EasyCivilian"));
        _layerMask |= (1 << LayerMask.NameToLayer("MediumCivilian"));
        _layerMask |= (1 << LayerMask.NameToLayer("HardCivilian"));
        _layerMask |= (1 << LayerMask.NameToLayer("NegativeCivilian"));
        _civilianCountText.text = _civilianInside + "/" + _civilianMaxCount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if((_layerMask.value & (1 << other.transform.gameObject.layer)) != 0 && !_full)
        {
            _civilianInside++;
            _civilianCountText.text = _civilianInside + "/" + _civilianMaxCount;
            other.gameObject.SetActive(false);
            _civilianCountText.color = Color.green;
            if (_civilianInside == _civilianMaxCount)
            {
                _full = true;
                Full?.Invoke(this, this.gameObject);
                _civilianCountText.color = Color.red;
                StartCoroutine(ScaleText());
            }
        }
        else if((_layerMask.value & (1 << other.transform.gameObject.layer)) != 0)
        {
            other.GetComponent<NewNpcBehavior>().SetTarget(null);
        }
    }

    public int GetCivilianCount()
    {
        return _civilianInside;
    }
    public int GetCivilianMaxCount()
    {
        return _civilianMaxCount;
    }
    public bool IsFull() {

        return _full; }
    public void ResetFill()
    {
        _full= false;
        _civilianInside = 0;
        _civilianCountText.text = _civilianInside + "/" + _civilianMaxCount;
        _civilianCountText.color = Color.white;
        Empty?.Invoke(this, this.gameObject);
    }

    IEnumerator ScaleText()
    {
        Tween.ShakeLocalRotation(_civilianCountText.gameObject.transform, strength: new Vector3(0, 0, 15), duration: 5, frequency: 5);
        yield return new WaitForSeconds(5f);
        ResetFill();
    }
}
