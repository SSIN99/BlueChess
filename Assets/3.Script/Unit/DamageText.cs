using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DamageText : MonoBehaviour
{
    [SerializeField] private Text damage;
    [SerializeField] private RectTransform rect;
    private Vector2 offset = new Vector2(20f, 20f);

    public void InitDamage(float damage, Vector2 pos)
    {
        this.damage.text = damage.ToString();
        rect.position = pos;

        rect.DOAnchorPos(rect.anchoredPosition + offset, 0.4f)
            .OnComplete(() =>
            {
                rect.DOAnchorPos(rect.anchoredPosition + Vector2.right * 5f, 0.4f);
                this.damage.DOFade(0f, 0.4f)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                });
            });
    }
}
