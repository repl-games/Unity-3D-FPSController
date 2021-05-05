using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSUIManager : MonoBehaviour {
  [Header("UI Sprites")]
  public Sprite cursor;
  public Sprite cursorAim;

  public GameObject cameraAim;
  public TMP_Text objectNameHover;

  void Start() {
    objectNameHover.SetText("Default");
  }

  void Awake() {
    // Assert cameraAim not nil
    if (cameraAim == null) {
      Debug.LogError("cameraAim UI canvas object must be set");
    }
  }

  public void UpdateCursor(string name) {
    objectNameHover.SetText(name);
  }
}
