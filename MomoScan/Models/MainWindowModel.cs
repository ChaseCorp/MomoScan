using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MomoScan.Common;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.Core;

namespace MomoScan.Models;

public class MainWindowModel
{
    #region members
    private string _dispatchTagNumber = string.Empty;
    private string _dispatchLocation = string.Empty;
    private DateTime _dispatchReturnTime;
    private string _checkTag = string.Empty;
    private ObservableCollection<DispatchItem> _dispatchePool = new ObservableCollection<DispatchItem>();
    private string _poolName = string.Empty;
    private string _poolTagNumber = string.Empty;
    private ObservableCollection<PoolItem> _volunteerPool = new ObservableCollection<PoolItem>();
    #endregion
    
    #region properties
    public string DispatchTagNumber
    {
        get => _dispatchTagNumber; set => _dispatchTagNumber = value; 
    }
    public string DispatchLocation
    {
        get => _dispatchLocation; set => _dispatchLocation = value;
    }
    public DateTime DispatchReturnTime
    {
        get => _dispatchReturnTime; set => _dispatchReturnTime = value;
    }

    public string CheckTag
    {
        get => _checkTag;
        set => _checkTag = value;
    }
    public ObservableCollection<DispatchItem> DispatchePool
    {
        get => _dispatchePool; set => _dispatchePool = value;
    }
    public string PoolName
    {
        get => _poolName; set => _poolName = value;
    }
    public string PoolTagNumber
    {
        get => _poolTagNumber; set => _poolTagNumber = value;
    }

    public ObservableCollection<PoolItem> VolunteerPool
    {
        get => _volunteerPool;
        set
        {
            _volunteerPool = value;
            RaisePropertyChanged(nameof(VolunteerPool));
        } 
    }
    #endregion
    
    #region commands
    public ICommand AddToPoolCommand{get;set;}
    public ICommand DispatchVolunteer{get;set;}
    public ICommand ClearDispatchTab{get;set;}
    #endregion

    public MainWindowModel()
    {
        DispatchePool = new ObservableCollection<DispatchItem>();
        VolunteerPool = new ObservableCollection<PoolItem>();
        
        AddToPoolCommand = new RelayCommand(_ => AddToPool());
        DispatchVolunteer = new RelayCommand(_ => AddToDispatch());
        NotificationHub.ScanHandler += Scanned;
    }
    
    public void AddToPool()
    {
        bool noDuplicate = true;

        foreach (var item in VolunteerPool)
        {
            if (item.TagNumber == PoolTagNumber)
                noDuplicate = false;
            if (item.Name == PoolName)
                noDuplicate = false;
        }

        if (noDuplicate)
        {
            var newVolunteer = new PoolItem(PoolName, PoolTagNumber);
            VolunteerPool.Add(newVolunteer);
            
            VolunteerPool.OrderBy(x => x.Name);
        }
        else
        {
            MessageBox.Show(
                "Duplicate Entry", 
                "Conflict With Name or Tag");
        }
        
        PoolName = string.Empty;
        PoolTagNumber = string.Empty;
        RaisePropertyChanged(nameof(PoolName));
        RaisePropertyChanged(nameof(PoolTagNumber));
    }

    public void AddToDispatch()
    {
        try
        {
            var matchedEntry = (
                    from entry
                        in VolunteerPool
                    where entry.TagNumber == DispatchTagNumber
                    select entry)
                .FirstOrDefault();
            
            var dispatchVolunteer = new DispatchItem(matchedEntry.Name, matchedEntry.TagNumber, DispatchLocation, DispatchReturnTime);
            DispatchePool.Remove(dispatchVolunteer);
            DispatchePool.Add(dispatchVolunteer);
            RaisePropertyChanged(nameof(DispatchePool));
        }
        catch
        {
            MessageBox.Show($"Unknown Tag: {DispatchTagNumber}", "Unknown Tag Scanned");
        }
    }

    private void Scanned(object sender, EventArgs e)
    {
        string tagNumber = sender as string;
        try
        {
            var matchedEntry = (
                    from entry
                        in VolunteerPool
                        where entry.TagNumber == tagNumber
                        select entry)
                        .FirstOrDefault();

            if (CheckIfReturnFromDispatcher(tagNumber))
            {
                var matchedDispatch = (
                    from entry 
                        in DispatchePool
                        where entry.TagNumber == tagNumber
                        select entry)
                        .FirstOrDefault();
                
                DispatchePool.Remove(matchedDispatch);
            }
            else
            {
                matchedEntry.Status = !matchedEntry.Status;
                matchedEntry.LastActivity = DateTime.Now;
                CheckTag = String.Empty;
            
                VolunteerPool.Remove(matchedEntry);
                VolunteerPool.Add(matchedEntry);
                RaisePropertyChanged(nameof(VolunteerPool));
            }
        }
        catch
        {
            MessageBox.Show($"Unknown Tag: {tagNumber}", "Unknown Tag Scanned");
        }
        
    }

    private bool CheckIfReturnFromDispatcher(string scannedTag)
    {
        foreach (var item in DispatchePool)
        {
            if (item.TagNumber == scannedTag)
                return true;
        }

        return false;
    }
    
    protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}