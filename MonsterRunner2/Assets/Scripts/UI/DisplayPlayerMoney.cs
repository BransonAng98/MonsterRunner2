using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayPlayerMoney : MonoBehaviour
{
    public PlayerDataSO playerData;
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        this.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        text.text = playerData.moneyAccumulatedInGame.ToString();
    }

    private void OnDestroy()
    {
        playerData.moneyAccumulatedInGame = 0;
    }
}
