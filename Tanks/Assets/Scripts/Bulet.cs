using UnityEngine;
using System.Collections;

public class Bulet : MonoBehaviour {

	public float buletSpeed = 0.35f;
	
	private Animator animator;
	private int animationState;					// поворот 
	private NetworkView networkView;

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
		print (animationState);
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
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

		print (animationState.ToString());
		transform.position = new Vector2(transform.position.x + deltaX,
		                                 transform.position.y + deltaY);  
	}
}
