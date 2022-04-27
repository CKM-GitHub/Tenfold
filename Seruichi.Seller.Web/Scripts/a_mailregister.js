const _url = {};

$(function () {
    _url.checkAll = common.appPath + '/a_mailregister/CheckAll';
    _url.updateMailAddress = common.appPath + '/a_mailregister/UpdateMailAddress';
    _url.gotoLogin = common.appPath + '/a_mypage_uinfo/GotoLogin';

    setValidation();
    addEvents();
    $('#Password').focus();
});

function setValidation() {
    $('#MailAddress')
        .addvalidation_errorElement("#errorMailAddress");
    $('#NewMailAddress')
        .addvalidation_errorElement("#errorNewMailAddress");
    $('#Password')
        .addvalidation_errorElement("#errorPassword")
        .addvalidation_reqired();
}

function addEvents()
{
    const $form = $('#form1');
    common.bindValidationEvent('#form1');

    $('#btnRegistration').on('click', function () {
        $form.hideChildErrors();

        if (!common.checkValidityOnSave('#form1')) {
            common.setFocusFirstError($form);
            return false;
        }

        const fd = new FormData(document.forms.form1);
        const model = Object.fromEntries(fd);

        common.callAjaxWithLoading(_url.updateMailAddress, model, this, function (result) {
            if (result && result.isOK) {
                //sucess
                $('#modal_mailchanged').modal('show');
            }
            if (result && result.data) {
                //error
                common.setValidationErrors(result.data);
                common.setFocusFirstError($form);
            }
        });
    });

    $('#btnRelogin').on('click', function () {
        const form = document.forms.form2;
        common.callSubmit(form, _url.gotoLogin);
    });
}
