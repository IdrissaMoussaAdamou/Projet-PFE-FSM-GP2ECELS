$(document).ready(function() {
    $('#periode-table').DataTable({
        responsive: true,
         "searching": false,
         "ordering":false,
         "searchable": false,
         "info": false,
         "paging": false,
         "language": {
          "emptyTable": "Aucune Donnée"
        }
    });

    CreateEditTypePeriode = function (Code) { 
        var url = "/TypePeriode/CreateEditTypePeriode?Code=" + Code;
        $('#create-edit-periode-body').load(url, function (param) { 
            var form = $('form#create-periode-form');
            $(form).removeData("validator")    
                   .removeData("unobtrusiveValidation");  
            $.validator.unobtrusive.parse(form);
            
            if(Code !='')
               $('#periode-create').html('Modifier Une Période')
            else
                $('#periode-create').html('Ajouter Une Période')
            $('#create-periode').modal("show");
        })
    }

    $(document).on('click', 'button#submit', function (e) {
        e.preventDefault();    
        var formData = $('form#create-periode-form').serialize();
        if(!$('form#create-periode-form').valid())
            return false;
        
        $.ajax({
            type: "POST",
            url: "/TypePeriode/StoreUpdate",
            data: formData,
            success: function (data) {
                let html = `<tr class="row-color" id="${ "row_" + data.code }">
                                <td>${ data.code }</td>
                                <td>${ data.intituleFr }</td>
                                <td>${ data.intituleAr }</td>
                                <td>${ data.intituleAbrg }</td>
                                <td>${ data.type }</td>
                                <td>${ data.duree }</td>
                                <td class="text-right">
                                    <a href="#" onclick="CreateEditTypePeriode(${ data.code })">
                                        <i class="fa fa-edit fa-2x" style="color: bleue;"></i>
                                    </a>
                                    <a href="#" onclick="ConfirmDelete(${ data.code })">
                                        <i class="fa fa-trash-o b-icon fa-2x" ></i>
                                    </a>   
                                </td>
                            </tr>`
                    if($("#periode-tbody td.dataTables_empty"))
                    {
                        $("#periode-tbody td.dataTables_empty").parent().remove();
                    }
                    if($("#row_" + data.code).length)
                        $("#row_" + data.code).replaceWith(html);
                    else
                    $("#periode-tbody").append(html);

                $('#create-periode').modal("hide");
               
            }
        });
    });

    ConfirmDelete = function(Code) {
        $('#periode-code').val(Code);
        $('#delete-periode').modal('show');
    }

    DeletePeriode = function () {
        var code = $('#periode-code').val();
        $.ajax({
            type: "POST",
            url: "/TypePeriode/Delete",
            data: { Code: code },
            success: function (data) {

                if(data.delete) {
                    let html = `<tr class="odd">
                                <td valign="top" colspan="7" class="dataTables_empty">Aucune Donnée</td>
                            </tr>`;

                    $('#row_' + data.code).remove();
                    if($("#periode-tbody tr").length == 0)
                        $("#periode-tbody").append(html);
                    $('#delete-periode').modal('hide');
                } else { 
                    $("#info-message").html('Ce Type De Période est lié à ' + data.element)
                    $('#delete-periode').modal('hide');
                    $("#info-modal").modal("show");
                }
            }
        });
    }
});