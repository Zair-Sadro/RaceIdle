using UnityEngine;
using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;

public enum TileType
{
    Junk,
    Iron,
    Rubber,
    Plastic,
    Default
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

    private TileSetter _tileSetter;
    private Tween _tween;

    public TileType Type
    {
        get => type;
        set 
        { 
            type = value;
        }
        
    }
 

    public bool IsTaken { get; private set; } = false;

    [SerializeField] private MeshRenderer _tileRenderer;
    public void OnTake()
    {
        IsTaken = true;
        coll.enabled = false;
        body.isKinematic = true;
    }
    public void JumpTween(Vector3 pos,float power,Action onJumpDone=null)
    {
        _tween?.Kill();
        _tween = transform.DOJump(pos, power, 1, 0.5f).SetEase(Ease.InSine)
            .OnComplete(()=>onJumpDone?.Invoke());
    }
    private Vector3 velocity = Vector3.zero;
    public IEnumerator JumpToPos_Cor(Vector3 pos)
    {
        while (Vector3.Distance(transform.position, pos) > 1f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, pos,ref velocity, 0.4f);
            yield return null;
        }
    }
    public IEnumerator AppearFromZero(Vector3 scale,Vector3 pos,float dur)
    {
        _tween?.Kill();
        transform.localScale = Vector3.zero;
        transform.position=pos;

        yield return transform.DOScale(scale, dur).WaitForCompletion();
    }
    public void OnStorage(Transform t)
    {
        gameObject.SetActive(true);
        transform.SetParent(t);
        transform.rotation = new Quaternion(0, 0, 0, 0);
        IsTaken = true;
        coll.enabled = false;
        body.isKinematic = true;
    }
    public void OnGround()
    {
        IsTaken = false;
        coll.enabled = true;
        body.isKinematic = false;

        if(this.gameObject.activeInHierarchy) 
            StartCoroutine(TimerTillDisappear());
    }

    internal void InjectTileSetter(TileSetter tileSetter)
    {
        _tileSetter = tileSetter;
    }

    public void SetColliderActive(bool value)
    {
        coll.enabled = value;
        body.isKinematic = !value;
    }
    public void ThrowTo(Vector3 place,float duration)
    {
        _tween?.Kill();
        _tween =transform.DOJump(place, transform.position.y + 5f, 1, duration).SetEase(Ease.InSine);
        if (_Rotate) _tween =transform.DORotate(_rotationIn, duration).SetEase(Ease.InOutExpo);
    }

    public void SetMaterial(Material mat)
    {
       _tileRenderer.material = mat;
    }

    IEnumerator TimerTillDisappear()
    {
        yield return new WaitForSeconds(StaticValues.tileDisapTimer);
        if(!IsTaken) gameObject.SetActive(false);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (PlayerDetector.IsPlayer(other.gameObject))
        {
            _tileSetter.AddTile(this);
            
        }
    }



}
