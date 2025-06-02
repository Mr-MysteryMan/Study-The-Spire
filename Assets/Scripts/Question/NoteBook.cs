using UnityEngine;

public class NoteBook : MonoBehaviour
{
    public GameObject noteBookUI;

    public GameObject page1;
    public GameObject page2;
    public GameObject QuestionStorePrefab;

    private int page = 0; // 当前页码
    void Start()
    {
        QuestionStore.Init(); // 确保题库已初始化
        upDatePage(); // 更新页面内容
    }

    public void nextPage()
    {
        if (page * 2 + 2 < QuestionStore.EnableQuestionListNames.Count)
        {
            page++;
            upDatePage();
        }
    }

    public void previousPage()
    {
        if (page > 0)
        {
            page--;
            upDatePage();
        }
    }

    public void edit()
    {
        // 创建题库UI
        Instantiate(QuestionStorePrefab);
        // 关闭笔记本
        close();
    }

    public void close()
    {
        Destroy(noteBookUI);
    }

    private void upDatePage()
    {
        string name1 = QuestionStore.EnableQuestionListNames[page * 2];
        page1.GetComponent<Page>().updateStatus(name1, QuestionStore.questionStore[name1].questions);
        if (QuestionStore.EnableQuestionListNames.Count > (page * 2 + 1))
        {
            string name2 = QuestionStore.EnableQuestionListNames[page * 2 + 1];
            page2.GetComponent<Page>().updateStatus(name2, QuestionStore.questionStore[name2].questions);
        }
        else
        {
            page2.GetComponent<Page>().updateStatus("", new QuestionData[0]);
        }
    }
}
