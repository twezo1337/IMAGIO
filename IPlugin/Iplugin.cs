namespace IPlugin
{
    public interface Iplugin
    {
        string IconSrc { get; }
        string Name { get; }
        bool IsProVersion { get; }
        string Function(string src, int id, int width = 0, int height = 0, float value = 0, int g = 0, int b = 0);
    }
}