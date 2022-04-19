const _url = {};

$(function () {
    _url.login = common.appPath + '/a_login/Login';
    _url.temporaryRegistration = common.appPath + '/a_login/TemporaryRegistration';
    _url.resetPassword = common.appPath + '/a_login/ResetPassword';
    _url.verifyMailAddress = common.appPath + '/a_login/VerifyMailAddress';

    setValidation();
    addEvents();
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
        .addvalidation_errorElement("#formVerifyMailAddress_errorBirthday");

    $('#formVerifyMailAddress_HandyPhone')
        .addvalidation_errorElement("#formVerifyMailAddress_errorHandyPhone")
        .addvalidation_custom("customValidation_checkHandyPhone");

    $('#btnVerifyMailAddress')
        .addvalidation_errorElement("#errorVerifyMailAddress");

    //パスワードを忘れた方
    $('#formResetPassword_SellerKana')
        .addvalidation_errorElement("#formResetPassword_errorSellerKana")
        .addvalidation_custom("customValidation_checkSellerKana");

    $('#formResetPassword_Birthday')
        .addvalidation_errorElement("#formResetPassword_errorBirthday");

    $('#formResetPassword_MailAddress')
        .addvalidation_errorElement("#formResetPassword_errorMailAddress");

    $('#btnResetPassword')
        .addvalidation_errorElement("#errorResetPassword");
}

function addEvents() {

    //共通チェック処理
    //common.bindValidationEvent('#formLogin');
    //common.bindValidationEvent('#formTemporaryRegistration');
    //common.bindValidationEvent('#formVerifyMailAddress');
    //common.bindValidationEvent('#formResetPassword');

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

        form.method = "POST";
        form.action = _url.login;
        form.submit();
    });

    $('#btnVerifyMailAddress').on('click', function () {
        const form = document.forms.formVerifyMailAddress;

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

function customValidation_checkHandyPhone(e) {
    const $this = $(e)

    $this.val(common.replaceDoubleToSingle($this.val()));

    return true;
}