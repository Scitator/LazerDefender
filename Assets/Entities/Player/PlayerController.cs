using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed = 15f;
	public float padding = 1f;
	public float health = 250f;

	public GameObject lazer;
	public float projectileSpeed = 10f;
	public float projectileRepeatRate = 0.2f;

	public AudioClip fireSound;

	private float xMin = -6f, xMax = 6f;
	void Start() {
		Camera camera = Camera.main;
		float distance = transform.position.z - camera.transform.position.z;
		xMin = camera.ViewportToWorldPoint (new Vector3 (0f, 0f, distance)).x + padding;
		xMax = camera.ViewportToWorldPoint (new Vector3 (1f, 1f, distance)).x - padding;
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			InvokeRepeating("Fire", 0.0001f, projectileRepeatRate);
		}

		if (Input.GetKeyUp (KeyCode.Space)) {
			CancelInvoke("Fire");
		}

		if (Input.GetKey (KeyCode.LeftArrow)) {
			transform.position = new Vector3 (
				Mathf.Clamp (transform.position.x-speed * Time.deltaTime, xMin, xMax), 
				transform.position.y, 
				transform.position.z);
		} else if (Input.GetKey (KeyCode.RightArrow)) {
			transform.position = new Vector3 (
				Mathf.Clamp (transform.position.x+speed * Time.deltaTime, xMin, xMax), 
				transform.position.y, 
				transform.position.z);
		}
	}

	void Fire() {
		GameObject beam = Instantiate(lazer, transform.position, Quaternion.identity) as GameObject;
		beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, projectileSpeed, 0f);
		AudioSource.PlayClipAtPoint (fireSound, transform.position);
	}

	void OnTriggerEnter2D(Collider2D collider) {
		Projectile missile = collider.gameObject.GetComponent<Projectile> ();
		if (missile) {
			health -= missile.GetDamage();
			missile.Hit();
			if (health <= 0) {
				LevelManager man = GameObject.Find ("LevelManager").GetComponent<LevelManager>();
				man.LoadLevel("WinScreen");
				Destroy(gameObject);

			}
		}
	}


}
