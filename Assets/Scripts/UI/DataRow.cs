using TMPro;
using UnityEngine;
using UnityEngine.UI;
//todo testy
//todo wywalic nieuzywane pliki
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
        => categoryType switch
        {
            DataItem.CategoryType.RED => badgeRed,
            DataItem.CategoryType.GREEN => badgeGreen,
            DataItem.CategoryType.BLUE => badgeBlue,
            _ => null,
        };
}
