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
        float totalWidth = CalculateTotalWidth();
        _elementSize = totalWidth / _elementCount;
    }

    void Update()
    {
        float newPosition = content.anchoredPosition.x;
        int direction = (newPosition - _currentPosition) > 0 ? -1 : 1;
        float elementsToMove = Mathf.Abs(newPosition - _currentPosition) / _elementSize;

        if (elementsToMove >= 1)
        {
            for (int i = 0; i < Mathf.FloorToInt(elementsToMove); i++)
            {
                if (direction > 0)
                {
                    MoveFirstToLast();
                }
                else
                {
                    MoveLastToFirst();
                }
            }
            _currentPosition = newPosition - (direction * Mathf.FloorToInt(elementsToMove) * _elementSize);
            content.anchoredPosition = new Vector2(_currentPosition, content.anchoredPosition.y);
        }
    }

    private void MoveFirstToLast()
    {
        RectTransform firstChild = _elements[0];
        firstChild.SetAsLastSibling();
        firstChild.anchoredPosition += Vector2.right * (_elementSize * _elementCount);
        _elements.RemoveAt(0);
        _elements.Add(firstChild);
    }

    private void MoveLastToFirst()
    {
        RectTransform lastChild = _elements[_elementCount - 1];
        lastChild.SetAsFirstSibling();
        lastChild.anchoredPosition -= Vector2.right * (_elementSize * _elementCount);
        _elements.RemoveAt(_elementCount - 1);
        _elements.Insert(0, lastChild);
    }
    private float CalculateTotalWidth()
    {
        float totalWidth = 0f;

        for (int i = 0; i < content.childCount; i++)
        {
            RectTransform child = content.GetChild(i).GetComponent<RectTransform>();
            totalWidth += child.rect.width;
        }

        totalWidth += layoutGroup.spacing * (content.childCount - 1);
        return totalWidth;
    }
}