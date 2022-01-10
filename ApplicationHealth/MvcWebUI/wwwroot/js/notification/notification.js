
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
        order: [[4, 'desc']],
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
                "data": "AppNotificationId",
                "render": function (data, type, JsonResultRow, meta) {
                    return "";
                }
            },
           
            {
                "data": "AppDef.Name",
                "orderable": false,
                "render": function (data, type, JsonResultRow, meta) {
                    var content = '<a href="/Application/detail/' + JsonResultRow.AppDef.AppDefId + '" class="btn  btn-sm btn-outline-secondary w-100" title="Detay">' + data + '</button> ';
                    return content;
                }
            },
           
            {
                "data": "Message",
                "render": function (data, type, JsonResultRow, meta) {
                    return data;
                }
            },
            { data: "Contact.Email" },
            {
                "data": "SentDateTime",
                "render": function (data, type, JsonResultRow, meta) {
                    return SetWithServerDateTime(systemDateTime, data);
                }
            },
        ],
    });
    setInterval(function () {
        table.ajax.reload(null, false);
    }, 5000);
}