using UnityEngine;

public class UIInputHandler : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tilde))
        {
            switch (GameState.Instance.CurrentState)
            {
                case GameState.GameStateEnum.InProgress:
                    new SetGameStateEvent(GameState.GameStateEnum.InTerminalWindow).Broadcast();
                    break;
                case GameState.GameStateEnum.InTerminalWindow:
                    new SetGameStateEvent(GameState.GameStateEnum.InProgress).Broadcast();
                    break;
                default:
                    break;
            }
            return;
        }
    }
}