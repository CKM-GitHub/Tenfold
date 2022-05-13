const _url = {};
$(function () {
    _url.get_r_com_profileData = common.appPath + '/r_com_profile/get_r_com_profileData';
    _url.insert_l_log = common.appPath + '/r_com_profile/Insert_l_log';
    setValidation();
    addEvents();
    $('#txtBusinessHours').focus();
});

function setValidation() {
    $('#txtBusinessHours')
        .addvalidation_errorElement("#errorBusinessHours")
        .addvalidation_maxlengthCheck(50);

    $('#txtCompanyHoliday')
        .addvalidation_errorElement("#errorCompanyHoliday")
        .addvalidation_maxlengthCheck(50);

    $('#txtPassword')
        .addvalidation_errorElement("#errorPassword")
        .addvalidation_maxlengthCheck(20);

    $('#txtConfirmPassword')
        .addvalidation_errorElement("#errorConfirmPassword")
        .addvalidation_maxlengthCheck(20);
}

function addEvents() {
    common.callAjaxWithLoading(_url.get_issueslist_Data, model, this, function (result) {
        if (result && result.isOK) {
            Bind_tbody(result.data);
        }
    });
}