const _url = {};
$(function () {
    _url.render_searchbox_detail = common.appPath + '/r_asmc_railway/Render_searchbox_detail';
    _url.gotoNextPage = common.appPath + '/r_asmc_railway/GotoNextPage';
    _url.openCheckTab = common.appPath + '/r_asmc_set_train_check_tab/Index';
    addEvents();
    $('#checkbox_expired').change();
});

function addEvents() {
    $(document).on('click', '.js-btn-select', function () {
        $this = $(this);
        const css = $this.data('target-class');
        $('.' + css + ':not(:disabled)').prop('checked', true);
    });

    $(document).on('click', '.js-btn-deselect', function () {
        $this = $(this);
        const css = $this.data('target-class');
        $('.' + css).prop('checked', false);
    });

    $('#checkbox_settings1, #checkbox_settings2').on('change', function () {
        const chk1 = $('#checkbox_settings1').prop('checked');
        const chk2 = $('#checkbox_settings2').prop('checked');

        if (chk1 && chk2) {
            $('.js-text1').removeClass('d-none');
            $('.js-text2').addClass('d-none');
            $('.js-text3').addClass('d-none');
        }
        else if (chk1 && !chk2) {
            $('.js-text1').addClass('d-none');
            $('.js-text2').removeClass('d-none');
            $('.js-text3').addClass('d-none');
        }
        else if (!chk1 && chk2) {
            $('.js-text1').addClass('d-none');
            $('.js-text2').addClass('d-none');
            $('.js-text3').removeClass('d-none');
        }
        else {
            $('.js-text1').addClass('d-none');
            $('.js-text2').addClass('d-none');
            $('.js-text3').addClass('d-none');
        }
    });

    $('#checkbox_expired').on('change', function () {
        const chk = $(this).prop('checked');
        $('.js-no-expired').prop('disabled', chk);
    });

    $('#btnSearchDetail').on('click', function () {

        let linecdCsv = '';
        $('.js-search-chk:checked').each(function () {
            linecdCsv += $(this).val() + ",";
        });

        if (!linecdCsv) {
            common.showErrorPopup('E301');
            return;
        }

        model = {
            linecdCsv: linecdCsv.slice(0, -1),
            settings1: $('#checkbox_settings1').prop('checked') ? 1 : 0,
            settings2: $('#checkbox_settings2').prop('checked') ? 1 : 0,
            expiredOnly: $('#checkbox_expired').prop('checked') ? 1 : 0,
        };

        common.callAjax(_url.render_searchbox_detail, model, function (result) {
            if (result) {
                // sucess
                $('#searchbox_detail_result').empty().append(result);
                $('#searchbox_detail').modal('show');
            }
            else {
                //error
                $('#searchbox_detail_result').empty();
            }
        });
    });

    $('#btnRegister').on('click', function () {
        let linecdCsv = '';
        $('.js-search-chk:checked').each(function () {
            linecdCsv += $(this).val() + ",";
        });

        if (!linecdCsv) {
            common.showErrorPopup('E301');
            return;
        }

        $('#hdnSelected_lines').val(linecdCsv.slice(0, -1));
        common.callSubmit(document.forms.form1, _url.gotoNextPage);
    });

    $('#btnRegistrationDetail').on('click', function () {
        let stationcdCsv = '';
        $('.js-search-detail-chk:checked').each(function () {
            stationcdCsv += $(this).val() + ",";
        });

        if (!stationcdCsv) {
            common.showErrorPopup('E301');
            return;
        }

        $('#hdnSelected_stations').val(stationcdCsv.slice(0, -1));
        common.callSubmit(document.forms.form1, _url.gotoNextPage);
    });
}

function openCheckTab(stationCD) {
    const query = {
        sc: stationCD
    }
    let newwin = window.open(_url.openCheckTab + common.querySerialize(query));
}