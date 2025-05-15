namespace WaltlustTest;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Windows.Data;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    public ConfigSettings _ConfigSetting;

    [ObservableProperty]
    private uint _CouponCount;

    [ObservableProperty]
    public ObservableCollection<string> _CouponList = new ObservableCollection<string>();

    [ObservableProperty]
    private string _SelectedCoupon = string.Empty;

    public ObservableCollection<string> LogList { get; set; } = new ObservableCollection<string>();

    private void NotifyPropertyChanged(string propertyName)
    {
        OnPropertyChanged(propertyName);
    }


    [ObservableProperty]
    private WaldService _Service;

    public ShopType ShopType { get; set; }

    public MainWindowViewModel()
    {
        // Constructor logic can go here if needed
        LoadConfiguration();

        Service = new WaldService(ConfigSetting.ServerUrl, ConfigSetting.ShopCode, ConfigSetting.UserPhoneNumber);
        
        RefreshCouponList();
    }

    private void LoadConfiguration()
    {
        try
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            ConfigSetting = config.GetRequiredSection("StoreInfo").Get<ConfigSettings>();
        }
        catch (Exception)
        {
        }
    }

    [RelayCommand]
    private void ModifySetting()
    {
        string output = JsonConvert.SerializeObject(ConfigSetting, Formatting.Indented);
        File.WriteAllText("appsettings.json", output);
    }

    [RelayCommand]
    private void SaveStamp()
    {
        Service.SaveStamp();
        LogList.Add(Service.console_log);

        GetUserInfo();
    }

    [RelayCommand]
    private void SaveStampCancel()
    {
        Service.CancelStamp();
        LogList.Add(Service.console_log);

        RefreshCouponList();
    }

    [RelayCommand]
    private void UseCoupon()
    {
        Service.UseCoupon(SelectedCoupon);
        LogList.Add(Service.console_log);

        RefreshCouponList();
    }

    [RelayCommand]
    private void UseCouponCancel()
    {
        Service.CancelCoupon();
        LogList.Add(Service.console_log);
    }

    [RelayCommand]
    private void GetCouponInfo()
    {
        Service.GetCouponInfo(SelectedCoupon);
        LogList.Add(Service.console_log);

        RefreshCouponList();
    }

    [RelayCommand]
    private void SavePoint()
    {
        LogList.Add(Service.console_log);
        Service.SavePoint();
    }

    [RelayCommand]
    private void SavePointCancel()
    {
        Service.CancelPoint();
        LogList.Add(Service.console_log);
    }

    [RelayCommand]
    private void UsePoint()
    {
        Service.UsePoint();
        LogList.Add(Service.console_log);
    }

    [RelayCommand]
    private void UsePointCancel()
    {
        Service.CancelUsePoint();
        LogList.Add(Service.console_log);
    }

    [RelayCommand]
    private void GetUserInfo()
    {
        Service.GetUserData();
        LogList.Add(Service.console_log);

        RefreshCouponList();
    }

    [RelayCommand]
    private void GetShopInfo()
    {
        Service.GetStoreData();
        LogList.Add(Service.console_log);
    }

    [RelayCommand]
    private void CopyLog(object param)
    {
        if (param is IEnumerable<object> objects)
        {
            System.Windows.Clipboard.SetText(string.Join(Environment.NewLine, objects));
        }
    }

    private void RefreshCouponList()
    {
        CouponCount = (uint)Service.Exist_coupon_queue.Count;

        var currentSelectedCoupon = SelectedCoupon;
        CouponList.Clear();
        foreach (var coupon in Service.Exist_coupon_queue)
        {
            CouponList.Add(coupon);
        }

        if (CouponList.Count > 0)
        {
            if (string.IsNullOrEmpty(currentSelectedCoupon) || CouponList.Contains(currentSelectedCoupon) == false)
            {
                SelectedCoupon = CouponList[0];
            }
            else
            {
                SelectedCoupon = currentSelectedCoupon;
            }
        }
    }
}

public partial class ConfigSettings : ObservableObject
{
    public string? ServerUrl { get; set; }
    public string? ShopCode { get; set; }
    public string? UserPhoneNumber { get; set; }
}