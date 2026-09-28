using UnityEngine;
using UnityEngine.InputSystem;

public class ExitMenuUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject exitPanel;
    [SerializeField] private GameManager gameManager;

    private PlayerControls inputControls;

    private void Awake()
    {
        inputControls = new PlayerControls();

        exitPanel.SetActive(false);
    }

    private void OnEnable()
    {
        inputControls.Player.Exit.performed += OnExitPressed;

        inputControls.Player.Enable();
    }

    private void OnDisable()
    {
        inputControls.Player.Exit.performed -= OnExitPressed;

        inputControls.Player.Disable();

        // Make sure the game is never left frozen.
        Time.timeScale = 1f;
    }

    private void OnExitPressed(InputAction.CallbackContext context)
    {
        ToggleExitPanel();
    }

    private void ToggleExitPanel()
    {
        bool isOpen = exitPanel.activeSelf;

        if (isOpen)
        {
            CloseExitMenu();
        }
        else
        {
            OpenExitMenu();
        }
    }

    private void OpenExitMenu()
    {
        exitPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    private void CloseExitMenu()
    {
        exitPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void ExitGame()
    {
        // Restore time before quitting.
        Time.timeScale = 1f;

        gameManager.QuitGame();
    }
}