//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.3
//     from Assets/Project/Modules/InputSystem/Scripts/PlayerAnchorInputControls.inputactions
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

namespace InputSystem
{
    public partial class @PlayerAnchorInputControls: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @PlayerAnchorInputControls()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerAnchorInputControls"",
    ""maps"": [
        {
            ""name"": ""Land"",
            ""id"": ""65464c2a-640c-4dff-b71c-4fb41e7f02a7"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""7d7acd2a-6efd-442e-8d34-dbe1977bc43d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""fe2dd623-e0fe-4ae0-acb8-efea930e51e4"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Button"",
                    ""id"": ""c240019a-3d40-4d30-a865-8c21cc5c5238"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Throw"",
                    ""type"": ""Button"",
                    ""id"": ""78022133-4efb-4377-af08-7b12a37d300e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PickUp"",
                    ""type"": ""Button"",
                    ""id"": ""bd94e9a1-b31b-46cf-b77f-7b906b3fa22c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Pull"",
                    ""type"": ""Button"",
                    ""id"": ""c65dcb9c-2172-44ae-ac42-7fcc00bdad4a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""b5371a25-52a4-419e-8009-4afd4f2e9d11"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Kick"",
                    ""type"": ""Button"",
                    ""id"": ""77027586-2d0c-4667-9bc5-70b31877f699"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""f514eea7-842b-4d22-bece-cef1b9240a45"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""93f7016c-6cb7-42bf-8846-f1360ab26b36"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ef9c925c-c66f-4299-b960-e50870762a20"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""16898f63-544e-40a3-98ca-b65b8dcb3466"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1a686d8d-df71-43b5-a4df-529dea93cf9a"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""2ca53492-ab53-46ab-9b2b-a1186fd64a69"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""7db6671d-0352-4bdb-93ae-35c5145dd2d5"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""17533388-8dcd-4db0-b682-82edd9390d55"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""7ada8fa1-a810-4b22-ae40-a3804a4e2811"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ad26aea3-64e6-4c31-9f8b-5f38a2b2912b"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""63d812dc-170c-4ec9-aae1-8723288a0dfb"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""2f2b026a-11f5-4ca1-bd17-af682875cbff"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5ff001e8-d6d5-476a-ae81-da280269a3e9"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""Throw"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7112c1b7-12f8-45a5-92ec-d31ff33472a4"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""Throw"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""31344599-6688-44b7-8217-337bbdc2b960"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""PickUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a68eb38b-af5b-4fe8-ab87-a68d758c1a88"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""PickUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""15cb640b-cf85-4e0e-ab6f-98fec3400cb8"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3cd36bd9-17e1-4ce0-b008-bbd99baf169d"",
                    ""path"": ""<Gamepad>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4323a02c-94c3-4eb0-8449-b31f775d2305"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ad71bada-d416-44b3-985e-64a9c58e44e2"",
                    ""path"": ""<Gamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b986edb1-554b-46b0-84a7-84f293eb4559"",
                    ""path"": ""<Gamepad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""43d45cba-04aa-41b5-890c-f72a45a388de"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""Pull"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""79e8283f-b9af-43e0-b4b6-cdd463fed519"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""Pull"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""588042c2-32f9-4a80-a475-3d5090299484"",
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
                    ""id"": ""5a1f3c3d-8e59-46ef-9ca8-de56c32863ed"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""72d3ab2e-538e-4c67-be06-f721167155f5"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""Kick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7dfa0d8f-c417-4b72-a5cc-85386756b77e"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""Kick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Desktop"",
            ""bindingGroup"": ""Desktop"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<DualShockGamepad>"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<XInputController>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Land
            m_Land = asset.FindActionMap("Land", throwIfNotFound: true);
            m_Land_Move = m_Land.FindAction("Move", throwIfNotFound: true);
            m_Land_Look = m_Land.FindAction("Look", throwIfNotFound: true);
            m_Land_Aim = m_Land.FindAction("Aim", throwIfNotFound: true);
            m_Land_Throw = m_Land.FindAction("Throw", throwIfNotFound: true);
            m_Land_PickUp = m_Land.FindAction("PickUp", throwIfNotFound: true);
            m_Land_Pull = m_Land.FindAction("Pull", throwIfNotFound: true);
            m_Land_Dash = m_Land.FindAction("Dash", throwIfNotFound: true);
            m_Land_Kick = m_Land.FindAction("Kick", throwIfNotFound: true);
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

        // Land
        private readonly InputActionMap m_Land;
        private List<ILandActions> m_LandActionsCallbackInterfaces = new List<ILandActions>();
        private readonly InputAction m_Land_Move;
        private readonly InputAction m_Land_Look;
        private readonly InputAction m_Land_Aim;
        private readonly InputAction m_Land_Throw;
        private readonly InputAction m_Land_PickUp;
        private readonly InputAction m_Land_Pull;
        private readonly InputAction m_Land_Dash;
        private readonly InputAction m_Land_Kick;
        public struct LandActions
        {
            private @PlayerAnchorInputControls m_Wrapper;
            public LandActions(@PlayerAnchorInputControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @Move => m_Wrapper.m_Land_Move;
            public InputAction @Look => m_Wrapper.m_Land_Look;
            public InputAction @Aim => m_Wrapper.m_Land_Aim;
            public InputAction @Throw => m_Wrapper.m_Land_Throw;
            public InputAction @PickUp => m_Wrapper.m_Land_PickUp;
            public InputAction @Pull => m_Wrapper.m_Land_Pull;
            public InputAction @Dash => m_Wrapper.m_Land_Dash;
            public InputAction @Kick => m_Wrapper.m_Land_Kick;
            public InputActionMap Get() { return m_Wrapper.m_Land; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(LandActions set) { return set.Get(); }
            public void AddCallbacks(ILandActions instance)
            {
                if (instance == null || m_Wrapper.m_LandActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_LandActionsCallbackInterfaces.Add(instance);
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
                @Throw.started += instance.OnThrow;
                @Throw.performed += instance.OnThrow;
                @Throw.canceled += instance.OnThrow;
                @PickUp.started += instance.OnPickUp;
                @PickUp.performed += instance.OnPickUp;
                @PickUp.canceled += instance.OnPickUp;
                @Pull.started += instance.OnPull;
                @Pull.performed += instance.OnPull;
                @Pull.canceled += instance.OnPull;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @Kick.started += instance.OnKick;
                @Kick.performed += instance.OnKick;
                @Kick.canceled += instance.OnKick;
            }

            private void UnregisterCallbacks(ILandActions instance)
            {
                @Move.started -= instance.OnMove;
                @Move.performed -= instance.OnMove;
                @Move.canceled -= instance.OnMove;
                @Look.started -= instance.OnLook;
                @Look.performed -= instance.OnLook;
                @Look.canceled -= instance.OnLook;
                @Aim.started -= instance.OnAim;
                @Aim.performed -= instance.OnAim;
                @Aim.canceled -= instance.OnAim;
                @Throw.started -= instance.OnThrow;
                @Throw.performed -= instance.OnThrow;
                @Throw.canceled -= instance.OnThrow;
                @PickUp.started -= instance.OnPickUp;
                @PickUp.performed -= instance.OnPickUp;
                @PickUp.canceled -= instance.OnPickUp;
                @Pull.started -= instance.OnPull;
                @Pull.performed -= instance.OnPull;
                @Pull.canceled -= instance.OnPull;
                @Dash.started -= instance.OnDash;
                @Dash.performed -= instance.OnDash;
                @Dash.canceled -= instance.OnDash;
                @Kick.started -= instance.OnKick;
                @Kick.performed -= instance.OnKick;
                @Kick.canceled -= instance.OnKick;
            }

            public void RemoveCallbacks(ILandActions instance)
            {
                if (m_Wrapper.m_LandActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(ILandActions instance)
            {
                foreach (var item in m_Wrapper.m_LandActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_LandActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public LandActions @Land => new LandActions(this);
        private int m_DesktopSchemeIndex = -1;
        public InputControlScheme DesktopScheme
        {
            get
            {
                if (m_DesktopSchemeIndex == -1) m_DesktopSchemeIndex = asset.FindControlSchemeIndex("Desktop");
                return asset.controlSchemes[m_DesktopSchemeIndex];
            }
        }
        public interface ILandActions
        {
            void OnMove(InputAction.CallbackContext context);
            void OnLook(InputAction.CallbackContext context);
            void OnAim(InputAction.CallbackContext context);
            void OnThrow(InputAction.CallbackContext context);
            void OnPickUp(InputAction.CallbackContext context);
            void OnPull(InputAction.CallbackContext context);
            void OnDash(InputAction.CallbackContext context);
            void OnKick(InputAction.CallbackContext context);
        }
    }
}
