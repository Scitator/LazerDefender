using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {

	static MusicPlayer instance = null;

	public AudioClip startClip;
	public AudioClip gameClip;
	public AudioClip endClip;

	private AudioSource music;

	void Start() {
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			GameObject.DontDestroyOnLoad (gameObject);
			music = GetComponent<AudioSource>();
			music.clip = startClip;
			music.loop = true;
			music.Play();
		}
	}

	void OnLevelWasLoaded(int level) {
		//Debug.Log ("MusicPlayer: loaded level " + level);
		music = GetComponent<AudioSource>();
		music.Stop ();
		switch (level) {
			case 0:
				music.clip = startClip;
				break;
			case 1:
				music.clip = gameClip;
				break;
			case 2:
				music.clip = endClip;
				break;
		}
		music.loop = true;
		music.Play ();
	}
}
