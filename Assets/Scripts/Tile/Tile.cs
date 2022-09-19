﻿using UnityEngine;
using DG.Tweening;

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
    }

    public void SetColliderActive(bool value)
    {
        coll.enabled = value;
        body.isKinematic = !value;
    }
    public void ThrowTo(Vector3 place,float duration)
    {
        transform.DOJump(place, transform.position.y + 5f, 1, duration);
    }
  //  private void OnTriggerEnter(Collider other)
  //  {
  //      if (other.gameObject.CompareTag("PlayerBlocker"))
  //          this.gameObject.SetColliderActive(false);
  //  }
  //
}
