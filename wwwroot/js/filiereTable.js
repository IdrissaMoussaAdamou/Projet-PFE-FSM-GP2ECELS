$(document).ready(function() {
    $('#filiere-table').DataTable({
        responsive: true,
        //pageLength: 1,
        "iDisplayLength": 10,
        "aLengthMenu": [[10, 20, -1], [10, 20,"Tous"]],
        //lengthChange: false,
        "language": {
            "infoEmpty": "",
            "info": "",
            "infoFiltered":   "",
           "lengthMenu": "Montrer : _MENU_",
            "search":   "Chercher: ",
            "zeroRecords":  "Aucun Résultat Trouvé",
            "paginate": {
                "next": "Suivant",
                "previous": "Précédent"
            }
        },
        'columns': [
            { data: 'Code' }, /* index = 0 */
            { data: 'Intitule Français' }, 
            { data: 'Abréviation' }, 
            { data: 'Département'},
            { data: 'Type Diplôme' },
            { data: 'Actions' } 
         ],
         'columnDefs': [ {
            'targets': [1,2,5], 
            'orderable': false, 
            'searchable': false,
         }]
    }); 

    CreateEditFiliere = function(idFiliere)
    {
        let url = "/Filiere/CreateEdit?Id=" + idFiliere;
        $("#filiere-body").load(url, function () { 
            let form = $('form#filiere-form');
                $(form).removeData("validator")    
                    .removeData("unobtrusiveValidation"); 

                $.validator.unobtrusive.parse(form);
                if(idFiliere != -1)
                   $('#myModalLabel').html('Modifier Une Filiére')
                else
                    $('#myModalLabel').html('Ajouter Une Filière')
                $('#create-filiere').modal("show");
        })
    }
    $('#all').on( 'click', function () {
        table.page.len( -1 ).draw();
    } );
    $(document).on("click", "#submit", function (e) {
        e.preventDefault();

        let formData = $("#filiere-form").serialize();
        if(!$('form#filiere-form').valid())
            return false;
        $.ajax({
            type: "POST",
            url: "/Filiere/StoreUpdate",
            data: formData,
            success: function (data) {
                
                let html = `<tr class="row-color" id="${ "row_" + data.id }">
                                <td>${  data.code }</td>
                                <td>${ data.intituleFr }</td>
                                <td>${ data.intituleAbrg }</td>
                                <td>${ data.intituleFrDepartement }</td>
                                <td>${ data.intituleFrTypeDiplome }</td>
                                <td class="text-right">
                                    <a href= ${ "/Filiere/Show/" + data.id }>
                                        <i class="fa fa-eye b-icon fa-2x "  title="Consulter"></i>
                                    </a>
                                    <a href="#" onclick="ConfirmFiliereDelete(${ data.id })">
                                        <i class="fa fa-trash-o b-icon fa-2x"  title="supprimer"></i>
                                    </a>
                                </td>
                            </tr>`
                if($("td.dataTables_empty"))
                {
                    $("td.dataTables_empty").parent().remove();
                }
                $("#filiere-table-body").append(html);
                $('#create-filiere').modal("hide");

            }
        });
    })

    ConfirmFiliereDelete = function (idFiliere) { 
        $('#ItemId').val(idFiliere);
        $("#delete-filiere").modal("show");
    }
    DeleteFiliere = function () { 
        var idFiliere = $("#ItemId").val();
        $.ajax({
            type: "POST",
            url: "/Filiere/Delete",
            data: { IdFiliere : idFiliere },
            success: function (data) {
                if(data.delete) {
                    var html = `<tr class="odd">
                                     <td valign="top" colspan="6" class="dataTables_empty">Aucune Donnée</td>
                                </tr>`;
                    $("#row_" + data.id).remove();

                    if($("#filiere-table-body tr").length == 0)
                        $("#filiere-table-body").append(html);
                    $("#delete-filiere").modal("hide");
                }
                else { 
                    $("#info-message").html('Cette Filière est liée à ' + data.element)
                    $("#delete-filiere").modal("hide");
                    $("#info-modal").modal("show");
                }
                
            }
        });
    }
});