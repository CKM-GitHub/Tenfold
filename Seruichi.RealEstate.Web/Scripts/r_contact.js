const _url = {};
$(function () {
    _url.checkAll = common.appPath + '/r_contact/CheckAll';
    _url.registerContact = common.appPath + '/r_contact/RegisterContact';


    setValidation();
    addEvents();
    $('#ContactName').focus();
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
        .addvalidation_singlebyte()
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
        .addvalidation_reqired()
        .addvalidation_singlebyte_doublebyte();
    $('#ContactIssue')
        .addvalidation_errorElement("#errorContactIssue")
        .addvalidation_reqired()
        .addvalidation_singlebyte_doublebyte();
}
function addEvents() {
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

        common.callAjaxWithLoading(_url.checkAll, model, this, function (result) {
            if (result && result.isOK) {
                //sucess
                Bind_Modal(model);
            }
            if (result && result.data) {
                //error
                common.setValidationErrors(result.data);
                common.setFocusFirstError($form);
            }
        });

        
    });

    $('#btn-modal-check').click(function () {

        const fd = new FormData(document.forms.form1);
        const model = Object.fromEntries(fd);
        model.ContactType = $('#ContactTypeCD option:selected').text();

        $('#modal-check').modal('hide');
        common.callAjaxWithLoading(_url.registerContact, model, this, function (result) {
            
            if (result && result.isOK) {
                // sucess
                $('#modal-2').modal('show');
            }
            if (result && result.data) {
                if (result.data.ContactAddress) {
                    Bind_site_error_modal('site-error-modal', result.data);
                }
                $('#r_contact_close_site_error_modal').click(function () {
                    $('#site-error-modal').modal('hide');
                    //error
                    common.setValidationErrors(result.data);
                    common.setFocusFirstError($form);
                })
            }
        });
    });
    $('#btn-modal-2').on('click', function () {
        window.location.href = common.appPath + '/r_dashboard/Index';
    });

    $('#btnAllClear').on('click', function () {
        $('#ContactName').val('');
        $('#ContactKana').val('');
        $('#ContactAddress').val('');
        $('#ContactPhone').val('');
        $('#ContactTypeCD').val('');
        $('#ContactAssID').val('');
        $('#ContactSubject').val('');
        $('#ContactIssue').val('');
    });
}

function customValidation_checkContactAddress(e) {
    const $this = $(e)

    if ($this.val().trim() == "") {
        $this.showError(common.getMessage('E202'));
        return false;
    }

    return true;
}

function customValidation_checkPhone(e) {
    const $this = $(e)

    let inputValue = $this.val();
    inputValue = inputValue.replace(/-/g, "")
    inputValue = inputValue.replace(/－/g, "")
    $this.val(inputValue);

    return true;
}

function Bind_Modal(model) {
    $('#modal-name').val(model.ContactName)
    $('#modal-kana').val(model.ContactKana)
    $('#modal-contactaddress').val(model.ContactAddress)
    $('#modal-contactphone').val(model.ContactPhone)
    $('#modal-contactTypeCD').val($('#ContactTypeCD').val())
    $('#modal-assId').val(model.ContactAssID)
    $('#modal-subject').val(model.ContactSubject)
    $('#modal-contactType').val(model.ContactIssue)
    $('#modal-check').modal('show');   
}