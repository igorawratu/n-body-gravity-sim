using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class BFGravityBodyStructure : GravityBodyStructure
{
    private List<Gravitybody> gbodies;
    public BFGravityBodyStructure()
    {
        gbodies = new List<Gravitybody>();
    }

    public override void AddGravityBody(Gravitybody body)
    {
        gbodies.Add(body);
    }

    public override List<Gravitybody> GetGravityBodies(Gravitybody body)
    {
        return gbodies;
    }

    public override void Update()
    {
    }

    public override Vector3 CenterOfMass()
    {
        Vector3 com = Vector3.zero;
        float totalMass = 0f;

        for(int i = 0; i < gbodies.Count; ++i)
        {
            com += gbodies[i].mass * gbodies[i].transform.position;
            totalMass += gbodies[i].mass;
        }

        com /= totalMass;

        return com;
    }
}
