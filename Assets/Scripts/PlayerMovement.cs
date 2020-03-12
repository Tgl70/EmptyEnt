using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

	// This is a reference to the Rigidbody component called "rb"
	public Rigidbody rb;

	public float forwardForce = 500f;  // Variable that determines the forward force
	public float sidewaysForce = 500f;  // Variable that determines the sideways force

	private void Start()
	{

	}

	// We marked this as "Fixed"Update because we
	// are using it to mess with physics.
	void FixedUpdate()
	{
		if (Input.GetKey("w"))  // If the player is pressing the "w" key
		{
			// Add a forward force
			rb.AddForce(0, 0, forwardForce * Time.deltaTime);
		}

		if (Input.GetKey("s"))  // If the player is pressing the "s" key
		{
			// Add a backward force
			rb.AddForce(0, 0, -forwardForce * Time.deltaTime);
		}

		if (Input.GetKey("d"))  // If the player is pressing the "d" key
		{
			// Add a force to the right
			rb.AddForce(sidewaysForce * Time.deltaTime, 0, 0);
		}

		if (Input.GetKey("a"))  // If the player is pressing the "a" key
		{
			// Add a force to the left
			rb.AddForce(-sidewaysForce * Time.deltaTime, 0, 0);
		}
	}
}
