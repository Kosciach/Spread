using PlayerStateMachineSystem;
using UnityEngine;

namespace PlayerVerticalVel
{
    public class PlayerVerticalVelController : MonoBehaviour
    {
        [Header("---StateMachine---")]
        [SerializeField] PlayerStateMachine _playerStateMachine; public PlayerStateMachine PlayerStateMachine { get { return _playerStateMachine; } }


        [Space(20)]
        [Header("---SubComponents---")]
        [SerializeField] PlayerVerticalVel_GroundCheck _groundCheck; public PlayerVerticalVel_GroundCheck GroundCheck { get { return _groundCheck; } }
        [SerializeField] PlayerVerticalVel_Gravity _gravity; public PlayerVerticalVel_Gravity Gravity { get { return _gravity; } }
        [SerializeField] PlayerVerticalVel_Jump _jump; public PlayerVerticalVel_Jump Jump { get { return _jump; } }



        private void Awake()
        {
            _groundCheck.OnAwake(this);
            _gravity.OnAwake(this);
            _jump.OnAwake(this);
        }
        private void Update()
        {
            _groundCheck.OnUpdate();
            _gravity.OnUpdate();
        }

        private void OnEnable()
        {
            _jump.OnEnable();
        }
        private void OnDisable()
        {
            _jump.OnDisable();
        }
    }
}