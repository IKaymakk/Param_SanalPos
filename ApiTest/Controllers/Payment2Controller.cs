// .NET 8 Core - Param Non-3D Ödeme Prototip

using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class Payment2Controller : ControllerBase
{
    private const string TestUrl = "https://testposws.param.com.tr/turkpos.ws/service_turkpos_prod.asmx";
    private const string ClientCode = "10738"; 
    private const string ClientUsername = "Test";
    private const string ClientPassword = "Test"; 
    private const string GUID = "0c13d406-873b-403b-9c09-a5766840d98c";

    [HttpPost("start-non-3d-payment")]
    public async Task<IActionResult> StartNon3DPayment()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var request = new PaymentRequests
        {
            KKNo = "4446763125813623",
            KKSKAy = "12",
            KKSKYil = "2026",
            KKCVC = "000",
            KK_Sahibi = "Test User",
            SiparisID = "SP999312",
            IslemTutar = "100,50",
            ToplamTutar = "100,50",
            HataURL = "https://test-fail.com",
            BasariliURL = "https://test-success.com",
            Taksit = 1
        };
        string hashData = GenerateHash(request);

        var ipAddress = "127.0.0.1"; 
        var postData = new StringContent(
    $"CLIENT_CODE={ClientCode}&CLIENT_USERNAME={ClientUsername}&CLIENT_PASSWORD={ClientPassword}&GUID={GUID}&KK_Sahibi={request.KK_Sahibi}&KK_No={request.KKNo}&KK_SK_Ay={request.KKSKAy}&KK_SK_Yil={request.KKSKYil}&KK_CVC={request.KKCVC}&Siparis_ID={request.SiparisID}&Islem_Tutar={request.IslemTutar}&Toplam_Tutar={request.ToplamTutar}&Hata_URL={request.HataURL}&Basarili_URL={request.BasariliURL}&Taksit={request.Taksit}&ISLEM_HASH={hashData}&ISLEM_GUVENLIK_TIP=NS&IPAdr={ipAddress}",
    Encoding.UTF8, "application/x-www-form-urlencoded");

        using var client = new HttpClient();
        var response = await client.PostAsync($"{TestUrl}/TP_WMD_UCD", postData);
        var responseData = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return BadRequest($"3D ödeme işlemi başlatılamadı. Yanıt: {responseData}");
        }

        return Content(responseData, "text/html");
    }

    private string GenerateHash(PaymentRequests request)
    {
        string rawData = $"{ClientCode}{GUID}{request.Taksit}{request.IslemTutar}{request.ToplamTutar}{request.SiparisID}{request.HataURL}{request.BasariliURL}{ClientUsername}{ClientPassword}";

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        using var sha1 = SHA1.Create();
        byte[] hashBytes = sha1.ComputeHash(Encoding.GetEncoding("ISO-8859-9").GetBytes(rawData));

        return Convert.ToBase64String(hashBytes);
    }
}

public class PaymentRequests
{
    public string KKNo { get; set; }
    public string KKSKAy { get; set; }
    public string KKSKYil { get; set; }
    public string KKCVC { get; set; }
    public string KK_Sahibi { get; set; }
    public string SiparisID { get; set; }
    public string IslemTutar { get; set; }
    public string ToplamTutar { get; set; }
    public string HataURL { get; set; }
    public string BasariliURL { get; set; }
    public int Taksit { get; set; }
}
