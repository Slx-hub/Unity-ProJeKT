//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/InputSystem/Default.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @DefaultActionsWrapper: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @DefaultActionsWrapper()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Default"",
    ""maps"": [
        {
            ""name"": ""WASD"",
            ""id"": ""a1cd0193-5f65-44a6-aa41-a8dc2d3ab99e"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""e9b20933-3b9f-43e9-b672-4051066187cd"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""c5c1f602-ae68-4af7-96c0-69619f729b27"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""47dfb383-3080-44b1-8991-63ca70b8d5c5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""c7b08271-84fc-47bb-8f3b-47e74879051d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Ability1"",
                    ""type"": ""Button"",
                    ""id"": ""663c43f9-7c4d-41f3-840c-2a0eae36b877"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Ability2"",
                    ""type"": ""Button"",
                    ""id"": ""87f13376-5767-4664-95c4-3c95aab809a8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Ability3"",
                    ""type"": ""Button"",
                    ""id"": ""83db5fe2-7cb5-4364-a8ba-d861f30b6785"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Reload Scene"",
                    ""type"": ""Button"",
                    ""id"": ""201db1a9-0505-46aa-8aa5-39759d1f3262"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""9cd34244-5ac8-4250-85b7-623d33d07ce6"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": ""NormalizeVector2"",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Up"",
                    ""id"": ""ca8cb7dc-592f-42d4-962a-18f3f58bf5a0"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Down"",
                    ""id"": ""306ad9f0-e450-40fb-b8c4-3cf554ae5ba2"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left"",
                    ""id"": ""b02e59e0-2895-463b-b6e5-8afcec8e0b85"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Right"",
                    ""id"": ""4dfe2b2f-9de3-43c7-bbac-46828e0390f9"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""deb614f0-21cf-4079-bf3d-7204d82b1897"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": ""NormalizeVector2,StickDeadzone"",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Up"",
                    ""id"": ""d70f8952-587d-4edf-afa7-db6db9881d5a"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Down"",
                    ""id"": ""9d6b03d2-50e5-4711-a28d-aeaf6f49a79f"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left"",
                    ""id"": ""02d227a7-2502-4d5f-ba1b-d023101bc8c4"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Right"",
                    ""id"": ""7b2f2a23-d771-459e-bdae-a1cab38b6b76"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""d94a88e7-63ce-4f20-80fe-d3832eca113c"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6a0b9929-528d-4541-9516-7d6073f15976"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3b4da20a-cb7b-4640-b1cd-6b0d8dee85c4"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""02d09a14-0b08-4bbd-a07c-18296cc32c53"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""60a2c23d-960f-44cf-853b-245c2f687e57"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3142c212-fc79-4844-ac35-bc17918c0f05"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e8d87c6a-6525-465f-9081-86dea213cc97"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Ability1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4cfee325-2be2-4d9c-b8fe-3011737a5c37"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Ability2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9b5794c9-7140-445a-ba0a-a9ea822498d1"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Ability3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a232da6e-fcbf-4221-b79c-2bcf78155949"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reload Scene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""New action map"",
            ""id"": ""4f95e5b6-97d0-43dc-9826-42b052cf2791"",
            ""actions"": [
                {
                    ""name"": ""New action"",
                    ""type"": ""Button"",
                    ""id"": ""d88d06a2-fb42-419a-8513-16118fc28acc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""2a9c6145-2ec7-448c-931c-eca48542d148"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // WASD
        m_WASD = asset.FindActionMap("WASD", throwIfNotFound: true);
        m_WASD_Move = m_WASD.FindAction("Move", throwIfNotFound: true);
        m_WASD_Jump = m_WASD.FindAction("Jump", throwIfNotFound: true);
        m_WASD_Dash = m_WASD.FindAction("Dash", throwIfNotFound: true);
        m_WASD_Fire = m_WASD.FindAction("Fire", throwIfNotFound: true);
        m_WASD_Ability1 = m_WASD.FindAction("Ability1", throwIfNotFound: true);
        m_WASD_Ability2 = m_WASD.FindAction("Ability2", throwIfNotFound: true);
        m_WASD_Ability3 = m_WASD.FindAction("Ability3", throwIfNotFound: true);
        m_WASD_ReloadScene = m_WASD.FindAction("Reload Scene", throwIfNotFound: true);
        // New action map
        m_Newactionmap = asset.FindActionMap("New action map", throwIfNotFound: true);
        m_Newactionmap_Newaction = m_Newactionmap.FindAction("New action", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // WASD
    private readonly InputActionMap m_WASD;
    private List<IWASDActions> m_WASDActionsCallbackInterfaces = new List<IWASDActions>();
    private readonly InputAction m_WASD_Move;
    private readonly InputAction m_WASD_Jump;
    private readonly InputAction m_WASD_Dash;
    private readonly InputAction m_WASD_Fire;
    private readonly InputAction m_WASD_Ability1;
    private readonly InputAction m_WASD_Ability2;
    private readonly InputAction m_WASD_Ability3;
    private readonly InputAction m_WASD_ReloadScene;
    public struct WASDActions
    {
        private @DefaultActionsWrapper m_Wrapper;
        public WASDActions(@DefaultActionsWrapper wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_WASD_Move;
        public InputAction @Jump => m_Wrapper.m_WASD_Jump;
        public InputAction @Dash => m_Wrapper.m_WASD_Dash;
        public InputAction @Fire => m_Wrapper.m_WASD_Fire;
        public InputAction @Ability1 => m_Wrapper.m_WASD_Ability1;
        public InputAction @Ability2 => m_Wrapper.m_WASD_Ability2;
        public InputAction @Ability3 => m_Wrapper.m_WASD_Ability3;
        public InputAction @ReloadScene => m_Wrapper.m_WASD_ReloadScene;
        public InputActionMap Get() { return m_Wrapper.m_WASD; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(WASDActions set) { return set.Get(); }
        public void AddCallbacks(IWASDActions instance)
        {
            if (instance == null || m_Wrapper.m_WASDActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_WASDActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @Dash.started += instance.OnDash;
            @Dash.performed += instance.OnDash;
            @Dash.canceled += instance.OnDash;
            @Fire.started += instance.OnFire;
            @Fire.performed += instance.OnFire;
            @Fire.canceled += instance.OnFire;
            @Ability1.started += instance.OnAbility1;
            @Ability1.performed += instance.OnAbility1;
            @Ability1.canceled += instance.OnAbility1;
            @Ability2.started += instance.OnAbility2;
            @Ability2.performed += instance.OnAbility2;
            @Ability2.canceled += instance.OnAbility2;
            @Ability3.started += instance.OnAbility3;
            @Ability3.performed += instance.OnAbility3;
            @Ability3.canceled += instance.OnAbility3;
            @ReloadScene.started += instance.OnReloadScene;
            @ReloadScene.performed += instance.OnReloadScene;
            @ReloadScene.canceled += instance.OnReloadScene;
        }

        private void UnregisterCallbacks(IWASDActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @Dash.started -= instance.OnDash;
            @Dash.performed -= instance.OnDash;
            @Dash.canceled -= instance.OnDash;
            @Fire.started -= instance.OnFire;
            @Fire.performed -= instance.OnFire;
            @Fire.canceled -= instance.OnFire;
            @Ability1.started -= instance.OnAbility1;
            @Ability1.performed -= instance.OnAbility1;
            @Ability1.canceled -= instance.OnAbility1;
            @Ability2.started -= instance.OnAbility2;
            @Ability2.performed -= instance.OnAbility2;
            @Ability2.canceled -= instance.OnAbility2;
            @Ability3.started -= instance.OnAbility3;
            @Ability3.performed -= instance.OnAbility3;
            @Ability3.canceled -= instance.OnAbility3;
            @ReloadScene.started -= instance.OnReloadScene;
            @ReloadScene.performed -= instance.OnReloadScene;
            @ReloadScene.canceled -= instance.OnReloadScene;
        }

        public void RemoveCallbacks(IWASDActions instance)
        {
            if (m_Wrapper.m_WASDActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IWASDActions instance)
        {
            foreach (var item in m_Wrapper.m_WASDActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_WASDActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public WASDActions @WASD => new WASDActions(this);

    // New action map
    private readonly InputActionMap m_Newactionmap;
    private List<INewactionmapActions> m_NewactionmapActionsCallbackInterfaces = new List<INewactionmapActions>();
    private readonly InputAction m_Newactionmap_Newaction;
    public struct NewactionmapActions
    {
        private @DefaultActionsWrapper m_Wrapper;
        public NewactionmapActions(@DefaultActionsWrapper wrapper) { m_Wrapper = wrapper; }
        public InputAction @Newaction => m_Wrapper.m_Newactionmap_Newaction;
        public InputActionMap Get() { return m_Wrapper.m_Newactionmap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(NewactionmapActions set) { return set.Get(); }
        public void AddCallbacks(INewactionmapActions instance)
        {
            if (instance == null || m_Wrapper.m_NewactionmapActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_NewactionmapActionsCallbackInterfaces.Add(instance);
            @Newaction.started += instance.OnNewaction;
            @Newaction.performed += instance.OnNewaction;
            @Newaction.canceled += instance.OnNewaction;
        }

        private void UnregisterCallbacks(INewactionmapActions instance)
        {
            @Newaction.started -= instance.OnNewaction;
            @Newaction.performed -= instance.OnNewaction;
            @Newaction.canceled -= instance.OnNewaction;
        }

        public void RemoveCallbacks(INewactionmapActions instance)
        {
            if (m_Wrapper.m_NewactionmapActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(INewactionmapActions instance)
        {
            foreach (var item in m_Wrapper.m_NewactionmapActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_NewactionmapActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public NewactionmapActions @Newactionmap => new NewactionmapActions(this);
    public interface IWASDActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
        void OnAbility1(InputAction.CallbackContext context);
        void OnAbility2(InputAction.CallbackContext context);
        void OnAbility3(InputAction.CallbackContext context);
        void OnReloadScene(InputAction.CallbackContext context);
    }
    public interface INewactionmapActions
    {
        void OnNewaction(InputAction.CallbackContext context);
    }
}
