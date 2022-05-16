const _url = {};

$(function () {
    _url.login = common.appPath + '/a_login/Login';
    _url.temporaryRegistration = common.appPath + '/a_login/TemporaryRegistration';
    _url.resetPassword = common.appPath + '/a_login/ResetPassword';
    _url.verifyMailAddress = common.appPath + '/a_login/VerifyMailAddress';

    setValidation();
    addEvents();
    $('#MailAddress').focus();
});

function setValidation() {
    //仮登録
    $('#formTemporaryRegistration_MailAddress')
        .addvalidation_errorElement("#formTemporaryRegistration_errorMailAddress")
        .addvalidation_custom("customValidation_checkMailAddress");

    //ログイン
    $('#MailAddress')
        .addvalidation_errorElement("#errorMailAddress")
        .addvalidation_custom("customValidation_checkMailAddress");

    $('#Password')
        .addvalidation_errorElement("#errorPassword")
        .addvalidation_custom("customValidation_checkPassword");

    //メールアドレスを忘れた方
    $('#formVerifyMailAddress_SellerKana')
        .addvalidation_errorElement("#formVerifyMailAddress_errorSellerKana")
        .addvalidation_custom("customValidation_checkSellerKana");

    $('#formVerifyMailAddress_Birthday')
        .addvalidation_errorElement("#formVerifyMailAddress_errorBirthday")
        .setInputModeNumber();

    $('#formVerifyMailAddress_HandyPhone')
        .addvalidation_errorElement("#formVerifyMailAddress_errorHandyPhone")
        .addvalidation_custom("customValidation_checkPhone");

    $('#btnVerifyMailAddress')
        .addvalidation_errorElement("#errorVerifyMailAddress");

    //パスワードを忘れた方
    $('#formResetPassword_SellerKana')
        .addvalidation_errorElement("#formResetPassword_errorSellerKana")
        .addvalidation_custom("customValidation_checkSellerKana");

    $('#formResetPassword_Birthday')
        .addvalidation_errorElement("#formResetPassword_errorBirthday")
        .setInputModeNumber();

    $('#formResetPassword_MailAddress')
        .addvalidation_errorElement("#formResetPassword_errorMailAddress");

    $('#btnResetPassword')
        .addvalidation_errorElement("#errorResetPassword");
}

function addEvents() {

    $('#formVerifyMailAddress_Birthday, #formResetPassword_Birthday').on('change', function () {
        common.birthdayCheck(this);
    });

    $('#linkVerifyMailAddress').on('click', function () {
        $modal = $('#modal_verifyMailAddress').hideChildErrors();
        $modal.find("input:not([type=hidden])").val('');
        $modal.modal('show');
        return false;
    });

    $('#linkResetPassword').on('click', function () {
        $modal = $('#modal_resetPassword').hideChildErrors();
        $modal.find("input:not([type=hidden])").val('');
        $modal.modal('show');
        return false;
    });

    $('#btnNew').on('click', function () {
        $modal = $('#modal_temporaryRegistration').hideChildErrors();
        $modal.find("input:not([type=hidden])").val('');
        $modal.modal('show');
    });

    $('#btnTemporaryRegistration').on('click', function () {
        const form = document.forms.formTemporaryRegistration;

        $(form).hideChildErrors();
        if (!common.checkValidityOnSave('#formTemporaryRegistration')) {
            common.setFocusFirstError(form);
            return false;
        }

        const fd = new FormData(form);
        const model = Object.fromEntries(fd);

        common.callAjaxWithLoading(_url.temporaryRegistration, model, this, function (result) {
            if (result && result.isOK) {
                $('#modal_temporaryRegistration').modal('hide');
                $('#modal_temporaryRegistration2').modal('show');
                $('#formTemporaryRegistration2_MailAddress').text(model.MailAddress);
            }
            else if (result && result.data) {
                common.setValidationErrors(result.data);
                common.setFocusFirstError(form);
            }
        });
    });

    $('#btnLogin').on('click', function () {
        const form = document.forms.formLogin;

        $(form).hideChildErrors();
        if (!common.checkValidityOnSave('#formLogin')) {
            common.setFocusFirstError(form);
            return false;
        }

        common.callSubmit(form, _url.login);
    });

    $('#btnVerifyMailAddress').on('click', function () {
        const form = document.forms.formVerifyMailAddress;

        $('#modal_verifyMailAddress').hideChildErrors();
        if (!common.checkValidityOnSave('#formVerifyMailAddress')) {
            common.setFocusFirstError(form);
            return false;
        }

        const fd = new FormData(form);
        const model = Object.fromEntries(fd);

        common.callAjaxWithLoading(_url.verifyMailAddress, model, this, function (result) {
            if (result && result.isOK) {
                $('#modal_verifyMailAddress').modal('hide');
                $('#modal_verifyMailAddress2').modal('show');
                $('#formVerifyMailAddress2_MailAddress').text(result.data);
            }
            else if (result && result.data) {
                common.setValidationErrors(result.data);
                common.setFocusFirstError(form);
            }
        });
    });

    $('#btnResetPassword').on('click', function () {
        const form = document.forms.formResetPassword;

        $('#modal_resetPassword').hideChildErrors();
        if (!common.checkValidityOnSave('#formResetPassword')) {
            common.setFocusFirstError(form);
            return false;
        }

        const fd = new FormData(form);
        const model = Object.fromEntries(fd);

        common.callAjaxWithLoading(_url.resetPassword, model, this, function (result) {
            if (result && result.isOK) {
                $('#modal_resetPassword').modal('hide');
                $('#modal_resetPassword2').modal('show');
                $('#formResetPassword2_MailAddress').text(model.MailAddress);
            }
            else if (result && result.data) {
                common.setValidationErrors(result.data);
                common.setFocusFirstError(form);
            }
        });
    });
}

function customValidation_checkMailAddress(e) {
    const $this = $(e)

    if ($this.val().trim() == "") {
        $this.showError(common.getMessage('E202'));
        return false;
    }

    return true;
}

function customValidation_checkPassword(e) {
    const $this = $(e)

    if ($this.val().trim() == "") {
        $this.showError(common.getMessage('E205'));
        return false;
    }

    return true;
}

function customValidation_checkSellerKana(e) {
    const $this = $(e)

    let inputvalue = $this.val();
    inputvalue = common.replaceSingleToDouble(inputvalue);
    inputvalue = common.replaceSingleToDoubleKana(inputvalue);
    $this.val(inputvalue);

    return true;
}

function customValidation_checkPhone(e) {
    const $this = $(e)

    let inputValue = $this.val();
    inputValue = inputValue.replace(/-/g, "")
    inputValue = inputValue.replace(/－/g, "")
    $this.val(common.replaceDoubleToSingle(inputValue));

    return true;
}
