using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowNearestEntity : MonoBehaviour
{
    public TextMeshProUGUI outputText;

    public Entity currentTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (currentTarget != null)
            outputText.text = currentTarget.gameObject.name + ":" + currentTarget.Health.ToString();
        else
            outputText.text = "No Target";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var ents = GameObject.FindGameObjectsWithTag("Entity");
        var minDist = float.MaxValue;

        foreach (var entry in ents)
        {
            var dist = Vector3.Distance(transform.position, entry.transform.position);
            var ent = entry.GetComponent<Entity>();
            if (dist < minDist && ent.enabled)
            {
                minDist = dist;
                currentTarget = ent;
            }
        }
    }
}
