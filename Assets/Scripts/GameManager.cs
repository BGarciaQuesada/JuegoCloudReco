using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Esta clase se dedica a almacenar los datos del juego y manejar los métodos que cambian la "fase"

    public static GameManager Instance { get; private set; }

    [Header("Fragments")]
    private int totalFragments = 5;
    private int collectedFragments = 0;

    [Header("UI")]
    [SerializeField] public TextMeshProUGUI fragmentCounterText;
    [SerializeField] public TextMeshProUGUI hintText;
    [SerializeField] public GameObject victoryPanel;

    // Si esto se fuera expandir se podría incluir un documento o algo del que leer esto 
    // en vez de asignarlo en el editor, pero para la escala actual se quedará como array
    [Header("Hints")]
    [SerializeField] public string[] hints; // tamaño = totalFragments

    private int currentHintIndex = 0;

    public int GetCurrentHintIndex => currentHintIndex;


    // Singleton
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    void Start()
    {
        UpdateCounter();
        hintText.text = hints[0]; // primera pista inmediata
    }

    public void FragmentCollected()
    {
        collectedFragments++;
        UpdateCounter();

        if (collectedFragments >= totalFragments)
        {
            ShowVictory();
        }
        else
        {
            currentHintIndex++;
            hintText.text = hints[currentHintIndex];
        }
    }

    void UpdateCounter()
    {
        fragmentCounterText.text = "Fragmentos: " + collectedFragments + " / " + totalFragments;
    }

    void ShowVictory()
    {
        victoryPanel.SetActive(true);
    }

}
