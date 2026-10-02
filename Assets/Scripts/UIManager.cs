using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public GameObject healthTextPrefab;
    public Canvas gameCanvas;

    private Camera mainCamera;

    [System.Obsolete]
    private void Awake() {
        mainCamera = Camera.main;
        if (gameCanvas == null) {
            gameCanvas = FindObjectOfType<Canvas>();
        }
    }

    private void OnEnable() {
        CharacterEvents.characterDamaged += CharacterTookDamage;
        CharacterEvents.characterHealed += CharacterHealed;
    }

    private void OnDisable() {
        CharacterEvents.characterDamaged -= CharacterTookDamage;
        CharacterEvents.characterHealed -= CharacterHealed;
    }

    public void CharacterTookDamage(GameObject character, int damageReceived) {
        Vector3 spawnPosition = gameCanvas.renderMode == RenderMode.ScreenSpaceOverlay 
            ? mainCamera.WorldToScreenPoint(character.transform.position) 
            : character.transform.position;

        TMP_Text tmpText = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform)
            .GetComponent<TMP_Text>();

        tmpText.text = damageReceived.ToString();
    }

    public void CharacterHealed(GameObject character, int healthRestored) {
        Vector3 spawnPosition = gameCanvas.renderMode == RenderMode.ScreenSpaceOverlay 
            ? mainCamera.WorldToScreenPoint(character.transform.position) 
            : character.transform.position;

        TMP_Text tmpText = Instantiate(healthTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform)
            .GetComponent<TMP_Text>();

        tmpText.text = healthRestored.ToString();
    }

    public void OnExit(InputAction.CallbackContext context) {
        if (context.started) {
            #if (UNITY_EDITOR || DEVELOPMENT_BUILD)
                Debug.Log($"{name} : {GetType()} : {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            #endif

            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #elif UNITY_STANDALONE
                Application.Quit();
            #elif UNITY_WEBGL
                SceneManager.LoadScene("QuitScene");
            #endif
        }
    }
}
