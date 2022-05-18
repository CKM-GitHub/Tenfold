const _url = {};
$(function () {
    _url.checkTenStaffCD = common.appPath + '/t_admin/CheckTenStaffCD';

    setValidation();
    addEvents();
});
function setValidation() {
    $('#TenStaffCD')
        .addvalidation_errorElement("#errorTenStaffCD")
        .addvalidation_reqired()
        .addvalidation_maxlengthCheck(10)
        .addvalidation_singlebyte()


}
function addEvents() {
    common.bindValidationEvent('#form1', '');

    $('#TenStaffCD').on('change', function () {
        const $this = $(this), $TenStaffCD = $('#TenStaffCD')

        if (!common.checkValidityInput($this)) {
            return false;
        }
        let model = {
            TenStaffCD: $TenStaffCD.val()
        };
        if (!model.TenStaffCD) {
            $($TenStaffCD).hideError();
            return;
        }
        common.callAjax(_url.checkTenStaffCD, model,
            function (result) {
                if (result && result.isOK) {
                    $($TenStaffCD).hideError();
                    const data = result.data;
                }
                if (result && !result.isOK) {
                    const message = result.message;
                    $this.showError(message.MessageText1);
                }
            });
    });
}