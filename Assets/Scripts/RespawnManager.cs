using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour
{
    // Синглтон для удобного доступа из других скриптов
    public static RespawnManager Instance { get; private set; }

    private void Awake()
    {
        // Если экземпляр ещё не существует, назначаем его и не уничтожаем при загрузке новых сцен
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Если экземпляр уже есть, уничтожаем дубликат
            Destroy(gameObject);
        }
    }

    // Публичный метод для перезагрузки текущей сцены
    public void Respawn()
    {
        // Получаем индекс текущей активной сцены и загружаем её заново
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}