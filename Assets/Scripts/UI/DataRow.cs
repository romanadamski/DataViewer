using TMPro;
using UnityEngine;
using UnityEngine.UI;
//todo testy
//todo wywalic nieuzywane pliki
//todo obsluga bledow - jakis komunikat
//todo komentarze
public class DataRow : MonoBehaviour
{
    [Header("Fields")]

    [SerializeField]
    private Image badge;
    [SerializeField]
    private TextMeshProUGUI rowNumber;
    [SerializeField]
    private TextMeshProUGUI dataText;
    [SerializeField]
    private GameObject glow;

    [Header("Badge sprites")]

    [SerializeField]
    private Sprite badgeRed;
    [SerializeField]
    private Sprite badgeGreen;
    [SerializeField]
    private Sprite badgeBlue;

    public void Init(DataItem dataItem, int number)
    {
        badge.sprite = GetRowBadge(dataItem.Category);
        dataText.text = dataItem.Description;
        rowNumber.text = number.ToString();
        glow.SetActive(dataItem.Special);
    }

    private Sprite GetRowBadge(DataItem.CategoryType categoryType)
    {
        switch (categoryType)
        {
            case DataItem.CategoryType.RED:
                return badgeRed;
            case DataItem.CategoryType.GREEN:
                return badgeGreen;
            case DataItem.CategoryType.BLUE:
                return badgeBlue;
            default:
                return null;
        }
    }
}
