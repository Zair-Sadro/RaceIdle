using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeAreaDetection : MonoBehaviour
{
    //Вешается на пустого родителя который растянут на весь экран 
	//его чайлд - элемент ui
	//это решение чтобы шапка айфона не перекрыла верхний ui (т.е) на нижние элементы не нужно это вешать
	private RectTransform _rectTransform;

	private void Awake()
	{
		_rectTransform = GetComponent<RectTransform>();
		RefreshPanel(Screen.safeArea);
	}


	private void RefreshPanel(Rect safeArea)
	{

		Vector2 anchorMin = safeArea.position;
		Vector2 anchorMax = safeArea.position + safeArea.size;

		anchorMin.x /= Screen.width;
		anchorMin.y /= Screen.height;
		anchorMax.x /= Screen.width;
		anchorMax.y /= Screen.height;

		_rectTransform.anchorMin = anchorMin;
		_rectTransform.anchorMax = anchorMax;
	}
}
