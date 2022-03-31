$(function () {

    _url.checkIDpsw = common.appPath + '/t_login/CheckIdpsw';
    setValidation();
    addEvents();
    $('#email').focus();
});

function setValidation() {
    //階
    $('#email')
        .addvalidation_errorElement("#erroremail")
        .addvalidation_reqired(true)
        .addvalidation_singlebyte_number();
       
    //階建て
    $('#password')
       
        .addvalidation_errorElement("#errorpassword")
        .addvalidation_reqired(true)
        .addvalidation_singlebyte_number();
       

}
function addEvents() {

    //共通チェック処理
    common.bindValidationEvent('#form1', "");

    $('#email,#password').on('change', function () {
        const $this = $(this), $email = $('#email'), $password = $('#password')

        if (!common.checkValidityInput($this)) {
            return false;
        }

        let model = {
            TenStaffCD: $email.val(),
            TenStaffPW: $password.val()
        };

        if (!model.TenStaffCD && !model.TenStaffPW) {
            $($email, $password).hideError();
            return;
        }

        common.callAjax(_url.checkIDpsw, model,
            function (result) {
                if (result && result.isOK) {
                    $($email, $password).hideError();
                    const data = result.data;
                }
                if (result && !result.isOK) {
                    const message = result.message;
                    $this.showError(message.MessageText1);
                }
            });
    });
    
}