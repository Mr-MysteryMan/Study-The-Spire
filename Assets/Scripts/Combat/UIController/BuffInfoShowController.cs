using Combat.Buffs;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BuffController))]
public class BuffInfoShowController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject buffInfoPanel;
    [SerializeField] private BuffController buffController;

    public void OnPointerEnter(PointerEventData eventData)
    {
        buffInfoPanel.SetActive(true);
        buffInfoPanel.GetComponentInChildren<TMPro.TMP_Text>().text = buffController.Buff.Description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buffInfoPanel.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.buffController = GetComponent<BuffController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
