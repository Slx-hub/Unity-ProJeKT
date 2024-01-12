using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityController : MonoBehaviour
{
    public Ability jAbility;
    public Ability kAbility;
    public Ability lAbility;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var keyboard = Keyboard.current;

        if (keyboard.jKey.wasPressedThisFrame) { jAbility.Use(); }
        if (keyboard.kKey.wasPressedThisFrame) { kAbility.Use(); }
        if (keyboard.lKey.wasPressedThisFrame) { lAbility.Use(); }
    }
}
