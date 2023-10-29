using PlayerStateMachineSystem;
using UnityEngine;

namespace PlayerMovement
{
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("---StateMachine---")]
        [SerializeField] PlayerStateMachine _playerStateMachine; public PlayerStateMachine PlayerStateMachine { get { return _playerStateMachine; } }


        [Space(20)]
        [Header("---MovementTypes---")]
        [SerializeField] PlayerMovement_OnGround _onGround; public PlayerMovement_OnGround OnGround { get { return _onGround; } }
        [SerializeField] PlayerMovement_InAir _inAir; public PlayerMovement_InAir InAir { get { return _inAir; } }




        private void Awake()
        {
            _onGround.OnAwake(this);
            _inAir.OnAwake(this);
        }
    }
}