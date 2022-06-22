var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/api/DataTable/getAllProducts"
        },
        "columns": [
            { "data": "name", "width": "25%" },
            { "data": "occasion", "width": "15%" },
            { "data": "itemType", "width": "15%" },
            { "data": "price", "width": "15%" },
            { "data": "wholesale", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                    <div class="w-75 btn-group" role="group">
                        <a href="/Admin/ProductItemPage/Upsert?id=${data}"
                        class="btn btn-success mx-2"><i class="bi bi-pencil-square"></i> </a>
                        <a onClick=Delete('/api/DataTable/deleteProductItem/${data}')
                        class="btn btn-danger mx-2"><i class="bi bi-trash"></i></a>
                    </div>
                    `
                },
                "width": "15%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}