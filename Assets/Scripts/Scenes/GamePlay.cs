using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GamePlay: MonoBehaviour
{
    [Header("Ui Components")]
    public Image backgroundGameplay;

    [Header("Trash Bin")]
    public Image tbOrganik;
    public Image tbAnOrganik;
    public Image tbB3;

    [Header("Progress Bars")]
    public Slider barTbOrganik;
    public Slider barTbAnOrganik;
    public Slider barTbB3;


    [Header("Text Mash Pro Bar")]
    public TMP_Text txtTbBarOrganik;
    public TMP_Text txtTbBarAnOrganik;
    public TMP_Text txtTbBarB3;
}
