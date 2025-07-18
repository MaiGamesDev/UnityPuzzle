using UnityEngine;
using UnityEngine.UI;

public class BgImageController : MonoBehaviour
{
    private Image backgroundImage;

    private void Awake()
    {
        backgroundImage = GetComponent<Image>();
    }

    public void UpdateBg(GameObject bgPrefab)
    {
        backgroundImage.sprite = bgPrefab.GetComponent<Image>().sprite;
    }
}
