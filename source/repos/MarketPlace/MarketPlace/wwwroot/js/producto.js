var datatable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    datatable = $('#tblProductos').DataTable({
        "language": {
            "lengthMenu": "Mostrar _MENU_ Registros por Pagina",
            "zeroRecords": "Ningun Registro",
            "info": "Mostrar pagina _PAGE_ de _PAGES_",
            "infoEmpty": "(filtered from _MAX_ total Registros)",
            "search": "Buscar",
            "paginate": {
                "firts": "Primero",
                "last": "Ultimo",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        "ajax": {
            "url": "/Admin/Productos/ObtenerTodos"
        },
        "columns": [
            { "data": "serialNumber" },
            { "data": "name" },
            { "data": "description" },
            { "data": "categoria.name" },
            { "data": "marca.name" },
            
            {
                "data": "price", "className": "text-end",
                "render": function (data) {
                    var d = data.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
                    return d
                }
            },
            {
                "data": "tieneDescuento",
                "render": function (data) {
                    if (data == true) {
                        return "Si";
                    }
                    else {
                        return "No";
                    }
                },
            },
            {
                "data": "status",
                "render": function (data) {
                    if (data == true) {
                        return "Disponible";
                    }
                    else {
                        return "Agotado";
                    }
                },
            },
           
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Admin/Productos/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                <i class="bi bi-pencil-square"></i>
                            </a>
                            <a onClick=Delete("/Admin/Productos/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                <i class="bi bi-trash3-fill"></i>
                            </a>
                        </div>
                        `;
                }, "width": "10%"
            }
        ]
    });
}
function Delete(url) {
    swal({
        title: "Esta seguro de eliminar el producto?",
        text: "Este registro no se puede recuperar",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((borrar) => {
        if (borrar) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        datatable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}