const _url = {};

$(function () {
    _url.checkZipCode = common.appPath + '/a_mypage_uinfo/CheckZipCode';
    _url.checkAll = common.appPath + '/a_mypage_uinfo/CheckAll';
    _url.updateSellerData = common.appPath + '/a_mypage_uinfo/UpdateSellerData';
    _url.gotoNextPage = common.appPath + '/a_mypage_uinfo/GotoNextPage';
    _url.changeMailAddress = common.appPath + '/a_mypage_uinfo/ChangeMailAddress';
    _url.changePassword = common.appPath + '/a_mypage_uinfo/ChangePassword';
    _url.gotoLogin = common.appPath + '/a_mypage_uinfo/GotoLogin';

    $('#mypage_uinfo').addClass('active');

    setValidation();
    addEvents();
    $('#SellerName').focus();
});

function setValidation() {
    $('#SellerName')
        .addvalidation_errorElement("#errorSellerName")
        .addvalidation_reqired()
        .addvalidation_doublebyte();
    $('#SellerKana')
        .addvalidation_errorElement("#errorSellerKana")
        .addvalidation_reqired()
        .addvalidation_doublebyte_kana();
    $('#Birthday')
        .addvalidation_errorElement("#errorBirthday")
        .addvalidation_reqired()
        .setInputModeNumber();
    $('#ZipCode1')
        .addvalidation_errorElement("#errorZipCode")
        .addvalidation_singlebyte_number();
    $('#ZipCode2')
        .addvalidation_errorElement("#errorZipCode")
        .addvalidation_singlebyte_number();
    $('#PrefCD')
        .addvalidation_errorElement("#errorPrefCD")
        .addvalidation_reqired();
    $('#CityName')
        .addvalidation_errorElement("#errorCityName")
        .addvalidation_reqired()
        .addvalidation_doublebyte();
    $('#TownName')
        .addvalidation_errorElement("#errorTownName")
        .addvalidation_reqired()
        .addvalidation_doublebyte();
    $('#Address1')
        .addvalidation_errorElement("#errorAddress1")
        .addvalidation_reqired()
        .addvalidation_doublebyte();
    $('#Address2')
        .addvalidation_errorElement("#errorAddress2")
        .addvalidation_doublebyte();
    $('#HousePhone')
        .addvalidation_errorElement("#errorHousePhone")
        .addvalidation_singlebyte_number();
    $('#HandyPhone')
        .addvalidation_errorElement("#errorHandyPhone")
        .addvalidation_reqired()
        .addvalidation_singlebyte_number();
    $('#Fax')
        .addvalidation_errorElement("#errorFax")
        .addvalidation_singlebyte_number();
    $('#ConfirmPassword')
        .addvalidation_errorElement("#errorConfirmPassword")
        .addvalidation_reqired();

    //メールアドレス変更
    $('#formChangeMailAddress_NewMailAddress')
        .addvalidation_errorElement("#formChangeMailAddress_errorNewMailAddress")
        .addvalidation_reqired();
    //$('#formChangeMailAddress_Password')
    //    .addvalidation_errorElement("#formChangeMailAddress_errorPassword")
    //    .addvalidation_reqired();

    //パスワード変更
    $('#formChangePassword_Password')
        .addvalidation_errorElement("#formChangePassword_errorPassword")
        .addvalidation_reqired();
    $('#formChangePassword_NewPassword')
        .addvalidation_errorElement("#formChangePassword_errorNewPassword")
        .addvalidation_reqired();
    $('#formChangePassword_ConfirmNewPassword')
        .addvalidation_errorElement("#formChangePassword_errorConfirmNewPassword")
        .addvalidation_reqired();

}

function addEvents()
{
    const $form = $('#form1');
    let updateData;

    common.bindValidationEvent('#form1', ':not(#ZipCode1, #ZipCode2, #HousePhone, #HandyPhone, #Fax)');

    $('#HousePhone, #HandyPhone, #Fax').on('blur', function (e) {
        customValidation_checkPhone(this);
        return common.checkValidityInput(this);
    });

    $('#Birthday').on('blur', function () {
        common.birthdayCheck(this);
    });

    $('#ZipCode1, #ZipCode2').on('change', function () {
        const $this = $(this), $zipCode1 = $('#ZipCode1'), $zipCode2 = $('#ZipCode2')

        if (!common.checkValidityInput($this)) {
            return false;
        }

        let model = {
            zipCode1: $zipCode1.val(),
            zipCode2: $zipCode2.val()
        };

        if (!model.zipCode1 && !model.zipCode2) {
            $($zipCode1, $zipCode2).hideError();
            return;
        }

        if (model.zipCode1) {
            common.callAjax(_url.checkZipCode, model, function (result) {
                if (result && result.isOK) {
                    var data = result.data;
                    $($zipCode1).hideError();
                    $($zipCode2).hideError();
                    if (data.prefCD) $('#PrefCD').val(data.prefCD);
                    if (data.cityName) $('#CityName').val(data.cityName);
                    if (data.townName) $('#TownName').val(data.townName);
                }
                if (result && result.message) {
                    $zipCode1.showError(result.message.MessageText1);
                    if (model.zipCode2) $zipCode2.showError(result.message.MessageText1);
                }
            });
        }
    });

    $('#btnShowConfirmation').on('click', function () {
        $form.hideChildErrors();

        common.checkValidityOnSave('#form1');
        common.birthdayCheck('#Birthday');

        if (common.setFocusFirstError($form)) {
            return false;
        }

        const fd = new FormData(document.forms.form1);
        const model = Object.fromEntries(fd);
        model.PrefName = $('#PrefCD option:selected').text();

        common.callAjaxWithLoading(_url.checkAll, model, this, function (result) {
            if (result && result.isOK) {
                //sucess
                updateData = model;
                for (key in updateData) {
                    const target = document.getElementById('confirm_' + key);
                    if (target) $(target).val(updateData[key]);
                }
                $('#modal_2').modal('show');
            }
            if (result && result.data) {
                //error
                common.setValidationErrors(result.data);
                common.setFocusFirstError($form);
            }
        });
    });

    $('#btnRegistration').on('click', function () {
        common.callAjaxWithLoading(_url.updateSellerData, updateData, this, function (result) {
            if (result && result.isOK) {
                //sucess
                $('#modal_2').modal('hide');
                $('#modal_3').modal('show');
            }
            if (result && result.data) {
                //error
                $('#modal_2').modal('hide');
                common.setValidationErrors(result.data);
                common.setFocusFirstError($form);
            }
        });
    });

    $('#btnCompleted').on('click', function () {
        const form = document.forms.form2;
        common.callSubmit(form, _url.gotoNextPage);
    });

    $('#btnShowChangeMailAddress').on('click', function () {
        $modal = $('#modal_mail').hideChildErrors();
        $modal.find("input:not([type=hidden])").val('');
        $modal.modal('show');
    });

    $('#btnChangeMailAddress').on('click', function () {
        const form = document.forms.formChangeMailAddress;

        $(form).hideChildErrors();
        if (!common.checkValidityOnSave('#formChangeMailAddress')) {
            common.setFocusFirstError(form);
            return false;
        }

        const fd = new FormData(form);
        const model = Object.fromEntries(fd);
        model.MailAddress = $('#MailAddress').val();

        common.callAjaxWithLoading(_url.changeMailAddress, model, this, function (result) {
            if (result && result.isOK) {
                $('#modal_mail').modal('hide');
                $('#modal_mail2').modal('show');
                $('#formChangeMailAddress2_MailAddress').text(model.NewMailAddress);
            }
            else if (result && result.data) {
                common.setValidationErrors(result.data);
                common.setFocusFirstError(form);
            }
        });
    });

    $('#btnShowChangePassword').on('click', function () {
        $modal = $('#modal_pw').hideChildErrors();
        $modal.find("input:not([type=hidden])").val('');
        $modal.modal('show');
    });

    $('#btnChangePassword').on('click', function () {
        const form = document.forms.formChangePassword;

        $(form).hideChildErrors();
        if (!common.checkValidityOnSave('#formChangePassword')) {
            common.setFocusFirstError(form);
            return false;
        }

        const fd = new FormData(form);
        const model = Object.fromEntries(fd);
        model.MailAddress = $('#MailAddress').val();

        common.callAjaxWithLoading(_url.changePassword, model, this, function (result) {
            if (result && result.isOK) {
                $('#modal_pw').modal('hide');
                $('#modal_pw2').modal('show');
            }
            else if (result && result.data) {
                common.setValidationErrors(result.data);
                common.setFocusFirstError(form);
            }
        });
    });

    $('#btnRelogin').on('click', function () {
        const form = document.forms.formChangePassword2;
        common.callSubmit(form, _url.gotoLogin);
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
