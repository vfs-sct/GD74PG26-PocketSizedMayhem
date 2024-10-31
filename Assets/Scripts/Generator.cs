using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UI;

public class Generator : MonoBehaviour
{
    [SerializeField] private GameObject _spark;
    [SerializeField] private Image _fillBar;

    [SerializeField] List<GameObject> _shields;

    private float health;

    void Start()
    {
        health = 100;
    }

    void Update()
    {
        _fillBar.fillAmount = health/100;
        if (health <= 0)
        {
            health = 0;
            _spark.SetActive(true);
            _shields[0].SetActive(false);
        }
        else
        {
            _spark.SetActive(false);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        health--;
        health--;
    }
}
