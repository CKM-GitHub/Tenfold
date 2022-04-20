const _url = {};
$(function () {
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
                window.location.href = common.appPath +'/t_dashboard';    
            }
            if (result && !result.isOK) {
                if (result.message != null) {
                    const message = result.message;
                    $this.showError(message.MessageText1);
                }
                else {
                    const errors = result.data;
                    for (key in errors) {
                        const target = document.getElementById(key);
                        $(target).showError(errors[key]);
                        $form.getInvalidItems().get(0).focus();
                    }
                }
            }
        });
    });
}