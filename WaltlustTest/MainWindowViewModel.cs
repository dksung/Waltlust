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
    private string? _ServerUrl;
    partial void OnServerUrlChanged(string? value)
    {
        ConfigSetting.ServerUrl = value;
    }

    [ObservableProperty]
    private string? _ShopCode;
    partial void OnShopCodeChanged(string? value)
    {
        ConfigSetting.ShopCode = value;
    }

    [ObservableProperty]
    private string? _UserPhoneNumber;
    partial void OnUserPhoneNumberChanged(string? value)
    {
        ConfigSetting.UserPhoneNumber = value;
    }

    private ConfigSettings ConfigSetting { get; set; }

    [ObservableProperty]
    private uint _CouponCount;

    public ObservableCollection<string> CouponList { get; set; } = new ObservableCollection<string>();

    [ObservableProperty]
    private string _SelectedCoupon = string.Empty;

    public ObservableCollection<string> LogList { get; set; } = new ObservableCollection<string>();


    [ObservableProperty]
    private WaldService _Service;

    public ShopType ShopType { get; set; }

    public MainWindowViewModel()
    {
        // Constructor logic can go here if needed
        LoadConfiguration();

        Service = new WaldService(ServerUrl, ShopCode, UserPhoneNumber);
        CouponCount = (uint)Service.exist_coupon_queue.Count;
        CouponList = [.. Service.exist_coupon_queue];
    }

    private void LoadConfiguration()
    {
        try
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            ConfigSetting = config.Get<ConfigSettings>();
            ServerUrl = ConfigSetting?.ServerUrl;
            ShopCode = ConfigSetting?.ShopCode;
            UserPhoneNumber = ConfigSetting?.UserPhoneNumber;
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
    }

    [RelayCommand]
    private void SaveStampCancel()
    {
        Service.CancelStamp();
        LogList.Add(Service.console_log);
    }

    [RelayCommand]
    private void UseCoupon()
    {
        Service.UseCoupon();
        LogList.Add(Service.console_log);
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
    }

    [RelayCommand]
    private void GetShopInfo()
    {
        Service.GetStoreData();
        LogList.Add(Service.console_log);
    }
}

public class ConfigSettings
{
    public string? ServerUrl { get; set; }
    public string? ShopCode { get; set; }
    public string? UserPhoneNumber { get; set; }
}