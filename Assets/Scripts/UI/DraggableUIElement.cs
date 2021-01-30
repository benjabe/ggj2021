using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Yeeter
{
    /// <summary>
    /// Makes it possible to drag a UI element around.
    /// </summary>
    public class DraggableUIElement : MonoBehaviour
    {
        private static List<DraggableUIElement> _draggableUIElements = new List<DraggableUIElement>();

        [Tooltip("The handle's RectTransform. If the player presses inside the handle they'll be able to drag the UI element.")]
        [SerializeField] private RectTransform _handle = null;
        [Tooltip("If true the entire element must be inside its parent.")]
        [SerializeField] private bool _constrainToParentBounds = false;
        [Tooltip("If true, dragging will cause the element to move over other draggable elements.")]
        [SerializeField] private bool _moveToTopWhenDragged = true;

        private Canvas _canvas;
        private RectTransform _rectTransform;
        private RectTransform _parentRectTransform;
        private bool _isDragging = false;
        private bool _lastClicked = false;
        private Vector3 _previousMousePosition;

        private void Awake()
        {
            _draggableUIElements.Add(this);
            _canvas = FindObjectsOfType<Canvas>().Where(
                canvas => canvas.GetComponentsInChildren<DraggableUIElement>().Contains(this)
            ).First();

            if (_canvas == null)
            {
                Debug.Log("<color=red>" + name + ": ResizableUIElement couldn't find its canvas.</color>");
            }
            _rectTransform = GetComponent<RectTransform>();
            if (_rectTransform.parent != null)
            {
                _parentRectTransform = _rectTransform.parent.GetComponent<RectTransform>();
            }
            else
            {
                _constrainToParentBounds = false;
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Start dragging.
                if (IsMouseInRect(_handle))
                {
                    _isDragging = true;
                    foreach (var other in _draggableUIElements)
                    {
                        if (other == this) continue;
                        if (!other.gameObject.activeInHierarchy || !other.IsMouseInRect(other._handle)) continue;
                        Debug.Log($"oh yeah! other: {other.transform.GetSiblingIndex()}, this: {transform.GetSiblingIndex()}");
                        if (other._isDragging && other.transform.GetSiblingIndex() > transform.GetSiblingIndex())
                        {
                            Debug.Log("OH it more ye");
                            _isDragging = false;
                            break;
                        }
                    }
                    if (_isDragging)
                    {
                        foreach (var other in _draggableUIElements)
                        {
                            if (other == this) continue;
                            other._isDragging = false;
                        }
                    }
                }
                if (_moveToTopWhenDragged && IsMouseInRect(_rectTransform))
                {
                    _lastClicked = true;
                    foreach (var other in _draggableUIElements)
                    {
                        if (other == this) continue;
                        if (!other.gameObject.activeInHierarchy || !other.IsMouseInRect(other._rectTransform)) continue;
                        if (!other._moveToTopWhenDragged) continue;
                        Debug.Log($"oh yeah! other: {other.transform.GetSiblingIndex()}, this: {transform.GetSiblingIndex()}");
                        if (other._lastClicked && other.transform.GetSiblingIndex() > transform.GetSiblingIndex())
                        {
                            Debug.Log("OH other top mannen MITHA krabs");
                            _lastClicked = false;
                            break;
                        }
                    }
                    if (_lastClicked)
                    {
                        int highestSiblingIndex = transform.GetSiblingIndex();
                        foreach (var other in _draggableUIElements)
                        {
                            if (other.transform.GetSiblingIndex() > highestSiblingIndex)
                            {
                                highestSiblingIndex = other.transform.GetSiblingIndex();
                            }
                        }
                        transform.SetSiblingIndex(highestSiblingIndex + 1);
                    }
                }
            }
            if (_isDragging && Input.GetMouseButton(0))
            {
                // Drag.
                var mouseDelta = Input.mousePosition - _previousMousePosition;
                _rectTransform.anchoredPosition += new Vector2(mouseDelta.x, mouseDelta.y);

                if (_constrainToParentBounds)
                {
                    // Limit the rect transform to be inside its parent.
                    // 0 = bottom left, 1 = bottom right, 2 = top right, 3 = top left
                    var corners = new Vector3[4];
                    _rectTransform.GetWorldCorners(corners);
                    var parentCorners = new Vector3[4];
                    _parentRectTransform.GetWorldCorners(parentCorners);
                    var min = corners[0] - parentCorners[0];
                    var max = corners[2] - parentCorners[2];
                    // Left.
                    if (mouseDelta.x < 0.0f && min.x < 0.0f)
                    {
                        _rectTransform.position -= Vector3.right * min.x;
                    }
                    // Right
                    if (mouseDelta.x > 0.0f && max.x > 0.0f)
                    {
                        _rectTransform.position -= Vector3.right * max.x;
                    }
                    // Top
                    if (mouseDelta.y > 0.0f && max.y > 0.0f)
                    {
                        _rectTransform.position -= Vector3.up * max.y;
                    }
                    // Bottom
                    if (mouseDelta.y < 0.0f && min.y < 0.0f)
                    {
                        _rectTransform.position -= Vector3.up * min.y;
                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                // Stop dragging.
                _isDragging = false;
            }
            _previousMousePosition = Input.mousePosition;
        }

        private void OnDestroy()
        {
            _draggableUIElements.Remove(this);
        }

        private bool IsMouseInRect(RectTransform rt)
        {
            if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                return RectTransformUtility.RectangleContainsScreenPoint(rt, Input.mousePosition, _canvas.worldCamera);
            }
            else
            {
                return RectTransformUtility.RectangleContainsScreenPoint(rt, Input.mousePosition);
            }
        }
    }
}