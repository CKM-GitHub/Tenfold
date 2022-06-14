const _url = {};

$(function () {
    _url.checkZipCode = common.appPath + '/t_seller_new/CheckZipCode';
    _url.checkAll = common.appPath + '/t_seller_new/CheckAll';
    _url.insertSellerData = common.appPath + '/t_seller_new/InsertSellerData';

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
        .addvalidation_custom("customValidation_convertFullWidth")
        .addvalidation_doublebyte_kana();
    $('#Birthday')
        .addvalidation_errorElement("#errorBirthday")
        .addvalidation_reqired()
        .addvalidation_datecheck()
        .addvalidation_maxlengthCheck(10)
        .setInputModeNumber();
    $('#Password')
        .addvalidation_errorElement("#errorPassword")
        .addvalidation_reqired()
        .addvalidation_singlebyte()
        .addvalidation_passwordcompare()
        .addvalidation_minlengthCheck(8)
        .addvalidation_maxlengthCheck(20);
    $('#ConfirmPassword')
        .addvalidation_errorElement("#errorConfirmPassword")
        .addvalidation_reqired()
        .addvalidation_singlebyte()
        .addvalidation_passwordcompare()
        .addvalidation_minlengthCheck(8)
        .addvalidation_maxlengthCheck(20);
    $('#MemoText')
        .addvalidation_errorElement("#errorMemoText")
        .addvalidation_reqired()
        .addvalidation_maxlengthCheck(2000);
    $('#ZipCode1')
        .addvalidation_errorElement("#errorZipCode")
        .addvalidation_maxlengthCheck(3)
        .addvalidation_singlebyte_number();
    $('#ZipCode2')
        .addvalidation_errorElement("#errorZipCode")
        .addvalidation_maxlengthCheck(4)
        .addvalidation_singlebyte_number();
    $('#PrefName')
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
        .addvalidation_reqired()
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
        .addvalidation_maxlengthCheck(100);
}

function addEvents() {
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