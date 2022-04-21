const _url = {};
$(function () {
    _url.checkRealECD = common.appPath + '/r_login/checkRealECD';
    _url.checkREStaffCD = common.appPath + '/r_login/checkREStaffCD';
    _url.checkREPassword = common.appPath + '/r_login/checkREPassword';
    _url.select_M_RealEstateStaff = common.appPath + '/r_login/select_M_RealEstateStaff'

    setValidation();
    addEvents();
    $('#realECD').focus();
});

function setValidation() {

    $('#realECD')
        .addvalidation_errorElement("#errorrealECD")
        .addvalidation_requirednullorempty1();

    $('#reStaffCD')
        .addvalidation_errorElement("#errorreStaffCD")
        .addvalidation_requirednullorempty2();
   
    $('#rePassword')
        .addvalidation_errorElement("#errorrePassword")
        .addvalidation_requirednullorempty3();


    $('#btnLogin')
        .addvalidation_errorElement("#errorbtnLogin");

}
function addEvents() {
    common.bindValidationEvent('#form1', '');
    $('#realECD').on('change', function () {
        const $this = $(this), $realECD = $('#realECD')

        if (!common.checkValidityInput($this)) {
            return false;
        }

        let model = {
            RealECD: $realECD.val()
        };

        if (!model.RealECD) {
            $($realECD).hideError();
            return;
        }

        common.callAjax(_url.checkRealECD, model,
            function (result) {
                if (result && result.isOK) {
                    $($realECD).hideError();
                    const data = result.data;                  
                }
                if (result && !result.isOK) {
                    alert(result.message);
                    const message = result.message;
                    $this.showError(message.MessageText1);
                }
            });
    });
    $('#reStaffCD').on('change', function () {
        const $this = $(this), $realECD = $('#realECD'), $reStaffCD = $('#reStaffCD')

        if (!common.checkValidityInput($this)) {
            return false;
        }

        let model = {
            RealECD: $realECD.val(),
            REStaffCD: $reStaffCD.val()
        };
        if (!model.RealECD && !model.REStaffCD) {
            $($realECD, $reStaffCD).hideError();
            return;
        }

        common.callAjax(_url.checkREStaffCD, model,
            function (result) {
                if (result && result.isOK) {
                    $($reStaffCD).hideError();
                    const data = result.data;
                }
                if (result && !result.isOK) {
                    const message = result.message;
                    $this.showError(message.MessageText1);
                }
            });
    });
    $('#rePassword').on('change', function () {
        const $this = $(this), $realECD = $('#realECD'), $reStaffCD = $('#reStaffCD'), $rePassword = $('#rePassword')

        if (!common.checkValidityInput($this)) {
            return false;
        }

        let model = {
            RealECD: $realECD.val(),
            REStaffCD: $reStaffCD.val(),
            REPassword: $rePassword.val()
        };
        if (!model.RealECD && !model.REStaffCD && !model.REPassword) {
            $($realECD, $reStaffCD, $rePassword).hideError();
            return;
        }

        common.callAjax(_url.checkREPassword, model,
            function (result) {
                if (result && result.isOK) {
                    $($rePassword).hideError();
                    const data = result.data;
                }
                if (result && !result.isOK) {
                    const message = result.message;
                    $this.showError(message.MessageText1);
                }
            });
    });
    $('#btnLogin').on('click', function () {
        $form = $('#form1').hideChildErrors();

        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        const $this = $(this), $realECD = $('#realECD'), $reStaffCD = $('#reStaffCD'), $rePassword = $('#rePassword')
        let model1 = {
            RealECD: $realECD.val(),
            REStaffCD: $reStaffCD.val(),
            REPassword: $rePassword.val()
        };
        common.callAjaxWithLoading(_url.select_M_RealEstateStaff, model1, this, function (result) {
            if (result && result.isOK) {
                //sucess
                //window.location.href = './t_dashboard/Index'; 
                alert("Success");
            }
            if (result && !result.isOK) {
                if (result.message != null) {
                    const message = result.message;
                    $this.showError(message.MessageText1);
                }
                else {
                    const errors = result.data;
                    for (key in errors) {
                        const target = document.getElementById(key);
                        $(target).showError(errors[key]);
                        $form.getInvalidItems().get(0).focus();
                    }
                }
            }
        });
    });
}