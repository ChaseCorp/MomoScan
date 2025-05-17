using System;
using System.IO;
using System.Xml.Serialization;
using MomoScan.Common;

namespace MomoScan.Utils;

public static class SessionTool
{
    #region members
    private static string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    private static string appFolder = Path.Combine(localAppDataPath, "MomoScan");
    private static string _volPoolPath = $"{appFolder} + vol_pool.xml";
    private static string _disPoolPath = "";
    private static string _tagAssociationsPath = "";
    #endregion
    
    public static void SaveSession(object data, SessionType type)
    {
        switch (type)
        {
            case SessionType.VolunteerPool:
                if (data != null)
                    SaveVolunteerPool(data);
                break;
            case SessionType.DispatchPool:
                break;
            case SessionType.TagAssociationList:
                break;
            default:
                break;
        } 
    }

    private static void SaveVolunteerPool(object data)
    {
        try
        {
            List<PoolItem> volunteerPool = data as List<PoolItem>;

            if (volunteerPool.Count > 0)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<PoolItem>));

                using (FileStream fs = new FileStream(_volPoolPath, FileMode.Create))
                {
                    serializer.Serialize(fs, volunteerPool);
                }
            }
        }
        catch (Exception ex)
        {
#if DEBUG
            Console.WriteLine($"Ex caught in SessionTool: {ex.Message}");
#endif
        }
    }
}

public enum SessionType
{
    VolunteerPool,
    DispatchPool,
    TagAssociationList,
    None
}