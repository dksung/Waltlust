using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WaltlustTest;

public enum ShopType
{
    Stamp,
    Point
}

public partial class WaldService : ObservableObject
{
    string ServerUrl { get; set; }
    string ShopCode { get; set; }
    string UserPhoneNumber { get; set; }

    public WaltServiceInfo ServiceInfo { get; set; }

    [ObservableProperty]
    public ShopType _Type;


    public string console_log = string.Empty;
    private string userinfo_log = string.Empty;
    public string save_admit_code { get; set; } = string.Empty;
    public int max_use_point { get; set; } = -1;
    public int current_stamp_count { get; set; } = -1;

    public Queue<string> save_stamp_reward_id_queue = new Queue<string>();
    public Queue<string> save_point_reward_id_queue = new Queue<string>();
    public Queue<string> use_point_id_queue = new Queue<string>();
    public Queue<string> use_coupon_id_queue = new Queue<string>();

    [ObservableProperty]
    public Queue<string> _exist_coupon_queue = new Queue<string>();

    public WaldService(string serverUrl, string shopCode, string userPhoneNumber)
    {
        ServerUrl = serverUrl;
        ShopCode = shopCode;
        UserPhoneNumber = userPhoneNumber;
        
        Initialize();
    }

    private void Initialize()
    {
        try
        {
            WaltDll.SetTitleURL(ServerUrl);
            WaltDll.SaveStampResultCallback = SaveStampResult;
            WaltDll.CancelStampResultCallback = CancelStampResult;
            WaltDll.UseCouponResultCallback = UseCouponResult;
            WaltDll.CancelCouponResultCallback = CancelCouponResult;
            WaltDll.GetCouponInfoResultCallback = GetCouponInfoResult;
            WaltDll.SavePointResultCallback = SavePointResult;
            WaltDll.CancelPointResultCallback = CancelPointResult;
            WaltDll.UsePointResultCallback = UsePointResult;
            WaltDll.CancelUsePointResultCallback = CancelUsePointResult;
            WaltDll.GetUserDataResultCallback = GetUserDataResult;

            ServiceInfo = new WaltServiceInfo();
        }
        catch (Exception ex)
        {
        }

        GetStoreData();
        GetUserData();
    }

    public void GetStoreData()
    {
        int result = WaltDll.WaldInitialize(ShopCode);
        Type = (ShopType)result;

        console_log = result switch
        {
            0 => "스탬프 매장",
            1 => "포인트 매장",
            2 => "스탬프, 포인트 같이 적용한 매장",
            -1 => "에러, (상점 아이디가 잘못된 경우)",
            _ => "알수없음"
        };

        console_log = $"[발트] 매장 조회 응답 : {console_log}";
    }

    public void GetUserData()
    {
        WaltDll.GetUserData(UserPhoneNumber, ShopCode, GetUserDataResult);
    }

    public void SaveStamp()
    {
        List<WaltDll.Product> products =
        [
            new() { product_code = "000001", product_name = "테스트두줄줄바꿈테스트1", product_price = 5400, product_cnt = 1},
            new() { product_code = "280004", product_name = "코코넛쿠키", product_price = 3000, product_cnt = 2},
        ];

        string order_id = $"{Environment.TickCount}";
        string order_date = $"{DateTime.Now:yyyyMMddHHmmss}";

        WaltDll.SaveStamp(UserPhoneNumber, ShopCode, order_id, order_date, 0, products.ToArray(), products.Count, SaveStampResult);
    }

    public void CancelStamp()
    {
        if (save_stamp_reward_id_queue.Count == 0)
        {
            console_log = "적립된 스탬프가 없습니다.";
            return;
        }

        WaltDll.CancelStamp(save_stamp_reward_id_queue.Peek(), CancelStampResult);
    }

    public void UseCoupon(string couponCode)
    {
        if (Exist_coupon_queue.Count == 0)
        {
            console_log = "사용 가능한 쿠폰이 없습니다.";
            return;
        }

        List<WaltDll.Product> products =
        [
            new () { product_code = "000001", product_name = "테스트두줄줄바꿈테스트1", product_price = 5400, product_cnt = 1},
            new () { product_code = "280004", product_name = "코코넛쿠키", product_price = 3000, product_cnt = 2},
        ];

        WaltDll.UseCoupon(ShopCode, Exist_coupon_queue.Peek(), products.ToArray(), products.Count, UseCouponResult);
    }

    public void CancelCoupon()
    {
        if (use_coupon_id_queue.Count == 0)
        {
            console_log = "사용된 쿠폰이 없습니다.";
            return;
        }
        WaltDll.CancelCoupon(ShopCode, use_coupon_id_queue.Peek(), CancelCouponResult);
    }
    public void GetCouponInfo(string couponCode)
    {
        if (Exist_coupon_queue.Count == 0)
        {
            console_log = "조회 가능한 쿠폰이 없습니다.";
            return;
        }


        List<WaltDll.Product> products =
        [
            new () { product_code = "000001", product_name = "테스트두줄줄바꿈테스트1", product_price = 5400, product_cnt = 2},
            new () { product_code = "280004", product_name = "코코넛쿠키", product_price = 3000, product_cnt = 2},
        ];

        WaltDll.GetCouponInfo(ShopCode, couponCode, products.ToArray(), products.Count, GetCouponInfoResult);
    }

    public void SavePoint()
    {
        List<WaltDll.Product> products =
        [
            new () { product_code = "000001", product_name = "테스트두줄줄바꿈테스트1", product_price = 5400, product_cnt = 1},
            new () { product_code = "280004", product_name = "코코넛쿠키", product_price = 3000, product_cnt = 2},
        ];
        
        string order_id = $"{Environment.TickCount}";
        string order_date = $"{DateTime.Now.ToString("yyyyMMddHHmmss")}";

        WaltDll.SavePoint(UserPhoneNumber, ShopCode, order_id, order_date, products.ToArray(), products.Count, SavePointResult);
    }
    public void CancelPoint()
    {
        if (save_point_reward_id_queue.Count == 0)
        {
            console_log = "적립된 포인트가 없습니다.";
            return;
        }

        WaltDll.CancelPoint(save_point_reward_id_queue.Peek(), ShopCode, CancelPointResult);
    }
    public void UsePoint()
    {
        if (max_use_point < 100)
        {
            console_log = "사용가능한 포인트가 없습니다.";
            return;
        }
        Random rnd = new Random();
        var use_point = rnd.Next(1, max_use_point / 100) * 100;
        WaltDll.UsePoint(UserPhoneNumber, ShopCode, use_point, UsePointResult);
    }

    public void CancelUsePoint()
    {
        if (use_point_id_queue.Count == 0)
        {
            console_log = "사용된 포인트가 없습니다.";
            return;
        }
        WaltDll.CancelUsePoint(use_point_id_queue.Peek(), ShopCode, CancelUsePointResult);
    }


    public void SaveStampResult(bool result, string result_msg, int reward_stamp, int stamp_count, bool coupon_issue, string reward_id)
    {
        console_log = $"[발트] 스탬프 적립 응답 : {result}, {result_msg}, reward_stamp : {reward_stamp}, stamp_count : {stamp_count}, coupon_issue : {coupon_issue}, reward_id : {reward_id}";

        if (result == true)
        {
            save_stamp_reward_id_queue.Enqueue(reward_id);
        }
    }

    public void CancelStampResult(bool result, string result_msg, int cancel_stamp, int stamp_count)
    {
        console_log = $"[발트] 스탬프 적립취소 응답 : {result}, {result_msg}, cancel_stamp: {cancel_stamp}, stamp_count : {stamp_count}";
        if (result == true)
        {
            save_stamp_reward_id_queue.Dequeue();
        }
    }

    public void UseCouponResult(bool result, string result_msg, string product_code, string coupon_id, int type, int amount, bool is_total)
    {
        console_log = $"[발트] 쿠폰 사용 응답 : {result}, {result_msg}, product_code : {product_code}, coupon_id : {coupon_id}, type : {type}, amount : {amount}, is_total : {is_total}";

        if (result == true)
        {
            var id = Exist_coupon_queue.Dequeue();
            use_coupon_id_queue.Enqueue(coupon_id);
        }
    }

    public void CancelCouponResult(bool result, string result_msg)
    {
        console_log = $"[발트] 쿠폰 사용 취소 응답 : {result}, {result_msg}";
        if (result == true)
        {
            var coupon = use_coupon_id_queue.Dequeue();
            Exist_coupon_queue.Enqueue(coupon);
        }
    }

    public void GetCouponInfoResult(bool result, string result_msg, int type, int amount, string item_code, bool is_total)
    {
        console_log = $"[발트] 쿠폰 정보 응답 : {result}, {result_msg}, type : {type}, amount : {amount}, item_code : {item_code}, is_total : {is_total}";
    }

    public void SavePointResult(bool result, string result_msg, int reward_point, int total_point, string reward_id)
    {
        console_log = $"[발트] 포인트 적립 응답 : {result}, {result_msg}, reward_point : {reward_point}, total_point : {total_point}, reward_id : {reward_id}";
        if (result == true)
        {
            save_point_reward_id_queue.Enqueue(reward_id);
            max_use_point = total_point;
        }
    }

    public void CancelPointResult(bool result, string result_msg, int cancel_point, int total_point)
    {
        console_log = $"[발트] 포인트 적립 취소 응답 : {result}, {result_msg}, cancel_point : {cancel_point}, total_point : {total_point}";
        if (result == true)
        {
            save_point_reward_id_queue.Dequeue();
            max_use_point = total_point;
        }
    }

    public void UsePointResult(bool result, string result_msg, string use_id, int use_point, int total_point)
    {
        console_log = $"[발트] 포인트 사용 응답 : {result}, {result_msg}, use_id : {use_id}, use_point : {use_point}, total_point : {total_point}";

        if (result == true)
        {
            use_point_id_queue.Enqueue(use_id);
            max_use_point = total_point;
        }
    }

    public void CancelUsePointResult(bool result, string result_msg, int return_point, int total_point)
    {
        console_log = $"[발트] 포인트 사용 취소 응답 : {result}, {result_msg}, return_point : {return_point}, total_point : {total_point}";

        if (result == true)
        {
            use_point_id_queue.Dequeue();
            max_use_point = total_point;
        }
    }
    public void GetUserDataResult(bool result, string result_msg, int point, int stamp, string coupons)
    {
        console_log = $"[발트] 고객 조회 응답 : {result}, {result_msg}, point : {point}, stamp : {stamp}, coupons : {coupons}";

        if (result == true)
        {
            if (string.IsNullOrEmpty(coupons) == false)
            {
                Exist_coupon_queue.Clear();
                coupons.Split(",").ToList().ForEach(Exist_coupon_queue.Enqueue);
            }
            max_use_point = point;
            current_stamp_count = stamp;
        }

        userinfo_log = $"GetUserDataResult: {result}, {result_msg}, point : {point}, stamp : {stamp}, coupons {Exist_coupon_queue.Count}개 : {coupons}";
    }
}

public class WaltServiceInfo
{
    bool result { get; set; }
    string message { get; set; }
    int change_count { get; set; }
    int reward_count { get; set; }
    string current_reward_id { get; set; }
}
