public interface IBuff {
    BuffType Type { get ;}
    string Name { get ;}
    string Description { get ;}

    int Count { get ;}

    void OnApply();

    void OnRemove();

    void OnUpdate(int count);
}