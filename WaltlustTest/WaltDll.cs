using System.Runtime.InteropServices;

namespace WaltlustTest
{
    public class WaltDll
    {
        #region DllImport
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct Product
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 24)]
            public string product_code; // pos에 등록된 상품코드

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
            public string product_name;// pos에 등록된 상품이름
            public int product_price;// 상품가격(상품 수에 곱한 값)
            public int product_cnt;// 상품 수
        };

        public const string PathModule = @"WaldDll.dll";

        [DllImport(PathModule, EntryPoint = "SetTitleURL", CharSet = CharSet.Unicode)]
        public static extern void SetTitleURL(string title);

        [DllImport(PathModule, EntryPoint = "WaldInitialize", CharSet = CharSet.Unicode)]
        public static extern int WaldInitialize(string storeId);

        [DllImport(PathModule, EntryPoint = "SaveStamp", CharSet = CharSet.Unicode)]
        public static extern void SaveStamp(string user_id, string store_id, string order_id, string order_date, int coupon_cnt, Product[] products, int product_cnt, CALLBACKSaveStampResult callback);

        [DllImport(PathModule, EntryPoint = "CancelStamp", CharSet = CharSet.Unicode)]
        public static extern void CancelStamp(string reward_id, CALLBACKCancelStampResult callback);

        [DllImport(PathModule, EntryPoint = "UseCoupon", CharSet = CharSet.Unicode)]
        public static extern void UseCoupon(string store_id, string coupon_id, Product[] products, int product_cnt, CALLBACKUseCouponResult callback);

        [DllImport(PathModule, EntryPoint = "CancelCoupon", CharSet = CharSet.Unicode)]
        public static extern void CancelCoupon(string store_id, string coupon_id, CALLBACKCancelCouponResult callback);

        [DllImport(PathModule, EntryPoint = "GetCouponInfo", CharSet = CharSet.Unicode)]
        public static extern void GetCouponInfo(string store_id, string coupon_id, Product[] products, int product_cnt, CALLBACKGetCouponInfoResult callback);

        [DllImport(PathModule, EntryPoint = "SavePoint", CharSet = CharSet.Unicode)]
        public static extern void SavePoint(string user_id, string store_id, string order_id, string order_date, Product[] products, int product_cnt, CALLBACKSavePointResult callback);

        [DllImport(PathModule, EntryPoint = "CancelPoint", CharSet = CharSet.Unicode)]
        public static extern void CancelPoint(string order_id, string store_id, CALLBACKCancelPointResult callback);

        [DllImport(PathModule, EntryPoint = "UsePoint", CharSet = CharSet.Unicode)]
        public static extern void UsePoint(string user_id, string store_id, int point, CALLBACKUsePointResult callback);

        [DllImport(PathModule, EntryPoint = "CancelUsePoint", CharSet = CharSet.Unicode)]
        public static extern void CancelUsePoint(string use_id, string store_id, CALLBACKCancelUsePointResult callback);

        [DllImport(PathModule, EntryPoint = "GetUserData", CharSet = CharSet.Unicode)]
        public static extern void GetUserData(string user_id, string store_id, CALLBACKGetUserDataResult callback);

        /// <summary>
        /// 스탬프 적립 결과 콜백 함수
        /// </summary>
        /// <param name="result">적립결과값 (정상:true,오류:false)
        /// <param name="result_msg">적립결과 메시지 (정상:“정상”,오류:상황별오류메세지)</param>
        /// <param name="reward_stamp">적립된스탬프수 (오류시에는 -1)</param>
        /// <param name="stamp_count">현재적립된스탬프수 (오류시에는-1)</param>
        /// <param name="coupon_issue">쿠폰발급유무 (true 시에현재적립으로인해쿠폰이발급됨)</param>
        /// <param name="reward_id">적립 결과 아이디값. 취소시에 전달되어야함</param>
        public delegate void CALLBACKSaveStampResult(bool result, [MarshalAs(UnmanagedType.LPWStr)] string result_msg, int reward_stamp, int stamp_count, bool coupon_issue, [MarshalAs(UnmanagedType.LPWStr)] string reward_id);

        /// <summary>
        /// 스탬프 취소 결과 콜백 함수
        /// </summary>
        /// <param name="result">적립 취소 결과값 (정상:true,오류:false)</param>
        /// <param name="result_msg">적립 취소 결과 메시지 (정상:“정상”,오류:상황별오류메세지)</param>
        /// <param name="cancel_stamp">적립 취소된 스탬프 수</param>
        /// <param name="stamp_count">현재 스탬프 수</param>
        public delegate void CALLBACKCancelStampResult(bool result, [MarshalAs(UnmanagedType.LPWStr)] string result_msg, int cancel_stamp, int stamp_count);

        /// <summary>
        /// 쿠폰 사용 결과 콜백 함수
        /// </summary>
        /// <param name="result">쿠폰 사용 결과값 (정상:true,오류:false)</param>
        /// <param name="result_msg">결과 메시지 (정상:“정상”,오류:상황별오류메세지)</param>
        /// <param name="product_code">사용된 쿠폰의 아이디</param>
        /// <param name="coupon_id">사용된 쿠폰의 아이디</param>
        /// <param name="type">[ 0">금액할인] [ 1">퍼센트할인 ] [ -1">에러]</param>
        /// <param name="amount">[ type 이 0 일시">금액액수] [ type 이 1 일시">% 할인값 ] [ -1">에러]</param>
        /// <param name="is_total"></param>
        public delegate void CALLBACKUseCouponResult(bool result, [MarshalAs(UnmanagedType.LPWStr)] string result_msg, [MarshalAs(UnmanagedType.LPWStr)] string product_code, [MarshalAs(UnmanagedType.LPWStr)] string coupon_id, int type, int amount, bool is_total);

        /// <summary>
        /// 쿠폰 취소 결과 콜백 함수
        /// </summary>
        /// <param name="result">쿠폰 사용 취소결과값 (정상:true,오류:false)</param>
        /// <param name="result_msg">결과 메시지 (정상:“정상”,오류:상황별오류메세지)</param>
        public delegate void CALLBACKCancelCouponResult(bool result, [MarshalAs(UnmanagedType.LPWStr)] string result_msg);

        /// <summary>
        /// 쿠폰 정보 조회 결과 콜백 함수
        /// </summary>
        /// <param name="result">쿠폰사용취소결과값 (정상">true, 오류">false)</param>
        /// <param name="result_msg">결과메세지( 정상">“정상”, 오류">상황별오류메세지)</param>
        /// <param name="type">[ 0">금액할인] [ 1">퍼센트할인 ] [ -1">에러]</param>
        /// <param name="amount">[ type 이 0 일시">금액액수] [ type 이 1 일시">% 할인값 ] [ -1">에러]</param>
        /// <param name="item_code"></param>
        /// <param name="is_total"></param>
        public delegate void CALLBACKGetCouponInfoResult(bool result, [MarshalAs(UnmanagedType.LPWStr)] string result_msg, int type, int amount, [MarshalAs(UnmanagedType.LPWStr)] string item_code, bool is_total);

        /// <summary>
        /// 포인트 적립 결과 콜백 함수
        /// </summary>
        /// <param name="result">적립결과값 (정상:true,오류:false)</param>
        /// <param name="result_msg">적립결과 메시지 (정상:“정상”,오류:상황별오류메세지)</param>
        /// <param name="reward_point">적립된포인트 (오류시에는 -1)</param>
        /// <param name="total_point">현재 적립된 총 포인트 (오류시에는 -1)</param>
        /// <param name="reward_id">적립결과아이디값.취소시에전달되어야함</param>
        public delegate void CALLBACKSavePointResult(bool result, [MarshalAs(UnmanagedType.LPWStr)] string result_msg, int reward_point, int total_point, [MarshalAs(UnmanagedType.LPWStr)] string reward_id);

        /// <summary>
        /// 포인트 취소 결과 콜백 함수
        /// </summary>
        /// <param name="result">적립 취소 결과값 (정상:true,오류:false)</param>
        /// <param name="result_msg">적립 취소 결과 메시지 (정상:“정상”,오류:상황별오류메세지)</param>
        /// <param name="cancel_point">적립 취소된 포인트</param>
        /// <param name="total_point">현재 적립된 총 포인트 (오류시에는 -1)</param>
        public delegate void CALLBACKCancelPointResult(bool result, [MarshalAs(UnmanagedType.LPWStr)] string result_msg, int cancel_point, int total_point);

        /// <summary>
        /// 포인트 사용 결과 콜백 함수
        /// </summary>
        /// <param name="result">포인트 사용 결과값 (정상:true, 오류:false)</param>
        /// <param name="result_msg">결과 메시지 (정상:“정상”,오류:상황별오류메세지)</param>
        /// <param name="use_id">사용 결과 아이디값. 취소시에 전달되어야함</param>
        /// <param name="use_point">사용된 포인트</param>
        /// <param name="total_point">현재 적립된 총 포인트(오류시에는 0)</param>
        public delegate void CALLBACKUsePointResult(bool result, [MarshalAs(UnmanagedType.LPWStr)] string result_msg, [MarshalAs(UnmanagedType.LPWStr)] string use_id, int use_point, int total_point);

        /// <summary>
        /// 포인트 사용 취소 결과 콜백 함수
        /// </summary>
        /// <param name="result">쿠폰 사용 취소 결과값 (정상:true,오류:false)</param>
        /// <param name="result_msg">결과 메시지 (정상:“정상”,오류:상황별오류메세지)</param>
        /// <param name="return_point">취소로 인해 되돌아온 포인트</param>
        /// <param name="total_point">현재 적립된 총 포인트 (오류시에는-1)</param>
        public delegate void CALLBACKCancelUsePointResult(bool result, [MarshalAs(UnmanagedType.LPWStr)] string result_msg, int return_point, int total_point);

        /// <summary>
        /// 사용자 정보 조회 결과 콜백 함수
        /// </summary>
        /// <param name="result">사용자 조회 결과값 (정상:true, 오류:false)</param>
        /// <param name="result_msg">결과메세지 ( 정상:“정상”, 오류:상황별오류메세지)</param>
        /// <param name="point">현재 적립된 총 포인트</param>
        /// <param name="stamp">현재 적립된 스탬프 수</param>
        /// <param name="coupons">사용자가 가지고 있는 쿠폰 아이디 리스트
        public delegate void CALLBACKGetUserDataResult(bool result, [MarshalAs(UnmanagedType.LPWStr)] string result_msg, int point, int stamp, [MarshalAs(UnmanagedType.LPWStr)] string coupons);

        public static CALLBACKSaveStampResult? SaveStampResultCallback;
        public static CALLBACKCancelStampResult? CancelStampResultCallback;
        public static CALLBACKUseCouponResult? UseCouponResultCallback;
        public static CALLBACKCancelCouponResult? CancelCouponResultCallback;
        public static CALLBACKGetCouponInfoResult? GetCouponInfoResultCallback;
        public static CALLBACKSavePointResult? SavePointResultCallback;
        public static CALLBACKCancelPointResult? CancelPointResultCallback;
        public static CALLBACKUsePointResult? UsePointResultCallback;
        public static CALLBACKCancelUsePointResult? CancelUsePointResultCallback;
        public static CALLBACKGetUserDataResult? GetUserDataResultCallback;

        #endregion
    }
}
