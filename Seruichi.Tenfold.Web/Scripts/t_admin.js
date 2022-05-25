const _url = {};
$(function () {
    _url.checkTenStaffCD = common.appPath + '/t_admin/CheckTenStaffCD';
    _url.save_M_TenfoldStaff = common.appPath + '/t_admin/Save_M_TenfoldStaff';

    setValidation();
    addEvents();
});
function setValidation() {
    $('#TenStaffCD')
        .addvalidation_errorElement("#errorTenStaffCD")
        .addvalidation_maxlengthCheck(10)
        .addvalidation_singlebyte();

    $(".update-data").each(function (index) {
        var i = index + 1;        

        $('#txtpw_' + i)
            .addvalidation_errorElement("#error_pw_" + i)
            .addvalidation_reqired()
            .addvalidation_maxlengthCheck(30)
            .addvalidation_singlebyte();

        $('#txtname_'+i)
            .addvalidation_errorElement("#error_name_"+i)
            .addvalidation_reqired()
            .addvalidation_doublebyte()
            .addvalidation_maxlengthCheck(15);
    });

    $('#txtPassword')
        .addvalidation_errorElement("#error_txtPassword")
        .addvalidation_reqired()
        .addvalidation_singlebyte()
        .addvalidation_minlengthCheck(8)
        .addvalidation_maxlengthCheck(30)

    $('#txtConfirmPassword')
        .addvalidation_errorElement("#error_txtConfirmPassword")
        .addvalidation_reqired()
        .addvalidation_singlebyte()
        .addvalidation_minlengthCheck(8)
        .addvalidation_maxlengthCheck(30)
        .addvalidation_passwordcompare()
}
function addEvents() {
    common.bindValidationEvent('#form1', '');

    $('#TenStaffCD').on('change', function () {
       
        if ($('#TenStaffCD').val().trim().length !== 0) {
            $('#TenStaffPW')
                .addvalidation_errorElement("#errorTenStaffPW")
                .addvalidation_reqired()
                .addvalidation_maxlengthCheck(10)
                .addvalidation_singlebyte();

            $('#TenStaffName')
                .addvalidation_errorElement("#errorTenStaffName")
                .addvalidation_reqired()
                .addvalidation_doublebyte()
                .addvalidation_maxlengthCheck(15);
        }
        else {
            $('#TenStaffPW')
                .removeValidation_required()
                .removeValidation_singlebyte();
            $('#TenStaffName')
                .removeValidation_required()
                .removeValidation_doublebyte();

            $('#TenStaffPW').hideError();
            $('#TenStaffName').hideError();
        }
        

        const $this = $(this), $TenStaffCD = $('#TenStaffCD')

        if (!common.checkValidityInput($this)) {
            return false;
        }
        let model = {
            TenStaffCD: $TenStaffCD.val()
        };
        if (!model.TenStaffCD) {
            $($TenStaffCD).hideError();
            return;
        }
        common.callAjax(_url.checkTenStaffCD, model,
            function (result) {
                if (result && result.isOK) {
                    $($TenStaffCD).hideError();
                    const data = result.data;
                }
                if (result && !result.isOK) {
                    const message = result.message;
                    $this.showError(message.MessageText1);
                }
            });
    });
    

    $('#btn_save_update').on('click', function () {
        $form = $('#form1').hideChildErrors();

        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }

        if ($('#chk_Invalidflg').is(':checked')) {
            $('#InvalidFLG').val(1);
        }
        else {
            $('#InvalidFLG').val(0);
        }

        const fd = new FormData(document.forms.form1);
        const model = Object.fromEntries(fd);
        model.lst_AdminModel = Update_list_M_Tenfold();       
        Save_M_TenfoldStaff(model, $form)
    });
}

function Update_list_M_Tenfold() {
    let lst_update_AdminModel = [];
    $(".update-data").each(function (index) {
        var i = index + 1;
        if ($('#chk_Invalidflg').is(':checked')) {
            $('#InvalidFLG').val(1);
        }
        else {
            $('#InvalidFLG').val(0);
        }
        const data = {
            TenStaffCD: $('#txtcd_' + i).val(),
            TenStaffPW: $('#txtpw_' + i).val(),
            TenStaffName: $('#txtname_' + i).val(),
            InvalidFLG: $('#txtflg_' + i).is(':checked') ? "1" :"0",
            DeleteFLG: $('#txtdel_' + i).is(':checked') ? "1" : "0"
        }        
        lst_update_AdminModel.push(data);
    });
   
    return lst_update_AdminModel;
}
function Save_M_TenfoldStaff(model, $form){
    common.callAjaxWithLoading(_url.save_M_TenfoldStaff, model, this, function (result) {
        if (result && result.isOK) {
            window.location.reload();
        }
        if (result && !result.isOK) {
            const errors = result.data;
            for (key in errors) {
                const target = document.getElementById(key);
                $(target).showError(errors[key]);
                $form.getInvalidItems().get(0).focus();
            }
        }
    });
}