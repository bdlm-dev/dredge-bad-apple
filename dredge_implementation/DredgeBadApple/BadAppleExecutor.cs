using Winch.Core;

namespace DredgeBadApple;

public class BadAppleExecutor
{
    private PotManager _potManager;
    private DataLoader _dataLoader;

    public BadAppleExecutor()
    {
        try {
            _dataLoader = new DataLoader();
            _potManager = PotManager.Instance(_dataLoader);
        } catch (Exception e) {
            WinchCore.Log.Debug(e.ToString());
        }
    }
}