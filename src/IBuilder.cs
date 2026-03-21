public interface IBuilder
{
    string Name { get; }
    bool CanHandle(string extension);
    void Build(ProjectConfig config);
}