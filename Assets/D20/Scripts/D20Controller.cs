using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEngine;

public class D20Controller : NetworkBehaviour
{
    public GameObject ColliderPrefab;
    public float PoweredThreshold = 3f;

    public bool IsGrounded
    {
        get { return ObjsInContact > 0; }
    }

    public bool IsContactingWall
    {
        get { return WallsInContact.Count > 0; }
    }

    public bool IsPowered
    {
        get { return AngularVelocity > PoweredThreshold; }
    }

    public Collision LastCollision { get; private set; }

    private NetworkVariable<float> m_angularVelocity = new(writePerm: NetworkVariableWritePermission.Owner);
    public float AngularVelocity { get => m_angularVelocity.Value;}

    public AudioSource AudioSource { get; private set; }

    private List<Action> CollisionListeners = new();

    public Action CollisionListener
    {
        set
        {
            CollisionListeners.Add(value);
        }
    }

    public int CurrentFaceValue { get; private set; }

    private Rigidbody Rigidbody;
    private NetworkRigidbody NetworkRigidbody;
    private Entity entity;
    private readonly Dictionary<Vector3, int> FaceToValueLUT = new()
    {
        { new Vector3(0.15f, -0.46f, 0.63f)  , 11 },
        { new Vector3(-0.63f, -0.46f, 0.15f) , 1 },
        { new Vector3(-0.24f, 0.74f, -0.15f) , 12 },
        { new Vector3(0.24f, 0.74f, 0.15f)   , 2 },
        { new Vector3(-0.39f, -0.28f, 0.63f) , 13 },
        { new Vector3(-0.15f, -0.46f, -0.63f), 3 },
        { new Vector3(0.78f, 0.00f, 0.15f)   , 14 },
        { new Vector3(0.48f, 0.00f, 0.63f)   , 4 },
        { new Vector3(-0.63f, 0.46f, 0.15f)  , 15 },
        { new Vector3(-0.39f, 0.28f, 0.63f)  , 5 },
        { new Vector3(0.39f, -0.28f, -0.63f) , 16 },
        { new Vector3(0.63f, -0.46f, -0.15f) , 6 },
        { new Vector3(-0.48f, 0.00f, -0.63f) , 17 },
        { new Vector3(-0.78f, 0.00f, -0.15f) , 7 },
        { new Vector3(0.15f, 0.46f, 0.63f)   , 18 },
        { new Vector3(0.39f, 0.28f, -0.63f)  , 8 },
        { new Vector3(-0.24f, -0.74f, -0.15f), 19 },
        { new Vector3(0.24f, -0.74f, 0.15f)  , 9 },
        { new Vector3(0.63f, 0.46f, -0.15f)  , 20 },
        { new Vector3(-0.15f, 0.46f, -0.63f) , 10 },
    };

    private List<float> AngularVelocities = new();
    private int ObjsInContact = 0;
    private List<Collider> WallsInContact = new List<Collider>();

    private Transform groundCollider;

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        NetworkRigidbody = GetComponent<NetworkRigidbody>();
        entity = GetComponent<Entity>();
        AudioSource = this.AddComponent<AudioSource>();

        ColliderPrefab = Instantiate(ColliderPrefab,transform.parent);
        ColliderPrefab.GetComponent<CollisionBroadcaster>().onTrigger = OnCustomWallTrigger;
        groundCollider = ColliderPrefab.transform.GetChild(0);
        groundCollider.GetComponent<CollisionBroadcaster>().onTrigger = OnCustomGroundTrigger;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
        }
    }

    void FixedUpdate()
    {
        AngularVelocities.Add(Rigidbody.angularVelocity.magnitude);
        if (AngularVelocities.Count > 50)
            AngularVelocities.RemoveAt(0);
        m_angularVelocity.Value = AngularVelocities.Average();
                    
        float maxDot = 0.0f;
        Vector3 localUp = transform.InverseTransformVector(Vector3.up);
        foreach (var entry in FaceToValueLUT)
        {
            if (Vector3.Dot(entry.Key, localUp) > maxDot)
            {
                maxDot = Vector3.Dot(entry.Key, localUp);
                CurrentFaceValue = entry.Value;
            }       
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

    public override string ToString()
    {
        return "> D20Controller:" +
            "\n  Current value:\t\t" + CurrentFaceValue.ToString() +
            "\n  Is on ground:\t\t" + IsGrounded.ToString() + "(" + ObjsInContact + ")"+
            "\n  Walls in Contact:\t" + WallsInContact.Count.ToString() +
            "\n  Health:\t\t\t" + entity?.Health.ToString() +
            "\n  Smooth angular V:\t" + AngularVelocity +
            "\n  Dice velocity:\t\t" + Rigidbody.velocity.magnitude;
    }

    public void PlaySoundRandomPitch(string sound)
    {
        AudioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        AudioSource.volume = UnityEngine.Random.Range(0.9f, 1f);
        AudioSource.PlayOneShot(SoundManager.GetSoundByName(sound));
    }
}
