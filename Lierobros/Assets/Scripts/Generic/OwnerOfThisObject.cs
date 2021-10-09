using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerOfThisObject : MonoBehaviour
{

	[SerializeField]
	private GameObject owner;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public GameObject GetOwner() {
		return owner;
	}

	public bool SetOwner(GameObject g) {
		owner = g;
		if (owner == g) {
			return true;
		}
		else return false;
	}
}
