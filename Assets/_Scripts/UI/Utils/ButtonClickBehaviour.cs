using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonClickBehaviour : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float resizeCoefficient = 1.1f;
    [SerializeField] private float animDuration = 0.35f;

    public Button button;
    
    private Vector2 normalSize;
    private RectTransform _rectTransform;
    
    // Start is called before the first frame update
    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        normalSize = _rectTransform.sizeDelta;

        if (!button)
            button = GetComponent<Button>();
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (button && button.interactable && button.enabled)
            _rectTransform.DOSizeDelta(normalSize * resizeCoefficient, animDuration);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (button && button.interactable && button.enabled)
            _rectTransform.DOSizeDelta(normalSize,animDuration);
    }

    public void HideButton()
    {
        gameObject.SetActive(false);
    }
    public void ShowButton()
    {
        gameObject.SetActive(true);
    }
}
