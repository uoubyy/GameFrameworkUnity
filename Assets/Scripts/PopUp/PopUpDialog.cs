using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Events;

public class PopUpDialog : MonoBehaviour
{
    [SerializeField] Canvas m_dialogCanvas;
    [SerializeField] GameObject m_dialogPanel;

    [SerializeField] TextMeshProUGUI m_dialogTitle;
    [SerializeField] TextMeshProUGUI m_dialgeMessage;

    [SerializeField] Button m_btnClose;
    [SerializeField] Button m_btnConfirm;

    [SerializeField] Ease m_hideEase = Ease.InBack;
    [SerializeField] Ease m_showEase = Ease.OutBack;

    void Start()
    {
        m_btnClose.onClick.AddListener(Hide);
    }

    public void Init(UnityAction closeAction, UnityAction confirmAction, string title = "News", string message = "", int layer = 0)
    {
        m_btnClose.onClick.RemoveAllListeners();
        m_btnClose.onClick.AddListener(Hide);
        m_btnClose.onClick.AddListener(closeAction);

        m_btnConfirm.onClick.RemoveAllListeners();
        m_btnConfirm.onClick.AddListener(Hide);
        m_btnConfirm.onClick.AddListener(confirmAction);

        m_dialogTitle.text = title;
        m_dialgeMessage.text = message;

        m_dialogCanvas.sortingOrder = layer;
    }

    public void Show()
    {
        m_dialogPanel.transform.localScale = Vector3.zero;
        this.gameObject.SetActive(true);

        m_dialogPanel.transform.DOScale(1.0f, 0.5f).SetEase(m_showEase);
    }

    public void Hide()
    {
        m_dialogPanel.transform.DOScale(0.0f, 0.5f).SetEase(m_hideEase).OnComplete(delegate () { this.gameObject.SetActive(false); });
    }

}
