// GENERATED AUTOMATICALLY FROM 'Assets/InputActions/MainInputAction.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @MainInputAction : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @MainInputAction()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""MainInputAction"",
    ""maps"": [
        {
            ""name"": ""Main"",
            ""id"": ""e99fc6d4-b0f0-45cc-81b1-09cfb81c7072"",
            ""actions"": [
                {
                    ""name"": ""Drag"",
                    ""type"": ""Button"",
                    ""id"": ""531b73c3-9db3-4ba2-8411-cf968b05f758"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseWheel"",
                    ""type"": ""Value"",
                    ""id"": ""93b0f540-d3e6-4060-9c1d-ee181fae90cd"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ce26f860-fb07-4a6c-80cb-b565c56c33c3"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Drag"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aeb5251e-4e6b-4930-ba77-908da5a8d9e7"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseWheel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Main
        m_Main = asset.FindActionMap("Main", throwIfNotFound: true);
        m_Main_Drag = m_Main.FindAction("Drag", throwIfNotFound: true);
        m_Main_MouseWheel = m_Main.FindAction("MouseWheel", throwIfNotFound: true);
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

    // Main
    private readonly InputActionMap m_Main;
    private IMainActions m_MainActionsCallbackInterface;
    private readonly InputAction m_Main_Drag;
    private readonly InputAction m_Main_MouseWheel;
    public struct MainActions
    {
        private @MainInputAction m_Wrapper;
        public MainActions(@MainInputAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @Drag => m_Wrapper.m_Main_Drag;
        public InputAction @MouseWheel => m_Wrapper.m_Main_MouseWheel;
        public InputActionMap Get() { return m_Wrapper.m_Main; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MainActions set) { return set.Get(); }
        public void SetCallbacks(IMainActions instance)
        {
            if (m_Wrapper.m_MainActionsCallbackInterface != null)
            {
                @Drag.started -= m_Wrapper.m_MainActionsCallbackInterface.OnDrag;
                @Drag.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnDrag;
                @Drag.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnDrag;
                @MouseWheel.started -= m_Wrapper.m_MainActionsCallbackInterface.OnMouseWheel;
                @MouseWheel.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnMouseWheel;
                @MouseWheel.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnMouseWheel;
            }
            m_Wrapper.m_MainActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Drag.started += instance.OnDrag;
                @Drag.performed += instance.OnDrag;
                @Drag.canceled += instance.OnDrag;
                @MouseWheel.started += instance.OnMouseWheel;
                @MouseWheel.performed += instance.OnMouseWheel;
                @MouseWheel.canceled += instance.OnMouseWheel;
            }
        }
    }
    public MainActions @Main => new MainActions(this);
    public interface IMainActions
    {
        void OnDrag(InputAction.CallbackContext context);
        void OnMouseWheel(InputAction.CallbackContext context);
    }
}
