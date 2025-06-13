💳 Param Sanal Pos Entegrasyonu (Test Uygulaması)
Bu proje, Param Sanal Pos altyapısını kullanarak ödeme işlemlerini test etmek ve temel seviye bir entegrasyon senaryosu uygulamak amacıyla geliştirilmiştir. Gerçek ortam yerine test senaryoları üzerinden ilerlenmiş, sanal kart ile ödeme işlemi simüle edilmiştir.

🎯 Proje Amacı
Amaç, ödeme sistemleri konusunda temel bilgi edinmek ve Param Pos'un sunduğu API’yi anlamaktır. Proje kapsamında:

Param test ortamı üzerinden ödeme isteği gönderme

Başarılı ve başarısız ödeme durumlarını senaryolama

API’ye gelen response verisini anlamlandırma
gibi işlemler gerçekleştirilmiştir.

⚙️ Kullanılan Teknolojiler
Teknoloji	Açıklama
.NET Core 8	API 
HttpClient	Param API’sine istek göndermek için
MSSQL	(Opsiyonel) ödeme geçmişi kaydı vb. için
Postman	Test senaryolarını manuel olarak çalıştırmak için

🧪 Test Senaryoları
✅ Başarılı ödeme (doğru kart bilgileri ile)

❌ Hatalı ödeme (geçersiz kart bilgisi / eksik parametre)

🛡️ API doğrulama hatası (eksik auth / signature vs.)

Testler, Param’ın sağladığı dokümantasyon ve test kart bilgileri ile gerçekleştirilmiştir.

🧠 Katkı Sağladığı Alanlar
Ödeme servislerinin çalışma mantığını anlamak

Dış servislerle güvenli iletişim kurmak (signature, token, hash vb.)

Gerçek hayattaki e-ticaret projelerine hazırlık yapmak
