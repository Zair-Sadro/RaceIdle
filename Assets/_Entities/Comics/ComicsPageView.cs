
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComicsPageView : UIPanel
{
    [SerializeField] private Image _comicsImage;
    [SerializeField] private TMP_Text _comcisText;
    [SerializeField] private Button _exitButton;
    
    protected override void Start()
    {
        base.Awake();
        PanelInit(GetPanelAnimation());
        _exitButton.onClick.AddListener(Close);
    }
    public void SetComics(IComics iComics)
    {
        _comicsImage.sprite =  iComics.ComicsPicture;
        _comcisText.text = iComics.ComicsText;
    }

    public void OpenComics() => Open();

    public void CloseComics() => Close();
    #region PanelAnimations

    [SerializeField, Tooltip("Скорость появления панели")]
    private float _showSpeed = 0.5f;
    private Tween GetPanelAnimation()
    {
        return s_canvasGroup.DOFade(1, _showSpeed).Pause();

    }
  
    #endregion
}
