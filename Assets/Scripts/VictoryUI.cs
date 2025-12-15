using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Heredo IPointerClickHandler también para que detecte hacer clic en cualquier lugar de la pantalla (abrir cofre)
public class VictoryUI : MonoBehaviour, IPointerClickHandler
{
    // Esta clase maneja todo lo relacionado al panel final (abrir el cofre y regreso al menú principal)

    [Header("Chest")]
    [SerializeField] private Image chestImage;
    [SerializeField] private Sprite closedChestSprite;
    [SerializeField] private Sprite openChestSprite;

    [Header("Victory UI")]
    [SerializeField] private GameObject congratsGroup;

    private bool opened = false;

    private void Awake()
    {
        chestImage.sprite = closedChestSprite;
        congratsGroup.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OpenChest();
    }

    public void OpenChest()
    {
        // Si ya se ha abierto, da igual cuanto tap, no volvemos a ejecutar el método
        if (opened) return;

        opened = true;

        chestImage.sprite = openChestSprite;
        congratsGroup.SetActive(true); // congrats group es texto de enhorabuena + boton de regreso
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
