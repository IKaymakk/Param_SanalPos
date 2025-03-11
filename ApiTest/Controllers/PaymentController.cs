// .NET 8 Core - Param 3D Ödeme Basit Prototip

using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Encodings.Web;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private const string TestUrl = "https://test-dmz.param.com.tr/turkpos.ws/service_turkpos_test.asmx";
    private const string ClientCode = "10738"; // Test verisi
    private const string ClientUsername = "Test"; // Test verisi
    private const string ClientPassword = "Test"; // Test verisi
    private const string GUID = "0c13d406-873b-403b-9c09-a5766840d98c"; // Test verisi

    public PaymentController()
    {
        // Register the encoding provider for Windows-1254
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    [HttpPost("start-3d-payment")]
    public async Task<IActionResult> Start3DPayment()
    {
        var request = new PaymentRequest
        {
            KKNo = "4446763125813623",
            KKSKAy = "12",
            KKSKYil = "26",
            KKCVC = "000",
            KK_Sahibi = "Test User",
            SiparisID = "SP123456",
            IslemTutar = "100,50",
            ToplamTutar = "100,50",
            HataURL = "https://test-fail.com",
            BasariliURL = "https://test-success.com",
            Taksit = 1
        };
        string hashData = GenerateHash(request);

        var postData = new StringContent(
            $"CLIENT_CODE={ClientCode}&GUID={GUID}&KK_Sahibi={request.KK_Sahibi}&KK_No={request.KKNo}&KK_SK_Ay={request.KKSKAy}&KK_SK_Yil={request.KKSKYil}&KK_CVC={request.KKCVC}&Siparis_ID={request.SiparisID}&Islem_Tutar={request.IslemTutar}&Toplam_Tutar={request.ToplamTutar}&Hata_URL={request.HataURL}&Basarili_URL={request.BasariliURL}&Taksit={request.Taksit}&ISLEM_HASH={hashData}",
            Encoding.UTF8, "application/x-www-form-urlencoded");

        using var client = new HttpClient();
        var response = await client.PostAsync($"{TestUrl}/SHA2B64", postData);
        var responseData = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return BadRequest($"3D ödeme işlemi başlatılamadı. Yanıt: {responseData}");
        }

        return Content(responseData, "text/html");
    }

   


    private string GenerateHash(PaymentRequest request)
    {
        string rawData = $"{ClientCode}{GUID}{request.Taksit}{request.IslemTutar}{request.ToplamTutar}{request.SiparisID}{request.HataURL}{request.BasariliURL}";
        using var sha256 = SHA256.Create();
        byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
        return Convert.ToBase64String(hashBytes);
    }

    private string ParseRedirectUrl(string responseData)
    {
        return "https://bank-3d-secure-page.com";
    }
}

public class PaymentRequest
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


#region
//   private string ParseHtmlContent(string responseData)
//{
//    // XML veya HTML içeriğinde "UCD_HTML" değerini çıkarma
//    var startTag = "<UCD_HTML>";
//    var endTag = "</UCD_HTML>";
//    var startIndex = responseData.IndexOf(startTag) + startTag.Length;
//    var endIndex = responseData.IndexOf(endTag);

//    if (startIndex > 0 && endIndex > startIndex)
//    {
//        return responseData.Substring(startIndex, endIndex - startIndex);
//    }

//    return "<p>3D doğrulama ekranı alınamadı.</p>";
//}

//private string ParseHtmlContent(string responseData)
//{
//    // XML veya HTML içeriğinde "UCD_HTML" değerini çıkarma
//    var startTag = "<UCD_HTML>";
//    var endTag = "</UCD_HTML>";
//    var startIndex = responseData.IndexOf(startTag) + startTag.Length;
//    var endIndex = responseData.IndexOf(endTag);

//    if (startIndex > 0 && endIndex > startIndex)
//    {
//        return responseData.Substring(startIndex, endIndex - startIndex);
//    }

//    return "<p>3D doğrulama ekranı alınamadı.</p>";
//}


#endregion