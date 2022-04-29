const _url = {};
$(function () {
    setValidation();
    addEvents();
});
function setValidation() {
    $('#ContactName')
        .addvalidation_errorElement("#errorContactName")
        .addvalidation_reqired()
        .addvalidation_maxlengthCheck(25)
        .addvalidation_doublebyte();
    $('#ContactKana')
        .addvalidation_errorElement("#errorContactKana")
        .addvalidation_reqired()
        .addvalidation_maxlengthCheck(25)
        .addvalidation_doublebyte_kana();
    $('#ContactAddress')
        .addvalidation_errorElement("#errorContactAddress")
        .addvalidation_reqired()
        .addvalidation_maxlengthCheck(100)        
        .addvalidation_custom("customValidation_checkContactAddress");
    $('#ContactPhone')
        .addvalidation_errorElement("#errorContactPhone")
        .addvalidation_reqired()
        .addvalidation_maxlengthCheck(15)
        .addvalidation_singlebyte_number();
    $('#ContactTypeCD')
        .addvalidation_errorElement("#errorContactTypeCD")
        .addvalidation_reqired();
    $('#ContactAssID')
        .addvalidation_errorElement("#errorContactAssID")
        .addvalidation_maxlengthCheck(10)
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