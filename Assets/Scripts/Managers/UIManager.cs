using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject gameOverPanel;
        
    public void HideAllPanels()
    {
        gameOverPanel.SetActive(false);
    }
        
    public void OnGameOverEvent()
    {
        gameOverPanel.SetActive(true);
    }
}
