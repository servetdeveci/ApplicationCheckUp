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


### Docker ile Çalıştırılması
Docker Desktop uygulası kurularak docker-compose.yml dosyası ile tüm proje çalıştırılabilir. Projelerin docker build dosyaları ayrıca ana dizinde bulunmaktadır. 

```diff
+ $ docker-compose build
+ $ docker-compose up

```

### Visual Studio ile Çalıştırılması
Mevcut veritabanı sunucusuna DbContext migrate edilerek ve ConnectionString de gerekli değişiklikler yapılarak Visual Studio üzerinden çoklu proje çalıştırma seçeneği ile proje çalıştırılabilir.

```diff
- Dikkat! Docker veya Visual studio ile çalıştırışırken connectionString'lere dikkat edilmelidir.
+ docker için 'database' diğer host'lar için ip veya domain adı girilmelidir
```

### Uygulama genel mantığını anlatan  ekran görüntüleri 

Uygulamalar
![image](https://user-images.githubusercontent.com/62391718/148823000-2c3ce110-22dd-43d6-8215-08b3f47abd60.png)

Bildirim kaydı eklemesi
![image](https://user-images.githubusercontent.com/62391718/148823215-085c78ea-319a-4343-9faa-30fb2ea7b2cc.png)
 
 Uygulama Detayında kişi ve gönderilen bildirimler
 ![image](https://user-images.githubusercontent.com/62391718/148844229-13d717ee-a572-4124-9978-622ee4fe35a9.png)


 Worker Servisin arka planda çalışması
 ![image](https://user-images.githubusercontent.com/62391718/148832286-2262b6dd-e375-4d14-9e44-d1eed8684b53.png)



 Gönderilen bildirimler
 ![image](https://user-images.githubusercontent.com/62391718/148831383-1ac692b0-3da4-45da-a559-7eeb403435ae.png)

Gelen Email'ler
![image](https://user-images.githubusercontent.com/62391718/148832011-cf712566-d047-4e71-9e62-e52ea43d1fe5.png)


