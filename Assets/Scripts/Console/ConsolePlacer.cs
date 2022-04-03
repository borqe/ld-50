using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsolePlacer : MonoBehaviour
{
    [SerializeField] private ConsolePlacerData Data;

    private List<Console> SpawnedConsoles;

    private void Awake()
    {
        SpawnedConsoles = new List<Console>();
    }

    private void OnEnable()
    {
        GameStateChangedEvent.AddListener(OnGameStateChanged);
    }
    private void OnDisable()
    {
        GameStateChangedEvent.RemoveListener(OnGameStateChanged);
    }

    private void OnGameStateChanged(GameStateChangedEvent data)
    {
        if ((data.PreviousState == GameState.GameStateEnum.GameNotStarted || data.PreviousState == GameState.GameStateEnum.GameOver) &&
            data.State == GameState.GameStateEnum.InProgress)
        {
            DestroyConsoles();
            SpawnConsoles();
        }
    }

    private void DestroyConsoles()
    {
        while (SpawnedConsoles.Count > 0)
        {
            Console c = SpawnedConsoles.Last();

            SpawnedConsoles.Remove(c);
            Destroy(c.gameObject);
        }
    }

    private void SpawnConsoles()
    {
        float angle = 0;
        float deltaAngle = Mathf.PI * 2f / Data.AmountOfConsoles;
        for (int i = 0; i < Data.AmountOfConsoles; i++)
        {
            Console c = Instantiate(Data.ConsolePrefab, transform);
            c.transform.position = GetPoint(angle);
            c.transform.LookAt(Vector3.zero);

            angle += deltaAngle;

            SpawnedConsoles.Add(c);
        }
    }

    private Vector3 GetPoint(float angle)
    {
        return new Vector3(Mathf.Cos(angle) * Data.DistanceFromCenter, 0, Mathf.Sin(angle) * Data.DistanceFromCenter);
    }
}
