using System.Collections.Generic;
using Combat;
using Combat.Buffs;
using UnityEngine;

public class BuffListController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buffs = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    [SerializeField] private BuffPrefabMap buffPrefabMap;

    [SerializeField] private CombatSystem combatSystem;

    private List<GameObject> buffs;

    public void AddBuff(IBuff buff)
    {
        var buffController = CreateBuffObject(buff);
        buffController.OnApply();
    }

    public void RemoveBuff(IBuff buff)
    {
        // 找到所有满足条件的对象并销毁
        buffs.RemoveAll(obj =>
        {
            if (obj != null)
            {
                var buffController = obj.GetComponent<BuffController>();
                if (buffController == null || buffController.Buff != buff) return false;
                buffController.OnRemove();
                Destroy(obj); // 销毁 GameObject
                return true; // 从列表中移除
            }
            return false; // 保留在列表中
        });
    }

    public void ClearBuffs()
    {
        buffs.ForEach(obj =>
        {
            if (obj != null)
            {
                var buffController = obj.GetComponent<BuffController>();
                if (buffController != null) buffController.OnRemove();
                Destroy(obj); // 销毁 GameObject
            }
        });
        buffs.Clear(); // 清空列表
    }

    public void SetBuffs(List<IBuff> buffs)
    {
        ClearBuffs();
        buffs.ForEach(AddBuff);
    }

    private BuffController CreateBuffObject(IBuff buff)
    {
        var buffPrefab = buffPrefabMap.GetBuffPrefab(buff);
        var buffObject = Instantiate(buffPrefab, transform);
        buffs.Add(buffObject); // 添加到列表中
        var buffController = buffObject.GetComponent<BuffController>();
        buffController.Init(buff, combatSystem); // 初始化 BuffController
        return buffController;
    }

    public void Init(CombatSystem combatSystem)
    {
        this.combatSystem = combatSystem;
    }

    private void UpdatePosition() {
    }
}
