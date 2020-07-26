using UnityEngine;
using System.Collections;

public class DiscreteFollow : MonoBehaviour {
	public Vector2 offset;	
	public Transform target;
	void Update () {
		transform.position = new Vector3(((int)(target.position.x/offset.x))*offset.x,((int)(target.position.y/offset.y))*offset.y,transform.position.z);	
	}
}
