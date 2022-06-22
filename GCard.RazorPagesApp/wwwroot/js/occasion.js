var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/api/DataTable/getAllOccasions"
        },
        "columns": [
            { "data": "name", "width": "75%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                    <div class="w-75 btn-group" role="group">
                        <a href="/Admin/OccasionPage/Upsert?id=${data}"
                        class="btn btn-success mx-2"><i class="bi bi-pencil-square"></i> </a>
                        <a onClick=Delete('/Admin/Occasion/Delete/${data}')
                        class="btn btn-danger mx-2"><i class="bi bi-x-circle"></i> </a>
                    </div>
                    `
                },
                "width": "25%"
            }
        ]
    });
}