using PrimeTween;
using System.Collections;
using TMPro;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField] private float _respawnTimer;
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private Vector3 _treePosition;
    [SerializeField] private Quaternion _treeRotation;
    [SerializeField] private int _bouncePower;
    [SerializeField] private GameObject _pointPopUp;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private int point;
    private void Start()
    {
        _canvas = FindAnyObjectByType<Canvas>();
        _treePosition = transform.position;
        _treeRotation = transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Mallet")
        {
            _rigidBody.AddForce((other.gameObject.transform.position - gameObject.transform.position).normalized * _bouncePower * -1 + Vector3.up * 10, ForceMode.Impulse);
            StartCoroutine(RespawmPieces());
            Vector3 pointPos = Camera.main.WorldToScreenPoint(this.gameObject.transform.position);
            pointPos += new Vector3(UnityEngine.Random.Range(0, 300), UnityEngine.Random.Range(0, 300), 0);
            GameObject pointPopUp;
                pointPopUp = Instantiate(_pointPopUp, pointPos, _pointPopUp.transform.rotation, _canvas.transform);

            PlayerStats.Points += point;
            pointPopUp.GetComponent<TextMeshProUGUI>().text = "" + point;
            Tween.Scale(pointPopUp.transform, Vector3.zero, duration: 1, ease: Ease.InOutSine);
        }
    }

    IEnumerator RespawmPieces()
    {
        yield return new WaitForSeconds(3f);
        _rigidBody.isKinematic = true;
        Tween.Position(this.gameObject.transform, _treePosition, duration: 2);
        Tween.Rotation(this.gameObject.transform, _treeRotation, duration: 2);
        yield return new WaitForSeconds(2);
        _rigidBody.isKinematic = false;
    }
}
