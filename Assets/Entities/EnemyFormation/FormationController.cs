using UnityEngine;
using System.Collections;

public class FormationController : MonoBehaviour {

	public GameObject enemyPrefab;

	public float width = 10f;
	public float height = 5f;

	public float speed = 5f;
	private int direction = 1;
	private float boundaryRightEdge;
	private float boundaryLeftEdge;
	public float padding = 1f;

	public float spawnDelaySeconds = 1f;

	// Use this for initialization
	void Start () {
		Camera camera = Camera.main;
		float distance = transform.position.z - camera.transform.position.z;
		boundaryLeftEdge = camera.ViewportToWorldPoint (new Vector3 (0f, 0f, distance)).x + padding;
		boundaryRightEdge = camera.ViewportToWorldPoint (new Vector3 (1f, 1f, distance)).x - padding;

		SpawnUntilFull ();
	}

	void OnDrawGizmos() {
		float xMin, xMax, yMin, yMax;

		xMin = transform.position.x - 0.5f * width;
		xMax = transform.position.x + 0.5f * width;
		yMin = transform.position.y - 0.5f * height;
		yMax = transform.position.y + 0.5f * height;

		Gizmos.DrawLine (new Vector3 (xMin, yMin, 0f), new Vector3 (xMin, yMax, 0f));
		Gizmos.DrawLine (new Vector3 (xMin, yMax, 0f), new Vector3 (xMax, yMax, 0f));
		Gizmos.DrawLine (new Vector3 (xMax, yMax, 0f), new Vector3 (xMax, yMin, 0f));
		Gizmos.DrawLine (new Vector3 (xMax, yMin, 0f), new Vector3 (xMin, yMin, 0f));
	}
	
	// Update is called once per frame
	void Update () {

		float formationRightEdge = transform.position.x + 0.5f * width;
		float formationLeftEdge = transform.position.x - 0.5f * width;

		if (formationRightEdge > boundaryRightEdge) {
			direction = -1;
		}

		if (formationLeftEdge < boundaryLeftEdge) {
			direction = 1;
		}

		transform.position += new Vector3 (direction * speed * Time.deltaTime, 0f, 0f);
		if (AllMembersAreDead ()) {
			SpawnUntilFull();
		}
	}
	
	bool AllMembersAreDead() {
		foreach (Transform child in transform) {
			if (child.childCount > 0) {
				return false;
			}
		}
		return true;
	}

	void SpawnUntilFull() {
		Transform freePos = NextFreePosition ();
		GameObject enemy = Instantiate (enemyPrefab, freePos.position, Quaternion.identity) as GameObject;
		enemy.transform.parent = freePos;
		if (FreePositionExists ()) {
			Invoke("SpawnUntilFull", spawnDelaySeconds);
		}
	}

	bool FreePositionExists() {
		foreach (Transform child in transform) {
			if (child.childCount <= 0) {
				return true;
			}
		}
		return false;
	}

	Transform NextFreePosition() {
		foreach (Transform child in transform) {
			if (child.childCount <= 0) {
				return child;
			}
		}
		return null;
	}

}
