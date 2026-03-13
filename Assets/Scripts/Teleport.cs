using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Teleport : MonoBehaviour
{
    [Header("UI References")]
    public GameObject choicePanel;
    public Button prevButton;
    public Button currentButton;
    public Button nextButton;
    public TextMeshProUGUI prevText;
    public TextMeshProUGUI currentText;
    public TextMeshProUGUI nextText;

    private int currentSceneIndex;
    private int totalScenesInBuild;

    void Start()
    {
        if (choicePanel != null)
            choicePanel.SetActive(false);

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        totalScenesInBuild = SceneManager.sceneCountInBuildSettings;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ShowChoicePanel();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HideChoicePanel();
        }
    }

    void ShowChoicePanel()
    {
        if (choicePanel == null) return;

        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;

        // Левая кнопка
        if (currentIndex > 1) // есть предыдущий уровень (не главное меню)
        {
            prevText.text = "Предыдущий";
            prevButton.onClick.RemoveAllListeners();
            prevButton.onClick.AddListener(() => LoadSceneByIndex(currentIndex - 1));
        }
        else // первый уровень или главное меню
        {
            prevText.text = "Главное меню";
            prevButton.onClick.RemoveAllListeners();
            prevButton.onClick.AddListener(() => LoadSceneByIndex(0));
        }

        // Центральная кнопка
        currentText.text = "Этот уровень";
        currentButton.onClick.RemoveAllListeners();
        currentButton.onClick.AddListener(() => LoadSceneByIndex(currentIndex));

        // Правая кнопка
        if (currentIndex < totalScenes - 1) // есть следующий уровень
        {
            nextText.text = "Следующий";
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(() => LoadSceneByIndex(currentIndex + 1));
        }
        else // последний уровень
        {
            nextText.text = "Главное меню";
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(() => LoadSceneByIndex(0));
        }

        choicePanel.SetActive(true);
    }

    void HideChoicePanel()
    {
        if (choicePanel != null)
            choicePanel.SetActive(false);
    }

    void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }
}