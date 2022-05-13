﻿const _url = {};
$(function () {
    _url.render_searchbox_detail = common.appPath + '/r_asmc_address/Render_searchbox_detail';
    addEvents();
});

function addEvents() {
    $(document).on('click', '.js-btn-select', function () {
        $this = $(this);
        const css = $this.data('target-class');
        $('.' + css).prop('checked', true);
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

    $('#btnSearchDetail').on('click', function () {

        let citycdCsv = '';
        $('.js-search-chk:checked').each(function () {
            citycdCsv += $(this).val() + ",";
        });

        if (!citycdCsv) return;

        model = {
            citycdCsv : citycdCsv.slice(0, -1),
            settings1 : $('#checkbox_settings1').prop('checked')? 1 : 0,
            settings2 : $('#checkbox_settings2').prop('checked')? 1 : 0
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

}