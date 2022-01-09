
var table;

$(document).ready(function () {
    table = $('#AppTable').DataTable({
        serverSide: true,
        processing: false,
        responsive: true,
        lengthMenu: lengthArray,
        pageLength: 10,
        scrollX: true,
        ajax: {
            "url": '/home/GetAppDefDataTable',
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.mainFilter = $("#AppTable_filter > label > input").val();
            }
        },
        autoWidth: true,
        language: { url: "/lib/DataTables/Turkish.json" },
        columns: [
            {
                "data": "AppDefId",
                "orderable": false,
                "render": function (data, type, JsonResultRow, meta) {
                    var content = '<button onclick="DeleteApp(' + JsonResultRow.AppDefId + ')" class="btn  btn-sm btn-outline-danger" title="Sil"> Sil</button> ';
                    content += '<button onclick="UpdateApp(' + JsonResultRow.AppDefId + ')" class="btn  btn-sm btn-outline-info" title="Güncelle"> Güncelle</button> ';
                    return content;
                }
            },
            { data: "Name" },
            { data: "Url" },
            { data: "Interval" },
            { data: "CreatedBy" },
            {
                "data": "CreatedDate",
                
            },
            {
                "data": "UpdatedDate",
                
            },
        ],
    });
});

function LoadInsertAppPartial() {
    LoadPage("/Home/_InsertApp", "#dynamicContent");
}
function AddApp() {

    var model = {
        Name: $("#name").val(),
        Url: $("#url").val(),
        Interval: $("#interval").val()
    }
    $.ajax({
        type: "post",
        url: "/Home/InsertApp",
        data: { app: model },
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
                    url: "/Home/DeleteApp",
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


function LoadUpdateAppPartial() {

}
function UpdateApp(_id) {

    $.ajax({
        type: "post",
        url: "/Home/DeleteApp",
        data: { id: id },
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
}