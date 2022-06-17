const _url = {};

$(function () {
    _url.get_t_seller_Info = common.appPath + '/t_seller_profile/get_t_seller_Info';
    _url.get_t_seller_profile_DisplayData = common.appPath + '/t_seller_profile/get_t_seller_profile_DisplayData';
    _url.checkZipCode = common.appPath + '/t_seller_profile/CheckZipCode';
    _url.modify_SellerData = common.appPath + '/t_seller_profile/modify_SellerData';
    _url.modify_SellerPW = common.appPath + '/t_seller_profile/modify_SellerPW';

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
        .addvalidation_doublebyte_kana();
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
        .addvalidation_maxlengthCheck(1000);
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
    common.bindValidationEvent('#pw', ':not(#ZipCode1, #ZipCode2, #HousePhone, #HandyPhone, #Fax)');

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
        $('#SellerCD').val(common.getUrlParameter('SellerCD'));
        $('#PrefName').val($('#PrefCD').text());
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

    $('#btnPW').on('click', function () {
        $pw = $('#pw').hideChildErrors();
        $('#txtPassword').focus();
        $('#Change_PW').modal('show');
    })

    $('#btnModifyPW').on('click', function () {
        $pw = $('#pw').hideChildErrors();
        if (!common.checkValidityOnSave('#pw')) {
            $pw.getInvalidItems().get(0).focus();
            return false;
        }
        debugger;
        var model = {
            SellerCD: common.getUrlParameter('SellerCD'),
            SellerName: $('#SellerName').val(),
            Password: $('#txtPassword').val(),
            ConfirmPassword: $('#txtConfirmPassword').val()
        };
        Modify_SellerPW(model, $pw);
    });

    var model = {
        SellerCD: common.getUrlParameter('SellerCD')
    }
    get_SellerProfile_Data(model);
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

function get_SellerProfile_Data(model) {
    common.callAjaxWithLoadingSync(_url.get_t_seller_profile_DisplayData, model, this, function (result) {
        if (result && result.isOK) {
            Bind_SellerProfile_Data(result.data);
        }
        else {
            const errors = result.data;
            for (key in errors) {
                const target = document.getElementById(key);
                $(target).showError(errors[key]);
                this.getInvalidItems().get(0).focus();
            }
        }
    })
}

function Bind_SellerProfile_Data(result) {
    var data = JSON.parse(result);
    if (data.length > 0) {
        $('#SellerName').val(data[0]['SellerName']);
        $('#SellerKana').val(data[0]['SellerKana']);
        $('#Birthday').val(data[0]['Birthday']);
        $('#MemoText').val(data[0]['Remark']);
        $('#ZipCode1').val(data[0]['ZipCode1']);
        $('#ZipCode2').val(data[0]['ZipCode2']);
        $('#PrefCD').val(data[0]['PrefCD']);
        $('#PrefName').val(data[0]['PrefName']);
        $('#CityName').val(data[0]['CityName']);
        $('#TownName').val(data[0]['TownName']);
        $('#Address1').val(data[0]['Address1']);
        $('#HousePhone').val(data[0]['HousePhone']);
        $('#HandyPhone').val(data[0]['HandyPhone']);
        $('#Fax').val(data[0]['Fax']);
        $('#MailAddress').val(data[0]['MailAddress']);
        $('#txtPassword').val(data[0]['Password']);
        $('#txtConfirmPassword').val(data[0]['Password']);
    }
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

function Modify_SellerPW(model, $pw) {
    common.callAjaxWithLoading(_url.modify_SellerPW, model, this, function (result) {
        if (result && result.isOK) {
            $('#Change_PW').modal('hide');
            $('#Process_OK').modal('show');
        }
        if (result && !result.isOK) {
            const errors = result.data;
            for (key in errors) {
                const target = document.getElementById(key);
                $(target).showError(errors[key]);
                $pw.getInvalidItems().get(0).focus();
            }
        }
    });
}
