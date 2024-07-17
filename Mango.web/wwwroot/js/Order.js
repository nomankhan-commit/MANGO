var dataTable;

$(document).ready(function () {
    LoadDataTable();
})

function LoadDataTable() {

    datatable = $("#datatable").DataTable({
        "ajax": { url: "/order/Getall" },
        "columns": [
            { data: "OrderHeaderId", width: '5%'},
            { data: "Name", width: '25%'},
            { data: "Email", width: '25%'},
            { data: "Status", width: '10%'},
            { data: "PhoneNumber", width: '25%'},
            { data: "Total", width: '25%'},
        ]
    })

}