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
            { data: 'Abréviation'},
            { data: 'Actions' } 
         ],
         'columnDefs': [ {
            'targets': [1,2,4],
            'orderable': false,
            'searchable': false,
         }]
        
    });

    CreateEditTypeDiplome = function(Code){

        var url = "/TypeDiplome/CreateEditTypeDiplome?Code=" + Code;
        $("#create-edit-typediplome-body").load(url, function(){
            var form = $('form#typediplome-form');
            $(form).removeData("validator")    
            .removeData("unobtrusiveValidation");  
            $.validator.unobtrusive.parse(form);
            if(Code !='')
               $('#create-typediplome').html('Modifier Un Type De Diplome')
            else
                $('#create-typediplome').html('Ajouter Un Type De Diplome')
            $('#typediplome-create').modal("show");
        })
    }
    $(document).on('click', 'button#submit', function (e) {
        e.preventDefault();    
        var formData = $('form#typediplome-form').serialize();
        if(!$('form#typediplome-form').valid())
            return false;
        
        $.ajax({
            type: "POST",
            url: "/TypeDiplome/StoreUpdate",
            data: formData,
            success: function (data) {
                let html = `<tr class="row-color" id="${ "row_" + data.code}">
                                <td>${ data.code }</td>
                                <td>${ data.intituleFr }</td>
                                <td>${ data.intituleAr }</td>
                                <td>${ data.intituleAbrg }</td>
                                <td class="text-right">
                                    <a href="#" onclick="CreateEditTypeDiplome(${ data.code })">
                                        <i class="fa fa-edit fa-2x" style="color: #337ab7;"></i>
                                    </a>
                                    <a href="#" onclick="ConfirmDelete(${ data.code })">
                                        <i class="fa fa-trash-o b-icon fa-2x" ></i>
                                    </a>
                                </td>
                            </tr>`
                    if($("#diplome-tbody td.dataTables_empty"))
                    {
                        $("#diplome-tbody td.dataTables_empty").parent().remove();
                    }
                    if($("#row_" + data.code).length)
                        $("#row_" + data.code).replaceWith(html);
                    else
                    $("#diplome-tbody").append(html);
                       $('#typediplome-create').modal("hide");
            }
        });
    });

    ConfirmDelete = function(Code) {
        $('#typedeplome-code').val(Code);
        $('#delete-typediplome').modal('show');
    }

    DeleteTypeDiplome = function () {
        var code = $('#typedeplome-code').val();
        $.ajax({
            type: "POST",
            url: "/TypeDiplome/Delete",
            data: { pCode: code },
            success: function (data) {

                if(data.delete) {
                    let html = `<tr class="odd">
                                <td valign="top" colspan="5" class="dataTables_empty">Aucune Donnée</td>
                            </tr>`;

                    $('#row_' + data.code).remove();
                    if($("#diplome-tbody tr").length == 0)
                        $("#diplome-tbody").append(html);
                    $('#delete-typediplome').modal('hide');
                } else { 
                    $("#info-message").html('Ce Type De Diplôme est lié à ' + data.element)
                    $('#delete-typediplome').modal('hide');
                    $("#info-modal").modal("show");
                }
                
            }
        });
    }   
});




