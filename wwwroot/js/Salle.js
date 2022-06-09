 var datatablee
$(document).ready(function () {

    datatablee = $('#Salle-datable').DataTable({
        responsive: true,
        "iDisplayLength": 10,
        "aLengthMenu": [[10, 20, -1], [10, 20, "Tous"]],
        "language": {
            "infoEmpty": "",
            "info": "",
            "infoFiltered": "",
            "lengthMenu": "Montrer : _MENU_",
            "search": "Chercher: ",
            "zeroRecords": "Aucun Résultat Trouvé",
            "paginate": {
                "next": "Suivant",
                "previous": "Précédent"
            }
        },
        'columns': [
            { data: 'Code' },
            { data: 'Type' },
            { data: 'CodeBatiment' },
            { data: 'Etat' },
            { data: 'Action' }
        ],
        'columnDefs': [{
            //'targets': [1, 2, 3],
            'orderable': false,
            'searchable': false,
        }]

    });

    CreateEditSalle = function (Code) {
        let url = "/Locaux/CreateEditSalle?Code=" + Code;
        $("#create-edit-Salle-body").load(url, function () {
            let form = $('form#Salle-form');
            $(form).removeData("validator")    // Added by jQuery Validate
                .removeData("unobtrusiveValidation");   // Added by jQuery Unobtrusive Validation
            $.validator.unobtrusive.parse(form);
            if (Code != '')
                $('#create-Salle').html('Modifier Une Salle')
            else
                $('#create-Salle').html('Ajouter Une Salle')
            $('#Salle-create').modal("show");
        })
    }

    $(document).on("click", "#submit", function (e) {
        e.preventDefault();
        let formData = $('form#Salle-form').serialize();

        if (!$('form#Salle-form').valid())
            return false;

        $.ajax({
            type: "POST",
            url: "/Locaux/StoreUpdate",
            data: formData,
            success: function (data) {
                //debugger
                //let html = `<tr class="row-color" id="${"row_" + Salle.Code}">
                //            <td>${Salle.Code}</td>
                //            <td>${Salle.type}</td>
                //            <td>${Salle.CodeBatiment}</td>
                //            <td>${Salle.Etat}</td>
                //            <td class="text-right" >
                //                <a href="#" onclick="ShowSalle(${Salle.Code})">
                //                    <i class="fa fa-eye b-icon fa-2x "  title="Consulter"></i>
                //                </a>
                //                <a href="#" onclick="CreateEditSalle(${Salle.Code})">
                //                    <i class="fa fa-edit fa-2x" style="color: #337ab7;" title="Modifier"></i>
                //                </a>
                //                <a href="#" onclick="ConfirmDeleteSalle(${Salle.Code})">
                //                    <i class="fa fa-trash-o b-icon fa-2x"  title="Supprimer"></i>
                //                </a>
                //            </td>
                //        </tr>`
                //if ($("td.dataTables_empty")) {
                //    $("td.dataTables_empty").parent().remove();
                //}
                //if ($("#row_" + Salle.Code).length)
                //    $("#row_" + Salle.Code).replaceWith(html);
                //else
                //    $("#Salle-table-body").append(html);

                //$("#Salle-create").modal("hide");
                if (data[0] == "") {
                    location.reload();
                    //$('#Salle-datable').DataTable().ajax.reload();
                }
                else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: data[0],
                        //footer: '<a href>Why do I have this issue?</a>'
                    })
                }
            }
        });
    })

    ConfirmDelete = function (Code) {
        $('#Sallecode').val(Code);
        $('#delete-Salle').modal('show');
    }

    DeleteSalle = function () {
        let code = $('#Sallecode').val();
        $.ajax({
            type: "GET",
            url: "/Locaux/Delete?Code="+code,
            //data: code,
            success: function (data) {

                if (data.delete) {
                    let html = `<tr class="odd">
                                    <td valign="top" colspan="4" class="dataTables_empty">Aucune Donnée</td>
                                </tr>`;
                    $('#row_' + data.code).remove();

                    if ($("#Salle-tbody tr").length == 0)
                        $("#Salle-tbody").append(html);
                    $('#delete-Salle').modal('hide');
                } else {
                    $("#info-message").html('Cette salle est lié à ' + data.element)
                    $('#delete-Salle').modal('hide');
                    $("#info-modal").modal("show");
                }
            }
        });
    }    
    ShowSalle = function (code) {
        let url = "/Locaux/Show?code=" + code;
        $("#Salle-info").load(url, function () {
            $("#show-Salle").modal("show");
        })
    }
});