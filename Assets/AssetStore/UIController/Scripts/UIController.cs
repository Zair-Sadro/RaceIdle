using UnityEngine;
using DG.Tweening;
using System;

[RequireComponent(typeof(CanvasGroup))]
public class UIController : MonoBehaviour {

	protected CanvasGroup s_canvasGroup;
	public enum OnHideAction {
		None,
		Disable,
		Destroy,
	}

    protected virtual void Awake()
    {
        s_canvasGroup= GetComponent<CanvasGroup>();
    }
	protected virtual void Start()
	{

	}
    public void PanelInit(Tween tween,Tween backTween = null)
    {

		_tween = tween;
        _tween.onComplete += (() => { s_canvasGroup.blocksRaycasts = true; });

        _backwardTween = backTween;
		_backTweenInitialized = backTween != null;

		_tween.SetAutoKill(false);

		if (showOnAwake) Show();
    }

	public bool showOnAwake = false;
	private OnHideAction onHideAction = OnHideAction.Disable;

	public event Action OnPanelShow, OnPanelHide;
	private Tween _tween;
	private Tween _backwardTween;
	private bool _backTweenInitialized;

	public bool isShow 
	{
		get 
		{
			return _tween.ElapsedPercentage() > 0.5f;
		}
		private set 			 
		{
            if (value)
            {
				
				_tween.PlayForward();
				

                return;
			}

            if (_backTweenInitialized && !value)
            {
                s_canvasGroup.blocksRaycasts = false;
                _backwardTween.PlayForward();
			}
            else
            {
                s_canvasGroup.blocksRaycasts = false;
                _tween.PlayBackwards();
				return;
            }
		}
	}
    public bool isPlaying 
	{
		get
		{   if (_backTweenInitialized)
			{
				return _backwardTween.IsPlaying() || _tween.IsPlaying();
			}
            else
            {
				return _tween.IsPlaying();
			}
		}
	}

	public virtual void Show() {
		if (this.isShow) {
			if (!this.isPlaying) {
				this.OnShow();
			}
			return;
		}

		if (!this.gameObject.activeSelf) {
			this.gameObject.SetActive(true);
		}
		this.isShow = true;
		OnPanelShow?.Invoke();

    }
	public virtual void Hide() {
		if (!this.isShow) {
			if (!this.isPlaying) {
				this.OnHide();
			}
			return;
		}
		this.isShow = false;
        OnPanelHide?.Invoke();
    }
	protected virtual void OnShow() {}
	protected virtual void OnHide() 
	{
		switch (this.onHideAction) 
		{
			case OnHideAction.None:
				break;

			case OnHideAction.Disable:
				this.gameObject.SetActive(false);
				break;

			case OnHideAction.Destroy:
                Destroy(this.gameObject);
				break;
		}

	}
}
