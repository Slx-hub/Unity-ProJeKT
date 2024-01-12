using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileModul : AbilityModule
{
    public Vector3 SpawnOffset;
    public GameObject ProjectilePrefab;
    public int ProjectileCount;

    private List<Transform> projectileHitTransforms;
    private List<Vector3> projectileHitLocations;

    private List<Projectile> projectiles;
    private bool spawned = false;

    // Update is called once per frame
    void Update()
    {
        if(!spawned) return;
        if(projectileHitLocations.Count + projectileHitTransforms.Count != ProjectileCount ) { return; }

        ModuleState = ModuleState.Success;
        spawned = false;
    }

    public override void Execute()
    {
        ModuleState = ModuleState.Wait;

        projectiles = new();

        projectileHitTransforms = new();
        projectileHitLocations = new();

        for (int i = 0; i < ProjectileCount; i++)
        {
            projectiles.Add(GameObject.Instantiate(ProjectilePrefab, transform.position + SpawnOffset, Quaternion.LookRotation(transform.forward)).GetComponent<Projectile>());
            projectiles[i].transformCallback = ProjectileCallbackTransform;
            projectiles[i].locationCallback = ProjectileCallbackVector3;
        }

        spawned = true;

        base.Execute();
    }

    public void ProjectileCallbackTransform(Transform hitTransform)
    {
        projectileHitTransforms.Add(hitTransform); 
    }
    public void ProjectileCallbackVector3(Vector3 hitPosition)
    {
        projectileHitLocations.Add(hitPosition);
    }
}
