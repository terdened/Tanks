using UnityEngine;
using System.Collections;

public class Bulet : MonoBehaviour {

	public float buletSpeed = 0.35f;
	
	private Animator animator;
	private int animationState;					// поворот 
	private NetworkView networkView;
	int collisionCount = 0;

	void Awake () {
		animationState = 0;
		animator = (Animator)GetComponent<Animator> ();
		animator.SetInteger ("State", animationState);
		networkView = GetComponent<NetworkView> ();
	}

	public void InitDirection(int direction)
	{
		animationState = direction;
		animator.SetInteger ("State", animationState);
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
		HandleDestroy ();
	}

	void HandleDestroy()
	{
		if(transform.position.x > 30 || transform.position.x < -30
			|| transform.position.y > 30 || transform.position.y < -30)
			Network.Destroy(GetComponent<NetworkView>().viewID);

		
		var animationState = animator.GetCurrentAnimatorStateInfo(0).nameHash;

		if(animationState == 1344932113 && collisionCount > 1)
			Network.Destroy(GetComponent<NetworkView>().viewID);
	}

	// Вызывается с определенной частотой. Отвечает за сереализицию переменных
	void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info) {
		Vector3 syncPosition = Vector3.zero;
		if (stream.isWriting) {
			animationState = animator.GetInteger("State");
			syncPosition = transform.position;
			
			stream.Serialize(ref syncPosition);
			stream.Serialize(ref animationState);
		} else {
			stream.Serialize(ref syncPosition);
			stream.Serialize(ref animationState);
			
			animator.SetInteger ("State", animationState);
			transform.position = syncPosition;
		}
	}
	
	void Move()
	{
		var deltaX = 0.0f;
		var deltaY = 0.0f;

		var animationState = animator.GetCurrentAnimatorStateInfo(0).nameHash;
		if (animationState.ToString() == "1858765060") {
			deltaY = buletSpeed;
		}
		
		if (animationState.ToString() == "-1969128098") {
			deltaY = -buletSpeed;
		}
		
		if (animationState.ToString() == "-331806195") {
			deltaX = -buletSpeed;
		}
		
		if (animationState.ToString() == "-1987789266") {
			deltaX = buletSpeed;
		}
		transform.position = new Vector2(transform.position.x + deltaX,
		                                 transform.position.y + deltaY);  
	}

	void OnTriggerEnter2D(Collider2D other) {
		collisionCount++;

		if (collisionCount > 1) {
			var forceVector = new Vector2 (0, 0);

			if (animator.GetInteger ("State") == 1) {
				forceVector.y = 100;
			}
		
			if (animator.GetInteger ("State") == 2) {
				forceVector.y = -100;
			}
		
			if (animator.GetInteger ("State") == 3) {
				forceVector.x = -100;
			}
		
			if (animator.GetInteger ("State") == 4) {
				forceVector.x = 100;
			}

			other.attachedRigidbody.AddForce (forceVector);
			animator.SetInteger ("State", 5);

			//Network.Destroy (GetComponent<NetworkView> ().viewID);
		}
	}
}
