const _url = {};

$(function () {
    _url.validateLogin = common.appPath + '/a_login/ValidateLogin';

    setValidation();
    addEvents();
});

function setValidation() {
    //ログイン
    $('#MailAddress')
        .addvalidation_errorElement("#errorMailAddress")
        .addvalidation_custom("customValidation_checkMailAddress");
    $('#Password')
        .addvalidation_errorElement("#errorPassword")
        .addvalidation_custom("customValidation_checkPassword");

    //仮登録（新規登録）
    $('#TemporaryRegistration_MailAddress')
        .addvalidation_errorElement("#errorTemporaryRegistration_MailAddress")
        .addvalidation_custom("customValidation_checkMailAddress");

    //メールアドレスを忘れた方
    $('#ForgotMailAddress_SellerKana')
        .addvalidation_errorElement("#errorForgotMailAddress_SellerKana")
        .addvalidation_reqired();
    $('#ForgotMailAddress_Birthday')
        .addvalidation_errorElement("#errorForgotMailAddress_Birthday")
        .addvalidation_reqired();
    $('#ForgotMailAddress_HandyPhone')
        .addvalidation_errorElement("#errorForgotMailAddress_HandyPhone")
        .addvalidation_reqired();

    //パスワードを忘れた方
    $('#ForgotPassword_SellerKana')
        .addvalidation_errorElement("#errorForgotPassword_SellerKana")
        .addvalidation_reqired();
    $('#ForgotPassword_Birthday')
        .addvalidation_errorElement("#errorForgotPassword_Birthday")
        .addvalidation_reqired();
    $('#ForgotPassword_MailAddress')
        .addvalidation_errorElement("#errorForgotPassword_MailAddress")
        .addvalidation_custom("customValidation_checkMailAddress");
}

function addEvents() {

    //共通チェック処理
    common.bindValidationEvent('#formLogin');
    common.bindValidationEvent('#formTemporaryRegistration');
    common.bindValidationEvent('#formForgotMailAddress');
    common.bindValidationEvent('#formForgotPassword');

    $('#btnLogin').on('click', function () {
        var model = {
            mailAddress: $('#MailAddress').val(),
            password: $('#Password').val()
        };
        common.callAjax(_url.validateLogin, model, this, function (result) {
            if (result && result.isOK) {
                //sucess
                document.formLogin.submit();
            }
            else if (result && result.data) {
                //server validation error
                const errors = result.data;
                for (key in errors) {
                    const target = document.getElementById(key);
                    $(target).showError(errors[key]);
                    $form.getInvalidItems().get(0).focus();
                }
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