$(document).ready(function() {
    $('#year-table').DataTable({
        responsive: true,
        "searching": false,
        "ordering":false,
        "info": false,
        "paging": false,
        "language": {
            'emptyTable': "Aucune Donnée"
        }
    });
    

    CreateAnneeUniversitaire = function (code) { 
        var url = "/AnneeUniversitaire/CreateAnneeUniversitaire?Code=" + code;
        $("#anneeuniv-body").load(url,function () { 

            let today = new Date();
            let dd = String(today.getDate()).padStart(2, '0');
            let mm = String(today.getMonth() + 1).padStart(2, '0'); 
            let yyyy = today.getFullYear();

            let datePicker = yyyy + '-' + mm + '-' + dd;
            let datePicker2 = (today.getFullYear() + 1) + '-' + mm + '-' + dd;
            $("#datePicker2").val(datePicker2);
            $("#datePicker1").val(datePicker);
            $("#create-year").modal("show");
        }) 
    }

    $(document).on("click", "#submit", function (e) { 
        e.preventDefault();

        var AnneeUniversitaire = {
            Code: "",
            DateDebut: $("#datePicker1").val(),
            DateFin: $("#datePicker2").val(),
        }

        $.ajax({
            type: "POST",
            url: "/AnneeUniversitaire/Store",
            data: AnneeUniversitaire,
            success: function (data) {
                
                if(data.nserted) {
                    window.location.href  = data.url;              
                } else {
                    $("#info-title").html('<i class="fa fa-info-circle b-icon fa-lg"></i> Ajout Impossible')
                    $("#info-message").html(`L' année Universitaire: <strong>${ data.codeAnneeUniv + "</strong> est en Cours"}`)
                    $("#info-action").html("ok");
                    $("#create-year").modal("hide");
                    $("#info-modal").modal("show");
                }
            }
        });
    })

    ArchiveAnneeUniv = function (codeAnneeUniv) { 
        $("#CodeAnneeUniv").val(codeAnneeUniv);
        $("#archive-annee").modal("show");
    }

    Archive = function () { 
        let codeAnneeUniv = $("#CodeAnneeUniv").val();        
       $.ajax({
            type: "POST",
            url: "/AnneeUniversitaire/ArchiveAnneeUniversitaire",
            data: { CodeAnneeUniv : codeAnneeUniv },
            success: function (code) {
                $("#cellule_" + code).html("Cloturée");
                $(".fa-archive").parent().remove();
                $("#archive-annee").modal("hide");
            }
        });
    }
});