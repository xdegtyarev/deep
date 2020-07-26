using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour
{
	public bool isLanded;
	public bool isMovingRight;
	public bool isMovingLeft;
	public bool isMovingUp;
	public bool isMovingDown;

	public bool isLaddered;
	public float horizontalSpeed;
	public Rigidbody2D body;
	public Animator animator;
	public Rigidbody2D floors;
	void OnEnable()
	{
		TouchManager.TouchBeganEvent += OnTouchBegan;
		TouchManager.TouchEndedEvent += OnTouchEnd;
	}

	void OnTouchEnd(TouchInfo touch)
	{
		isMovingLeft = false;
		isMovingRight = false;
	}

	void OnTouchBegan(TouchInfo touch)
	{
		if (isLanded) {
			if (touch.position.x > Screen.width / 2f) {
				MoveRight();
			} else {
				MoveLeft();
			}
		}
	}
	Collider2D groundToIgnore;
	void FixedUpdate(){
		if(isLanded){
			Debug.DrawLine(transform.position,(Vector2)transform.position+dir);
			if(isMovingRight){
				transform.localScale = new Vector3(1,1,1);
				body.velocity = dir*horizontalSpeed*Time.fixedDeltaTime;
			}
			if(isMovingLeft){
				transform.localScale = new Vector3(-1,1,1);
				body.velocity = -dir*horizontalSpeed*Time.fixedDeltaTime;
			}
		}
		if(isLaddered){
			rigidbody2D.gravityScale = 0f;
			if(isMovingUp){
				if(groundToIgnore == null){
					groundToIgnore = Physics2D.Raycast(transform.position,Vector2.up,10f,1<<8).collider;
					Debug.Log(groundToIgnore);
				}
				if(transform.position.y - groundToIgnore.transform.position.y < 0.2f){
					Physics2D.IgnoreLayerCollision(12,8,true);	
				}else{
					Physics2D.IgnoreLayerCollision(12,8,false);
					groundToIgnore = null;
				}
				body.velocity = Vector2.up * horizontalSpeed * Time.fixedDeltaTime;
			} else if(isMovingDown){
				if(groundToIgnore == null){
					groundToIgnore = Physics2D.Raycast(transform.position,-Vector2.up,10f,1<<8).collider;
					Debug.Log(groundToIgnore);
				}
				if(transform.position.y - groundToIgnore.transform.position.y > 0.2f){
					Physics2D.IgnoreLayerCollision(12,8,true);	
				}else{
					Physics2D.IgnoreLayerCollision(12,8,false);
					groundToIgnore = null;
				}
				body.velocity = -Vector2.up * horizontalSpeed * Time.fixedDeltaTime;
			} else {
				Physics2D.IgnoreLayerCollision(12,8,false);
			}
		}
		else{
			rigidbody2D.gravityScale = 1f;
			groundToIgnore = null;
			Physics2D.IgnoreLayerCollision(12,8,false);
		}
		isLanded = false;
		isLaddered = false;
		body.isKinematic = false;

	}

	void Update(){
		ProcessKeyboardInput();
		animator.SetFloat("WalkDir",Mathf.Abs(body.velocity.x));
	}

	void ProcessKeyboardInput(){
		if(Input.GetKeyDown(KeyCode.LeftArrow)){
			MoveLeft();
		}
		if(Input.GetKeyDown(KeyCode.RightArrow)){
			MoveRight();
		}
		if(Input.GetKeyDown(KeyCode.UpArrow)){
			isMovingUp = true;
		}
		if(Input.GetKeyDown(KeyCode.DownArrow)){
			isMovingDown = true;
		}
		if(Input.GetKeyUp(KeyCode.LeftArrow)){
			isMovingLeft = false;
		}
		if(Input.GetKeyUp(KeyCode.RightArrow)){
			isMovingRight = false;
		}
		if(Input.GetKeyUp(KeyCode.UpArrow)){
			isMovingUp = false;
		}
		if(Input.GetKeyUp(KeyCode.DownArrow)){
			isMovingDown = false;
		}
	}	

	Vector2 dir;
	void OnCollisionStay2D(Collision2D coll) {
		if(coll.gameObject.layer == 10){
			Debug.DrawLine(transform.position,(Vector2)transform.position+coll.contacts[0].normal,Color.cyan);
			dir = new Vector2(coll.contacts[0].normal.y,-coll.contacts[0].normal.x);
		}
		if(coll.gameObject.layer == 8){
			dir = Vector2.right;
		}
		if(coll.gameObject.layer == 9){
			animator.SetBool("isPushing",true);
		}
		if(coll.gameObject.layer == 9 || coll.gameObject.layer == 8 || coll.gameObject.layer == 10){
			isLanded = true;
		}
	}

	void OnTriggerStay2D(Collider2D other){
		if(other.gameObject.layer == 11){
			isLaddered = true;
		}
	}

	void MoveRight ()
	{
		isMovingRight = true;
	}

	void MoveLeft ()
	{
		isMovingLeft = true;
	}
}