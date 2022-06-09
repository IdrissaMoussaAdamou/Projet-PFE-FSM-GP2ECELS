$(document).ready(function(){


    // TypeEncadrement

    CreateEditTypeEncadrement = function(id) {
        let url = "/TypeEncadrement/CreateEdit?Id=" + id;
            $("#TypeEncadrementModalBody").load(url, function(){
                let form = $('form#type-encadrement-from');

                $(form).removeData("validator")    
                    .removeData("unobtrusiveValidation");  
                $.validator.unobtrusive.parse(form);
                if(id != -1)
                   $('#type-title').html('Modifier Un Type  D\'EnCadrement')
                else
                    $('#type-title').html('Ajouter  Un Type  D\'EnCadrement')
                $("#TypeEncadrementModal").modal("show");
            })
    }

    $(document).on("click", "#submit-typeEncadrement", function (e) { 
        e.preventDefault();
        if(!$("#type-encadrement-from").valid())
            return false;
        let formData = $("#type-encadrement-from").serialize();
        $.ajax({
            type: "Post",
            url: "/TypeEncadrement/StoreUpdate",
            data: formData,
            success: function (tyEncadrement) {
                let html = `<tr class="row-color" id="${ "row_" + tyEncadrement.id }">
                                    <td>${ tyEncadrement.libelle }</td>
                                    <td>${ tyEncadrement.cycle }</td>
                                    <td>${ tyEncadrement.natureCharge }</td>
                                    <td>${ tyEncadrement.volumeHebdoCharge }</td>
                                    <td>${ tyEncadrement.periode.concat(tyEncadrement.numPeriodeDansAnnee) }</td>
                                    <td>${ tyEncadrement.nbSemainesPeriode}</td>
                                    <td class="text-center">
                                        <a href="#" onclick="CreateEditTypeEncadrement(${ tyEncadrement.id })">
                                            <i class="fa fa-edit fa-2x" style="color: #337ab7;" title="Modifier"></i>
                                        </a>
                                        <a href="#" onclick="ConfirmDeleteTypeEncadrement(${ tyEncadrement.id })">
                                            <i class="fa fa-trash-o b-icon fa-2x"  title="Supprimer"></i>
                                        </a>
                                    </td>
                                </tr>`
                if($("#row_" + tyEncadrement.id).length)
                    $("#row_" + tyEncadrement.id).replaceWith(html);
                else
                    $("#encadrement-table-body").append(html);
                $("#TypeEncadrementModal").modal("hide");
            }
        });
    })

    ConfirmDeleteTypeEncadrement = function (id) { 
        $("#IdTypeEncadrement").val(id);
        $("#DeleteTypeEncadrement").modal("show");
    }

    DeleteTypeEncadrement = function () { 
        let id = $("#IdTypeEncadrement").val();
        $.ajax({
            type: "POST",
            url: "/TypeEncadrement/Delete",
            data: { Id : id },
            success: function (data) {
                if(data.delete) {
                    $("#row_" + data.id).remove();
                    $("#DeleteTypeEncadrement").modal("hide");
                }
                else { 
                    $("#info-message").html('Cet Type D\'Encadrement est lié à ' + data.element)
                    $("#DeleteTypeEncadrement").modal("hide");
                    $("#info-modal").modal("show");
                }
            }
        });
    }

    // Type Charges Divers
    CreateEditTypeChargeDiverse = function(id) {
        let url = "/TypeEncadrement/CreateEditTypeChargeDiverse?Id=" + id;
            $("#TypeChargeDiverseModalBody").load(url, function(){
                let form = $('form#TypeChargeDiverse-Form');

                $(form).removeData("validator")    
                    .removeData("unobtrusiveValidation");  
                $.validator.unobtrusive.parse(form);
                if(id != -1)
                   $('#typecharge-title').html('Modifier Un Type  De Charge Divers')
                else
                    $('#typecharge-title').html('Ajouter Un Type  De Charge Divers')
                $("#TypeChargeDiverseModal").modal("show");
            })
    }

    $(document).on("click", "#submit-typeChargeDiverse", function (e) { 
        e.preventDefault();
        if(!$("#TypeChargeDiverse-Form").valid())
            return false;
        let formData = $("#TypeChargeDiverse-Form").serialize();
        $.ajax({
            type: "Post",
            url: "/TypeEncadrement/StoreUpdateTypeChargeDiverse",
            data: formData,
            success: function (tyChrgDivers) {
                let html = `<tr class="row-color" id="${ "row-" + tyChrgDivers.id }">
                                    <td>${ tyChrgDivers.libelle }</td>
                                    <td class="text-center">
                                        <a href="#" onclick="CreateEditTypeChargeDiverse(${ tyChrgDivers.id })">
                                            <i class="fa fa-edit fa-2x" style="color: #337ab7;" title="Modifier"></i>
                                        </a>
                                        <a href="#" onclick="ConfirmDeleteTypeChargeDiverse(${ tyChrgDivers.id })">
                                            <i class="fa fa-trash-o b-icon fa-2x"  title="Supprimer"></i>
                                        </a>
                                    </td>
                                </tr>`

                if($("#row-" + tyChrgDivers.id).length)
                    $("#row-" + tyChrgDivers.id).replaceWith(html);
                else
                    $("#typeChargeDiverse-tbody").append(html);
                $("#TypeChargeDiverseModal").modal("hide");
            }
        });
    })

    ConfirmDeleteTypeChargeDiverse = function (id) { 
        $("#IdTypeCharDivers").val(id);
        $("#DeleteTypeChargeDiverse").modal("show");
    }

    DeleteTypeCharDivers = function () { 
        let id = $("#IdTypeCharDivers").val();
        $.ajax({
            type: "POST",
            url: "/TypeEncadrement/DeleteTypeChargeDiverse",
            data: { Id : id },
            success: function (data) {
                if(data.delete) {
                    $("#row-" + data.id).remove();
                    $("#DeleteTypeChargeDiverse").modal("hide");
                }
                else { 
                    $("#info-message").html('Cet Type de Charge Divers est lié à ' + data.element)
                    $("#DeleteTypeChargeDiverse").modal("hide");
                    $("#info-modal").modal("show");
                }
                
            }
        });
    }

    $("#FieldName").change(function (e) { 
        e.preventDefault();
        let fieldName =  $("#FieldName").find(":selected").val();
        
        if(fieldName == "Departement") {
            $.ajax({
                type: "Post",
                url: "/ChargeEnseignement/SelectDepartementsNames",
                success: function (listDeptsNames) {
                    let html1 = `<select class="form-control" id="FieldValue"></select>`
                    $("#FieldValue").replaceWith(html1);
    
                    let html = "";
                    listDeptsNames.forEach((name) => {
                        html += `<option value="${name}">${name}</option>`;
                    })
                    $("#FieldValue").html(html);  
                }
            });
        } else if(fieldName == "Grade") {
            let html1 = `<select class="form-control" id="FieldValue"></select>`
                    $("#FieldValue").replaceWith(html1);
                    
            let html = `<option value="Assistant">A</option>
                        <option value="PES">PES</option>
                        <option value="Professeur">Pr</option>
                        <option value="Maître assistant">MA</option>
                        <option value="Maître de conférence">MC</option>
                        `
            $("#FieldValue").html(html);  
        } else if(fieldName == "Statut") {
            let html1 = `<select class="form-control" id="FieldValue"></select>`
            $("#FieldValue").replaceWith(html1);
    
            let html = `<option value="Contractuel">Contractuel</option>
                        <option value="Expert">Expert</option>
                        <option value="Pr">Pr</option>
                        <option value="Permanant">Permanant</option>
                        <option value="Vacataire">Vacataire</option>
                        `
            $("#FieldValue").html(html);
        } else if(fieldName == "Tous") {
            let html = `<input class="form-control" id="FieldValue" value="Tous">`
            $("#FieldValue").replaceWith(html);
        } 
        else {
            let html = `<input class="form-control" id="FieldValue">`
            $("#FieldValue").replaceWith(html);
        }            
    });

    $(document).on("click", "#checkAll", function () {
        $('#import-form input:checkbox').not(this).prop('checked', this.checked);
    });

    $("#FieldName2").change(function (e) { 
        e.preventDefault();
        let fieldName =  $("#FieldName2").find(":selected").val();
        
        if(fieldName == "Departement") {
            $.ajax({
                type: "Post",
                url: "/ChargeEnseignement/SelectDepartementsNames",
                success: function (listDeptsNames) {
                    let html1 = `<select class="form-control" id="FieldValue2"></select>`
                    $("#FieldValue2").replaceWith(html1);

                    let html = "";
                    listDeptsNames.forEach((name) => {
                        html += `<option value="${name}">${name}</option>`;
                    })
                    $("#FieldValue2").html(html);  
                }
            });
        } else if(fieldName == "Grade") {
            let html1 = `<select class="form-control" id="FieldValue2"></select>`
                    $("#FieldValue2").replaceWith(html1);
                    
            let html = `<option value="A">A</option>
                        <option value="PES">PES</option>
                        <option value="Pr">Pr</option>
                        <option value="MA">MA</option>
                        <option value="MC">MC</option>
                        `
            $("#FieldValue2").html(html);  
        } else if(fieldName == "Statut") {
            let html1 = `<select class="form-control" id="FieldValue2"></select>`
            $("#FieldValue2").replaceWith(html1);

            let html = `<option value="Contractuel">Contractuel</option>
                        <option value="Expert">Expert</option>
                        <option value="Pr">Pr</option>
                        <option value="Permanant">Permanant</option>
                        <option value="Vacataire">Vacataire</option>
                        `
            $("#FieldValue2").html(html);
        } else {
            let html = `<input class="form-control" id="FieldValue2">`
            $("#FieldValue2").replaceWith(html);
        }            
    });

    $("#search").click(function(e){
        e.preventDefault();
        let fieldName =  $("#FieldName").find(":selected").val();

        if(fieldName != "" && fieldName != undefined) {
            let fieldValue;

            if(fieldName == "Departement" || fieldName == "Grade" || fieldName == "Statut")
                fieldValue = $("#FieldValue").find(":selected").val();
            else
                fieldValue = $("#FieldValue").val();
            
            if(fieldValue != "" && fieldValue != undefined) {
                let codeAnneeUniv = $("#CodeAnneeUniv").val();

                $.ajax({
                    type: "Post",
                    url: "/ChargeEnseignement/FilterEnseignant",
                    data: { FieldName : fieldName, FieldValue : fieldValue, CodeAnneeUniv : codeAnneeUniv },
                    success: function (teachers) {
                        $("#year-teachers").empty();
                        teachers.forEach((teacher) => {
                            let color = etatColor(teacher);
                            let html = `<tr class="row-color" id="${ "row_" + teacher.id}">
                                            <td class="border-no">
                                                <input type="checkbox" value="${ teacher.id }">
                                            </td>
                                            <td>
                                                ${color}
                                            </td>
                                            <td>${ teacher.cin }</td>
                                            <td>${ teacher.nom }</td>
                                            <td>${ teacher.prenom }</td>
                                            <td>${ teacher.grade }</td>
                                            <td>${ teacher.statut }</td>
                                            <td>${ teacher.intituleFrDepartement }</td>
                                            <td>00</td>
                                            <td>00</td>
                                            <td>00</td>
                                            <td>00</td>
                                            <td>00</td>
                                            <td>00</td>
                                            <td class="text-center">
                                                <a asp-controller="ChargeEnseignement" asp-action="Consult"  title="Consulter">
                                                    <i class="fa fa-eye b-icon fa-2x  text-success"></i>
                                                </a>
                                                <a href="#" onclick="confirmDeleteAUteacher(${ teacher.id})">
                                                    <i class="fa fa-trash-o b-icon fa-2x"  title="supprimer"></i>
                                                </a>
                                            </td>
                                        </tr>`
                            $("#year-teachers").append(html);
                        })
                    }
                });
            }

        }
        
    })
    function etatColor(teacher)
    {
        

        let htmlEtat = "";
        if(teacher.validationChargeAdministration =="Validee") {
            htmlEtat = '<i class="fa fa-check-circle fa-2x g-check"></i>';
        } else if( teacher.validationChargeDepartement =="Validee" && teacher.validationChargeAdministration =="Non Validee") {
            htmlEtat =  '<i class="fa fa-check-circle fa-2x y-check"></i>';
        } else if(teacher.etatSaisie == "Verrouillee") {
            htmlEtat = '<i class="fa fa-check-circle fa-2x r-check">';
        } else {
             htmlEtat = '<i class="fa fa-check-circle fa-2x b-icon"></i>';
        }

        return htmlEtat
    }
    $("#search2").click(function(e){
        e.preventDefault();
        let fieldName =  $("#FieldName2").find(":selected").val();

        if(fieldName != "" && fieldName != undefined) {
            let fieldValue;

            if(fieldName == "Departement" || fieldName == "Grade" || fieldName == "Statut")
                fieldValue = $("#FieldValue2").find(":selected").val();
            else
                fieldValue = $("#FieldValue2").val();
            
            if(fieldValue != "" && fieldValue != undefined) {
                let codeAnneeUniv = $("#CodeAnneeUniv").val();
                
                let url = "/ChargeEnseignement/ImportEnseignants?Code=".concat(fieldName).concat(",",fieldValue).concat(",", codeAnneeUniv);
                $("#body-modal").load(url,function(){
                    $("#import-teacher").modal("show");
                });
            }

        }
        
    })

    $(document).on("click", "#submit", function (e) { 
        e.preventDefault();
        $("#AnneeUniv").val($("#CodeAnneeUniv").val());
        
        let formData = $("#import-form").serialize();
        $.ajax({
            type: "POST",
            url: "/ChargeEnseignement/StoreImportEnseignants",
            data: formData,
            success: function (teachers) {
                        let html = ""
                        teachers.forEach((teacher) => {
                            let color = etatColor(teacher);
                            html += `<tr class="row-color" id="${ "row_" + teacher.id}">
                                            <td class="border-no">
                                                <input type="checkbox" value="${ teacher.id }">
                                            </td>
                                            <td>
                                                ${ color }
                                            </td>
                                            <td>${ teacher.cin }</td>
                                            <td>${ teacher.nom }</td>
                                            <td>${ teacher.prenom }</td>
                                            <td>${ teacher.grade }</td>
                                            <td>${ teacher.statut }</td>
                                            <td>${ teacher.intituleFrDepartement }</td>
                                            <td>00</td>
                                            <td>00</td>
                                            <td>00</td>
                                            <td>00</td>
                                            <td>00</td>
                                            <td>00</td>
                                            <td class="text-center">
                                                <a asp-controller="ChargeEnseignement" asp-action="Consult"  title="Consulter">
                                                    <i class="fa fa-eye b-icon fa-2x  text-success"></i>
                                                </a>
                                                <a href="#" onclick="confirmDeleteAUteacher(${ teacher.id})">
                                                    <i class="fa fa-trash-o b-icon fa-2x"  title="supprimer"></i>
                                                </a>
                                            </td>
                                        </tr>`
                        })
                        $("#year-teachers").append(html);
                        
                html = `<div class="form-group row">
                            <div class="col-md-12">
                                <label class="col-md-4 col-lg-offset-1" for=""><strong>Nom & Prénom</strong> </label>
                                <label class="col-md-3" for=""><strong>CIN</strong> </label>
                                <label class="col-md-4 " for=""><strong>Département </strong></label>
                            </div>
                            <div class="col-md-12 empty-tab"></div>
                        </div>`
                $("#import-teacher").modal("hide");
                $("#body-modal").empty();
                $("#body-modal").append(html);

            }
        });
    })

    addTeacher = function () { 
        $("#create-teacher-body").load("/ChargeEnseignement/CreateAnneeUniversitaireEnseignant", function () { 
            let form = $('form#create-teacher-form');
                $(form).removeData("validator")    // Added by jQuery Validate
                .removeData("unobtrusiveValidation");   // Added by jQuery Unobtrusive Validation
                $.validator.unobtrusive.parse(form);
            $("#create-teacher").modal("show");
        })
    }

    $(document).on("click", "#submit-teacher", function (e) { 
            $("#codeAnneeUniv").val($("#CodeAnneeUniv").val());

            if(!$("form#create-teacher-form").valid())
                return false;
            let formData = $("form#create-teacher-form").serialize();

            $.ajax({
                type: "POST",
                url: "/ChargeEnseignement/StoreAnneeUniversitaireEnseignant",
                data: formData,
                success: function (teacher) {
                    if( teacher != null) {
                        let color = etatColor(teacher)
                        let html = `<tr class="row-color" id="${ "row_" + teacher.id}">
                                                <td class="border-no">
                                                    <input type="checkbox" value="${ teacher.id }">
                                                </td>
                                                <td>
                                                    ${ color}
                                                </td>
                                                <td>${ teacher.cin }</td>
                                                <td>${ teacher.nom }</td>
                                                <td>${ teacher.prenom }</td>
                                                <td>${ teacher.grade }</td>
                                                <td>${ teacher.statut }</td>
                                                <td>${ teacher.intituleFrDepartement }</td>
                                                <td>00</td>
                                                <td>00</td>
                                                <td>00</td>
                                                <td>00</td>
                                                <td>00</td>
                                                <td>00</td>
                                                <td class="text-center">
                                                    <a asp-controller="ChargeEnseignement" asp-action="Consult"  title="Consulter">
                                                        <i class="fa fa-eye b-icon fa-2x  text-success"></i>
                                                    </a>
                                                    <a href="#" onclick="confirmDeleteAUteacher(${ teacher.id})">
                                                        <i class="fa fa-trash-o b-icon fa-2x"  title="supprimer"></i>
                                                    </a>
                                                </td>
                                            </tr>`
                        $("#year-teachers").append(html);
                    }
                    $("#create-teacher").modal("hide");
                }
            });
    })

    confirmDeleteAUteacher = function(id)
    {
        $("#TeacherId").val(id);
        $("#delete-annee-teacher").modal("show");
    }

    DeleteAUTeacher = function () { 
        let id =  $("#TeacherId").val();
    $.ajax({
        type: "POST",
        url: "/ChargeEnseignement/DeleteAnneeUniversitaireEnseignant",
        data: { Id : id },
        success: function (data) {
            if(data.delete) {
                $("#row_" + data.id).remove();
                $("#delete-annee-teacher").modal("hide");
            }
            else { 
                $("#info-message").html('Cet Enseignant est lié à ' + data.element)
                $("#delete-annee-teacher").modal("hide");
                $("#info-modal").modal("show");
            }
        }
    });
    }

    
    $("#check-All").click(function () { 
        $('#year-teachers input:checkbox').not(this).prop('checked', this.checked);
    });
    
    function selectedTeachers() {
        let ids = []

        $.each($("#year-teachers input:checkbox:checked"), function(){
            ids.push($(this).val());
        });

        let intarray = []
        ids.forEach( element =>{
            intarray.push(parseInt(element));
        })

        return intarray;
    }

    lockcharges = function() {
       let ids = selectedTeachers();
       if(ids.length > 0) {
           $.ajax({
               type: "POST",
               url: "/ChargeEnseignement/Lockcharges",
               data: { StrIds: JSON.stringify(ids) },
               success: function (data) {
                    $('#year-teachers input:checkbox').not(this).prop('checked', false);
                    $('#check-All').not(this).prop('checked', false);
                    showInfo(data.title, data.message);
               }
            });
       }
    }

    unlockcharges = function() {
        let ids = selectedTeachers();
        if(ids.length > 0) {
            $.ajax({
                type: "POST",
                url: "/ChargeEnseignement/UnLockcharges",
                data: { StrIds: JSON.stringify(ids) },
                success: function (data) {
                    $('#year-teachers input:checkbox').not(this).prop('checked', false);
                    $('#check-All').not(this).prop('checked', false);
                    showInfo(data.title, data.message);
                }
            });
        }
    }

    validateCharges = function() {
        let ids = selectedTeachers();
        if(ids.length > 0) {
            $.ajax({
                type: "POST",
                url: "/ChargeEnseignement/ValidateCharges",
                data: { StrIds: JSON.stringify(ids) },
                success: function (data) {
                    $('#year-teachers input:checkbox').not(this).prop('checked', false);
                    $('#check-All').not(this).prop('checked', false);
                    showInfo(data.title, data.message);
                }
            });
        }
    }

    unvalidateCharges = function() {
        let ids = selectedTeachers();
        if(ids.length > 0) {
            $.ajax({
                type: "POST",
                url: "/ChargeEnseignement/UnValidateCharges",
                data: { StrIds: JSON.stringify(ids) },
                success: function (data) {
                    $('#year-teachers input:checkbox').not(this).prop('checked', false);
                    $('#check-All').not(this).prop('checked', false);
                    showInfo(data.title, data.message);
                }
            });
        }
    }
    function showInfo( title, message) {
        $("#info-title").html('<i class="fa fa-info-circle b-icon fa-lg"></i>'.concat(" ", title));
        $("#info-message").html(message)
        $("#info-action").html("Ok");
        $("#info-modal").modal("show");
    }
});