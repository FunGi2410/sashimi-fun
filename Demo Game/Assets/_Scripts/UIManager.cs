using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject settingTable;

    public GameObject joystick;

    bool isOpenSettingTable;

    private void Start()
    {
        this.isOpenSettingTable = false;
        this.settingTable.SetActive(isOpenSettingTable);
    }
    public void DisplaySetting()
    {
        this.isOpenSettingTable = !this.isOpenSettingTable;
        this.settingTable.SetActive(isOpenSettingTable);
        joystick.SetActive(!isOpenSettingTable);
    }
}
