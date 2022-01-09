
var table;

$(document).ready(function () {
    table = $('#AppTable').DataTable({
        dom: '<"toolbar">Blfrtip',
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
        caseInsensitive: true,
        fixedHeader: true,
        autoWidth: true,
        language: { url: "/lib/DataTables/Turkish.json" },
        columns: [
            {
                "data": "AppDefId",
                "orderable": false,
                "render": function (data, type, JsonResultRow, meta) {
                    var content = '<button onclick="Delete(\'' + JsonResultRow.AppDefId + '\')" class="btn  btn-sm btn-outline-danger" title="Sil"><i class=" fas fa-trash"></i> Sil</button> ';
                    content += '<button onclick="Update(\'' + JsonResultRow.AppDefId + '\')" class="btn  btn-sm btn-outline-info" title="Güncelle"><i class=" fas fa-edit"></i> Güncelle</button> ';
                    return content;
                }
            },
            { data: "Name" },
            { data: "Url" },
            { data: "Interval" },
            { data: "CreatedBy" },
            { data: "CreatedDate" },
            { data: "UpdatedDate" }
        ],
        buttons:[

                {
                    extend: 'excel',
                    title: GetTime() + ' - Ayarlar EXCEL Raporu',
                    text: '<span class=""><i class="fas fa-file-excel"></i></span> Excel',
                    className: 'btn  btn-outline-secondary btn-sm hidden-md-down',
                    exportOptions: {
                        columns: [1,2,3,4]
                    }
                },
                {
                    extend: 'pdf',
                    orientation: 'landscape',
                    title: GetTime() + ' - Ayarlar PDF Raporu',
                    text: '<span class=""><i class="fas fa-file-pdf"></i></span> PDF',
                    className: 'btn  btn-outline-secondary btn-sm',
                    exportOptions: {
                        columns: [1, 2, 3, 4]

                    }
                },
                
            ],
    });
});

function InsertApp() {

}
