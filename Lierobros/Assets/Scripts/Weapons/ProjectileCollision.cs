using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class ProjectileCollision : MonoBehaviour

{
	// Start is called before the first frame update
	private GameObject owner;
	[SerializeField][Tooltip("You can build all explosion logic in to this prefab. For ex: particle, drill hole, knockback and damage can all be here(if its a splash weapon)")]
	private GameObject effectPrefab;
	[SerializeField]
	[Tooltip("You could have sound prefabs in effects, but a separate one can be placed here")]
	private GameObject soundPrefab;

	// general values
	public float initVelocity = 5f;
	public float maxLifeTime = 10f;

	public bool diesOnImpact = true;

	bool dead = false;

	// things to be remembered in private
	Rigidbody2D rg;

	private void Awake() {
		rg = this.GetComponent<Rigidbody2D>();
		rg.AddForce(transform.right * initVelocity, ForceMode2D.Impulse);
	}

	private void Start() {
		
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		print("siip");
		OnDeath(false);
	}


	void OnDeath(bool timedDeath) {
		if (effectPrefab != null) {
			GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity) as GameObject;
		}
		if (soundPrefab != null) {
			GameObject sound = Instantiate(soundPrefab, transform.position, Quaternion.identity) as GameObject;
		}
		if (diesOnImpact) {
			Destroy(this.gameObject);
		}
		if (timedDeath) Destroy(this.gameObject);
	}
	// Update is called once per frame
	void Update()
    {
		if (maxLifeTime <= 0) OnDeath(true);
		maxLifeTime -= Time.deltaTime;
    }
}
