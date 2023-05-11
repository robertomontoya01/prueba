using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ObjectType {
  Coin, Gem, Speed, Balloon, Rocket, GoDown
}

public enum Gamemodes { Normal, Ship }

public class Movement : MonoBehaviour {
  private int RespawnScene;
  public float CurrentSpeed;
  private float previousSpeed;
  private float previousDrag;
  public Gamemodes CurrentGamemode;
  public Transform GroundCheckTransform;
  public float GroundCheckRadius;
  public AudioClip JumpSound;
  public float JumpSoundVolumeMultiplier = 1f;
  public LayerMask GroundMask;
  public Transform Sprite;
  Rigidbody2D rb;
  int Gravity;
  bool Jumping;
  private bool switchingGravity;

  void Start() {
    RespawnScene = 0;
    Gravity = 1;
    Jumping = false;
    switchingGravity = false;
    rb = GetComponent<Rigidbody2D>();
  }

  IEnumerator GainSpeed() {
    CurrentSpeed *= 1.2f;
    yield return new WaitForSeconds(3f);
    CurrentSpeed /= 1.2f;
  }

  void ToggleGravity() {
    previousDrag = rb.drag;
    rb.drag = 10f;
    switchingGravity = true;
    Gravity *= -1;
    rb.gravityScale = Mathf.Abs(rb.gravityScale) * Gravity;
    previousSpeed = CurrentSpeed;
    CurrentSpeed = 0f;
  }

  void EnableBalloons() {
    if (CurrentGamemode == Gamemodes.Normal) {
      SpriteScript.instance.EnableGravitySprite();
      ToggleGravity();
    }
  }

  void GoDown() {
    if (CurrentGamemode == Gamemodes.Normal) {
      SpriteScript.instance.EnablePlayerSprite();
      ToggleGravity();
    } else if (CurrentGamemode == Gamemodes.Ship) {
      SpriteScript.instance.EnablePlayerSprite();
      CurrentGamemode = Gamemodes.Normal;
    }
  }

  IEnumerator EnableShip() {
    if (CurrentGamemode == Gamemodes.Normal) {
      SpriteScript.instance.EnableShipSprite();
      Jump(15f);
      CurrentGamemode = Gamemodes.Ship;
      yield return new WaitForSeconds(6f);

      if (CurrentGamemode == Gamemodes.Ship) {
        CurrentGamemode = Gamemodes.Normal;
        SpriteScript.instance.EnablePlayerSprite();
      }
    }
  }

  void FixedUpdate() {
    transform.position +=
        Vector3.right * CurrentSpeed * Time.deltaTime;

    if (rb.velocity.y * Gravity < -24.2f)
      rb.velocity = new Vector2(rb.velocity.x, -24.2f * Gravity);

    if (rb.velocity.y * Gravity > 24.2f) {
      rb.velocity = new Vector2(rb.velocity.x, 24.2f * Gravity);
    }

    Invoke(CurrentGamemode.ToString(), 0);
  }

  bool OnGround() {
    return Physics2D.OverlapBox(
        GroundCheckTransform.position + Vector3.up -
            Vector3.up * (Gravity - 1 / -2),
        Vector2.right * 1.1f + Vector2.up * GroundCheckRadius,
        0,
        GroundMask);
  }

  bool TouchingWall() {
    return Physics2D.OverlapBox(
        (Vector2)transform.position + (Vector2.right * 0.55f),
        Vector2.up * 0.8f + (Vector2.right * GroundCheckRadius),
        0,
        GroundMask);
  }

  void Jump(float power) {
    rb.velocity = Vector2.zero;
    rb.AddForce(Vector2.up * power * Gravity, ForceMode2D.Impulse);
  }

  void Normal() {
    if (TouchingWall()) {
      SceneManager.LoadScene(RespawnScene);
    }

    if (OnGround()) {
      Jumping = false;
      Vector3 Rotation = Sprite.rotation.eulerAngles;
      Rotation.z = Mathf.Round(Rotation.z / 360) * 360;
      Sprite.rotation = Quaternion.Euler(Rotation);

      if (switchingGravity) {
        CurrentSpeed = previousSpeed;
        rb.drag = previousDrag;
        switchingGravity = false;
      }

      if (Input.GetMouseButton(0)) {
        SoundEffectScript.PlaySound(JumpSound, JumpSoundVolumeMultiplier);
        Jumping = true;
        Jump(26.6581f);
      }
    } else if (Jumping) {
      Sprite.Rotate(Vector3.back, 2 * 452.4152186f * Time.deltaTime * Gravity);
    }

    rb.gravityScale = 12.41067f * Gravity;
  }

  void Ship() { 
    if (TouchingWall()) {
      SceneManager.LoadScene(RespawnScene);
    }

    Sprite.rotation = Quaternion.Euler(0, 0, rb.velocity.y * 2);

    if (Input.GetMouseButton(0))
      rb.gravityScale = -4.314969f;
    else
      rb.gravityScale = 4.314969f;

    rb.gravityScale = rb.gravityScale * Gravity;
  }

  public void ActivateObject(ObjectType Type, int Points, AudioClip AudioClip, float VolumeMultiplier) {
    if (Type == ObjectType.GoDown) {
      SoundEffectScript.StopSound();
    }

    bool isRocketAndGetsRocket = Type == ObjectType.Rocket && CurrentGamemode == Gamemodes.Ship;
    bool isRocketAndGetsBalloon = Type == ObjectType.Balloon && CurrentGamemode == Gamemodes.Ship;

    if (!isRocketAndGetsRocket && !isRocketAndGetsBalloon) {
      SoundEffectScript.PlaySound(AudioClip, VolumeMultiplier, Type == ObjectType.Balloon);
    }

    switch (Type) {
      case ObjectType.Coin:
        ScoreManager.instance.UpdateScore(Points);
        break;
      case ObjectType.Gem:
        ScoreManager.instance.UpdateScore(Points);
        break;
      case ObjectType.Speed:
        StopCoroutine(GainSpeed());
        StartCoroutine(GainSpeed());
        ScoreManager.instance.UpdateScore(Points);
        break;
      case ObjectType.Balloon:
        EnableBalloons();
        ScoreManager.instance.UpdateScore(Points);
        break;
      case ObjectType.GoDown:
        GoDown();
        ScoreManager.instance.UpdateScore(Points);
        break;
      case ObjectType.Rocket:
        StopCoroutine(EnableShip());
        StartCoroutine(EnableShip());
        ScoreManager.instance.UpdateScore(Points);
        break;
    }
  }
}
