using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitybodyManagerCPU : MonoBehaviour
{
    public GameObject gravitybodyPrefab;

    public Camera camera;
    public float cameraDistToCOM = 10;

    public int initialObjects = 100;
    
    //parsecs.km/s.solarmass
    public float G = 0.00430009f;

    public float minMass = 2;
    public float maxMass = 5;
    public Vector3 minStartVelocity = new Vector3(-1, -1, -1);
    public Vector3 maxStartVelocity = new Vector3(1, 1, 1);
    public Vector3 minStartPosition = new Vector3(-10, -10, -10);
    public Vector3 maxStartPosition = new Vector3(10, 10, 10);

    private GravityBodyStructure gbodystructure;

    void Start()
    {
        gbodystructure = new BFGravityBodyStructure();

        for(int i = 0; i < initialObjects; ++i)
        {
            AddGravityBody();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            AddGravityBody();
        }

        cameraDistToCOM = Mathf.Clamp(cameraDistToCOM + -Input.mouseScrollDelta.y * Time.deltaTime * 10f, 1f, 100f);

        Vector3 com = gbodystructure.CenterOfMass();
        Vector3 camdir = (com - camera.transform.position).normalized;
        camera.gameObject.transform.position = com - camdir * cameraDistToCOM;
        camera.gameObject.transform.forward = camdir;

        Debug.Log(com);
        
    }

    void LateUpdate()
    {
        gbodystructure.Update();
    }

    private void AddGravityBody()
    {
        GameObject gbobject = Instantiate(gravitybodyPrefab);
        Gravitybody gbody = gbobject.GetComponent<Gravitybody>();
        if(gbody != null)
        {
            gbody.mass = (maxMass - minMass) * Random.value + minMass;
            gbody.velocity = new Vector3(
                (maxStartVelocity.x - minStartVelocity.x) * Random.value + minStartVelocity.x, 
                (maxStartVelocity.y - minStartVelocity.y) * Random.value + minStartVelocity.y,
                (maxStartVelocity.z - minStartVelocity.z) * Random.value + minStartVelocity.z);

            gbody.transform.position = new Vector3(
                (maxStartPosition.x - minStartPosition.x) * Random.value + minStartPosition.x,
                (maxStartPosition.y - minStartPosition.y) * Random.value + minStartPosition.y,
                (maxStartPosition.z - minStartPosition.z) * Random.value + minStartPosition.z);

            gbody.gravityBodyManager = this;

            gbodystructure.AddGravityBody(gbody);
        }
    }

    public List<Gravitybody> GetAffectingBodies(Gravitybody body)
    {
        return gbodystructure.GetGravityBodies(body);
    }
}
