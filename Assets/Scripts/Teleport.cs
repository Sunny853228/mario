using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    [Header("UI References")]
    public GameObject choicePanel;          // панель с кнопками выбора уровня
    public Button prevButton;               // кнопка "Предыдущий уровень"
    public Button currentButton;            // кнопка "Текущий уровень"
    public Button nextButton;               // кнопка "Следующий уровень"
    public TextMeshProUGUI prevText;        // текст на кнопке "Предыдущий"
    public TextMeshProUGUI currentText;      // текст на кнопке "Текущий"
    public TextMeshProUGUI nextText;         // текст на кнопке "Следующий"

    [Header("Levels")]
    public GameObject[] levels;              // массив объектов уровней (порядок важен!)
    public Transform[] spawnPoints;          // массив точек спавна для каждого уровня (соответствует порядку уровней)

    private int currentLevelIndex;           // индекс текущего активного уровня

    void Start()
    {
        // Скрываем панель при старте
        if (choicePanel != null)
            choicePanel.SetActive(false);

        // Определяем, какой уровень сейчас активен (ищем включённый)
        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i].activeSelf)
            {
                currentLevelIndex = i;
                break;
            }
        }
    }

    // При входе игрока в зону телепорта
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ShowChoicePanel();
        }
    }

    // При выходе игрока из зоны телепорта
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HideChoicePanel();
        }
    }

    // Показать панель выбора и настроить кнопки
    void ShowChoicePanel()
    {
        if (choicePanel == null) return;

        // --- Левая кнопка (предыдущий уровень или главное меню) ---
        if (currentLevelIndex > 0) // если есть предыдущий уровень
        {
            prevText.text = "Предыдущий уровень";
            prevButton.onClick.RemoveAllListeners();
            prevButton.onClick.AddListener(() => SwitchLevel(currentLevelIndex - 1));
        }
        else // если это первый уровень
        {
            prevText.text = "Главное меню";
            prevButton.onClick.RemoveAllListeners();
            prevButton.onClick.AddListener(() => LoadMainMenu());
        }

        // --- Центральная кнопка (текущий уровень) ---
        currentText.text = "Этот уровень";
        currentButton.onClick.RemoveAllListeners();
        currentButton.onClick.AddListener(() => SwitchLevel(currentLevelIndex));

        // --- Правая кнопка (следующий уровень или главное меню) ---
        if (currentLevelIndex < levels.Length - 1) // если есть следующий уровень
        {
            nextText.text = "Следующий уровень";
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(() => SwitchLevel(currentLevelIndex + 1));
        }
        else // если это последний уровень
        {
            nextText.text = "Главное меню";
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(() => LoadMainMenu());
        }

        // Активируем панель
        choicePanel.SetActive(true);
    }

    // Переключение на уровень с указанным индексом
    void SwitchLevel(int index)
    {
        if (index < 0 || index >= levels.Length)
        {
            Debug.LogError($"SwitchLevel: индекс {index} вне диапазона (0-{levels.Length - 1})");
            return;
        }

        // Отладочный вывод
        Debug.Log($"SwitchLevel вызван с индексом {index}");

        // Выключаем все уровни
        foreach (GameObject level in levels)
        {
            if (level != null)
                level.SetActive(false);
        }

        // Включаем нужный уровень
        levels[index].SetActive(true);
        currentLevelIndex = index;

        // Перемещаем игрока в точку спавна этого уровня
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found! Проверьте тег Player.");
            HideChoicePanel();
            return;
        }

        if (spawnPoints[index] == null)
        {
            Debug.LogError($"Spawn point для уровня {index} не назначен!");
            HideChoicePanel();
            return;
        }

        // Устанавливаем позицию через Rigidbody2D, если есть, иначе через transform
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.position = spawnPoints[index].position;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        else
        {
            player.transform.position = spawnPoints[index].position;
        }

        // Синхронизируем физику, чтобы коллайдеры обновились
        Physics2D.SyncTransforms();

        Debug.Log($"Player position set to: {player.transform.position}");

        // Скрываем панель
        HideChoicePanel();
    }

    // Загрузка главного меню (если оно в отдельной сцене)
    void LoadMainMenu()
    {
        Debug.Log("Загрузка главного меню");
        SceneManager.LoadScene("MainMenu"); // Убедитесь, что сцена называется именно так
    }

    // Скрыть панель выбора
    void HideChoicePanel()
    {
        if (choicePanel != null)
            choicePanel.SetActive(false);
    }
}