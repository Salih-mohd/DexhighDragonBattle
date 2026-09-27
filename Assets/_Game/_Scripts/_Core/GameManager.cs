using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Characters")]
    [SerializeField] private DragonHealth playerHealth;
    [SerializeField] private DragonHealth enemyHealth;

    [Header("State Machines")]
    [SerializeField] private PlayerStateMachine playerStateMachine;
    [SerializeField] private EnemyStateMachine enemyStateMachine;

    [Header("Winner UI")]
    [SerializeField] private GameObject winnerPanel;
    [SerializeField] private TMP_Text winnerText;

    [Header("Timing")]
    [SerializeField] private float winnerScreenDelay = 0.75f;

    private bool fightEnded;

    private void OnEnable()
    {
        playerHealth.OnDied += HandlePlayerDied;
        enemyHealth.OnDied += HandleEnemyDied;
    }

    private void OnDisable()
    {
        playerHealth.OnDied -= HandlePlayerDied;
        enemyHealth.OnDied -= HandleEnemyDied;
    }

    private void Start()
    {
        winnerPanel.SetActive(false);
    }

    private void HandlePlayerDied()
    {
        EndFight("ENEMY WINS!");
         
    }

    private void HandleEnemyDied()
    {
        EndFight("PLAYER WINS!");
    }

    private void EndFight(string winner)
    {
        if (fightEnded)
            return;

        fightEnded = true;

        StartCoroutine(
            EndFightRoutine(winner)
        );
    }

    private IEnumerator EndFightRoutine(string winner)
    {
        yield return null;

        yield return new WaitForSeconds(
            winnerScreenDelay
        );

        if (winner == "PLAYER WINS!")
        {
            playerStateMachine.SwitchState(
                playerStateMachine.IdleState
            );
        }
        else if (winner == "ENEMY WINS!")
        {
            enemyStateMachine.SwitchState(
                enemyStateMachine.IdleState
            );
        }

        yield return null;

        playerStateMachine.enabled = false;
        enemyStateMachine.enabled = false;

        winnerText.text = winner;
        winnerPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}