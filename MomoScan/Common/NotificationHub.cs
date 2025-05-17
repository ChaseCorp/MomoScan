namespace MomoScan.Common;

public static class NotificationHub
{
    #region members
    #endregion
    
    #region properties
    public static event EventHandler ScanHandler = delegate { };
    #endregion
    
    static NotificationHub(){}

    public static void ScanOccured(object sender, EventArgs e) => ScanHandler(sender, e);
}