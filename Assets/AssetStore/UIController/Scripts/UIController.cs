using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class UIController : MonoBehaviour {

	public enum OnHideAction {
		None,
		Disable,
		Destroy,
	}
	public class PlayAsync : CustomYieldInstruction {

		public PlayAsync(UIController controller, bool isShow) {
			this.m_Obj = controller;
			this.m_ObjName = controller.ToString();

			if (isShow) {
				controller.Show(this.OnCompleted);
			}
			else {
				controller.Hide(this.OnCompleted);
			}
		}

		public override bool keepWaiting {
			get {
				if (this.m_Obj == null) {
					throw new System.Exception(this.m_ObjName + " is be destroy, you can't keep waiting.");
				}
				return !this.m_IsDone;
			}
		}

		private UIController m_Obj;
		private string m_ObjName;
		private bool m_IsDone;

		private void OnCompleted() {
			this.m_IsDone = true;
		}
	}

	public void PanelInit(Tween tween,Tween backTween = null)
    {
		_tween = tween;
		_backwardTween = backTween;
		_backTweenInitialized = backTween != null;

		_tween.SetAutoKill(false);

    }

	public bool showOnAwake = true;
	public OnHideAction onHideAction = OnHideAction.Disable;

	[SerializeField] private UnityEvent m_OnShow = new UnityEvent();
	[SerializeField] private UnityEvent m_OnHide = new UnityEvent();
	private UnityEvent m_OnShowDisposable = new UnityEvent();
	private UnityEvent m_OnHideDisposable = new UnityEvent();

	private Tween _tween;
	private Tween _backwardTween;
	private bool _backTweenInitialized;

	public UnityEvent onShow {
		get { return this.m_OnShow; }
	}
	public UnityEvent onHide {
		get { return this.m_OnHide; }
	}
	public bool isShow 
	{
		get 
		{
			return _tween.ElapsedPercentage() > 0.99f;
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
				_backwardTween.PlayForward();
			}
            else
            {
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
	private bool isShowWhenNoController;
	private int lastShowAtFrame = -1;

	// Show/Hide must fast by Show(UnityAction)Hide(UnityAction), make SendMessage("Show/Hide") working in Inspector
	public virtual void Show() {
		if (this.isShow) {
			if (!this.isPlaying) {
				this.OnShow();
			}
			return;
		}

		if (!this.gameObject.activeSelf) {
			this.lastShowAtFrame = Time.frameCount;
			this.gameObject.SetActive(true);
		}
		this.isShow = true;
	}
	public virtual void Hide() {
		if (!this.isShow) {
			if (!this.isPlaying) {
				this.OnHide();
			}
			return;
		}
		this.isShow = false;
	}
	public void Show(UnityAction onShow) {
		if (onShow != null) {
			m_OnShowDisposable.AddListener(onShow);
		}
		this.Show();
	}
	public void Hide(UnityAction onHide) {
		if (onHide != null) {
			m_OnHideDisposable.AddListener(onHide);
		}
		this.Hide();
	}
	public PlayAsync ShowAsync() {
		return new PlayAsync(this, true);
	}
	public PlayAsync HideAsync() {
		return new PlayAsync(this, false);
	}

	protected virtual void OnShow() {
		this.onShow.Invoke();
		this.m_OnShowDisposable.Invoke();
		this.m_OnShowDisposable.RemoveAllListeners();
	}
	protected virtual void OnHide() {
		switch (this.onHideAction) {
			case OnHideAction.None:
				break;
			case OnHideAction.Disable:
				this.gameObject.SetActive(false);
				_tween.Kill();
				break;
			case OnHideAction.Destroy:
				Destroy(this.gameObject);
				break;
		}
		this.onHide.Invoke();
		this.m_OnHideDisposable.Invoke();
		this.m_OnHideDisposable.RemoveAllListeners();
	}
}
