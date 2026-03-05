using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct SelectionButton
{
    public Button teamButton;
    public GameObject selectionBox;
    public Color color;
}
public class TeamSelector : MonoBehaviour
{
    [SerializeField] private TeamColorLookup colorLookup;
    [SerializeField] private SpriteRenderer[] playerSprites;
    [SerializeField] private SelectionButton[] selectionButtons;
    [SerializeField] private int teamIndex = 0;

    public const string PlayerTeamKey = "PlayerTeamIndex";

    private void OnValidate()
    {
        for(int i = 0; i < selectionButtons.Length; i++)
        {
            selectionButtons[i].color = (Color)colorLookup.GetTeamColor(i);
        }   

        foreach(SelectionButton selection in selectionButtons)
        {
            selection.teamButton.image.color = selection.color;  
        }
    }
    private void Start()
    {
        teamIndex = PlayerPrefs.GetInt(PlayerTeamKey, 0);
        HandleTeamChanged();
    }

    public void HandleTeamChanged()
{
    // 1. ตรวจสอบก่อนว่ามีข้อมูลใน Array หรือไม่
    if (selectionButtons == null || selectionButtons.Length == 0) return;

    // 2. ป้องกัน Index เกิน (Clamping)
    // ถ้า index น้อยกว่า 0 ให้เป็น 0, ถ้ามากกว่าจำนวนที่มี ให้เป็นตัวสุดท้าย
    teamIndex = Mathf.Clamp(teamIndex, 0, selectionButtons.Length - 1);

    foreach (SelectionButton selection in selectionButtons)
    {
        // เพิ่ม null check เล็กน้อยเพื่อความชัวร์
        if (selection.selectionBox != null)
            selection.selectionBox.SetActive(false);            
    }

    foreach (SpriteRenderer sprite in playerSprites)
    {
        if (sprite != null)
            sprite.color = selectionButtons[teamIndex].color;
    }

    if (selectionButtons[teamIndex].selectionBox != null)
        selectionButtons[teamIndex].selectionBox.SetActive(true);
}

    public void SelectTeam(int teamIndex)
    {
        this.teamIndex = teamIndex;
        HandleTeamChanged();
    }

    public void SaveTeam()
    {
        PlayerPrefs.SetInt(PlayerTeamKey, teamIndex);
    }
}
 