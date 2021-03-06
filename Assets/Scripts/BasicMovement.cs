using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour {

	public CharacterController2D controller;
	public bool jump = false;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonUp("Jump"))
		{
			GetComponent<Rigidbody2D>().isKinematic = false;
         	transform.parent = null;
			jump = true;
		}

	}

	void FixedUpdate ()
	{
		// Move our character
		controller.Jump(jump);
		jump = false;
	}
}