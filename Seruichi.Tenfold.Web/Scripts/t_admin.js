const _url = {};
$(function () {
    setValidation();
    addEvents();
});
function setValidation() {
    $('#TenStaffCD')
        .addvalidation_errorElement("#errorTenStaffCD")
        .addvalidation_reqired()
        .addvalidation_maxlengthCheck(10)
}
function addEvents() {
    common.bindValidationEvent('#form1', '');
}