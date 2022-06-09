$(document).ready(function () {
    
    $("input:checkbox[class=ajoutsuprime]").change(function () {
        var tab = this.value.split(",");
        let ids = $("#idsPS").val();
        let FieldNameNiv = $("#FieldNameNiv").find(":selected").val();
        let FieldNameJou = $("#FieldNameJou").find(":selected").val();
        let FieldNamesean = $("#FieldNamesean").find(":selected").val();
        if (this.checked === true) {
            $.ajax({
                type: "POST",
                url: "/SessionExamenSalle/StoreUpdateSES",
                data: { IdSe: tab[0], IdSalle: tab[1], ids: ids },
                success: function (data) {
                    if (data[0] == "") {
                        location.reload();
                    }
                }
            });
        }
        else {
            $.ajax({
                type: "POST",
                url: "/SessionExamenSalle/DeleteSES",
                data: { IdSe: tab[0], IdSalle: tab[1], ids: ids },
                //data: code,
                success: function (data) {

                    if (data.delete) {
                        location.reload();
                    }
                }
            });
        }
    }); 


    $(document).on("click", "#searchrepsalle", function (e) {
        e.preventDefault();
        let FieldNameNiv = $("#FieldNameNiv").find(":selected").val();
        let FieldNameJou = $("#FieldNameJou").find(":selected").val();
        let FieldNamesean = $("#FieldNamesean").find(":selected").val();
        if (FieldNameNiv != "" && FieldNameNiv != undefined && FieldNameJou != "" && FieldNameJou != undefined && FieldNamesean != "" && FieldNamesean != undefined) {
            let ids = $("#idsPS").val();
            location.href = "/SessionExamenSalle?ids=" + ids + "&idSessionJour=" + FieldNameJou + "&idSessionseance=" + FieldNamesean + "&niveau=" + FieldNameNiv;

        }
    });

});