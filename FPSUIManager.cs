using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPSUIManager : MonoBehaviour {
  [Header("UI Sprites")]
  public Sprite cursor;
  public Sprite cursorAim;

  public GameObject cameraAim;
  public TMP_Text objectNameHover;

  private bool gamePaused = false;

  void Start() {
  }

  void Awake() {
    // Assert cameraAim not nil
    if (cameraAim == null) {
      Debug.LogError("cameraAim UI canvas object must be set");
    }
  }

  public void PauseGame() {
    Time.timeScale = 0;
    gamePaused = true;
    // Enable menu UI
  }

  public void UnpauseGame() {
    // Disable menu UI
    Time.timeScale = 1;
    gamePaused = false;
  }

  public void UpdateObjectNameText(string name) {
    objectNameHover.SetText(name);
  }

  public void ObjectFocus() {
    cameraAim.GetComponent<Image>().sprite = cursorAim;
  }

  public void ObjectUnfocus() {
    cameraAim.GetComponent<Image>().sprite = cursor;
  }
}
