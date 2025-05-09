namespace WaltlustTest;


using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private string _URL = "Hello, World!";
    
    public MainWindowViewModel()
    {
        // Constructor logic can go here if needed
        LoadConfiguration();
    }

    private void LoadConfiguration()
    {
        // Load configuration logic can go here if needed
    }
}
