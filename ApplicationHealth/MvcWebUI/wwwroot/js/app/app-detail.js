$('#appContact').DataTable({
    order: [[3, 'desc']],
    language: { url: "/lib/DataTables/Turkish.json" },
});
$('#appNotiTable').DataTable({
    order: [[3, 'desc']],
    language: { url: "/lib/DataTables/Turkish.json" },
});
function DeleteContact(_id) {
    $.ajax({
        type: "post",
        url: "/Notification/DeleteContact",
        data: { id: _id },
        success: function (res) {
            $.toast({
                heading: res.header,
                text: res.message,
                position: 'top-right',
                loaderBg: '#FF6849',
                icon: res.icon,
                show: 3500,
                stack: 6,
            });
            window.location.reload();
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