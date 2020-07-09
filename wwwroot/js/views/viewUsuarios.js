
$(document).ready(function () {
    //document.getElementById("rolId").style.display = "none !important";
    //document.getElementById("rolId").style.setProperty('display', 'none', 'important');
    $('.perfilId').hide();
});

$('#tbUsuarios').DataTable({
    dom: '<"row"<"col-md-12"<"row"<"col-md-6"B><"col-md-6"f> > ><"col-md-12"rt> <"col-md-12"<"row"<"col-md-5"i><"col-md-7"p>>> >',
    buttons: {
        buttons: [
            { extend: 'excel', className: 'btn' }
        ]
    },
    "oLanguage": {
        "oPaginate": { "sPrevious": '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-arrow-left"><line x1="19" y1="12" x2="5" y2="12"></line><polyline points="12 19 5 12 12 5"></polyline></svg>', "sNext": '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-arrow-right"><line x1="5" y1="12" x2="19" y2="12"></line><polyline points="12 5 19 12 12 19"></polyline></svg>' },
        "sInfo": "Mostrando página _PAGE_ de _PAGES_",
        "sSearch": '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-search"><circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line></svg>',
        "sSearchPlaceholder": "Buscar...",
        "sLengthMenu": "Resultados :  _MENU_",
    },
    "stripeClasses": [],
    "lengthMenu": [7, 10, 20, 50],
    "pageLength": 7
});


$("#btnPerfil").click(function (e) {
    var idRol = $("#cboPerfil").children("option:selected").val();
    var txtRol = $("#cboPerfil").children("option:selected").text();
    var table = document.getElementById("tbPerfil");

    if (idRol != "") {
        for (var i = 1, row; row = table.rows[i]; i++) {
            var id = document.getElementById("tbPerfil").rows[i].cells.namedItem("perfilId").innerHTML;
            var descrip = document.getElementById("tbPerfil").rows[i].cells.namedItem("perfilDesc").innerHTML;//Evaluar una sola columna
            if (descrip == txtRol) {
                alert("Registro ya insertado");
                return;

            }
            //for (var j = 0, col; col = row.cells[j]; j++) { //Evaluar columna y fila
            //    var x = document.getElementById("tbRol").rows[i].cells[j].innerHTML;
            //    alert(x);
            //}
        }
        agregarListaPerfiles(idRol, txtRol);
    } else {
        alert("Debe seleccionar un perfil");
        return;
    }
});

function agregarListaPerfiles(value, text) {
    if ($("#txtUsuario").val() !== "") {
        var tableCount = document.getElementById("tbPerfil");
        var tbody = tableCount.tBodies[0];
        $("#tbPerfil").append(`<tr><td class="perfilId" id="perfilId" style="display: none;">${value}</td><td id="perfilDesc">${text}</td><td>      
           <a id='aPerfil${value}' href='javascript:eliminarFila("tbPerfil",${tbody.rows.length + 1}, "perfil")'><svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-x-circle table-cancel"><circle cx="12" cy="12" r="10"></circle><line x1="15" y1="9" x2="9" y2="15"></line><line x1="9" y1="9" x2="15" y2="15"></line></svg>
           </a>
        </td></tr>`);
        $('#detPerfil').append(`<input type='hidden' id='perfil${tbody.rows.length}' name='perfil${tbody.rows.length}' value='${value}' />`)
        document.getElementById("countRows").value = tbody.rows.length;
    } else {
        alert("Para registrar un perfil debe ingresar usuario");
        return;
    }
    //document.getElementById("disabledTextInput").value = tbody.rows.length;
}

function eliminarFila(table, row, idInput) {
    document.getElementById("" + table + "").deleteRow(row);

    if (idInput != "")
        $("#" + idInput + "" + row + "").remove();

    var idCountRows = $("#countRows").val();
    var countRows = "";
    if (idCountRows - 1 == 0)
        countRows = "";
    else
        countRows = idCountRows - 1
    document.getElementById("countRows").value = countRows;
    //document.getElementById("disabledTextInput").value = countRows;

    var tableCount = document.getElementById("" + table + "");
    var tbody = tableCount.tBodies[0];
    var j = 1;
    var i = 1
    var h = 1;
    var k = 1;
    for (i; i <= tbody.rows.length; i++) {
        if ($("#aPerfil" + j + "").length) {
            document.getElementById("aPerfil" + j + "").href = "javascript:eliminarFila('" + table + "','" + i + "', '" + idInput + "')";
        } else {
            i = i - 1;
        }
        j = j + 1;
    }
    for (h; h <= tbody.rows.length; h++) {
        if ($("#perfil" + k + "").length) { 
            document.getElementById("perfil" + k + "").id = "perfil" + h + "";
            document.getElementById("perfil" + h + "").setAttribute("name", "perfil" + h + "");
        } else {
            h = h - 1;
        }
        k = k + 1;
    }
}

function deleteUser(id) {
    //alert(id);
    const r = confirm('¿Desea eliminar el registro? ' + id);
    if (r === true)
        location.href = "/Usuario/DelUsuario?id=" + id;
    else
        return;
}