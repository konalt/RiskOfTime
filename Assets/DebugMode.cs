using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugMode : MonoBehaviour
{
    public CharacterController2D player;

    private TextMeshProUGUI text;

    private string Property(string pname, string value)
    {
        return $"{pname}: {value}\n";
    }
    private string GetText()
    {
        string str = "Risk of Time - Debug\n";
        str += Property("PlayerX", ((float)Mathf.FloorToInt(player.transform.position.x * 100)/100f).ToString());
        str += Property("PlayerY", ((float)Mathf.FloorToInt(player.transform.position.y * 100)/100f).ToString());
        str += "- Checks\n";
        str += Property("  CheckGround", player.floor.ToString());
        str += Property("  CheckCeiling", player.ceiling.ToString());
        str += Property("  CheckLWall", player.leftWall.ToString());
        str += Property("  CheckRWall", player.rightWall.ToString());
        return str;
    }
    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.SetText(GetText());
    }
    private void Update()
    {
        text.SetText(GetText());
    }
}
