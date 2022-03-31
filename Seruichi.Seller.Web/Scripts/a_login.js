const _url = {};

$(function () {
    _url.login = common.appPath + '/a_login/login';

    setValidation();
    addEvents();
});

function setValidation() {
    //ログイン
    $('#UserID')
        .addvalidation_errorElement("#errorUserID")
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