$(document).ready(function () {

    dataTable=$('#Salle2-datable').DataTable({
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

    ImporterSalle = function () {
        var imp_arr = [];
        // Read all checked checkboxes
        $("input:checkbox[class=import_check]:checked").each(function () {
            imp_arr.push($(this).val());
        });

        // Check checkbox checked or not
        if (imp_arr.length > 0) {
            // Confirm alert
            var confirmimport = confirm("Voullez vous vraiment importer ces salles ?");
            if (confirmimport == true) {
                $.ajax({
                    url: '/Session/ImporterSalle',
                    type: 'POST',
                    data: { tab: imp_arr },
                    success: function (response) {
                        //dataTable.ajax.reload();
                        location.reload();
                    }
                });
            }
        }
    }

});