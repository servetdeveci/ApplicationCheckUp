# Proje Yapısı ve Açıklaması

## ****** Shared *******
Repository ve uow yapısı generic olarak yazılmıstır. Temel olarak buraya eklenen her projenin Domain ve Infrastructure katmanları bu yapıdan türeyecektir.
### Shared.Domain
Bu katman ile implement edilecek soyut yapılar tanımlanacaktır

### Shared.Infrastructure
Bu katmanda genel olarak DataAccess kodlarının ve bunların uygulamaları bulunacaktır. Domain'den uygulanan yapılar burada implement edilecektir.

### ApplicationHealth.MvcWebUI
Uygulamaların eklendiği canlı olarak takip edildiği yapıdır. uygulamalar eklenir liste halinde görüntülenir. ayrıca her bir uygulama içinde birden fazla bildirim kişisi (Contact) eklenebilir. 
Gönderilen bildirimlerin tümü bildirimler menusünden görüntülenebilir. 

### ApplicationHealth.WorkerService
Uygulamların ayakta olup olmadığını kontrol eden timeout sonunda bildirim gönderen uygualamadır. Bildirim sadece email ile olmaktadır. Ancak sms olması içinde alt yapı mevcuttur. 

## Çalıştırılması
### Docker ile Çalıştırılması
Docker Desktop uygulası kurularak docker-compose.yml dosyası ile tüm proje çalıştırılabilir. Projelerin docker build dosyaları ayrıca ana dizinde bulunmaktadır. 

### Visual Studio ile Çalıştırılması
Mevcut veritabanı sunucusuna DbContext migrate edilerek ve ConnectionString de gerekli değişiklikler yapılarak Visual Studio üzerinden çoklu proje çalıştırma seçeneği ile proje çalıştırılabilir.




