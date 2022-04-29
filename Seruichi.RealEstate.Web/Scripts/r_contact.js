const _url = {};
$(function () {
    setValidation();
    addEvents();
});
function setValidation() {
    $('#ContactName')
        .addvalidation_errorElement("#errorContactName")
        .addvalidation_reqired()
        .addvalidation_doublebyte();
    //$('#ContactKana')
    //    .addvalidation_errorElement("#errorContactKana")
    //    .addvalidation_reqired()
    //    .addvalidation_doublebyte_kana();
    //$('#ContactAddress')
    //    .addvalidation_errorElement("#errorContactAddress")
    //    .addvalidation_reqired()
    //    .addvalidation_custom("customValidation_checkContactAddress");
    //$('#ContactPhone')
    //    .addvalidation_errorElement("#errorContactPhone")
    //    .addvalidation_reqired()
    //    .addvalidation_singlebyte_number();
    //$('#ContactTypeCD')
    //    .addvalidation_errorElement("#errorContactTypeCD")
    //    .addvalidation_reqired();
    //$('#ContactAssID')
    //    .addvalidation_errorElement("#errorContactAssID")
    //    .addvalidation_singlebyte();
    //$('#ContactSubject')
    //    .addvalidation_errorElement("#errorContactSubject")
    //    .addvalidation_reqired();
    //$('#ContactIssue')
    //    .addvalidation_errorElement("#errorContactIssue")
    //    .addvalidation_reqired();
}
function addEvents() {
    const $form = $('#form1');
    common.bindValidationEvent('#form1', '');
}