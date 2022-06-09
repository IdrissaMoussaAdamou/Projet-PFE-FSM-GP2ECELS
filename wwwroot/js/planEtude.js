$(document).ready(function() {
    $('#unite-table, #module-table').DataTable({
        "searching": false,
        "ordering":false,
        "info": false,
        "paging": false,
        responsive: false,
        "language": {
            'emptyTable': "Aucune Donnée"
        },
      
    });

    $("#codeFiliere").change(function (e) { 
        e.preventDefault();
        var idFiliere = $("#codeFiliere").val();
        $.ajax({
            type: "POST",
            url: "/Home/PlanEtude",
            data: { IdFiliere : idFiliere},
            success: function (data) {

                //Afficher les Niveaux
                var niveauItem = "";
                
                $("#codeNiveau").empty();
                data.listNiveaux.forEach((niveau) => {
                    niveauItem += '<option value="' + niveau.id + '">' + niveau.intituleAbrg + '</option>'
                })
                $("#codeNiveau").html(niveauItem);
                
                //Afficher les Parcours
                var parcoursItem = "";

                $("#codeParcours").empty();
                data.listParcours.forEach((parcours) => {
                    parcoursItem += '<option value="' + parcours.id + '">' + parcours.intituleFr + '</option>'
                })
                $("#codeParcours").html(parcoursItem);  
            }
        });
    });

    // Module
    CreateEditModule = function (idModule) { 
    
        var idFiliere = $("#codeFiliere").find(":selected").val();
        var idNiveau = $("#codeNiveau").find(":selected").val();
        var idParcours = $("#codeParcours").find(":selected").val();
        var periode = $("#codePeriode").find(":selected").val();
        var codes = idFiliere + "," + idNiveau + "," + idParcours + "," + periode + "," + idModule;
        
        var url = "/Module/CreateEditModule?Code=" + codes ;
        $("#module-body").load(url, function(){
            var form = $('form#module-form');
                $(form).removeData("validator")    
                       .removeData("unobtrusiveValidation");  
                $.validator.unobtrusive.parse(form);
                if(idModule != -1)
                   $('#niveau-module-label').html('Modifier Un Module')
                else
                    $('#niveau-module-label').html('Ajouter Un Module')
                $('#niveau-module').modal("show");
        })

    }

    $(document).on("click", "button#submit-module", function (e) { 
        e.preventDefault();
        if(!$('form#module-form').valid())
                return false;    
            
        var formData = $('form#module-form').serialize();
        $.ajax({
            type: "POST",
            url: "/Module/StoreUpdate",
            data: formData,
            success: function (module) {
                let html = `<tr class="row-color" id="${ "row_" + module.idModule }">
                                <td>${ module.code }</td>
                                <td>${ module.intituleFr }</td>
                                <td>${ module.intituleAbrg }</td>
                                <td>${ module.periode }</td>
                                <td>${ module.intileFrUniteE }</td>
                                <td class="text-right">
                                    <a href="#" onclick="ShowModule(${ module.idModule })">
                                        <i class="fa fa-eye b-icon fa-2x  text-success" title="Consulter"></i>
                                    </a>
                                    <a href="#" onclick="CreateEditModule(${ module.idModule })">
                                        <i class="fa fa-edit fa-2x" style="color: #337ab7;"
                                            title="Modifier"></i>
                                    </a>
                                    <a href="#"
                                        onclick="ConfirmDeleteModule(${ module.idModule })">
                                        <i class="fa fa-trash-o b-icon fa-2x" title="Supprimer"></i>
                                    </a>
                                </td>
                            </tr>`
                if($("#module-table-body td.dataTables_empty"))
                {
                    $("#module-table-body td.dataTables_empty").parent().remove();
                }
                if($("#row_" + module.idModule).length)
                    $("#row_" + module.idModule).replaceWith(html);
                else
                    $("#module-table-body").append(html);
                $('#niveau-module').modal("hide");
                            
            }
        });
    })

    ConfirmDeleteModule = function (idModule) { 
       
        $("#hidden-codeModule").val(idModule);
        $("#delete-module").modal("show");
    }

    DeleteModule = function(){
        var idModule = $("#hidden-codeModule").val();
       
        $.ajax({
            type: "POST",
            url: "/Module/Delete",
            data: { Id : idModule},
            success: function (data) {
                
                if(data.delete) {
                    var html = `<tr class="odd">
                                <td valign="top" colspan="6" class="dataTables_empty">Aucune Donnée</td>
                            </tr>`;
                    $("#row_" + data.id).remove();
                    
                    if($("#module-table-body tr").length == 0)
                        $("#module-table-body").append(html);
                    $("#delete-module").modal("hide");
                
                }
                else { 
                    $("#info-message").html('Ce Module est lié à ' + data.element)
                    $("#delete-module").modal("hide");
                    $("#info-modal").modal("show");
                }
            }
        });
    }
    
    ShowModule = function(idModule){
        var url = "/Module/Show?Id=" + idModule;
        $("#module-info").load(url,function () { 
            $("#show-module").modal("show");
        })
    }

    // Unite D'Enseignement

    CreateEditUnite = function (idUnite) { 
    
        var idFiliere = $("#codeFiliere").find(":selected").val();
        var idNiveau = $("#codeNiveau").find(":selected").val();
        var idParcours = $("#codeParcours").find(":selected").val();
        var Periode = $("#codePeriode").find(":selected").val();
        var codes = idFiliere + "," + idNiveau + "," + idParcours + "," + Periode + "," + idUnite;

        var url = "/UEnseignement/CreateEditUnite?Code=" + codes ;
        $("#unite-body").load(url, function(){
            var form = $('form#unite-form');
                $(form).removeData("validator")    
                       .removeData("unobtrusiveValidation");  
                $.validator.unobtrusive.parse(form);
                if(idUnite != -1)
                   $('#unite-label').html('Modifier Une Unité D\'Enseignement')
                else
                    $('#unite-label').html('Ajouter Une Unité D\'Enseignement')
                $('#create-unite').modal("show");
        })

    }

    $(document).on("click", "button#submit-unite", function (e) { 
        e.preventDefault();
        if(!$('form#unite-form').valid())
                return false;    
            
        var formData = $('form#unite-form').serialize();
        $.ajax({
            type: "POST",
            url: "/UEnseignement/StoreUpdate",
            data: formData,
            success: function (unite) {
                let html = `<tr class="row-color" id="${ "row-" + unite.idUniteEnseignement }">
                                <td>${ unite.code }</td>
                                <td>${ unite.intituleFr }</td>
                                <td>${ unite.intituleAbrg }</td>
                                <td>${ unite.periode }</td>
                                <td>${ unite.nature }</td>
                                <td class="text-right">
                                    <a href="#" onclick="ShowUnite(${ unite.idUniteEnseignement })">
                                        <i class="fa fa-eye b-icon fa-2x  text-success" title="Consulter"></i>
                                    </a>
                                    <a href="#" onclick="CreateEditUnite(${ unite.idUniteEnseignement })">
                                        <i class="fa fa-edit fa-2x" style="color: #337ab7;"></i>
                                    </a>
                                    <a href="#" onclick="ConfirmDeleteUnite(${ unite.idUniteEnseignement })">
                                        <i class="fa fa-trash-o b-icon fa-2x" ></i>
                                    </a>
                                </td>
                            </tr>`
                if($("#unite-table-body td.dataTables_empty"))
                {
                    $("#unite-table-body td.dataTables_empty").parent().remove();
                }
                if($("#row-" + unite.idUniteEnseignement).length)
                    $("#row-" + unite.idUniteEnseignement).replaceWith(html);
                else
                    $("#unite-table-body").append(html);
                    $('#create-unite').modal("hide");
            }
        });
    })

    ConfirmDeleteUnite = function (idUnite) { 
        $("#hidden-codeUnite").val(idUnite);
        $("#delete-unite").modal("show");
    }

    DeleteUnite = function(){
        let idUnite = $("#hidden-codeUnite").val();
        $.ajax({
            type: "POST",
            url: "/UEnseignement/Delete",
            data: { IdUniteEnseignement: idUnite },
            success: function (data) {

                if(data.delete) {
                    var html = `<tr class="odd">
                                <td valign="top" colspan="6" class="dataTables_empty">Aucune Donnée</td>
                            </tr>`;
                    $("#row-"+ data.id).remove();
                    
                    if($("#unite-table-body tr").length == 0)
                        $("#unite-table-body").append(html);
                    $("#delete-unite").modal("hide");
                }
                else { 
                    $("#info-message").html('Cette Unité D\'Enseignement est liée à ' + data.element)
                    $("#delete-unite").modal("hide");
                    $("#info-modal").modal("show");
                }
                
            }
        });
    }
    
    ShowUnite = function(idUniteEnseignement){
        var url = "/UEnseignement/Show?Id=" + idUniteEnseignement ;
        $("#unite-info").load(url,function () { 
            $("#show-unite").modal("show");
        })
    }

});