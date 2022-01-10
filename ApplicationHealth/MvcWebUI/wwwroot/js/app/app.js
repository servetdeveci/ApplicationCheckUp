
var table;
var systemDateTime;

$(document).ready(function () {
    InitializeDataTable();
});

///////////////////// CRUD /////////////////////

function LoadInsertAppPartial() {
    LoadPage("/Application/_InsertApp", "#dynamicContent");
}
function AddApp() {
    var model = {
        Name: $("#name").val(),
        Url: $("#url").val(),
        Interval: $("#interval").val()
    }
    $.ajax({
        type: "post",
        url: "/Application/InsertApp",
        data: { app: model },
        success: function (res) {
            table.ajax.reload(null, false);
            if (res.icon == "success") {
                $('#crudModal').modal('hide');
                CheckAppIsUp(res.data);
            }
            $.toast({
                heading: res.header,
                text: res.message,
                position: 'top-right',
                loaderBg: '#FF6849',
                icon: res.icon,
                show: 3500,
                stack: 6,
            });
        },
        error: function (xhr, ajaxOptions, thrownError) {
            $.toast({
                heading: xhr.status,
                text: xhr.message,
                position: 'top-right',
                loaderBg: '#FF6849',
                icon: 'error',
                hideAfter: 5000,
                stack: 6
            });
        },
    });
}
function DeleteApp(_id) {

    swal({
        title: "Dikkat!",
        text: "Silmek istediğinize emin misiniz?",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Evet",
        cancelButtonText: "İptal",
        closeOnConfirm: true,
        closeOnCancel: true
    },
        function (isConfirm) {
            if (isConfirm) {
                $.ajax({
                    type: "post",
                    url: "/Application/DeleteApp",
                    data: { id: _id },
                    success: function (res) {
                        table.ajax.reload(null, false);

                        $.toast({
                            heading: res.header,
                            text: res.message,
                            position: 'top-right',
                            loaderBg: '#FF6849',
                            icon: res.icon,
                            show: 3500,
                            stack: 6,
                        });
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        $.toast({
                            heading: xhr.status,
                            text: xhr.message,
                            position: 'top-right',
                            loaderBg: '#FF6849',
                            icon: 'error',
                            hideAfter: 5000,
                            stack: 6
                        });
                    },
                });

            } else {
                swal("İptal!", "Silme işleminden vazgeçildi", "error");
            }
        });
}
function LoadUpdateAppPartial(_id) {
    LoadPage("/Application/_UpdateApp/" + _id, "#dynamicContent");
}
function UpdateApp(_id) {
    var model = {
        AppDefId: _id,
        Name: $("#name").val(),
        Url: $("#url").val(),
        Interval: $("#interval").val()
    }
    $.ajax({
        type: "post",
        url: "/Application/UpdateApp",
        data: { app: model },
        success: function (res) {
            table.ajax.reload(null, false);
            if (res.icon == "success") {
                $('#crudModal').modal('hide');
            }
            CheckAppIsUp(res.data);
            $.toast({
                heading: res.header,
                text: res.message,
                position: 'top-right',
                loaderBg: '#FF6849',
                icon: res.icon,
                show: 3500,
                stack: 6,
            });
        },
        error: function (xhr, ajaxOptions, thrownError) {
            $.toast({
                heading: xhr.status,
                text: xhr.message,
                position: 'top-right',
                loaderBg: '#FF6849',
                icon: 'error',
                hideAfter: 5000,
                stack: 6
            });
        },
    });
}
function CheckAppIsUp(_id) {
    $.ajax({
        type: "post",
        url: "/Application/CheckAppIsUp",
        data: { id: _id },
        success: function (res) {
            table.ajax.reload(null, false);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            $.toast({
                heading: xhr.status,
                text: xhr.message,
                position: 'top-right',
                loaderBg: '#FF6849',
                icon: 'error',
                hideAfter: 5000,
                stack: 6
            });
        },
    });
}
function LoadAddContact(_id) {
    LoadPage("/Notification/_AddNotificationContactToApp/" + _id, "#dynamicContent");
}
function AddNotificationContactToApp(_id) {
    var model = {
        AppDefId: _id,
        NotificationContactName: $("#name").val(),
        Email: $("#email").val(),
        Phone: $("#phone").val(),
        NotificationType: $("#type").val()
    };

    $.ajax({
        type: "post",
        url: "/Notification/AddNotificationContactToApp",
        data: { contact: model },
        success: function (res) {
            if (res.icon == "success") {
                $('#crudModal').modal('hide');
            }
            $.toast({
                heading: res.header,
                text: res.message,
                position: 'top-right',
                loaderBg: '#FF6849',
                icon: res.icon,
                show: 3500,
                stack: 6,
            });
        },
        error: function (xhr, ajaxOptions, thrownError) {
            $.toast({
                heading: xhr.status,
                text: xhr.message,
                position: 'top-right',
                loaderBg: '#FF6849',
                icon: 'error',
                hideAfter: 5000,
                stack: 6
            });
        },
    });
}
///////////////////// CRUD Bitiş ///////////////

function InitializeDataTable() {
    table = $('#AppTable').DataTable({
        serverSide: true,
        processing: false,
        responsive: true,
        order: [[7, 'desc']],
        lengthMenu: lengthArray,
        pageLength: 10,
        scrollX: true,
        ajax: {
            "url": '/Application/GetAppDefDataTable',
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.mainFilter = $("#AppTable_filter > label > input").val();
            }
        },
        language: { url: "/lib/DataTables/Turkish.json" },
        drawCallback: function (settings) {
            var res = settings.json;
            systemDateTime = res.SystemDateTime;
        },
        columns: [
            {
                "data": "AppDefId",
                "render": function (data, type, JsonResultRow, meta) {
                    return "";
                }
            },
            {
                "data": "AppDefId",
                "orderable": false,
                "render": function (data, type, JsonResultRow, meta) {
                    var content = '<button onclick="DeleteApp(' + JsonResultRow.AppDefId + ')" class="btn  btn-sm btn-outline-danger" title="Bu uygulamayı sil"><i class="fa fa-trash"></i></button> ';
                    content += '<button onclick="LoadUpdateAppPartial(' + JsonResultRow.AppDefId + ')" data-toggle="modal" data-target="#crudModal" class="btn  btn-sm btn-outline-info" title="Kayıt ayarlarını güncelle"><i class="fa fa-edit"></i></button> ';
                    content += '<button onclick="CheckAppIsUp(' + JsonResultRow.AppDefId + ')" class="btn  btn-sm btn-outline-success" title="Uygulama manuel olarak check et"><i class="fas fa-bolt"></i></button> ';
                    content += '<button onclick="LoadAddContact(' + JsonResultRow.AppDefId + ')" class="btn  btn-sm btn-outline-secondary" data-toggle="modal" data-target="#crudModal" title="Bildirim gönderilmesi için kişi kaydı ekler"><i class="fa fa-user"></i></button> ';
                    return content;
                }
            },
            {
                "data": "Name",
                "orderable": false,
                "render": function (data, type, JsonResultRow, meta) {
                    var content = '<a href="/Application/detail/' + JsonResultRow.AppDefId + '" class="btn  btn-sm btn-outline-secondary w-100" title="Detay">' + data + '</button> ';
                    return content;
                }
            },
            {
                "data": "Url",
                "orderable": false,
                "render": function (data, type, JsonResultRow, meta) {
                    var content = '<a target="_blank" href="' + data + '" class="btn  btn-sm btn-outline-secondary w-100 mw-200" title="Yeni sekmede aç --- ' + data + '">' + data + '</button> ';
                    return content;
                }
            },
            {
                "data": "Interval",
                "render": function (data, type, JsonResultRow, meta) {
                    return data + " dk";
                }
            },
            { data: "CreatedBy" },
            {
                "data": "CreatedDate",
                "render": function (data, type, JsonResultRow, meta) {
                    return SetWithServerDateTime(systemDateTime, data);
                }
            },
            { data: "UpdatedBy" },
            {
                "data": "UpdatedDate",
                "render": function (data, type, JsonResultRow, meta) {
                    return SetWithServerDateTime(systemDateTime, data);
                }
            },
            {
                "data": "LastControlDateTime",
                "render": function (data, type, JsonResultRow, meta) {
                    return SetWithServerDateTime(systemDateTime, data);
                }
            },
            {
                "data": "IsUp",
                "render": function (data, type, JsonResultRow, meta) {
                    if (data == true)
                        return "<span class='text-success'> <i class='fa fa-check'></i> " + "UP </span>"
                    else
                        return "<span class='text-danger'> <i class='fa fa-circle'></i> " + "DOWN </span>"
                }
            },
        ],
    });
    setInterval(function () {
        table.ajax.reload(null, false);
    }, 5000);
}