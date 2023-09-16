var datatable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    datatable = $('#tblUsuarios').DataTable({
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
            "url": "/Admin/Usuarios/ObtenerTodos"
        },
        "columns": [
            { "data": "userName", "width": "15%" },
            { "data": "name", "width": "15%" },
            { "data": "lastName", "width": "15%" },
            { "data": "email", "width": "20%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "role", "width": "15%" },
            {
                "data": {
                    id: "id", lockoutEnd: "lockoutEnd"
                },
                "render": function (data) {
                    let today = new Date().getTime();
                    let bloqueo = new Date(data.lockoutEnd).getTime();
                    if (bloqueo > today) {
                        //Usuario esta bloqueado
                        return `
                        <div class="text-center">
                            <a onClick=BloquearDesbloquear('${data.id}') class="btn btn-danger text-white" style="cursor:pointer; width:150px;">
                                <i class="bi bi-unlock-fill"></i> Desbloquear
                            </a>
                        </div>
                        `;
                    }
                    else {
                        return `
                        <div class="text-center">
                            <a onClick=BloquearDesbloquear('${data.id}') class="btn btn-success text-white" style="cursor:pointer; width:150px;" >
                                <i class="bi bi-lock-fill"></i> Bloquear
                            </a>
                        </div>
                        `;
                    }

                }, "width": "5%"
            }
        ]
    });
}
function BloquearDesbloquear(id) {
    $.ajax({
        type: "POST",
        url: '/Admin/Usuarios/BloquearDesbloquear',
        data: JSON.stringify(id),
        contentType: "application/json",
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