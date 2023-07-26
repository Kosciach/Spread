//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/Inputs/ThrowableInput.inputactions
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

public partial class @ThrowableInput : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @ThrowableInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""ThrowableInput"",
    ""maps"": [
        {
            ""name"": ""Map"",
            ""id"": ""161f2dcf-4509-40ca-99fe-51be1a9cf5c6"",
            ""actions"": [
                {
                    ""name"": ""Detonate"",
                    ""type"": ""Button"",
                    ""id"": ""afa4753d-a111-4a1c-b59c-13af30bb8181"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c5efad20-10a1-4692-a6cc-a64484e1d0ba"",
                    ""path"": ""<Keyboard>/b"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Detonate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Map
        m_Map = asset.FindActionMap("Map", throwIfNotFound: true);
        m_Map_Detonate = m_Map.FindAction("Detonate", throwIfNotFound: true);
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

    // Map
    private readonly InputActionMap m_Map;
    private IMapActions m_MapActionsCallbackInterface;
    private readonly InputAction m_Map_Detonate;
    public struct MapActions
    {
        private @ThrowableInput m_Wrapper;
        public MapActions(@ThrowableInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Detonate => m_Wrapper.m_Map_Detonate;
        public InputActionMap Get() { return m_Wrapper.m_Map; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MapActions set) { return set.Get(); }
        public void SetCallbacks(IMapActions instance)
        {
            if (m_Wrapper.m_MapActionsCallbackInterface != null)
            {
                @Detonate.started -= m_Wrapper.m_MapActionsCallbackInterface.OnDetonate;
                @Detonate.performed -= m_Wrapper.m_MapActionsCallbackInterface.OnDetonate;
                @Detonate.canceled -= m_Wrapper.m_MapActionsCallbackInterface.OnDetonate;
            }
            m_Wrapper.m_MapActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Detonate.started += instance.OnDetonate;
                @Detonate.performed += instance.OnDetonate;
                @Detonate.canceled += instance.OnDetonate;
            }
        }
    }
    public MapActions @Map => new MapActions(this);
    public interface IMapActions
    {
        void OnDetonate(InputAction.CallbackContext context);
    }
}