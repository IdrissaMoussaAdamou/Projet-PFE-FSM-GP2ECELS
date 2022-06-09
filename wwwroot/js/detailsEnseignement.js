$(document).ready(function(){
  
    $('#TeachersTab').DataTable({
       pageLength: 10,
       lengthChange: false,
       "destroy": true,
       "pagingType": "simple",
       "searching": false,
       "ordering":false,
       "searchable": false,
       "language": {
           "infoEmpty": "",
           "zeroRecords":  "Faites Une Recherche",
           "info": "Page:  _PAGE_ de _PAGES_ | Total _TOTAL_",
           "infoFiltered":   "",
       "lengthMenu": "Montrer : _MENU_",
           "paginate": {
               "next" : '<i class="fa  fa-angle-double-right fa-fw" title="suivant"></i>',
               "previous" : '<i class="fa fa-angle-double-left fa-fw" title="précédent"></i>'
           }
       }
       
    });

    function isEditable(idTeacher) {
        let editable;
        $.ajax({
            type: "POST",
            url: "/ChargeEnseignement/checkDeptValidation",
            data: { Id : idTeacher },
            async: false,
            success: function (data) {
                editable = data;
            }
        });

        return editable;
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
                   
           let html = `<option value="A">A</option>
                       <option value="PES">PES</option>
                       <option value="Pr">Pr</option>
                       <option value="MA">MA</option>
                       <option value="MC">MC</option>
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

   $("#search").click(function(e){
       e.preventDefault();
       
       // Supprimer les infos de L'Enseignant courant
       $("#enseignement-tab-tbody").empty();
       $("#encadrement-tab-tbody").empty();
       $("#charge-divers-tbody").empty();
       $("#TeacherId").val(-1);

       $("#TeacherFirstName").html("");
       $("#TeacherLastName").html("");
       $("#TeacherCIN").html("");
       $("#TeacherStatut").html("");
       $("#TeacherGrade").html("");
       $("#TeacherDept").html("");
       $("#etat-charge").empty("");

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
                       $('#TeachersTab').DataTable({
                           pageLength: 10,
                           lengthChange: false,
                           "destroy": true,
                           "pagingType": "simple",
                           "searching": false,
                           "ordering":false,
                           "searchable": false,
                           "language": {
                               "infoEmpty": "",
                               "zeroRecords":  "Aucun Résultat Trouvé",
                               "info": "Page:  _PAGE_ de _PAGES_ | Total _TOTAL_",
                               "infoFiltered":   "",
                           "lengthMenu": "Montrer : _MENU_",
                               "paginate": {
                                   "next" : '<i class="fa  fa-angle-double-right fa-fw" title="suivant"></i>',
                                   "previous" : '<i class="fa fa-angle-double-left fa-fw" title="précédent"></i>'
                               }
                           },

                           data : teachers,
                           'columns': [
                               { data: 'cin' }, 
                               { data: 'nom' }, 
                               { data: 'prenom' }, 
                               { data: 'grade'},
                               { data: 'statut' },
                             ],
                             "createdRow": function( row, data, dataIndex ) {
                                 $(row).addClass('row-color rows');
                                 $(row).attr('id', "row_" + data.id);
                                 
                             },
                       });
                   }
                });
           }

       }
       
   })
   

   $("#TeachersTab").delegate("tr.rows", "click", function(){
       
       let strId = this.id;
       let id = parseInt(strId.split("_")[1]);
        
       $.ajax({
           type: "POST",
           url: "/ChargeEnseignement/AnneeUniversitaireEnseignantInfos",
           data: { Id: id},
           success: function (data) {
               // Teacher Informations
               $("#TeacherFirstName").html(data.teacher.nom);
               $("#TeacherLastName").html(data.teacher.prenom);
               $("#TeacherCIN").html(data.teacher.cin);
               $("#TeacherStatut").html(data.teacher.statut);
               $("#TeacherGrade").html(data.teacher.grade);
               $("#TeacherDept").html(data.teacher.intituleFrDepartement);
               
               // Etat charge
               constructEtatCharge(data.teacher);

               // save teacher id
               $("#TeacherId").val(data.teacher.id);

               // ChargeParModule de l'Enseignement
               constructChrgEnParModule(data);   

               // ChargeEncadrement de l'Enseignement
               constructChrgEncadrement(data);
               
               // ChargeDiverse de l'Enseignement
               constructChargeDiverse(data);
               //afficher la charge
               construtChargeCalcule(data.teacherCharge, data.teacher)
           }
       });
   });

   function constructChrgEnParModule(data) {
       if(data.teacherChrgEnParModule)
       {
           $("#enseignement-tab-tbody").empty();
           let html = "";
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
                           <td>${ chrgEncadrement.typeEncad.periode.concat(chrgEncadrement.typeEncad.numPeriodeDansAnnee) }</td>
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

   function constructEtatCharge(teacher) {
       let htmlEtat = "";
       if(teacher.validationChargeAdministration =="Validee") {
           htmlEtat = '<span id="etat-charge" class="text-danger"><i class="fa fa-check-circle fa-2x g-check"></i></span>';
       } else if( teacher.validationChargeDepartement =="Validee" && teacher.validationChargeAdministration =="Non Validee") {
           htmlEtat =  '<span id="etat-charge" class="text-danger"><i class="fa fa-check-circle fa-2x y-check"></i></span>';
       } else if(teacher.etatSaisie == "Verrouillee") {
           htmlEtat = '<span id="etat-charge" class="text-danger"><i class="fa fa-check-circle fa-2x r-check"></i></span>';
       } else if(teacher.etatSaisie == "Verifiee") {
            htmlEtat = '<span id="etat-charge" class="text-danger"><i class="fa fa-check-circle fa-2x o-check"></i></span>';
       } else {
            htmlEtat = '<span id="etat-charge" class="text-danger"><i class="fa fa-check-circle fa-2x b-icon"></i></span>';           
       }
       $("#etat-charge").replaceWith(htmlEtat);
   }

   editAnneeUniversitaireTeacher = function()
   {
       let id = $("#TeacherId").val();
       if( id > 0) {
           $("#update-teacher-body").load("/ChargeEnseignement/EditAnneeUniversitaireEnseignant?Id=".concat("",id), function () { 
               let form = $('form#update-teacher-form');
                   $(form).removeData("validator")    
                   .removeData("unobtrusiveValidation");   
                   $.validator.unobtrusive.parse(form);
               $("#update-teacher").modal("show");
           })
       }
   }

   $(document).on("click", "#submit-teacher", function (e) { 
       $("#codeAnneeUniv").val($("#CodeAnneeUniv").val());

       if(!$("form#update-teacher-form").valid())
           return false;
       let formData = $("form#update-teacher-form").serialize();
       $.ajax({
           type: "POST",
           url: "/ChargeEnseignement/UpdateAnneeUniversitaireEnseignant",
           data: formData,
           success: function (teacher) {

               //update info
               $("#TeacherFirstName").html(teacher.nom);
               $("#TeacherLastName").html(teacher.prenom);
               $("#TeacherStatut").html(teacher.statut);
               $("#TeacherGrade").html(teacher.grade);
               $("#TeacherDept").html(teacher.intituleFrDepartement);

               // replace the row in table

               let html = `<tr class="row-color rows odd" id="${ "row_" + teacher.id }" role="row">
                               <td>${ teacher.cin }</td>
                               <td>${ teacher.prenom }</td>
                               <td>${ teacher.nom }</td>
                               <td>${ teacher.grade }</td>
                               <td>${ teacher.statut }</td>
                           </tr>`
               $("#row_" + teacher.id).replaceWith(html);
               $("#update-teacher").modal("hide");
           }
       });

   })

   // ChargeParModule

   CreateEditChargeEParModule = function(id) {
       let teacherId =  $("#TeacherId").val();
       if(teacherId > 0) {
            let editable = isEditable(teacherId)
            if(editable.edit) {
                $("#anneeuniv-enseignement-body").load("/ChargeEnseignement/CreateEditChargeEParModule?Id=".concat("", teacherId).concat(",", id), function () {
                    let form = $('form#anneeuniv-enseignement-form');
                    $(form).removeData("validator")    
                        .removeData("unobtrusiveValidation");  
                    $.validator.unobtrusive.parse(form);
                    
                    if(id > 0)
                    $('#anneeuniv-enseignement-title').html('Modifier Une Charge D\'Enseignement  Par Module')
                    else
                        $('#anneeuniv-enseignement-title').html('Ajouter Une Charge D\'Enseignement  Par Module')
                    $("#anneeuniv-enseignement-modal").modal("show");
                })
            } else {
                showInfo(editable.title, editable.message)
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

                   let html = `<option value="none"> - - - - - - - - - - - - - - - - - - - - - - - - - - -  </option>`;
                   data.listNiveaux.forEach((niveau) => {
                       html += `<option value="${ niveau.id }">${ niveau.intituleAbrg }</option>`;
                   })
                   $("#IdNiveau").append(html);
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

                    $("#anneeuniv-enseignement-modal").modal("hide");
               }
           }
       });
   })

   confirmDeleteChargEnParModule = function(id)
   {
        let teacherId =  $("#TeacherId").val();
        if(teacherId > 0) {
            let editable = isEditable(teacherId)
            if(editable.edit) {
                $("#ChargeEnModuleId").val(id);
                $("#delete-chargeEnModule").modal("show");
            } else {
                showInfo(editable.title, editable.message)
            }
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
                }
                $("#delete-chargeEnModule").modal("hide");
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
            if(editable.edit) {
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
            } else {
                showInfo(editable.title, editable.message)
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
                                    <td>${ chrgEncadrement.typeEncad.periode.concat(chrgEncadrement.typeEncad.numPeriodeDansAnnee) }</td>
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
            if(teacherId > 0) {
                let editable = isEditable(teacherId)
                if(editable.edit) {
                $("#ChargeEndrementId").val(id);
                $("#delete-charge-encadrement").modal("show");
            } else {
                showInfo(editable.title, editable.message)
            }
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
                }
                $("#delete-charge-encadrement").modal("hide");
           }
       });
   }

   // ChargeDiverse

   createEditChargeDiverse = function(id)
   {
       let teacherId =  $("#TeacherId").val();
       if(teacherId > 0)     
       {
           let editable = isEditable(teacherId);
           if(editable.edit) {
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
            showInfo(editable.title, editable.message)
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
       if(teacherId > 0)
       {
           let editable = isEditable(teacherId)
           if(editable.edit) {
                $("#ChargeDiverseId").val(id);
                $("#delete-charge-divers").modal("show");
           } else {
            showInfo(editable.title, editable.message)
           }
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
                }
                $("#delete-charge-divers").modal("hide");
           }
       });
   }

   $('#choose-mode input').on('change', function() {
       let mode = $('input:checked', '#choose-mode').val();
       $("mode-container").empty(); 
       $("#mode-container").load("/ChargeEnseignement/ChangeMode?mode=".concat(mode));

       // charge Data
       let id = $("#TeacherId").val();
       if(id > 0)
       {
           $.ajax({
               type: "POST",
               url: "/ChargeEnseignement/AnneeUniversitaireEnseignantInfos",
               data: { Id : id },
               success: function (data) {
                   if(data)
                   {
                       // Etat charge
                       constructEtatCharge(data.teacher);
                       // ChargeParModule de l'Enseignement
                       constructChrgEnParModule(data);   
               
                       // ChargeEncadrement de l'Enseignement
                       constructChrgEncadrement(data);
                       
                       // ChargeDiverse de l'Enseignement
                       constructChargeDiverse(data);
                       //afficher la charge
                        construtChargeCalcule(data.teacherCharge, data.teacher)
                   }              
               }
           });
       }
   })
   
   
   // Workflow 

   lockCharge = function(){
       let id = $("#TeacherId").val();
       if(id > 0) {
           $.ajax({
               type: "POST",
               url: "/ChargeEnseignement/LockCharge",
               data: { IdTeacher : id },
               success: function (data) {
                       if(data) {
                            if(data.teacher != undefined)
                            {
                                constructEtatCharge(data.teacher);
                            }
                           showInfo(data.title, data.message);
                       }
                   }
           });
       }
       
   }
   
   unLockCharge = function(){
       let id = $("#TeacherId").val();
       if(id > 0) {
           $.ajax({
               type: "POST",
               url: "/ChargeEnseignement/UnLockCharge",
               data: { IdTeacher : id },
               success: function (data) {
                       if(data) {
                            if(data.teacher != undefined)
                            {
                                constructEtatCharge(data.teacher);
                            }
                           showInfo(data.title, data.message);
                       }
                   }
           });
       }
       
   }
   ChargeVerified = function() {
        let id = $("#TeacherId").val();
            if(id > 0) {
                $.ajax({
                    type: "POST",
                    url: "/ChargeEnseignement/ChargeVerified",
                    data: { IdTeacher : id },
                    success: function (data) {
                        if(data) {
                            if(data.teacher != undefined) {
                                constructEtatCharge(data.teacher);
                            }
                            showInfo(data.title, data.message);
                        }
                        construtChargeCalcule(data.teacherCharge, data.teacher)
                    }
                   
                });
            }
   }
    validateCharge = function() {
        let id = $("#TeacherId").val();
        if(id > 0) {
           $.ajax({
               type: "POST",
               url: "/ChargeEnseignement/ValidateCharge",
               data: { IdTeacher : id },
               success: function (data) {
                    if(data) {
                        
                        if(data.teacher != undefined)
                        {
                            constructEtatCharge(data.teacher);
                        }
                        showInfo(data.title, data.message);
                    }
                }
           });
       }
       
    }

    unValidateCharge = function() {
        let id = $("#TeacherId").val();
        if(id > 0) {
           $.ajax({
               type: "POST",
               url: "/ChargeEnseignement/InValidateCharge",
               data: { IdTeacher : id },
               success: function (data) {
                        if(data) {
                           
                            if(data.teacher != undefined)
                            {
                                constructEtatCharge(data.teacher);
                            }
                            showInfo(data.title, data.message);
                        }
                   }
           });
       }
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
   function showInfo( title, message) {
       $("#info-title").html('<i class="fa fa-info-circle b-icon fa-lg"></i>'.concat(" ", title));
       $("#info-message").html(message)
       $("#info-action").html("Ok");
       $("#info-modal").modal("show");
   }

});