using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Align : MonoBehaviour {
    GameObject m_character;

    SteeringOutput steering;
	// Use this for initialization
	void Start () {
        m_character = this.gameObject;
	}
	
	public SteeringOutput GetSteering(Vector3 target)
    {
        steering = new SteeringOutput();
        //Get relative rotation
        Vector3 direction = target - m_character.transform.position;
        //Create Quaternion for rotation change
        Quaternion rotation = Quaternion.FromToRotation(m_character.transform.forward, direction);
        steering.m_angular = rotation * m_character.transform.rotation;
        steering.m_linear = Vector3.zero;

        return steering;
    }
}
