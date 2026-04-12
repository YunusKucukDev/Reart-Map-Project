# Reart Map Project

ASP.NET Core 8 tabanlı, MongoDB Atlas üzerinde çalışan, API + MVC mimarisiyle geliştirilmiş kurumsal bir harita portföy uygulaması.

---

## Proje Yapısı

```
MapProject/
├── MapProject.Api/        # REST API — JWT kimlik doğrulama, MongoDB
├── MapProject.WebUI/      # MVC Web Arayüzü — Cookie auth, SignalR, e-posta
├── MapProject.DtoLayer/   # Paylaşılan DTO modelleri
├── docker-compose.yml
├── .env                   # Ortam değişkenleri (git'e gönderilmez)
└── .env.example           # .env şablonu
```

---

## Teknoloji Yığını

| Katman | Teknoloji |
|--------|-----------|
| Backend API | ASP.NET Core 8 Web API |
| Web Arayüzü | ASP.NET Core 8 MVC |
| Veritabanı | MongoDB Atlas |
| Kimlik Doğrulama (API) | JWT Bearer Token |
| Kimlik Doğrulama (UI) | Cookie Authentication |
| Kullanıcı Yönetimi | ASP.NET Core Identity (MongoDB store) |
| Gerçek Zamanlı | SignalR |
| E-posta | MailKit (Gmail SMTP) |
| Nesne Haritalama | AutoMapper |
| Container | Docker + Docker Compose |

---

## Özellikler

### Kullanıcı Arayüzü
- Harita kategorileri listeleme (favoriler dahil)
- Kategori detay sayfaları (3'e kadar görsel)
- İletişim formu (e-posta bildirimi ile)
- Hakkımızda / istatistik bölümü (deneyim, proje sayısı vb.)
- Kullanıcı kaydı ve girişi

### Admin Paneli
- Kategori yönetimi (oluştur, düzenle, sil, favori işaretle)
- İletişim mesajlarını görüntüleme
- Kullanıcı bilgileri yönetimi
- Harita kimlik açıklaması yönetimi
- **Gerçek zamanlı aktif ziyaretçi sayacı** (SignalR)

### API Endpoints

| Endpoint | Açıklama |
|----------|----------|
| `POST /api/Accounts/login` | JWT token al |
| `GET  /api/Accounts/getuser` | Oturum bilgilerini getir |
| `GET  /api/Categories` | Tüm kategorileri listele |
| `GET  /api/Categories/favorites` | Favori kategorileri listele |
| `POST /api/Categories` | Yeni kategori oluştur (form-data + resimler) |
| `PUT  /api/Categories` | Kategori güncelle |
| `DELETE /api/Categories/{id}` | Kategori sil |
| `GET  /api/Contacts` | Tüm mesajları listele |
| `POST /api/Contacts` | Yeni mesaj gönder |
| `GET  /api/UserInformations` | Kullanıcı bilgilerini getir |
| `GET  /api/MapIdentityDescriptions` | Kimlik açıklamalarını getir |
| `GET  /api/VisitorLogs` | Ziyaretçi kayıtları |

---

## Kurulum ve Çalıştırma

### Gereksinimler
- [Docker](https://www.docker.com/get-started) ve Docker Compose

### 1. Ortam Değişkenlerini Hazırla

```bash
cp .env.example .env
```

`.env` dosyasını açıp gerçek değerleri gir:

```env
DatabaseSettings__ConnectionString=mongodb+srv://<user>:<password>@<cluster>.mongodb.net/
DatabaseSettings__DatabaseName=Project
JWTSecurity__SecretKey=<en_az_32_karakter_guclu_anahtar>
ApiSettings__BaseUrl=http://api:8080
EmailSettings__SenderEmail=<email@gmail.com>
EmailSettings__Password=<gmail_uygulama_sifresi>
EmailSettings__ReceiverEmail=<alici@gmail.com>
```

### 2. Docker ile Başlat

```bash
docker compose up -d --build
```

| Servis | Adres |
|--------|-------|
| Web Arayüzü | http://localhost:80 |
| API | http://localhost:5000 |
| Swagger | http://localhost:5000/swagger |

### 3. Durdur

```bash
docker compose down
```

---

## Lokal Geliştirme (.NET CLI)

```bash
# API'yi başlat (ayrı terminalde)
cd MapProject.Api
dotnet run

# WebUI'yi başlat (ayrı terminalde)
cd MapProject.WebUI
dotnet run
```

Lokal geliştirmede `appsettings.Development.json` dosyasına bağlantı bilgilerini ekle ya da `dotnet user-secrets` kullan.

---

## Ortam Değişkenleri Referansı

ASP.NET Core, `__` (çift alt çizgi) ile iç içe JSON anahtarlarını okur. Tüm değişkenler `.env` dosyasında tanımlanır ve Docker Compose tarafından container'lara aktarılır.

| Değişken | Açıklama |
|----------|----------|
| `DatabaseSettings__ConnectionString` | MongoDB Atlas bağlantı dizesi |
| `DatabaseSettings__DatabaseName` | Veritabanı adı |
| `JWTSecurity__SecretKey` | JWT imzalama anahtarı (min. 32 karakter) |
| `ApiSettings__BaseUrl` | WebUI'nin API'ye erişim adresi |
| `EmailSettings__Host` | SMTP sunucu adresi |
| `EmailSettings__Port` | SMTP port (Gmail: 465) |
| `EmailSettings__UseSsl` | SSL kullan (true/false) |
| `EmailSettings__SenderEmail` | Gönderen e-posta adresi |
| `EmailSettings__Password` | Gmail uygulama şifresi |
| `EmailSettings__ReceiverEmail` | Alıcı e-posta adresi |

---

## Güvenlik Notları

- `.env` dosyası `.gitignore`'a eklenmiştir — **asla git'e gönderme**
- Gmail için normal şifre değil, [Uygulama Şifresi](https://support.google.com/accounts/answer/185833) kullanılmalıdır
- Production ortamında JWT anahtarı en az 32 karakter olmalı ve rastgele üretilmelidir
- Görseller API tarafından WebUI'nin `wwwroot/images/` klasörüne kaydedilir; production'da bir volume mount veya nesne deposu (S3, Azure Blob vb.) kullanılması önerilir

---

## Lisans

Bu proje özel kullanım içindir.
