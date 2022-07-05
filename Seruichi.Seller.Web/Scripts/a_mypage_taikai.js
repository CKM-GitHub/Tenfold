const _url = {};

$(function () {
    _url.UpdateData = common.appPath + '/a_mypage_taikai/UpdateData';
    setValidation();
    addEvents();
    $('#mypage_taikai').addClass('active');
});

function setValidation() {

    $('#btnOut')
        .addvalidation_errorElement("#errorbtnOut");
}

function addEvents() {
    $('#btnOut').on('click', function () {
        $form = $('#form1').hideChildErrors();
        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
       

        $('#chk1').on('change', function () {
            this.value = this.checked ? 1 : 0;
        }).change();

        const $this = $(this), $chk = $("#chk1").val()

        let model = {
            chk: $chk
        };
        if (model.chk == '0') {
            $('#btnOut').showError(common.getMessage('E213'));
            $('#modal-taikaicheck').modal('hide');
        }
        else {
            $('#modal-taikaicheck').modal('show');
        }
    });

    $('#btnCancel').on('click', function () {
        $('#modal-taikaicheck').modal('hide');
    });
    

    $('#btnOK').on('click', function () {
       
        let model = {
            SellerID: null
        }

        common.callAjaxWithLoading(_url.UpdateData, model, this, function (result) {
            if (result && result.isOK) {
            }
            else if (result && !result.isOK) {
                const errors = result.data;
                for (key in errors) {
                    const target = document.getElementById(key);
                    $(target).showError(errors[key]);
                }
            }
        });

        
    });

    $('#btnLogOut').on('click', function () {
        window.location.href = common.appPath + '/a_login/Index'; 
    });
}