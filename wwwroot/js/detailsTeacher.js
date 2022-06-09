$(document).ready(function () {
   
    function isEditable(idTeacher) {
        let editable;
        $.ajax({
            type: "POST",
            url: "/ChargeEnseignement/CheckEtatSaisie",
            data: { Id : idTeacher },
            async: false,
            success: function (result) {
                editable = result;
            }
        });

        return editable;
    }

    $('#choose-mode input').on('change', function() {
        let mode = $('input:checked', '#choose-mode').val();
        $("mode-container").empty(); 
        $("#mode-container").load("/ChargeEnseignement/ChangeMode?mode=".concat(mode), function () { 
            $("#ch-mode").addClass("col-p30");
        });

        // charge Data
        let codeAnneeUniv = $("#CodeAnneeUniv").val();
        if(codeAnneeUniv != "" && codeAnneeUniv != undefined)
        {
            $.ajax({
                type: "POST",
                url: "/ChargeEnseignement/AnneeUniversitaireTeacherInfos",
                data: { CodeAnneeUniv : codeAnneeUniv },
                success: function (data) {
                    if(data)
                    {
                         // charge
                         construtChargeCalcule(data.teacherCharge, data.teacher)
                        // Etat charge
                        constructEtatCharge(data);
                        // ChargeParModule de l'Enseignement
                        constructChrgEnParModule(data);   
                
                        // ChargeEncadrement de l'Enseignement
                        constructChrgEncadrement(data);
                        
                        // ChargeDiverse de l'Enseignement
                        constructChargeDiverse(data);
                    }              
                }
            });
        }
    })

    $("#TeachersTab").delegate("tr.rows", "click", function(){

        let codeAnneeUniv = $('td:first', $(this)).text();
         
        $.ajax({
            type: "POST",
            url: "/ChargeEnseignement/AnneeUniversitaireTeacherInfos",
            data: { CodeAnneeUniv : codeAnneeUniv},
            success: function (data) {
                
                // Teacher Informations
                $("#TeacherFirstName").html(data.teacher.nom);
                $("#TeacherLastName").html(data.teacher.prenom);
                $("#TeacherCIN").html(data.teacher.cin);
                $("#TeacherStatut").html(data.teacher.statut);
                $("#TeacherGrade").html(data.teacher.grade);
                $("#TeacherDept").html(data.teacher.intituleFrDepartement);
                
                // Etat charge
                constructEtatCharge(data);

                // save teacher id and the year code
                $("#TeacherId").val(data.teacher.id);
                $("#CodeAnneeUniv").val(data.teacher.codeAnneeUniv);
                
                // charge
                construtChargeCalcule(data.teacherCharge, data.teacher)
                // ChargeParModule de l'Enseignement
                constructChrgEnParModule(data);   
                
                // ChargeEncadrement de l'Enseignement
                constructChrgEncadrement(data);
                

                // ChargeDiverse de l'Enseignement
                constructChargeDiverse(data);

            }
        });
    });
    
    function constructChrgEnParModule(data) {
        if(data.teacherChrgEnParModule)
        {
            $("#enseignement-tab-tbody").empty();
            let html = "";
            let periode;
            data.teacherChrgEnParModule.forEach(chgEnParModule => {
                
                html += `<tr class="row-color" id="${ "row-" + chgEnParModule.id }">
                            <td>${ chgEnParModule.intituleFrModule }</td>
                            <td>${ chgEnParModule.intituleAbrgNiveau }</td>
                            <td>${ chgEnParModule.periode.concat(chgEnParModule.numPeriodeDansAnnee) }</td>
                            <td>${ chgEnParModule.volumeHebdoParGroupe }</td>
                            <td>${ chgEnParModule.natureEnseignement }</td>
                            <td>${ chgEnParModule.nbGroupes }</td>
                            <td>${ chgEnParModule.nbSemainesPeriode }</td>
                            <td class="text-right">
                                <a href="#" onclick="CreateEditChargeEParModule(${ chgEnParModule.id })">
                                    <i class="fa fa-edit fa-2x" style="color: #337ab7;" title="Modifier"></i>
                                </a>
                                <a href="#" onclick="confirmDeleteChargEnParModule(${ chgEnParModule.id })">
                                    <i class="fa fa-trash-o b-icon fa-2x"  title="Supprimer"></i>
                                </a>
                            </td>      
                        </tr>`        
            });
            $("#enseignement-tab-tbody").append(html);
        }    
    }

    function constructChrgEncadrement(data) {
        if(data.teacherChrgEncadrement)
        {
            $("#encadrement-tab-tbody").empty();
            let html = "";
            data.teacherChrgEncadrement.forEach(chrgEncadrement => {
                
                html += `<tr class="row-color" id="${ "Row_" + chrgEncadrement.id }">
                           <td>${ chrgEncadrement.typeEncad.libelle }</td>
                           <td>${ chrgEncadrement.typeEncad.cycle }</td>
                           <td>${ chrgEncadrement.typeEncad.natureCharge }</td>
                           <td>${ chrgEncadrement.typeEncad.volumeHebdoCharge }</td>
                           <td>${ chrgEncadrement.typeEncad.periode }</td>
                           <td>${ chrgEncadrement.nbEncadrements }</td>
                           <td class="text-right">
                               <a href="#" onclick="createEditEncadrement(${ chrgEncadrement.id })">
                                   <i class="fa fa-edit fa-2x" style="color: #337ab7;" title="Modifier"></i>
                               </a>
                               <a href="#" onclick="confirmDeleteChargeEncadrement(${ chrgEncadrement.id })">
                                   <i class="fa fa-trash-o b-icon fa-2x"  title="Supprimer"></i>
                               </a>
                           </td>
                       </tr>`        
            });
            $("#encadrement-tab-tbody").append(html);
        }
    }

    function constructChargeDiverse(data) {
        if(data.teacherChrgDivers)
        {
            $("#charge-divers-tbody").empty();
           let html = "";
            data.teacherChrgDivers.forEach(ChargeDiverse => {
                
                html += `<tr class="row-color" id="${ "Row-" + ChargeDiverse.id }">
                           <td>${ ChargeDiverse.natureCharge }</td>
                           <td>${ ChargeDiverse.periode.concat(ChargeDiverse.numPeriodeDansAnnee) }</td>
                           <td>${ ChargeDiverse.volume }</td>
                           <td>${ ChargeDiverse.nbSemainesPeriode }</td>
                           <td>${ ChargeDiverse.uniteVolume }</td>
                           <td class="text-right">
                               <a href="#" onclick="createEditChargeDiverse(${ ChargeDiverse.id })">
                                   <i class="fa fa-edit fa-2x" style="color: #337ab7;" title="Modifier"></i>
                               </a>
                               <a href="#" onclick="confirmDeleteChargeDiverse(${ ChargeDiverse.id })">
                                   <i class="fa fa-trash-o b-icon fa-2x"  title="Supprimer"></i>
                               </a>
                           </td>
                       </tr>`        
            });
            $("#charge-divers-tbody").append(html);
        }
    }

    function constructEtatCharge(data) {
        let htmlEtat = "";
        if(data.teacher.validationChargeAdministration =="Validee") {
            htmlEtat = '<span id="etat-charge" class="text-danger"><i class="fa fa-check-circle fa-2x g-check"></i></span>';
        } else if(data.teacher.validationChargeAdministration =="Non Validee" && data.teacher.validationChargeDepartement =="Validee") {
            htmlEtat =  '<span id="etat-charge" class="text-danger"><i class="fa fa-check-circle fa-2x y-check"></i></span>';
        } else if(data.teacher.etatSaisie == "Verrouillee") {
            htmlEtat = '<span id="etat-charge" class="text-danger"><i class="fa fa-check-circle fa-2x r-check"></i></span>';
        } else if(data.teacher.etatSaisie == "Verifiee"){
            htmlEtat = '<span id="etat-charge" class="text-danger"><i class="fa fa-check-circle fa-2x o-check"></i></span>';
        } else {
            htmlEtat = '<span id="etat-charge" class="text-danger"><i class="fa fa-check-circle fa-2x b-icon"></i></span>';
        }
        $("#etat-charge").replaceWith(htmlEtat);
    }

    // ChargeParModule

    CreateEditChargeEParModule = function(id) {
        let teacherId =  $("#TeacherId").val();
        if(teacherId > 0) {
            let editable = isEditable(teacherId)

            if(editable) {
                $("#anneeuniv-enseignement-body").load("/ChargeEnseignement/CreateEditChargeEParModule?Id=".concat("", teacherId).concat(",", id), function () {
                    let form = $('form#anneeuniv-enseignement-form');
                    $(form).removeData("validator")    
                           .removeData("unobtrusiveValidation");  
                    $.validator.unobtrusive.parse(form);
                    
                    if(id > 0)
                       $('#anneeuniv-enseignement-title').html('Modifier Une Charge D\ Enseignement Par Module')
                    else
                        $('#anneeuniv-enseignement-title').html('Ajouter Une Charge D\ Enseignement Par Module')
                    $("#anneeuniv-enseignement-modal").modal("show");
                })
            } else {
                showInfo();
            }
            
        }
    }

    $(document).on("change", "#IdFiliere", function (e) { 
        e.preventDefault();
        let idFiliere = $("#IdFiliere").find(":selected").val();
        if(idFiliere == "none") {
            $("#IdNiveau").empty();
            $("#PeriodeAnnee").empty();
            $("#IdModule").empty();
        } else {
            $.ajax({
                type: "POST",
                url: "/ChargeEnseignement/GetNiveauxByFiliere",
                data: { IdFiliere : idFiliere},
                success: function (data) {
                    $("#IdNiveau").empty();

                    let html = `<option value="none"> - - - - - - - - - - - - </option>`;
                    data.listNiveaux.forEach((niveau) => {
                        html += `<option value="${ niveau.id }">${ niveau.intituleAbrg }</option>`;
                    })
                    $("#IdNiveau").append(html);
                    
                    html = null;
                    if(data.filiere.typePeriode == "Semestre") {
                        html = `<option value="Semestre1">Semestre 1</option>
                                <option value="Semestre2">Semestre 2</option>`;

                    } else if(data.filiere.typePeriode == "Trimestre") {
                        html = `<option value="Trimestre1">Trimestre 1</option>
                                <option value="Trimestre2">Trimestre 2</option>
                                <option value="Trimestre3">Trimestre 3</option>`;
                                
                    }else {
                        html = `<option value="Quater1">Quater 1</option>
                                <option value="Quater2">Quater 2</option>
                                <option value="Quater3">Quater 3</option>
                                <option value="Quater4">Quater 4</option>`;
                                
                    }

                    $("#PeriodeAnnee").append(html);
                }
            });
        }
    
    })

    $(document).on("change", "#IdNiveau",  function (e) { 
        e.preventDefault();
 
        let idNiveau = $("#IdNiveau").find(":selected").val();
        let codeAnneeUniv = $("#CodeAnneeUniv").val();
 
        if( idNiveau == "none")
            $("#IdModule").empty();
        else {
                 $.ajax({
                     type: "POST",
                     url: "/ChargeEnseignement/GetModulesByNiveauPeriode",
                     data: { IdNiveau : idNiveau, CodeAnneeUniv : codeAnneeUniv },
                     success: function (data) {
                         let html = "";
                         $("#IdModule").empty();
 
                         data.forEach((module) => {
                             html += `<option value="${ module.idModule }">${ module.intituleFr }</option>`
                         });
                         $("#IdModule").append(html);
                     }
                 });
             }
    })
   
    $(document).on("click", "#anneeuniv-enseignement-submit", function (e) { 
        e.preventDefault();
        if(!$("#anneeuniv-enseignement-form").valid())
            return false;
        $("#codeAnneeuniv").val($("#CodeAnneeUniv").val());
        $("#IdAUEnseignant").val($("#TeacherId").val());

        let formData = $("#anneeuniv-enseignement-form").serialize();

        $.ajax({
            type: "POST",
            url: "/ChargeEnseignement/StoreUpdateChargeParModule",
            data: formData,
            success: function (chgEnParModule) {
                if(chgEnParModule != null)
                {
                    let html = `<tr class="row-color" id="${ "row-" + chgEnParModule.id }">
                                    <td>${ chgEnParModule.intituleFrModule }</td>
                                    <td>${ chgEnParModule.intituleAbrgNiveau }</td>
                                    <td>${ chgEnParModule.periode.concat(chgEnParModule.numPeriodeDansAnnee) }</td>
                                    <td>${ chgEnParModule.volumeHebdoParGroupe }</td>
                                    <td>${ chgEnParModule.natureEnseignement }</td>
                                    <td>${ chgEnParModule.nbGroupes }</td>
                                    <td>${ chgEnParModule.nbSemainesPeriode }</td>
                                    <td class="text-right">
                                        <a href="#" onclick="CreateEditChargeEParModule(${ chgEnParModule.id })">
                                            <i class="fa fa-edit fa-2x" style="color: #337ab7;" title="Modifier"></i>
                                        </a>
                                        <a href="#" onclick="confirmDeleteChargEnParModule(${ chgEnParModule.id })">
                                            <i class="fa fa-trash-o b-icon fa-2x"  title="Supprimer"></i>
                                        </a>
                                    </td>      
                    </tr>`
                        
                    if($("#row-" + chgEnParModule.id).length)
                        $("#row-" + chgEnParModule.id).replaceWith(html); 
                    else
                        $("#enseignement-tab-tbody").append(html);
                }
                $("#anneeuniv-enseignement-modal").modal("hide");
            }
        });
    })

    confirmDeleteChargEnParModule = function(id)
    {
        let teacherId =  $("#TeacherId").val();
        let editable = isEditable(teacherId)

        if(editable) {
            $("#ChargeEnModuleId").val(id);
            $("#delete-chargeEnModule").modal("show");
        } else {
            showInfo();
        }
    }

    DeleteChargEnParModule = function()
    {
        let teacherId =  $("#TeacherId").val();
        let id = $("#ChargeEnModuleId").val();
        $.ajax({
            type: "POST",
            url: "/ChargeEnseignement/DeleteChargeParModule",
            data: { Id : id , IdTeacher : teacherId},
            success: function (data) {
                if(data.delete) {
                    $("#row-" + data.id).remove();
                    $("#delete-chargeEnModule").modal("hide");
                } else {
                    $("#delete-chargeEnModule").modal("hide");
                    showInfo();
                } 
            }
        });
    }
    
    // ChargeParModule
    createEditEncadrement = function(id)
    {
        let teacherId =  $("#TeacherId").val();
        if(teacherId > 0)     
        {
            let editable = isEditable(teacherId)
            if(editable)
            {
                $("#charge-encadrement-body").load("/ChargeEnseignement/CreateEditChargeEncadrement?Ids=".concat(teacherId,",",id), function () {
                    let form = $('form#charge-encadrement-form');
                    $(form).removeData("validator")    
                        .removeData("unobtrusiveValidation");  
                    $.validator.unobtrusive.parse(form);
    
                    if(id > 0)
                    $('#charge-encadrement-title').html('Modifier Une Charge D\'Encadrement')
                    else
                    $('#charge-encadrement-title').html('Ajouter Une Charge D\'Encadrement')
                    $("#charge-encadrement-modal").modal("show");
                });
            }
            else
            {
                showInfo();
            }
        }
    }

    $(document).on("click", "#submit-charge-encadrement", function (e) { 
        e.preventDefault();
        if(!$("#charge-encadrement-form").valid())
            return false;

        $("#codeAnnee").val($("#CodeAnneeUniv").val());
        $("#IdAUEnseignant").val($("#TeacherId").val());
        let formData = $("#charge-encadrement-form").serialize();

        $.ajax({
            type: "POST",
            url: "/ChargeEnseignement/StoreUpdateChargeEncadrement",
            data: formData,
            success: function (chrgEncadrement) {
                if(chrgEncadrement != null)
                {
                    let html =  `<tr class="row-color" id="${ "Row_" + chrgEncadrement.id }">
                                    <td>${ chrgEncadrement.typeEncad.libelle }</td>
                                    <td>${ chrgEncadrement.typeEncad.cycle }</td>
                                    <td>${ chrgEncadrement.typeEncad.natureCharge }</td>
                                    <td>${ chrgEncadrement.typeEncad.volumeHebdoCharge }</td>
                                    <td>${ chrgEncadrement.typeEncad.periode }</td>
                                    <td>${ chrgEncadrement.nbEncadrements }</td>
                                    <td class="text-right">
                                        <a href="#" onclick="createEditEncadrement(${ chrgEncadrement.id })">
                                            <i class="fa fa-edit fa-2x" style="color: #337ab7;" title="Modifier"></i>
                                        </a>
                                        <a href="#" onclick="confirmDeleteChargeEncadrement(${ chrgEncadrement.id })">
                                            <i class="fa fa-trash-o b-icon fa-2x"  title="Supprimer"></i>
                                        </a>
                                    </td>
                                </tr>`
                    if($("#Row_" + chrgEncadrement.id).length)
                        $("#Row_" + chrgEncadrement.id).replaceWith(html);
                    else
                        $("#encadrement-tab-tbody").append(html);
                    $("#charge-encadrement-modal").modal("hide");
                }
                $("#charge-encadrement-modal").modal("hide");
            }
        });

    })

    confirmDeleteChargeEncadrement = function(id)
    {
        let teacherId =  $("#TeacherId").val();
        let editable = isEditable(teacherId)

        if(editable) {
            $("#ChargeEndrementId").val(id);
            $("#delete-charge-encadrement").modal("show");
        } else {
            showInfo()
        }
    }

    deleteChargeEncadrement = function()
    {
        let teacherId =  $("#TeacherId").val();
        let id = $("#ChargeEndrementId").val();
        $.ajax({
            type: "POST",
            url: "/ChargeEnseignement/DeleteChargeEncadrement",
            data: { Id : id, IdTeacher : teacherId },
            success: function (data) {
                if(data.delete) {
                    $("#Row_" + data.id).remove();
                    $("#delete-charge-encadrement").modal("hide");
                } else {
                    $("#delete-charge-encadrement").modal("hide");
                    showInfo();
                }
            }
        });
    }

    // ChargeDiverse
    createEditChargeDiverse = function(id)
    {
        let teacherId =  $("#TeacherId").val();
        if(teacherId > 0) {
            let editable = isEditable(teacherId)
            if(editable) {
                $("#charge-divers-body").load("/ChargeEnseignement/CreateEditChargeDiverse?Ids=".concat(teacherId,",",id), function () {
                    let form = $('form#charge-divers-form');
                    $(form).removeData("validator")    
                        .removeData("unobtrusiveValidation");  
                    $.validator.unobtrusive.parse(form);
    
                    if(id > 0)
                        $('#charge-divers-title').html('Modifier Une Charge Divers')
                    else
                        $('#charge-divers-title').html('Ajouter Une Charge Divers')
                        $("#charge-divers-modal").modal("show");
                    });
            } else {
                showInfo();
            }
        }
    }

    $(document).on("click", "#charge-divers-submit", function (e) { 
        e.preventDefault();
        if(!$("#charge-divers-form").valid())
        return false;

        $("#AnneeUniv").val($("#CodeAnneeUniv").val());
        $("#IdAUEnseignant").val($("#TeacherId").val());
        let formData = $("#charge-divers-form").serialize();

        $.ajax({
        type: "POST",
        url: "/ChargeEnseignement/StoreUpdateChargeDiverse",
        data: formData,
        success: function (ChargeDiverse) {
            if(ChargeDiverse != null)
            {
                let html =  `<tr class="row-color" id="${ "Row-" + ChargeDiverse.id }">
                                <td>${ ChargeDiverse.natureCharge }</td>
                                <td>${ ChargeDiverse.periode.concat(ChargeDiverse.numPeriodeDansAnnee) }</td>
                                <td>${ ChargeDiverse.volume }</td>
                                <td>${ ChargeDiverse.nbSemainesPeriode }</td>
                                <td>${ ChargeDiverse.uniteVolume }</td>
                                <td class="text-right">
                                    <a href="#" onclick="createEditChargeDiverse(${ ChargeDiverse.id })">
                                        <i class="fa fa-edit fa-2x" style="color: #337ab7;" title="Modifier"></i>
                                    </a>
                                    <a href="#" onclick="confirmDeleteChargeDiverse(${ ChargeDiverse.id })">
                                        <i class="fa fa-trash-o b-icon fa-2x"  title="Supprimer"></i>
                                    </a>
                                </td>
                            </tr>`
                    if($("#Row-" + ChargeDiverse.id).length)
                        $("#Row-" + ChargeDiverse.id).replaceWith(html);
                    else
                        $("#charge-divers-tbody").append(html);
                    $("#charge-divers-modal").modal("hide");
            }
            $("#charge-divers-modal").modal("hide");
            }
        });

    })

    confirmDeleteChargeDiverse = function(id)
    {
        let teacherId =  $("#TeacherId").val();
        let editable = isEditable(teacherId);
        if(editable) {
            $("#ChargeDiverseId").val(id);
            $("#delete-charge-divers").modal("show");
        } else {
            showInfo();
        }
    }

    deleteChargeDiverse = function()
    {
        let teacherId =  $("#TeacherId").val();
        let id = $("#ChargeDiverseId").val();
        $.ajax({
            type: "POST",
            url: "/ChargeEnseignement/DeleteChargeDiverse",
            data: { Id : id , IdTeacher: teacherId },
            success: function (data) {
                if(data.delete){
                    $("#Row-" + data.id).remove();
                    $("#delete-charge-divers").modal("hide");
                } else {
                    $("#delete-charge-divers").modal("hide");
                    showInfo();
                }
            }
        });
    }

    function showInfo() {
        $("#info-title").html('<i class="fa fa-info-circle b-icon fa-lg"></i>'.concat(" ", "Charge Vérouillée"));
        $("#info-message").html("La charge d'enseignement est vérrouillée, Veuillez Contacter votre Département de tutelle")
        $("#info-action").html("Ok");
        $("#info-modal").modal("show");
    }

    construtChargeCalcule = function(teacherCharge, teacher)
    {
        $("#charge-calculee").empty();
        if(teacherCharge != null && teacherCharge.length >0)
        {
            let html ="";
            teacherCharge.forEach( (charge, index) => {
                if(index < 2)
                {
                    html += `<tr class="row-color">
                                <td>${ charge.periode.concat(" ", charge.numPeriodeDansAnnee) }</td>
                                <td>${ charge.volumeCours }</td>
                                <td>${ charge.volumeTD }</td>
                                <td>${ charge.volumeTP }</td>
                            </tr>`
                }
                else
                {
                    html += `<tr class="row-color">
                                <td>Année</td>
                                <td>${ charge.volumeCours }</td>
                                <td>${ charge.volumeTD }</td>
                                <td>${ charge.volumeTP }</td>
                            </tr>`
                    if(teacher.statut == "Permanent" || teacher.statut == "Contractuel") {
                        html += `<tr class="row-color">
                            <td>Heure D'Enseignement Supplémentaire</td>
                            <td>${ charge.volumeSuppCours }</td>
                            <td>${ charge.volumeSuppTD }</td>
                            <td>${ charge.volumeSuppTP }</td>
                        </tr>`
                    }
                }

            })

            $("#charge-calculee").append(html);
        }
        else
        {
            let html = `<tr class="row-color">
                        <td>Semestre 1</td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr class="row-color">
                        <td>Semestre 2</td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr class="row-color">
                        <td>Année</td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>`
            if(teacher.statut == "Permanent" || teacher.statut == "Contractuel") {
                html += `<tr class="row-color">
                            <td>Heure D'Enseignement Supplémentaire</td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>`
            }
            $("#charge-calculee").append(html);
        }
    }
});