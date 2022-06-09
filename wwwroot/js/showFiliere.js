
$(document).ready(function() {
    $('#niveau-table,#parcours-table').DataTable({
         responsive: false,
         "searching": false,
         "ordering":false,
         "info": false,
         "paging": false,
         "language": {
         "emptyTable": "Aucune Donnée"
        }
    });
    
    //              Niveau

    CreateEditNiveau = function (idFilire, idNiveau) { 

        var url = "/Niveau/CreateEditNiveau?Codes=" + idFilire + "," + idNiveau;
        $("#niveau-body").load(url, function(){
            var form = $('form#niveau-form');
                $(form).removeData("validator")    
                       .removeData("unobtrusiveValidation");  
                $.validator.unobtrusive.parse(form);
                if(idNiveau != -1)
                   $('#create-niveau').html('Modifier Un Niveau')
                else
                    $('#create-niveau').html('Ajouter Un Niveau')
                $('#niveau-create').modal("show");
        })
    }

    $(document).on("click", "button#submit-niveau", function (e) { 
        e.preventDefault();    
            var formData = $('form#niveau-form').serialize();

            if(!$('form#niveau-form').valid())
                return false;
            $.ajax({
                type: "POST",
                url: "/Niveau/StoreUpdate",
                data: formData,
                success: function (data) {
                    let html = `<tr class="row-color" id="${ "row_" + data.id }">
										<td>${ data.code }</td>
										<td>${ data.intituleFr }</td>
										<td>${ data.intituleAr }</td>
										<td>${ data.intituleAbrg }</td>
										<td class="text-right">
											<a href="#" onclick="CreateEditNiveau(${ data.idFiliere },${ data.id })">
												<i class="fa fa-edit fa-2x" style="color: #337ab7;"></i>
											</a>
											<a href="#" onclick="ConfirmDeleteNiveau(${ data.id })">
												<i class="fa fa-trash-o b-icon fa-2x" ></i>
											</a>
										</td>
                                </tr>`
                    if($(" #niveau-table-body td.dataTables_empty"))
                    {
                        $("#niveau-table-body td.dataTables_empty").parent().remove();
                    }
                    if($("#row_" + data.id).length)
                        $("#row_" + data.id).replaceWith(html);
                    else
                    $("#niveau-table-body").append(html);
                    
                    $('#niveau-create').modal("hide");
                }
            });
    })

    ConfirmDeleteNiveau = function (idNiveau) { 
        $("#hidden-codeNiveau").val(idNiveau);
        $("#delete-niveau").modal("show");
    }

    DeleteNiveau = function(){
        var idNiveau = $("#hidden-codeNiveau").val();
        $.ajax({
            type: "POST",
            url: "/Niveau/Delete",
            data: { Id : idNiveau},
            success: function (data) {

                if(data.delete) {
                    var html = `<tr class="odd">
                                <td valign="top" colspan="5" class="dataTables_empty">Aucune Donnée</td>
                            </tr>`;
                    $("#row_" + data.id).remove();

                    if($("#niveau-table-body tr").length == 0)
                        $("#niveau-table-body").append(html);
                    $("#delete-niveau").modal("hide");
                }
                else { 
                    $("#info-message").html('Ce Niveau est lié à ' + data.element)
                    $("#delete-niveau").modal("hide");
                    $("#info-modal").modal("show");
                }
            }
        });
    }

    // Parcours

    CreateEditParcours = function (idFilire, idParcours) { 

        var url = "/Parcours/CreateEditParcours?Codes=" + idFilire + "," + idParcours;
        $("#parcours-body").load(url, function(){
            var form = $('form#parcours-form');
                $(form).removeData("validator")    
                       .removeData("unobtrusiveValidation");  
                $.validator.unobtrusive.parse(form);
                if(idParcours != -1)
                   $('#create-parcours').html('Modifier Un Parcours')
                else
                    $('#create-parcours').html('Ajouter Un Parcours')
                $('#parcours-create').modal("show");
        })
    }

    $(document).on("click", "button#submit-parcours", function (e) { 
        e.preventDefault();    
            let formData = $('form#parcours-form').serialize();

            if(!$('form#parcours-form').valid())
                return false;
            $.ajax({
                type: "POST",
                url: "/Parcours/StoreUpdate",
                data: formData,
                success: function (parcours) {
                    let html = `<tr class="row-color" id="${ "row-" + parcours.id }">
                                    <td>${ parcours.code }</td>
                                    <td>${ parcours.intituleFr }</td>
                                    <td>${ parcours.intituleAr }</td>
                                    <td>${ parcours.intituleAbrg }</td>
                                    <td>${ parcours.periodeHabilitation }</td>
                                    <td class="text-right">
                                        <a href="#" onclick="CreateEditParcours(${ parcours.idFiliere },${ parcours.id })">
                                            <i class="fa fa-edit fa-2x" style="color: #337ab7;"></i>
                                        </a>
                                        <a href="#" onclick="ConfirmDeleteParcours(${ parcours.id })">
                                            <i class="fa fa-trash-o b-icon fa-2x" ></i>
                                        </a>
                                    </td>
                                </tr> `
                    if($("#parcours-table-body td.dataTables_empty"))
                    {
                        $("#parcours-table-body td.dataTables_empty").parent().remove();
                    }
                    if($("#row-" + parcours.id).length)
                        $("#row-" + parcours.id).replaceWith(html);
                    else
                    $("#parcours-table-body").append(html);
                    $('#parcours-create').modal("hide");
                }
            });
    })

    ConfirmDeleteParcours = function (idParcours) { 
        $("#hidden-codeParcours").val(idParcours);
        $("#delete-parcours").modal("show");
    }

    DeleteParcours = function(){
        var idParcours = $("#hidden-codeParcours").val();
        $.ajax({
            type: "POST",
            url: "/Parcours/Delete",
            data: { Id: idParcours},
            success: function (data) {

                if(data.delete) {
                    var html = `<tr class="odd">
                                <td valign="top" colspan="6" class="dataTables_empty">Aucune Donnée</td>
                            </tr>`;
                    $("#row-" + data.id).remove();

                    if($("#parcours-table-body tr").length == 0)
                        $("#parcours-table-body").append(html);
                    $("#delete-parcours").modal("hide");
                }
                else { 
                    $("#info-message").html('Ce Parcours est lié à ' + data.element)
                    $("#delete-parcours").modal("hide");
                    $("#info-modal").modal("show");
                }
            }
        });
    }

});
