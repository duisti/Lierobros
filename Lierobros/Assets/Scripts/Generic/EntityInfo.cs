using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityInfo : MonoBehaviour
{
	Rigidbody2D rg;
	CharacterController controller;

	public float health = 100f;
	[HideInInspector]
	public float maxHealth = 100f;

	bool populated = false;

	// Start is called before the first frame update
	private void Awake() {
		Populate();
	}

	void Start()
    {
		Populate();
    }

    // Update is called once per frame
    void Update()
    {
		if (!populated) {
			Populate();
		}
	}

	void Populate() {
		if (populated) return;
		populated = true;
		rg = GetComponent<Rigidbody2D>();
		controller = GetComponent<CharacterController>();
	}

	public Rigidbody2D GetRigidBody() {
		return rg;
	}

	public CharacterController GetCharacterController() {
		return controller;
	}

	public float GetHealth() {
		return health;
	}

	public void ApplyHeal(float h, bool overheal) {
		var targetHealth = health + h;
		health = overheal ? Mathf.Clamp(targetHealth, 0, maxHealth) : targetHealth;
	}

	public void ApplyDamage(float d) {
		health -= d;
	}
}
