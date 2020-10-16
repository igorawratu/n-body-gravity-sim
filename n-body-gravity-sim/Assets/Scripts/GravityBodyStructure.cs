using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

abstract class GravityBodyStructure
{
    public abstract List<Gravitybody> GetGravityBodies(Gravitybody body);
    public abstract void AddGravityBody(Gravitybody body);
    public abstract void Update();
    public abstract Vector3 CenterOfMass();
}
