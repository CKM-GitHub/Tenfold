const _url = {};
$(function () {

    _url.checkID = common.appPath + '/t_login/CheckId';
    _url.checkPassword = common.appPath + '/t_login/CheckPassword';
    _url.select_M_TenfoldStaff = common.appPath + '/t_login/select_M_TenfoldStaff'

    setValidation();
    addEvents();
    $('#email').focus();
});

function setValidation() {

    $('#email')
        .addvalidation_errorElement("#erroremail")
        .addvalidation_reqired()
        .addvalidation_onebyte_character()
        .addvalidation_MaxLength(10);
   
    $('#password')
        .addvalidation_errorElement("#errorpassword")
        .addvalidation_reqired()
        .addvalidation_onebyte_character()
        .addvalidation_MaxLength(10);


    $('#btnLogin')
        .addvalidation_errorElement("#errorbtnLogin");

}
function addEvents() {
    common.bindValidationEvent('#form1', '');
    $('#email').on('change', function () {
        const $this = $(this), $email = $('#email')
        let model = {
            TenStaffCD: $email.val()
        };
        common.callAjax(_url.checkID, model,
            function (result) {
                if (result && result.isOK) {
                    $($email).hideError();
                    const data = result.data;
                }
                if (result && !result.isOK) {
                    const message = result.message;
                    $this.showError(message.MessageText1);
                }
            });
    });
    $('#password').on('change', function () {
        const $this = $(this), $password = $('#password')
        let model = {
            TenStaffPW: $password.val()
        };
        common.callAjax(_url.checkPassword, model,
            function (result) {
                if (result && result.isOK) {
                    $($password).hideError();
                    const data = result.data;
                }
                if (result && !result.isOK) {
                    const message = result.message;
                    $this.showError(message.MessageText1);
                }
            });
    });
    $('#btnLogin').on('click', function () {
        $form = $('#form1').hideChildErrors();

        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        const $this = $(this), $email = $('#email'), $password = $('#password')
        let model1 = {
            TenStaffCD: $email.val(),
            TenStaffPW: $password.val()
        };
        common.callAjaxWithLoading(_url.select_M_TenfoldStaff, model1, this, function (result) {
            if (result && result.isOK) {
                //sucess
               // window.location.reload();
                window.location.href = '/t_dashboard/Index';    
            }
            if (result && !result.isOK) {
                const message = result.message;
                $this.showError(message.MessageText1);
            }
        });
    });
}