using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;
using FMODUnity;
using System.Collections;
using PrimeTween;
using TMPro;
using static NewNpcBehavior;
public class CivilianDeath : MonoBehaviour
{
    [field: SerializeField] public EventReference DeathSFX { get; set; }

    [SerializeField] private GameObject _bloodEffect;
    [SerializeField] private CapsuleCollider _capsuleCollider;
    [SerializeField] private CapsuleCollider _triggerCollider;
    [SerializeField] private RagdollOnOffController _ragdollController;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rb;
    //[SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private NewNpcBehavior _civilianBehaviour;
    [SerializeField] private int _animNo;
    
    public event EventHandler<GameObject> OnKilled;
    private bool _pointGiven;
    private bool _isFading = false;
    private float _fadeTime;
    private float _fadeThresholdTime;
    private float _fadeAmount;
    [SerializeField] private Canvas canvas;

    [SerializeField] private GameObject _pointPopUp;
    private Renderer _objectRenderer;
    private Material _objectMaterial;
    private Color _objectColor;
    private int pointValueOnDeath;
    private void Start()
    {
        pointValueOnDeath = _civilianBehaviour.GetPoint();
        canvas = GameObject.Find("HUD_Alpha").GetComponent<Canvas>();
        _objectRenderer = GetComponentInChildren<Renderer>();
        _objectMaterial = _objectRenderer.material;
        _fadeTime = 0;
        _fadeThresholdTime = 8;
        _pointGiven = false;
    }
    private void Update()
    {
        if(_isFading)
        {
            if(_fadeTime >= _fadeThresholdTime)
            {
                this.gameObject.SetActive(false);
                _ragdollController.RagdollModeOff();
                _capsuleCollider.enabled = true;
                _civilianBehaviour.enabled = true;
                _triggerCollider.enabled = true;
                _fadeTime = 0;
                _isFading = false;
                _objectMaterial.SetFloat("_Alpha", 1);
            }
            else
            {
                _fadeTime += Time.deltaTime;
                _fadeAmount = Mathf.Lerp(1, 0, _fadeTime / _fadeThresholdTime);
                _objectMaterial.SetFloat("_Alpha", _fadeAmount);
            }
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Mallet")
        {
            _capsuleCollider.enabled = false;
            _civilianBehaviour.enabled = false;
            _ragdollController.RagdollModeOn();
            _ragdollController.DeathBounce();

            GameObject impactVFX = BloodVFXPool.instance.GetPooledObject();

            if (impactVFX != null)
            {
                impactVFX.transform.position = transform.position;
                impactVFX.GetComponent<VisualEffect>().Play();
            }

            OnKilled?.Invoke(this, this.gameObject);
            
            if (!_pointGiven)
            {
                PlayerStats.Points += _civilianBehaviour.GetPoint();
                _pointGiven= true;
            }
            if (!DeathSFX.IsNull)
            {
                RuntimeManager.PlayOneShot(DeathSFX, this.gameObject.transform.position);
            }
            _triggerCollider.enabled = false;
            Vector3 pointPos = Camera.main.WorldToScreenPoint(this.gameObject.transform.position);
            pointPos.y += 100;
            GameObject point = Instantiate(_pointPopUp, pointPos, _pointPopUp.transform.rotation, canvas.transform);
            PlayerStats.Points += pointValueOnDeath;
            point.GetComponent<TextMeshProUGUI>().text = "" + pointValueOnDeath;
            Tween.Scale(point.transform, Vector3.zero, duration: 1, ease: Ease.InOutSine);
            pointPos.y += 200;
            switch (_civilianBehaviour.GetDifficultyType())
            {
                case TypeDifficulty.EASY:
                    PlayerStats.EasyCivilianKilled++;
                    break;
                case TypeDifficulty.NORMAL:
                    PlayerStats.MediumCivilianKilled++;
                    break;
                case TypeDifficulty.HARD:
                    PlayerStats.HardCivilianKilled++;
                    break;
                case TypeDifficulty.NEGATIVE:
                    PlayerStats.NegativeCivilianKilled++;
                    break;
            }
            Tween.Position(point.transform, pointPos, duration: 1, ease: Ease.OutSine);
            StartCoroutine(StartFading());
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("Door"))
        {
            this.gameObject.SetActive(false);
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            _animator.SetTrigger("GroundHit");
            this.GetComponent<NavMeshAgent>().enabled = true;
            _triggerCollider.enabled = true;
        }
    }
    IEnumerator StartFading()
    {
        yield return new WaitForSeconds(2);
        _isFading = true;
    }
}
