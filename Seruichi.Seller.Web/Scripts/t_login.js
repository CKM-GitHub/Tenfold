$(function () {

    setValidation();
    addEvents();
    $('#email').focus();
});

function setValidation() {
    //階
    $('#email')
        .addvalidation_errorElement("#erroremail")
        .addvalidation_reqired(true)
       /* .addvalidation_singlebyte_number();*/
    //階建て
    $('#password')
        /*.addvalidation_errorElement("#errorFloors")*/
        .addvalidation_reqired(true)
        /*.addvalidation_singlebyte_number();*/

}
function addEvents() {

    //共通チェック処理
    common.bindValidationEvent('#form1',"");
    
}