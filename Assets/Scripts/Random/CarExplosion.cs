using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarExplosion : MonoBehaviour
{
    [field: SerializeField] public EventReference AttackSFX { get; set; }
    [SerializeField] private GameObject _explosion;
    [SerializeField] private GameObject _wheelBounce;
    [SerializeField] private MeshRenderer _meshRenderer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Mallet")
        {
            RuntimeManager.PlayOneShot(AttackSFX, this.gameObject.transform.position);
            Instantiate(_explosion,this.gameObject.transform.position,this.gameObject.transform.rotation);
            Instantiate(_wheelBounce, this.gameObject.transform.position, this.gameObject.transform.rotation);
            this.gameObject.SetActive(false);
        }
    }
}
