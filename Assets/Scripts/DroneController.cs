using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public class DroneController : MonoBehaviour {
    [SerializeField] GameObject leader;
    [SerializeField] float m_maxSpeed;
    [SerializeField] float m_turnSpeed;
    [SerializeField] GameObject m_droneGO;

    Separation separation;
    Arrive arrive;
    Align align;
    VelocityMatch velocityMatching;
    SteeringOutput arriveOutput;
    SteeringOutput separationOutput;
    SteeringOutput alignOutput;
    SteeringOutput velocityOutput;
    // Use this for initialization
    void Start () {
        separation = this.GetComponent<Separation>();
        arrive = this.GetComponent<Arrive>();
        align = this.GetComponent<Align>();
        velocityMatching = this.GetComponent<VelocityMatch>();
        //m_droneGO.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0,50), Random.Range(0, 50), Random.Range(0, 50)),ForceMode.VelocityChange);
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 center = new Vector3(0, 0, 0);
        float count = 0;
        GameObject[] drones = GameObject.FindGameObjectsWithTag("drone");
        foreach(GameObject drone in drones)
        {
           center += drone.transform.position;
           count++;
        }
        var theCenter = center / count;
        Vector3 velocity = new Vector3(0, 0, 0);
        count = 0;
        foreach (GameObject drone in drones)
        {
            velocity += drone.GetComponent<Rigidbody>().velocity;
            count++;
        }

        var theVelocity = velocity / count;
        separationOutput = separation.GetSteering();
        velocityOutput = velocityMatching.GetSteering(leader.GetComponent<Rigidbody>().velocity);
        arriveOutput = arrive.GetSteering(leader.transform.position);
        float step = m_turnSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, leader.transform.rotation, Time.deltaTime * m_turnSpeed);
        //m_droneGO.GetComponent<Transform>().rotation = Quaternion.RotateTowards(transform.rotation, leader.transform.rotation , step);
        Move((2.5f) * separationOutput.m_linear + (1.5f) * velocityOutput.m_linear + (0.75f) * arriveOutput.m_linear);
    }

    void Move(Vector3 move)
    {
        //Clip max speed
        if (move.magnitude > m_maxSpeed)
        { 
            move.Normalize();
            move *= m_maxSpeed;
        }
        //Apply velocity to the drone in the target direction
        m_droneGO.GetComponent<Rigidbody>().AddForce(move * Time.deltaTime, ForceMode.VelocityChange);
    }
}
