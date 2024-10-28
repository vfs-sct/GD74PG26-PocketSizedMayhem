using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarExplosion : MonoBehaviour
{
    [SerializeField] private GameObject _explosion;
    [SerializeField] private GameObject _wheelBounce;
    [SerializeField] private MeshRenderer _meshRenderer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Mallet")
        {
            _wheelBounce.gameObject.SetActive(true);
            _explosion.gameObject.SetActive(true);

            _meshRenderer.enabled = false;
            this.enabled = false;
        }
    }
}
