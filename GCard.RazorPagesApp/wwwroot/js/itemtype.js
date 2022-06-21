var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/api/ItemType"
        },
        "columns": [
            { "data": "name", "width": "50%" },
            { "data": "displayOrder", "width": "25%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                    <div class="w-75 btn-group" role="group">
                        <a href="/Admin/ItemTypePage/Upsert?id=${data}"
                        class="btn btn-success mx-2"><i class="bi bi-pencil-square"></i> </a>
                        <a onClick=Delete('/Admin/ItemType/Delete/${data}')
                        class="btn btn-danger mx-2"><i class="bi bi-x-circle"></i> </a>
                    </div>
                    `
                },
                "width": "25%"
            }
        ]
    });
}

/*class="w-75 btn-group" role = "group"*/

//<a href="/Admin/ItemTypePage/Upsert?id=${data}"
//class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i> Edit</a>

//<a onClick=Delete('/Admin/Book/Delete/${data}')
//class="btn btn-danger mx-2" > <i class="bi bi-x-circle"></i> Delete</a >