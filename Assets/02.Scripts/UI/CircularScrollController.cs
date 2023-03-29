using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularScrollController : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform content;
    [SerializeField] private HorizontalLayoutGroup layoutGroup;

    private List<RectTransform> _elements;
    private int _elementCount;
    private float _elementSize;
    private float _currentPosition;

    private void Awake()
    {
        _elements = new List<RectTransform>();
        for (int i = 0; i < content.childCount; i++)
        {
            _elements.Add(content.GetChild(i).GetComponent<RectTransform>());
        }
        _elementCount = _elements.Count;
        _elementSize = layoutGroup.preferredWidth / _elementCount;
    }

    private void Update()
    {
        _currentPosition = scrollRect.horizontalNormalizedPosition * ((_elementCount - 1) * _elementSize);
        UpdateElementsPosition();
    }

    private void UpdateElementsPosition()
    {
        for (int i = 0; i < _elementCount; i++)
        {
            float distance = Mathf.Abs(_currentPosition - _elements[i].anchoredPosition.x);
            if (distance > _elementCount * _elementSize * 0.5f)
            {
                int sign = _currentPosition < _elements[i].anchoredPosition.x ? 1 : -1;
                _elements[i].anchoredPosition += new Vector2(sign * _elementCount * _elementSize, 0);
            }
        }
    }
}