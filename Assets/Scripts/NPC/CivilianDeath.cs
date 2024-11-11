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

    [SerializeField] private CapsuleCollider _capsuleCollider;
    [SerializeField] private RagdollOnOffController _ragdollController;
    [SerializeField] private GameObject _ragdollBody;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private NewNpcBehavior _civilianBehaviour;
    [SerializeField] private GameObject _pointPopUp;
    
    public event EventHandler<GameObject> OnKilled;
    private bool _pointGiven;
    private bool _isFading = false;
    private float _fadeTime;
    private float _fadeThresholdTime;
    private float _fadeAmount;
    
    private Renderer _objectRenderer;
    private Material _objectMaterial;
    private Color _objectColor;
    private int pointValueOnDeath;
    private TypeDifficulty _typeDifficulty;
    private void Start()
    {
        _typeDifficulty = _civilianBehaviour.GetDifficultyType();
        pointValueOnDeath = _civilianBehaviour.GetPoint();
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
                _ragdollBody.SetActive(false);
                _civilianBehaviour.enabled = true;
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

    IEnumerator StartFading()
    {
        yield return new WaitForSeconds(2);
        _isFading = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Mallet")
        {
            _capsuleCollider.enabled = false;
            _civilianBehaviour.enabled = false;
            _ragdollBody.SetActive(true);
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
                _pointGiven = true;
            }
            if (!DeathSFX.IsNull)
            {
                RuntimeManager.PlayOneShot(DeathSFX, this.gameObject.transform.position);
            }
            //Vector3 pointPos = Camera.main.WorldToScreenPoint(this.gameObject.transform.position);
            //pointPos.y += 100;
            //// GameObject point = Instantiate(_pointPopUp, pointPos, _pointPopUp.transform.rotation, canvas.transform);
            //PlayerStats.Points += pointValueOnDeath;
            //point.GetComponent<TextMeshProUGUI>().text = "" + pointValueOnDeath;
            //Tween.Scale(point.transform, Vector3.zero, duration: 1, ease: Ease.InOutSine);
            //pointPos.y += 200;
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
            //Tween.Position(point.transform, pointPos, duration: 1, ease: Ease.OutSine);
            StartCoroutine(StartFading());
        }
        else if (_typeDifficulty != TypeDifficulty.NEGATIVE && collision.gameObject.layer == LayerMask.NameToLayer("Door"))
        {
            OnKilled?.Invoke(this, this.gameObject);
            this.gameObject.SetActive(false);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            _animator.SetTrigger("GroundHit");
            this.GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}
