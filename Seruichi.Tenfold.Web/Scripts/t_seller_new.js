const _url = {};

$(function () {
    _url.checkZipCode = common.appPath + '/t_seller_new/CheckZipCode';
    _url.modify_SellerData = common.appPath + '/t_seller_new/modify_SellerData';

    setValidation();
    addEvents();
    $('#SellerName').focus();
});

function setValidation() {
    $('#SellerName')
        .addvalidation_errorElement("#errorSellerName")
        .addvalidation_reqired()
        .addvalidation_maxlengthCheck(25)
        .addvalidation_custom("customValidation_convertFullWidth")
        .addvalidation_doublebyte();
    $('#SellerKana')
        .addvalidation_errorElement("#errorSellerKana")
        .addvalidation_reqired()
        .addvalidation_maxlengthCheck(25)
        .addvalidation_doublebyte_namae_kana();
    $('#Birthday')
        .addvalidation_errorElement("#errorBirthday")
        .addvalidation_reqired()
        .addvalidation_datecheck()
        .addvalidation_maxlengthCheck(10)
        .setInputModeNumber();
    $('#txtPassword')
        .addvalidation_errorElement("#errorPassword")
        .addvalidation_reqired()
        .addvalidation_singlebyte()
        .addvalidation_minlengthCheck(8)
        .addvalidation_maxlengthCheck(20);
    $('#txtConfirmPassword')
        .addvalidation_errorElement("#errorConfirmPassword")
        .addvalidation_reqired()
        .addvalidation_singlebyte()
        .addvalidation_passwordcompare()
        .addvalidation_minlengthCheck(8)
        .addvalidation_maxlengthCheck(20);
    $('#MemoText')
        .addvalidation_errorElement("#errorMemoText")
        .addvalidation_reqired()
        .addvalidation_singlebyte_doublebyte();
    $('#ZipCode1')
        .addvalidation_errorElement("#errorZipCode")
        .addvalidation_maxlengthCheck(3)
        .addvalidation_singlebyte_number();
    $('#ZipCode2')
        .addvalidation_errorElement("#errorZipCode")
        .addvalidation_maxlengthCheck(4)
        .addvalidation_singlebyte_number();
    $('#PrefCD')
        .addvalidation_errorElement("#errorPrefName")
        .addvalidation_reqired();
    $('#CityName')
        .addvalidation_errorElement("#errorCityName")
        .addvalidation_reqired()
        .addvalidation_maxlengthCheck(50)
        .addvalidation_doublebyte();
    $('#TownName')
        .addvalidation_errorElement("#errorTownName")
        .addvalidation_reqired()
        .addvalidation_maxlengthCheck(50)
        .addvalidation_doublebyte();
    $('#Address1')
        .addvalidation_errorElement("#errorAddress1")
        .addvalidation_maxlengthCheck(50)
        .addvalidation_doublebyte();
    $('#HousePhone')
        .addvalidation_errorElement("#errorHousePhone")
        .addvalidation_maxlengthCheck(15)
        .addvalidation_singlebyte_number();
    $('#HandyPhone')
        .addvalidation_errorElement("#errorHandyPhone")
        .addvalidation_reqired()
        .addvalidation_maxlengthCheck(15)
        .addvalidation_singlebyte_number();
    $('#Fax')
        .addvalidation_errorElement("#errorFax")
        .addvalidation_maxlengthCheck(15)
        .addvalidation_singlebyte_number();
    $('#MailAddress')
        .addvalidation_errorElement("#errorMailAddress")
        .addvalidation_reqired()
        .addvalidation_emailCheck()
        .addvalidation_maxlengthCheck(100);
}

function addEvents() {
    common.bindValidationEvent('#form1', ':not(#ZipCode1, #ZipCode2, #HousePhone, #HandyPhone, #Fax)');

    $('#HousePhone, #HandyPhone, #Fax').on('blur', function (e) {
        customValidation_checkPhone(this);
        return common.checkValidityInput(this);
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

    $('#btnConfirm').on('click', function () {
        $form = $('#form1').hideChildErrors();
        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }

        $('#PrefName').val($('#PrefCD option:selected').text());
        const fd = new FormData(document.forms.form1);
        const model = Object.fromEntries(fd);
        Bind_ModalData(model);
        $('#confirm').modal('show');
    });

    $('#btnProcess').on('click', function () {
        $form = $('#form1').hideChildErrors();
        $('#PrefName').val($('#PrefCD option:selected').text());
        const fd = new FormData(document.forms.form1);
        const model = Object.fromEntries(fd);
        Modify_SellerData(model, $form);
    });

    $('#btnProcess_OK').on('click', function () {
        window.location.reload();
    });

    $('#btnClear').on('click', function () {
        window.location.reload();
    })
}

function customValidation_checkPhone(e) {
    const $this = $(e)

    let inputValue = $this.val();
    inputValue = inputValue.replace(/-/g, "")
    inputValue = inputValue.replace(/－/g, "")
    $this.val(inputValue);

    return true;
}

function customValidation_convertFullWidth(e) {
    const $this = $(e)

    let inputvalue = $this.val();
    inputvalue = common.replaceSingleToDouble(inputvalue);
    inputvalue = common.replaceSingleToDoubleKana(inputvalue);
    $this.val(inputvalue);

    return true;
}

function Bind_ModalData(model) {
    $('#m_SellerName').val(model.SellerName);
    $('#m_SellerKana').val(model.SellerKana);
    $('#m_Birthday').val(model.Birthday);
    $('#m_MemoText').val(model.MemoText);
    $('#m_ZipCode1').val(model.ZipCode1);
    $('#m_ZipCode2').val(model.ZipCode2);
    $('#m_PrefName').val(model.PrefName);
    $('#m_CityName').val(model.CityName);
    $('#m_TownName').val(model.TownName);
    $('#m_Address1').val(model.Address1);
    $('#m_HousePhone').val(model.HousePhone);
    $('#m_HandyPhone').val(model.HandyPhone);
    $('#m_Fax').val(model.Fax);
    $('#m_MailAddress').val(model.MailAddress);
}

function Modify_SellerData(model, $form) {
    common.callAjaxWithLoading(_url.modify_SellerData, model, this, function (result) {
        if (result && result.isOK) {
            $('#confirm').modal('hide');
            $('#Process_OK').modal('show');
        }
        if (result && !result.isOK) {
            $('#confirm').modal('hide');
            const errors = result.data;
            for (key in errors) {
                const target = document.getElementById(key);
                $(target).showError(errors[key]);
                $form.getInvalidItems().get(0).focus();
            }
        }
    });
}