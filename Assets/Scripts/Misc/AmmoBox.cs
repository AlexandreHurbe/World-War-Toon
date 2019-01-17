using UnityEngine;

public class AmmoBox : MonoBehaviour {

	public float radius = 3f;

    AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void CollectAmmoSound() {
        audioSource.Play();
    }
}
