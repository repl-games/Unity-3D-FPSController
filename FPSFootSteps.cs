using UnityEngine;

public class FPSFootSteps : MonoBehaviour {
  [SerializeField]
  private AudioClip[] grassSteps;
  // Add more clips

  private AudioSource audioSource;
  private TerrainDetector terrainDetector;

  void Awake() {
    audioSource = GetComponent<AudioSource>();
    terrainDetector = new TerrainDetector();
  }

  public void Step() {
    audioSource.PlayOneShot(GetStepClip());
  }

  private AudioClip GetStepClip() {
    // The order of the terrain will return in the order they show up in the
    // layer pallete profile in the editor when you paint a texture on a terrain
    int terrainTextureIndex = terrainDetector.GetActiveTerrainTextureIdx(transform.position);

    switch(terrainTextureIndex) {
      case 0:
        return grassSteps[UnityEngine.Random.Range(0, grassSteps.Length)];
      default:
        return grassSteps[UnityEngine.Random.Range(0, grassSteps.Length)];
    }
  }
}
