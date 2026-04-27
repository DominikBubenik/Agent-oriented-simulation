using System.IO;

namespace Simulation;

public class Config
{
    public static readonly string BACKGROUND_IMG_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "background.jpg");
    public static readonly Image BACKGROUND_IMG;
    static Config()
    {
        BACKGROUND_IMG = Image.FromFile(BACKGROUND_IMG_PATH);
    }
}