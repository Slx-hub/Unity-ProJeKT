using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamSpawner : MonoBehaviour
{
    public SpawnAreaMaker spawnArea;
    public GameObject beamPrefab;

    public float interval;
    public int maxNum;
    public bool started = false;

    private float innerTimer = 0f;
    private List<BeamControl> beamPool;
    // Start is called before the first frame update
    void Start()
    {
        beamPool= new List<BeamControl>();

        for(int i = 0; i < maxNum; i++)
        {
            var go = GameObject.Instantiate(beamPrefab);
            go.transform.SetParent(transform);
            go.SetActive(false);
            beamPool.Add(go.GetComponent<BeamControl>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!started) return;

        if(innerTimer > interval)
        {
            innerTimer = 0f;

            var num = (int)(Random.value * maxNum + 0.5f);

            for (int i = 0; i < num; i++)
            {
                beamPool[i].Activate(spawnArea.GetRandomPointInSpawnArea(),
                    spawnArea.GetRandomPointInSpawnArea());
            }
        }

        innerTimer += Time.deltaTime;
    }

    public void StartSpawner() { started = true; }
    public void StopSpawner() { started = false; }
    public bool IsSpawning() { return started; }
}
