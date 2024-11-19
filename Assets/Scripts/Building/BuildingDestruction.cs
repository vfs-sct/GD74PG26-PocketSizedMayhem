using FMODUnity;
using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NewNpcBehavior;
using UnityEngine.AI;

public class BuildingDestruction : MonoBehaviour
{
    [SerializeField] private int _force;
    [SerializeField] private List<GameObject> _pieces;
    [SerializeField] private float _respawnTimer;
    [SerializeField] private List<Quaternion> _pieceRotation;
    [SerializeField] private List<Vector3> _piecePosition;
    [SerializeField] private GameObject _shattered;
    [SerializeField] private GameObject _unShattered;
    private bool _isDestoyed;
    private int _spawnCount;
    private float _spawnWeightTotal;
    [SerializeField ]private float yOffset;
    [SerializeField ]private float _respawnSpeed;
    [SerializeField ]private CivilianFill civilianFill;

    [Header("Object References")]
    [SerializeField] private List<GameObject> _civilians;
    [SerializeField] private List<Transform> _topSpawnPoints;
    [SerializeField] private List<Transform> _leftSpawnPoints;
    [SerializeField] private List<Transform> _bottomSpawnPoints;
    [SerializeField] private List<Transform> _rightSpawnPoints;

    [Header("Civilian Type Weights")]
    [SerializeField] private float _easyCivilianWeight;
    [SerializeField] private float _mediumCivilianWeight;
    [SerializeField] private float _hardCivilianWeight;
    [SerializeField] private float _negativeCivilianWeight;
    [field: SerializeField] public EventReference DeathSFX { get; set; }
    void Awake()
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
    private void Start()
    {
        _spawnWeightTotal = (_easyCivilianWeight + _mediumCivilianWeight + _hardCivilianWeight + _negativeCivilianWeight);
        _easyCivilianWeight = (_easyCivilianWeight / _spawnWeightTotal) * 100;
        _mediumCivilianWeight = (_mediumCivilianWeight / _spawnWeightTotal) * 100;
        _hardCivilianWeight = (_hardCivilianWeight / _spawnWeightTotal) * 100;
        _negativeCivilianWeight = (_negativeCivilianWeight / _spawnWeightTotal) * 100;
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
            _spawnCount = civilianFill.GetCivilianCount();
            civilianFill.ResetFill();
            StartCoroutine(RespawmPieces());
            SpawnAtPoint();
        }
    }

    IEnumerator RespawmPieces()
    {
        yield return new WaitForSeconds(_respawnTimer);
        for (int i = 0; i < _pieces.Count; i++)
        {
            _pieces[i].GetComponent<Rigidbody>().isKinematic = true;
            _pieces[i].transform.rotation = _pieceRotation[i];
            _pieces[i].transform.position = _piecePosition[i] + Vector3.up * yOffset;
        }
        for (int i = 0; i < _pieces.Count; i++)
        {
            Tween.PositionY(_pieces[i].transform, endValue: _piecePosition[i].y, duration: _respawnSpeed, ease: Ease.OutExpo);
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(_respawnSpeed + 0.2f);
        _isDestoyed = false;
        if(_shattered!=null)
        {
            _unShattered.SetActive(true);
            _shattered.SetActive(false);
        }
    }

    public void SpawnAtPoint()
    {
        for (int i = 0; i < _spawnCount; i++)
        {
            SpawnCivilians(this.gameObject.transform);
        }
    }

    private Transform SpawnCivilians(Transform point)
    {
        float selection = Random.Range(0, 100);
        GameObject civilian;
        if (selection >= 0 && selection < _easyCivilianWeight)
        {
            civilian = NPCObjectPool.instance.GetPooledObject(TypeDifficulty.EASY);
            civilian.SetActive(true);
            civilian.transform.position = point.position;
            civilian.transform.rotation = point.rotation;
        }
        else if (selection >= _easyCivilianWeight && selection < (_easyCivilianWeight + _mediumCivilianWeight))
        {
            civilian = NPCObjectPool.instance.GetPooledObject(TypeDifficulty.NORMAL);
            civilian.SetActive(true);
            civilian.transform.position = point.position;
            civilian.transform.rotation = point.rotation;
        }
        else if (selection >= (_easyCivilianWeight + _mediumCivilianWeight) && selection < (_easyCivilianWeight + _mediumCivilianWeight + _hardCivilianWeight))
        {
            civilian = NPCObjectPool.instance.GetPooledObject(TypeDifficulty.HARD);
            civilian.SetActive(true);
            civilian.transform.position = point.position;
            civilian.transform.rotation = point.rotation;
        }
        else
        {
            civilian = NPCObjectPool.instance.GetPooledObject(TypeDifficulty.NEGATIVE);
            civilian.SetActive(true);
            civilian.transform.position = point.position;
            civilian.transform.rotation = point.rotation;
        }
        civilian.GetComponent<NewNpcBehavior>().BuildingSpawn();
        civilian.GetComponent<NavMeshAgent>().enabled = false;
        civilian.GetComponent<CivilianDeath>().enabled = false;
        civilian.GetComponent<Rigidbody>().AddForce(civilian.transform.forward * 200 + Vector3.up * 2000);
        return point;
    }
}
