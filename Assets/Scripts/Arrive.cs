using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrive : MonoBehaviour {
    GameObject m_character;

    [SerializeField] float maxAcceleration;
    [SerializeField] float maxSpeed;

    [SerializeField] float targetRadius;
    [SerializeField] float slowRadius;

    [SerializeField] float timeToTarget;
	// Use this for initialization
	void Start () {
        m_character = this.gameObject;
	}
	
	// Update is called once per frame
	public SteeringOutput GetSteering ( Vector3 target) {
        SteeringOutput steering = new SteeringOutput();
        float targetSpeed;
        Vector3 direction = target - m_character.transform.position;
        float distance = direction.magnitude;
        
        if(distance > slowRadius)
        {
            targetSpeed = maxSpeed;
        }
        else
        {
            targetSpeed = maxSpeed * distance / slowRadius;
        }
        Vector3 targetVelocity = direction;
        targetVelocity.Normalize();
        targetVelocity *= targetSpeed;

        steering.m_linear = targetVelocity - m_character.GetComponent<Rigidbody>().velocity;
        steering.m_linear /= timeToTarget;

        if(steering.m_linear.magnitude > maxAcceleration)
        {
            steering.m_linear.Normalize();
            steering.m_linear *= maxAcceleration;
        }
        steering.m_angular = Quaternion.Euler(0,0,0);
        return steering;

	}
}
