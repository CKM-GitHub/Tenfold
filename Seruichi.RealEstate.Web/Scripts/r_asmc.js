const _url = {};
$(function () {
    _url.redirect_207address = common.appPath + '/r_asmc/redirect_207address';
    _url.redirect_208railway = common.appPath + '/r_asmc/redirect_208railway';
    setValidation();
    addEvents();
});

function setValidation() {
    $('#SearchBox')
        .addvalidation_errorElement("#SearchBoxError")
        .addvalidation_reqired();
}

function addEvents() {
    common.bindValidationEvent('#mansion', '');

    $('.js-chihou').on('click', function () {
        $this = $(this);
        $('#form5101_region').val($this.data('chihou-name'));
        $('.modal').modal('hide');
        $('#tourokuhouhou').modal('show');
    });

    $('#btnSearch5101_1').on('click', function () {
        common.callSubmit(document.forms.form5101, _url.redirect_207address);
    });

    $('#btnSearch5101_2').on('click', function () {
        common.callSubmit(document.forms.form5101, _url.redirect_208railway);
    });

    $('#btnReg_list').on('click', function () {
        window.location.href = common.appPath + '/r_asmc_ms_reged_list/Index'; 
    });

    $('#btnlist_sh').on('click', function () {
        $form = $('#mansion').hideChildErrors();
        if (!common.checkValidityOnSave('#mansion')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        var PMansionName = $('#SearchBox').val();
        window.location.href = common.appPath + '/r_asmc_ms_list_sh/Index?MansionName=' + PMansionName;
    })
}