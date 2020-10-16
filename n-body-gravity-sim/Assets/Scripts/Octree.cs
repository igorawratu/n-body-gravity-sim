using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public class Octree
{
    public class OctreeNode
    {
        static Vector3[] child_factors = {new Vector3(1, 1, 1), new Vector3(1, 1, -1), new Vector3(1, -1, 1), new Vector3(1, -1, -1),
            new Vector3(-1, 1, 1), new Vector3(-1, 1, -1), new Vector3(-1, -1, 1), new Vector3(-1, -1, -1)};

        public OctreeNode() : this(Vector3.zero, Vector3.one)
        {

        }

        public OctreeNode(Vector3 center, Vector3 halfdims)
        {
            this.center = center;
            this.half_dim = halfdims;
            children = new List<OctreeNode>();
            com = center;
            mean_mass = 0f;
            dirty = false;
    }

        public void Grow()
        {
            if(children.Count > 0)
            {
                return;
            }

            Vector3 child_hd = half_dim / 2;
            for(int i = 0; i < 8; ++i)
            {
                Vector3 child_center = center + Vector3.Scale(child_hd, child_factors[i]);
                OctreeNode child = new OctreeNode(child_center, child_hd);
                child.parent = this;
                children.Add(child);
            }
        }

        public bool Leaf()
        {
            return children.Count == 0;
        }

        public bool InNode(Gravitybody body)
        {
            Vector3 body_p = body.gameObject.transform.position;
            body_p = body_p - center;
            body_p = new Vector3(Mathf.Abs(body_p.x), Mathf.Abs(body_p.y), Mathf.Abs(body_p.z));

            return body_p.x <= half_dim.x && body_p.y <= half_dim.y && body_p.z <= half_dim.z;
        }

        public Vector3 center;
        public Vector3 half_dim;
        public List<OctreeNode> children;
        public Vector3 com;
        public float mean_mass;
        public bool dirty;

        OctreeNode parent;
    }

    public Octree() : this(Vector3.one, -Vector3.one, 1f)
    {

    }

    private void GrowTree(OctreeNode node, float leaf_size_thresh)
    {
        float node_size = node.half_dim.magnitude * 2;
        if(node_size > leaf_size_thresh)
        {
            node.Grow();

            foreach(var child in node.children)
            {
                GrowTree(child, leaf_size_thresh);
            }
        }
    }

    private void RegisterLeafOTNs(OctreeNode node)
    {
        if (node.Leaf())
        {
            node_bodies_[node] = new HashSet<Gravitybody>();
        }
        else
        {
            foreach(var child in node.children)
            {
                RegisterLeafOTNs(child);
            }
        }
    }

    public Octree(Vector3 max, Vector3 min, float leaf_size_thresh)
    {
        Vector3 center = (max + min) / 2;
        Vector3 halfdim = (max - min) / 2;

        root_ = new OctreeNode(center, halfdim);
        GrowTree(root_, leaf_size_thresh);
        RegisterLeafOTNs(root_);
        body_nodes_ = new Dictionary<Gravitybody, OctreeNode>();
    }

    public void AddBody(Gravitybody body)
    {

    }

    public void UpdateVelocities(float sa_thresh)
    {

    }

    public void UpdatePositions()
    {

    }

    public void UpdateTree()
    {

    }

    private OctreeNode root_;

    private Dictionary<Gravitybody, OctreeNode> body_nodes_;
    private Dictionary<OctreeNode, HashSet<Gravitybody>> node_bodies_;
}
