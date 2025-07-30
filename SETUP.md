# SD Sinema - Kurulum Rehberi

## Gereksinimler

- .NET 9.0 SDK
- SQL Server (Express, LocalDB veya Full)
- Visual Studio 2022 veya VS Code

## Kurulum Adımları

### 1. Projeyi Klonlayın
```bash
git clone https://github.com/SametDulger/SD_Sinema.git
cd SD_Sinema
```

### 2. Veritabanı Bağlantısını Yapılandırın

#### API Projesi için:
1. `SD_Sinema.API/appsettings.example.json` dosyasını `appsettings.json` olarak kopyalayın
2. Connection string'i kendi SQL Server'ınıza göre güncelleyin:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=SD_Sinema;Trusted_Connection=true;TrustServerCertificate=true;MultipleActiveResultSets=true"
  }
}
```

#### Web Projesi için:
1. `SD_Sinema.Web/appsettings.example.json` dosyasını `appsettings.json` olarak kopyalayın
2. API Base URL'ini güncelleyin:

```json
{
  "ApiBaseUrl": "http://localhost:5000/"
}
```

### 3. Veritabanını Oluşturun

```bash
# API projesi dizininde
cd SD_Sinema.API
dotnet ef database update
```

### 4. Projeyi Çalıştırın

#### API Projesi:
```bash
cd SD_Sinema.API
dotnet run
```

#### Web Projesi:
```bash
cd SD_Sinema.Web
dotnet run
```

### 5. Testleri Çalıştırın

```bash
cd SD_Sinema.Tests
dotnet test
```


## Geliştirme Ortamı

- API: http://localhost:5000
- Web: http://localhost:5001
- Swagger UI: http://localhost:5000/swagger

## Test Coverage

Proje 126 test içerir:
- Unit Tests: Service ve Repository katmanları
- Integration Tests: API endpoint'leri
- Controller Tests: Mock dependencies ile

Tüm testlerin geçmesi için gerekli konfigürasyonları yapmayı unutmayın. 