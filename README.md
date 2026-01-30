# Bartu Bütçe Takibi

Modern ve kullanıcı dostu bir kişisel bütçe takip uygulaması. Gelir ve giderlerinizi kolayca yönetin, kategorilere ayırın ve finansal hedeflerinize ulaşın.

## Özellikler

- **Genel Bakış Dashboard**: Gelir/gider özeti, grafikler ve filtreleme
- **İşlem Yönetimi**: Gelir ve gider kayıtları, tarih bazlı gruplandırma
- **Kategori Yönetimi**: Gelir ve gider kategorilerini ayrı ayrı yönetme
- **Responsive Tasarım**: Mobil uyumlu modern arayüz
- **Güvenli**: ASP.NET Core ile güvenli veri yönetimi

## Teknoloji Stack

- **Backend**: ASP.NET Core 8.0, C#
- **Database**: SQLite (Entity Framework Core)
- **Frontend**: Bootstrap 5, Font Awesome, Chart.js
- **Charts**: Doughnut ve Spline grafikleri

## Kurulum

1. **Gereksinimler**:
   - .NET 8.0 SDK
   - Visual Studio Code veya Visual Studio

2. **Projeyi Klonlayın**:

   ```bash
   git clone <repository-url>
   cd ButceTakip
   ```

3. **Bağımlılıkları Yükleyin**:

   ```bash
   dotnet restore
   ```

4. **Veritabanını Oluşturun**:

   ```bash
   dotnet ef database update
   ```

5. **Uygulamayı Çalıştırın**:

   ```bash
   dotnet run
   ```

6. **Tarayıcıda Açın**:
   - http://localhost:5074

## Executable ile Çalıştırma (Terminal ve VS Code Gerekmeden)

Proje, .NET SDK'sız çalıştırılabilecek bağımsız executable olarak publish edilebilir.

1. **Publish Edin**:

   ```bash
   dotnet publish -c Release -r win-x64 --self-contained true
   ```

2. **Çıktıyı Bulun**:
   - `bin\Release\net10.0\win-x64\publish\`

3. **Executable'yi Çalıştırın**:
   - `ButceTakip.exe` dosyasını çift tıklayarak çalıştırın
   - Uygulama otomatik olarak tarayıcıda açılır

4. **Başka Bilgisayara Taşıma**:
   - Tüm `publish` klasörünü zipleyin ve hedef bilgisayara taşıyın
   - .NET SDK yüklemeye gerek yoktur, tüm bağımlılıklar dahil edilmiştir
   - **Veritabanını Dahil Etmek İçin**: `ButceTakip.db` dosyasını `publish` klasörüne kopyalayın (`cp ButceTakip.db bin/Release/net10.0/win-x64/publish/`)

## Kullanım

### Ana Sayfa

- Hoşgeldin mesajı ve typewriter efekti
- Modern navbar ve sidebar menü

### Genel Bakış

- Aylık gelir/gider özeti
- Kategori bazlı harcama grafikleri
- Tarih ve kategori filtreleme

### İşlem Hareketleri

- Tüm gelir/gider kayıtları
- Ay bazlı gruplandırma
- Filtreleme ve sıralama

### Kategori Yönetimi

- Gelir ve gider kategorilerini ayrı bölümlerde görüntüleme
- Yeni kategori ekleme

## Proje Yapısı

```
ButceTakip/
├── Controllers/          # MVC Controllers
├── Models/              # Entity Models
├── Views/               # Razor Views
├── Data/                # Database Context
├── Migrations/          # EF Migrations
├── wwwroot/             # Static Files (CSS, JS, Images)
└── Program.cs           # Application Entry Point
```

## Katkıda Bulunma

1. Fork edin
2. Feature branch oluşturun (`git checkout -b feature/amazing-feature`)
3. Commit edin (`git commit -m 'Add amazing feature'`)
4. Push edin (`git push origin feature/amazing-feature`)
5. Pull Request açın

## Lisans

Bu proje MIT lisansı altında lisanslanmıştır.

## İletişim

Bartu - bartu@example.com

Proje Linki: [https://github.com/bartu/butce-takip](https://github.com/bartu/butce-takip)
