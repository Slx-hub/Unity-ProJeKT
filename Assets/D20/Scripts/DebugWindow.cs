using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.InputSystem.DefaultInputActions;

public class DebugWindow : MonoBehaviour
{
    public int Height = 400;
    public int Width = 200;
    public GUIStyle GUIStyle;

    private MonoBehaviour[] allScripts;

    void Start()
    {
        allScripts = gameObject.GetComponents<MonoBehaviour>();
    }

    void OnGUI()
    {
        if (Application.isEditor)
        {
            var debugTextList = new List<string>();

            foreach (var script in allScripts)
            {
                var scriptDump = script.ToString();
                if (scriptDump.StartsWith(">"))
                    debugTextList.Add(scriptDump);
            }

            GUI.Box(new Rect(5, 5, Width, Height), string.Join("\n\n", debugTextList), GUIStyle);
        }
    }
}
