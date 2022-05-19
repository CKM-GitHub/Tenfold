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
        .addvalidation_reqired()
        .addvalidation_maxlengthCheck(10)
        .addvalidation_singlebyte();

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
function addEvents() {
    common.bindValidationEvent('#form1', '');

    $('#TenStaffCD').on('change', function () {
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

    //const $TenStaffCD = $("#TenStaffCD").val(), $TenStaffPW = $("#TenStaffPW").val(), $TenStaffName = $("#TenStaffName").val(),
    //    $InvalidFLG = $("#InvalidFLG").val()

    //let model = {
    //    TenStaffCD: $TenStaffCD,
    //    TenStaffPW: $TenStaffPW,
    //    TenStaffName: $TenStaffName,
    //    InvalidFLG: $InvalidFLG
    //};

    $('#btn_save_update').on('click', function () {
        $form = $('#form1').hideChildErrors();

        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        
        const fd = new FormData(document.forms.form1);
        const model = Object.fromEntries(fd);
        Save_M_TenfoldStaff(model, $form)
    });
}
function Save_M_TenfoldStaff(model, $form){
    common.callAjaxWithLoading(_url.save_M_TenfoldStaff, model, this, function (result) {
        if (result && result.isOK) {

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