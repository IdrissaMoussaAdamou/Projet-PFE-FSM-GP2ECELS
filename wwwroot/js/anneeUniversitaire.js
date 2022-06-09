
$(document).ready(function () {

    function diseableButtons() {
        let codeAnneeUniv = $("#CodeAnneeUniv").val();
        
        $.ajax({
            type: "POST",
            url: "/AnneeUniversitaire/VerifyAnneeUniversitaireStatus",
            data: { CodeAnneeUniv : codeAnneeUniv},
            success: function (data) {
                if (data.etatPlanEtudes == "Cloturée")
                    $("button.archive-btn").attr("disabled", true);
            }
        });
        
    }
    diseableButtons();

    $("#search").click(function () {
        let idFiliere;

        if($("#codeFiliere").length == 0)
            idFiliere =  $("#code-filiere").val();
        else
            idFiliere = $("#codeFiliere").find(":selected").val();
    
        let idNiveau = $("#codeNiveau").find(":selected").val();
        let idParcours = $("#codeParcours").find(":selected").val();
        let periode = $("#codePeriode").find(":selected").val();
        let codeAnneeUniv = $("#code-year").val();

        if((idFiliere > 0) && (idNiveau > 0))
        {        
            let codes = idFiliere + "," + idNiveau + "," + idParcours + "," + periode + "," + codeAnneeUniv;
        
                $.ajax({
                    type: "POST",
                    url: "/AnneeUniversitaire/PlanEude",
                    data: { Codes : codes },
                    success: function (data) {

                        let filiere = data.listFilieres[0].intituleFrTypeDiplome;
                        let titre = data.listFilieres[0].typePeriode.concat(" ", periode);
                        $("#option-container").show(); 
                        ConstructHeader(filiere,titre);
                        ConstructBody(data);
                        constructFooter(data);
                    }
                });
        }
        
    })

    function ConstructHeader(filiere, periode) {
        let header = `<tr><th colspan='14'>${filiere} : ${periode}</th></tr><tr><th  colspan='2'>\
        Unité d’enseignement</th><th  colspan='2'>Elément constitutif de l’UE (ECUE)</th><th colspan='4'>Volume Horaire semestriel\
       </th><th colspan='2'>Crédits</th><th colspan='2'>Coefficient</th><th colspan='2'>regime D'examen</th></tr>\
       <tr><th>Code</th><th>Intitule</th><th>Code</th><th>Intitule</th><th>Cours</th><th>TD</th><th>TP</th><th>Total</th><th>ECUE</th><th>EU</th><th>ECUE</th><th>EU</th><th>CC</th>\
       <th>Mixte</th></tr>`;
        $("#plan-header").html(header);
    }

    function ConstructMiddle(nature)
    {
        let html = `<tr class="row-nature"><th colspan='14'>${nature} </tr></th>`;
        $("#plan-body").append(html);
    }

    function ConstructBody(data) {
        let fondamentaleUnites = data.listUnites.filter(unite => unite.nature == "Fondamentale");
        let transversaleUnites = data.listUnites.filter(unite => unite.nature == "Transversale");
        let optionnelleUnites = data.listUnites.filter(unite => unite.nature == "Optionnelle");
        $("#plan-body").empty();

        if(fondamentaleUnites != null) {
            ConstructMiddle('Unités d’enseignement fondamentales');
            fondamentaleUnites.forEach((unite) => {
                let uniteModules = data.listModules.filter(module => module.idUniteEnseignement == unite.idUniteEnseignement);
                addRow(unite, uniteModules);
            })
        }

        if(transversaleUnites != null) {
            ConstructMiddle('Unités d’enseignement transversale');
            transversaleUnites.forEach((unite) => {
                let uniteModules = data.listModules.filter(module => module.idUniteEnseignement == unite.idUniteEnseignement);
                addRow(unite, uniteModules);
            })
        }

        if(optionnelleUnites != null) {
            ConstructMiddle('Unités d’enseignement optionnelle');
            optionnelleUnites.forEach((unite) => {
                let uniteModules = data.listModules.filter(module => module.idUniteEnseignement == unite.idUniteEnseignement);
                addRow(unite, uniteModules);
            })
        }
        
    }

    function addRow(unite, uniteModules)
    {
        if(uniteModules != null)
        {
           let html ="";
           uniteModules.forEach((module, index) => {
               if(index == 0) {
                 
                     html += `<tr>
                                 <td rowspan="${ uniteModules.length }">${ unite.code }</td>
                                 <td rowspan="${ uniteModules.length }">${ unite.intituleFr }</td>
                                 <td>${ module.code }</td>
                                 <td id="${ "cellule_" + module.idModule }">${ module.intituleFr }</td>
                                 <td class="td-center">${ module.nbHeuresCours == 0 ? "" : module.nbHeuresCours }</td>
                                 <td class="td-center">${ module.nbHeuresTD == 0 ? "" : module.nbHeuresTD }</td>
                                 <td class="td-center">${ module.nbHeuresTP == 0 ? "" : module.nbHeuresTP }</td>
                                 <td class="td-center">${ module.nbHeuresCours + module.nbHeuresTD + module.nbHeuresTP }</td>
                                 <td class="td-center">${ module.credits}</td>
                                 <td class="td-center" rowspan="${uniteModules.length}">${unite.credits}</td>
                                 <td class="td-center">${module.coefficient}</td>
                                 <td class="td-center" rowspan="${uniteModules.length}">${unite.coefficient}</td>
                                 <td class="td-center">${module.regimeExamen == "Controle Continu" ? "x" : ""}</td>
                                 <td class="td-center">${module.regimeExamen == "Regime Mixte" ? "x" : ""}</td>
                             </tr>`
                 }
                 else { 
                     html += `<tr>
                                 <td>${module.code}</td>
                                 <td id="${ "cellule_" + module.idModule }">${module.intituleFr}</td>
                                 <td class="td-center">${ module.nbHeuresCours == 0 ? "" : module.nbHeuresCours }</td>
                                 <td class="td-center">${ module.nbHeuresTD == 0 ? "" : module.nbHeuresTD }</td>
                                 <td class="td-center">${ module.nbHeuresTP == 0 ? "" : module.nbHeuresTP }</td>
                                 <td class="td-center">${module.nbHeuresCours + module.nbHeuresTD + module.nbHeuresTP }</td>
                                 <td class="td-center">${module.credits}</td>
                                 <td class="td-center">${module.coefficient}</td>
                                 <td class="td-center">${module.regimeExamen == "Regime Mixte" ? "" : "x"}</td>
                                 <td class="td-center">${module.regimeExamen == "Regime Mixte" ? "x" : ""}</td>
                             </tr>`

                }
           })
           $("#plan-body").append(html);

        }
    }

    function constructFooter(data){
        let totalCours = 0, totalTD = 0, totalTP = 0, totalT = 0, totalCreditsEU = 0, totalCoefficientEU = 0;

        data.listUnites.forEach( (unite) => {
            totalCreditsEU += unite.credits;
            totalCoefficientEU += unite.coefficient;
        });

        data.listModules.forEach((module) => {
            totalCours += module.nbHeuresCours;
            totalTD += module.nbHeuresTD;
            totalTP += module.nbHeuresTP;
            totalT += module.nbHeuresCours + module.nbHeuresTD + module.nbHeuresTP;
        })
        $("#plan-footer").empty();
        let html = `<tr>
                        <td colspan="4"><strong>Total</strong></td>
                        <td>${ totalCours == 0? "" : totalCours }</td>
                        <td>${ totalTD == 0? "" : totalTD }</td>
                        <td>${ totalTP == 0? "" : totalTP }</td>
                        <td>${ totalT == 0? "" : totalT }</td>
                        <td>${ totalCreditsEU == 0? "" : totalCreditsEU }</td>
                        <td>${ totalCreditsEU == 0? "" : totalCreditsEU }</td>
                        <td>${ totalCoefficientEU == 0? "" : totalCoefficientEU }</td>
                        <td>${ totalCoefficientEU == 0? "" : totalCoefficientEU }</td>
                        <td></td>
                        <td></td>
                    </tr>`
        $("#plan-footer").append(html);
    }

    $("#codeFiliere").change(function (e) { 
        e.preventDefault();
        var codeFiliere = $("#codeFiliere").val();
          
        $.ajax({
            type: "POST",
            url: "/AnneeUniversitaire/ConsultSearch",
            data: { CodeFiliere : codeFiliere},
            success: function (data) {

                //Afficher les Niveaux
                var niveauItem = "";
                
                $("#codeNiveau").empty();
                data.listNiveaux.forEach((niveau) => {
                    niveauItem += '<option value="' + niveau.code + '">' + niveau.intituleAbrg + '</option>'
                })
                $("#codeNiveau").html(niveauItem);
                
                //Afficher les Parcours
                var parcoursItem = "";

                $("#codeParcours").empty();
                data.listParcours.forEach((parcours) => {
                    parcoursItem += '<option value="' + parcours.code + '">' + parcours.intituleFr + '</option>'
                })
                $("#codeParcours").html(parcoursItem);  
            }
        });
    });

    $(document).on("change", "#code-filiere", function (e) { 
        e.preventDefault();
        var codeFiliere = $("#code-filiere").val();
          
        $.ajax({
            type: "POST",
            url: "/AnneeUniversitaire/ConsultSearch",
            data: { CodeFiliere : codeFiliere},
            success: function (data) {

                //Afficher les Niveaux
                var niveauItem = "";
                
                $("#code-niveau").empty();
                data.listNiveaux.forEach((niveau) => {
                    niveauItem += '<option value="' + niveau.code + '">' + niveau.intituleAbrg + '</option>'
                })
                $("#code-niveau").html(niveauItem);
                
                //Afficher les Parcours
                var parcoursItem = "";

                $("#code-parcours").empty();
                data.listParcours.forEach((parcours) => {
                    parcoursItem += '<option value="' + parcours.code + '">' + parcours.intituleFr + '</option>'
                })
                $("#code-parcours").html(parcoursItem);  
            }
        });
    });

    $(document).on("change", "#CodeFiliere", function (e) { 
        e.preventDefault();
        var codeFiliere = $("#CodeFiliere").val();
          
        $.ajax({
            type: "POST",
            url: "/AnneeUniversitaire/ConsultSearch",
            data: { CodeFiliere : codeFiliere},
            success: function (data) {

                //Afficher les Niveaux
                var niveauItem = "";
                
                $("#CodeNiveau").empty();
                data.listNiveaux.forEach((niveau) => {
                    niveauItem += '<option value="' + niveau.code + '">' + niveau.intituleAbrg + '</option>'
                })
                $("#CodeNiveau").html(niveauItem);
                
                //Afficher les Parcours
                var parcoursItem = "";

                $("#CodeParcours").empty();
                data.listParcours.forEach((parcours) => {
                    parcoursItem += '<option value="' + parcours.code + '">' + parcours.intituleFr + '</option>'
                })
                $("#CodeParcours").html(parcoursItem);  
            }
        });
    });
    
    $("#btn-option").on("click", function () {

        let idFiliere;

        if($("#codeFiliere").length == 0)
            idFiliere =  $("#code-filiere").val();
        else
            idFiliere = $("#codeFiliere").find(":selected").val();

        
        let idNiveau = $("#codeNiveau").find(":selected").val();
        let idParcours = $("#codeParcours").find(":selected").val();
        let periode = $("#codePeriode").find(":selected").val();
        let codeAnneeUniv = $("#code-year").val();

        if(idFiliere > 0)
        {
            let codes = idFiliere + "," + idNiveau + "," + idParcours + "," + periode + "," + codeAnneeUniv;

            let url = "/AnneeUniversitaire/CreateEditOptions?Codes=" + codes;
            $("#nom-option-body").load(url, function () { 
                let form = $('form#nom-option-form');  
                $(form).removeData("validator")    
                        .removeData("unobtrusiveValidation");  
                $.validator.unobtrusive.parse(form);
                
                $("#nom-option").modal("show");
            });        
        }
    });

    $(document).on("click","#submit-nom-option", function (e) { 
        e.preventDefault();
        if(!$("form#nom-option-form").valid())
            return false;

        let formData = $("#nom-option-form").serialize();
        $.ajax({
            type: "POST",
            url: "/AnneeUniversitaire/StoreUpdateNomOption",
            data: formData,
            success: function (data) {

                data.forEach((ModuleOptionnel) => {
                    $("#cellule_" + ModuleOptionnel.idModule).html(ModuleOptionnel.intitule);
                });
                $("#nom-option").modal("hide");
            }
        });
    })

    
    // Année Universitaire Filières et Parcours enseignés
    $('#annee-filieres, #annee-niv-pars').DataTable({ 
        "searching": false,
         "ordering":false,
         "info": false,
         "paging": false,
         responsive: true,
         "language": {
             'emptyTable': "Aucune Donnée"
         }
    });

    $("#FiliereParcours").click(function () { 
        let codeAnneeUniv = $("#CodeAnneeUniv").val();
        let url ="/AnneeUniversitaire/CreateAnneeUniversitaireFiliere?CodeAnneeUniv=" + codeAnneeUniv;
        $("#annee-filiere-header").load(url,function () { 
            $("#Create-annee-filiere").modal("show");
        })
    });

    $(document).on("change", "#CodeTypeD", function (e) { 
        e.preventDefault();
        let codeTypeDiplome = $("#CodeTypeD").find(":selected").val();
        let codeAnneeUniv = $("#CodeAnneeUniv").val();
        
        $.ajax({
            type: "POST",
            url: "/AnneeUniversitaire/TypeDiplomeFilieres",
            data: {CodeTypeDiplome : codeTypeDiplome, CodeAnneeUniv: codeAnneeUniv},
            success: function (data) {
                $("#FiliereId").empty();
                if(data !=null)
                {
                    let FiliereItem = "";

                    data.forEach((filiere) => {
                        FiliereItem += '<option value="' + filiere.id + '">' + filiere.intituleFr + '</option>'
                    })
                    $("#FiliereId").html(FiliereItem);
                }
            }
        });
        
    })

    $(document).on("click", "#add-annee-filiere", function(e){
        e.preventDefault();
        let idFiliere = $("#FiliereId").find(":selected").val();
        let codeAnneeUniv = $("#CodeAnneeUniv").val();
        
        if(idFiliere > 0  && (codeAnneeUniv != "" && codeAnneeUniv != undefined))
        {
            $.ajax({
                type: "POST",
                url: "/AnneeUniversitaire/StoreAnneeUniversitaireFiliere",
                data: { IdFiliere: idFiliere, CodeAnneeUniv: codeAnneeUniv},
                success: function (filiere) {
                    let codes = codeAnneeUniv.split("-");
                    var html = `<tr class="row-color" id="${ "row_" + filiere.id }">
                                    <td>${ filiere.intituleFrTypeDiplome }</td>
                                    <td>${ filiere.domaine }</td>
                                    <td>${ filiere.intituleFr }</td>
                                    <td>${ filiere.periodeHabilitation }</td>
                                    <td class="text-right">
                                        <a href="#" onclick="ShowDetails(${ codes },${ filiere.id })" title="Consulter">
                                            <i class="fa fa-eye b-icon fa-2x  text-success"></i>
                                        </a>
                                        <a href="#" onclick="DeleteAnneUnivFiliere(${ filiere.id })">
                                            <i class="fa fa-trash-o b-icon fa-2x"  title="Supprimer"></i>
                                        </a>
                                    </td>
                                </tr>`;
                    if($("td.dataTables_empty"))
                    {
                        $("td.dataTables_empty").parent().remove();
                    }
                    $("#ann-filiere-tbody").append(html);
                }
            });

            $("#Create-annee-filiere").modal("hide");
        }
        else
        $("#Create-annee-filiere").modal("hide");

    })

    DeleteAnneUnivFiliere = function (idFiliere) { 
        let codeAnneeUniv = $("#CodeAnneeUniv").val();
       
        $.ajax({
            type: "POST",
            url: "/AnneeUniversitaire/DeleteAnneeUniversitaireFiliere",
            data: { IdFiliere: idFiliere, CodeAnneeUniv: codeAnneeUniv},
            success: function (id) {
                let html = `<tr class="odd">
                                <td valign="top" colspan="5" class="dataTables_empty">Aucune Donnée</td>
                            </tr>`;
                $("#row_" + id).remove();

                if($("#ann-filiere-tbody tr").length == 0)
                    $("#ann-filiere-tbody").append(html);
            }
        });
    
    }
    
    // Année Universitaire Niveaux - parcours

    ShowDetail = function(codeAnneeUniv, idFiliere)
    {
        window.location.href = "/AnneeUniversitaire/AnneeUniversitaireNivParcours?Annee=" + codeAnneeUniv + "?Filiere=" + idFiliere;
    }

    ShowDetails = function(codeDebut , codeFin, codeFiliere)
    {
        let codeAnneeUniv = codeDebut + "-" + codeFin; 
        window.location.href = "/AnneeUniversitaire/AnneeUniversitaireNivParcours?Annee=" + codeAnneeUniv + "?Filiere=" + codeFiliere;
    }

    CreateEditNivParcours = function (idNiveauParcours, idFiliere, codeAnneeUniv) { 
        let code = idNiveauParcours + ',' + idFiliere + ',' + codeAnneeUniv;

        let url =  "/AnneeUniversitaire/CreateNiveauParcours?Code=" + code;

         $("#niveau-parcours-content").load(url, function () {
            let form = $('form#niveau-parcours-form');
           
           $(form).removeData("validator")    
                .removeData("unobtrusiveValidation");   
            $.validator.unobtrusive.parse(form);
            $("#niveau-parours").modal("show");
        })
    }

    $(document).on("click", "#submit-niveau-parcours", function (e) {
        e.preventDefault();
        $("#NewNiveauParcours_IdFiliere").val($("#CodeFiliere").val());
        $("#NewNiveauParcours_CodeAnneeUniv").val($("#CodeAnneeUniv").val());
        $("#NewNiveauParcours_IdNiveau").val($("#CodeNiveau").val());
        $("#NewNiveauParcours_IdParcours").val($("#CodeParcours").val());
        let codeAnneeUniv = $("#CodeAnneeUniv").val();
        
        if(!$('form#niveau-parcours-form').valid())
            return false;  

        let formData = $("#niveau-parcours-form").serialize();
       
        $.ajax({
            type: "POST",
            url: "/AnneeUniversitaire/StoreUpdateNiveauParcours",
            data: formData,
            success: function (data) {
                
                if(data.add) {
                    let html = `<tr class="row-color" id="${"row_" + data.anneeNivPars.id }">
                                <td>${ data.anneeNivPars.intituleFrNiveau }</td>
                                <td>${ data.anneeNivPars.intituleFrParcours }</td>
                                <td>${ data.anneeNivPars.intituleAbrgNiveau }</td>
                                <td>${ data.anneeNivPars.nbGroupesC }</td>
                                <td>${ data.anneeNivPars.nbGroupesTD }</td>
                                <td>${ data.anneeNivPars.nbGroupesTP }</td>
                                <td>${ data.anneeNivPars.nbEtudiants }</td>
                                <td>${ data.anneeNivPars.periode }</td>
                                <td class="text-right">
                                    <a href="#" onclick="CreateEditNivParcours(${ data.anneeNivPars.id },${ data.anneeNivPars.idFiliere },${ codeAnneeUniv })">
                                        <i class="fa fa-edit fa-2x" style="color: #337ab7;" title="Modifier"></i>
                                    </a>
                                    <a href="#" onclick="DeleteNiveauParcours(${ data.anneeNivPars.id })">
                                        <i class="fa fa-trash-o b-icon fa-2x"  title="Supprimer"></i>
                                    </a>
                                </td>
                            </tr>`;

                    if($("td.dataTables_empty"))
                    {
                        $("td.dataTables_empty").parent().remove();
                    }
                    
                    if($("#row_" + data.anneeNivPars.id).length)
                        $("#row_" + data.anneeNivPars.id).replaceWith(html);
                    else
                        $("#niveau-parcours-tbody").append(html);
                    
                    $("#niveau-parours").modal("hide");
                } else
                {
                    $("#info-title").html('<i class="fa fa-info-circle b-icon fa-lg"></i> Ajout Impossible');
                    $("#info-message").html("Ce Niveau-Parcours Existé Déjà");
                    $("#niveau-parours").modal("hide");
                    $("#info-modal").modal("show");
                }
                
            }
        });

    })

    DeleteNiveauParcours = function(id) {
        $.ajax({
            type: "POST",
            url: "/AnneeUniversitaire/DeleteNiveauParcours",
            data: { Id: id },
            success: function (response) {
                $("#row_" + id).remove();

                var html = `<tr class="odd">
                                <td valign="top" colspan="9" class="dataTables_empty">Aucune Donnée</td>
                            </tr>`;
                if($("#niveau-parcours-tbody tr").length == 0)
                $("#niveau-parcours-tbody").append(html);              
            }
        });
    }


    $("#codeDiplome").change(function (e) {
        e.preventDefault();
        let codeDiplome = $("#codeDiplome").find(":selected").val();
        let codeAnneeUniv = $("#code-year").val();

        $.ajax({
            type: "POST",
            url:  "/AnneeUniversitaire/FilterFiliereByTypeDiplome",
            data: { CodeTypeDiplome: codeDiplome, CodeAnneeUniv: codeAnneeUniv },
            success: function (data) { 

                //Afficher les Filière
                var filiereItem = "";
                
                $("#codeFiliere").empty();
                data.listFilieres.forEach((filiere) => {
                    filiereItem += '<option value="' + filiere.id + '">' + filiere.intituleFr + '</option>'
                })
                $("#codeFiliere").html(filiereItem);

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
        })
    });

    // Plan Etude Filiere

    ShowFilierePlanEtude = function(codeAnneeUniv, idFiliere)
    {
        window.location.href = "/AnneeUniversitaire/PlanEtudeFiliere?Filiere=" + idFiliere + "?Annee=" + codeAnneeUniv;
    }

    // Exporter pdf
    $(document).on("click", "#export-pdf", function () { 
        var doc = new jsPDF('landscape')
        doc.autoTable({ 
            html: '#plan-table',
            theme: 'grid',
            headerStyles: {
                lineWidth: 0.01,
                lineColor: [215,215,215],
                halign: 'center',    
                fillColor: [226, 239, 217],
                textColor: [19, 18, 16],
                fontStyle: 'bold',
            },
            bodyStyles : {
                textColor: [19, 18, 16]
            },
            footStyles : {
                lineWidth: 0.01,
                lineColor: [215,215,215],
                halign: 'center',    
                fillColor: [247, 243, 214],
                textColor: [19, 18, 16],
                fontStyle: 'bold'
            }
            
        })

        doc.save('plan-etude.pdf')
    })
 
});