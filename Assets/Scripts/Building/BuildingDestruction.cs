using FMODUnity;
using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class BuildingDestruction : MonoBehaviour
{
    [SerializeField] private int _force;
    [SerializeField] private List<GameObject> _pieces;
    [SerializeField] private GameObject _civilians;
    [SerializeField] private float _respawnTimer;
    [SerializeField] private List<Quaternion> _pieceRotation;
    [SerializeField] private List<Vector3> _piecePosition;
    private bool _isDestoyed;
    [SerializeField ]private float yOffset;

    [field: SerializeField] public EventReference DeathSFX { get; set; }
    void Start()
    {
        _piecePosition = new List<Vector3>();
        _pieceRotation = new List<Quaternion>();
        _isDestoyed = false;
        for (int i = 0; i < _pieces.Count; i++)
        {
            _pieceRotation.Add(_pieces[i].transform.rotation);
            _piecePosition.Add(_pieces[i].transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Mallet" && !_isDestoyed)
        {
            RuntimeManager.PlayOneShot(DeathSFX, this.gameObject.transform.position);
            this.GetComponent<Rigidbody>().isKinematic = false;
            foreach (GameObject piece in _pieces)
            {
                piece.GetComponent<Rigidbody>().isKinematic = false;
                Vector3 direction = Random.insideUnitCircle.normalized;
                piece.GetComponent<Rigidbody>().AddForce(direction * _force, ForceMode.Impulse);
            }
            _isDestoyed = true;
            StartCoroutine(RespawmPieces());
            //StartCoroutine(AssignDebri());
        }
    }

    IEnumerator RespawmPieces()
    {
        Debug.Log("sdd");
        yield return new WaitForSeconds(_respawnTimer);
        for (int i = 0; i < _pieces.Count; i++)
        {
            _pieces[i].GetComponent<Rigidbody>().isKinematic = true;
            _pieces[i].transform.rotation = _pieceRotation[i];
            _pieces[i].transform.position = _piecePosition[i] + Vector3.up * yOffset;
        }
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < _pieces.Count; i++)
        {
            Tween.PositionY(_pieces[i].transform, endValue: _piecePosition[i].y, duration: 2, ease: Ease.InExpo);
            yield return new WaitForSeconds(0.5f);
        }
        _isDestoyed = false;
    }

    IEnumerator AssignDebri()
    {
        yield return new WaitForSeconds(1f);
        foreach (GameObject piece in _pieces)
        {
            piece.layer = LayerMask.NameToLayer("Debris");
        }
    }
}
