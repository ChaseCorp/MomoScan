using MomoScan.Models;

namespace MomoScan.ViewModels;

public class MainWindowViewModel
{
    private MainWindowModel model;

    public MainWindowModel Model
    {
        get => model;
        set => model = value;
    }
    
    public MainWindowViewModel()
    {
        model = new MainWindowModel();
    }
}