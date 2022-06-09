$(document).ready(function() {
    $('#dataTables-example').DataTable({
        responsive: true,
        "iDisplayLength": 10,
        "aLengthMenu": [[10, 20, -1], [10, 20,"Tous"]],
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
            { data: 'Code' }, 
            { data: 'Intitule Français' }, 
            { data: 'Intitule Arabe'},
            { data: 'Actions' } 
         ],
         'columnDefs': [ {
            'targets': [1,2,3],
            'orderable': false,
            'searchable': false,
         }]
        
    });

        CreateEditDepartement = function(Code){
            let url = "/Departement/CreateEditDepartement?Code=" + Code;
            $("#create-edit-departemet-body").load(url, function(){
                let form = $('form#departement-form');
                $(form).removeData("validator")    // Added by jQuery Validate
                .removeData("unobtrusiveValidation");   // Added by jQuery Unobtrusive Validation
                $.validator.unobtrusive.parse(form);
                if(Code !='')
                   $('#create-departement').html('Modifier Un Département')
                else
                    $('#create-departement').html('Ajouter Un Département')
                $('#departement-create').modal("show");
            })
        }

        $(document).on('click', 'button#submit', function (e) {
            e.preventDefault();    
            let formData = $('form#departement-form').serialize();
            if(!$('form#departement-form').valid())
                return false;
            
            $.ajax({
                type: "POST",
                url: "/Departement/StoreUpdate",
                data: formData,
                success: function (data) {
                    let html = `<tr class="row-color" id="${ "row_" + data.code }">
                                    <td>${ data.code }</td>
                                    <td>${ data.intituleFr }</td>
                                    <td>${ data.intituleAr }</td>
                                    <td class="text-right">
                                        <a href="#" onclick="CreateEditDepartement(${ data.code })">
                                            <i class="fa fa-edit fa-2x" style="color: #337ab7;" title="Modifier"></i>
                                        </a>
                                        <a href="#" onclick="ConfirmDelete(${ data.code })">
                                            <i class="fa fa-trash-o b-icon fa-2x"  title="Supprimer"></i>
                                        </a>
                                    </td>
                                </tr>`

                    if($("#departement-tbody td.dataTables_empty"))
                    {
                        $("#departement-tbody td.dataTables_empty").parent().remove();
                    }
                    if($("#row_" + data.code).length)
                        $("#row_" + data.code).replaceWith(html);
                    else
                    $("#departement-tbody").append(html);
                    
                    $('#departement-create').modal("hide");
                }
                
            });
        });   

        ConfirmDelete = function(Code) {
            $('#departemetcode').val(Code);
            $('#delete-departement').modal('show');
        }

        DeleteDepartement = function () {
            let code = $('#departemetcode').val();
            $.ajax({
                type: "POST",
                url: "/Departement/Delete",
                data: { pCode: code },
                success: function (data) {

                    if(data.delete) {
                        let html = `<tr class="odd">
                                    <td valign="top" colspan="4" class="dataTables_empty">Aucune Donnée</td>
                                </tr>`;
                        $('#row_' + data.code).remove();

                        if($("#departement-tbody tr").length == 0)
                            $("#departement-tbody").append(html);
                        $('#delete-departement').modal('hide');
                    } else { 
                        $("#info-message").html('Ce Département est lié à ' + data.element)
                        $('#delete-departement').modal('hide');
                        $("#info-modal").modal("show");
                    }
                }
            });
        }    
});

 
