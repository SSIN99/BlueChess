using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DamageText : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvas;
    [SerializeField] private Text damage;
    [SerializeField] private GameObject critical;
    [SerializeField] private RectTransform rect;
    private Vector2 offset = new Vector2(15f, 20f);

    public void InitDamage(float d, Vector2 pos, bool crit)
    {
        damage.text = d.ToString();
        rect.position = pos;

        if (crit)
        {
            critical.SetActive(true);
        }
        else
        {
            critical.SetActive(false);
        }
        
        rect.DOAnchorPos(rect.anchoredPosition + offset, 0.4f)
            .OnComplete(() =>
            {
                rect.DOAnchorPos(rect.anchoredPosition + Vector2.right * 5f, 0.4f);
                canvas.DOFade(0f, 0.4f)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                });
            });
    }
}
