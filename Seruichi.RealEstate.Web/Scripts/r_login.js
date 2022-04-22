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
        .addvalidation_requirednullorempty1()
        .addvalidation_onebyte_character()
        .addvalidation_MaxLength(10);

    $('#reStaffCD')
        .addvalidation_errorElement("#errorreStaffCD")
        .addvalidation_requirednullorempty2()
        .addvalidation_onebyte_character()
        .addvalidation_MaxLength(10);
   
    $('#rePassword')
        .addvalidation_errorElement("#errorrePassword")
        .addvalidation_requirednullorempty3()
        .addvalidation_onebyte_character()
        .addvalidation_MaxLength(10);


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

        let model1 = {
            RealECD: $realECD.val()
        };

        if (!model1.RealECD) {
            $($realECD).hideError();
            return;
        }

        common.callAjax(_url.checkRealECD, model1,
            function (result) {
                if (result && result.isOK) {
                    $($realECD).hideError();
                    const data = result.data;                  
                }
                if (result && !result.isOK) {
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

        let model2 = {
            RealECD: $realECD.val(),
            REStaffCD: $reStaffCD.val()
        };
        if (!model2.RealECD && !model2.REStaffCD) {
            $($realECD, $reStaffCD).hideError();
            return;
        }

        common.callAjax(_url.checkREStaffCD, model2,
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

        let model3 = {
            RealECD: $realECD.val(),
            REStaffCD: $reStaffCD.val(),
            REPassword: $rePassword.val()
        };
        if (!model3.RealECD && !model3.REStaffCD && !model3.REPassword) {
            $($realECD, $reStaffCD, $rePassword).hideError();
            return;
        }

        common.callAjax(_url.checkREPassword, model3,
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
        let model4 = {
            RealECD: $realECD.val(),
            REStaffCD: $reStaffCD.val(),
            REPassword: $rePassword.val()
        };
        if (!model4.RealECD && !model4.REStaffCD && !model4.REPassword) {
            $($realECD, $reStaffCD, $rePassword).hideError();
            return;
        }
        common.callAjaxWithLoading(_url.select_M_RealEstateStaff, model4, this, function (result) {
            if (result && result.isOK) {
                //sucess
                //window.location.href = './t_dashboard/Index'; 
                const data = result.data;
                let model5 = {
                    RealECD: data.RealECD,
                    REStaffCD: data.REStaffCD,
                    PermissionChat: data.PermissionChat,
                    PermissionSetting: data.PermissionSetting,
                    PermissionPlan: data.PermissionPlan,
                    PermissionInvoice: data.PermissionInvoice
                };
                var link = './r_dashboard/Index/P会社ID=' + model5.RealECD + "&PスタッフID=" + model5.REStaffCD + "&P権限_チャット=" + model5.PermissionChat + "&P権限_設定=" + model5.PermissionSetting + "&P権限_プラン変更=" + model5.PermissionPlan + "&P権限_請求書=" + model5.PermissionInvoice;
                alert(link);
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