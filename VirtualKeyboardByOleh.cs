using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using System.Reflection;
using UnityEngine.EventSystems;


public class VirtualKeyboardByOleh : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public TMP_InputField _InputField = null;
    int m_CaretPosition = 0;
    bool isPressed = false;


    public void UpdateCaretPosition(int newPos) => _InputField.caretPosition = newPos;

    public void OnPointerDown(PointerEventData eventData)
    {
        if(gameObject.tag == "VirtualKey")
        {
            OnKeyDown(gameObject);
        }

        if (gameObject.tag == "VirtualKeyBackspace")
        {
            Backspace();
        }

        if (gameObject.tag == "VirtualKeyMoveCaretLeft")
        {
            MoveCaretLeft();
        }

        if (gameObject.tag == "VirtualKeyMoveCaretRight")
        {
            MoveCaretRight();
        }

        isPressed = true;
        StartCoroutine(MultiKey());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopCoroutine(MultiKey());
        isPressed = false;
    }

    public void SendKeyString(string keyString)
    {
        m_CaretPosition = _InputField.caretPosition;

        if (m_CaretPosition > 0)
        {
            if (_InputField.caretPosition < _InputField.text.Length || _InputField.caretPosition < _InputField.text.Length - 1) // Remove After CaretPosition
            {
                _InputField.text = _InputField.text.Insert(m_CaretPosition, keyString);
                ++m_CaretPosition;
                UpdateCaretPosition(m_CaretPosition);
                _InputField.ActivateInputField();
                return;
            }
            _InputField.text += keyString;
            ++m_CaretPosition;
            UpdateCaretPosition(m_CaretPosition);
        }
        if (m_CaretPosition == 0)
        {
            _InputField.text = _InputField.text.Insert(0, keyString);
            ++m_CaretPosition;
            UpdateCaretPosition(m_CaretPosition);
        }

        _InputField.ActivateInputField();
    }

    public void OnKeyDown(GameObject currentButton)
    {
        string keyName = currentButton.name;

        // check if text is selected
        if (_InputField.selectionFocusPosition != _InputField.caretPosition || _InputField.selectionAnchorPosition != _InputField.caretPosition)
        {
            if (_InputField.selectionAnchorPosition > _InputField.selectionFocusPosition) // right to left
            {
                int start_InputFieldSelectionFocusPosition = _InputField.selectionFocusPosition;
                _InputField.text = _InputField.text.Substring(0, _InputField.selectionFocusPosition) + _InputField.text.Substring(_InputField.selectionAnchorPosition);
                _InputField.caretPosition = start_InputFieldSelectionFocusPosition;
            }
            else // left to right
            {
                _InputField.text = _InputField.text.Substring(0, _InputField.selectionAnchorPosition) + _InputField.text.Substring(_InputField.selectionFocusPosition);
                _InputField.caretPosition = _InputField.selectionAnchorPosition;
            }

            m_CaretPosition = _InputField.caretPosition;
            _InputField.selectionAnchorPosition = m_CaretPosition;
            _InputField.selectionFocusPosition = m_CaretPosition;
            _InputField.ActivateInputField();
            SendKeyString(keyName);

        }
        else
        {
            m_CaretPosition = _InputField.caretPosition;

            if (m_CaretPosition > 0)
            {
                if (_InputField.caretPosition < _InputField.text.Length) // Remove After CaretPosition
                {
                    _InputField.text = _InputField.text.Insert(m_CaretPosition, keyName);
                    ++m_CaretPosition;
                    UpdateCaretPosition(m_CaretPosition);
                    _InputField.ActivateInputField();
                    return;
                }

                _InputField.text += keyName;
                ++m_CaretPosition;
                UpdateCaretPosition(m_CaretPosition);
            }

            if (m_CaretPosition == 0)
            {
                _InputField.text = _InputField.text.Insert(0, keyName);
                ++m_CaretPosition;
                UpdateCaretPosition(m_CaretPosition);
            }
        }
        _InputField.ActivateInputField();
    }

    /// <summary>
    /// Delete the character before the caret.
    /// </summary>
    public void Backspace()
    {
        // check if text is selected
        if (_InputField.selectionFocusPosition != _InputField.caretPosition || _InputField.selectionAnchorPosition != _InputField.caretPosition)
        {
            if (_InputField.selectionAnchorPosition > _InputField.selectionFocusPosition) // right to left
            {
                _InputField.text = _InputField.text.Substring(0, _InputField.selectionFocusPosition) + _InputField.text.Substring(_InputField.selectionAnchorPosition);
                _InputField.caretPosition = _InputField.selectionFocusPosition;
            }
            else // left to right
            {
                _InputField.text = _InputField.text.Substring(0, _InputField.selectionAnchorPosition) + _InputField.text.Substring(_InputField.selectionFocusPosition);
                _InputField.caretPosition = _InputField.selectionAnchorPosition;
            }

            m_CaretPosition = _InputField.caretPosition;
            _InputField.selectionAnchorPosition = m_CaretPosition;
            _InputField.selectionFocusPosition = m_CaretPosition;
        }
        else
        {
            m_CaretPosition = _InputField.caretPosition;
            if (m_CaretPosition > 0)
            {
                if (_InputField.caretPosition < _InputField.text.Length) // Remove After CaretPosition
                {
                    --m_CaretPosition;
                    _InputField.text = _InputField.text.Remove(m_CaretPosition, 1);
                    UpdateCaretPosition(m_CaretPosition);
                    _InputField.ActivateInputField();
                    return;
                }
                --m_CaretPosition;
                _InputField.text = _InputField.text.Remove(m_CaretPosition, 1);
                UpdateCaretPosition(m_CaretPosition);
            }
        }
        _InputField.ActivateInputField();
    }

    /// <summary>
    /// Move caret to the left.
    /// </summary>
    public void MoveCaretLeft()
    {
        m_CaretPosition = _InputField.caretPosition;
        if (m_CaretPosition > 0)
        {
            --m_CaretPosition;
            UpdateCaretPosition(m_CaretPosition);
        }
        _InputField.ActivateInputField();
    }

    /// <summary>
    /// Move caret to the right.
    /// </summary>
    public void MoveCaretRight()
    {
        if (_InputField.caretPosition == _InputField.text.Length)
        {
            m_CaretPosition = _InputField.caretPosition;
            _InputField.ActivateInputField();
            return;
        }

        m_CaretPosition = _InputField.caretPosition;

        if (m_CaretPosition < _InputField.text.Length)
        {
            ++m_CaretPosition;
            UpdateCaretPosition(m_CaretPosition);
        }
        _InputField.ActivateInputField();
    }

    IEnumerator MultiKey()
    {
        yield return new WaitForSeconds(1f);
        _InputField.caretBlinkRate = 100;
        while (isPressed)
        {
            if (gameObject.tag == "VirtualKey")
            {
                OnKeyDown(gameObject);
            }

            if (gameObject.tag == "VirtualKeyBackspace")
            {
                Backspace();
            }

            if (gameObject.tag == "VirtualKeyMoveCaretLeft")
            {
                MoveCaretLeft();
            }

            if (gameObject.tag == "VirtualKeyMoveCaretRight")
            {
                MoveCaretRight();
            }
            yield return new WaitForSeconds(0.1f);
            yield return null;
        }
        _InputField.caretBlinkRate = 2;
    }
}
