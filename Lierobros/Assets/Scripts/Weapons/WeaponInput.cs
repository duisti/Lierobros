using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInput : MonoBehaviour {

	[SerializeField]
	private bool firing = false;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		firing = Input.GetButton("Fire1");
    }

	public bool GetFiringState() {
		return firing;
	}
}
