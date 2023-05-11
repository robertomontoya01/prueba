using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour {
  public int Points;
  public ObjectType Type;
  public AudioClip AudioClip;
  public float VolumeMultiplier = 1f;

  void OnCollisionEnter2D(Collision2D collision) {
    try {
      Destroy(gameObject);
      Movement movement = collision.gameObject.GetComponent<Movement>();
      movement.ActivateObject(Type, Points, AudioClip, VolumeMultiplier);
    } catch {}
  }
}
