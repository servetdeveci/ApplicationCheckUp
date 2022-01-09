var loadingHtml = "<div class='col-xs-12 d-block text-center' ><img class='loaderImg' src='/img/loading/loading.svg' /> <h3 class='loaderText'>Yükleniyor...</h3></div>";
//Tüm datatableların gösterilecek satır sayısının ortak olarak belirlendiği dizidir
var lengthArray = [[5, 10, 15, 20, 25, 50, 75, 100, 250, 500, 1000], [5, 10, 15, 20, 25, 50, 75, 100, 250, 500, 1000]];

// bir çok yerde kullanılıyor. ('data-url' olan herhangi bir elemente tıklandığında asenkron olarak sayfayı 'istenilenYere' yükler)
function LoadPage(url, loadAreaSelector, ajax, pageTitle = "Atlas Sayaç", changeUrl = false, _async = true, type = "GET") {
    var content = $(loadAreaSelector);
    content.html("");
    //content.append("<div class='col-sm-12'><img class='loaderImg img-responsive' src='/img/loading-layout.png' /></div>");

    content.append(loadingHtml);
    $.ajax({
        type: type,
        async: _async,
        url: url,
        data: { "ajax": ajax },
        success: function (res) {
            //console.log(res);
            content.html(res);
            //content.show();
            //console.log("Yüklenen --> " + url);
            if (window.outerWidth < 993) {
                $(".inbox-sidebar").addClass("hidden-xs hidden-sm");
            }
            if (changeUrl) {
                document.title = pageTitle;
                window.history.pushState({ "url": url, "pageTitle": pageTitle }, "", url);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            $.toast({
                heading: 'Yüklenemedi. LoadPage()',
                text: url + " " + xhr.status,
                position: 'top-right',
                loaderBg: '#ff6849',
                icon: 'warning',
                hideAfter: 3500,
                stack: 6
            });
        },
    });
}

// javascript ile kullanılan bilgisayarın zamanına göre alır
function SetTime(time, culture) {
    if (culture == null) { culture = "tr-TR"; }
    var arrLang = {
        "tr-TR": {
            "sn": " sn ",
            "sa": " sa ",
            "dk": " dk ",
            "once": "önce"
        },
        "en-US": {
            "sn": " sec ",
            "sa": " hour ",
            "dk": " min ",
            "once": "ago"
        }
    };
    if (time == "0001-01-01T00:00:00") {
        return "Bekleniyor";
    }
    var updateTime = moment(time);
    var current = moment().add(2, 'seconds');
    var duration = (current.diff(updateTime, 'seconds') + 1) + arrLang[culture].sn + arrLang[culture].once;
    if (current.diff(updateTime, 'hours') >= 1) {
        if (current.diff(updateTime, 'hours') >= 24) //24 saatden sonra tarih ve saat göstermesini sağlıyor
        {
            var a = new Date(time);
            return a.toLocaleDateString() + " " + a.toLocaleTimeString('tr-TR');
        }
        duration = current.diff(updateTime, 'hours') + arrLang[culture].sa + (current.diff(updateTime, 'minutes')) % 60 + arrLang[culture].dk;
    }
    else if (current.diff(updateTime, 'minutes') >= 1) {
        duration = current.diff(updateTime, 'minutes') + arrLang[culture].dk + (current.diff(updateTime, 'seconds') + 1) % 60 + arrLang[culture].sn;
    }
    return duration;
}
// sunucucudan dönen saate göre işlem yapılmalıdır. örnek olarak datatable da systemdatetime olarak gelen veri. bu kullanılacak ise current time in sunucudan gelmesi gerekmektedir. 
function SetWithServerDateTime(currentTime, _updatedTime, culture) {
    if (culture == null) { culture = "tr-TR"; }
    var arrLang = {
        "tr-TR": {
            "sn": " sn ",
            "sa": " sa ",
            "dk": " dk ",
            "once": "önce"
        },
        "en-US": {
            "sn": " sec ",
            "sa": " hour ",
            "dk": " min ",
            "once": "ago"
        }
    };
    if (_updatedTime == "0001-01-01T00:00:00") {
        return "-";//"Bekleniyor";
    }
    var updateTime = moment(_updatedTime);
    var current = moment(currentTime);
    var snDiff = current.diff(updateTime, 'seconds') + 1;
    if (snDiff <= 0)
        snDiff = 1;
    var duration = snDiff + arrLang[culture].sn + arrLang[culture].once;
    if (current.diff(updateTime, 'hours') >= 1) {
        if (current.diff(updateTime, 'hours') >= 24) //24 saatden sonra tarih ve saat göstermesini sağlıyor
        {
            var a = new Date(_updatedTime);
            return a.toLocaleDateString() + " " + a.toLocaleTimeString('tr-TR');
        }

        duration = current.diff(updateTime, 'hours') + arrLang[culture].sa + (current.diff(updateTime, 'minutes')) % 60 + arrLang[culture].dk;
    }
    else if (current.diff(updateTime, 'minutes') >= 1) {
        if ((current.diff(updateTime, 'seconds') + 1) % 60 == 0) {
            duration = Number(Number(current.diff(updateTime, 'minutes')) + Number(1)) + arrLang[culture].dk;
        } else {
            duration = current.diff(updateTime, 'minutes') + arrLang[culture].dk + (current.diff(updateTime, 'seconds') + 1) % 60 + arrLang[culture].sn;
        }
    }
    return duration;
}

function GetTime() {
    var date = new Date();
    return date.getHours() + ":" + date.getMinutes() + " " + date.getDate() + '.' + (date.getMonth() + 1) + '.' + date.getFullYear();
}

function removeItemOnce(arr, value) {
    var index = arr.indexOf(value);
    if (index > -1) {
        arr.splice(index, 1);
    }
    return arr;
}

function removeItemAll(arr, value) {
    var i = 0;
    while (i < arr.length) {
        if (arr[i] === value) {
            arr.splice(i, 1);
        } else {
            ++i;
        }
    }
    return arr;
}

function delay(callback, ms) {
    var timer = 0;
    return function () {
        var context = this, args = arguments;
        clearTimeout(timer);
        timer = setTimeout(function () {
            callback.apply(context, args);
        }, ms || 0);
    };
}