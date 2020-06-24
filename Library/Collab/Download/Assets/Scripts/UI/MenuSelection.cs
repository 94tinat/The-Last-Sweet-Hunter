using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSelection : MonoBehaviour
{
    public EnumPanel[] Panels;

    public static MenuSelection Instance { get; private set; } = null;

    #region Unity.MonoBehaviour Callbacks
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    #region User Methods
    public void EnablePanel(EnumPanel panelType)
    {
        foreach (EnumPanel panel in Panels)
        {
            panel.gameObject.SetActive((panel.myActivePanel == panelType.myActivePanel) ? true : false);
        }
    }
    #endregion
}
