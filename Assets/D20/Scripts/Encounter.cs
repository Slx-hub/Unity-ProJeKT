using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Encounter : MonoBehaviour
{
    public Entity pylonA;
    public Entity pylonB;
    public Entity pylonC;
    public Entity bossShield;
    public Entity boss;
    public Transform d20;

    public Swell shockWave;
    public RaisingFloor lava;
    public BeamSpawner beamSpawner;
    public PathMaker pathMaker;

    public Transform dronePrefab;

    public bool pylonAAlive = true;
    public bool pylonBAlive = true;
    public bool pylonCAlive = true;

    private bool finalKnockback = false;

    private List<Transform> drones = new List<Transform>();
    private float innerTimer = 0f;
    private EventControler ec;

    // Start is called before the first frame update
    void Start()
    {
        ec = GetComponent<EventControler>();
    }

    // Update is called once per frame
    void Update()
    {
        innerTimer += Time.deltaTime;

        if(pylonAAlive && !pylonA.IsAlive())
        {
            pylonAAlive = false;
            PylonDestroyed();
        }
        if (pylonBAlive && !pylonB.IsAlive())
        {
            pylonBAlive = false;
            PylonDestroyed();
        }
        if (pylonCAlive && !pylonC.IsAlive())
        {
            pylonCAlive = false;
            PylonDestroyed();
        }

        if(!finalKnockback && boss.Health < 250)
        {
            ec.AddEvent(1f, ActivateKnockback, true);
            finalKnockback = true;
            lava.yE += 1f;
            lava.Raise();
        }

        if(boss.Health < 0)
        {
            lava.gameObject.SetActive(false);
            beamSpawner.StopSpawner();
            drones.ForEach(drone => { drone.gameObject.SetActive(false); });
        }
    }

    private void PylonDestroyed()
    {
        bossShield.Hurt(100, true);
        var drone = GameObject.Instantiate(dronePrefab);
        var droneC = drone.GetComponent<DroneControl>();
        var followPathC = drone.GetComponent<FollowPath>();
        droneC.SetTarget(d20);
        followPathC.pathMaker = pathMaker;

        drones.Add(drone);

        if (!pylonAAlive && !pylonBAlive && !pylonCAlive)
        {
            boss.invulernalbe = false;
            bossShield.gameObject.SetActive(false);
            ec.AddEvent(2f, RaiseLavaPermanently, true);
            beamSpawner.interval = 15f;
            ec.AddEvent(2f, ActivateBeamsPermamently, true);
            boss.enabled= true;
        }
        else
        {
            ec.AddEvent(2f, RaiseLava, true);
            ec.AddEvent(15f, ActivateBeams, true);
        }
    }

    private void RaiseLava()
    {
        lava.Raise();
        ec.AddEvent(45f, LowerLava, true);
        ec.AddEvent(50f, ActivateKnockback, true);
    }
    private void RaiseLavaPermanently()
    {
        lava.Raise();
    }

    private void LowerLava()
    {
        lava.Lower();
    }

    private void ActivateBeams()
    {
        beamSpawner.StartSpawner();
        ec.AddEvent(45f, DeactivateBeams, true);
    }
    private void ActivateBeamsPermamently()
    {
        beamSpawner.StartSpawner();
    }

    private void DeactivateBeams()
    {
        beamSpawner.StopSpawner();
    }
    private void ActivateKnockback()
    {
        shockWave.gameObject.SetActive(true);
    }

    public override string ToString()
    {
        return "> Encounter:" +
            "\n  Plyon A:\t\t\t" + (pylonAAlive ? "Alive" : "Dead") +
            "\n  Plyon B:\t\t\t" + (pylonB ? "Alive" : "Dead") +
            "\n  Plyon C:\t\t\t" + (pylonC ? "Alive" : "Dead") +
            "\n\n Lava:\t\t\t" + (lava.IsRaised() ? "Raised" : lava.IsChaningState() ? "In transit" : "Lowered") +
            "\n Beams:\t\t\t" + (beamSpawner.IsSpawning() ? "Spawning" : "Idle") +
            "\n Drones active:\t\t" + drones.Count.ToString() +
            "\n\n Shield Health:\t\t" + bossShield.Health.ToString() +
            "\n Boss Health:\t\t" + boss.Health.ToString();
    }
}
