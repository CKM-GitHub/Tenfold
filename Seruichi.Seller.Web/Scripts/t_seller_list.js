$(function () {

    setValidation();
    addEvents();
});

function setValidation() {
    //階
    $('#inlineFormInput')
        .addvalidation_reqired(true)
   
}
function addEvents() {

    //共通チェック処理
    common.bindValidationEvent('#form1', "");

}