
$(document).ready(function () {
    $('#teacher-datable').DataTable({
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
            { data: 'Nom & Prénom' }, 
            { data: 'Grade' }, 
            { data: 'Statut'},
            { data: 'Email'},
            { data: 'Département'},
            { data: 'Actions' } 
         ],
         'columnDefs': [ {
            'targets': [2,3,5],
            'orderable': false,
            'searchable': false,
         }]
        
    });

    CreateEditTeacher = function (id) {
        let url = "/Enseignant/CreateEditEnseignant?Id=" + id;

        $("#teacher-modal-body").load(url, function () { 
            var form = $('form#teacher-form');
            $(form).removeData("validator")    
            .removeData("unobtrusiveValidation");   
            $.validator.unobtrusive.parse(form);
            if(id != -1)
               $('#create-teacher').html('Modifier Les Informations De L\' Enseignant')
            else
                $('#create-teacher').html('Ajouter Un Enseignant')
            $("#teacher-create").modal("show");
        })
        
    }

    $(document).on("click", "#submit", function (e) { 
        e.preventDefault();    
        let formData = $('form#teacher-form').serialize();
        
        if(!$('form#teacher-form').valid())
            return false;
            
        $.ajax({
            type: "POST",
            url: "/Enseignant/StoreUpdate",
            data: formData,
            success: function (teacher) {
                debugger
                let html = `<tr class="row-color" id="${ "row_" + teacher.id }">
                            <td>${ teacher.nom + " " + teacher.prenom }</td>
                            <td>${ teacher.grade }</td>
                            <td>${ teacher.statut }</td>
                            <td>${ teacher.email1 }</td>
                            <td>${ teacher.intituleFrDepartement}</td>
                            <td class="text-right" >
                                <a href="#" onclick="ShowTeacher(${ teacher.id })">
                                    <i class="fa fa-eye b-icon fa-2x "  title="Consulter"></i>
                                </a>
                                <a href="#" onclick="CreateEditTeacher(${ teacher.id })">
                                    <i class="fa fa-edit fa-2x" style="color: #337ab7;" title="Modifier"></i>
                                </a>
                                <a href="#" onclick="ConfirmDeleteTeacher(${ teacher.id })">
                                    <i class="fa fa-trash-o b-icon fa-2x"  title="Supprimer"></i>
                                </a>
                            </td>
                        </tr>`
                if($("td.dataTables_empty"))
                {
                    $("td.dataTables_empty").parent().remove();  
                }
                if($("#row_" + teacher.id).length)
                    $("#row_" + teacher.id).replaceWith(html);
                else
                    $("#teacher-table-body").append(html);
            
                $("#teacher-create").modal("hide");
            }
        });
    })

    ConfirmDeleteTeacher = function (id) { 
        $("#teacherId").val(id);
        $("#delete-teacher").modal("show");
        
    }

    DeleteTeacher = function () { 
        let id =  $("#teacherId").val();
        $.ajax({
            type: "POST",
            url: "/Enseignant/Delete",
            data: { Id : id },
            success: function (data) {
                if(data.delete) {
                    $("#row_" + data.id).remove();
                    var html = `<tr class="odd">
                                    <td valign="top" colspan="6" class="dataTables_empty">Aucune Donnée</td>
                                </tr>`;
                    if($("#teacher-table-body tr").length == 0)
                    $("#teacher-table-body").append(html);
                    
                    $("#delete-teacher").modal("hide");
                }else {
                    $("#info-message").html('Cet Enseignant est lié à ' + data.element)
                    $("#delete-teacher").modal("hide");
                    $("#info-modal").modal("show");
                }

            }
        });
    }
    ShowTeacher = function (id) { 
        let url = "/Enseignant/Show?Id=" + id;
        $("#teacher-info").load(url, function () { 
            $("#show-teacher").modal("show");
        }) 
    }
});