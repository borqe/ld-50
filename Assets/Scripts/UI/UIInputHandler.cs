    using UnityEngine;

    public class UIInputHandler: MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                switch (GameState.CurrentState)
                {
                    case GameState.GameStateEnum.InProgress:
                        GameEventInvoker.onPauseGame?.Invoke();
                        break;
                    case GameState.GameStateEnum.Paused:
                        GameEventInvoker.onUnpauseGame?.Invoke();
                        break;
                    default:
                        break;
                }
                return;
            }
        }
    }