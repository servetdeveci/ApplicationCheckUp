
var table;
var systemDateTime;

$(document).ready(function () {
    InitializeDataTable();
});

function InitializeDataTable() {
    table = $('#AppNotiTable').DataTable({
        serverSide: true,
        processing: false,
        responsive: true,
        order: [[7, 'desc']],
        lengthMenu: lengthArray,
        pageLength: 10,
        scrollX: true,
        ajax: {
            "url": '/Notification/GetNotificationDataTable',
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.mainFilter = $("#AppNotiTable_filter > label > input").val();
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