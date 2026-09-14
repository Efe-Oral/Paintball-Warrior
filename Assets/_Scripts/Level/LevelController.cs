using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    private enum State { Playing, Won, Lost }

    [SerializeField] private GameplayConfig config;
    [SerializeField] private CoverageGrid coverageGrid;
    [SerializeField] private PaintTank paintTank;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PaintSprayer paintSprayer;

    public event Action<int, float> LevelWon;
    public event Action<float> LevelLost;

    private State state = State.Playing;

    private void OnEnable()
    {
        coverageGrid.CoverageChanged += HandleCoverageChanged;
        paintTank.Emptied += HandleTankEmptied;
    }

    private void OnDisable()
    {
        coverageGrid.CoverageChanged -= HandleCoverageChanged;
        paintTank.Emptied -= HandleTankEmptied;
    }

    private void Update()
    {
        if (state == State.Playing)
        {
            return;
        }

        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void HandleCoverageChanged(float percent)
    {
        if (state == State.Playing && percent >= config.coverageThreshold)
        {
            Win();
        }
    }

    private void HandleTankEmptied()
    {
        if (state == State.Playing)
        {
            Lose();
        }
    }

    private void Win()
    {
        state = State.Won;
        playerMovement.enabled = false;
        paintSprayer.enabled = false;

        float remaining = paintTank.NormalizedRemaining;
        int stars = 1;
        if (remaining >= config.star3RemainingThreshold)
        {
            stars = 3;
        }
        else if (remaining >= config.star2RemainingThreshold)
        {
            stars = 2;
        }

        LevelWon?.Invoke(stars, coverageGrid.CoveragePercent);
    }

    private void Lose()
    {
        state = State.Lost;
        playerMovement.enabled = false;
        paintSprayer.enabled = false;

        LevelLost?.Invoke(coverageGrid.CoveragePercent);
    }
}
