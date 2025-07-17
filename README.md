> **Uyarı:** Bu proje geliştirme aşamasındadır. Kod ve işlevlerde hata, eksiklik veya değişiklikler olabilir. Üretim ortamında kullanmadan önce kapsamlı testler yapmanız önerilir.

# SD_Sinema

## Teknik Bilgiler ve Mimari

SD_Sinema, .NET 9 ile geliştirilmiş, katmanlı mimariye sahip bir sinema otomasyon sistemidir. Proje, SOLID prensiplerine ve Clean Architecture yaklaşımına uygun olarak tasarlanmıştır.

- **Katmanlar:**
  - **Core:** Temel entity ve interface tanımları.
  - **Data:** Entity Framework Core ile repository ve context yapısı, SQL Server veritabanı yönetimi.
  - **Business:** Servisler, iş kuralları, DTO yapıları ve iş mantığı.
  - **API:** ASP.NET Core Web API ile RESTful servisler, tüm CRUD işlemleri ve dışa açık uç noktalar.
  - **Web:** ASP.NET Core MVC tabanlı web arayüzü, yalnızca API ile haberleşir.

- **Kullanılan Teknolojiler:**
  - .NET 9 SDK
  - ASP.NET Core MVC & Web API
  - Entity Framework Core 
  - SQL Server
  - Bootstrap & jQuery 
  - Newtonsoft.Json 

- **Temel Özellikler:**
  - Film, salon, seans, koltuk, bilet tipi, kullanıcı ve rezervasyon yönetimi
  - Tüm CRUD işlemleri (oluşturma, listeleme, güncelleme, silme)
  - Katmanlı mimari ve SOLID prensiplerine uygun yapı
  - API ile tam uyumlu DTO ve ViewModel dönüşümleri
  - Repository ve servislerde performans optimizasyonu (Include, filtreleme)
  - Kullanıcı dostu arayüz, dropdown listeler ve validasyonlar
  - Koltuk seçimi, tarih validasyonu ve rezervasyon işlemleri
  - Null referans ve model uyumsuzluklarına karşı güvenli kod
  - Migration ile veritabanı şeması yönetimi
  - Hem istemci hem sunucu tarafında validasyon ve hata yönetimi

- **Geliştirme Durumu:**
  - Proje halen geliştirme aşamasındadır; kodda hata ve eksiklikler olabilir.
---

## Kurulum ve Kullanım

Aşağıdaki adımları izleyerek projeyi kendi bilgisayarınızda çalıştırabilirsiniz:

### 1. Gereksinimler
- .NET 9 SDK
- SQL Server 

### 2. Projeyi İndirme
- GitHub üzerinden repoyu indirin veya klonlayın:
  ```bash
  git clone https://github.com/SametDulger/SD_Sinema.git
  cd SD_Sinema
  ```

### 3. Veritabanı Ayarları
- `SD_Sinema.Data/appsettings.json` ve `SD_Sinema.API/appsettings.json` dosyalarında kendi SQL Server bağlantı bilginizi girin:
  ```json
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=SD_Sinema;Trusted_Connection=True;"
  }
  ```
- Migration işlemleri hazırdır. Gerekirse veritabanını güncelleyin:
  ```bash
  dotnet ef database update --project SD_Sinema.Data
  ```

### 4. NuGet Paketlerini Yükleme
- Proje kök dizininde:
  ```bash
  dotnet restore
  ```

### 5. Projeyi Derleme
- Proje kök dizininde:
  ```bash
  dotnet build
  ```

### 6. API ve Web Katmanını Başlatma
- Önce API katmanını başlatın:
  ```bash
  dotnet run --project SD_Sinema.API
  ```
- Ardından Web katmanını başlatın (yeni bir terminalde):
  ```bash
  dotnet run --project SD_Sinema.Web
  ```
- Web arayüzü, API ile iletişim kurarak çalışır. API ve Web için portlar `launchSettings.json` veya terminalde belirtilen portlar üzerinden ayarlanabilir.

### 7. İlk Giriş ve Kullanım
- Web arayüzüne tarayıcıdan erişin (ör: http://localhost:5001).
- Film, salon, seans, koltuk, bilet tipi, kullanıcı ve rezervasyon işlemlerini menülerden gerçekleştirebilirsiniz.
- Rezervasyon eklerken koltuk seçimi, tarih validasyonu ve tüm dropdown listeler eksiksiz çalışır.
- Tüm işlemler API üzerinden gerçekleşir, doğrudan veri erişimi yoktur.

### 8. Sık Karşılaşılan Sorunlar
- **Bağlantı Hatası:** API ve Web katmanlarının portlarının uyumlu olduğundan ve connection string'inizin doğru olduğundan emin olun.
- **Migration Hatası:** Migration işlemi için Entity Framework CLI'nın yüklü olması gerekir (`dotnet tool install --global dotnet-ef`).
- **Eksik Paketler:** `dotnet restore` komutunu çalıştırın.
