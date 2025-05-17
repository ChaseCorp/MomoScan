namespace MomoScan.Common;

public class DispatchItem
{
    public string Name { get; set; }
    public string TagNumber { get; set; }
    public string Location { get; set; }
    public DateTime ReturnTime { get; set; }

    public DispatchItem(string name, string tagNumber, string location, DateTime returnTimeTime)
    {
        Name = name;
        TagNumber = tagNumber;
        Location = location;
        ReturnTime = returnTimeTime;
    }
}