using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AyoooAIUI : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI hpTextVisual;
    PlayerController _player;
    void Start()
    {
        _player = PlayerController.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = _player.GetCurrentHealth();
        hpTextVisual.text = $"{_player.GetCurrentHealth()}/{_player.GetMaxHealth()}";
    }
}
