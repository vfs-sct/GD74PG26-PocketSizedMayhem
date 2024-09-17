using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossStunBar : MonoBehaviour
{
    [SerializeField] private float _maxStun;
    [SerializeField] public float _stunDuration;
    private float percentage;
    [SerializeField] private Image _fillBar;
    [SerializeField] private NavMeshAgent _agent;
    private float _elapsedTime;
    private float _startTime;
    private void Start()
    {
        _maxStun = 10;
        _stunDuration = 0;
    }
    private void OnValidate()
    {

    }

    private void Update()
    {

        percentage = _stunDuration/_maxStun;
        _fillBar.fillAmount = percentage;
        Debug.Log(_fillBar.fillAmount);
        Debug.Log(percentage);
        if(_stunDuration > 0 )
        {
            _agent.isStopped = true;

        }
        else
        {
            _agent.isStopped = false;
        }
        _stunDuration -= Time.deltaTime;

        transform.rotation = Camera.main.transform.rotation;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Mallet")
        {
            _stunDuration += 3;
        }
    }
    
}
