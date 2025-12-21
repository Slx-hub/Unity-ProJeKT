using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEngine;

public class SimplePlayerControler : NetworkBehaviour
{
    public GameObject ColliderPrefab;

    public bool IsGrounded
    {
        get { return ObjsInContact > 0; }
    }

    public bool IsContactingWall
    {
        get { return WallsInContact.Count > 0; }
    }

    public Collision LastCollision { get; private set; }


    private List<Action> CollisionListeners = new();

    public Action CollisionListener
    {
        set
        {
            CollisionListeners.Add(value);
        }
    }


    private Rigidbody Rigidbody;
    private NetworkRigidbody NetworkRigidbody;
    private Entity entity;
   

    private List<float> AngularVelocities = new();
    private int ObjsInContact = 0;
    private List<Collider> WallsInContact = new List<Collider>();

    private Transform groundCollider;

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        NetworkRigidbody = GetComponent<NetworkRigidbody>();
        entity = GetComponent<Entity>();

        ColliderPrefab.GetComponent<CollisionBroadcaster>().onTrigger = OnCustomWallTrigger;
        groundCollider = ColliderPrefab.transform;
        groundCollider.GetComponent<CollisionBroadcaster>().onTrigger = OnCustomGroundTrigger;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
        }
    }

    private void LateUpdate()
    {
        ColliderPrefab.transform.position = transform.position;
    }

    public Vector3 GetWallDirection()
    {
        var collisionPoint = WallsInContact[0].ClosestPoint(transform.position);
        var collisionNormal = (transform.position - collisionPoint).normalized;
        return collisionNormal;
    }

    void OnCustomWallTrigger(Collider other, bool entered)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast")
            || other.gameObject.Equals(this.gameObject))
            return;

        if (entered)
            WallsInContact.Add(other);
        else
            WallsInContact.Remove(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        LastCollision = collision;
    }

    void OnCustomGroundTrigger(Collider other, bool entered)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast")
            || other.gameObject.Equals(this.gameObject))
            return;

        ObjsInContact += entered ? 1 : -1;
        CollisionListeners.ForEach(a => a.Invoke());
    }
}
