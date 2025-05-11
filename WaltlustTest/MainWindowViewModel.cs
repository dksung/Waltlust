namespace WaltlustTest;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
    private WaldService _Service;

    public ShopType ShopType { get; set; }

    public MainWindowViewModel()
    {
        // Constructor logic can go here if needed
        LoadConfiguration();

        Service = new WaldService(ServerUrl, ShopCode, UserPhoneNumber);
    }

    private void LoadConfiguration()
    {
        // Load configuration logic can go here if needed
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        ConfigSetting = config?.Get<ConfigSettings>();
        ServerUrl = ConfigSetting?.ServerUrl;
        ShopCode = ConfigSetting?.ShopCode;
        UserPhoneNumber = ConfigSetting?.UserPhoneNumber;
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
    }

    [RelayCommand]
    private void SaveStampCancel()
    {
        Service.CancelStamp();
    }

    [RelayCommand]
    private void UseCoupon()
    {
        Service.UseCoupon();
    }

    [RelayCommand]
    private void UseCouponCancel()
    {
        Service.CancelCoupon();
    }

    [RelayCommand]
    private void SavePoint()
    {
        Service.SavePoint();
    }

    [RelayCommand]
    private void SavePointCancel()
    {
        Service.CancelPoint();
    }

    [RelayCommand]
    private void UsePoint()
    {
        Service.UsePoint();
    }

    [RelayCommand]
    private void UsePointCancel()
    {
        Service.CancelUsePoint();
    }

    [RelayCommand]
    private void GetUserInfo()
    {
        Service.GetUserData();
    }

    [RelayCommand]
    private void GetShopInfo()
    {
        Service.GetStoreData();
    }

    // enum값을 bool값으로 변경하는 함수를 만들어줘 IValueConverter를 사용해서
    public bool Convert(ShopType value)
    {
        return value == ShopType.Stamp;
    }
}

public class ConfigSettings
{
    public string? ServerUrl { get; set; }
    public string? ShopCode { get; set; }
    public string? UserPhoneNumber { get; set; }
}