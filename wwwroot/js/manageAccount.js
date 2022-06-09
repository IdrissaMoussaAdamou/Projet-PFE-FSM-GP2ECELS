
$(document).ready(function() {
    let userAccounts = $('#users-accounts').DataTable({
        pageLength: 15,
        lengthChange: false,
        "destroy": true,
        "pagingType": "simple",
        "searching": false,
        "ordering":false,
        "searchable": false,
        "language": {
            "infoEmpty": "",
            "zeroRecords":  "Aucun Compte Utilisateur",
            "info": "Page:  _PAGE_ de _PAGES_ | Total _TOTAL_",
            "infoFiltered":   "",
        "lengthMenu": "Montrer : _MENU_",
            "paginate": {
                "next" : '<i class="fa  fa-angle-double-right fa-fw" title="suivant"></i>',
                "previous" : '<i class="fa fa-angle-double-left fa-fw" title="précédent"></i>'
            }
        }
       
    });

    createEditUseraccount = function (idUser) { 
        $("#user-register-body").load("/Auth/Register?IdUser=".concat(idUser), function () { 
            let form = $('form#user-register-form');
            $(form).removeData("validator")   
                .removeData("unobtrusiveValidation"); 
            $.validator.unobtrusive.parse(form);
            if(idUser > 0)
               $('#user-register-title').html('Modifier Un Compte Utilisateur')
            else
                $('#user-register-title').html('Ajouter Un Compte Utilisateur')
            $('#user-register-modal').modal("show");
        })
    }
    editAccount = function () { 
        $("#user-update-body").load("/Auth/UpdateAccount", function () { 
            var form = $('form#user-update-form');
            $(form).removeData("validator")   
                .removeData("unobtrusiveValidation"); 
            $.validator.unobtrusive.parse(form);
            $('#user-update-modal').modal("show");
        })
    }

    $(document).on("click", "#update-user", function (e) { 
        e.preventDefault();

        if(!$("#user-update-form").valid())
            return false;
        
        let formData = $("#user-update-form").serialize();

        $.ajax({
            type: "POST",
            url: "/Auth/UpdateAccount",
            data: formData,
            success: function (data) {
                
                if(data)
                {
                    $("#user-name").html(data.userName);
                    $("#user-email").html(data.userEmail);
                }
                $('#user-update-modal').modal("hide");
            }
        });
    })
 
    $(document).on("change", "#Profil", function (e) { 
        e.preventDefault();
        let profil = $("#Profil").find(":selected").val();
        if(profil == "none") {
            $("#Affiliation").empty();
        } else {
            $.ajax({
                type: "POST",
                url: "/Home/Affiliations",
                data: { Profil : profil },
                success: function (affiliations) {
                    $("#Affiliation").empty();
                    if(affiliations)
                    {
                        let html = "";
                        affiliations.forEach(affiliation =>{
                            html += `<option value="${affiliation}">${affiliation}</option>`
                        })
                        $("#Affiliation").append(html);
                    }
                }
            });
        }
    })

    $(document).on("click", "#register-user-submit", function(e) {
        e.preventDefault();
        if(!$("#user-register-form").valid())
           return false;
        let formData = $("#user-register-form").serialize();
        
        $.ajax({
            type: "POST",
            url: "/Auth/Register",
            data: formData,
            success: function (user) {
                if(user) {
                    let data = [
                        user.nom.concat(" ", user.prenom),
                        user.email,
                        user.cin,
                        user.profil,
                        user.affiliation,
                        `<a href="#" onclick="changePassword(${ user.id })">
                            <i class="fa  fa-key fa-2x b-icon" title="Changer le Mot de Passe"></i>
                        </a>
                        <a href="#" onclick="createEditUseraccount(${ user.id })">
                            <i class="fa fa-edit fa-2x b-icon" title="Modifier"></i>
                        </a>
                        <a href="#" onclick="confirmDeleteaccount(${ user.id })">
                            <i class="fa fa-trash-o b-icon fa-2x" title="Supprimer"></i>
                        </a>
                        `
                    ];

                    if($("#row_" + user.id ).length) {
                        userAccounts.row("#row_" + user.id).data(data).draw();
                    }else {
                        let rowNode = userAccounts.row.add(data).draw().node();

                        $(rowNode).addClass("row-color");
                        $(rowNode).attr("id","row_"+ user.id);
                    }
                }
                $('#user-register-modal').modal("hide");
            }
        });
    });

    confirmDeleteaccount = function (idUser) {
        if(idUser > 0) {
            $("#IdUser").val(idUser);
            $("#delete-account").modal("show");
        } 
    }

    deleteUserAccount = function (idAccount) { 
        let idUser = $("#IdUser").val();
        $.ajax({
            type: "POST",
            url: "/Auth/DeleteAccount",
            data: { Id : idUser },
            success: function (idAccount) {
                if(idAccount) {
                    userAccounts.rows('#row_' + idAccount).remove().draw();
                }
                $('#delete-account').modal('hide');
            }
        });
    }

    changePassword = function (idUser) {
        if(idUser > 0) {
            $("#update-password-body").load("/Auth/ChangePassword?Id=".concat(idUser),function () { 
                let form = $('form#update-password-form');
                $(form).removeData("validator")   
                    .removeData("unobtrusiveValidation"); 
                $.validator.unobtrusive.parse(form);

                $("#update-password-modal").modal("show");
            })
        } 
    }

    $(document).on("click", "#update-password-submit", function (e) { 
        e.preventDefault();
        if(!$("#update-password-form"))
            return false
        
        let formData = $("#update-password-form").serialize();
        debugger 
            $.ajax({
                type: "POST",
                url: "/Auth/ChangePassword",
                data: formData,
                success: function (response) {
                    if(response)
                    {
                        $("#info-title").html('<i class="fa fa-info-circle b-icon fa-lg"></i> Nouveau Mot De Passe')
                        $("#info-message").html('Le mot de passe à été changé')
                    }
                    else {
                        $("#info-title").html('<i class="fa fa-info-circle b-icon fa-lg"></i> Nouveau Mot De Passe')
                        $("#info-message").html('Mot de Passe Invalide')
                    }
                    $("#update-password-modal").modal("hide");
                    $("#info-modal").modal("show");
                }
            });
    })

    $(document).on("click", "#submit", function (e) { 
        e.preventDefault();
        if(!$("#edit-password-form").valid())
            return false
        
        //let formData = $("#edit-password-form").serialize();
        let model = {
            Id : 0,
            NewPassword : $("#PasswordModel_NewPassword").val(),
            ConfirmNewPassword : $("#PasswordModel_ConfirmNewPassword").val(),
        }
            $.ajax({
                type: "POST",
                url: "/Auth/ChangePassword",
                data: model,
                success: function (response) {

                    $("#edit-password-form input").val("");
                    if(response)
                    {
                        $("#info-title").html('<i class="fa fa-info-circle b-icon fa-lg"></i> Nouveau Mot De Passe')
                        $("#info-message").html('Le mot de passe à été changé')
                    }
                    else {
                        $("#info-title").html('<i class="fa fa-info-circle b-icon fa-lg"></i> Nouveau Mot De Passe')
                        $("#info-message").html('Mot de Passe Invalide')
                    }
                    $("#update-password-modal").modal("hide");
                    $("#info-action").html("Ok");
                    $("#info-modal").modal("show");
                }
            });
    })
    
});
