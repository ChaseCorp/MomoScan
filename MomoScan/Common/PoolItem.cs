namespace MomoScan.Common;

public class PoolItem
{
    #region members
    private bool _status = false;
    private string _displayStatus = "OFFLINE";
    private DateTime _lastActivity = DateTime.Now;
    #endregion
    
    #region properties
    public string Name { get; set; }

    public bool Status
    {
        get => _status;
        set
        {
            if (value)
            {
                _displayStatus = "ONLINE";
            }
            else
            {
                _displayStatus = "OFFLINE";
            }
            _status = value;
        }
    }
    public string TagNumber { get; set; }

    public string DisplayStatus
    {
        get => _displayStatus;
    }
    
    public DateTime LastActivity { get => _lastActivity; set => _lastActivity = value; }
    #endregion

    public PoolItem(string name, string tagNumber)
    {
        Name = name;
        TagNumber = tagNumber;
    }

    public PoolItem()
    {
        
    }
}