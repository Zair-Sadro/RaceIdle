using UnityEngine;
using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;

public enum TileType
{
    Junk,
    Iron,
    Rubber,
    Plastic
}

[RequireComponent(typeof(Rigidbody))]
public class Tile : MonoBehaviour
{
    [SerializeField] private TileType type;
    [SerializeField] private Collider coll;
    [SerializeField] private Rigidbody body;

    [Space(20)]
    [Header("Tile Move Options")]

    [SerializeField] private bool _Rotate;
    [SerializeField,HideIf("_Rotate",false)] private Vector3 _rotationIn;
    [SerializeField] private Ease _ease;

    [Zenject.Inject] private TileSetter _tileSetter;

    public TileType Type => type;

    private bool _isTaken = false;

    public bool IsTaken => _isTaken;

    public void OnBack()
    {
        _isTaken = true;
        coll.enabled = false;
        body.isKinematic = true;
    }

    public void OnGround()
    {
        _isTaken = false;
        coll.enabled = true;
        body.isKinematic = false;

        if(this.gameObject.activeInHierarchy) 
            StartCoroutine(TimerTillDisappear());
    }

    public void SetColliderActive(bool value)
    {
        coll.enabled = value;
        body.isKinematic = !value;
    }
    public void ThrowTo(Vector3 place,float duration)
    {

        transform.DOJump(place, transform.position.y + 5f, 1, duration).SetEase(_ease);
        if (_Rotate) transform.DORotate(_rotationIn, duration).SetEase(Ease.InOutExpo);
    }

    IEnumerator TimerTillDisappear()
    {
        yield return new WaitForSeconds(StaticValues.tileDisapTimer);
        if(!_isTaken) gameObject.SetActive(false);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (PlayerDetector.IsPlayer(other.gameObject))
        {
            _tileSetter.AddTile(this);
            Debug.Log($"{this.name} player collision detect");
        }
    }



}
