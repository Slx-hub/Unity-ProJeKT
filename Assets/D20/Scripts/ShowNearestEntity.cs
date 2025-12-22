using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ShowNearestEntity : NetworkBehaviour
{
    public TextMeshProUGUI outputText;

    public Entity currentTarget;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        outputText = GetComponent<TextMeshProUGUI>();
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
        if (NetworkManager?.LocalClient?.PlayerObject == null)
            return;

        if (player == null)
        {
            var maybePlayer = NetworkManager.LocalClient.PlayerObject;
            if (maybePlayer == null) { return; }
            player = maybePlayer.transform;
        }

        var ents = GameObject.FindGameObjectsWithTag("Entity");
        var minDist = float.MaxValue;

        foreach (var entry in ents)
        {
            var dist = Vector3.Distance(player.position, entry.transform.position);
            var ent = entry.GetComponent<Entity>();

            if (ent == null)
                continue;

            if (dist < minDist && ent.enabled)
            {
                minDist = dist;
                currentTarget = ent;
            }
        }
    }
}
