const _url = {};
$(function () {
    _url.redirect_206railway = common.appPath + '/r_asmc/redirect_206railway';
    _url.redirect_207address = common.appPath + '/r_asmc/redirect_207address';
    _url.redirect_208ms_list_sh = common.appPath + '/r_asmc/redirect_208ms_list_sh';
    _url.redirect_209ms_map_add = common.appPath + '/r_asmc/redirect_209ms_map_add';
    _url.redirect_210ms_reged_list = common.appPath + '/r_asmc/redirect_210ms_reged_list';
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
        common.callSubmit(document.forms.form5101, _url.redirect_206railway);
    });

    $('.js-chihou-mansion').on('click', function () {
        $this = $(this);
        $('#form5201_region').val($this.data('chihou-name'));
        common.callSubmit(document.forms.form5201, _url.redirect_209ms_map_add);
    });

    $('#btnReg_list').on('click', function () {
        common.callSubmit(document.forms.form5200, _url.redirect_210ms_reged_list);
    });

    $('#btnlist_sh').on('click', function () {
        $form = $('#mansion').hideChildErrors();
        if (!common.checkValidityOnSave('#mansion')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        $('#form5200_mansionName').val($('#SearchBox').val());
        common.callSubmit(document.forms.form5200, _url.redirect_208ms_list_sh);
    })
}