using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Separation : MonoBehaviour {

    GameObject m_character; //Data of the character
    ArrayList targets; //Potential targets
    [SerializeField] float m_threshold; //Threshold to take action
    [SerializeField] float m_decayCoefficient; //Coefficient for inverse square law force
    [SerializeField] float m_maxAcceleration; //Character maximum acceleration
    [SerializeField] float m_fieldOfView;

    private void Start()
    {
        targets = new ArrayList();
        m_character = this.gameObject;
    }
    public SteeringOutput GetSteering()
    {
        SteeringOutput steering = new SteeringOutput();

        GetNearbyTargets(); //Get targets in a cone in front of the Object

        foreach(Transform target in targets) //Loop through each target
        {
            Vector3 direction = target.position - m_character.transform.position; //Check if target is close
            float distance = direction.magnitude;
            if(distance < m_threshold)
            {
                float strength = Mathf.Min(m_decayCoefficient / (distance * distance), m_maxAcceleration); //Calculate the strength of repulsion
                direction.Normalize();
                steering.m_linear -= strength * direction; //Add acceleration
            }
        }
        //We have done through all targets, return the result
        return steering;
    }

    void GetNearbyTargets()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, m_threshold);

        foreach(Collider collider in hitColliders)
        {
            Vector3 direction = collider.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);
            if (angle < m_fieldOfView * 0.5f)
            {
                RaycastHit hit;
                if(Physics.Raycast(transform.position + transform.up, direction.normalized, out hit , m_threshold))
                {
                    if(hit.collider.tag == "drone")
                    {
                        targets.Add(hit.collider.transform);
                    }
                }
            }
        }
    }


}
