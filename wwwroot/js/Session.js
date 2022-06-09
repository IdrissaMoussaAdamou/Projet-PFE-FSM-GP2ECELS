
$(document).ready(function () {

    $('#Session-datable').DataTable({
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
            { data: 'Désignation' },
            { data: 'AnneeUniversitaire' },
            { data: 'Periode' },
            { data: 'Etat' },
            { data: 'Action' }
        ],
        'columnDefs': [{
            //'targets': [1, 2, 3],
            'orderable': false,
            'searchable': false,
        }]

    });

    CreateEditSession = function (Code) {
        let url = "/Session/CreateEditSession?Code=" + Code;
        $("#create-edit-Session-body").load(url, function () {
            let form = $('form#Session-form');
            $(form).removeData("validator")    // Added by jQuery Validate
                .removeData("unobtrusiveValidation");   // Added by jQuery Unobtrusive Validation
            $.validator.unobtrusive.parse(form);
            if (Code != '')
                $('#create-Session').html('Modifier Une Session')
            else
                $('#create-Session').html('Ajouter Une Session')
            $('#Session-create').modal("show");
        })
    }

    $(document).on("click", "#submit", function (e) {
        e.preventDefault();
        let formData = $('form#Session-form').serialize();

        if (!$('form#Session-form').valid())
            return false;

        $.ajax({
            type: "POST",
            url: "/Session/StoreUpdate",
            data: formData,
            success: function (data) {
                if (data[0] == "") {
                    location.reload();
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
        $('#Sessioncode').val(Code);
        $('#delete-Session').modal('show');
    }

    DeleteSession = function () {
        let code = $('#Sessioncode').val();
        $.ajax({
            type: "GET",
            url: "/Session/Delete?id="+code,
            //data: code,
            success: function (data) {

                if (data.delete) {
                    let html = `<tr class="odd">
                                    <td valign="top" colspan="4" class="dataTables_empty">Aucune Donnée</td>
                                </tr>`;
                    $('#row_' + data.code).remove();

                    if ($("#Session-tbody tr").length == 0)
                        $("#Session-tbody").append(html);
                    $('#delete-Session').modal('hide');
                } else {
                    $("#info-message").html('Cette Session est lié à ' + data.element)
                    $('#delete-Session').modal('hide');
                    $("#info-modal").modal("show");
                }
            }
        });
    }
});