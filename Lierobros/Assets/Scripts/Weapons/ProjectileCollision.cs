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

	[Tooltip("50 as a base seems pretty good for a stock rigidbody2d")]
	public float initVelocity = 50f;
	[Tooltip("Useful if you want a failsafe to keep the projectile bouncing (try PhysicsMaterial first), recommended: 5-10% of initVelocity. " +
		"Don't set too high in case you hit a ceiling. Negative value has funny results.")]
	public float rebounceVelocity = 0f;
	[Tooltip("Seconds until object is force destroyed. Enter 0 for no expiration timer")]
	public float maxLifeTime = 10f;
	[Tooltip("A handy way if you want the projectile to bounce around; 0 = infinite bounces, explode on impact = 1, more means that many bounces...")]
	public int expireAfterBounces = 1;

	bool dead = false;

	// things to be remembered in private
	Rigidbody2D rg;

	private void Awake() {
		if (maxLifeTime == 0) {
			maxLifeTime = Mathf.Infinity;
		}
		if (expireAfterBounces == 0) {
			expireAfterBounces = 1111111;
		}
		rg = this.GetComponent<Rigidbody2D>();
		rg.AddForce(transform.right * initVelocity, ForceMode2D.Impulse);
	}

	private void Start() {
		
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		print("Collided with: " + collision.gameObject);

		if (rebounceVelocity != 0) {
			rg.AddForce(transform.up * rebounceVelocity, ForceMode2D.Impulse);
		}
		OnDeath(false);
		expireAfterBounces -= 1;
	}


	void OnDeath(bool timedDeath) {
		if (effectPrefab != null) {
			GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity) as GameObject;
		}
		if (soundPrefab != null) {
			GameObject sound = Instantiate(soundPrefab, transform.position, Quaternion.identity) as GameObject;
		}
		if (expireAfterBounces <= 0) {
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
