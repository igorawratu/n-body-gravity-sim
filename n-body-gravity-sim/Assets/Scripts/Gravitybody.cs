using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravitybody : MonoBehaviour
{
    public float mass;
    public Vector3 velocity;
    public Vector3 prevPos;
    public GravitybodyManagerCPU gravityBodyManager;

    // Start is called before the first frame update
    void Start()
    {
        prevPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        List<Gravitybody> affectingBodies = gravityBodyManager.GetAffectingBodies(this);

        float m = mass * gravityBodyManager.G;

        Vector3 netForce = Vector3.zero;

        for (int i = 0; i < affectingBodies.Count; ++i)
        {
            if (this == affectingBodies[i])
            {
                continue;
            }
            Vector3 dir = affectingBodies[i].gameObject.transform.position - gameObject.transform.position;
            float d2 = Mathf.Max(0.2f, Vector3.SqrMagnitude(dir));
            float f = m * affectingBodies[i].mass / d2;

            netForce += dir.normalized * f;
        }

        Vector3 acceleration = netForce / mass;
        velocity += acceleration * Time.deltaTime;
        gameObject.transform.position += velocity * Time.deltaTime;
    }
}
