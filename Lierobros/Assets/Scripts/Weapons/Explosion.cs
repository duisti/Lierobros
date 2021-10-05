using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Destructible2D;

public class Explosion : MonoBehaviour {
	/// <summary>The layers the explosion should work on.</summary>
	public LayerMask stampMask = -1;

	/// <summary>Should the explosion stamp a shape?</summary>
	public bool Stamp = true;

	/// <summary>The paint type.</summary>
	public D2dDestructible.PaintType StampPaint;

	/// <summary>The shape of the stamp.</summary>
	public Texture2D StampShape;

	/// <summary>The stamp shape will be multiplied by this.
	/// Solid White = No Change</summary>
	public Color StampColor = Color.white;

	[HideInInspector]
	/// <summary>The size of the explosion stamp in world space. Preferably is at least 1.5x of Radius.</summary>
	Vector2 StampSize; //irrelevant to set here, this gets calculated on start and on SetValues.
	[Tooltip("5 times means visually same as the float 'radius'. We don't necessarily want it to match.")]
	public float stampMult = 5f; // 5 times means visually same as the float "radius". We don't necessarily want it to match.

	/// <summary>Randomly rotate the stamp?</summary>
	public bool StampRandomDirection = true;

	//these will be given by the parent
	public float force = 10f;
	public float radius = 5f;
	public float damage = 50f;
	[Range(0, 1)]
	public float falloff = 0.25f; // 0 = damage falls to zero when we reach max range, 1 has no falloff. Has an effect on force too.
								  // -1 means all
	public LayerMask effectMask = -1;
	bool exploded = false;
	// Start is called before the first frame update
	void Start() {
		RecalculateStampRadius();
	}

	void RecalculateStampRadius() {
		StampSize = new Vector2(radius * stampMult, radius * stampMult);
	}

	//in case we want to set new values (double damage?)
	public void SetValues(float _force, float _radius, float _damage, float _falloff) {
		force = _force;
		radius = _radius;
		damage = _damage;
		falloff = _falloff;
		RecalculateStampRadius();
	}

	public void Explode() {
		//stamp(drill a hole)
		//in case there's no stamp size yet
		if (StampSize == null) {
			RecalculateStampRadius();
		}
		exploded = true;
		if (Stamp == true) {
			var stampPosition = transform.position;
			var stampAngle = StampRandomDirection == true ? Random.Range(-180.0f, 180.0f) : 0.0f;

			D2dStamp.All(StampPaint, stampPosition, StampSize, stampAngle, StampShape, StampColor, stampMask);
		}
		//circlecast
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, effectMask);
		List<EntityInfo> alreadyCollided = new List<EntityInfo>();
		//get the info packages, conveniently ignores everything useless at the same time, and do stuff at the same time
		foreach (Collider2D c in colliders) {
			var e = c.GetComponent<EntityInfo>();
			if (e != null && !alreadyCollided.Contains(e)) {
				alreadyCollided.Add(e);
				ApplyEffects(e);
			}
		}
	}

	void ApplyEffects(EntityInfo e) {
		var rg = e.GetRigidBody();
		var heading = e.transform.position - transform.position;
		var distance = Vector2.Distance(e.transform.position, transform.position);
		var direction = heading / distance;
		var maxDistance = radius;
		if (rg != null) {

			var minForce = force * falloff;
			var maxForce = force;
			//rg.AddForceAtPosition(direction * force, transform.position, ForceMode2D.Impulse); <- doesnt seem to work so well
			rg.AddForce(direction * Mathf.FloorToInt(Mathf.Lerp(maxForce, minForce, distance / maxDistance)), ForceMode2D.Impulse);
		}
		//calculate the true damage, taking in to account distance and falloff
		var minDamage = damage * falloff;
		var maxDamage = damage;
		var trueDamage = Mathf.FloorToInt(Mathf.Lerp(maxDamage, minDamage, distance / maxDistance));
		print("Target: " + e.gameObject.name + ", Distance was: " + distance + " of Maxdistance: " + maxDistance + ", damage is " + trueDamage + " with min/max: " + minDamage + "/" + maxDamage);
		e.ApplyDamage(trueDamage);
	}

	// Update is called once per frame
	void Update() {

	}
}
