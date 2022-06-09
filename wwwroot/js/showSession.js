
$(document).ready(function () {

    $('#table-Enseignant, #table-Section, #table-Salle, #table-Jour, #table-Séance, #Salle2-datable').DataTable({
        responsive: true,
        "iDisplayLength": 10,
        "aLengthMenu": [[10, 20, -1], [10, 20, "Tous"]],
        "language": {
            'emptyTable': "Aucune Donnée",
            "infoEmpty": "",
            "info": "",
            "infoFiltered": "",
            "lengthMenu": "Montrer : _MENU_",
            "search": "Chercher: ",
            "zeroRecords": "Aucun Résultat Trouvé",
            "paginate": {
                "next": "Suivant",
                "previous": "Précédent"
            }
        },

    });


    CreateEditSession = function (Code) {
        let url = "/Session/CreateEditSession?Code=" + Code;
        $("#create-edit-Session-body").load(url, function () {
            let form = $('form#Session-form');
            $(form).removeData("validator")    // Added by jQuery Validate
                .removeData("unobtrusiveValidation");   // Added by jQuery Unobtrusive Validation
            $.validator.unobtrusive.parse(form);
            if (Code != '')
                $('#create-Session').html('Modifier Une Session')
            else
                $('#create-Session').html('Ajouter Une Session')
            $('#Session-create').modal("show");
        })
    }

    $(document).on("click", "#submit", function (e) {
        e.preventDefault();
        let formData = $('form#Session-form').serialize();

        if (!$('form#Session-form').valid())
            return false;

        $.ajax({
            type: "POST",
            url: "/Session/StoreUpdate",
            data: formData,
            success: function (data) {
                if (data[0] == "") {
                    location.reload();
                }
                else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: data[0],
                        //footer: '<a href>Why do I have this issue?</a>'
                    })
                }
            }
        });
    })

    CreateEditSessionJ = function (id, ids) {
        let url = "/Session/CreateEditSessionJ";
        let data = { id: id, ids: ids };
        $("#create-edit-SessionJour-body").load(url, data, function () {
            let form = $('form#SessionJour-form');
            $(form).removeData("validator")    // Added by jQuery Validate
                .removeData("unobtrusiveValidation");   // Added by jQuery Unobtrusive Validation
            $.validator.unobtrusive.parse(form);
            if (id != 0)
                $('#create-SessionJour').html('Modifier une Journée de la Session')
            else
                $('#create-SessionJour').html('Ajouter une Journée à la Session')
            $('#SessionJour-create').modal("show");
        })
    }

    $(document).on("click", "#submitJ", function (e) {
        e.preventDefault();
        let formData = $('form#SessionJour-form').serialize();

        if (!$('form#SessionJour-form').valid())
            return false;

        $.ajax({
            type: "POST",
            url: "/Session/StoreUpdateJ",
            data: formData,
            success: function (data) {
                if (data[0] == "") {
                    location.reload();
                }
                else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: data[0],
                        //footer: '<a href>Why do I have this issue?</a>'
                    })
                }
            }
        });
    })

    ShowJour = function (id) {
        let url = "/Session/showJ?id=" + id;
        $("#SessionJ-info").load(url, function () {
            $("#show-SessionJ").modal("show");
        })
    }

    ConfirmDeleteJ = function (Code) {
        $('#SessionJcode').val(Code);
        $('#delete-SessionJ').modal('show');
    }

    DeleteSessionJ = function () {
        let code = $('#SessionJcode').val();
        $.ajax({
            type: "GET",
            url: "/Session/DeleteJ?id=" + code,
            //data: code,
            success: function (data) {

                if (data.delete) {
                    let html = `<tr class="odd">
                                    <td valign="top" colspan="4" class="dataTables_empty">Aucune Donnée</td>
                                </tr>`;
                    $('#row_' + data.code).remove();

                    if ($("#Session-tbody tr").length == 0)
                        $("#Session-tbody").append(html);
                    $('#delete-SessionJ').modal('hide');
                } else {
                    $("#info-message").html('Cette Journée Session est lié à ' + data.element)
                    $('#delete-SessionJ').modal('hide');
                    $("#info-modal").modal("show");
                }
            }
        });
    }

    CreateEditSessionSalle = function (ids) {
        let url = "/Session/CreateEditSessionSalle?ids=" + ids;
        $("#create-edit-SessionSalle-body").load(url, function () {
            let form = $('form#SessionSalle-form');
            $(form).removeData("validator")    // Added by jQuery Validate
                .removeData("unobtrusiveValidation");   // Added by jQuery Unobtrusive Validation
            $.validator.unobtrusive.parse(form);
            $('#create-SessionSalle').html('Ajouter Une Salle à la Session')
           
            $('#SessionSalle-create').modal("show");
        })
    }

    ImporterSalle = function (ids) {
        var imp_arr = [];
        // Read all checked checkboxes
        $("input:checkbox[class=import_check]:checked").each(function () {
            imp_arr.push($(this).val());
        });

        // Check checkbox checked or not
        if (imp_arr.length > 0) {
            // Confirm alert
            var confirmimport = confirm("Voullez vous vraiment importer ces Salles ?");
            if (confirmimport == true) {
                $.ajax({
                    url: '/Session/ImporterSalle',
                    type: 'POST',
                    data: { tab: imp_arr, ids: ids },
                    success: function (data) {
                        if (data[0] == "") {
                            //$("#SessionSalle-create").modal("hide");
                            location.reload();
                        }
                        else {
                            Swal.fire({
                                icon: 'error',
                                title: 'Oops...',
                                text: data[0],
                                //footer: '<a href>Why do I have this issue?</a>'
                            })
                        }
                    }
                });
            }
        }
    }

    ShowSalle = function (code) {
        let url = "/Session/ShowSalle?code=" + code;
        $("#SessionSalle-info").load(url, function () {
            $("#show-SessionSalle").modal("show");
        })
    }

    ConfirmDeleteS = function (Code) {
        $('#SessionScode').val(Code);
        $('#delete-SessionS').modal('show');
    }

    DeleteSessionS = function () {
        let code = $('#SessionScode').val();
        $.ajax({
            type: "GET",
            url: "/Session/DeleteS?id=" + code,
            //data: code,
            success: function (data) {

                if (data.delete) {
                    let html = `<tr class="odd">
                                    <td valign="top" colspan="4" class="dataTables_empty">Aucune Donnée</td>
                                </tr>`;
                    $('#row_' + data.code).remove();

                    if ($("#Session-tbody tr").length == 0)
                        $("#Session-tbody").append(html);
                    $('#delete-SessionS').modal('hide');
                } else {
                    $("#info-message").html('Cette Salle Session est lié à ' + data.element)
                    $('#delete-SessionS').modal('hide');
                    $("#info-modal").modal("show");
                }
            }
        });
    }

    CreateEditSessionSc = function (id, ids) {
        let url = "/Session/CreateEditSessionSc";
        let data = { id: id, ids: ids };
        $("#create-edit-SessionSeance-body").load(url, data, function () {
            let form = $('form#SessionSeance-form');
            $(form).removeData("validator")    // Added by jQuery Validate
                .removeData("unobtrusiveValidation");   // Added by jQuery Unobtrusive Validation
            $.validator.unobtrusive.parse(form);
            if (id != 0)
                $('#create-SessionSeance').html('Modifier cette Séance de la Session')
            else
                $('#create-SessionSeance').html('Ajouter une Séance à la Session')
            $('#SessionSeance-create').modal("show");
        })
    }

    $(document).on("click", "#submitSc", function (e) {
        e.preventDefault();
        let formData = $('form#SessionSeance-form').serialize();

        if (!$('form#SessionSeance-form').valid())
            return false;

        $.ajax({
            type: "POST",
            url: "/Session/StoreUpdateSc",
            data: formData,
            success: function (data) {
                if (data[0] == "") {
                    location.reload();
                }
                else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: data[0],
                        //footer: '<a href>Why do I have this issue?</a>'
                    })
                }
            }
        });
    })

    ShowSeance = function (id) {
        let url = "/Session/showSc?id=" + id;
        $("#SessionSc-info").load(url, function () {
            $("#show-SessionSc").modal("show");
        })
    }

    ConfirmDeleteSc = function (Code) {
        $('#SessionSccode').val(Code);
        $('#delete-SessionSc').modal('show');
    }

    DeleteSessionSc = function () {
        let code = $('#SessionSccode').val();
        $.ajax({
            type: "GET",
            url: "/Session/DeleteSc?id=" + code,
            //data: code,
            success: function (data) {

                if (data.delete) {
                    let html = `<tr class="odd">
                                    <td valign="top" colspan="4" class="dataTables_empty">Aucune Donnée</td>
                                </tr>`;
                    $('#row_' + data.code).remove();

                    if ($("#Session-tbody tr").length == 0)
                        $("#Session-tbody").append(html);
                    $('#delete-SessionSc').modal('hide');
                } else {
                    $("#info-message").html('Cette Séance Session est lié à ' + data.element)
                    $('#delete-SessionSc').modal('hide');
                    $("#info-modal").modal("show");
                }
            }
        });
    }

    CreateEditSessionSurveillant = function (ids) {
        let url = "/Session/CreateEditSessionSurveillant?ids=" + ids;
        $("#create-edit-SessionSurveillant-body").load(url, function () {
            let form = $('form#SessionSurveillant-form');
            $(form).removeData("validator")    // Added by jQuery Validate
                .removeData("unobtrusiveValidation");   // Added by jQuery Unobtrusive Validation
            $.validator.unobtrusive.parse(form);
            $('#create-SessionSurveillant').html('Ajouter Un Surveillant à la Session')

            $('#SessionSurveillant-create').modal("show");
        })
    }

    ImporterSurveillant = function (ids) {
        var imp_arr = [];
        // Read all checked checkboxes
        $("input:checkbox[class=import2_check]:checked").each(function () {
            imp_arr.push($(this).val());
        });

        // Check checkbox checked or not
        if (imp_arr.length > 0) {
            // Confirm alert
            var confirmimport = confirm("Voullez vous vraiment importer ces Surveillants ?");
            if (confirmimport == true) {
                $.ajax({
                    url: '/Session/ImporterSurveillant',
                    type: 'POST',
                    data: { tab: imp_arr, ids: ids },
                    success: function (data) {
                        if (data[0] == "") {
                            //$("#SessionSalle-create").modal("hide");
                            location.reload();
                        }
                        else {
                            Swal.fire({
                                icon: 'error',
                                title: 'Oops...',
                                text: data[0],
                                //footer: '<a href>Why do I have this issue?</a>'
                            })
                        }
                    }
                });
            }
        }
    }

    ShowSurveillant = function (code) {
        let url = "/Session/ShowSurveillant?Cin=" + code;
        $("#SessionSurveillant-info").load(url, function () {
            $("#show-SessionSurveillant").modal("show");
        })
    }

    ConfirmDeleteSurveillant = function (Code) {
        $('#SessionSurveillantcode').val(Code);
        $('#delete-SessionSurveillant').modal('show');
    }

    DeleteSessionSurveillant = function () {
        let code = $('#SessionSurveillantcode').val();
        $.ajax({
            type: "GET",
            url: "/Session/DeleteSurveillant?id=" + code,
            //data: code,
            success: function (data) {

                if (data.delete) {
                    let html = `<tr class="odd">
                                    <td valign="top" colspan="4" class="dataTables_empty">Aucune Donnée</td>
                                </tr>`;
                    $('#row_' + data.code).remove();

                    if ($("#Session-tbody tr").length == 0)
                        $("#Session-tbody").append(html);
                    $('#delete-SessionSurveillant').modal('hide');
                } else {
                    $("#info-message").html('Cette Journée Session est lié à ' + data.element)
                    $('#delete-SessionSurveillant').modal('hide');
                    $("#info-modal").modal("show");
                }
            }
        });
    }

    CreateEditSessionSection = function (ids, date) {
        let url = "/Session/CreateEditSessionSection";
        let data = { ids: ids, date: date };
        $("#create-edit-SessionSection-body").load(url, data, function () {
            let form = $('form#SessionSection-form');
            $(form).removeData("validator")    // Added by jQuery Validate
                .removeData("unobtrusiveValidation");   // Added by jQuery Unobtrusive Validation
            $.validator.unobtrusive.parse(form);
            $('#create-SessionSection').html('Ajouter Une Section à la Session')

            $('#SessionSection-create').modal("show");
        })
    }

    ImporterSection = function (ids) {
        var imp_arr = [];
        // Read all checked checkboxes
        $("input:checkbox[class=import3_check]:checked").each(function () {
            imp_arr.push($(this).val());
        });

        // Check checkbox checked or not
        if (imp_arr.length > 0) {
            // Confirm alert
            var confirmimport = confirm("Voullez vous vraiment importer ces Sections ?");
            if (confirmimport == true) {
                $.ajax({
                    url: '/Session/ImporterSection',
                    type: 'POST',
                    data: { tab: imp_arr, ids: ids },
                    success: function (data) {
                        if (data[0] == "") {
                            //$("#SessionSalle-create").modal("hide");
                            location.reload();
                        }
                        else {
                            Swal.fire({
                                icon: 'error',
                                title: 'Oops...',
                                text: data[0],
                                //footer: '<a href>Why do I have this issue?</a>'
                            })
                        }
                    }
                });
            }
        }
    }

    ShowSection = function (code) {
        let url = "/Session/ShowSection?code=" + code;
        $("#SessionSection-info").load(url, function () {
            $("#show-SessionSection").modal("show");
        })
    }

    ConfirmDeleteSection = function (Code) {
        $('#SessionSectioncode').val(Code);
        $('#delete-SessionSection').modal('show');
    }

    DeleteSessionSection = function () {
        let code = $('#SessionSectioncode').val();
        $.ajax({
            type: "GET",
            url: "/Session/DeleteSection?id=" + code,
            //data: code,
            success: function (data) {

                if (data.delete) {
                    let html = `<tr class="odd">
                                    <td valign="top" colspan="4" class="dataTables_empty">Aucune Donnée</td>
                                </tr>`;
                    $('#row_' + data.code).remove();

                    if ($("#Session-tbody tr").length == 0)
                        $("#Session-tbody").append(html);
                    $('#delete-SessionSection').modal('hide');
                } else {
                    $("#info-message").html('Cette Journée Session est lié à ' + data.element)
                    $('#delete-SessionSection').modal('hide');
                    $("#info-modal").modal("show");
                }
            }
        });
    }

    $(document).on("change", "#FieldNameSalle", function (e) {
        e.preventDefault();
        let fieldName = $("#FieldNameSalle").find(":selected").val();
        if (fieldName == "Batiment") {
            $.ajax({
                type: "Post",
                url: "/Session/SelectBatiment",
                success: function (l) {
                    let html1 = `<select class="form-control" id="FieldValueSalle"></select>`
                    $("#FieldValueSalle").replaceWith(html1);

                    let html = "";
                    l.forEach((bat) => {
                        html += `<option value="${bat.code}">${bat.nom}</option>`;
                    })
                    $("#FieldValueSalle").html(html);
                }
            });
        } else if (fieldName == "type") {
            let html1 = `<select class="form-control" id="FieldValueSalle"></select>`
            $("#FieldValueSalle").replaceWith(html1);

            let html = `<option value="amphi">Amphithéatre</option>
                        <option value="Bibliothèque">Bibliothèque</option>
                        <option value="Local">Local</option>
                        <option value="Laboratoire">Laboratoire</option>
                        `
            $("#FieldValueSalle").html(html);
        }
        else if (fieldName == "Etat") {

            let html1 = `<select class="form-control" id="FieldValueSalle"></select>`
            $("#FieldValueSalle").replaceWith(html1);

            let html = `      <option value="Disponible">Disponible</option>
                              <option value="Indisponible">Indisponible</option>
                              <option value="En travaux">En travaux</option>
                        `
            $("#FieldValueSalle").html(html);
        }

        else {
                let html = `<input class="form-control" id="FieldValueSalle" value="Tous">`
                $("#FieldValueSalle").replaceWith(html);
        }
    });

    $(document).on("change", "#FieldNameSurv", function (e) {
        e.preventDefault();
        let fieldName = $("#FieldNameSurv").find(":selected").val();
        if (fieldName == "Departement") {
            $.ajax({
                type: "Post",
                url: "/Session/SelectDepartementt",
                success: function (l) {
                    let html1 = `<select class="form-control" id="FieldValueSurv"></select>`
                    $("#FieldValueSurv").replaceWith(html1);

                    let html = "";
                    l.forEach((dep) => {
                        html += `<option value="${dep.intituleFr}">${dep.intituleFr}</option>`;
                    })
                    $("#FieldValueSurv").html(html);
                }
            });
        } else if (fieldName == "Grade") {
            let html1 = `<select class="form-control" id="FieldValueSurv"></select>`
            $("#FieldValueSurv").replaceWith(html1);

            let html = `<option value="Assistant">Assistant</option>
                        <option value="PES">PES</option>
                        <option value="Professeur">Professeur</option>
                        <option value="Maître assistant">Maître assistant</option>
                        <option value="Maître de conférence">Maître de conférence</option>
                        `
            $("#FieldValueSurv").html(html);
        }
        else if (fieldName == "Statut") {

            let html1 = `<select class="form-control" id="FieldValueSurv"></select>`
            $("#FieldValueSurv").replaceWith(html1);

            let html = `<option value="Contractuel">Contractuel</option>
                        <option value="Expert">Expert</option>
                        <option value="Permanant">Permanant</option>
                        <option value="Vacataire">Vacataire</option>
                        `
            $("#FieldValueSurv").html(html);
        }

        else {
            let html = `<input class="form-control" id="FieldValueSurv" value="Tous">`
            $("#FieldValueSurv").replaceWith(html);
        }
    });

    $(document).on("click", "#searchSalle", function (e) {
         e.preventDefault();
        let fieldName = $("#FieldNameSalle").find(":selected").val();
        if (fieldName != "" && fieldName != undefined) {
      
            let fieldValue;

            if (fieldName == "Batiment" || fieldName == "type" || fieldName == "Etat")
                fieldValue = $("#FieldValueSalle").find(":selected").val();
            else
                fieldValue = $("#FieldValueSalle").val();

            if (fieldValue != "" && fieldValue != undefined) {
                let ids = $("#ids").val();

                $.ajax({
                    type: "Post",
                    url: "/Session/FilterSalle",
                    data: { FieldName: fieldName, FieldValue: fieldValue, ids: ids },
                    success: function (Salles) {
                        $("#Salle2-table-body").empty();
                        Salles.forEach((Salle) => {
                            let html = `<tr class="row-color" id="${"row_" + Salle.code}">
                                            <td class="checkz">
                                                <input type="checkbox" class='import_check' name="ID" value="${Salle.Code}">
                                            </td>
                                            <td>${Salle.code}</td>
                                            <td>${Salle.capaciteEnseignement}</td>
                                            <td>${Salle.capaciteExamen}</td>
                                            <td>${Salle.nbSurveillants}</td>
                                            <td>${Salle.etat}</td>
                                        </tr>`
                            $("#Salle2-table-body").append(html);
                        })
                    }
                });
            }

        }
    });

    $(document).on("click", "#searchSurv", function (e) {
        e.preventDefault();
        let fieldName = $("#FieldNameSurv").find(":selected").val();
        if (fieldName != "" && fieldName != undefined) {

            let fieldValue;

            if (fieldName == "Departement" || fieldName == "Grade" || fieldName == "sq")
                fieldValue = $("#FieldValueSurv").find(":selected").val();
            else
                fieldValue = $("#FieldValueSurv").val();

            if (fieldValue != "" && fieldValue != undefined) {
                let ids = $("#ids2").val();

                $.ajax({
                    type: "Post",
                    url: "/Session/FilterSurveillant",
                    data: { FieldName: fieldName, FieldValue: fieldValue, ids: ids },
                    success: function (Surveillants) {
                        $("#Surveill-table-body").empty();
                        Surveillants.forEach((Surveillant) => {
                            let html = `<tr class="row-color" id="${"row_" + Surveillant.id}">
                                            <td class="checkz">
                                                <input type="checkbox" class='import2_check' name="ID" value="${Surveillant.cin}">
                                            </td>
                                            <td>${Surveillant.cin}</td>
                                            <td>${Surveillant.nom}</td>
                                            <td>${Surveillant.prenom}</td>
                                            <td>${Surveillant.grade}</td>
                                            <td>${Surveillant.statut}</td>
                                            <td>${Surveillant.intituleFrDepartement}</td>
                                            <td>${Surveillant.situationAdministrative}</td>
                                        </tr>`
                            $("#Surveill-table-body").append(html);
                        })
                    }
                });
            }

        }
    });
   
});