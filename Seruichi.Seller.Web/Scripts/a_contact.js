const _url = {};

$(function () {
    _url.registerContact = common.appPath + '/a_contact/RegisterContact';
    _url.gotoNextPage = common.appPath + '/a_contact/GotoNextPage';

    setValidation();
    addEvents();
});

function setValidation() {
    $('#ContactName')
        .addvalidation_errorElement("#errorContactName")
        .addvalidation_reqired()
        .addvalidation_doublebyte();
    $('#ContactKana')
        .addvalidation_errorElement("#errorContactKana")
        .addvalidation_reqired()
        .addvalidation_doublebyte_kana();
    $('#ContactAddress')
        .addvalidation_errorElement("#errorContactAddress")
        .addvalidation_reqired()
        .addvalidation_custom("customValidation_checkContactAddress");
    $('#ContactPhone')
        .addvalidation_errorElement("#errorContactPhone")
        .addvalidation_reqired()
        .addvalidation_singlebyte_number();
    $('#ContactTypeCD')
        .addvalidation_errorElement("#errorContactTypeCD")
        .addvalidation_reqired();
    $('#ContactAssID')
        .addvalidation_errorElement("#errorContactAssID")
        .addvalidation_singlebyte();
    $('#ContactSubject')
        .addvalidation_errorElement("#errorContactSubject")
        .addvalidation_reqired();
    $('#ContactIssue')
        .addvalidation_errorElement("#errorContactIssue")
        .addvalidation_reqired();
}

function addEvents()
{
    const $form = $('#form1');

    common.bindValidationEvent('#form1', ':not(#ContactPhone)');

    $('#ContactPhone').on('blur', function (e) {
        customValidation_checkPhone(this);
        return common.checkValidityInput(this);
    });

    $('#ContactTypeCD').on('change', function (e) {
        $('#ContactSubject').val($('#ContactTypeCD option:selected').data('subject'));
    });

    $('#btnRegistration').on('click', function () {
        $form.hideChildErrors();

        if (!common.checkValidityOnSave('#form1')) {
            common.setFocusFirstError($form);
            return;
        }

        const fd = new FormData(document.forms.form1);
        const model = Object.fromEntries(fd);
        model.ContactType = $('#ContactTypeCD option:selected').text();

        common.callAjaxWithLoading(_url.registerContact, model, this, function (result) {
            if (result && result.isOK) {
                //sucess
                $('#modal_2').modal('show');
            }
            if (result && result.data) {
                //error
                common.setValidationErrors(result.data);
                common.setFocusFirstError($form);
            }
        });
    });

    $('#btnCompleted').on('click', function () {
        const form = document.forms.form2;
        common.callSubmit(form, _url.gotoNextPage);
    });
}

function customValidation_checkPhone(e) {
    const $this = $(e)

    let inputValue = $this.val();
    inputValue = inputValue.replace(/-/g, "")
    inputValue = inputValue.replace(/－/g, "")
    $this.val(inputValue);

    return true;
}

function customValidation_checkContactAddress(e) {
    const $this = $(e)

    if ($this.val().trim() == "") {
        $this.showError(common.getMessage('E202'));
        return false;
    }

    return true;
}

