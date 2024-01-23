using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DebugWindowManager : MonoBehaviour
{
    public Vector2Int PositionOffset = Vector2Int.zero;
    public bool StartAllMinimized = false;
    public GUIStyle GUIStyle;

    private List<DebugWindow> windows = new();
    // Start is called before the first frame update
    void Start()
    {
        if(!Application.isEditor)
            gameObject.SetActive(false);

        windows = GameObject.FindObjectsByType<DebugWindow>(FindObjectsSortMode.InstanceID).ToList();
        windows = windows.OrderBy(m => m.priority).ToList();
        windows.ForEach(w => w.GUIStyle= GUIStyle);

        if(StartAllMinimized)
            windows.ForEach(w => w.Minimized = true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        int yOffset = 0;
        for(int i = 0; i < windows.Count; i++)
        {
            DebugWindow window = windows[i];

            window.PositionOffset = new Vector2Int(PositionOffset.x, yOffset);
            yOffset += window.Minimized ? window.MimimizedHeight : window.Height;
        }
    }
}
