var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/ItemType/GetAll"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "displayOrder", "width": "15%" },
            //{
            //    "data": "id",
            //    "render": function (data) {
            //        return `
            //        <div class="w-75 btn-group" role="group">
            //            <a href="/Admin/Book/Upsert?id=${data}" 
            //            class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i> Edit</a>
            //            <a onClick=Delete('/Admin/Book/Delete/${data}')
            //            class="btn btn-danger mx-2"><i class="bi bi-x-circle"></i> Delete</a>
            //        </div>
            //        `
            //    },
            //    "width": "15%"
            //}
        ]
    });
}