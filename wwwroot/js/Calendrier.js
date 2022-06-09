$(document).ready(function () {
    $(document).on("click", "#searchNiveau", function (e) {
        e.preventDefault();
        let fieldName = $("#FieldNameNiveau").find(":selected").val();
        if (fieldName != "" && fieldName != undefined) {
            let ids = $("#idsC").val();
            location.href = "/SessionExamen?ids=" + ids +"&niveau=" + fieldName; 
           
        }
    });

    CreateEditSessionExamen = function (ids, idjour, idsection, numcell) {
        let url = "/SessionExamen/CreateEditSessionExamen";
        let data = { ids: ids, idjour: idjour, idsection: idsection, numcell: numcell };
        $("#create-edit-SessionExamen-body").load(url, data, function () {
            let form = $('form#SessionExamen-form');
            $(form).removeData("validator")    // Added by jQuery Validate
                .removeData("unobtrusiveValidation");   // Added by jQuery Unobtrusive Validation
            $.validator.unobtrusive.parse(form);
            $('#create-SessionExamen').html('Ajouter les informations de la SessionExamen')
            $('#SessionExamen-create').modal("show");
        })
    }

    $(document).on("click", "#submitExam", function (e) {
        e.preventDefault();
        let formData = $('form#SessionExamen-form').serialize();

        if (!$('form#SessionExamen-form').valid())
            return false;

        $.ajax({
            type: "POST",
            url: "/SessionExamen/StoreUpdateSE",
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

    $(document).on("click", "#PDF", function (e) {
        var sTable = document.getElementById('Calendrier').innerHTML;

        var style = "<style>";
        style = style + "table {width: 100%;font: 17px Calibri;}";
        style = style + "table, th, td {border: solid 1px #DDD; border-collapse: collapse;";
        style = style + "padding: 2px 3px;text-align: center;}";
        style = style + "</style>";

        // CREATE A WINDOW OBJECT.
        var win = window.open('', '', 'height=700,width=700');

        win.document.write('<html><head>');
        win.document.write('<title>Calendrier</title>');   // <title> FOR PDF HEADER.
        win.document.write(style);          // ADD STYLE INSIDE THE HEAD TAG.
        win.document.write('</head>');
        win.document.write('<body>');
        win.document.write(sTable);         // THE TABLE CONTENTS INSIDE THE BODY TAG.
        win.document.write('</body></html>');

        win.document.close(); 	// CLOSE THE CURRENT WINDOW.

        win.print(); 
    })

    $(document).on("click", "#PDF2", function (e) {
        let ids = $("#idsC").val();
        let niveau = $("#Niveauimp").val();
        location.href = "/SessionExamen/Imprimer/?ids=" + ids + "&niveau=" + niveau; 
    })

    ConfirmDelete = function (ids, idj, idsec, nbc) {
        $.ajax({
            type: "Get",
            url: "/SessionExamen/DeleteCalCell",
            data: { ids: ids, idj: idj, idsec: idsec, nbc: nbc },
            success: function (data) {

                if (data.delete) {
                    location.reload();
                }
                else {
                    alert("ya un problème");
                }
            }
        });
    }


});