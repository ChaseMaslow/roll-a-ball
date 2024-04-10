using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class PlayerController : MonoBehaviour
{
	private Rigidbody rb;
	private int count;
	private float movementX;
	private float movementY;
	private float jumpState = 0.0f;
	private bool onGround = true;
	private bool doubleJumpAble = false;

	public float speed = 0;
	public float jumpForce = 500;
	public TextMeshProUGUI countText;
	public GameObject winTextObject;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
		count = 0;
		SetCountText();
		winTextObject.SetActive(false);
    }

	void OnMove(InputValue movementValue)
	{
		Vector2 movementVector = movementValue.Get<Vector2>();
		movementX = movementVector.x;
		movementY = movementVector.y;
	}

	void OnJump(InputValue jumpValue)
	{
		jumpState = jumpValue.Get<float>();
	}

	void SetCountText()
	{
		countText.text = "Count: " + count.ToString();
		if (count >= 5) 
		{
			winTextObject.SetActive(true);
		}
	}

	private void FixedUpdate()
	{
		Vector3 movement = new Vector3(movementX, 0.0f, movementY);
		rb.AddForce(movement * speed);

		if (Physics.Raycast(transform.position, Vector3.down, 0.6f))
			onGround = true;
		else
			onGround = false;
		if (jumpState == 1.0f && (onGround || doubleJumpAble)) {
			Vector3 jumpVector = new Vector3(0.0f, jumpForce, 0.0f);
			rb.AddForce(jumpVector);
			jumpState = 0.0f;
			if (onGround)
				doubleJumpAble = true;
			else
				doubleJumpAble = false;
		} else if (jumpState == 1.0f) {
			jumpState = 0.0f;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("PickUp"))
		{
			other.gameObject.SetActive(false);
			count = count + 1;
			SetCountText();
		}
	}
}
