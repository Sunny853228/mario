using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Teleport : MonoBehaviour
{
    [Header("UI References")]
    public GameObject choicePanel;          // панель с кнопками
    public Button prevButton;               // кнопка "Предыдущий уровень"
    public Button currentButton;             // кнопка "Этот уровень"
    public Button nextButton;                // кнопка "Следующий уровень"
    public TextMeshProUGUI prevText;         // текст на кнопке "Предыдущий"
    public TextMeshProUGUI currentText;       // текст на кнопке "Текущий"
    public TextMeshProUGUI nextText;          // текст на кнопке "Следующий"

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
        //.Log("Trigger");
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
        //Debug.Log($"Текущий индекс: {SceneManager.GetActiveScene().buildIndex}, Всего сцен: {SceneManager.sceneCountInBuildSettings}");

        if (choicePanel == null) return;

        int totalScenes = SceneManager.sceneCountInBuildSettings;
        int currentIndex = SceneManager.GetActiveScene().buildIndex;

        // Настройка левой кнопки (предыдущий / главное меню)
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

        // Центральная кнопка (текущий уровень)
        currentText.text = "Этот уровень";
        currentButton.onClick.RemoveAllListeners();
        currentButton.onClick.AddListener(() => LoadSceneByIndex(currentIndex));

        // Настройка правой кнопки (следующий / главное меню)
        if (currentIndex < totalScenes - 1) // есть следующий уровень
        {
            nextText.text = "Следующий";
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(() => LoadSceneByIndex(currentIndex + 1));
        }
        else // последний уровень (или главное меню, если оно последнее)
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