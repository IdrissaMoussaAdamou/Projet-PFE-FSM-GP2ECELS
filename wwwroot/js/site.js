$(document).ready(function () {

    $(document).on("click", "#laycalendrier", function (e) {
        let ids = $("#idSession").val();
        $.ajax({
            url: '/Session/Calendrier',
            type: 'POST',
            data: { ids: ids },
        });
    });

});