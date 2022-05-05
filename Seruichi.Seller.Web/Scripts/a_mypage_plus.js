const _url = {};
$(function () {
    // setValidation();
    _url.Get_Calculate_extra_charge = common.appPath + '/a_mypage_plus/Get_Calculate_extra_charge';
    _url.Insert_D_SellerPossible_OK = common.appPath + '/a_mypage_plus/Insert_D_SellerPossible_OK';
    _url.Insert_D_SellerPossible_NG = common.appPath + '/a_mypage_plus/Insert_D_SellerPossible_NG';
    addEvents();
});

function setValidation() {
    $('#typeNumber')
        .addvalidation_errorElement("#CheckNumber")
        .addvalidation_less_than_zero(); 
   // $("#typeNumber").focus();
}

function addEvents() {
    common.bindValidationEvent('#form1', '');
    const $this = $(this), $typeNum = $('#typeNumber').val()
    let model = { Change_Count: $typeNum };
    Get_Calculate_extra_charge(model, this);

    $('#typeNumber').on('change', function () {
        $form = $('#form1').hideChildErrors();
        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        const $this = $(this), $typeNum = $('#typeNumber').val()
        if (!common.checkValidityInput($this)) {
            return false;
        }
        let model = {
            Change_Count: $typeNum
        };

        if (model.Change_Count > 0) {
            $("#typeNumber").hideError();
            $("#typeNumber").focus();
            Get_Calculate_extra_charge(model, this);
            return;

        }
        else {
            // alert(-1);
            $("#typeNumber").showError(common.getMessage('E105'));
            $("#typeNumber").focus();
            return;
        }
    });

    $('#btnPayment').on('click', function () {
        $form = $('#form1').hideChildErrors();

        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        const $this = $(this), $typeNum = $('#typeNumber').val(), $extraCharge = $('#ChargeFee').val(), $afterValue = $('#afterValue').val()
        let model = {
            Change_Count: $typeNum,
            ChangeFee: $extraCharge,
            Possible_Time: $afterValue
        };

        common.callAjax(_url.Get_Calculate_extra_charge, model, function (result) {
            if (result && result.isOK) {
                const $this = $(this), $typeNum = $('#typeNumber').val(), $extraCharge = document.getElementById('ChargeFee').innerHTML, $afterValue = document.getElementById('afterValue').innerHTML;
               
                let model = {
                    Change_Count: $typeNum,
                    ChangeFee: $extraCharge,
                    Possible_Time: $afterValue
                };

                common.callAjax(_url.Insert_D_SellerPossible_OK, model, function (result) {
                    if (result && result.isOK) {

                       
                    }
                    if (result && !result.isOK) {
                        const errors = result.data;
                        for (key in errors) {
                            const target = document.getElementById(key);
                            $(target).showError(errors[key]);
                        }
                    }
                });
                
            }
            if (result && !result.isOK)
            {
                const $this = $(this), $typeNum = $('#typeNumber').val(), $extraCharge = document.getElementById('ChargeFee').innerHTML, $afterValue = document.getElementById('afterValue').innerHTML;

                let model = {
                    Change_Count: $typeNum,
                    ChangeFee: $extraCharge,
                    Possible_Time: $afterValue
                };
                common.callAjax(_url.Insert_D_SellerPossible_NG, model, function (result) {
                    if (result && result.isOK) {

                    }
                    if (result && !result.isOK) {
                        const errors = result.data;
                        for (key in errors) {
                            const target = document.getElementById(key);
                            $(target).showError(errors[key]);
                        }
                    }
                });
            }
        });
    });
  }
function Get_Calculate_extra_charge(model, $form) {

    common.callAjax(_url.Get_Calculate_extra_charge, model, function (result) {
        if (result && result.isOK) {

            Bind_Charges(result.data);
        }
        if (result && !result.isOK) {
            const errors = result.data;
            for (key in errors) {
                const target = document.getElementById(key);
                $(target).showError(errors[key]);
                $form.getInvalidItems().get(0).focus();
            }
        }
    });
}


function Bind_Charges(result) {
    $('#extraCharge').empty();
    let data = JSON.parse(result);
    let html = "";
    if (data.length > 0) {
        for (var i = 0; i < data.length; i++) {
            html = '<h3 class="d-xl-flex" Maxlength="7" id="ChargeFee">' + data[0]["Change_Count"]+'</h3>\
                <p class="d-xl-flex align-items-xl-end" style = "height: 33px;"> 円</p>'
        }
        $('#extraCharge').append(html);
    }
}
