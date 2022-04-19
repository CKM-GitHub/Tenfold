const _url = {};

$(function () {
    _url.checkZipCode = common.appPath + '/a_register/CheckZipCode';
    _url.checkAll = common.appPath + '/a_register/CheckAll';
    _url.insertSellerData = common.appPath + '/a_register/InsertSellerData';
    _url.gotoNextPage = common.appPath + '/a_register/GotoNextPage';

    $('#modal_1').modal('show');

    setValidation();
    addEvents();
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
        .addvalidation_reqired();
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
    $('#MailAddress')
        .addvalidation_errorElement("#errorMailAddress")
    $('#Password')
        .addvalidation_errorElement("#errorPassword")
        .addvalidation_reqired()
    $('#ConfirmPassword')
        .addvalidation_errorElement("#errorConfirmPassword")
        .addvalidation_reqired()
}

function addEvents()
{
    const $form = $('#form1');
    let updateData;

    common.bindValidationEvent('#form1');

    $('#formCheckPolicy, #formCheckRules').on('change', function () {
        if ($('#formCheckPolicy').is(':checked') && $('#formCheckRules').is(':checked')) {
            $('#btnAgree').prop('disabled', false);
        }
        else {
            $('#btnAgree').prop('disabled', true);
        }
    });

    $('#btnAutoSet').on('click', function () {
        const $ZipCode1 = $('#ZipCode1'), $ZipCode2 = $('#ZipCode2');
        const model = {
            zipCode1: $ZipCode1.val(),
            zipCode2: $ZipCode2.val()
        }
        if (model.zipCode1) {
            common.callAjaxWithLoading(_url.checkZipCode, model, this, function (result) {
                if (result && result.isOK) {
                    var data = result.data;
                    if (data.prefCD) $('#PrefCD').val(data.prefCD);
                    if (data.cityName) $('#CityName').val(data.cityName);
                    if (data.townName) $('#TownName').val(data.townName);
                }
                if (result && result.message) {
                    $($ZipCode1, $ZipCode2).showError(result.message.MessageText1);
                }
            });
        }
    });

    $('#btnShowConfirmation').on('click', function () {
        $form.hideChildErrors();

        if (!common.checkValidityOnSave('#form1')) {
            common.setFocusFirstError($form);
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
                //server validation error
                common.setValidationErrors(result.data);
                common.setFocusFirstError($form);
            }
        });
    });

    $('#btnRegistration').on('click', function () {
        common.callAjaxWithLoading(_url.insertSellerData, updateData, this, function (result) {
            if (result && result.isOK) {
                //sucess
                $('#modal_2').modal('hide');
                $('#modal_3').modal('show');
            }
            if (result && result.data) {
                //server validation error
                $('#modal_2').modal('hide');
                common.setValidationErrors(result.data);
                common.setFocusFirstError($form);
            }
        });
    });

    $('#btnStartAssessment').on('click', function () {
        const form = document.forms.form3;
        form.method = "POST";
        form.action = _url.gotoNextPage;
        form.submit();
    });
}
