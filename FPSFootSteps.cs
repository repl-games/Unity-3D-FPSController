using UnityEngine;

public class FPSFootSteps : MonoBehaviour {
  [SerializeField]
  private AudioClip[] grassSteps;
  // Add more clips

  private AudioSource audioSource;
  private TerrainDetector terrainDetector;

  private bool playClipCooldown = false;
  private float walkStepInterval = 0.6f;
  private float runStepInterval = 0.4f;

  void Awake() {
    audioSource = GetComponent<AudioSource>();
    terrainDetector = new TerrainDetector();
  }

  public void Step(bool isRunning) {
    if (!playClipCooldown) {
      float stepInterval = walkStepInterval;
      if (isRunning) {
        stepInterval = runStepInterval;
      }

      playClipCooldown = true;
      audioSource.PlayOneShot(GetStepClip());
      // NOTE: Utilities.DoAfter is defined in: github.com/repl-games/Unity-DoAfter-CSharp
      StartCoroutine(Utilities.DoAfter(stepInterval, ()=> playClipCooldown = false));
    }
  }

  private AudioClip GetStepClip() {
    // The order of the terrain will return in the order they show up in the
    // layer pallete profile in the editor when you paint a texture on a terrain
    int terrainTextureIndex = terrainDetector.GetActiveTerrainTextureIdx(transform.position);

    switch(terrainTextureIndex) {
      case 0: // grass
        return grassSteps[UnityEngine.Random.Range(0, grassSteps.Length)];
      default:
        return grassSteps[UnityEngine.Random.Range(0, grassSteps.Length)];
    }
  }
}
