using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteScript : MonoBehaviour {
  public static SpriteScript instance;
  private SpriteRenderer SpriteRenderer;
  private Sprite PlayerSprite, PlayerGravitySprite, PlayerShipSprite;
  // Start is called before the first frame update

  void Awake() {
    instance = this;
  }

  void Start() {
    SpriteRenderer = GetComponent<SpriteRenderer>();
    PlayerSprite = Resources.Load<Sprite>("Player");
    PlayerGravitySprite = Resources.Load<Sprite>("PlayerGravity");
    PlayerShipSprite = Resources.Load<Sprite>("PlayerShip");
    SpriteRenderer.sprite = PlayerSprite;
  }

  // Update is called once per frame
  public void EnableGravitySprite() {
    SpriteRenderer.sprite = PlayerGravitySprite;
  }

  public void EnablePlayerSprite() {
    SpriteRenderer.sprite = PlayerSprite;
  }

  public void EnableShipSprite() {
    SpriteRenderer.sprite = PlayerShipSprite;
  }
}
